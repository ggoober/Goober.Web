using Indusoft.Web.GridView.Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Indusoft.Web.GridView.Helpers
{
    public static class PredicateExpressionBuilder
    {
        public static Expression<Func<T, bool>> BuildFilterPredicate<T>(object value, ExpressionEquationEnum equation, params string[] properties)
        {
            var parameterExpression = Expression.Parameter(typeof(T), typeof(T).Name);
            return (Expression<Func<T, bool>>)BuildNavigationExpression(parameterExpression, equation, value, properties);
        }

        public static Expression<T> Combine<T>(Expression<T> first, Expression<T> second, Func<Expression, Expression, Expression> merge)
        {
            // zip parameters (map from parameters of second to parameters of first)    
            var map = first.Parameters
                .Select((f, i) => new { f, s = second.Parameters[i] })
                .ToDictionary(p => p.s, p => p.f);

            // replace parameters in the second lambda expression with the parameters in the first    
            var secondBody = ParameterRebinder.ReplaceParameters(map, second.Body);

            // create a merged lambda expression with parameters from the first expression    
            return Expression.Lambda<T>(merge(first.Body, secondBody), first.Parameters);
        }

        public static Expression<Func<T, bool>> True<T>() 
        {
            Expression<Func<T, bool>> expression = param => true;
            return expression;
        }
        public static Expression<Func<T, bool>> InRange<T>(object valueFrom, object valueTo, string property)
        {
           
            var entityExpression = Expression.Parameter(typeof(T), typeof(T).Name);
            var field = entityExpression.Type.GetProperty(property, BindingFlags.Public
                                                                       | BindingFlags.Instance
                                                                       | BindingFlags.IgnoreCase);

            var propertyExpression = Expression.Property(entityExpression, field); 
            var predicate = Expression.AndAlso(
                Expression.GreaterThan(propertyExpression,
                     Expression.Constant(valueFrom)),
                Expression.LessThan(propertyExpression,
                     Expression.Constant(valueTo)));

            return (Expression<Func<T, bool>>)MakeLambda(entityExpression, predicate); 
        }

        public static IEnumerable<T> OrderByDirection<T>(this IEnumerable<T> source, SortTypeEnum sortType, string sortField)
        {
            if (string.IsNullOrEmpty(sortField) == true)
                return source;

            var sortExpression = GetOrderByExpression<T>(sortField);
            return sortType == SortTypeEnum.ASC ? source.OrderBy(sortExpression) : source.OrderByDescending(sortExpression);
        }

        private static Func<T, object> GetOrderByExpression<T>(string sortField)
        {
            if (string.IsNullOrEmpty(sortField) == true)
                return null;

            Type entitiyType = typeof(T);
            var property = entitiyType.GetProperty(sortField, BindingFlags.Public
                                                                       | BindingFlags.Instance
                                                                       | BindingFlags.IgnoreCase);

            Func<T, object> orderByExpr = (data => property.GetValue(data, null));
            return orderByExpr;
        }

        private static Expression BuildNavigationExpression(Expression parameter, ExpressionEquationEnum equation, object value, params string[] properties)
        {
            Expression resultExpression = null;
            Expression childParameter, predicate;
            Type childType = null;

            if (properties.Count() > 1)
            {
                //build path
                parameter = Expression.Property(parameter, properties[0]);
                var isCollection = typeof(IEnumerable).IsAssignableFrom(parameter.Type);
                //if it´s a collection we later need to use the predicate in the methodexpressioncall
                if (isCollection)
                {
                    childType = parameter.Type.GetGenericArguments()[0];
                    childParameter = Expression.Parameter(childType, childType.Name);
                }
                else
                {
                    childParameter = parameter;
                }
                //skip current property and get navigation property expression recursivly
                var innerProperties = properties.Skip(1).ToArray();
                predicate = BuildNavigationExpression(childParameter, equation, value, innerProperties);
                if (isCollection)
                {
                    //build subquery
                    resultExpression = BuildSubQuery(parameter, childType, predicate);
                }
                else
                {
                    resultExpression = predicate;
                }
            }
            else
            {
                //build final predicate
                resultExpression = BuildCondition(parameter, properties[0], equation, value);
            }
            return resultExpression;
        }

        private static Expression BuildSubQuery(Expression parameter, Type childType, Expression predicate)
        {
            var anyMethod = typeof(Enumerable).GetMethods().Single(m => m.Name == "Any" && m.GetParameters().Length == 2);
            anyMethod = anyMethod.MakeGenericMethod(childType);
            predicate = Expression.Call(anyMethod, parameter, predicate);
            return MakeLambda(parameter, predicate);
        }

        private static Expression BuildCondition(Expression parameter, string property, ExpressionEquationEnum equation, object value)
        {
            var childProperty = parameter.Type.GetProperty(property, BindingFlags.Public
                                                                        | BindingFlags.Instance
                                                                        | BindingFlags.IgnoreCase);
            var left = Expression.Property(parameter, childProperty);
            var right = Expression.Constant(value);
            var predicate = BuildComparsion(left, equation, right);
            return MakeLambda(parameter, predicate);
        }

        private static Expression BuildComparsion(Expression left, ExpressionEquationEnum equation, Expression right)
        {
            var stringEquations = new List<ExpressionEquationEnum>{
                ExpressionEquationEnum.Contains,
                ExpressionEquationEnum.StartsWith,
                ExpressionEquationEnum.EndsWith
            };

            if (stringEquations.Contains(equation) && left.Type != typeof(string))
            {
                equation = ExpressionEquationEnum.Equals;
            }
            if (stringEquations.Contains(equation) == false)
            {
                return Expression.MakeBinary((ExpressionType)equation, left, Expression.Convert(right, left.Type));
            }
            return BuildStringCondition(left, equation, right);
        }

        private static Expression BuildStringCondition(Expression left, ExpressionEquationEnum equation, Expression right)
        {
            var stringMethods = typeof(string).GetMethods().Where(m => m.Name.Equals(Enum.GetName(typeof(ExpressionEquationEnum), equation)) 
            && m.GetParameters().Count() == 1
            && m.GetParameters().Any(p => p.ParameterType == typeof(string)));
            var compareMethod = stringMethods.First();
            //we assume ignoreCase, so call ToLower on paramter and memberexpression
            var toLowerMethod = typeof(string).GetMethods().Single(m => m.Name.Equals("ToLower") && m.GetParameters().Count() == 0);
            left = Expression.Call(left, toLowerMethod);
            right = Expression.Call(right, toLowerMethod);
            return Expression.Call(left, compareMethod, right);
        }

        private static Expression MakeLambda(Expression parameter, Expression predicate)
        {
            var resultParameterVisitor = new ParameterVisitor();
            resultParameterVisitor.Visit(parameter);
            var resultParameter = resultParameterVisitor.Parameter;
            return Expression.Lambda(predicate, (ParameterExpression)resultParameter);
        }

        private class ParameterVisitor : ExpressionVisitor
        {
            public Expression Parameter
            {
                get;
                private set;
            }
            protected override Expression VisitParameter(ParameterExpression node)
            {
                Parameter = node;
                return node;
            }
        }
    }

    public class ParameterRebinder : ExpressionVisitor
    {
        readonly Dictionary<ParameterExpression, ParameterExpression> map;

        ParameterRebinder(Dictionary<ParameterExpression, ParameterExpression> map)
        {
            this.map = map ?? new Dictionary<ParameterExpression, ParameterExpression>();
        }

        public static Expression ReplaceParameters(Dictionary<ParameterExpression, ParameterExpression> map, Expression exp)
        {
            return new ParameterRebinder(map).Visit(exp);
        }

        protected override Expression VisitParameter(ParameterExpression p)
        {
            ParameterExpression replacement;

            if (map.TryGetValue(p, out replacement))
            {
                p = replacement;
            }

            return base.VisitParameter(p);
        }
    }
}

using Indusoft.Web.GridView.Enums;
using Indusoft.Web.GridView.Helpers;
using Indusoft.Web.GridView.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Indusoft.Web.GridView.Services.Implementation
{
    class FilterQueryBuilder : IFilterQueryBuilder
    {
        public Func<T, bool> BuildFilterFunc<T>(FilterModelDto filterModel) where T : class
        {
            Expression<Func<T, bool>> resultExpression = PredicateExpressionBuilder.True<T>();
            List<Expression<Func<T, bool>>> filterModelExpressions = new List<Expression<Func<T, bool>>>();

            foreach (var filter in filterModel.Filters)
            {
                if (filter.Operator.HasValue == true)
                {
                    Expression<Func<T, bool>> allConditionsExpression = null;
                    foreach (var condition in filter.Conditions)
                    {
                        var predicate = BuldExpressionForCondition<T>(condition, filter.FieldName);
                        if (allConditionsExpression == null)
                        {
                            allConditionsExpression = predicate;
                        }
                        else
                        {
                            if (filter.Operator.Value == FilterOperatorEnum.AND)
                            {
                                PredicateExpressionBuilder.Combine(allConditionsExpression,
                                                                    predicate,
                                                                    Expression.AndAlso);
                            }
                            else
                            {
                                PredicateExpressionBuilder.Combine(allConditionsExpression,
                                                                    predicate,
                                                                    Expression.OrElse);
                            }
                        }
                    }
                }
                else
                {
                    var firstCondition = filter.Conditions.First();
                    
                    var firstConditionPredicate = BuldExpressionForCondition<T>(firstCondition, filter.FieldName);
                    filterModelExpressions.Add(firstConditionPredicate);
                }

            }

            foreach (var expression in filterModelExpressions)
            {
                resultExpression = PredicateExpressionBuilder.Combine(resultExpression, expression, Expression.AndAlso);
            }

            return resultExpression.Compile();
        }

        private Expression<Func<T, bool>> BuldExpressionForCondition<T>(FilterConditionDto condition, string fieldName) where T : class
        {
            var equation = MapToEquation(condition.Type);
            var filterValue = MapFilterValueToFieldType<T>(condition.Filter, fieldName);
            if (equation == ExpressionEquationEnum.InRange)
            {
                var filterToValue = MapFilterValueToFieldType<T>(condition.FilterTo, fieldName);
                var predicate = PredicateExpressionBuilder.InRange<T>(filterValue, filterToValue, fieldName);
                return predicate;
            }
            else
            {
                var predicate = PredicateExpressionBuilder.BuildFilterPredicate<T>(filterValue,
                                                                                   equation,
                                                                                   fieldName);
                return predicate;
            }
        }

        private object MapFilterValueToFieldType<T>(string filterValue, string fieldName)
        {
            var entityType = typeof(T);
            var field = entityType.GetProperty(fieldName, BindingFlags.Public
                                                         | BindingFlags.Instance
                                                         | BindingFlags.IgnoreCase);
            if (field == null)
                throw new InvalidOperationException($"Unexpected property name '{fieldName}' in type '{entityType}'");

            var type = field.PropertyType;
            switch(Type.GetTypeCode(type))
            {
                case TypeCode.Int32:
                    return int.Parse(filterValue);
                case TypeCode.Int64:
                    return long.Parse(filterValue);
                case TypeCode.Double:
                    return double.Parse(filterValue);
                case TypeCode.DateTime:
                    return DateTime.Parse(filterValue);
                case TypeCode.String:
                    return filterValue;
                case TypeCode.Boolean:
                    return bool.Parse(filterValue);
                default:
                    throw new ArgumentOutOfRangeException($"Unsupported field type '{type}'");
            }
        }

        private ExpressionEquationEnum MapToEquation(FilterEquationTypeEnum equation)
        {
            switch (equation)
            {
                case FilterEquationTypeEnum.Equals:
                    return ExpressionEquationEnum.Equals;
                case FilterEquationTypeEnum.NotEquals:
                    return ExpressionEquationEnum.NotEqual;
                case FilterEquationTypeEnum.Contains:
                    return ExpressionEquationEnum.Contains;
                case FilterEquationTypeEnum.NotContains:
                    return ExpressionEquationEnum.NotContains;
                case FilterEquationTypeEnum.StartWith:
                    return ExpressionEquationEnum.StartsWith;
                case FilterEquationTypeEnum.EndWith:
                    return ExpressionEquationEnum.EndsWith;
                case FilterEquationTypeEnum.LessThan:
                    return ExpressionEquationEnum.LessThan;
                case FilterEquationTypeEnum.LessThanOrEqual:
                    return ExpressionEquationEnum.LessThanOrEqual;
                case FilterEquationTypeEnum.GreaterThan:
                    return ExpressionEquationEnum.GreaterThan;
                case FilterEquationTypeEnum.GreaterThanOrEqual:
                    return ExpressionEquationEnum.GreaterThanOrEqual;
                case FilterEquationTypeEnum.InRange:
                    return ExpressionEquationEnum.InRange;
                default:
                    throw new ArgumentOutOfRangeException($"Unexpected equation type '{equation}'.");
            }
        }
    }
}

using System.Linq.Expressions;

namespace Indusoft.Web.GridView.Helpers
{
    public enum ExpressionEquationEnum
    {
        Contains,
        NotContains,
        StartsWith,
        EndsWith,
        InRange,
        Equals = ExpressionType.Equal,
        GreaterThan = ExpressionType.GreaterThan,
        GreaterThanOrEqual = ExpressionType.GreaterThanOrEqual,
        LessThan = ExpressionType.LessThan,
        LessThanOrEqual = ExpressionType.LessThanOrEqual,
        NotEqual = ExpressionType.NotEqual,
    }
}

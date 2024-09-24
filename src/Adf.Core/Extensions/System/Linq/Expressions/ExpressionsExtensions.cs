namespace System.Linq.Expressions
{
    public static class ExpressionsExtensions
    {
        public static string GetPropertyName<T>(this Expression<Func<T, Object>> expression)
        {
            if (expression.Body is MemberExpression memberExpression)
            {
                return memberExpression.Member.Name;
            }
            else
            {
                var op = ((UnaryExpression)expression.Body).Operand;
                return ((MemberExpression)op).Member.Name;
            }
        }
    }
}

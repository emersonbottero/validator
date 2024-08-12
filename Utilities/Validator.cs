using System.Linq.Expressions;

public interface IValidator<T>
{
    Dictionary<string, List<string>> Validate(T item);
}

public abstract class Validator<T> : IValidator<T>
{
    private readonly List<Expression<Func<T, bool>>> _validators = new List<Expression<Func<T, bool>>>();
    private readonly List<Func<T, string>> _errorMessages = new List<Func<T, string>>();
    private readonly List<string> _propertyNames = new List<string>();

    protected void Should(Expression<Func<T, bool>> validation, string errorMessage)
    {
        _validators.Add(validation);
        _errorMessages.Add(item => validation.Compile()(item) ? null : errorMessage);
        _propertyNames.Add(GetPropertyName(validation));
    }

    protected void ShouldNot(Expression<Func<T, bool>> validation, string errorMessage)
    {
        _validators.Add(validation);
        _errorMessages.Add(item => !validation.Compile()(item) ? null : errorMessage);
        _propertyNames.Add(GetPropertyName(validation));
    }

    public Dictionary<string, List<string>> Validate(T item)
    {
        var results = new Dictionary<string, List<string>>();

        for (int i = 0; i < _validators.Count; i++)
        {
            var isValid = _validators[i].Compile()(item);
            var errorMessage = _errorMessages[i](item);

            if (!isValid && errorMessage != null)
            {
                var propertyName = _propertyNames[i];
                if (!results.ContainsKey(propertyName))
                {
                    results[propertyName] = new List<string>();
                }
                results[propertyName].Add(errorMessage);
            }
        }

        return results;
    }

    public static string GetPropertyName(Expression<Func<T, bool>> expression)
    {
        var memberExpression = GetMemberExpression(expression.Body);
        return memberExpression?.Member.Name;
    }


    //private static string GetPropertyName(Expression<Func<T, bool>> expression)
    //{
    //    if (expression.Body is BinaryExpression binaryExpression)
    //    {
    //        var leftMember = GetMemberExpression(binaryExpression.Left);
    //        var rightMember = GetMemberExpression(binaryExpression.Right);

    //        if (leftMember != null)
    //            return leftMember.Member.Name;
    //        if (rightMember != null)
    //            return rightMember.Member.Name;
    //    }

    //    return null;
    //}

    private static MemberExpression GetMemberExpression(Expression expression)
    {
        if (expression is MemberExpression memberExpression)
            return memberExpression;
        if (expression is UnaryExpression unaryExpression)
            return GetMemberExpression(unaryExpression.Operand);
        //if (expression is PropertyExpression propertyExpression)
        //{
        //    return GetMemberExpression(propertyExpression.Expression);
        //}
        if (expression is BinaryExpression binaryExpression)
        {
            return GetMemberExpression(binaryExpression.Left);
        }
        if (expression is MethodCallExpression methodCallExpression)
            return GetMemberExpression(methodCallExpression.Object);

        return null;
    }
}

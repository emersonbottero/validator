//using System.Linq.Expressions;

//public abstract class ValidatorBase<T> where T : class
//{
//    protected T Entity { get; private set; }
//    private readonly Dictionary<string, List<string>> _errors = new Dictionary<string, List<string>>();

//    protected ValidatorBase(T entity)
//    {
//        Entity = entity;
//    }

//    protected void Should(Expression<Func<T, object>> propertyExpression, Func<T, bool> predicate, string errorMessage)
//    {
//        var propertyName = GetPropertyName(propertyExpression);
//        if (!predicate(Entity))
//        {
//            AddError(propertyName, errorMessage);
//        }
//    }

//    protected void ShouldNot(Expression<Func<T, object>> propertyExpression, Func<T, bool> predicate, string errorMessage)
//    {
//        var propertyName = GetPropertyName(propertyExpression);
//        if (predicate(Entity))
//        {
//            AddError(propertyName, errorMessage);
//        }
//    }

//    private void AddError(string propertyName, string errorMessage)
//    {
//        if (!_errors.ContainsKey(propertyName))
//        {
//            _errors[propertyName] = new List<string>();
//        }
//        _errors[propertyName].Add(errorMessage);
//    }

//    private string GetPropertyName(Expression<Func<T, object>> propertyExpression)
//    {
//        if (propertyExpression.Body is MemberExpression member)
//        {
//            return member.Member.Name;
//        }

//        if (propertyExpression.Body is UnaryExpression unary && unary.Operand is MemberExpression memberOperand)
//        {
//            return memberOperand.Member.Name;
//        }

//        throw new InvalidOperationException("Invalid property expression");
//    }

//    public Dictionary<string, List<string>> Validate()
//    {
//        // DefineRules();

//        if (_errors.Count > 0)
//        {
//            throw new ValidationException("Validation failed.", _errors);
//        }

//        return _errors;
//    }

//    //protected abstract void DefineRules();
//}

//public class ValidationException : Exception
//{
//    public Dictionary<string, List<string>> Errors { get; }

//    public ValidationException(string message, Dictionary<string, List<string>> errors) : base(message)
//    {
//        Errors = errors;
//    }
//}
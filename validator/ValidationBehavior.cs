using Mediator;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IQuery<TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async ValueTask<TResponse> Handle(TRequest request, CancellationToken cancellationToken, MessageHandlerDelegate<TRequest, TResponse> next)
    {
        var errors = new Dictionary<string, List<string>>();

        foreach (var validator in _validators)
        {
            var validationResults = validator.Validate(request);
            foreach (var result in validationResults)
            {
                if (errors.ContainsKey(result.Key))
                {
                    errors[result.Key].AddRange(result.Value);
                }
                else
                {
                    errors[result.Key] = new List<string>(result.Value);
                }
            }
        }

        if (errors.Count != 0)
        {
            throw new ValidationException(JsonSerializer.Serialize(errors));
        }

        return await next(request, cancellationToken);
    }


}

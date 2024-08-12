//using System.Text.Json;

//public class ValidationFilter : IEndpointFilter
//{
//    public async Task<IResult> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
//    {
//        var request = context.HttpContext.Request;
//        var requestBody = await new StreamReader(request.Body).ReadToEndAsync();
//        request.Body.Position = 0; // Reset the stream position for further reading

//        // Deserialize the request body to get the request model
//        var requestModelType = context.Endpoint.Metadata.OfType<Type>().FirstOrDefault();
//        if (requestModelType == null)
//        {
//            return Results.BadRequest("Invalid request model.");
//        }

//        var requestModel = JsonSerializer.Deserialize(requestBody, requestModelType);

//        if (requestModel == null)
//        {
//            return Results.BadRequest("Invalid request body.");
//        }

//        // Retrieve the validator for the request model
//        var validatorType = typeof(ValidatorBase<>).MakeGenericType(requestModel.GetType());
//        var validator = context.HttpContext.RequestServices.GetService(validatorType) as ValidatorBase<object>;

//        if (validator == null)
//        {
//            return Results.BadRequest("No validator found for the request model.");
//        }

//        // Validate the request model
//        var errors = validator.Validate().ToList(); // Assuming Validate returns a dictionary of errors

//        if (errors.Count > 0)
//        {
//            // Return validation errors
//            return Results.BadRequest(new { Errors = errors });
//        }

//        // If validation passes, continue processing the request
//        return await next(context);
//    }

//    ValueTask<object?> IEndpointFilter.InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
//    {
//        if (context.Arguments.OfType<TRequest>().FirstOrDefault() is TRequest request)
//        {
//            // Perform validation logic here
//            var validationResult = ValidateRequest(request);
//            if (!validationResult.IsValid)
//            {
//                // Return a validation failure result
//                return Results.BadRequest(validationResult.Errors);
//            }
//        }

//        // Proceed with the next filter or endpoint
//        return await next(context);
//    }
//}

//using FluentValidation;
//using MediatR;

//namespace EmployeeManagement.Application.Behaviors;

//public sealed class ValidationBehavior<TRequest, TResponse>
//    : IPipelineBehavior<TRequest, TResponse>
//    where TRequest : notnull
//{
//    private readonly IEnumerable<IValidator<TRequest>> _validators;

//    public ValidationBehavior(
//        IEnumerable<IValidator<TRequest>> validators)
//    {
//        _validators = validators;
//    }

//    public async Task<TResponse> Handle(
//        TRequest request,
//        RequestHandlerDelegate<TResponse> next,
//        CancellationToken cancellationToken)
//    {
//        if (!_validators.Any())
//        {
//            return await next();
//        }

//        var context = new ValidationContext<TRequest>(request);

//        var validationResults = await Task.WhenAll(
//            _validators.Select(
//                validator => validator.ValidateAsync(
//                    context,
//                    cancellationToken)));

//        var failures = validationResults
//            .SelectMany(x => x.Errors)
//            .Where(x => x is not null)
//            .ToList();

//        if (failures.Count != 0)
//        {
//            throw new ValidationException(failures);
//        }

//        return await next();
//    }
//}

using EmployeeManagement.Application.Common.Results;
using FluentValidation;
using MediatR;

namespace EmployeeManagement.Application.Behaviors;

public sealed class ValidationBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
    where TResponse : IApiResponse
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehavior(
        IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (!_validators.Any())
        {
            return await next();
        }

        var context = new ValidationContext<TRequest>(request);

        var validationResults = await Task.WhenAll(
            _validators.Select(validator =>
                validator.ValidateAsync(
                    context,
                    cancellationToken)));

        var failures = validationResults
            .SelectMany(x => x.Errors)
            .Where(x => x is not null)
            .ToList();

        if (failures.Count != 0)
        {
            var errors = failures
                .Select(x => x.ErrorMessage)
                .Distinct()
                .ToList();

            return CreateErrorResponse(
                "Validation failed.",
                errors);
        }

        return await next();
    }

    private static TResponse CreateErrorResponse(
        string message,
        IEnumerable<string> errors)
    {
        var responseType = typeof(TResponse);

        if (!responseType.IsGenericType ||
            responseType.GetGenericTypeDefinition() !=
            typeof(ApiResponse<>))
        {
            throw new InvalidOperationException(
                $"ValidationBehavior requires TResponse to be ApiResponse<T>. " +
                $"Actual type: {responseType.Name}");
        }

        var dataType = responseType.GetGenericArguments()[0];

        var apiResponseType =
            typeof(ApiResponse<>).MakeGenericType(dataType);

        var method = apiResponseType.GetMethod(
            nameof(ApiResponse<object>.ErrorResponse));

        if (method is null)
        {
            throw new InvalidOperationException(
                "Could not find ApiResponse.ErrorResponse method.");
        }

        var result = method.Invoke(
            null,
            new object?[]
            {
                message,
                errors
            });

        return (TResponse)result!;
    }
}
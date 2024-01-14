using System.Diagnostics.CodeAnalysis;
using FluentValidation;
using FluentValidation.Results;
using TestDotnetReact.Server.Api;

namespace TestDotnetReact.Server.Filters;

public class RequestValidationFilter : IEndpointFilter
{
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var httpCtx = context.HttpContext;
        var cancellationToken = httpCtx.RequestAborted;
        var request = context.Arguments.FirstOrDefault(a => a is IValidatable);
        var validator = ResolveValidator(httpCtx.RequestServices, request?.GetType());
        var validationResult = await validator.ValidateAsync(new ValidationContext<object>(request), cancellationToken);

        return validationResult.IsValid
            ? await next(context)
            : validationResult.ToValidationProblem();
    }

    private static IValidator ResolveValidator(IServiceProvider serviceProvider, Type? requestType) =>
        requestType is not null
            ? (IValidator?)serviceProvider.GetService(typeof(IValidator<>).MakeGenericType(requestType))
                ?? EmptyValidator.Instance
            : EmptyValidator.Instance;
}

file static class Extensions
{
    public static IResult ToValidationProblem(this ValidationResult validationResult) =>
        Results.ValidationProblem(validationResult.Errors.ToDictionary(e => e.PropertyName, e => new[] { e.ErrorMessage }));
}

file sealed class EmptyValidator : IValidator<object>
{
    static readonly ValidationResult ValidationResult = new();
    static readonly Task<ValidationResult> ValidationResultTask = Task.FromResult(ValidationResult);

    public static readonly EmptyValidator Instance = new();

    EmptyValidator() { }

    [DoesNotReturn]
    public bool CanValidateInstancesOfType(Type type) => throw new NotImplementedException();

    [DoesNotReturn]
    public IValidatorDescriptor CreateDescriptor() => throw new NotImplementedException();

    [DoesNotReturn]
    public ValidationResult Validate(object instance) => throw new NotImplementedException();

    [DoesNotReturn]
    public ValidationResult Validate(IValidationContext context) => throw new NotImplementedException();

    public Task<ValidationResult> ValidateAsync(object instance, CancellationToken cancellation = default) => ValidationResultTask;

    public Task<ValidationResult> ValidateAsync(IValidationContext context, CancellationToken cancellation = default) => ValidationResultTask;
}
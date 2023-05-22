namespace Politicz.News.Validations;

public class ValidationBehavior<TRequest, TResult> : IPipelineBehavior<TRequest, OneOf<TResult, Failure>>, IPipelineBehavior<TRequest, OneOf<TResult, NotFound, Failure>>
    where TRequest : IMessage
{
    private readonly IValidator<TRequest> _validator;

    public ValidationBehavior(IValidator<TRequest> validator)
        => _validator = validator;

    public async ValueTask<OneOf<TResult, Failure>> Handle(
        TRequest message,
        CancellationToken cancellationToken,
        MessageHandlerDelegate<TRequest, OneOf<TResult, Failure>> next)
    {
        var validationResult = await _validator.ValidateAsync(message, cancellationToken);

        return validationResult.IsValid
            ? await next(message, cancellationToken)
            : new Failure(validationResult.ToString());
    }

    public async ValueTask<OneOf<TResult, NotFound, Failure>> Handle(
        TRequest message,
        CancellationToken cancellationToken,
        MessageHandlerDelegate<TRequest, OneOf<TResult, NotFound, Failure>> next)
    {
        var validationResult = await _validator.ValidateAsync(message, cancellationToken);

        return validationResult.IsValid
            ? await next(message, cancellationToken)
            : new Failure(validationResult.ToString());
    }
}

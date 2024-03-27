using FluentValidation;
using MediatR;

namespace Mini_Core_Banking_Project
{
    public sealed class ValidationBehaviourPipeline<TRequest,  TResponse> 
        : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public ValidationBehaviourPipeline(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }
        public async Task<TResponse> Handle(
            TRequest request, 
            RequestHandlerDelegate<TResponse> next, 
            CancellationToken cancellationToken)
        {
            if (_validators.Any())
            {
                var context = new ValidationContext<TRequest>(request);

                var validationResults = await Task.WhenAll(_validators.Select(validator => validator.ValidateAsync(context)));

                var errors = validationResults.Where(validationResult => !validationResult.IsValid)
                                              .SelectMany(validationResult => validationResult.Errors)
                                              .Where(f => f != null).ToList();
                if (errors.Any())
                {
                    throw new ValidationException(errors);
                }
            }
            return await next();
        }
    }
}

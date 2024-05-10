using Application.ResultType;
using Application.Interfaces;
using MediatR;

namespace Application.Utility
{
    public sealed record DecryptCommand(string EncryptedString) : IRequest<Result>;

    public sealed class DecryptCommandHandler : IRequestHandler<DecryptCommand, Result>
    {
        private readonly IDecrypt _decrypt;

        public DecryptCommandHandler(IDecrypt decrypt)
        {
            _decrypt = decrypt;
        }

        public async Task<Result> Handle(DecryptCommand command, CancellationToken cancellationToken)
        {
            string decryptedString = _decrypt.Decrypt(command.EncryptedString);
            if (!decryptedString.Contains("+"))
            {
                return Result.Failure<DecryptCommand>("Does not contain +");
            }
            string[] splitString = decryptedString.Split('+');
           
            return Result.Success<DecryptCommand>( $"{splitString[0]} {splitString[1]}");

        }
    }
}

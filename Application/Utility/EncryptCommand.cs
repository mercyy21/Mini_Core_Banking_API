using Application.DTO;
using Application.Interfaces;
using Application.ResultType;
using MediatR;

namespace Application.Utility
{
    public sealed record EncryptCommand(TransactionDetailsDTO SignatureDTO) : IRequest<Result>;

    public sealed class EncryptCommandHandler : IRequestHandler<EncryptCommand, Result>
    {
        private readonly IEncrypt _encrypt;

        public EncryptCommandHandler(IEncrypt encrypt)
        {
            _encrypt = encrypt;
        }

        public async Task<Result> Handle(EncryptCommand command, CancellationToken cancellationToken)
        {
            string combinedString = $"{command.SignatureDTO.AccountNumber}+{command.SignatureDTO.Email}";
            string encryptedSignature = _encrypt.Encrypt(combinedString);
            return Result.Success<EncryptCommand>("Encryption Successful", encryptedSignature);
        }
    }
}

using Application.DTO;
using Application.Interfaces;
using Application.ResultType;
using MediatR;

namespace Application.Utility
{
    public sealed record EncryptCommand(TransactionDetailsDTO SignatureDTO) : IRequest<ResultType.Result>;

    public sealed class EncryptCommandHandler : IRequestHandler<EncryptCommand, ResultType.Result>
    {
        private readonly IEncrypt _encrypt;

        public EncryptCommandHandler(IEncrypt encrypt)
        {
            _encrypt = encrypt;
        }

        public async Task<ResultType.Result> Handle(EncryptCommand command, CancellationToken cancellationToken)
        {
            string combinedString = $"{command.SignatureDTO.AccountNumber}+{command.SignatureDTO.Email}";
            string encryptedSignature = _encrypt.Encrypt(combinedString);
            return ResultType.Result.Success<EncryptCommand>("Encryption Successful", encryptedSignature);
        }
    }
}

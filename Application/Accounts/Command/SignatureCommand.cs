using Application.UtilityService;
using Domain.DTO;
using Domain.Interfaces;
using MediatR;

namespace Application.Accounts.Command
{
    public sealed record SignatureCommand(SignatureDTO SignatureDTO): IRequest<ResponseModel>;

    public sealed class SignatureCommandHandler: IRequestHandler<SignatureCommand,ResponseModel>
    {
        private readonly IEncrypt _encrypt;
        
        public SignatureCommandHandler(IEncrypt encrypt)
        {
            _encrypt = encrypt;
        }

        public async Task<ResponseModel> Handle(SignatureCommand command, CancellationToken cancellationToken)
        {
            string combinedString = $"{command.SignatureDTO.AccountNumber}+{command.SignatureDTO.Email}";
            string encryptedSignature = _encrypt.Encrypt(combinedString);
            return new ResponseModel { Data = encryptedSignature, Message = "Encryption Successful", Success = true };
        }
    }
}

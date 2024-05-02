using Application.Accounts.Command;
using Domain.DTO;
using Domain.Interfaces;
using Moq;
using System.Runtime.CompilerServices;

namespace Mini_Core_Banking_Project.Test.Accounts.Command;

public class SignatureCommandTest
{
    private readonly Mock<IEncrypt> _encryptMock;

    public SignatureCommandTest()
    {
        _encryptMock = new Mock<IEncrypt>();
    }
    [Fact]
    public async Task SignatureSuccess()
    {
        //Arrange
        _encryptMock.Setup(x => x.Encrypt(It.IsAny<string>())).Returns("Ughfol193ejak=");
        SignatureDTO signatureDTO = new SignatureDTO
        {
            AccountNumber= "7894621348",
            Email= "cyxsa20@gmail.com"
        };

        //Act
        var request = new SignatureCommand(signatureDTO);
        var handler = new SignatureCommandHandler(_encryptMock.Object);
        var result = await handler.Handle(request,CancellationToken.None);

        //Assert
        Assert.NotNull(result);
        Assert.Equal("Encryption Successful", result.Message.ToString());
    }
}

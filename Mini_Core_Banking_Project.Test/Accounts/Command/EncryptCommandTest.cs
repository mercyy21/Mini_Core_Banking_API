using Application.DTO;
using Application.Interfaces;
using Application.Utility;
using Moq;

namespace API.Test.Accounts.Command;

public class EncryptCommandTest
{
    private readonly Mock<IEncrypt> _encryptMock;

    public EncryptCommandTest()
    {
        _encryptMock = new Mock<IEncrypt>();
    }
    [Fact]
    public async Task SignatureSuccess()
    {
        //Arrange
        _encryptMock.Setup(x => x.Encrypt(It.IsAny<string>())).Returns("Ughfol193ejak=");
        TransactionDetailsDTO signatureDTO = new TransactionDetailsDTO
        {
            AccountNumber= "7894621348",
            Email= "cyxsa20@gmail.com"
        };

        //Act
        var request = new EncryptCommand(signatureDTO);
        var handler = new EncryptCommandHandler(_encryptMock.Object);
        var result = await handler.Handle(request,CancellationToken.None);

        //Assert
        Assert.NotNull(result);
        Assert.Equal("Encryption Successful", result.Message.ToString());
    }
}

using Application.Accounts.AccountCommand;
using API.Test.Generate;
using API.Test.Services;
using Moq;
using Application.Interfaces;

namespace API.Test.Accounts.Command;

public class DeleteAccountCommandTest
{
    private readonly Mock<IMiniCoreBankingDbContext> _contextMock;

    public DeleteAccountCommandTest()
    {
        _contextMock= new Mock<IMiniCoreBankingDbContext>();
    }

    [Fact]
    public async Task DeleteAccountSuccess()
    {
        //Arrange 
        var mock = MockDBContext.GetQueryableMockDbSet(FakeAccount.GenerateFakeActiveAccount());
        _contextMock.Setup(x => x.Accounts).Returns(mock);
        Guid customerId = Guid.Parse("ee99627b-a78d-47df-8bc1-94bd3501a4fd");

        //Act 
        var request = new DeleteAccountCommand(customerId);
        var handler = new DeleteAccountCommandHandler(_contextMock.Object);
        var result = await handler.Handle(request,CancellationToken.None);

        //Assert
        Assert.NotNull(result);
        Assert.Equal("Account Deleted Successfully", result.Message.ToString());
    }
    [Fact]
    public async Task DeleteAccount_AccountDoesNotExist()
    {
        //Arrange 
        var mock = MockDBContext.GetQueryableMockDbSet(FakeAccount.GenerateFakeActiveAccount());
        _contextMock.Setup(x => x.Accounts).Returns(mock);
        Guid customerId = Guid.Parse("639b9d02-9a5e-4215-b9f4-a05825e52cfa");

        //Act 
        var request = new DeleteAccountCommand(customerId);
        var handler = new DeleteAccountCommandHandler(_contextMock.Object);
        var result = await handler.Handle(request, CancellationToken.None);

        //Assert
        Assert.NotNull(result);
        Assert.Equal("Account does not exist", result.Message.ToString());
    }
}

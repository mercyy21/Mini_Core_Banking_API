using Application.Accounts.AccountCommand;
using API.Test.Generate;
using API.Test.Services;
using Moq;
using Application.Interfaces;

namespace API.Test.Accounts.Command;

public class ActivateAccountCommandTest
{
    private readonly Mock<IMiniCoreBankingDbContext> _contextMock;

    public ActivateAccountCommandTest()
    {
        _contextMock = new Mock<IMiniCoreBankingDbContext>();
    }

    [Fact]
    public async Task ActivateAccountSuccess()
    {
        //Arrange
        Guid accountId = Guid.Parse("4957c1f1-4236-4b82-8a0e-abee71200d1b");
        var mock = MockDBContext.GetQueryableMockDbSet(FakeAccount.GenerateFakeInactiveAccount());
        _contextMock.Setup(x => x.Accounts).Returns(mock);

        //Act
        var request = new ActivateAccountCommand(accountId);
        var handler = new ActivateAccountCommandHandler(_contextMock.Object);
        var result =await handler.Handle(request,CancellationToken.None);

        //Assert
        Assert.NotNull(result);
        Assert.Equal("Account activated", result.Message.ToString());
    }
    [Fact]
    public async Task ActivateAccount_AccountDoesNotExist()
    {
        //Arrange
        Guid accountId = Guid.Parse("639b9d02-9a5e-4215-b9f4-a05825e52cfa");
        var mock = MockDBContext.GetQueryableMockDbSet(FakeAccount.GenerateFakeInactiveAccount());
        _contextMock.Setup(x => x.Accounts).Returns(mock);

        //Act
        var request = new ActivateAccountCommand(accountId);
        var handler = new ActivateAccountCommandHandler(_contextMock.Object);
        var result = await handler.Handle(request, CancellationToken.None);

        //Assert
        Assert.NotNull(result);
        Assert.Equal("Account does not exist", result.Message.ToString());
    }
    [Fact]
    public async Task ActivateAccount_AlreadyActive()
    {
        //Arrange
        Guid accountId = Guid.Parse("4957c1f1-4236-4b82-8a0e-abee71200d1b");
        var mock = MockDBContext.GetQueryableMockDbSet(FakeAccount.GenerateFakeActiveAccount());
        _contextMock.Setup(x => x.Accounts).Returns(mock);

        //Act
        var request = new ActivateAccountCommand(accountId);
        var handler = new ActivateAccountCommandHandler(_contextMock.Object);
        var result = await handler.Handle(request, CancellationToken.None);

        //Assert
        Assert.NotNull(result);
        Assert.Equal("Account is already active", result.Message.ToString());
    }
}

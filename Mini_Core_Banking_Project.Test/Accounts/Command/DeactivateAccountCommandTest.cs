using Application.Accounts.AccountCommand;
using Infrastructure.DBContext;
using Mini_Core_Banking_Project.Test.Generate;
using Mini_Core_Banking_Project.Test.Services;
using Moq;

namespace Mini_Core_Banking_Project.Test.Accounts.Command;

public class DeactivateAccountCommandTest
{
    private readonly Mock<IMiniCoreBankingDbContext> _contextMock;

    public DeactivateAccountCommandTest()
    {
        _contextMock = new Mock<IMiniCoreBankingDbContext>();
    }

    [Fact]
    public async Task DeactivateAccountSuccess()
    {
        //Arrange
        Guid accountId = Guid.Parse("4957c1f1-4236-4b82-8a0e-abee71200d1b");
        var mock = MockDBContext.GetQueryableMockDbSet(FakeAccount.GenerateFakeActiveAccount());
        _contextMock.Setup(x => x.Accounts).Returns(mock);

        //Act
        var request = new DeactivateAccountCommand(accountId);
        var handler = new DeactivateAccountCommandHandler(_contextMock.Object);
        var result = await handler.Handle(request, CancellationToken.None);

        //Assert
        Assert.NotNull(result);
        Assert.Equal("Account Deactivated", result.Message.ToString());
    }

    [Fact]
    public async Task ActivateAccount_AccountDoesNotExist()
    {
        //Arrange
        Guid accountId = Guid.Parse("639b9d02-9a5e-4215-b9f4-a05825e52cfa");
        var mock = MockDBContext.GetQueryableMockDbSet(FakeAccount.GenerateFakeActiveAccount());
        _contextMock.Setup(x => x.Accounts).Returns(mock);

        //Act
        var request = new DeactivateAccountCommand(accountId);
        var handler = new DeactivateAccountCommandHandler(_contextMock.Object);
        var result = await handler.Handle(request, CancellationToken.None);

        //Assert
        Assert.NotNull(result);
        Assert.Equal("Account does not exist", result.Message.ToString());
    }
    [Fact]
    public async Task DeactivateAccount_AlreadyInactive()
    {
        //Arrange
        Guid accountId = Guid.Parse("4957c1f1-4236-4b82-8a0e-abee71200d1b");
        var mock = MockDBContext.GetQueryableMockDbSet(FakeAccount.GenerateFakeInactiveAccount());
        _contextMock.Setup(x => x.Accounts).Returns(mock);

        //Act
        var request = new DeactivateAccountCommand(accountId);
        var handler = new DeactivateAccountCommandHandler(_contextMock.Object);
        var result = await handler.Handle(request, CancellationToken.None);

        //Assert
        Assert.NotNull(result);
        Assert.Equal("Account is already inactive", result.Message.ToString());
    }
}

using Application.Accounts.AccountCommand;
using AutoMapper;
using Domain.DTO;
using Domain.Enums;
using Infrastructure.DBContext;
using Mini_Core_Banking_Project.Test.Generate;
using Mini_Core_Banking_Project.Test.Services;
using Moq;

namespace Mini_Core_Banking_Project.Test.Accounts.Command;

public class CreateAccountCommandTest
{
    private readonly Mock<IMiniCoreBankingDbContext> _contextMock;
    private readonly Mock<IMapper> _mapper;

    public CreateAccountCommandTest()
    {
        _contextMock = new Mock<IMiniCoreBankingDbContext>();
        _mapper = new Mock<IMapper>();
    }

    [Fact]
    public async Task CreateAccountSuccess()
    {
        //Arrange 
        var mock = MockDBContext.GetQueryableMockDbSet(FakeAccount.GenerateFakeActiveAccount());
        var customersMock = MockDBContext.GetQueryableMockDbSet(FakeCustomer.GenerateCustomerDifferentId());
        _contextMock.Setup(x => x.Accounts).Returns(mock);
        _contextMock.Setup(x => x.Customers).Returns(customersMock);
        AccountDTO accountDTO = new AccountDTO
        {
            CustomerId= Guid.Parse("fefc120c-d9f2-4072-ab9f-fe357a2f9cc3"),
            AccountType= AccountType.Savings
        };

        //Act
        var request = new CreateAccountCommand(accountDTO);
        var handler = new CreateAccountCommandHandler(_contextMock.Object,_mapper.Object);
        var result = await handler.Handle(request,CancellationToken.None);

        //Assert
        Assert.NotNull(result);
        Assert.Equal("Account created successfully", result.Message.ToString());
    }

    public async Task CreateAccount_AccountAlreadyExists()
    {
        //Arrange 
        var mock = MockDBContext.GetQueryableMockDbSet(FakeAccount.GenerateFakeActiveAccount());
        _contextMock.Setup(x => x.Accounts).Returns(mock);
        AccountDTO accountDTO = new AccountDTO
        {
            CustomerId = Guid.Parse("fefc120c-d9f2-4072-ab9f-fe357a2f9cc3"),
            AccountType = AccountType.Savings
        };

        //Act
        var request = new CreateAccountCommand(accountDTO);
        var handler = new CreateAccountCommandHandler(_contextMock.Object, _mapper.Object);
        var result =await  handler.Handle(request, CancellationToken.None);

        //Assert
        Assert.NotNull(result);
        Assert.Equal("Customer already has an account", result.Message.ToString());
    }
    public async Task CreateAccount_CustomerDoesNotExist()
    {
        //Arrange 
        var mock = MockDBContext.GetQueryableMockDbSet(FakeAccount.GenerateFakeActiveAccount());
        var customersMock = MockDBContext.GetQueryableMockDbSet(FakeCustomer.GenerateCustomer());
        _contextMock.Setup(x => x.Accounts).Returns(mock);
        _contextMock.Setup(x => x.Customers).Returns(customersMock);
        AccountDTO accountDTO = new AccountDTO
        {
            CustomerId = Guid.Parse("04dffd1d-c188-4f44-ae98-c824f278fac1"),
            AccountType = AccountType.Savings
        };

        //Act
        var request = new CreateAccountCommand(accountDTO);
        var handler = new CreateAccountCommandHandler(_contextMock.Object, _mapper.Object);
        var result = await handler.Handle(request, CancellationToken.None);

        //Assert
        Assert.NotNull(result);
        Assert.Equal("Customer does not exist", result.Message.ToString());
    }
}

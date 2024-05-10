using Application.Accounts.AccountCommand;
using Application.TransactionHistory;
using Application.Domain.Entity;
using Application.DTO;
using Application.Interfaces;
using MediatR;
using API.Test.Generate;
using API.Test.Services;
using Moq;
using Application.ResultType;

namespace API.Test.Accounts.Command;

public class DepositCommandTest
{
    private readonly Mock<IMiniCoreBankingDbContext> _contextMock;
    private readonly Mock<IMediator> _mediator;
    private readonly Mock<IDecrypt> _decryptService;

    public DepositCommandTest()
    {
        _contextMock= new Mock<IMiniCoreBankingDbContext>();
        _mediator= new Mock<IMediator>();
        _decryptService= new Mock<IDecrypt>();

    }

    [Fact]
    public async Task DepositCommandSuccess()
    {
        //Arrange 
        var customerMock = MockDBContext.GetQueryableMockDbSet(FakeCustomer.GenerateCustomer());
        var accountMock = MockDBContext.GetQueryableMockDbSet(FakeAccount.GenerateFakeActiveAccount());
        string decryptedSignature = "7894621348+cyxsa20@gmail.com";
        _contextMock.Setup(x => x.Customers).Returns(customerMock);
        _contextMock.Setup(x=>x.Accounts).Returns(accountMock);
        Transaction transaction = FakeTransactionHistory.GenerateFakeTransactionHistory().First(x=> x.Id== Guid.Parse("e47fd612-f843-4470-a53b-eb63700a8b04"));
        TransactDTO transactDTO = new TransactDTO
        {
            TransactionDetails = "Ughfol193ejak=",
            Amount = 200
        };
        Result response = Result.Success<RecordTransactionCommand>("Transaction recorded successfully");
        _mediator.Setup(x => x.Send(It.IsAny<RecordTransactionCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(response);
        _decryptService.Setup(x => x.Decrypt(It.IsAny<string>())).Returns(decryptedSignature);

        //Act
        var request = new DepositCommand(transactDTO);
        var handler = new DepositCommandHandler(_contextMock.Object,_mediator.Object,_decryptService.Object);
        var result = await handler.Handle(request, CancellationToken.None);

        //Assert
        Assert.NotNull(result);
        Assert.Equal("Amount successfully deposited", result.Message.ToString());

    }

    [Fact]
    public async Task DepositCommand_InvalidSignature()
    {
        //Arrange 
        var customerMock = MockDBContext.GetQueryableMockDbSet(FakeCustomer.GenerateCustomer());
        var accountMock = MockDBContext.GetQueryableMockDbSet(FakeAccount.GenerateFakeActiveAccount());
        string decryptedSignature = "7894621348cyxsa20@gmail.com";
        _contextMock.Setup(x => x.Customers).Returns(customerMock);
        _contextMock.Setup(x => x.Accounts).Returns(accountMock);
        TransactDTO transactDTO = new TransactDTO
        {
            TransactionDetails = "Ughfol193ejak=",
            Amount = 200
        };
        _decryptService.Setup(x => x.Decrypt(It.IsAny<string>())).Returns(decryptedSignature);

        //Act
        var request = new DepositCommand(transactDTO);
        var handler = new DepositCommandHandler(_contextMock.Object, _mediator.Object, _decryptService.Object);
        var result = await handler.Handle(request, CancellationToken.None);

        //Assert
        Assert.NotNull(result);
        Assert.Equal("Invalid Signature", result.Message.ToString());

    }
    [Fact]
    public async Task DepositCommand_DoesNotExist()
    {
        //Arrange 
        var customerMock = MockDBContext.GetQueryableMockDbSet(FakeCustomer.GenerateCustomer());
        var accountMock = MockDBContext.GetQueryableMockDbSet(FakeAccount.GenerateFakeActiveAccount());
        string decryptedSignature = "9694520348+cyxsa20@gmail.com";
        _contextMock.Setup(x => x.Customers).Returns(customerMock);
        _contextMock.Setup(x => x.Accounts).Returns(accountMock);
        Transaction transaction = FakeTransactionHistory.GenerateFakeTransactionHistory().First(x => x.Id == Guid.Parse("e47fd612-f843-4470-a53b-eb63700a8b04"));
        TransactDTO transactDTO = new TransactDTO
        {
            TransactionDetails = "Ughfol193ejak=",
            Amount = 200
        };
        _decryptService.Setup(x => x.Decrypt(It.IsAny<string>())).Returns(decryptedSignature);

        //Act
        var request = new DepositCommand(transactDTO);
        var handler = new DepositCommandHandler(_contextMock.Object, _mediator.Object, _decryptService.Object);
        var result = await handler.Handle(request, CancellationToken.None);

        //Assert
        Assert.NotNull(result);
        Assert.Equal("Account does not exist", result.Message.ToString());

    }
    [Fact]
    public async Task DepositCommand_InactiveAccount()
    {
        //Arrange 
        var accountMock = MockDBContext.GetQueryableMockDbSet(FakeAccount.GenerateFakeInactiveAccount());
        string decryptedSignature = "7894621348+cyxsa20@gmail.com";
        _contextMock.Setup(x => x.Accounts).Returns(accountMock);
        Transaction transaction = FakeTransactionHistory.GenerateFakeTransactionHistory().First(x => x.Id == Guid.Parse("e47fd612-f843-4470-a53b-eb63700a8b04"));
        TransactDTO transactDTO = new TransactDTO
        {
            TransactionDetails = "Ughfol193ejak=",
            Amount = 200
        };
        _decryptService.Setup(x => x.Decrypt(It.IsAny<string>())).Returns(decryptedSignature);

        //Act
        var request = new DepositCommand(transactDTO);
        var handler = new DepositCommandHandler(_contextMock.Object, _mediator.Object, _decryptService.Object);
        var result = await handler.Handle(request, CancellationToken.None);

        //Assert
        Assert.NotNull(result);
        Assert.Equal("Account is inactive", result.Message.ToString());

    }

    [Fact]
    public async Task DepositCommand_EmailDoesNotMatch()
    {
        //Arrange 
        var customerMock = MockDBContext.GetQueryableMockDbSet(FakeCustomer.GenerateCustomer());
        var accountMock = MockDBContext.GetQueryableMockDbSet(FakeAccount.GenerateFakeActiveAccount());
        string decryptedSignature = "7894621348+dsa20@gmail.com";
        _contextMock.Setup(x => x.Customers).Returns(customerMock);
        _contextMock.Setup(x => x.Accounts).Returns(accountMock);
        Transaction transaction = FakeTransactionHistory.GenerateFakeTransactionHistory().First(x => x.Id == Guid.Parse("e47fd612-f843-4470-a53b-eb63700a8b04"));
        TransactDTO transactDTO = new TransactDTO
        {
            TransactionDetails = "Ughfol193ejak=",
            Amount = 200
        };
        _decryptService.Setup(x => x.Decrypt(It.IsAny<string>())).Returns(decryptedSignature);

        //Act
        var request = new DepositCommand(transactDTO);
        var handler = new DepositCommandHandler(_contextMock.Object, _mediator.Object, _decryptService.Object);
        var result = await handler.Handle(request, CancellationToken.None);

        //Assert
        Assert.NotNull(result);
        Assert.Equal("Email does not match", result.Message.ToString());

    }
    [Fact]
    public async Task DepositCommand_AmountLessThanZero()
    {
        //Arrange 
        var customerMock = MockDBContext.GetQueryableMockDbSet(FakeCustomer.GenerateCustomer());
        var accountMock = MockDBContext.GetQueryableMockDbSet(FakeAccount.GenerateFakeActiveAccount());
        string decryptedSignature = "7894621348+cyxsa20@gmail.com";
        _contextMock.Setup(x => x.Customers).Returns(customerMock);
        _contextMock.Setup(x => x.Accounts).Returns(accountMock);
        Transaction transaction = FakeTransactionHistory.GenerateFakeTransactionHistory().First(x => x.Id == Guid.Parse("e47fd612-f843-4470-a53b-eb63700a8b04"));
        TransactDTO transactDTO = new TransactDTO
        {
            TransactionDetails = "Ughfol193ejak=",
            Amount = 0
        };
        _decryptService.Setup(x => x.Decrypt(It.IsAny<string>())).Returns(decryptedSignature);

        //Act
        var request = new DepositCommand(transactDTO);
        var handler = new DepositCommandHandler(_contextMock.Object, _mediator.Object, _decryptService.Object);
        var result = await handler.Handle(request, CancellationToken.None);

        //Assert
        Assert.NotNull(result);
        Assert.Equal("Amount must be greater than zero", result.Message.ToString());
    }
}

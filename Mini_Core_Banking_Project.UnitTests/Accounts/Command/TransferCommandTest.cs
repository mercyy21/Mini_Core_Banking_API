using Application.Accounts.Command;
using Application.TransactionHistory;
using Application.Domain.Entity;
using Application.DTO;
using Infrastructure.DBContext;
using MediatR;
using Microsoft.AspNetCore.Http;
using API.Test.Generate;
using API.Test.Services;
using Moq;
using System.Security.Claims;
using Application.Interfaces;
using Application.ResultType;

namespace API.Test.Accounts.Command;

public class TransferCommandTest
{
    private readonly Mock<IMiniCoreBankingDbContext> _contextMock;
    private readonly Mock<IMediator> _mediatorMock;

    public TransferCommandTest()
    {
        _contextMock= new Mock<IMiniCoreBankingDbContext>();
        _mediatorMock= new Mock<IMediator>();
    }

    [Fact]
    public async Task TransferCommandSuccess()
    {
        //Arrange 
        var mock = MockDBContext.GetQueryableMockDbSet(FakeAccount.GenerateFakeActiveAccount());

        _contextMock.Setup(x => x.Accounts).Returns(mock);
        Transaction transaction = FakeTransactionHistory.GenerateFakeTransactionHistory().First(x=> x.Id== Guid.Parse("e47fd612-f843-4470-a53b-eb63700a8b04"));
        TransferDTO transferDTO = new TransferDTO
        {
            SendersAccountNumber= "7894621348",
            ReceiversAccountNumber = "6894628348",
            Amount = 100
        };
        Application.ResultType.Result response = Application.ResultType.Result.Success<RecordTransactionCommand>("Transaction recorded successfully", transaction);
        _mediatorMock.Setup(x => x.Send(It.IsAny<RecordTransactionCommand>(), It.IsAny < CancellationToken>())).ReturnsAsync(response);

        //Act
        var request = new TransferCommand(transferDTO);
        var handler = new TransferCommandHandler(_contextMock.Object, _mediatorMock.Object);
        var result = await handler.Handle(request, CancellationToken.None);

        //Assert
        Assert.NotNull(result);
        Assert.Equal("Transfer Success", result.Message.ToString());
    }
    [Fact]
    public async Task TransferCommand_SendersAccountDoesNotExist()
    {
        //Arrange 
        var mock = MockDBContext.GetQueryableMockDbSet(FakeAccount.GenerateFakeActiveAccount());
        _contextMock.Setup(x => x.Accounts).Returns(mock);
        Transaction transaction = FakeTransactionHistory.GenerateFakeTransactionHistory().First(x => x.Id == Guid.Parse("e47fd612-f843-4470-a53b-eb63700a8b04"));
        TransferDTO transferDTO = new TransferDTO
        {
            SendersAccountNumber = "7895621348",
            ReceiversAccountNumber = "6894628348",
            Amount = 100
        };

        //Act
        var request = new TransferCommand(transferDTO);
        var handler = new TransferCommandHandler(_contextMock.Object, _mediatorMock.Object);
        var result = await handler.Handle(request, CancellationToken.None);

        //Assert
        Assert.NotNull(result);
        Assert.Equal("Senders account does not exist", result.Message.ToString());
    }

    [Fact]
    public async Task TransferCommand_ReceiversAccountDoesNotExist()
    {
        //Arrange 
        var mock = MockDBContext.GetQueryableMockDbSet(FakeAccount.GenerateFakeActiveAccount());
        _contextMock.Setup(x => x.Accounts).Returns(mock);
        Transaction transaction = FakeTransactionHistory.GenerateFakeTransactionHistory().First(x => x.Id == Guid.Parse("e47fd612-f843-4470-a53b-eb63700a8b04"));
        TransferDTO transferDTO = new TransferDTO
        {
            SendersAccountNumber = "7894621348",
            ReceiversAccountNumber = "6894828348",
            Amount = 100
        };

        //Act
        var request = new TransferCommand(transferDTO);
        var handler = new TransferCommandHandler(_contextMock.Object, _mediatorMock.Object);
        var result = await handler.Handle(request, CancellationToken.None);

        //Assert
        Assert.NotNull(result);
        Assert.Equal("Receivers account does not exist", result.Message.ToString());
    }

    [Fact]
    public async Task TransferCommand_NoSelfTransfer()
    {
        //Arrange 
        var mock = MockDBContext.GetQueryableMockDbSet(FakeAccount.GenerateFakeActiveAccount());
        _contextMock.Setup(x => x.Accounts).Returns(mock);
        Transaction transaction = FakeTransactionHistory.GenerateFakeTransactionHistory().First(x => x.Id == Guid.Parse("e47fd612-f843-4470-a53b-eb63700a8b04"));
        TransferDTO transferDTO = new TransferDTO
        {
            SendersAccountNumber = "7894621348",
            ReceiversAccountNumber = "7894621348",
            Amount = 100
        };

        //Act
        var request = new TransferCommand(transferDTO);
        var handler = new TransferCommandHandler(_contextMock.Object, _mediatorMock.Object);
        var result = await handler.Handle(request, CancellationToken.None);

        //Assert
        Assert.NotNull(result);
        Assert.Equal("Cannot transfer to self", result.Message.ToString());
    }
    [Fact]
    public async Task TransferCommand_InactiveAccount()
    {
        //Arrange 
        var mock = MockDBContext.GetQueryableMockDbSet(FakeAccount.GenerateFakeInactiveAccount());

        _contextMock.Setup(x => x.Accounts).Returns(mock);
        Transaction transaction = FakeTransactionHistory.GenerateFakeTransactionHistory().First(x => x.Id == Guid.Parse("e47fd612-f843-4470-a53b-eb63700a8b04"));
        TransferDTO transferDTO = new TransferDTO
        {
            SendersAccountNumber = "7894621348",
            ReceiversAccountNumber = "6894628348",
            Amount = 100
        };

        //Act
        var request = new TransferCommand(transferDTO);
        var handler = new TransferCommandHandler(_contextMock.Object, _mediatorMock.Object);
        var result = await handler.Handle(request, CancellationToken.None);

        //Assert
        Assert.NotNull(result);
        Assert.Equal("Account is inactive", result.Message.ToString());
    }
    [Fact]
    public async Task TransferCommand_InsufficientFunds()
    {
        //Arrange 
        var mock = MockDBContext.GetQueryableMockDbSet(FakeAccount.GenerateFakeActiveAccount());
        _contextMock.Setup(x => x.Accounts).Returns(mock);
        Transaction transaction = FakeTransactionHistory.GenerateFakeTransactionHistory().First(x => x.Id == Guid.Parse("e47fd612-f843-4470-a53b-eb63700a8b04"));
        TransferDTO transferDTO = new TransferDTO
        {
            SendersAccountNumber = "7894621348",
            ReceiversAccountNumber = "6894628348",
            Amount = 1000000
        };

        //Act
        var request = new TransferCommand(transferDTO);
        var handler = new TransferCommandHandler(_contextMock.Object, _mediatorMock.Object);
        var result = await handler.Handle(request, CancellationToken.None);

        //Assert
        Assert.NotNull(result);
        Assert.Equal("Insufficient Funds", result.Message.ToString());
    }

}

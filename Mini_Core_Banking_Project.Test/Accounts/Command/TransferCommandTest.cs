using Application.Accounts.Command;
using Application.TransactionHistory;
using Domain.Domain.Entity;
using Domain.DTO;
using Infrastructure.DBContext;
using MediatR;
using Microsoft.AspNetCore.Http;
using Mini_Core_Banking_Project.Test.Generate;
using Mini_Core_Banking_Project.Test.Services;
using Moq;
using System.Security.Claims;

namespace Mini_Core_Banking_Project.Test.Accounts.Command;

public class TransferCommandTest
{
    private readonly Mock<IMiniCoreBankingDbContext> _contextMock;
    private readonly Mock<IHttpContextAccessor> _contextAccessorMock;
    private readonly Mock<IMediator> _mediatorMock;

    public TransferCommandTest()
    {
        _contextMock= new Mock<IMiniCoreBankingDbContext>();
        _contextAccessorMock= new Mock<IHttpContextAccessor>();
        _mediatorMock= new Mock<IMediator>();
    }

    [Fact]
    public async Task TransferCommandSuccess()
    {
        //Arrange 
        var mock = MockDBContext.GetQueryableMockDbSet(FakeAccount.GenerateFakeActiveAccount());
        var claimsIdentity = new ClaimsIdentity(new Claim[]
        {
            new Claim(ClaimTypes.NameIdentifier, "ee99627b-a78d-47df-8bc1-94bd3501a4fd")
        });

        var userPrincipal = new ClaimsPrincipal(claimsIdentity);
        _contextMock.Setup(x => x.Accounts).Returns(mock);
        _contextAccessorMock.Setup(x => x.HttpContext.User).Returns(userPrincipal);
        Transaction transaction = FakeTransactionHistory.GenerateFakeTransactionHistory().First(x=> x.Id== Guid.Parse("e47fd612-f843-4470-a53b-eb63700a8b04"));
        TransferDTO transferDTO = new TransferDTO
        {
            AccountNumber = "6894628348",
            Amount = 100
        };
        ResponseModel response = new ResponseModel
        {
            Data = transaction,
            Message = "Transaction recorded successfully",
            Success = true
        };
        _mediatorMock.Setup(x => x.Send(It.IsAny<RecordTransactionCommand>(), It.IsAny < CancellationToken>())).ReturnsAsync(response);

        //Act
        var request = new TransferCommand(transferDTO);
        var handler = new TrasferCommandHandler(_contextMock.Object,_contextAccessorMock.Object, _mediatorMock.Object);
        var result = await handler.Handle(request, CancellationToken.None);

        //Assert
        Assert.NotNull(result);
        Assert.Equal("Transfer Success", result.Message.ToString());
    }

    [Fact]
    public async Task TransferCommand_ReceiversAccountDoesNotExist()
    {
        //Arrange 
        var mock = MockDBContext.GetQueryableMockDbSet(FakeAccount.GenerateFakeActiveAccount());
        var claimsIdentity = new ClaimsIdentity(new Claim[]
        {
            new Claim(ClaimTypes.NameIdentifier, "ee99627b-a78d-47df-8bc1-94bd3501a4fd")
        });

        var userPrincipal = new ClaimsPrincipal(claimsIdentity);
        _contextMock.Setup(x => x.Accounts).Returns(mock);
        _contextAccessorMock.Setup(x => x.HttpContext.User).Returns(userPrincipal);
        Transaction transaction = FakeTransactionHistory.GenerateFakeTransactionHistory().First(x => x.Id == Guid.Parse("e47fd612-f843-4470-a53b-eb63700a8b04"));
        TransferDTO transferDTO = new TransferDTO
        {
            AccountNumber = "6894828348",
            Amount = 100
        };

        //Act
        var request = new TransferCommand(transferDTO);
        var handler = new TrasferCommandHandler(_contextMock.Object, _contextAccessorMock.Object, _mediatorMock.Object);
        var result = await handler.Handle(request, CancellationToken.None);

        //Assert
        Assert.NotNull(result);
        Assert.Equal("Account does not exist", result.Message.ToString());
    }

    [Fact]
    public async Task TransferCommand_CustomersIdNotFound()
    {
        //Arrange 
        var mock = MockDBContext.GetQueryableMockDbSet(FakeAccount.GenerateFakeActiveAccount());
        var claimsIdentity = new ClaimsIdentity(new Claim[]
        {
            new Claim(ClaimTypes.NameIdentifier, "")
        });

        var userPrincipal = new ClaimsPrincipal(claimsIdentity);
        _contextMock.Setup(x => x.Accounts).Returns(mock);
        _contextAccessorMock.Setup(x => x.HttpContext.User).Returns(userPrincipal);
        Transaction transaction = FakeTransactionHistory.GenerateFakeTransactionHistory().First(x => x.Id == Guid.Parse("e47fd612-f843-4470-a53b-eb63700a8b04"));
        TransferDTO transferDTO = new TransferDTO
        {
            AccountNumber = "6894628348",
            Amount = 100
        };

        //Act
        var request = new TransferCommand(transferDTO);
        var handler = new TrasferCommandHandler(_contextMock.Object, _contextAccessorMock.Object, _mediatorMock.Object);
        var result = await handler.Handle(request, CancellationToken.None);

        //Assert
        Assert.NotNull(result);
        Assert.Equal("Customer's ID not found", result.Message.ToString());
    }

    [Fact]
    public async Task TransferCommand_NoSelfTransfer()
    {
        //Arrange 
        var mock = MockDBContext.GetQueryableMockDbSet(FakeAccount.GenerateFakeActiveAccount());
        var claimsIdentity = new ClaimsIdentity(new Claim[]
        {
            new Claim(ClaimTypes.NameIdentifier, "ee99627b-a78d-47df-8bc1-94bd3501a4fd")
        });

        var userPrincipal = new ClaimsPrincipal(claimsIdentity);
        _contextMock.Setup(x => x.Accounts).Returns(mock);
        _contextAccessorMock.Setup(x => x.HttpContext.User).Returns(userPrincipal);
        Transaction transaction = FakeTransactionHistory.GenerateFakeTransactionHistory().First(x => x.Id == Guid.Parse("e47fd612-f843-4470-a53b-eb63700a8b04"));
        TransferDTO transferDTO = new TransferDTO
        {
            AccountNumber = "7894621348",
            Amount = 100
        };

        //Act
        var request = new TransferCommand(transferDTO);
        var handler = new TrasferCommandHandler(_contextMock.Object, _contextAccessorMock.Object, _mediatorMock.Object);
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
        var claimsIdentity = new ClaimsIdentity(new Claim[]
        {
         new Claim(ClaimTypes.NameIdentifier, "ee99627b-a78d-47df-8bc1-94bd3501a4fd")
        });

        var userPrincipal = new ClaimsPrincipal(claimsIdentity);
        _contextMock.Setup(x => x.Accounts).Returns(mock);
        _contextAccessorMock.Setup(x => x.HttpContext.User).Returns(userPrincipal);
        Transaction transaction = FakeTransactionHistory.GenerateFakeTransactionHistory().First(x => x.Id == Guid.Parse("e47fd612-f843-4470-a53b-eb63700a8b04"));
        TransferDTO transferDTO = new TransferDTO
        {
            AccountNumber = "6894628348",
            Amount = 100
        };

        //Act
        var request = new TransferCommand(transferDTO);
        var handler = new TrasferCommandHandler(_contextMock.Object, _contextAccessorMock.Object, _mediatorMock.Object);
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
        var claimsIdentity = new ClaimsIdentity(new Claim[]
        {
         new Claim(ClaimTypes.NameIdentifier, "ee99627b-a78d-47df-8bc1-94bd3501a4fd")
        });

        var userPrincipal = new ClaimsPrincipal(claimsIdentity);
        _contextMock.Setup(x => x.Accounts).Returns(mock);
        _contextAccessorMock.Setup(x => x.HttpContext.User).Returns(userPrincipal);
        Transaction transaction = FakeTransactionHistory.GenerateFakeTransactionHistory().First(x => x.Id == Guid.Parse("e47fd612-f843-4470-a53b-eb63700a8b04"));
        TransferDTO transferDTO = new TransferDTO
        {
            AccountNumber = "6894628348",
            Amount = 1000000
        };

        //Act
        var request = new TransferCommand(transferDTO);
        var handler = new TrasferCommandHandler(_contextMock.Object, _contextAccessorMock.Object, _mediatorMock.Object);
        var result = await handler.Handle(request, CancellationToken.None);

        //Assert
        Assert.NotNull(result);
        Assert.Equal("Insufficient Funds", result.Message.ToString());
    }

}

using Application.Accounts.AccountCommand;
using Application.Customers.CustomerCommand;
using Domain.DTO;
using Infrastructure.DBContext;
using MediatR;
using Mini_Core_Banking_Project.Test.Generate;
using Mini_Core_Banking_Project.Test.Services;
using Moq;

namespace Mini_Core_Banking_Project.Test.Customers.Command;

public class DeleteCustomerCommandTest
{
    private readonly Mock<IMediator> _mediator;
    private readonly Mock<IMiniCoreBankingDbContext> _contextMock;

    public DeleteCustomerCommandTest()
    {
        _mediator = new Mock<IMediator>();
        _contextMock = new Mock<IMiniCoreBankingDbContext>();
    }

    [Fact]
    public async void DeleteCustomerSuccess()
    {
        //Arrange
        var mock = MockDBContext.GetQueryableMockDbSet(FakeCustomer.GenerateCustomer());
        _contextMock.Setup(x => x.Customers).Returns(mock);
        Guid customerId = Guid.Parse("ee99627b-a78d-47df-8bc1-94bd3501a4fd");
        ResponseModel response = new ResponseModel
        {
            Message = "Account Deleted Successfully",
            Success = true
        };
        //Act
        var request = new DeleteCustomerCommand(customerId);
        _mediator.Setup(x => x.Send(It.IsAny<DeleteAccountCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(response);
        var handler = new DeleteCustomerCommandHandler(_contextMock.Object, _mediator.Object);
        var result= await handler.Handle(request,CancellationToken.None);

        //Assert
        Assert.NotNull(result);
        Assert.Equal("Deleted Successfully", result.Message.ToString());

    }
    [Fact]
    public async void DeleteCustomer_CustomerDoesNotExist()
    {
        //Arrange
        var mock = MockDBContext.GetQueryableMockDbSet(FakeCustomer.GenerateCustomer());
        _contextMock.Setup(x => x.Customers).Returns(mock);
        Guid customerId = Guid.Parse("ad89527b-a78d-47df-8bc1-94bd3501a7fb");
        //Act
        var request = new DeleteCustomerCommand(customerId);
        var handler = new DeleteCustomerCommandHandler(_contextMock.Object, _mediator.Object);
        var result = await handler.Handle(request, CancellationToken.None);

        //Assert
        Assert.NotNull(result);
        Assert.Equal("Customer does not exist", result.Message.ToString());

    }


}

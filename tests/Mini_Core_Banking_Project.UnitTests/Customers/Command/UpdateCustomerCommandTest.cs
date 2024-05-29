using Application.Customers.CustomerCommand;
using AutoMapper;
using Application.DTO;
using API.Test.Generate;
using API.Test.Services;
using Moq;
using Application.Interfaces;

namespace API.Test.Customers.Command;

public class UpdateCustomerCommandTest
{
    private readonly Mock<IMiniCoreBankingDbContext> _contextMock;
    private readonly Mock<IMapper> _mapper;

    public UpdateCustomerCommandTest()
    {
        _contextMock= new Mock<IMiniCoreBankingDbContext>();
        _mapper= new Mock<IMapper>();
    }
    [Fact]
    public async Task UpdateCustomerSuccess()
    {
        //Arrange
        var mock = MockDBContext.GetQueryableMockDbSet(FakeCustomer.GenerateCustomer());
        _contextMock.Setup(x => x.Customers).Returns(mock);
        Guid customerId = Guid.Parse("ee99627b-a78d-47df-8bc1-94bd3501a4fd");
        CustomerDTO customerDTO = new CustomerDTO
        {
            FirstName = "Mercy",
            LastName = "Johnson",
            Email = "cynthia2019@gmail.com",
            PhoneNumber = "0912786534",
            Address = "21A, foster street",
        };

        //Act
        var request = new UpdateCustomerCommand(customerId, customerDTO);
        var handler = new UpdateCustomerCommandHandler(_contextMock.Object, _mapper.Object);
        var result = await handler.Handle(request,CancellationToken.None);

        //Assert
        Assert.NotNull(result);
        Assert.Equal("Customer Updated Successfully", result.Message.ToString());

    }
    [Fact]
    public async Task UpdateCustomer_CustomerDoesNotExist()
    {
        //Arrange
        var mock = MockDBContext.GetQueryableMockDbSet(FakeCustomer.GenerateCustomer());
        _contextMock.Setup(x => x.Customers).Returns(mock);
        Guid customerId = Guid.Parse("bd69827b-a78d-47df-8bc1-94bd3501a4fd");
        CustomerDTO customerDTO = new CustomerDTO
        {
            FirstName = "Mercy",
            LastName = "Johnson",
            Email = "cynthia2019@gmail.com",
            PhoneNumber = "0912786534",
            Address = "21A, foster street",
        };

        //Act
        var request = new UpdateCustomerCommand(customerId, customerDTO);
        var handler = new UpdateCustomerCommandHandler(_contextMock.Object, _mapper.Object);
        var result = await handler.Handle(request, CancellationToken.None);

        //Assert
        Assert.NotNull(result);
        Assert.Equal("Customer does not exist", result.Message.ToString());

    }
}

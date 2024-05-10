using Application.Customers.CustomerQuery;
using AutoMapper;
using API.Test.Generate;
using API.Test.Services;
using Moq;
using Application.Interfaces;

namespace API.Test.Customers.Query;

public class ViewCustomerByIdQueryTest
{
    private readonly Mock<IMiniCoreBankingDbContext> _contextMock;
    private readonly Mock<IMapper> _mapper;

    public ViewCustomerByIdQueryTest()
    {
        _contextMock = new Mock<IMiniCoreBankingDbContext>();
        _mapper = new Mock<IMapper>();
    }

    [Fact]
    public async Task ViewCustomerByIdSuccess()
    {
        //Arrange
        var mock = MockDBContext.GetQueryableMockDbSet(FakeCustomer.GenerateCustomer());
        _contextMock.Setup(x => x.Customers).Returns(mock);
        Guid customerId = Guid.Parse("ee99627b-a78d-47df-8bc1-94bd3501a4fd");

        //Act
        var request = new ViewCustomerByIdQuery(customerId);
        var handler = new ViewCustomerByIdQueryHandler(_contextMock.Object, _mapper.Object);
        var result = await handler.Handle(request, CancellationToken.None);

        //Assert 
        Assert.NotNull(result);
        Assert.Equal("Customer returned successfully", result.Message.ToString());
    }

    [Fact]
    public async Task ViewCustomerById_CustomerDoesNotExist()
    {
        //Arrange
        var mock = MockDBContext.GetQueryableMockDbSet(FakeCustomer.GenerateCustomer());
        _contextMock.Setup(x => x.Customers).Returns(mock);
        Guid customerId = Guid.Parse("db50827b-b86d-47df-8bc1-94bd3501a4fd");

        //Act
        var request = new ViewCustomerByIdQuery(customerId);
        var handler = new ViewCustomerByIdQueryHandler(_contextMock.Object, _mapper.Object);
        var result = await handler.Handle(request, CancellationToken.None);

        //Assert 
        Assert.NotNull(result);
        Assert.Equal("Customer does not exist", result.Message.ToString());
    }
}

using Application.Customers.CustomerQuery;
using AutoMapper;
using API.Test.Generate;
using API.Test.Services;
using Moq;
using Application.Interfaces;

namespace API.Test.Customers.Query;

public class ViewCustomersQueryTest
{
    private readonly Mock<IMiniCoreBankingDbContext> _contextMock;
    private readonly Mock<IMapper> _mapper;

    public ViewCustomersQueryTest()
    {
        _contextMock = new Mock<IMiniCoreBankingDbContext>();
        _mapper = new Mock<IMapper>();
    }
    [Fact]
    public async Task ViewCustomersQuerySuccess()
    {
        //Arrange
        var mock = MockDBContext.GetQueryableMockDbSet(FakeCustomer.GenerateCustomer());
        _contextMock.Setup(x => x.Customers).Returns(mock);

        //Act
        var request = new ViewCustomersQuery();
        var handler = new ViewCustomerQueryHandler(_contextMock.Object, _mapper.Object);
        var result = await handler.Handle(request, CancellationToken.None);

        //Assert
        Assert.NotNull(result);
        Assert.Equal("Customers returned successfully", result.Message.ToString());
    }

    [Fact]
    public async Task ViewCustomersQuery_NoCustomersCreated()
    {
        //Arrange
        var mock = MockDBContext.GetQueryableMockDbSet(FakeCustomer.GenerateEmptyCustomer());
        _contextMock.Setup(x => x.Customers).Returns(mock);

        //Act
        var request = new ViewCustomersQuery();
        var handler = new ViewCustomerQueryHandler(_contextMock.Object, _mapper.Object);
        var result = await handler.Handle(request, CancellationToken.None);

        //Assert
        Assert.NotNull(result);
        Assert.Equal("No customer has been created", result.Message.ToString());
    }
}

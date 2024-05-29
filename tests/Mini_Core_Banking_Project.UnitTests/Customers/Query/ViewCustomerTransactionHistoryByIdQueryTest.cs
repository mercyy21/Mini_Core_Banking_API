using Application.Customers.Query;
using API.Test.Generate;
using API.Test.Services;
using Moq;
using Application.Interfaces;

namespace API.Test.Customers.Query;

public class ViewCustomerTransactionHistoryByIdQueryTest
{
    private readonly Mock<IMiniCoreBankingDbContext> _contextMock;

    public ViewCustomerTransactionHistoryByIdQueryTest()
    {
        _contextMock = new Mock<IMiniCoreBankingDbContext>();
    }

    [Fact]
    public async Task ViewCustomerTransactionHistoryById_Success()
    {
        //Arrange
        var mock = MockDBContext.GetQueryableMockDbSet(FakeTransactionHistory.GenerateFakeTransactionHistory());
        Guid customerId = Guid.Parse("ee99627b-a78d-47df-8bc1-94bd3501a4fd");
        var customerMock = MockDBContext.GetQueryableMockDbSet(FakeCustomer.GenerateCustomer());    
        _contextMock.Setup(x => x.Transactions).Returns(mock);
        _contextMock.Setup(x=> x.Customers).Returns(customerMock);

        //Act
        var request = new ViewCustomerTransaction_HistoryByIdQuery(customerId);
        var handler = new ViewCustomerTransaction_HistoryByIdQueryHandler(_contextMock.Object);
        var result = await handler.Handle(request, CancellationToken.None);

        //Assert
        Assert.NotNull(result);
        Assert.Equal("Customers transaction history returned successfully", result.Message.ToString());
    }

    [Fact]
    public async Task ViewCustomerTransactionHistoryById_CustomerDoesNotExist()
    {
        //Arrange
        var mock = MockDBContext.GetQueryableMockDbSet(FakeTransactionHistory.GenerateFakeTransactionHistory());
        var customerMock = MockDBContext.GetQueryableMockDbSet(FakeCustomer.GenerateCustomer());
        Guid customerId = Guid.Parse("db50827b-b86d-47df-8bc1-94bd3501a4fd");
        _contextMock.Setup(x => x.Transactions).Returns(mock);
        _contextMock.Setup(x => x.Customers).Returns(customerMock);

        //Act
        var request = new ViewCustomerTransaction_HistoryByIdQuery(customerId);
        var handler = new ViewCustomerTransaction_HistoryByIdQueryHandler(_contextMock.Object);
        var result = await handler.Handle(request, CancellationToken.None);

        //Assert 
        Assert.NotNull(result);
        Assert.Equal("Customer does not exist", result.Message.ToString());
    }
    [Fact]
    public async Task ViewCustomerTransactionHistoryById_ZeroTransactions()
    {
        //Arrange
        var mock = MockDBContext.GetQueryableMockDbSet(FakeTransactionHistory.GenerateEmptyFakeTransactionHistory());
        Guid customerId = Guid.Parse("ee99627b-a78d-47df-8bc1-94bd3501a4fd");
        var customerMock = MockDBContext.GetQueryableMockDbSet(FakeCustomer.GenerateCustomer());
        _contextMock.Setup(x => x.Transactions).Returns(mock);
        _contextMock.Setup(x => x.Customers).Returns(customerMock);

        //Act
        var request = new ViewCustomerTransaction_HistoryByIdQuery(customerId);
        var handler = new ViewCustomerTransaction_HistoryByIdQueryHandler(_contextMock.Object);
        var result = await handler.Handle(request, CancellationToken.None);

        //Assert 
        Assert.NotNull(result);
        Assert.Equal("Customer has made zero transactions", result.Message.ToString());
    }

}

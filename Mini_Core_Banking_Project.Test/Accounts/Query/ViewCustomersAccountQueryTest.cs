using Application.Accounts.AccountQuery;
using AutoMapper;
using Infrastructure.DBContext;
using Mini_Core_Banking_Project.Test.Generate;
using Mini_Core_Banking_Project.Test.Services;
using Moq;

namespace Mini_Core_Banking_Project.Test.Accounts.Query;

public class ViewCustomersAccountQueryTest
{
    private readonly Mock<IMiniCoreBankingDbContext> _contextMock;
    private readonly Mock<IMapper> _mapper;

    public ViewCustomersAccountQueryTest()
    {
        _contextMock = new Mock<IMiniCoreBankingDbContext>();
        _mapper = new Mock<IMapper>();
    }

    [Fact]
    public async Task ViewCustomersAccountByIdSuccess()
    {
        //Arrange
        var mock = MockDBContext.GetQueryableMockDbSet(FakeAccount.GenerateFakeActiveAccount());
        _contextMock.Setup(x => x.Accounts).Returns(mock);
        Guid customerId = Guid.Parse("ee99627b-a78d-47df-8bc1-94bd3501a4fd");

        //Act
        var request = new ViewCustomersAccountQuery(customerId);
        var handler = new ViewCustomersAccountQueryHandler(_contextMock.Object, _mapper.Object);
        var result = await handler.Handle(request, CancellationToken.None);

        //Assert 
        Assert.NotNull(result);
        Assert.Equal("Customers Account returned successfully", result.Message.ToString());
    }

    [Fact]
    public async Task ViewCustomersAccountById_CustomersAccountDoesNotExist()
    {
        //Arrange
        var mock = MockDBContext.GetQueryableMockDbSet(FakeAccount.GenerateFakeActiveAccount());
        _contextMock.Setup(x => x.Accounts).Returns(mock);
        Guid customerId = Guid.Parse("db50827b-b86d-47df-8bc1-94bd3501a4fd");

        //Act
        var request = new ViewCustomersAccountQuery(customerId);
        var handler = new ViewCustomersAccountQueryHandler(_contextMock.Object, _mapper.Object);
        var result = await handler.Handle(request, CancellationToken.None);

        //Assert 
        Assert.NotNull(result);
        Assert.Equal("Account does not exist", result.Message.ToString());
    }


}

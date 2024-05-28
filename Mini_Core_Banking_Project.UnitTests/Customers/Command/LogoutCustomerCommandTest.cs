using Application.Customers.Command;
using Microsoft.AspNetCore.Http;
using API.Test.Generate;
using API.Test.Services;
using Moq;
using System.Security.Claims;
using Application.Interfaces;

namespace API.Test.Customers.Command
{
    public class LogoutCustomerCommandTest
    {
        private readonly Mock<IMiniCoreBankingDbContext> _contextMock;
        private readonly Mock<IHttpContextAccessor> _mockHttpContextAccessor;

        public LogoutCustomerCommandTest()
        {
            _contextMock= new Mock<IMiniCoreBankingDbContext>();
            _mockHttpContextAccessor= new Mock<IHttpContextAccessor>();
        }
        [Fact]
        public async Task LogoutCustomerSuccess()
        {
            //Arrange
            var mock = MockDBContext.GetQueryableMockDbSet(FakeCustomer.GenerateCustomer());
            _contextMock.Setup(x => x.Customers).Returns(mock);
            var claimsIdentity = new ClaimsIdentity(new Claim[]
        {
            new Claim(ClaimTypes.NameIdentifier, "ee99627b-a78d-47df-8bc1-94bd3501a4fd")
        });

            var userPrincipal = new ClaimsPrincipal(claimsIdentity);

            _mockHttpContextAccessor.Setup(x => x.HttpContext.User)
                                  .Returns(userPrincipal);
            //Act
            var request = new LogoutCustomerCommand();
            var handler = new LogoutCommandHandler(_contextMock.Object, _mockHttpContextAccessor.Object);
            var result = await handler.Handle(request, CancellationToken.None);

            //Assert
            Assert.NotNull(result);
            Assert.Equal("Logout successful", result.Message.ToString());

        }
        public async Task LogoutCustomer_CustomerDoesNotExist()
        {
            //Arrange
            var mock = MockDBContext.GetQueryableMockDbSet(FakeCustomer.GenerateCustomer());
            _contextMock.Setup(x => x.Customers).Returns(mock);
            var claimsIdentity = new ClaimsIdentity(new Claim[]
        {
            new Claim(ClaimTypes.NameIdentifier, "eb82627b-a78d-47df-8bc1-94bd3501a4fd")
        });

            var userPrincipal = new ClaimsPrincipal(claimsIdentity);

            _mockHttpContextAccessor.Setup(x => x.HttpContext.User)
                                  .Returns(userPrincipal);
            //Act
            var request = new LogoutCustomerCommand();
            var handler = new LogoutCommandHandler(_contextMock.Object, _mockHttpContextAccessor.Object);
            var result = await handler.Handle(request, CancellationToken.None);

            //Assert
            Assert.NotNull(result);
            Assert.Equal("Customer does not exist", result.Message.ToString());

        }
        public async Task LogoutCustomer_CustomerIdNotFound()
        {
            //Arrange
            var mock = MockDBContext.GetQueryableMockDbSet(FakeCustomer.GenerateCustomer());
            _contextMock.Setup(x => x.Customers).Returns(mock);
            var claimsIdentity = new ClaimsIdentity(new Claim[]
        {
            new Claim(ClaimTypes.NameIdentifier, null)
        });

            var userPrincipal = new ClaimsPrincipal(claimsIdentity);

            _mockHttpContextAccessor.Setup(x => x.HttpContext.User)
                                  .Returns(userPrincipal);
            //Act
            var request = new LogoutCustomerCommand();
            var handler = new LogoutCommandHandler(_contextMock.Object, _mockHttpContextAccessor.Object);
            var result = await handler.Handle(request, CancellationToken.None);

            //Assert
            Assert.NotNull(result);
            Assert.Equal("Customer's ID not found", result.Message.ToString());

        }
    }
}

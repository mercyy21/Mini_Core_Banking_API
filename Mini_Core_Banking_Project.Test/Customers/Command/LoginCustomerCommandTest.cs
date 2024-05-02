using Application.Customers.Command;
using Application.Customers.PasswordHasher;
using Domain.Domain.Entity;
using Infrastructure.DBContext;
using Mini_Core_Banking_Project.Test.Generate;
using Mini_Core_Banking_Project.Test.Services;
using Moq;

namespace Mini_Core_Banking_Project.Test.Customers.Command
{
    public class LoginCustomerCommandTest
    {
       /* private readonly Mock<IMiniCoreBankingDbContext> _contextMock;
        private readonly Mock<IHasher> _mockHasher;

        public LoginCustomerCommandTest()
        {
            _contextMock= new Mock<IMiniCoreBankingDbContext>();
            _mockHasher = new Mock<IHasher>();
        }
        [Fact]
        public async Task LoginSuccessful()
        {
            //Arrange
            var mock = MockDBContext.GetQueryableMockDbSet(FakeCustomer.GenerateCustomer());
            _contextMock.Setup(x => x.Customers).Returns(mock);
            string email = "cyxsa20@gmail.com";
            string password = "mercy12345";

            //Act
            var request = new LoginCustomerCommand(email, password);
            _mockHasher.Setup(h => h.VerifyPassword(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                 .Returns(true);
            _contextMock.Setup(db => db.RefreshTokens.AddAsync(It.IsAny<RefreshToken>(), default))
                  .Returns(Task.CompletedTask);
            var handler = new LoginCustomerCommandHandler(_contextMock.Object,_mockHasher.Object);
            var result = await handler.Handle(request, CancellationToken.None);

            //Assert 
            Assert.NotNull(result);
            Assert.Equal("Login Successful", result.Message.ToString());
        }*/
    }
}

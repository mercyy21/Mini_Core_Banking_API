using Application.Customers.Command;
using Application.Interfaces;
using Application.Domain.Entity;
using API.Test.Generate;
using API.Test.Services;
using Moq;

namespace API.Test.Customers.Command
{
    public class LoginCustomerCommandTest
    {
        private readonly Mock<IMiniCoreBankingDbContext> _contextMock;
        private readonly Mock<IHasher> _mockHasher;
        private readonly Mock<IJwtToken> _jwtTokenMock;

        public LoginCustomerCommandTest()
        {
            _contextMock= new Mock<IMiniCoreBankingDbContext>();
            _mockHasher = new Mock<IHasher>();
            _jwtTokenMock= new Mock<IJwtToken>();
        }
        [Fact]
        public async Task LoginSuccessful()
        {
            //Arrange
            var mock = MockDBContext.GetQueryableMockDbSet(FakeCustomer.GenerateCustomer());
            var refreshTokenMock = MockDBContext.GetQueryableMockDbSet(FakeRefreshToken.GenerateFakeResfreshToken());
            string email = "cyxsa20@gmail.com";
            string password = "mercy12345";
            string tokenString = "ertyuiolkjhgfdxcvbdfserfghjgrt5tghnmjku76tgfvcdferr";
            string refreshToken = "2werthgfr45678iu870=9_efm";
            Customer existingCustomer = FakeCustomer.GenerateCustomer().First(x=> x.Email==email);
            _contextMock.Setup(x => x.Customers).Returns(mock);
            _mockHasher.Setup(x=>x.VerifyPassword(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(true);
            _jwtTokenMock.Setup(x => x.GenerateJWTToken(existingCustomer)).Returns(tokenString);
            _jwtTokenMock.Setup(x => x.GenerateRefreshToken()).Returns(refreshToken);
            _contextMock.Setup(x => x.RefreshTokens).Returns(refreshTokenMock);

            //Act
            var request = new LoginCustomerCommand(email, password);
            var handler = new LoginCustomerCommandHandler(_contextMock.Object,_mockHasher.Object,_jwtTokenMock.Object);
            var result = await handler.Handle(request, CancellationToken.None);

            //Assert 
            Assert.NotNull(result);
            Assert.Equal("Login Successful", result.Message.ToString());
        }

        [Fact]
        public async Task Login_EmailDoesNotExist()
        {
            //Arrange
            var mock = MockDBContext.GetQueryableMockDbSet(FakeCustomer.GenerateCustomer());
            var refreshTokenMock = MockDBContext.GetQueryableMockDbSet(FakeRefreshToken.GenerateFakeResfreshToken());
            string email = "pdsa20@gmail.com";
            string password = "mercy12345";
            string tokenString = "ertyuiolkjhgfdxcvbdfserfghjgrt5tghnmjku76tgfvcdferr";
            string refreshToken = "2werthgfr45678iu870=9_efm";
            _contextMock.Setup(x => x.Customers).Returns(mock);
            _mockHasher.Setup(x => x.VerifyPassword(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(true);
            _jwtTokenMock.Setup(x => x.GenerateJWTToken(It.IsAny<Customer>())).Returns(tokenString);
            _jwtTokenMock.Setup(x => x.GenerateRefreshToken()).Returns(refreshToken);
            _contextMock.Setup(x => x.RefreshTokens).Returns(refreshTokenMock);

            //Act
            var request = new LoginCustomerCommand(email, password);
            var handler = new LoginCustomerCommandHandler(_contextMock.Object, _mockHasher.Object, _jwtTokenMock.Object);
            var result = await handler.Handle(request, CancellationToken.None);

            //Assert 
            Assert.NotNull(result);
            Assert.Equal("This email does not exist in our database", result.Message.ToString());
        }
         [Fact]
         public async Task Login_WrongPassword()
         {
             //Arrange
             var mock = MockDBContext.GetQueryableMockDbSet(FakeCustomer.GenerateCustomer());
             var refreshTokenMock = MockDBContext.GetQueryableMockDbSet(FakeRefreshToken.GenerateFakeResfreshToken());
             string email = "cyxsa20@gmail.com";
             string password = "Marcy21";
             string tokenString = "ertyuiolkjhgfdxcvbdfserfghjgrt5tghnmjku76tgfvcdferr";
             string refreshToken = "2werthgfr45678iu870=9_efm";
             Customer existingCustomer = FakeCustomer.GenerateCustomer().First(x=> x.Email==email);
             _contextMock.Setup(x => x.Customers).Returns(mock);
             _mockHasher.Setup(x=>x.VerifyPassword(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(false);
             _jwtTokenMock.Setup(x => x.GenerateJWTToken(existingCustomer)).Returns(tokenString);
             _jwtTokenMock.Setup(x => x.GenerateRefreshToken()).Returns(refreshToken);
             _contextMock.Setup(x => x.RefreshTokens).Returns(refreshTokenMock);

             //Act
             var request = new LoginCustomerCommand(email, password);
             var handler = new LoginCustomerCommandHandler(_contextMock.Object,_mockHasher.Object,_jwtTokenMock.Object);
             var result = await handler.Handle(request, CancellationToken.None);

             //Assert 
             Assert.NotNull(result);
             Assert.Equal("Wrong password, try again", result.Message.ToString());
         }
    }
}

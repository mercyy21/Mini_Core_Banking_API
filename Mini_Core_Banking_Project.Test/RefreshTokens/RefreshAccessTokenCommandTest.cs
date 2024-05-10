using Application.RefreshTokens;
using AutoFixture;
using API.Test.Generate;
using API.Test.Services;
using Moq;
using Application.Interfaces;

namespace API.Test.RefreshTokens;

public class RefreshAccessTokenCommandTest
{
    private readonly Mock<IMiniCoreBankingDbContext> _contextMock;
    private readonly Mock<IJwtToken> _jwtTokenMock;

    public RefreshAccessTokenCommandTest()
    {
        _contextMock= new Mock<IMiniCoreBankingDbContext>();
        _jwtTokenMock= new Mock<IJwtToken>();
    }

    [Fact]
    public async Task RefreshAccessTokenSuccess()
    {
        //Arrange
        var mock = MockDBContext.GetQueryableMockDbSet(FakeRefreshToken.GenerateFakeResfreshToken());
        var customerMock = MockDBContext.GetQueryableMockDbSet(FakeCustomer.GenerateCustomer());
        string token = "2werthgfr45678iu870=9_efm";
        _contextMock.Setup(x => x.Customers).Returns(customerMock);
        _contextMock.Setup(x => x.RefreshTokens).Returns(mock);

        //Act
        var request = new RefreshAccessTokenCommand(token);
        var handler = new RefreshTokenHandler(_contextMock.Object, _jwtTokenMock.Object);
        var result = await handler.Handle(request, CancellationToken.None);

        //Assert
        Assert.NotNull(result);
        Assert.Equal("Access Token Generated Successfully", result.Message.ToString());
    }
    public async Task RefreshAccessToken_InvalidToken()
    {
        //Arrange
        var mock = MockDBContext.GetQueryableMockDbSet(FakeRefreshToken.GenerateFakeResfreshToken());
        string token = "3qrtthgfr45678iu870g9_edm";
        _contextMock.Setup(x => x.RefreshTokens).Returns(mock);

        //Act
        var request = new RefreshAccessTokenCommand(token);
        var handler = new RefreshTokenHandler(_contextMock.Object, _jwtTokenMock.Object);
        var result = await handler.Handle(request, CancellationToken.None);

        //Assert
        Assert.NotNull(result);
        Assert.Equal("Invalid Token", result.Message.ToString());
    }
    public async Task RefreshAccessToken_TokenHasExpired()
    {
        //Arrange
        var mock = MockDBContext.GetQueryableMockDbSet(FakeRefreshToken.GenerateFakeResfreshToken_Expired());
        string token = "2werthgfr45678iu870=9_efm";
        _contextMock.Setup(x => x.RefreshTokens).Returns(mock);

        //Act
        var request = new RefreshAccessTokenCommand(token);
        var handler = new RefreshTokenHandler(_contextMock.Object, _jwtTokenMock.Object);
        var result = await handler.Handle(request, CancellationToken.None);

        //Assert
        Assert.NotNull(result);
        Assert.Equal("Refresh token has expired", result.Message.ToString());
    }

}

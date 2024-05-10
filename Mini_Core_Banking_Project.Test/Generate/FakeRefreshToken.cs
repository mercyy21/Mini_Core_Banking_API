using Application.Domain.Entity;

namespace API.Test.Generate;

public static class FakeRefreshToken
{
    public static List<RefreshToken> GenerateFakeResfreshToken()
    {
        return new List<RefreshToken>
        {
            new RefreshToken
            {
                Id= Guid.Parse("2b26d33b-c138-4261-bc90-9c285013765d"),
                CustomerId= Guid.Parse("ee99627b-a78d-47df-8bc1-94bd3501a4fd"),
                ExpiresAt= DateTime.UtcNow.AddDays(7),
                Token= "2werthgfr45678iu870=9_efm"
            }
        };
    }
    public static List<RefreshToken> GenerateFakeResfreshToken_Expired()
    {
        return new List<RefreshToken>
        {
            new RefreshToken
            {
                Id= Guid.Parse("2b26d33b-c138-4261-bc90-9c285013765d"),
                CustomerId= Guid.Parse("ee99627b-a78d-47df-8bc1-94bd3501a4fd"),
                ExpiresAt= DateTime.UtcNow,
                Token= "2werthgfr45678iu870=9_efm"
            }
        };
    }
}

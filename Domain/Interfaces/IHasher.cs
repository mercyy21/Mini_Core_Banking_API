namespace Application.Customers.PasswordHasher
{
    public interface IHasher
    {
        public (string HashedPassword, string Salt) HashPassword(string password);
        public bool VerifyPassword(string password, string hashedPassword, string salt);
        

    }
}

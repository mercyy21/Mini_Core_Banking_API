namespace Application.DTO
{
    public class AuthenticatedResponse
    {
        public string AccessToken { get; set; }
        public string Message { get; set; }
        public bool Success { get; set; }
    }
}

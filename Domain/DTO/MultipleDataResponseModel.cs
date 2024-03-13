namespace Domain.DTO
{
    public class MultipleDataResponseModel
    {
        public List<object> Data { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }
    }
}

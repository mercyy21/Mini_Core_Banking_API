namespace Application.DTO
{
    public class TransferDTO
    {
        public string? SendersAccountNumber { get; set; }
        public string? ReceiversAccountNumber { get; set; }
        public double Amount { get; set; }
    }
}

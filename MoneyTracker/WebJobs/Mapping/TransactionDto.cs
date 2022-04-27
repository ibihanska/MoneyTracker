namespace WebJobs
{
    public class TransactionDto
    {
        public Guid Id { get; set; }
        public Guid? FromAccountId { get; set; }
        public Guid? ToAccountId { get; set; }
        public DateTime TransactionDate { get; set; }
        public string? TagName { get; set; }
        public string TransactionType { get; set; }
        public decimal Amount { get; set; }
        public string? Note { get; set; }
        public string? FromAccountName { get; set; }
        public string? ToAccountName { get; set; }
    }
}

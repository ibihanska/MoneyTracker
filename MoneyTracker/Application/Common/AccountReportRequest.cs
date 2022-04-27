namespace MoneyTracker.Application.Common
{
    public class AccountReportRequest
    {
        public const string QueueName = "account-report-generation-queue";
        public Guid AccountId { get; set; }
    }
}

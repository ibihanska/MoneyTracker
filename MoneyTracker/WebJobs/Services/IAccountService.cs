namespace WebJobs
{
    public interface IAccountService
    {
        List<TransactionDto> GetReport(Guid accountId);
    }
}

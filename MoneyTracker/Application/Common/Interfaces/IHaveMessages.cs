using MoneyTracker.Application.Common;

namespace MoneyTracker.Application.TransactionCommands
{
    public interface IHaveMessages
    {
        List<AccountReportMessage> AccountReportMessages { get; set; }
    }
}

using MoneyTracker.Infrastructure.Services;

namespace MoneyTracker.SPA.Services
{
    public class MachineDateTime : IDateTime
    {
        public DateTime Now => DateTime.Now;

        public int CurrentYear => DateTime.Now.Year;
    }
}

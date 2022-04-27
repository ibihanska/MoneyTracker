using MoneyTracker.Application.Common.Interfaces;

namespace WebJobs.Services
{
    public class MachineDateTime : IDateTime
    {
        public DateTime Now => DateTime.Now;

        public int CurrentYear => DateTime.Now.Year;
    }
}

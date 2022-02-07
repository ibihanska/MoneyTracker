using System;

namespace MoneyTracker.Domain.Common
{
    public class BadRequestException : Exception
    {
        public BadRequestException(string message)
            : base(message)
        {
        }
    }
}

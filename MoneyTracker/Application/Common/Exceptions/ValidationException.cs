using FluentValidation.Results;
using System.Linq;

namespace MoneyTracker.Application.Common.Exceptions
{
    public class ValidationException : Exception
    {
        public ValidationException()
            : base("One or more validation failures have occurred.")
        {
            Failures = new Dictionary<string, IEnumerable<string>>();
        }

        public ValidationException(List<ValidationFailure> failures)
            : this()
        {
            Failures = failures.GroupBy(x => x.PropertyName)
                   .ToDictionary(
                     x => x.Key,
                     x => x.Select(f => f.ErrorMessage));
        }

        public IReadOnlyDictionary<string, IEnumerable<string>> Failures { get; } 
    }
}

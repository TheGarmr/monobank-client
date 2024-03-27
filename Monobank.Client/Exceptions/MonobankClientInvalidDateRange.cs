using System;

namespace Monobank.Client.Exceptions
{
    public class MonobankClientInvalidDateRange : Exception
    {
        public MonobankClientInvalidDateRange(string message) : base(message)
        {

        }

        public static MonobankClientInvalidDateRange Create(DateTime from, DateTime to)
        {
            return new MonobankClientInvalidDateRange($"Time range between {from} and {to} is invalid. Difference between 'from' and 'to' should be less than 31 day + 1 hour.");
        }
    }
}
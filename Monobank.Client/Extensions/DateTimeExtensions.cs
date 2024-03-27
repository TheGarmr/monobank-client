using System;

namespace Monobank.Client.Extensions
{
    internal static class DateTimeExtensions
    {
        internal static int ToUnixTime(this DateTime date)
        {
            return (int) date.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
        }
    }
}

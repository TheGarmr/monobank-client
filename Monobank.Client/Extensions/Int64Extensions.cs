using System;

namespace Monobank.Client.Extensions
{
    internal static class Int64Extensions
    {
        internal static DateTime ToDateTime(this long seconds)
        {
            return new DateTime(1970, 1, 1).AddSeconds(seconds);
        }

        internal static double AsMoney(this long input)
        {
            var value = input.ToString();

            if (string.IsNullOrWhiteSpace(value) || value.Length < 3)
            {
                return 0;
            }

            var balance = value.Insert(value.Length - 2, ".");
            var parsed = double.TryParse(balance, out var parsedValue);
            return parsed ? parsedValue : 0;
        }

        internal static double AsMoney(this long? input)
        {
            return input?.AsMoney() ?? 0;
        }
    }
}

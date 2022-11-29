using System;

namespace Monobank.Client.Extensions
{
    public static class Int64Extensions
    {
        public static DateTime ToDateTime(this long seconds)
        {
            return new DateTime(1970, 1, 1).AddSeconds(seconds);
        }
        
        public static double AsMoney(this long input)
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
    }
}

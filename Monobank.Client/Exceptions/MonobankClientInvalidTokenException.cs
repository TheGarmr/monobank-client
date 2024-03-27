using System;

namespace Monobank.Client.Exceptions
{
    public class MonobankClientInvalidTokenException : Exception
    {
        public MonobankClientInvalidTokenException(string message) : base(message)
        {

        }

        public static MonobankClientInvalidTokenException Create(string token)
        {
            return new MonobankClientInvalidTokenException($"User token {token} is invalid.");
        }
    }
}
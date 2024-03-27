using System;

namespace Monobank.Client.Exceptions
{
    public class MonobankClientApiException : Exception
    {
        public MonobankClientApiException()
        {

        }

        public MonobankClientApiException(string message) : base(message)
        {

        }

        public static MonobankClientApiException Create(string description)
        {
            return new MonobankClientApiException($"An error occurred while requesting the Monobank api. Error message: {description}.");
        }
    }
}
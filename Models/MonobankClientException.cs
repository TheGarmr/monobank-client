using System;

namespace Monobank.Client.Models
{
#pragma warning disable S3925 // "ISerializable" should be implemented correctly
    public class MonobankClientException : Exception
#pragma warning restore S3925 // "ISerializable" should be implemented correctly
    {
        public MonobankClientException()
        {
            
        }

        public MonobankClientException(string message) : base(message)
        {
            
        }

        public MonobankClientException(string message, System.Exception innerException) : base(message, innerException)
        {
            
        }
    }
}
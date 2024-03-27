using System;

namespace Monobank.Client.Exceptions
{
    public class MonobankClientInvalidWebhookException : Exception
    {
        public MonobankClientInvalidWebhookException(string message) : base(message)
        {

        }

        public static MonobankClientInvalidWebhookException Create(string url)
        {
            return new MonobankClientInvalidWebhookException($"Webhook url {url} is invalid.");
        }
    }
}
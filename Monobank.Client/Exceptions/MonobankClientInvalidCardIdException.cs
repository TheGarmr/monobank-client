using System;

namespace Monobank.Client.Exceptions
{
    public class MonobankClientInvalidCardIdException : Exception
    {
        public MonobankClientInvalidCardIdException(string message) : base(message)
        {

        }

        public static MonobankClientInvalidCardIdException Create(string cardId)
        {
            return new MonobankClientInvalidCardIdException($"Card id {cardId} is invalid.");
        }
    }
}
using System;
using System.Runtime.Serialization;

namespace EntityDataModel.CustomExceptions
{
    public class NoBookReservationsExistingException : Exception
    {
        public NoBookReservationsExistingException()
        {
        }

        public NoBookReservationsExistingException(string message) : base(message)
        {
        }

        public NoBookReservationsExistingException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected NoBookReservationsExistingException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
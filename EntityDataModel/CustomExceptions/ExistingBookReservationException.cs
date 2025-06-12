using System;
using System.Runtime.Serialization;

namespace EntityDataModel.CustomExceptions
{
    public class ExistingBookReservationException : Exception
    {
        public ExistingBookReservationException()
        {
        }

        public ExistingBookReservationException(string message) : base(message)
        {
        }

        public ExistingBookReservationException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ExistingBookReservationException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
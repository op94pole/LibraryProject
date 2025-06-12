using System;
using System.Runtime.Serialization;

namespace EntityDataModel.CustomExceptions
{
    public class AlreadyExistingReservationException : Exception
    {

        public AlreadyExistingReservationException() 
        {
        }

        public AlreadyExistingReservationException(string message) : base(message)
        {
        }

        public AlreadyExistingReservationException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected AlreadyExistingReservationException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
using System;
using System.Runtime.Serialization;

namespace EntityDataModel.CustomExceptions
{
    public class NoAvailableCopiesException : Exception
    {
        public NoAvailableCopiesException()
        {
        }
        public NoAvailableCopiesException(string message) : base(message) 
        {
        }

        public NoAvailableCopiesException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected NoAvailableCopiesException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
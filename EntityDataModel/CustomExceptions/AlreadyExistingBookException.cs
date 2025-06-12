using System;
using System.Runtime.Serialization;

namespace EntityDataModel.CustomExceptions
{
    public class AlreadyExistingBookException : Exception
    {
        public AlreadyExistingBookException()
        {
        }

        public AlreadyExistingBookException(string message) : base(message)
        {
        }

        public AlreadyExistingBookException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected AlreadyExistingBookException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
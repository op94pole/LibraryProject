using System;
using System.Runtime.Serialization;

namespace EntityDataModel.CustomExceptions
{
    public class NoMatchingResultsException : Exception
    {
        public NoMatchingResultsException()
        {
        }

        public NoMatchingResultsException(string message) : base(message)
        {
        }

        public NoMatchingResultsException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected NoMatchingResultsException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
using System;
using System.Runtime.Serialization;

namespace AssertExLib.Exceptions
{
    public class AssertExException : Exception
    {
        private string message;
        private Exception innerException;

        public AssertExException()
            : base()
        {
        }

        public AssertExException(string message)
            : base(message)
        {
            this.message = message;
        }

        public AssertExException(string message, Exception innerException)
            : base(message, innerException)
        {
            this.message = message;
            this.innerException = innerException;
        }
    }
}

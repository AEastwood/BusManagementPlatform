using System;

namespace Bus.Exceptions
{
    class InvalidAuthTokenException : Exception
    {
        public InvalidAuthTokenException() { }

        public InvalidAuthTokenException(string message) : base(message) { }

        public InvalidAuthTokenException(string message, Exception inner) : base(message, inner) { }

    }
}

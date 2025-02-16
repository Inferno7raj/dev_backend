using System;

namespace PersonalDetailsAPI.Exceptions
{
    public class DuplicateEntryException : Exception
    {
        public DuplicateEntryException() : base() { }

        public DuplicateEntryException(string message) : base(message) { }

        public DuplicateEntryException(string message, Exception innerException) : base(message, innerException) { }
    }
}

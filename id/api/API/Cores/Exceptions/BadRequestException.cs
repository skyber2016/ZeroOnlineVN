using System;

namespace API.Cores.Exceptions
{
    public class BadRequestException : Exception
    {
        public BadRequestException(string mess) : base(mess) { }
    }
}

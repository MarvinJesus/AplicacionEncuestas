using Entities_POJO;
using System;

namespace Exceptions
{
    public class BussinessException : Exception
    {
        public int Code;
        public ApplicationMessage AppMessage { get; set; }

        public BussinessException()
        {
        }

        public BussinessException(int code)
        {
            Code = code;
        }

        public BussinessException(int code, Exception innerException) : this(code)
        {
        }
    }
}

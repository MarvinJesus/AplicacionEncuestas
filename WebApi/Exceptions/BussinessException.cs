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

        public BussinessException(int exceptionId)
        {
            Code = exceptionId;
        }

        public BussinessException(int exceptionId, Exception innerException) : this(exceptionId)
        {
        }
    }
}

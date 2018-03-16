using System;
using System.Collections.Generic;
using System.Text;

namespace ITGA.Common.Exceptions
{
    public class ITGAException : Exception
    {
        public string Code { get; }

        public ITGAException()
        {
            
        }

        public ITGAException(string code)
        {
            Code = code;
        }

        public ITGAException(string message, params object[] args) : this(string.Empty, message, args)
        {
        }

        public ITGAException(string code, string message, params object[] args) : this(null, code, message, args)
        {
        }

        public ITGAException(Exception innerException, string message, params object[] args) : this(innerException,
            string.Empty, message, args)
        {

        }

        public ITGAException(Exception innerException, string code, string message, params object[] args) : base(
            string.Format(message, args), innerException)
        {
            Code = code;
        }
    }
}

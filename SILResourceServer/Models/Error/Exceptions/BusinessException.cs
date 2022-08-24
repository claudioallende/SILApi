using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ResourceServer.Models.Error.Exceptions
{
    public class BusinessException : System.Exception
    {
        public BusinessException(string message) : base(message) { }
    }
}
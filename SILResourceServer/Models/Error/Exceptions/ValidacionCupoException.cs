using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ResourceServer.Models.Error.Exceptions
{
    public class ValidacionCupoException : BusinessException
    {
        public ValidacionCupoException(string message) : base(message) { }
    }
}
using ResourceServer.Models.Error;
using ResourceServer.Models.Error.Exceptions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Filters;

namespace ResourceServer.Models.AtributosValidacion
{
    public class ExceptionHandlingAttribute : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext context)
        {
            if (context.Exception is BusinessException)
            {
                ErrorLog.WriteWarning(context.Exception);
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent(context.Exception.Message),
                    ReasonPhrase = "Excepción"
                });
            }
            else 
            {
                ErrorLog.Write(context.Exception);
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent("Ocurrió un error"),
                    ReasonPhrase = "Error"
                });
            }
        }
    }
}
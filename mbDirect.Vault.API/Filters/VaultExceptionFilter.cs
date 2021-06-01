using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using mbDirect.Vault.Models.ExceptionType;
using System.Text;
using Microsoft.AspNetCore.Http.Features;

namespace mbDirect.Vault.API.Filters
{
    public class VaultExceptionFilter: ExceptionFilterAttribute
    {

        public override void OnException(ExceptionContext context)
        {

            System.Diagnostics.StackFrame stack = new System.Diagnostics.StackFrame(1);
            String SourceModule = context.Exception.TargetSite == null ? stack.GetMethod().Name : context.Exception.TargetSite.Name;

            var resp = new HttpResponseMessage(HttpStatusCode.InternalServerError)
            {
                Content = new StringContent("Unexpected Error"),
                ReasonPhrase = "UnexpectedError"
            };

            if (context.Exception.GetType() == typeof(AccountException))
            {
                var ex = context.Exception as AccountException;
                var responseContent = ex.ToStringContent(context.HttpContext.Request.ContentType);
                var b = Encoding.UTF8.GetBytes(responseContent.ReadAsStringAsync().Result.ToString());
                context.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = ex.Code;
                context.HttpContext.Response.StatusCode = (int) ex.HttpStatus;
                context.HttpContext.Response.ContentType = responseContent.Headers.ContentType.MediaType;
                context.HttpContext.Response.Body.WriteAsync(b, 0, b.Length);
            }
            else
            {
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                var b = Encoding.UTF8.GetBytes("Unexpected Exception Occurred");
                context.HttpContext.Response.Body.WriteAsync(b, 0, b.Length);
            }
        }
    }
}

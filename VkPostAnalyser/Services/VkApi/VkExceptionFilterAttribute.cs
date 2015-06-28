using System.Net;
using System.Net.Http;
using System.Web.Http.Filters;
using VKSharp.Helpers.Exceptions;

namespace VkPostAnalyser.Services.VkApi
{
    public class VkExceptionFilterAttribute : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext context)
        {
            if (context.Exception is VKException)
            {
                var vkException = (VKException)context.Exception;
                HttpStatusCode statusCode;
                switch (vkException.Code)
                {
                    case 15: statusCode = HttpStatusCode.Unauthorized; break;
                    case 18: statusCode = HttpStatusCode.Gone; break;
                    case 19: statusCode = HttpStatusCode.Forbidden; break;
                    default: statusCode = HttpStatusCode.InternalServerError; break;
                }
                context.Response = new HttpResponseMessage(statusCode)
                {
                    Content = new StringContent(context.Exception.Message)
                };
            }
            base.OnException(context);
        }
    }
}

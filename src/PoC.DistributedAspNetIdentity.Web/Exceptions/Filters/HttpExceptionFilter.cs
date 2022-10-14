using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace PoC.DistributedAspNetIdentity.Web.Exceptions.Filters
{
    public class HttpExceptionFilter : IExceptionFilter
    {
        private readonly ILogger<HttpExceptionFilter> _logger;

        public HttpExceptionFilter(ILogger<HttpExceptionFilter> logger)
        {
            _logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            if (context.Exception is ValidationProblemDetailsException validationProblemDetails)
            {
                context.ExceptionHandled = true;
                context.Result = new BadRequestObjectResult(validationProblemDetails.ProblemDetails);
                _logger.LogInformation(context.Exception, "Handled bad request exception");
            }
        }
    }
}

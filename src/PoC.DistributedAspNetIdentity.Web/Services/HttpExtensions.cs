using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace PoC.DistributedAspNetIdentity.Web.Services
{
    public static class HttpExtensions
    {
        public static async Task CheckErrorResponse(this HttpResponseMessage response, CancellationToken cancellationToken)
        {
            if (!response.IsSuccessStatusCode)
            {
                if (response.StatusCode == HttpStatusCode.BadRequest)
                {
                    var problemDetails = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>(cancellationToken: cancellationToken);
                    throw new ValidationProblemDetailsException(problemDetails);
                }
                throw new UnknownHttpException(response);
            }
        }
    }
}

using BuberDinner.Application.Common.Errors;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace BuberDinner.Api.Controllers
{
    public class ErrorsController : ControllerBase
    {
        [Route("/error")]
        public IActionResult Error()
        {
            Exception? exception = HttpContext.Features.Get<IExceptionHandlerFeature>().Error;

            // 透過 Switch 取得相應的 statusCode, message
            var (statusCode, message) = exception switch
            {
                IServiceException serviceException => ((int)serviceException.HttpStatusCode, serviceException.ErrorMessage),
                _ => (StatusCodes.Status500InternalServerError, "An unexpected error occurred.")
            };

            // 回傳 ProblemDetail 並傳入自己要設定的值 (沒設定就會自己抓)
            return Problem(title: message, statusCode: statusCode);
        }
    }
}

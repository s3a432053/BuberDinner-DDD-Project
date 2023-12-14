using BuberDinner.Api.Common.Http;
using ErrorOr;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace BuberDinner.Api.Controllers
{
    [ApiController]
    // Request 需要通過 Authorize 驗證才可以進入有 Authorize Attribute 的端點
    [Authorize]
    public class ApiController : ControllerBase
    {
        protected IActionResult Problem(List<Error> errors)
        {
            // 無錯誤 => 回傳 空 Problem
            if (!errors.Any())
            {
                return Problem();
            }

            // Validation Type 的錯誤 => 回傳 ValidationProblem
            if (errors.All(error => error.Type == ErrorType.Validation))
            {
                return ValidationProblem(errors);
            }

            // 在 Items 中新增一個 Errors 屬性 並且將 List<Error> 存入
            HttpContext.Items[HttpContextItemKeys.Errors] = errors;

            // 回傳第一個 Error 的 Problem
            return Problem(errors[0]);
        }

        private IActionResult Problem(Error error)
        {
            // 根據 ErrorOr 套件提供的 Type 決定要選擇哪一個 StatusCodes
            var statusCode = error.Type switch
            {
                ErrorType.Conflict => StatusCodes.Status409Conflict,
                ErrorType.Validation => StatusCodes.Status400BadRequest,
                ErrorType.NotFound => StatusCodes.Status404NotFound,
                _ => StatusCodes.Status500InternalServerError,
            };

            // 呼叫 ControllerBase 的 Problem 方法並回傳
            return Problem(statusCode: statusCode, title: error.Description);
        }

        private IActionResult ValidationProblem(List<Error> errors)
        {
            var modelStateDictionary = new ModelStateDictionary();

            // 迭代寫入 ModelStateDictionary
            foreach (var error in errors)
            {
                modelStateDictionary.AddModelError(
                    error.Code,
                    error.Description
                    );
            }

            // Validation 類型的錯誤 => 改回傳 ValidationProblem
            return ValidationProblem(modelStateDictionary);
        }
    }
}

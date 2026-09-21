using BuildingBlocks.Common;
using BuildingBlocks.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ProductService.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public abstract class ApiController : ControllerBase
    {
        private ISender? _sender;
        protected ISender Sender => _sender ??= HttpContext.RequestServices.GetRequiredService<ISender>();

        [NonAction]
        public IActionResult HandleFailure(Result result)
        {
            if (result.IsSuccess)
            {
                throw new InvalidOperationException("Success results should not call HandleFailure.");
            }

            int statusCode = result.Error.Type switch
            {
                ErrorType.Validation => StatusCodes.Status400BadRequest,
                ErrorType.Unauthorized => StatusCodes.Status401Unauthorized,
                ErrorType.Forbidden => StatusCodes.Status403Forbidden,
                ErrorType.NotFound => StatusCodes.Status404NotFound,
                ErrorType.Conflict => StatusCodes.Status409Conflict,
                ErrorType.Unexpected => StatusCodes.Status500InternalServerError,
                _ => StatusCodes.Status400BadRequest
            };

            return result switch
            {
                IValidationResult validationResult =>
                    BadRequest(
                        CreateProblemDetails(
                            "Validation Error",
                            StatusCodes.Status400BadRequest,
                            result.Error,
                            validationResult.Errors)),

                _ => StatusCode(statusCode,
                        CreateProblemDetails(
                            GetTitleForStatus(statusCode),
                            statusCode,
                            result.Error))
            };
        }

        private static string GetTitleForStatus(int statusCode) =>
            statusCode switch
            {
                StatusCodes.Status401Unauthorized => "Unauthorized Access",
                StatusCodes.Status403Forbidden => "Forbidden Access",
                StatusCodes.Status404NotFound => "Resource Not Found",
                StatusCodes.Status409Conflict => "Conflict / Duplicate",
                StatusCodes.Status500InternalServerError => "Internal Server Error",
                _ => "Bad Request"
            };

        private static ProblemDetails CreateProblemDetails(
            string title,
            int status,
            Error error,
            Error[]? errors = null) =>
            new()
            {
                Title = title,
                Type = error.Code,
                Detail = error.Message,
                Status = status,
                Extensions = { { "errors", errors } }
            };
    }
}

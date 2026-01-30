using ErrorOr;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace AccommodationBooking.Api.Controllers;

/// <summary>
/// Base controller providing common error handling functionality.
/// </summary>
[ApiController]
[Authorize]
public class ApiController : ControllerBase
{
    /// <summary>
    /// Converts a list of errors to an appropriate problem response.
    /// </summary>
    protected IActionResult Problem(List<Error> errors)
    {
        var count = errors.Count;

        if (count == 0)
            return Problem();

        if (count > 1 && errors.All(error => error.Type == ErrorType.Validation))
            return ValidationProblem(errors);

        return Problem(errors[0]);
    }

    private IActionResult Problem(Error error)
    {
        var statusCode = error.Type switch
        {
            ErrorType.Conflict => StatusCodes.Status409Conflict,
            ErrorType.Validation => StatusCodes.Status400BadRequest,
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            ErrorType.Unauthorized => StatusCodes.Status401Unauthorized,
            ErrorType.Forbidden => StatusCodes.Status403Forbidden,
            _ => StatusCodes.Status500InternalServerError,
        };

        return Problem(statusCode: statusCode, title: error.Description);
    }

    private IActionResult ValidationProblem(List<Error> errors)
    {
        var modelStateDictionary = new ModelStateDictionary();

        foreach (var error in errors)
        {
            modelStateDictionary.AddModelError(
                error.Code,
                error.Description);
        }

        return Problem(errors.FirstOrDefault());
    }
}

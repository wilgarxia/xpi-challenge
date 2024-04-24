using ErrorOr;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

using PortfolioManager.Api.Common;

namespace PortfolioManager.Api.Controllers;

public class ApiController(IHttpContextAccessor contextAccessor) : ControllerBase
{
    protected readonly IHttpContextAccessor _context = contextAccessor;

    protected IActionResult Problem(List<Error> errors)
    {
        if (errors.Count is 0)
            return Problem();

        return errors.All(error => error.Type == ErrorType.Validation) 
            ? ValidationProblem(errors) 
            : ControllerHelper.Problem(errors[0]);
    }

    private ActionResult ValidationProblem(List<Error> errors)
    {
        ModelStateDictionary modelStateDictionary = new();

        errors.ForEach(error => modelStateDictionary.AddModelError(
            error.Code, error.Description));

        return ValidationProblem(modelStateDictionary);
    }
}
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Nicknames.Server.Controllers;

namespace Nicknames.Server.Filters;

public class ModelStateValidationFilter : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        if (context.Controller.GetType() == typeof(AuthenticationController))
        {
            return;
        }
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        if (!context.ModelState.IsValid)
        {
            // Iterate through each entry in ModelState
            foreach (var entry in context.ModelState)
            {
                var fieldName = entry.Key; // The name of the field that failed validation
                var fieldErrors = entry.Value.Errors; // List of error messages for this field

                foreach (var error in fieldErrors)
                {
                    var errorMessage = error.ErrorMessage; // The specific error message
                    Console.WriteLine($"Field: {fieldName}, Error: {errorMessage}");
                }
            }

            var validationFailedResponse = new ValidationFailedResponse(context.ModelState);
            context.Result = new BadRequestObjectResult(validationFailedResponse);
        }
    }
}

public class ValidationFailedResponse
{
    public IDictionary<string, string[]> Errors { get; }

    public ValidationFailedResponse(ModelStateDictionary modelState)
    {
        Errors = modelState.ToDictionary(
            kvp => kvp.Key,
            kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
        );
    }
}

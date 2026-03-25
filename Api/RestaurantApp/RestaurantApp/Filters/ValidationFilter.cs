using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using RestaurantApp.Exceptions;

namespace RestaurantApp.Filters
{
    public class ValidationFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var errors = context.ModelState
                    .Where(ms => ms.Value?.Errors?.Count > 0)
                    .SelectMany(kvp => kvp.Value!.Errors.Select(e => e.ErrorMessage))
                    .ToList();

                throw new ValidationException(errors);
            }
        }

        public void OnActionExecuted(ActionExecutedContext context) { }
    }
}

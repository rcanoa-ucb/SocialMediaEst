using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SocialMedia.Infrastructure.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMedia.Infrastructure.Filters
{
    public class ValidationFilter : IAsyncActionFilter
    {
        private readonly IValidationService _validationService;

        public ValidationFilter(IValidationService validationService)
        {
            _validationService = validationService;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            // Verificar si hay algún parámetro que necesite validación
            var arguments = context.ActionArguments
                .Where(x => x.Value?.GetType().Namespace?.StartsWith("SocialMedia.Infrastructure") == true)
                .ToList();

            foreach (var argument in arguments)
            {
                var validationResult = await _validationService.ValidateAsync(argument.Value);

                if (!validationResult.IsValid)
                {
                    context.Result = new BadRequestObjectResult(new
                    {
                        Message = "Error de validación",
                        Errors = validationResult.Errors
                    });
                    return;
                }
            }

            await next();
        }
    }
}

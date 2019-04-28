using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Host.Extensions
{
    public static class ModelStateDictionaryExtensioins
    {
        public static ValidationProblemDetails GetValidationProblemDetails(this ModelStateDictionary modelState)
        {
            return new ValidationProblemDetails(modelState);
        }
    }
}

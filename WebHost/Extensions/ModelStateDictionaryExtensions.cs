using Host.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Host.Extensions
{
    public static class ModelStateDictionaryExtensions
    {
        public static object GetErrorObject(this ModelStateDictionary modelState)
        {
            return new { Errors = modelState.Select(x => new FieldErrors(x)) };
        }
    }
}

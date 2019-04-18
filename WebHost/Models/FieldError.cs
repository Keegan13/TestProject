using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Host.Models
{
    public class FieldErrors
    {
        public string Field;
        public IEnumerable<string> Messages;
        public FieldErrors(KeyValuePair<string,ModelStateEntry> field)
        {
            this.Field = field.Key;
            this.Messages = field.Value.Errors.Select(x=>x.ErrorMessage);
        }
    }
}

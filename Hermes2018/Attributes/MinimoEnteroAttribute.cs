using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Hermes2018.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class MinimoEnteroAttribute : ValidationAttribute, IClientModelValidator
    {
        private readonly int _minimo;

        public MinimoEnteroAttribute(int minimo)
        {
            _minimo = minimo;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is int valorActual && valorActual < _minimo)
            {
                return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
            }

            return ValidationResult.Success;
        }

        public void AddValidation(ClientModelValidationContext context)
        {
            MergeAttribute(context.Attributes, "data-val", "true");
            MergeAttribute(context.Attributes, "data-val-minimoentero", string.Format(this.ErrorMessageString, context.ModelMetadata.GetDisplayName(), _minimo));
            MergeAttribute(context.Attributes, "data-val-minimoentero-valor", _minimo.ToString());
        }

        private bool MergeAttribute(IDictionary<string, string> attributes, string key, string value)
        {
            if (attributes.ContainsKey(key))
            {
                return false;
            }
            attributes.Add(key, value);
            return true;
        }
    }
}

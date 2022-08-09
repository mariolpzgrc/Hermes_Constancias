using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Hermes2018.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class MaximoEnteroAttribute : ValidationAttribute, IClientModelValidator
    {
        private readonly int _maximo;

        public MaximoEnteroAttribute(int maximo)
        {
            _maximo = maximo;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var valorActual = Convert.ToInt32(value);
            if (valorActual <= 0 || valorActual > _maximo)
            {
                return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
            }

            return ValidationResult.Success;
        }

        public void AddValidation(ClientModelValidationContext context)
        {
            MergeAttribute(context.Attributes, "data-val", "true");
            MergeAttribute(context.Attributes, "data-val-maximoentero", string.Format(this.ErrorMessageString, context.ModelMetadata.GetDisplayName(), _maximo));
            MergeAttribute(context.Attributes, "data-val-maximoentero-valor", _maximo.ToString());
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

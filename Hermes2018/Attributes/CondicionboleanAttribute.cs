using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Hermes2018.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class CondicionboleanAttribute : ValidationAttribute, IClientModelValidator
    {
        private string _dependencia;

        public CondicionboleanAttribute (string dependencia)
        {
            _dependencia = dependencia;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var contenedorContexto = validationContext.ObjectInstance.GetType();
            var campoDependiente = contenedorContexto.GetProperty(_dependencia);

            if (campoDependiente != null)
            {
                var valorDependiente = campoDependiente.GetValue(validationContext.ObjectInstance, null);

                if (Convert.ToBoolean(valorDependiente))
                {
                    if (string.IsNullOrEmpty(Convert.ToString(value)))
                    {
                        return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
                    }
                }
            }

            return ValidationResult.Success;
        }

        public void AddValidation(ClientModelValidationContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            MergeAttribute(context.Attributes, "data-val", "true");
            MergeAttribute(context.Attributes, "data-val-condicionbolean", FormatErrorMessage(context.ModelMetadata.GetDisplayName()));
            MergeAttribute(context.Attributes, "data-val-condicionbolean-dependencia", _dependencia);
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

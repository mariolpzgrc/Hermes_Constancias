using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Hermes2018.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class CondicionAttribute : ValidationAttribute, IClientModelValidator
    {
        private string _dependencia;
        private string _referencia;

        public CondicionAttribute(string dependencia, string referencia)
        {
            _dependencia = dependencia;
            _referencia = referencia;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var contenedorContexto = validationContext.ObjectInstance.GetType();
            var campoDependiente = contenedorContexto.GetProperty(_dependencia);
            var campoReferencia = contenedorContexto.GetProperty(_referencia);

            if (campoDependiente != null)
            {
                var valorDependiente = campoDependiente.GetValue(validationContext.ObjectInstance, null);
                var valorReferencia = campoReferencia.GetValue(validationContext.ObjectInstance, null);

                if (!Convert.ToBoolean(valorDependiente))
                {
                    if (string.IsNullOrEmpty(Convert.ToString(value)) || Convert.ToString(valorReferencia) == Convert.ToString(value))
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
            MergeAttribute(context.Attributes, "data-val-condicion", FormatErrorMessage(context.ModelMetadata.GetDisplayName()));
            MergeAttribute(context.Attributes, "data-val-condicion-dependencia", _dependencia);
            MergeAttribute(context.Attributes, "data-val-condicion-referencia", _referencia);
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

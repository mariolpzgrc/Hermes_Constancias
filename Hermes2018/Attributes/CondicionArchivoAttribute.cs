using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Hermes2018.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class CondicionArchivoAttribute : ValidationAttribute, IClientModelValidator
    {
        private string _dependencia;
        private IEnumerable<string> _tiposValidos;

        public CondicionArchivoAttribute(string dependencia, string tiposValidos)
        {
            _dependencia = dependencia;
            _tiposValidos = tiposValidos.Split(',').Select(s => s.Trim().ToLower());
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            IFormFile file = value as IFormFile;
            //--
            var contenedorContexto = validationContext.ObjectInstance.GetType();
            var campoDependiente = contenedorContexto.GetProperty(_dependencia);

            if (campoDependiente != null)
            {
                var valorDependiente = campoDependiente.GetValue(validationContext.ObjectInstance, null);

                if (Convert.ToBoolean(valorDependiente))
                {
                    if (file == null)
                    {
                        return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
                    }
                    else {
                        if (file != null && !_tiposValidos.Any(e => file.FileName.EndsWith(e)))
                        {
                            return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
                        }
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
            MergeAttribute(context.Attributes, "data-val-condicionarchivo", FormatErrorMessage(context.ModelMetadata.GetDisplayName()));
            MergeAttribute(context.Attributes, "data-val-condicionarchivo-dependencia", _dependencia);
            MergeAttribute(context.Attributes, "data-val-condicionarchivo-tipos", string.Join(",", _tiposValidos));
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

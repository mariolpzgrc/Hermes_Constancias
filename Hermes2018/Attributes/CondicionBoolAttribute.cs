using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Hermes2018.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class CondicionanteboolAttribute : ValidationAttribute, IClientModelValidator
    {
        private string _enbase;
        private string _dependencia;
        private string _actual;

        public CondicionanteboolAttribute(string enbase,string dependencia, string actual)
        {
            _enbase = enbase;
            _dependencia = dependencia;
            _actual = actual;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var contenedorContexto = validationContext.ObjectInstance.GetType();
            var campoEnbase = contenedorContexto.GetProperty(_enbase);
            var campoDependiente = contenedorContexto.GetProperty(_dependencia);

            if (campoEnbase != null) {
                if (campoDependiente != null)
                {
                    var valorEnbase = campoEnbase.GetValue(validationContext.ObjectInstance, null);
                    var valorDependiente = campoDependiente.GetValue(validationContext.ObjectInstance, null);

                    if (!Convert.ToBoolean(valorEnbase))
                    {
                        if (Convert.ToBoolean(valorDependiente))
                        {
                            if (!Convert.ToBoolean(value))
                            {
                                return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
                            }
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
            MergeAttribute(context.Attributes, "data-val-condicionantebool", FormatErrorMessage(context.ModelMetadata.GetDisplayName()));
            MergeAttribute(context.Attributes, "data-val-condicionantebool-enbase", _enbase);
            MergeAttribute(context.Attributes, "data-val-condicionantebool-dependencia", _dependencia);
            MergeAttribute(context.Attributes, "data-val-condicionantebool-actual", _actual);
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

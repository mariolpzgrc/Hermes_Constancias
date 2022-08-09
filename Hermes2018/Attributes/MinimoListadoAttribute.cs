using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Hermes2018.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class MinimoListadoAttribute : ValidationAttribute
    {
        private string _referencia;
        public MinimoListadoAttribute(string referencia)
        {
            _referencia = referencia;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var contenedorContexto = validationContext.ObjectInstance.GetType();
            var campoReferencia = contenedorContexto.GetProperty(_referencia);
            var valorReferencia = campoReferencia.GetValue(validationContext.ObjectInstance, null);
            var total = Convert.ToInt32(valorReferencia);

            if (value is IList list && list.Count != total)
            {
                return new ValidationResult(string.Format(this.ErrorMessageString, validationContext.DisplayName));
            }

            return ValidationResult.Success;
        }
    }
}

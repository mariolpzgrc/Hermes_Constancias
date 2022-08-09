using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace Hermes2018.ViewModels
{
    public class DetectarAreaViewModelDBF
    {
        [Display(Name = "Nombre")]
        public string Nombre  { get; set; }

        [Display(Name = "Clave")]
        public string Clave { get; set; }

        [Display(Name = "Estado")]
        public string Estado { get; set; }

        [HiddenInput]
        public int EstadoId { get; set; }
    }
}

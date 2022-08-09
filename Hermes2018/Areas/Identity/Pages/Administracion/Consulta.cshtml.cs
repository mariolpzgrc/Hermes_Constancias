using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hermes2018.Helpers;
using Hermes2018.Services;
using Hermes2018.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Hermes2018.Areas.Identity.Pages.Administracion
{
    [Authorize]
    public class ConsultaModel : PageModel
    {
        private readonly IOracleService _oracleService;

        public ConsultaModel(IOracleService oracleService)
        {
            _oracleService = oracleService;
        }

        public string Usuario { get; set; }
        public BaseUsuarioOracleViewModel Listado { get; set; }
        
        public void OnGet(string id = "alereyes")
        {
            Usuario = id;
            Listado = _oracleService.ObtieneUsuariosOracleAsync(id);
        }
    }
}

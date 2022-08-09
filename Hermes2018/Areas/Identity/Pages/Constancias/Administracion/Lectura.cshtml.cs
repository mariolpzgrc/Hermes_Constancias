using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Hermes2018.Services;
using Hermes2018.Services.Interfaces;
using Hermes2018.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace Hermes2018.Areas.Identity.Pages.Constancias
{
    [Authorize]
    public class LecturaModel : PageModel
    {
        private readonly IUsuarioService _usuarioService;
        private readonly IUsuarioClaimService _usuarioClaimService;
        private readonly IDescargarConstanciaPDFService _descargarConstanciaPDFService;
        private readonly IConstanciaService _constanciaService;
        private readonly CultureInfo _cultureEs;

        public LecturaModel(
            IUsuarioService usuarioService,
            IUsuarioClaimService usuarioClaimService,
            IDescargarConstanciaPDFService descargarConstanciaPDFService,
            IConstanciaService constanciaService
        )
        {
            _usuarioService = usuarioService;
            _usuarioClaimService = usuarioClaimService;
            _descargarConstanciaPDFService = descargarConstanciaPDFService;
            _constanciaService = constanciaService;            
            _cultureEs = new CultureInfo("es-MX");
        }
        [BindProperty]
        public int IdConstanciaSolicitada { get; set; }
        [BindProperty]
        public int IdConstancia { get; set; }

        [BindProperty] 
        public int TipoPersonal { get; set; }

        [BindProperty]
        public ConstanciaViewModel ConstanciaModel { get; set; }
        public void OnGet(int idConstanciaSolicitada, int idConstancia)
        {
            IdConstanciaSolicitada = idConstanciaSolicitada;
            IdConstancia = idConstancia;


            /*string resultado = _constanciaService.ObtieneServMed(34004, 3);
            List<ConstanciaViewModel> data = new List<ConstanciaViewModel>();
            if (resultado != "")
                JsonConvert.DeserializeObject<List<ConstanciaViewModel>>(resultado);*/

            //Pongo el modelo ya llenado

        }

        public async Task<FileResult> OnPostBaseAsync()
        {
            var nombreConstancia = "";

            switch (IdConstancia)
            {
                case 1:
                    nombreConstancia = "Constancia de Servicio médico";
                    break;
                case 2:
                    nombreConstancia = "Constancia de Servicio médico con dependiente económico";
                    break;
                case 3:
                    nombreConstancia = "Constancia de Trabajo y percepciones";
                    break;
                case 4:
                    nombreConstancia = "Constancia de Horario laboral";
                    break;
                case 5:
                    nombreConstancia = "Constancia de Afiliación al IPE";
                    break;
                case 6:
                    nombreConstancia = "Constancia de Afiliación al Magisterio";
                    break;
                case 7:
                    nombreConstancia = "Constancia de Oficio baja del IPE";
                    break;
                case 8:
                    nombreConstancia = "Constancia de Oficio baja del Magisterio";
                    break;
                case 9:
                    nombreConstancia = "Constancia para VISA";
                    break;
                case 10:
                    nombreConstancia = "Constancia de VISA con dependiente económico";
                    break;
                case 11:
                    nombreConstancia = "Constancia PRODEP ";
                    break;
                case 12:
                    nombreConstancia = "Constancia Curricular (periodos laborados) ";
                    break;
                case 13:
                    nombreConstancia = "Hoja de Servicios";
                    break;
                case 14:
                    nombreConstancia = "Constancia de Jubilación";
                    break;
            }

            if (TipoPersonal != 4 && (IdConstancia == 7 || IdConstancia == 8))
            {
                return File(_descargarConstanciaPDFService.DescargarConstanciaNoDocente(ConstanciaModel, IdConstancia, nombreConstancia, TipoPersonal), System.Net.Mime.MediaTypeNames.Application.Pdf, string.Format("{0}.pdf", nombreConstancia));
            }
            else if(TipoPersonal == 4 && (IdConstancia == 7 || IdConstancia == 8)){
                return File(_descargarConstanciaPDFService.DescargarConstanciaDocente(ConstanciaModel, IdConstancia, nombreConstancia, TipoPersonal), System.Net.Mime.MediaTypeNames.Application.Pdf, string.Format("{0}.pdf", nombreConstancia));
            }
            else
            {
                return File(_descargarConstanciaPDFService.DescargarOficioBajaIPEMagisterio(), System.Net.Mime.MediaTypeNames.Application.Pdf, string.Format("{0}.pdf", nombreConstancia));
            }
        }     
    }
}

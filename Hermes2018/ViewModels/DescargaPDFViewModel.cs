using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hermes2018.ViewModels
{
    public class DescargaPDFViewModel
    {
        [BindProperty]
        [HiddenInput]
        public string Folio { get; set; }

        [BindProperty]
        [HiddenInput]
        public string AreaSuperior { get; set; }

        [BindProperty]
        [HiddenInput]
        public string Area { get; set; }

        [BindProperty]
        [HiddenInput]
        public string Region { get; set; }

        [BindProperty]
        [HiddenInput]
        public string Asunto { get; set; }
        [BindProperty]
        [HiddenInput]
        public string NumeroInterno { get; set; }

        [BindProperty]
        [HiddenInput]
        public string Fecha { get; set; }

        [BindProperty]
        [HiddenInput]
        public string ParaNombre { get; set; }

        [BindProperty]
        [HiddenInput]
        public string ParaNombreEnviado { get; set; }

        [BindProperty]
        [HiddenInput]
        public string ParaPuesto { get; set; }

        [BindProperty]
        [HiddenInput]
        public string ParaArea { get; set; }

        [BindProperty]
        [HiddenInput]
        public string DeDireccion { get; set; }

        [BindProperty]
        [HiddenInput]
        public string DeTelefono { get; set; }

        [BindProperty]
        [HiddenInput]
        public string DeCorreo { get; set; }

        [BindProperty]
        [HiddenInput]
        public string Cuerpo { get; set; }

        [BindProperty]
        [HiddenInput]
        public string DeNombre { get; set; }

        [BindProperty]
        [HiddenInput]
        public string DePuesto { get; set; }

        [BindProperty]
        [HiddenInput]
        public string DeArea { get; set; }

        [BindProperty]
        [HiddenInput]
        public List<string> CCP { get; set; }

        [BindProperty]
        [HiddenInput]
        public string LeyendaRecibido { get; set; }

        [BindProperty]
        [HiddenInput]
        public string CodigoQR { get; set; }

        [BindProperty]
        [HiddenInput]
        public int TipoUsuario { get; set; }

        [BindProperty]
        [HiddenInput]
        public string UsuariosParaCC { get; set; }

        //Traer informacion del  envio actual
        [BindProperty]
        [HiddenInput]
        public string UsuariosDeActual { get; set; }

        [BindProperty]
        [HiddenInput]
        public string UsuariosParaActual { get; set; }
        [BindProperty]
        [HiddenInput]
        public string UsuariosParaCCActual { get; set; }

        [BindProperty]
        [HiddenInput]
        public string Indicaciones { get; set; }

        [BindProperty]
        [HiddenInput]
        public string FechaCompromiso { get; set; }

        [BindProperty]
        [HiddenInput]
        public string FechaRecepcion { get; set; }

    }
}

using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hermes2018.ViewModels
{
    public class EncabezadoSeguimientoViewModel
    {
        [HiddenInput]
        public string Usuario { get; set; }
        //--
        public string Folio { get; set; }
        public string Asunto { get; set; }
        public string FechaEnvio { get; set; }
        public string FechaPropuesta { get; set; }
        public string FechaCompromiso { get; set; }

        public string De { get; set; }
        public string Para { get; set; }
        public string CCP { get; set; }

        [HiddenInput]
        public bool TieneAdjuntos { get; set; }
        [HiddenInput]
        public int TipoEnvioId { get; set; }
        public string Indicaciones { get; set; }

        [HiddenInput]
        public int EnvioId { get; set; }
        [HiddenInput]
        public int DocumentoId { get; set; }
        [HiddenInput]
        public int RecepcionId { get; set; }

        //Datos
        public string NoInterno { get; set; }
        public string Cuerpo { get; set; }

        //Visualización
        public int Visualizacion_Tipo { get; set; }
    }
    public class SeguimientoSolicitudJsonModel
    {
        public int EnvioId { get; set; }
        public int DocumentoId { get; set; }
        public string Folio { get; set; }
        public int TipoEnvioId { get; set; }
        public int RecepcionId { get; set; }
        public string Usuario { get; set; }
    }
    public class SeguimientoEnvioJsonModel
    {
        public string UsuarioOrigen { get; set; }
        public int? EnvioPadre { get; set; }
        public int EnvioId { get; set; }
        public int TipoEnvio { get; set; }
        public int DocumentoId { get; set; }
        
        public string Fecha { get; set; }
        public string Hora { get; set; }
        public string De { get; set; }
        public string UsuarioDe { get; set; }
        public int Estado { get; set; }
        public bool EsPublico { get; set; }
        public bool Actual { get; set; }

        public bool EsReenvio { get; set; }
        //Esta variable es suplente para la vizaualizacion del archivo dado  que se cambio el siguiemiento de los oficios.
        public int VisualizacionTipoEnvio { get; set; }

        public List<SeguimientoRecepcionJsonModel> Destinatarios { get; set; }
    }

    public class SeguimientoEnvioViewModel
    {
        public DateTime Fecha { get; set; }
        public string Usuario { get; set; }
        public int EnvioId { get; set; }
        public int TipoEnvioId { get; set; }
    }

    public class SeguimientoRecepcionJsonModel
    {
        public int RecepcionId { get; set; }
        public string Fecha { get; set; }
        public string Hora { get; set; }
        public string Para { get; set; }
        public string UsuarioPara { get; set; }
        public int TipoPara { get; set; }
        public int Estado { get; set; }
        public bool TieneRespuesta { get; set; }
       
    }
    public class SeguimientoEnvioRespuestaJsonModel
    {
        public int EnvioId { get; set; }
        public int DocumentoId { get; set; }
        public string Fecha { get; set; }
        public string De { get; set; }
        public bool Actual { get; set; }

        public List<SeguimientoRecepcionRespuestaJsonModel> Destinatarios  { get; set; }
    }
    public class SeguimientoRecepcionRespuestaJsonModel
    {
        public int RecepcionId { get; set; }
        public string Fecha { get; set; }
        public string Para { get; set; }
        public int TipoPara { get; set; }
    }
}

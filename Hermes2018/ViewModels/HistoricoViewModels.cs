using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hermes2018.ViewModels
{
    public class HistoricoPersonaViewModel
    {
        public int InfoUsuarioId { get; set; }
        public string UserName { get; set; }
        public string NombreCompleto { get; set; }
        public string FechaRegistro { get; set; }
        public string FechaActualizacion { get; set; }
        public string Region { get; set; }
        public string Area { get; set; }
        public string Puesto { get; set; }
    }
    public class HistoricoAreaViewModel
    {
        public int InfoUsuarioId { get; set; }
        public string UserName { get; set; }
        public string NombreCompleto { get; set; }
        public string FechaRegistro { get; set; }
        public string Puesto { get; set; }
    }
    public class BandejasViewModel
    {
        public int Recibidos { get; set; }
        //public int Enviados { get; set; }
        //public int Borradores { get; set; }
        public int Revision { get; set; }
    }
    public class CarpetasViewModel
    {
        public string Nombre { get; set; }
        public int ElementosSinLeer { get; set; }
        public string Fecha { get; set; }
    }
    public class HistoricoCorrespondenciaViewModel
    {
        public List<DocumentoRecibidoViewModel> Recibe { get; set; }
        public List<DocumentoEnviadoViewModel> Envia { get; set; }
        public List<DocumentoBorradorViewModel> Borrador { get; set; }
        public List<DocumentoRevisionViewModel> Revision { get; set; }
    }
    public class HistoricoDocumentoLecturaViewModel
    {
        // Origen
        public string Origen_Asunto { get; set; }
        public string Origen_Folio { get; set; }
        public string Origen_NoInterno { get; set; }
        public string Origen_Fecha { get; set; }
        public string Origen_Cuerpo { get; set; }
        //--
        public string Origen_Importancia { get; set; }
        public string Origen_Visibilidad { get; set; }
        public int Origen_TipoDocumentoId { get; set; }
        public string Origen_TipoDocumento { get; set; }
        public bool Origen_RequiereRespuesta { get; set; }
        public bool Origen_ExisteAdjuntos { get; set; }
        public List<string> Origen_ListadoCcp { get; set; }

        //Usuario De Origen
        public string Origen_UsuarioDe_Correo { get; set; }
        public string Origen_UsuarioDe_NombreCompleto { get; set; }
        public string Origen_UsuarioDe_Direccion { get; set; }
        public string Origen_UsuarioDe_Telefono { get; set; }
        public string Origen_UsuarioDe_AreaPadreNombre { get; set; }
        public string Origen_UsuarioDe_AreaNombre { get; set; }
        public int Origen_UsuarioDe_AreaId { get; set; }
        public string Origen_UsuarioDe_Region { get; set; }
        public string Origen_UsuarioDe_PuestoNombre { get; set; }

        //Usuario Para Origen
        public string Origen_UsuarioPara_NombreCompleto { get; set; }
        public string Origen_UsuarioPara_AreaNombre { get; set; }
        public string Origen_UsuarioPara_PuestoNombre { get; set; }
        public int Origen_UsuarioPara_Tipo { get; set; }

        //Nombre de quien lo ha creado/ ya sea el titular o el delegado
        public string Origen_NombreCreador { get; set; }
        public string Origen_UsuarioCreador { get; set; }
        public string Origen_NombreTitular { get; set; }
        public string Origen_UsuarioTitular { get; set; }

        //Envio Origen
        public int Origen_EnvioId { get; set; }
        public int Origen_TipoEnvioId { get; set; }
        public string Origen_TipoEnvio { get; set; }

        //Actual
        public int Actual_EnvioId { get; set; }
        [HiddenInput]
        public int Actual_RecepcionId { get; set; }
        public string Actual_AsuntoEnvio { get; set; }
        public string Actual_Importancia { get; set; }
        public string Actual_Visibilidad { get; set; }
        public bool Actual_RequiereRespuesta { get; set; }
        public string Actual_Fecha { get; set; } /*- Fecha de envio/recepción -*/
        public string Actual_FechaPropuesta { get; set; }
        public string Actual_FechaCompromiso { get; set; }
        public bool Actual_EstaLeido { get; set; }
        public int Actual_TipoEnvioId { get; set; }
        public string Actual_TipoEnvio { get; set; }
        public bool Actual_EsReenvio { get; set; }

        //Usuario De
        public string Actual_UsuarioDe_Correo { get; set; }
        public string Actual_UsuarioDe_NombreCompleto { get; set; }
        public string Actual_UsuarioDe_NombreUsuario { get; set; }

        //Usuario Envia Para
        public string Actual_UsuariosPara { get; set; }
        public string Actual_UsuariosCCP { get; set; }

        //Visualización
        public int Actual_Visualizacion_Tipo { get; set; }

        //Turnado
        public bool Actual_EsTurnado { get; set; }
        public string Actual_Indicaciones { get; set; }
        public bool Actual_ExisteAdjuntos { get; set; }
       
        //Respuesta
        public bool Actual_TieneRespuesta { get; set; }

        //Estado Envio-Recepcion   
        public int Actual_EstadoId { get; set; }
        public string Actual_Estado { get; set; }

        //Tipo de usuario que lee (Para o CCP)
        public int Actual_UsuarioLee_Tipo { get; set; }

        //Carpeta/subcarpeta
        public int? Actual_CarpetaId { get; set; }
        public string Actual_NombreCarpeta { get; set; }
        public string Actual_NombreCarpetaPadre { get; set; }
    }
    public class HistoricoDocumentoVisualizacionViewModel
    {
        //Documento Origen
        public int Origen_EnvioId { get; set; }
        public string Origen_Asunto { get; set; }
        public string Origen_Folio { get; set; }
        public string Origen_NoInterno { get; set; }
        public string Origen_Fecha { get; set; }
        public string Origen_Cuerpo { get; set; }
        //--
        public string Origen_Importancia { get; set; }
        public string Origen_Visibilidad { get; set; }
        public string Origen_TipoDocumento { get; set; }
        public int Origen_TipoDocumentoId { get; set; }
        //--
        public int Origen_TipoEnvioId { get; set; }
        public string Origen_TipoEnvio { get; set; }
        //--
        public bool Origen_RequiereRespuesta { get; set; }
        public bool Origen_ExisteAdjuntos { get; set; }
        public List<string> Origen_ListadoCcp { get; set; }

        //Usuario De Origen
        public string Origen_UsuarioDe_NombreCompleto { get; set; }
        public string Origen_UsuarioDe_Correo { get; set; }
        public string Origen_UsuarioDe_Direccion { get; set; }
        public string Origen_UsuarioDe_Telefono { get; set; }

        public string Origen_UsuarioDe_AreaPadreNombre { get; set; }
        public string Origen_UsuarioDe_AreaNombre { get; set; }
        public int Origen_UsuarioDe_AreaId { get; set; }
        public string Origen_UsuarioDe_Region { get; set; }
        public string Origen_UsuarioDe_PuestoNombre { get; set; }

        //Usuario Para Origen
        public string Origen_UsuarioPara_NombreCompleto { get; set; }
        public string Origen_UsuarioPara_AreaNombre { get; set; }
        public string Origen_UsuarioPara_PuestoNombre { get; set; }

        //Nombre de quien lo ha creado/ ya sea el titular o el delegado
        public string Origen_NombreCreador { get; set; }
        public string Origen_UsuarioCreador { get; set; }
        public string Origen_NombreTitular { get; set; }
        public string Origen_UsuarioTitular { get; set; }

        //Actual 
        public int Actual_EnvioId { get; set; }
        public string Actual_AsuntoEnvio { get; set; }
        public string Actual_Importancia { get; set; }
        public string Actual_Visibilidad { get; set; }
        public bool Actual_RequiereRespuesta { get; set; }
        public string Actual_Fecha { get; set; }
        public string Actual_FechaCompromiso { get; set; }
        public string Actual_FechaPropuesta { get; set; }
        public int Actual_TipoEnvioId { get; set; }
        public bool Actual_EsReenvio { get; set; }
        public string Actual_TipoEnvio { get; set; }
        //--
        public bool Actual_ExisteAdjuntos { get; set; }
        public bool Actual_EsTurnado { get; set; }
        public string Actual_Indicaciones { get; set; }

        //Usuario De
        public string Actual_UsuarioDe_Correo { get; set; }
        public string Actual_UsuarioDe_NombreCompleto { get; set; }
        //Usuarios Para y CCP
        public string Actual_UsuariosPara { get; set; }
        public string Actual_UsuariosCCP { get; set; }

        //Estado Envio-Recepcion   
        public int Actual_EstadoId { get; set; }
        public string Actual_Estado { get; set; }

        //Respuesta
        public bool Actual_TieneRespuesta { get; set; }

        //Visualización
        public int Actual_Visualizacion_Tipo { get; set; }

        //Tipo de usuario que lee (Para o CCP)
        public int Actual_UsuarioLee_Tipo { get; set; }
    }
    public class HistoricoResumenDestinatarioViewModel
    {
        //Usuario
        public string NombreCompleto { get; set; }
        //Recepción
        public string FechaRecepcion { get; set; }
        public string FechaCompromiso { get; set; }
        public int EstadoEnvioId { get; set; }
        public string EstadoEnvio { get; set; }
        public bool EstaLeido { get; set; }
        //--Respuesta
        public bool TieneRespuesta { get; set; }
        public int EnvioRespuestaId { get; set; }
        public int TipoEnvioRespuesta { get; set; }

        public int RecepcionId { get; set; }

        public List<HistoricoFechasCompromisoDestinatariosViewModel> FechasCompromiso { get; set; }
    }
    public class HistoricoFechasCompromisoDestinatariosViewModel
    {
        public int CompromisoId { get; set; }
        public int Estado { get; set; }
        public int Tipo { get; set; }
        public string Fecha { get; set; }

        public string Registro { get; set; }
    }
}

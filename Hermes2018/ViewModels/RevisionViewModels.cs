using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Hermes2018.ViewModels
{
    public class DocumentoEnRevisionViewModel
    {
        [HiddenInput]
        public int DocumentoBaseId { get; set; }

        [HiddenInput]
        public string RemitenteUsuario { get; set; }

        [HiddenInput]
        public string FolioSession { get; set; }

        [HiddenInput]
        public string Extensiones { get; set; }

        [HiddenInput]
        public int TipoVisualizacion { get; set; } //Mostrar el tipo de visualización para emisor y el receptor

        [HiddenInput]
        public int EstadoRemitente { get; set; } //Estado de la revisión

        [HiddenInput]
        public int EstadoDestinatario { get; set; } //Estado de la revisión

        [HiddenInput]
        public string Anexos { get; set; }

        public string Remitente { get; set; }
        public string Destinatario { get; set; }
        public string Categorias { get; set; }
        public string Folio { get; set; }
        public bool RequiereRespuesta { get; set; }
        public string RequiereRespuestaTexto { get; set; }
        public int ImportanciaId { get; set; }
        public string Importancia { get; set; }
        [HiddenInput]
        public int TipoId { get; set; }
        public string Tipo { get; set; }
        public int VisibilidadId { get; set; }
        public string Visibilidad { get; set; }
        public string Fecha { get; set; }
        public string Asunto { get; set; }
        public string Cuerpo { get; set; }
        public string NoInterno { get; set; }
       
        public SelectList PlantillasSelectList { get; set; }
        public SelectList ImportanciaSelectList { get; set; }
        public SelectList TiposSelectList { get; set; }
        public SelectList VisibilidadSelectList { get; set; }
    }
}

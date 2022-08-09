using Hermes2018.Resources;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Hermes2018.ViewModels
{
    public class NuevaCategoriaViewModel
    {
        public string Nombre { get; set; }
        public int Tipo { get; set; }
        public int InfoUsuarioId { get; set; }
    }
    public class CrearCategoriaViewModel
    {
        [Display(Name = "Nombre")]
        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(SharedResource))]
        public string Nombre { get; set; }
    }
    public class CategoriaDocumentoViewModel
    {
        public int CategoriaId { get; set; }
        public int DocumentoId { get; set; }
    }

    public class CategoriaDocumentoBaseViewModel
    {
        public int CategoriaDocumentoBaseId { get; set; }
        public int CategoriaId { get; set; }
        public int DocumentoBaseId { get; set; }
    }
    public class EditarCategoriaViewModel
    {
        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(SharedResource))]
        [HiddenInput]
        public int CategoriaId { get; set; }

        [Display(Name = "Nombre")]
        [Required(ErrorMessageResourceName = "required", ErrorMessageResourceType = typeof(SharedResource))]
        public string Nombre { get; set; }
    }
    public class ActualizarCategoriaViewModel
    {
        public int CategoriaId { get; set; }
        public string Nombre { get; set; }
        public string Usuario { get; set; }
    }
    public class CategoriaViewModel
    {
        public int CategoriaId { get; set; }
        public string Nombre { get; set; }
    }
    public class CategoriaSeleccionadaViewModel
    {
        public int CategoriaId { get; set; }
        public string Nombre { get; set; }
        public bool Estado { get; set; }
    }
    public class CrearCategoriaJsonModel
    {
        public string Usuario { get; set; }
        public string Categoria { get; set; }
    }
    public class CategoriaJsonModel
    {
        public int Estado { get; set; }
        public int CategoriaId { get; set; }
        public string Nombre { get; set; }
    }
    public class BorradorCategoriaJsonModel
    {
        public string Usuario { get; set; }
        public string Folio { get; set; }
    }
    public class SolicitudCategoriaJsonModel
    {
        public string Usuario { get; set; }
        public string Folio { get; set; }
        public int EnvioId { get; set; }
        public int TipoEnvio { get; set; }
    }
    public class ActualizacionCategoriaDocumentoJsonModel
    {
        public int EnvioId { get; set; }
        public string Usuario { get; set; }
        public string Categorias { get; set; }
        public int TipoEnvio { get; set; }
    }
    public class RespuestaCategoriaOficioJsonModel
    {
        public int Estado { get; set; }
    }
}

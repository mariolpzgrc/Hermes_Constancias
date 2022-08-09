using Microsoft.AspNetCore.Mvc;
using Hermes2018.Services.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Hermes2018.Helpers;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Hermes2018.Controllers.Api.Constancias
{
    //[Route("api/constancias")]
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ConstanciasController : Controller
    {
        private readonly IConstanciaService _constancias;
        private readonly JsonSerializerSettings _jsonSettings;

        public ConstanciasController(IConstanciaService converter)
        {
            _jsonSettings = new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() };
            _constancias = converter;
        }

        [Route("Get_HER_TipoPersonalConstancia")]
        [HttpPost]
        public IActionResult Get_HER_TipoPersonalConstancia()
        {
            return new JsonResult(_constancias.Get_HER_TipoPersonalConstancia(), _jsonSettings);
        }

        [Route("Get_HER_Constancias")]
        [HttpPost]
        public IActionResult Get_HER_Constancias([FromBody] CustomConstancias data)
        {
            return new JsonResult(_constancias.Get_HER_Constancias(data.TipoPersonal, data.UserId), _jsonSettings);
        }


        [Route("AddSolicitudConstancia")]
        [HttpPost]
        public IActionResult AddSolicitudConstancia([FromBody] ModelsDBF.HER_SolicitudConstancia data)
        {
            return new JsonResult(_constancias.AddSolicitudConstancia(data), _jsonSettings);
        }

        [Route("AddEstadoConstancias")]
        [HttpPost]
        public IActionResult AddEstadoConstancias([FromBody] ModelsDBF.HER_SolicitudConstanciaEstado data)
        {
            return new JsonResult(_constancias.AddEstadoConstancias(data), _jsonSettings);
        }

        [Route("GET_FiltersConstancias")]
        [HttpPost]
        public IActionResult GET_FiltersConstancias([FromBody] FiltersConstancias data)
        {
            return new JsonResult(_constancias.GET_FiltersConstancias(data.ConstanciaId, data.FechaInicio, data.FechaFinal, data.EstadoId, data.Busqueda, data.CampusId, data.NoPersonal, data.Folio, data.Dependencia, data.TipoPersonal, data.Pagina), _jsonSettings);
        }

        [Route("GET_HER_SolicitudConstancia")]
        [HttpPost]
        public IActionResult GET_HER_SolicitudConstancia([FromBody] SearchCatalog data)
        {
            return new JsonResult(_constancias.GET_HER_SolicitudConstancia(data.UsuarioId, data.NumPagina), _jsonSettings);
        }

        [Route("GET_ConstanciasSolicitadas")]
        [HttpPost]
        public IActionResult GET_ConstanciasSolicitadas([FromBody] SearchCatalog data)
        {
            return new JsonResult(_constancias.GET_ConstanciasSolicitadas(data.NumPagina), _jsonSettings);
        }

        [Route("GET_ConstanciaSolicitadaId")]
        [HttpPost]
        public IActionResult GET_ConstanciaSolicitadaId([FromBody] SearchCatalog data)
        {
            return new JsonResult(_constancias.GET_ConstanciaSolicitadaId(data.Id), _jsonSettings);
        }


        [Route("ObtieneCveLogin_TP")]
        [HttpPost]
        public IActionResult ObtieneCveLogin_TP([FromBody] CustomConstancias data) 
        {
            return new JsonResult(_constancias.ObtieneCveLogin_TP(data.sCveLogin), _jsonSettings);
        }

        [Route("ObtieneServMed")]
        [HttpPost]
        public IActionResult ObtieneServMed([FromBody] CustomConstancias data)
        {
            return new JsonResult(_constancias.ObtieneServMed(data.NumPersonal, data.TipoPersonal), _jsonSettings);
        }

        [Route("ObtieneServMedDep")]
        [HttpPost]
        public IActionResult ObtieneServMedDep([FromBody] CustomConstancias data)
        {
            return new JsonResult(_constancias.ObtieneServMedDep(data.NumPersonal, data.TipoPersonal), _jsonSettings);
        }

        [Route("ObtieneTrab_Perc")]
        [HttpPost]
        public IActionResult ObtieneTrab_Perc([FromBody] CustomConstancias data)
        {
            return new JsonResult(_constancias.ObtieneTrab_Perc(data.NumPersonal, data.TipoPersonal), _jsonSettings);
        }
        
        [Route("ObtieneIpe")]
        [HttpPost]
        public IActionResult ObtieneIpe([FromBody] CustomConstancias data)
        {
            return new JsonResult(_constancias.ObtieneIpe(data.NumPersonal, data.TipoPersonal), _jsonSettings);
        }

        [Route("ObtieneMag")]
        [HttpPost]
        public IActionResult ObtieneMag([FromBody] CustomConstancias data)
        {
            return new JsonResult(_constancias.ObtieneMag(data.NumPersonal, data.TipoPersonal), _jsonSettings);
        }

        [Route("ObtieneBajIpe")]
        [HttpPost]
        public IActionResult ObtieneBajIpe([FromBody] CustomConstancias data)
        {
            return new JsonResult(_constancias.ObtieneBajaIpe(data.NumPersonal, data.TipoPersonal), _jsonSettings);
        }

        [Route("ObtieneBajMag")]
        [HttpPost]
        public IActionResult ObtieneBajMag([FromBody] CustomConstancias data)
        {
            return new JsonResult(_constancias.ObtieneBajaMag(data.NumPersonal, data.TipoPersonal), _jsonSettings);
        }

        [Route("ObtieneVisa")]
        [HttpPost]
        public IActionResult ObtieneVisa([FromBody] CustomConstancias data)
        {
            return new JsonResult(_constancias.ObtieneVISA(data.NumPersonal, data.TipoPersonal), _jsonSettings);
        }

        [Route("ObtieneVisaDep")]
        [HttpPost]
        public IActionResult ObtieneVisaDep([FromBody] CustomConstancias data)
        {
            return new JsonResult(_constancias.ObtieneVISADep(data.NumPersonal, data.TipoPersonal), _jsonSettings);
        }

        [Route("ObtienePRODep")]
        [HttpPost]
        public IActionResult ObtienePRODep([FromBody] CustomConstancias data)
        {
            return new JsonResult(_constancias.ObtienePRODep(data.NumPersonal, data.TipoPersonal), _jsonSettings);
        }
    }
}

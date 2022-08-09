using Hermes2018.Models.Anexo;
using Hermes2018.Models.Area;
using Hermes2018.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hermes2018.Services
{
    public interface IAreaService
    {
        Task<List<HER_Area>> ObtenerAreaEnListaAsync(int areaId);
        Task<List<HER_Area>> ObtenerAreasAsync(int regionId);
        Task<List<HER_Area>> ObtenerAreasAsync(int regionId, int areaId);
        Task<List<HER_Area>> ObtenerAreasVisibleAsync(int regionId);
        Task<List<HER_Area>> ObtenerAreasVisibleAsync(int regionId, int areaId);
        Task<List<AreaViewModel>> ObtenerAreasPorRegionPorUsuarioAsync(int regionId, string userName);
        Task<List<AreaViewModel>> ObtenerAreasVisiblesPorRegionPorUsuarioAsync(int regionId, string userName);
        Task<List<AreaViewModel>> ObtenerAreasVisiblesPorAreaPadreConHijasAsync(int areaPadreId);
        //--
        Task<HER_Area> ObtenerAreaConRegionPorIdAsync(int areaId);
        Task<HER_Area> ObtenerAreaVisibleConRegionPorIdAsync(int areaId);
        Task<HER_Area> ObtenerAreaConRegionPorIdPorRegionAsync(int areaId, int regionId);
        Task<HER_Area> ObtenerAreaVisibleConRegionPorIdPorRegionAsync(int areaId, int regionId);
        //--
        Task<HER_Area> ObtenerAreaPorIdAsync(int areaId);
        Task<HER_Area> ObtenerAreaPorIdVisibleAsync(int areaId);
        Task<string> ObtenerNombreAreaAsync(int areaId);
        Task<string> ObtenerNombreAreaVisibleAsync(int areaId);
        Task<TitularAreaViewModel> ObtenerTitular(int areaId);
        Task<TitularAreaViewModel> ObtenerTitularConAreaVisible(int areaId);
        //--
        Task<List<ListadoAreaViewModel>> ObtenerListadoAreasAsync(int regionId);
        Task<List<ListadoAreaViewModel>> ObtenerListadoAreasAsync(int regionId, int areaId);
        Task<List<ListadoAreaPorRegionViewModel>> ObtenerListadoAreasDeAreaPadrePorRegiónAsync(int regionId, int? areaPadreId);
        Task<List<ListadoAreaViewModel>> ObtenerListadoAreasDeAreaPadreAsync(int regionId, int? areaPadreId);
        Task<bool> ExisteNombreArea(string area);
        Task<bool> ExisteAreaPorIdAsync(int areaId);
        Task<bool> GuardarAreaAsync(CrearAreaViewModel model, int regionId);
        Task<DetalleAreaViewModel> ObtenerDetallesArea(int areaId, int regionId);
        Task<DetalleAreaViewModel> ObtenerDetallesArea(int areaId);
        Task<EditarAreaViewModel> ObtenerAreaParaEditar(int areaId, int regionId);
        Task<bool> ActualizarAreaAsync(EditarAreaViewModel modelo, int regionId);

        Task<EditarAreaEnAdminViewModel> AdminObtenerAreaParaEditar(int areaId);
        Task<bool> AdminActualizarAreaAsync(EditarAreaEnAdminViewModel modelo);

        Task<BorrarAreaViewModel> ObtenerAreaParaBorrarAsync(int areaId, int regionId);
        Task<BorrarAreaViewModel> ObtenerAreaParaBorrarAsync(int areaId);
        Task<bool> DetectaAreaEnUsoAsync(int areaId);
        Task<bool> DetectaAreaUsuariosDadosBajaAsync(int areaId);
        Task<bool> EliminarAreaAsync(int areaId);
        Task<bool> DarBajaAreaByUsuariosInactivosAsync(int areaId);
        //--
        Task<HER_AnexoArea> ObtenerLogoInstitucionAsync(int areaId);
        Task<bool> ExisteLogoInstitucionAsync(int areaId);
        Task<DarDeBajaAreaViewModel> ObtenerAreaParaDarDeBajaAsync(int areaId, int regionId);
        //--
        Task<bool> ExisteAreaEnProcesoDeCambioPorIdAsync(int areaId);
        Task<bool> DarDeBajaAreaAsync(DarDeBajaAreaViewModel bajaAreaViewModel);
        Task<List<AreaEsViewModel>> ObtenerAreasEnCambiosAsync();
        Task<List<FamiliaAreaViewModel>> ObtenerFamiliaArea(int areaPadreId, string areaPadreNombre);
        Task<bool> EsAreaHija(int areaPadreId, int areaId);
        Task<int> ObtieneAreaPadre(int areaId);
        Task<List<FamiliaAreaCompuestaViewModel>> ObtenerFamiliaAreaCompuesta(int areaPadreId);
        Task<List<InfoAreaViewModel>> ObtenerAreasConSusHijasPorClaveAsync(List<string> claves);
        Task<bool> AgregarAreaAsync(AgregarAreaViewModel model);
        Task<bool> AgregarAreaManualAsync(AgregarAreaViewModel model);
        Task<string> GenerarClaveAreaHijoAsync(int areaPadreId, string claveAreaPadre);
        Task<bool> ExisteAreaEnSIIU(int areaId);
        Task<bool> ExisteClave(string clave);
        Task<List<BuscarAreaViewModel>> BusquedaAreasAsync(string keyword);
    }
}

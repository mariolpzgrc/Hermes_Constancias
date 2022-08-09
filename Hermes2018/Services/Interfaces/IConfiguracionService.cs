using Hermes2018.Models;
using Hermes2018.Models.Anexo;
using Hermes2018.Models.Configuracion;
using Hermes2018.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hermes2018.Services
{
    public interface IConfiguracionService
    {
        Task<InfoConfiguracionLDAPViewModel> ObtenerInfoConfiguracionLDAPAsync();
        Task<InfoConfiguracionEmailViewModel> ObtenerInfoConfiguracionEmailAsync();
        InfoConfiguracionEmailViewModel ObtenerInfoConfiguracionEmail();
        Task<InfoConfiguracionAvisoPrivacidadViewModel> ObtenerInfoConfiguracionAvisoPrivacidadAsync();

        Task<ConfiguracionUsuarioViewModel> ObtenerConfiguracionUsuarioAsync(string username);
        Task<bool> ObtenerEstadoNotificacionUsuarioAsync(string username);
        Task<int> ObtenerElementosPorPaginaPorUsuarioIdAsync(int infoUsuarioId);
        Task<int> ObtenerElementosPorPaginaUsuarioAsync(string username);
        Task<EditarConfiguracionViewModel> ObtenerConfiguracionUsuarioEditar(int configuracionUsuarioId, string username);
        Task<bool> ActualizarConfiguracionAsync(EditarConfiguracionViewModel modelo);
        Task<AvisoInhabilViewModel> ObtenerAvisoInhabilAsync();
        Task<string> ObtenerInstitucionAsync();
        Task<HER_AnexoGeneral> ObtenerLogoInstitucionAsync();
        Task<HER_AnexoGeneral> ObtenerImagenPortadaAsync();
        //-----
        string ObtenerPlantillaSolicitudParaTitular();
        string ObtenerPlantillaSolicitudParaSolicitante();
        string ObtenerPlantillaSolicitudAceptada();
        string ObtenerPlantillaSolicitudRechazada();
        string ObtenerPlantillaRegistroUsuario();

        string ObtenerPlantillaEnvioDocumento();
        string ObtenerPlantillaTurnarDocumento();
        string ObtenerPlantillaResponderDocumento();

        Task<DetalleConfiguracion> ObtenerDetalleConfiguracionGeneralAsync();
        Task<EditarConfiguracionGeneral> ObtenerConfiguracionTextoParaEditarAsync(int configId);
        Task<bool> ExistePropiedadConfiguracionAsync(string propiedad);
        Task<bool> ActualizarPropiedadConfiguracionAsync(EditarConfiguracionGeneral modelo);
        Task<bool> ExisteLogoPropiedadConfiguracionAsync(string archivo);
        Task<bool> ExistePortadaPropiedadConfiguracionAsync(string archivo);
        Task<bool> ActualizarImagenPropiedadConfiguracionAsync(EditarImagenConfiguracionGeneral modelo);
        Task<List<EditarColeccionConfiguracionGeneral>> ObtenerColores();
        Task<List<EditarColeccionConfiguracionGeneral>> ObtenerExtensiones();
        Task<string> ObtenerExtensionesEnCadena();
        Task<bool> ActualizarExtensionesConfiguracionAsync(List<EditarColeccionConfiguracionGeneral> actuales, List<EditarColeccionConfiguracionGeneral> nuevos);
        Task<bool> GuardarRutaBaseAsync(CrearAnexoRutaViewModel viewModel);
        Task<EditarAnexoRutaViewModel> ObtenerEditarRutaBaseAsync(int id);
        Task<bool> EditarRutaBaseAsync(EditarAnexoRutaViewModel viewModel);
        Task<int> TotalRutasActivasAsync(int rutaBaseId);
    }
}

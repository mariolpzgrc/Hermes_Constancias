using Hermes2018.Models;
using Hermes2018.ViewComponentsModels;
using Hermes2018.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hermes2018.Services
{
    public interface IUsuarioService
    {                
        Task<RemitenteDocumentoViewModel> ObtenerInfoPersonaDocumentoAsync(string userName);
        Task<List<int>> ObtenerIdentificadoresUsuariosAsync(List<string> usuarios);
        Task<List<IdentificadorUsuarioCompuestoViewModel>> ObtenerIdentificadoresCompuestosUsuariosAsync(List<string> usuarios);
        Task<int> ObtenerIdentificadorUsuarioAsync(string usuario);
        Task<IdentificadorUsuarioCompuestoViewModel> ObtenerIdentificadorCompuestoUsuarioAsync(string usuario);
        Task<string> ObtenerIdentificadorSoloUsuarioAsync(string usuario);
        Task<List<int>> ObtenerIdentificadoresPorNombreCompletoAsync(List<string> nombres);
        Task<int> ObtenerIdentificadorPorNombreCompletoAsync(string nombre);
        Task<bool> ExisteUsuarioActivoAsync(string userName);
        Task<bool> ExisteSoloUsuarioAsync(string userName, bool esAdmin);
        Task<bool> ExisteSoloInfoUsuarioAsync(string userName);
        Task<bool> ExisteUsuarioTitularAsync(int areaId);
        Task<DateTime> ObtenerFechaCompromisoAsync(DateTime fechaRecepcion, int usuarioId);
        Task<DateTime> ObtenerFechaProrrogaCompromisoAsync(DateTime fechaRecepcion);
        Task<string> ObtenerRolUsuarioAsync(string userName);
        Task<IEnumerable<string>> ObtenerRolesUsuarioAsync(string userName);
        //--
        Task<List<UsuarioLocalJsonModel>> BusquedaUsuariosLocalesAsync(string keyword);
        Task<List<UsuarioLocalJsonModel>> BusquedaUsuariosLocalesAsync(string keyword, string usercurrent);
        //buscar
        Task<List<UsuariosBuscarViewModel>> BusquedaUsuariosLocalesBuscarAsync(string keyword);
        Task<List<UsuarioLocalJsonModel>> BusquedaUsuariosLocalesColeccionAsync(List<string> usuarios);
        Task<List<UsuarioLocalJsonModel>> BusquedaUsuariosLocalesRevisionAsync(string userName, int areaId, string keyword);
        Task<List<UsuarioLocalJsonModel>> BusquedaUsuariosLocalesDelegarAsync(string userName, string keyword);
        Task<List<UsuarioLocalJsonModel>> BusquedaUsuariosLocalesReasignacionAsync(string keyword);
        Task<List<UsuarioADViewModel>> BusquedaUsuariosDirectorioActivoAsync(string keyword);
        Task<List<UsuarioADViewModel>> LimpiarBusquedaUsuariosDirectorioActivoAsync(List<UsuarioADViewModel> listado);
        Task<List<BandejasViewComponentModel>> ObtenerUsuariosDelegadosAsync(string usuarioTitular);

        //Módulo usuarios
        List<UsuariosAdministradoresViewModel> ObtenerUsuariosAdministradores(string usuarioActual);
        Task<List<UsuariosPorAreaViewModel>> ObtenerUsuariosPorAreaAsync(int areaId);
        Task<UsuariosDetallesViewModel> ObtenerDetalleUsuarioAsync(string rol, string username);
        Task<bool> GuardarUsuarioAsync(UsuariosCrearViewModel viewModel);
        Task<bool> AdminGuardarUsuarioAsync(AdminUsuariosCrearViewModel viewModel);
        Task<BajaUsuarioViewModel> ObtenerInfoUsuarioParaBajaAsync(string username, InfoConfigUsuarioViewModel info);
        Task<bool> ExisteUsuarioSinReasignarAsync(string username);
        Task<bool> ExisteUsuarioSinReasignarPorIdAsync(int infoUsuarioId);
        Task<bool> GuardarReasignacion(ReasignarUsuarioViewModel viewModel);
        Task<bool> AdminGuardarReasignacion(AdminReasignarUsuarioViewModel viewModel);
        Task<bool> DarDeBajaUsuarioAsync(BajaUsuarioViewModel bajaViewModel, IEnumerable<string> roles);
        Task<bool> UsuarioEstaAprobadoAsync(string username);
        Task<bool> UsuarioAceptoTerminosAsync(string username);
        Task<HER_Usuario> ObtenerUsuarioAsync(string username);
        //Solicitud
        Task<SolicitudUsuarioViewModel> ObtenerUsuarioADSolicitudAsync(string username);
        Task<string> ObtenerIdUsuarioTitularAsync(int areaId);
        Task<bool> GuardarUsuarioConSolicitudAsync(bool existeUsuario, bool existeInfoUsuario, bool estaActivo, InfoUsuarioOracleViewModel solicitud, string direccion, string telefono);
        Task<string> ObtenerNombreUsuarioDelInfoUsuarioPorIdAsync(int infoUsuarioId);
        Task<bool> EsTipoAdministradorAsync(string userName);

        //Terminos
        Task<bool> GuardarAceptacionTerminos(string username);
        //--
        Task<bool> DetectaCuentaDependenciaAsync(string username);
        Task<string> ObtenerCuentaPersonal(string username, string rol);

        //Obtener información del usuario para llenar los Claims
        Task<InfoUsuarioNormalClaims> ObtieneInfoUsuarioNormalParaClaims(string userId, string username, string cuentaDependencia);

        Task<PermisoAdminAreaViewModel> ObtenerInfoUsuarioParaPermisoAsync(string username, InfoConfigUsuarioViewModel info);
        Task<bool> GuardarPermisoUsuarioAsync(PermisoAdminAreaViewModel viewModel);
        Task<bool> ExisteAdministradorArea(int areaId);
    }
}

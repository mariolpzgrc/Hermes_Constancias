using Hermes2018.Models;
using Hermes2018.ModelsDBF;
using System.Collections.Generic;
using HER_Constancias = Hermes2018.Models.Constancia.HER_Constancias;
using HER_TipoPersonalConstancia = Hermes2018.Models.Constancia.HER_TipoPersonalConstancia;

namespace Hermes2018.Services.Interfaces
{
    public interface IConstanciaService
    {
        List<HER_TipoPersonalConstancia> Get_HER_TipoPersonalConstancia();
        List<HER_Constancias> Get_HER_Constancias(int tipoPersonal, string usuarioId);
        ReturnCRUD AddSolicitudConstancia(HER_SolicitudConstancia data);
        ListaPaginador<SolicitudConstanciaCustom> GET_HER_SolicitudConstancia(string idUsuario, int pagina);
        ListaPaginador<ConstanciasSolicitadasCustom> GET_ConstanciasSolicitadas(int pagina);
        ConstanciasSolicitadasCustom GET_ConstanciaSolicitadaId(int id);
        ReturnCRUD AddEstadoConstancias(HER_SolicitudConstanciaEstado data);
        ListaPaginador<Models.SolicitudConstanciaCustom> GET_FiltersConstancias(int constanciaId, string fechaI, string fechaT, int estadoId, string busqueda, int campusId, int noPersonal, string folio, string dependencia, int tipoPersonal, int pagina);

        /**Servicios de prueba de ls DSIA*/
        /**Servicios del usuario*/
        oLoginTP ObtieneCveLogin_TP(string sCveLogin);

        /**Constancias*/
        List<oLsServMed> ObtieneServMed(int numPer, int tipoPer);
        List<oLsServMed> ObtieneServMedDep(int numPer, int tipoPer);
        List<oLsIpe> ObtieneTrab_Perc(int numPer, int tipoPer);
        List<oLsIpe> ObtieneIpe(int numPer, int tipoPer);
        List<oLsIpe> ObtieneMag(int numPer, int tipoPer);
        List<oLsOfiBajIPE> ObtieneBajaIpe(int numPer, int tipoPer);
        DesencriptarJson ObtieneBajaMag(int numPer, int tipoPer);
        List<oLsVisa> ObtieneVISA(int numPer, int tipoPer);
        List<oLsVisa> ObtieneVISADep(int numPer, int tipoPer);
        List<oLsPRODEP> ObtienePRODep(int numPer, int tipoPer); 
    }
}

using System;
using System.Collections.Generic;
using System.Linq;

namespace Hermes2018.Models
{
    public class LoginCustom
    {
        public UsrToken UsrToken { get; set; }
    }
    public class UsrToken
    {
        public string sTokenUV { get; set; }
        public object sTokenMS { get; set; }
        public DateTime dtExpira { get; set; }
        public string sUsuario { get; set; }
        public object sError { get; set; }
        public object sClave { get; set; }
    }
    public class ServMedCustom
    {
        public List<oLsServMed> oLsServMed { get; set; }
        public oEncrip oEncrip { get; set; }
        public ServMedCustom() { }
    }

    public class ServMedDepCustom
    {
        public List<oLsServMed> oLsServMedDep { get; set; }
        public oEncrip oEncrip { get; set; }
        public ServMedDepCustom() { }
    }

    public class VisaCustom
    {
        public List<oLsVisa> oLsVisa { get; set; }
        public oEncrip oEncrip { get; set; }
        public VisaCustom() { }
    }

    public class VisaDepCustom
    {
        public List<oLsVisa> oLsVisaDep { get; set; }
        public oEncrip oEncrip { get; set; }
        public VisaDepCustom() { }
    }

    public class IpeCustom
    {
        public List<oLsIpe> oLsIpe { get; set; }
        public oEncrip oEncrip { get; set; }
        public IpeCustom() { }

    }

    public class MagCustom
    {
        public List<oLsIpe> oLsMag { get; set; }
        public oEncrip oEncrip { get; set; }
        public MagCustom() { }
    }

    public class TrabPercCustom
    {
        public List<oLsIpe> oLsTrabPerc { get; set; }
        public oEncrip oEncrip { get; set; }
        public TrabPercCustom() { }
    }

    public class OfiBajIPECustom
    {
        public List<oLsOfiBajIPE> oLsOfiBajIPE { get; set; }
        public oEncrip oEncrip { get; set; }
        public OfiBajIPECustom() { }
    }

    public class ProdepCustom
    {
        public List<oLsPRODEP> oLsPRODEP { get; set; }
        public oEncrip oEncrip { get; set; }
        public ProdepCustom() { }

    }

    public class EncriptarJson
    {
        public string sAesKey { get; set; }
        public string sClave { get; set; }
        public string sEncrip { get; set; }
    }

    public class LoginDataCustom
    {
        public oLoginTP oLoginTP { get; set; }
        public oEncrip oEncrip { get; set; }
    }

    public class oLoginTP { 
        public string iNumPer { get; set; }
        public string sTipPer { get; set; }
        public List<TipoPersonalCustom> TipoPersonal
        {
            get
            {
                List<TipoPersonalCustom> tipos = new List<TipoPersonalCustom>();
                if (sTipPer.Contains("ADMINISTRATIVO, TECNICO"))
                {
                    sTipPer = sTipPer.Replace("ADMINISTRATIVO, TECNICO", "ADMINISTRATIVO TECNICO");
                }
                string[] tiposP = sTipPer.Split(",");
                string[] tipoValue = null;
                TipoPersonalCustom tip = null;
                foreach (var data in tiposP)
                {
                    tipoValue = data.Split("-");
                    tip = new TipoPersonalCustom();
                    tip.Id = Convert.ToInt32(tipoValue[0].Trim());
                    tip.TipoPersonal = Convert.ToString(tipoValue[1].Trim());
                    if (tip.TipoPersonal.Contains("ADMINISTRATIVO TECNICO")) 
                    {
                        tip.TipoPersonal = tip.TipoPersonal.Replace("ADMINISTRATIVO TECNICO", "ADMINISTRATIVO, TECNICO");
                    }
                    tipos.Add(tip);
                }
                return tipos;
            }
        }
    }
    public class LoginInfoDatacustom 
    {
        public List<OLsLoginNP> oLsLoginNP { get; set; }
        public OEncrip oEncrip { get; set; }
    }

    public class OLsLoginNP
    {
        public int iNumPer { get; set; }
        public string sNombre { get; set; }
        public string sUsuario { get; set; }
        public string sNumDep { get; set; }
        public string sDescDep { get; set; }
        public string sTipo_Per { get; set; }
        public int iTipo_Per
        {
            get
            {
                string[] tipoValue = sTipo_Per.Split("-");
                int NumeroPersonal = Convert.ToInt32(tipoValue[0].Trim());
                return NumeroPersonal;
            }
        }
    }
    public class OEncrip
    {
        public string sEncript { get; set; }
    }

    public class oEncrip
    {
        public string sEncript { get; set; }
    }

    public class JsonForDesencript 
    {
        public string sEncript { get; set; }
    }

    public class DesencriptarJson
    {
        public string sAesKey { get; set; }
        public string sClave { get; set; }
        public string sJson { get; set; }
    }

    public class TipoPersonalCustom
    {
        public Int32 Id { get; set; }
        public String TipoPersonal { get; set; }
        public TipoPersonalCustom() { }
    }

    public class oLsServMed
    {
        public string sNumPer { get; set; }
        public string sNombre { get; set; }
        public string dtFIngreso { get; set; }
        public string sNumDep { get; set; }
        public string sDescDep { get; set; }
        public string sRegion { get; set; }
        public string sDesRegion { get; set; }
        public string sTPE { get; set; }
        public string sDesTPE { get; set; }
        public string sNumCont { get; set; }
        public string sDesCont { get; set; }
        public string sNumPuesto { get; set; }
        public string sDesPuesto { get; set; }
        public string sNumCat { get; set; }
        public string sDescCat { get; set; }
        public string sSuelPrest { get; set; }
        public string sNomDepen { get; set; }
        public string sParentesco { get; set; }
        public string sDirecPers { get; set; }
        public string sNPeri { get; set; }
        public string sHrs { get; set; }
        public oLsServMed(){}
    }

    public class oLsIpe
    {
        public string sNumPer { get; set; }
        public string sNombre { get; set; }
        public string dtFIngreso { get; set; }
        public string sNumDep { get; set; }
        public string sDescDep { get; set; }
        public string sRegion { get; set; }
        public string sDesRegion { get; set; }
        public string sTPE { get; set; }
        public string sDesTPE { get; set; }
        public string sNumCont { get; set; }
        public string sDesCont { get; set; }
        public string sNumPuesto { get; set; }
        public string sDesPuesto { get; set; }
        public string sNumCat { get; set; }
        public string sDescCat { get; set; }
        public string sSueldo { get; set; }
        public string sSuelPrest { get; set; }
        public string sDirecPers { get; set; }
        public string dtFCotIp { get; set; }
        public string sNPeri { get; set; }
        public string sHrs { get; set; }
        public oLsIpe()
        {
        }
    }
    public class oLsOfiBajIPE
    {
        public string sNumPer { get; set; }
        public string sNombre { get; set; }
        public string sNumDep { get; set; }
        public string sDescDep { get; set; }
        public string sRegion { get; set; }
        public string sDesRegion { get; set; }
        public string sNumPuesto { get; set; }
        public string sDesPuesto { get; set; }
        public string sNumCat { get; set; }
        public string sDescCat { get; set; }

        public string sDirecPers { get; set; }
        public string sFFinPla { get; set; }
        public oLsOfiBajIPE()
        {
        }
    }

    public class oLsPRODEP
    {
        public string sNumPer { get; set; }
        public string sNombre { get; set; }
        public string sNumDep { get; set; }
        public string sDescDep { get; set; }
        public string sRegion { get; set; }
        public string sDesRegion { get; set; }
        public string sTPE { get; set; }
        public string sDesTPE { get; set; }
        public string sNumCont { get; set; }
        public string sDesCont { get; set; }
        public string sNumPuesto { get; set; }
        public string sDesPuesto { get; set; }
        public string sNumCat { get; set; }
        public string sDescCat { get; set; }
        public string sDirecPers { get; set; }
        public string sFAltPlact { get; set; }
        public string sPeri { get; set; }
        public string sHrs { get; set; }
        public oLsPRODEP() { }
    }

    public class oLsVisa
    {
        public string sNumPer { get; set; }
        public string sNombre { get; set; }
        public string dtFIngreso { get; set; }
        public string sNumDep { get; set; }
        public string sDescDep { get; set; }
        public string sRegion { get; set; }
        public string sDesRegion { get; set; }
        public string sTPE { get; set; }
        public string sDesTPE { get; set; }
        public string sNumCont { get; set; }
        public string sDesCont { get; set; }
        public string sNumPuesto { get; set; }
        public string sDesPuesto { get; set; }
        public string sNumCat { get; set; }

        public string sDescCat { get; set; }
        public string sSueldo { get; set; }
        public string sSuelPrest { get; set; }
        public string sDirecPers { get; set; }
        public string dtFCotIp { get; set; }
        public string sNPeri { get; set; }
        public string sHrs { get; set; }
        public oLsVisa(){ }
    }

    public class SolicitudConstanciaCustom
    {
        public int Id { get; set; }
        public int ConstanciaId { get; set; }
        public string UsuarioId { get; set; }
        public string NombreConstancia { get; set; }
        public int NoPersonal { get; set; }
        public DateTime FechaSolicitud { get; set; }
        public DateTime? FechaEntrega
        {
            get
            {
                DateTime? fechaS = null;
                var estadoCons = EstadosConstancia.OrderByDescending(x => x.FechaHora).ToList();
                foreach (var info in estadoCons)
                {
                    if (info.EstadoId == 2 || info.EstadoId == 3 || info.EstadoId == 4)
                        fechaS = info.FechaHora;

                }

                if (fechaS != null)
                {
                    DateTime fechaProcesar = Convert.ToDateTime(fechaS);
                    int dias = 5;
                    while (dias > 0)
                    {
                        fechaProcesar = fechaProcesar.AddDays(1);
                        if (fechaProcesar.DayOfWeek == DayOfWeek.Saturday || fechaProcesar.DayOfWeek == DayOfWeek.Sunday)
                            fechaProcesar = fechaProcesar.AddDays(1);
                        else
                            dias = dias - 1;
                    }
                    fechaS = fechaProcesar;
                }
                return fechaS;
            }
        }
        public int EstadoId { get; set; }
        public string NombreEstado { get; set; }
        public string Folio { get; set; }
        public string Mensaje { get; set; }

        public int CveDep { get; set; }
        public string Dependencia { get; set; }
        public int? CampusId { get; set; }
        public string NombreUsuario { get; set; }
        public string NombreCampus { get; set; }
        public int TipoPersonal { get; set; }
        public List<ModelsDBF.HER_SolicitudConstanciaEstado> EstadosConstancia { get; set; }
        public SolicitudConstanciaCustom() { }

    }

    public class ConstanciasSolicitadasCustom
    {
        public int Id { get; set; }
        public int ConstanciaId { get; set; }
        public string UsuarioId { get; set; }
        public string NombreConstancia { get; set; }
        public int NoPersonal { get; set; }
        public DateTime FechaSolicitud { get; set; }
        public int EstadoId { get; set; }
        public string NombreEstado { get; set; }
        public string Folio { get; set; }
        public string Mensaje { get; set; }
        public int CveDep { get; set; }
        public string NombreUsuario { get; set; }

        public string NombreDependencia { get; set; }
        public int TipoPersonal { get; set; }

        public List<ModelsDBF.HER_SolicitudConstanciaEstado> EstadosConstancia { get; set; }
        public ConstanciasSolicitadasCustom() { }
    }

    public class General
    {

        private String LetterRandom()
        {
            Random ran = new Random();

            String b = "abcdefghijklmnopqrstuvwxyz";

            int length = 4;

            String random = "";

            for (int i = 0; i < length; i++)
            {
                int a = ran.Next(26);
                random = random + b.ElementAt(a);
            }
            return random;
        }

        private String NumberRandom()
        {
            Random rndNum = new Random(int.Parse(Guid.NewGuid().ToString().Substring(0, 8), System.Globalization.NumberStyles.HexNumber));
            int rnd = rndNum.Next(1000, 9999);
            return rnd.ToString();
        }

        public String ReturnFolio()
        {
            return LetterRandom() + NumberRandom() + "-" + DateTime.Now.ToString("yy");
        }

        public General() { }
    }

    public class ListaPaginador<T>
    {
        public int TotalElementos { get; set; }
        public int ElementosPagina { get; set; }
        public int Inicial
        {
            get
            {
                return PaginaActual * ElementosPagina - ElementosPagina;
            }
        }
        public int TotalPaginas
        {
            get
            {
                if (ElementosPagina <= 0)
                    return 0;

                return (int)Math.Ceiling((double)TotalElementos / ElementosPagina);
            }
        }
        public int PaginaActual { get; set; }
        public IEnumerable<T> Elementos { get; set; }

        public ListaPaginador()
        {
            //ElementosPagina = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["numPagina"]);
        }
    }
    public class ReturnCRUD
    {
        public ReturnCRUD(){}
        public ReturnCRUD(int code, string message)
        {
            this.CodeResponse = code;
            this.Message = message;
        }
        public Int32 CodeResponse { get; set; }
        public String Hash { get; set; }
        public String Message { get; set; }
    }
}
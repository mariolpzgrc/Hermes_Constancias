using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hermes2018.Data;
using Hermes2018.ModelsDBF;
using Hermes2018.OracleHelpers;
using Hermes2018.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Hermes2018.Helpers;
using Oracle.ManagedDataAccess.Client;
using System.Globalization;
using Microsoft.Extensions.Logging;

namespace Hermes2018.Services
{
    public class OracleService : IOracleService 
    {
        private readonly ApplicationDbContext _context;
        public readonly DBFContext _DBFContext;
        public readonly IConfiguration _configuration;
        public readonly IAreaService _areaService;
        private readonly ILogger<OracleService> _logger;
        private readonly CultureInfo _cultureEs;

        public OracleService(ApplicationDbContext context,
            DBFContext DBFContext,
            IConfiguration configuration,
            ILogger<OracleService> logger,
        IAreaService areaService)
        {
            _context = context;
            _DBFContext = DBFContext;
            _logger = logger;
            _configuration = configuration;
            _areaService = areaService;
            _cultureEs = new CultureInfo("es-MX");
        }

        //[Áreas] 
        public async Task<List<DetectarAreaViewModelDBF>> ObtenerAreasAsync(string clave, int areaId)
        {
            var claveAreasRegistradasQuery = _context.HER_Area
                .Where(x => x.HER_Area_PadreId == areaId
                         && x.HER_BajaInactividad == false
                         && x.HER_Estado == ConstEstadoArea.EstadoN1)
                .Select(x => x.HER_Clave)
                .AsNoTracking()
                .AsQueryable();

            var claveAreasRegistradas = await claveAreasRegistradasQuery.ToListAsync();

            var areasQuery = _DBFContext.CDEPEN
                .Where(x => x.NDEPA == clave)
                .Select(x => new DetectarAreaViewModelDBF
                {
                    Clave = x.NDEP,
                    Nombre = x.DDEPP,
                    Estado = claveAreasRegistradas.Contains(x.NDEP) ? ConstEstadoAreaPorAgregar.EstadoT2 : ConstEstadoAreaPorAgregar.EstadoT1,
                    EstadoId = claveAreasRegistradas.Contains(x.NDEP) ? ConstEstadoAreaPorAgregar.EstadoN2 : ConstEstadoAreaPorAgregar.EstadoN1
                })
                .OrderBy(x => x.Estado).ThenBy(x => x.Clave)
                .AsNoTracking()
                .AsQueryable();

            return await areasQuery.ToListAsync();
        }
        public async Task<List<DetectarAreaViewModelDBF>> ObtenerAreasAsync(int areaId)
        {
            List<DetectarAreaViewModelDBF> listado = new List<DetectarAreaViewModelDBF>();

            var claveAreaPadreQuery = _context.HER_Area
                .Where(x => x.HER_AreaId == areaId
                         && x.HER_BajaInactividad == false
                         && x.HER_Estado == ConstEstadoArea.EstadoN1)
                .Select(x => x.HER_Clave)
                .AsNoTracking()
                .AsQueryable();

            var claveAreaPadre = claveAreaPadreQuery.FirstOrDefault();
            //---
            if (!string.IsNullOrEmpty(claveAreaPadre)) 
            {
                var claveAreasRegistradasHijasQuery = _context.HER_Area
                    .Where(x => x.HER_Area_PadreId == areaId
                             && x.HER_Estado == ConstEstadoArea.EstadoN1)
                    .Select(x => x.HER_Clave)
                    .AsNoTracking()
                    .AsQueryable();

                var claveAreasRegistradas = await claveAreasRegistradasHijasQuery.ToListAsync();

                var areasQuery = _DBFContext.CDEPEN
                    .Where(x => x.NDEPA == claveAreaPadre)
                    .Select(x => new DetectarAreaViewModelDBF
                    {
                        Clave = x.NDEP,
                        Nombre = x.DDEPP,
                        Estado = claveAreasRegistradas.Contains(x.NDEP) ? ConstEstadoAreaPorAgregar.EstadoT2 : ConstEstadoAreaPorAgregar.EstadoT1,
                        EstadoId = claveAreasRegistradas.Contains(x.NDEP) ? ConstEstadoAreaPorAgregar.EstadoN2 : ConstEstadoAreaPorAgregar.EstadoN1
                    })
                    .OrderBy(x => x.Estado).ThenBy(x => x.Clave)
                    .AsNoTracking()
                    .AsQueryable();

                listado = await areasQuery.ToListAsync();
            }
            
            return listado;
        }
        public async Task<AgregarAreaViewModel> ObtenerAreaParaAgregarAsync(string clave)
        {
            var claveAreaRegistradaQuery = _context.HER_Area
                .Where(x => x.HER_Clave == clave
                         && x.HER_BajaInactividad == false
                         && x.HER_Estado == ConstEstadoArea.EstadoN1)
                .AsNoTracking()
                .AsQueryable();

            var claveAreaRegistrada = await claveAreaRegistradaQuery.AnyAsync();
            AgregarAreaViewModel modelo = new AgregarAreaViewModel();

            if (!claveAreaRegistrada)
            {
                var areasQuery = _DBFContext.CDEPEN
                    .Where(x => x.NDEP == clave)
                    .Select(x => new AgregarAreaViewModel
                    {
                        Nombre = x.DDEPP,
                        Clave = x.NDEP,
                        Direccion = x.DIREC1,
                    })
                    .AsNoTracking()
                    .AsQueryable();

                modelo = await areasQuery.FirstOrDefaultAsync();
            }

            return modelo;
        }
        public async Task<bool> ExisteAreaPorClaveAsync(string clave)
        {
            var result = false;

            if (!string.IsNullOrEmpty(clave)) 
            {
                var areasQuery = _DBFContext.CDEPEN
                      .Where(x => x.NDEP == clave)
                      .AsNoTracking()
                      .AsQueryable();

                result = await areasQuery.AnyAsync();
            }

            return result;
        }
        //[Usuarios]
        //General
        public BaseUsuarioOracleViewModel ObtieneUsuariosOracleAsync(string usuario)
        {
            var lsConexion = _configuration.GetConnectionString("OracleConnection");
            //string lsConexion = "User Id=ddsa;Password=r0mxh4r1;Data Source=148.226.208.232:1521/RHPROD;";
            string consulta = "SELECT * FROM DATPERS WHERE CVELOGIN=:CVELOGIN";
            
            Permiso seguridad = new Permiso();

            OracleConnection odbConn = new OracleConnection(lsConexion);
            //--
            var columns = new List<string>();
            var typesColumns = new List<string>();
            var valores = new List<UsuarioOracleViewModel>();
            //--
            try
            {
                odbConn.Open();
                if (seguridad.VerificarSeguridad("HWIPDOC", odbConn) == 0)
                {
                    using (OracleCommand command = new OracleCommand(consulta, odbConn))
                    {
                        OracleParameter id = new OracleParameter("id", usuario);
                        command.Parameters.Add(id);
                        //--
                        OracleDataReader reader = command.ExecuteReader();

                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            columns.Add(reader.GetName(i));
                            typesColumns.Add(reader.GetFieldType(i).ToString());
                        }

                        while (reader.Read())
                        {
                            valores.Add(new UsuarioOracleViewModel()
                            {
                                NOPER = (reader.IsDBNull(reader.GetOrdinal("NOPER")) ? 0 : reader.GetInt32(reader.GetOrdinal("NOPER"))),
                                NOMBRE = (reader.IsDBNull(reader.GetOrdinal("NOMBRE")) ? "NULL" : reader.GetString(reader.GetOrdinal("NOMBRE"))),
                                SEXO = (reader.IsDBNull(reader.GetOrdinal("SEXO")) ? "NULL" : reader.GetString(reader.GetOrdinal("SEXO"))),
                                RFC = (reader.IsDBNull(reader.GetOrdinal("RFC")) ? "NULL" : reader.GetString(reader.GetOrdinal("RFC"))),
                                CURP = (reader.IsDBNull(reader.GetOrdinal("CURP")) ? "NULL" : reader.GetString(reader.GetOrdinal("CURP"))),
                                PIDM = (reader.IsDBNull(reader.GetOrdinal("PIDM")) ? 0 : reader.GetInt32(reader.GetOrdinal("PIDM"))),
                                IPE = (reader.IsDBNull(reader.GetOrdinal("IPE")) ? 0 : reader.GetInt32(reader.GetOrdinal("IPE"))),
                                EDO_CIV = (reader.IsDBNull(reader.GetOrdinal("EDO_CIV")) ? "NULL" : reader.GetString(reader.GetOrdinal("EDO_CIV"))),
                                NACIONALIDAD = (reader.IsDBNull(reader.GetOrdinal("NACIONALIDAD")) ? "NULL" : reader.GetString(reader.GetOrdinal("NACIONALIDAD"))),
                                NIVEL = (reader.IsDBNull(reader.GetOrdinal("NIVEL")) ? "NULL" : reader.GetString(reader.GetOrdinal("NIVEL"))),
                                CALLE = (reader.IsDBNull(reader.GetOrdinal("CALLE")) ? "NULL" : reader.GetString(reader.GetOrdinal("CALLE"))),
                                NUMEXT = (reader.IsDBNull(reader.GetOrdinal("NUMEXT")) ? "NULL" : reader.GetString(reader.GetOrdinal("NUMEXT"))),
                                NUMINT = (reader.IsDBNull(reader.GetOrdinal("NUMINT")) ? "NULL" : reader.GetString(reader.GetOrdinal("NUMINT"))),
                                COLONIA = (reader.IsDBNull(reader.GetOrdinal("COLONIA")) ? "NULL" : reader.GetString(reader.GetOrdinal("COLONIA"))),
                                //CP = (reader.IsDBNull(reader.GetOrdinal("CP")) ? 0 : reader.GetInt32(reader.GetOrdinal("CP"))),
                                CP = (reader.IsDBNull(reader.GetOrdinal("CP")) ? "NULL" : reader.GetString(reader.GetOrdinal("CP"))),
                                CIUDAD = (reader.IsDBNull(reader.GetOrdinal("CIUDAD")) ? "NULL" : reader.GetString(reader.GetOrdinal("CIUDAD"))),
                                MUNICIPIO = (reader.IsDBNull(reader.GetOrdinal("MUNICIPIO")) ? "NULL" : reader.GetString(reader.GetOrdinal("MUNICIPIO"))),
                                ESTADO = (reader.IsDBNull(reader.GetOrdinal("ESTADO")) ? "NULL" : reader.GetString(reader.GetOrdinal("ESTADO"))),
                                TEL = (reader.IsDBNull(reader.GetOrdinal("TEL")) ? "NULL" : reader.GetString(reader.GetOrdinal("TEL"))),
                                NTPE = (reader.IsDBNull(reader.GetOrdinal("NTPE")) ? 0 : reader.GetInt16(reader.GetOrdinal("NTPE"))),
                                TIPO_PER = (reader.IsDBNull(reader.GetOrdinal("TIPO_PER")) ? "NULL" : reader.GetString(reader.GetOrdinal("TIPO_PER"))),
                                NPUE = (reader.IsDBNull(reader.GetOrdinal("NPUE")) ? 0 : reader.GetInt32(reader.GetOrdinal("NPUE"))),
                                DPUE = (reader.IsDBNull(reader.GetOrdinal("DPUE")) ? "NULL" : reader.GetString(reader.GetOrdinal("DPUE"))),
                                NCAT = (reader.IsDBNull(reader.GetOrdinal("NCAT")) ? 0 : reader.GetInt32(reader.GetOrdinal("NCAT"))),
                                CATEGORIA = (reader.IsDBNull(reader.GetOrdinal("CATEGORIA")) ? "NULL" : reader.GetString(reader.GetOrdinal("CATEGORIA"))),
                                NCON = (reader.IsDBNull(reader.GetOrdinal("NCON")) ? 0 : reader.GetInt16(reader.GetOrdinal("NCON"))),
                                TIPO_CONT = (reader.IsDBNull(reader.GetOrdinal("TIPO_CONT")) ? "NULL" : reader.GetString(reader.GetOrdinal("TIPO_CONT"))),
                                INGRESO = (reader.IsDBNull(reader.GetOrdinal("INGRESO")) ? DateTime.Now : reader.GetDateTime(reader.GetOrdinal("INGRESO"))),
                                ANT = (reader.IsDBNull(reader.GetOrdinal("ANT")) ? 0 : reader.GetInt16(reader.GetOrdinal("ANT"))),
                                NDEP = (reader.IsDBNull(reader.GetOrdinal("NDEP")) ? 0 : reader.GetInt32(reader.GetOrdinal("NDEP"))),
                                DDEP = (reader.IsDBNull(reader.GetOrdinal("DDEP")) ? "NULL" : reader.GetString(reader.GetOrdinal("DDEP"))),
                                DCDEP = (reader.IsDBNull(reader.GetOrdinal("DCDEP")) ? "NULL" : reader.GetString(reader.GetOrdinal("DCDEP"))),
                                TDEP = (reader.IsDBNull(reader.GetOrdinal("TDEP")) ? "NULL" : reader.GetString(reader.GetOrdinal("TDEP"))),
                                NSUBP = (reader.IsDBNull(reader.GetOrdinal("NSUBP")) ? 0 : reader.GetInt32(reader.GetOrdinal("NSUBP"))),
                                DSUBP = (reader.IsDBNull(reader.GetOrdinal("DSUBP")) ? "NULL" : reader.GetString(reader.GetOrdinal("DSUBP"))),
                                DCSSUBP = (reader.IsDBNull(reader.GetOrdinal("DCSSUBP")) ? "NULL" : reader.GetString(reader.GetOrdinal("DCSSUBP"))),
                                NAREA = (reader.IsDBNull(reader.GetOrdinal("NAREA")) ? 0 : reader.GetInt16(reader.GetOrdinal("NAREA"))),
                                ISSBP = (reader.IsDBNull(reader.GetOrdinal("ISSBP")) ? "NULL" : reader.GetString(reader.GetOrdinal("ISSBP"))),
                                CORREO = (reader.IsDBNull(reader.GetOrdinal("CORREO")) ? "NULL" : reader.GetString(reader.GetOrdinal("CORREO"))),
                                STATUS = (reader.IsDBNull(reader.GetOrdinal("STATUS")) ? "NULL" : reader.GetString(reader.GetOrdinal("STATUS"))),
                                SNI = (reader.IsDBNull(reader.GetOrdinal("SNI")) ? "NULL" : reader.GetString(reader.GetOrdinal("SNI"))),
                                LADA = (reader.IsDBNull(reader.GetOrdinal("LADA")) ? "NULL" : reader.GetString(reader.GetOrdinal("LADA"))),
                                TELEF_DEP = (reader.IsDBNull(reader.GetOrdinal("TELEF_DEP")) ? "NULL" : reader.GetString(reader.GetOrdinal("TELEF_DEP"))),
                                EXTEN = (reader.IsDBNull(reader.GetOrdinal("EXTEN")) ? "NULL" : reader.GetString(reader.GetOrdinal("EXTEN"))),
                                D_DIREC1 = (reader.IsDBNull(reader.GetOrdinal("D_DIREC1")) ? "NULL" : reader.GetString(reader.GetOrdinal("D_DIREC1"))),
                                D_DIR2 = (reader.IsDBNull(reader.GetOrdinal("D_DIR2")) ? "NULL" : reader.GetString(reader.GetOrdinal("D_DIR2"))),
                                EDIF = (reader.IsDBNull(reader.GetOrdinal("EDIF")) ? "NULL" : reader.GetString(reader.GetOrdinal("EDIF"))),
                                PISO = (reader.IsDBNull(reader.GetOrdinal("PISO")) ? "NULL" : reader.GetString(reader.GetOrdinal("PISO"))),
                                D_CIUDAD = (reader.IsDBNull(reader.GetOrdinal("D_CIUDAD")) ? "NULL" : reader.GetString(reader.GetOrdinal("D_CIUDAD"))),
                                D_CP = (reader.IsDBNull(reader.GetOrdinal("D_CP")) ? "NULL" : reader.GetString(reader.GetOrdinal("D_CP"))),
                                NZON = (reader.IsDBNull(reader.GetOrdinal("NZON")) ? 0 : reader.GetInt16(reader.GetOrdinal("NZON"))),
                                DZON = (reader.IsDBNull(reader.GetOrdinal("DZON")) ? "NULL" : reader.GetString(reader.GetOrdinal("DZON"))),
                                NSUBZ = (reader.IsDBNull(reader.GetOrdinal("NSUBZ")) ? 0 : reader.GetInt16(reader.GetOrdinal("NSUBZ"))),
                                SUBREGION = (reader.IsDBNull(reader.GetOrdinal("SUBREGION")) ? "NULL" : reader.GetString(reader.GetOrdinal("SUBREGION"))),
                                PPROMEP = (reader.IsDBNull(reader.GetOrdinal("PPROMEP")) ? "NULL" : reader.GetString(reader.GetOrdinal("PPROMEP"))),
                                FNAC = (reader.IsDBNull(reader.GetOrdinal("FNAC")) ? DateTime.Now : reader.GetDateTime(reader.GetOrdinal("FNAC"))),
                                LNAC = (reader.IsDBNull(reader.GetOrdinal("LNAC")) ? "NULL" : reader.GetString(reader.GetOrdinal("LNAC"))),
                                TURNO = (reader.IsDBNull(reader.GetOrdinal("TURNO")) ? "NULL" : reader.GetString(reader.GetOrdinal("TURNO"))),
                                APAT = (reader.IsDBNull(reader.GetOrdinal("APAT")) ? "NULL" : reader.GetString(reader.GetOrdinal("APAT"))),
                                AMAT = (reader.IsDBNull(reader.GetOrdinal("AMAT")) ? "NULL" : reader.GetString(reader.GetOrdinal("AMAT"))),
                                NOMB = (reader.IsDBNull(reader.GetOrdinal("NOMB")) ? "NULL" : reader.GetString(reader.GetOrdinal("NOMB"))),
                                CVELOGIN = (reader.IsDBNull(reader.GetOrdinal("CVELOGIN")) ? "NULL" : reader.GetString(reader.GetOrdinal("CVELOGIN"))),
                                NIVC = (reader.IsDBNull(reader.GetOrdinal("NIVC")) ? 0 : reader.GetInt16(reader.GetOrdinal("NIVC"))),
                                NPERI = (reader.IsDBNull(reader.GetOrdinal("NPERI")) ? 0 : reader.GetInt16(reader.GetOrdinal("NPERI"))),
                                FALT = (reader.IsDBNull(reader.GetOrdinal("FALT")) ? DateTime.Now : reader.GetDateTime(reader.GetOrdinal("FALT"))),
                                FBAJ = (reader.IsDBNull(reader.GetOrdinal("FBAJ")) ? DateTime.Now : reader.GetDateTime(reader.GetOrdinal("FBAJ"))),
                                URL = (reader.IsDBNull(reader.GetOrdinal("URL")) ? "NULL" : reader.GetString(reader.GetOrdinal("URL"))),
                                IURL = (reader.IsDBNull(reader.GetOrdinal("IURL")) ? "NULL" : reader.GetString(reader.GetOrdinal("IURL"))),
                                NCUR = (reader.IsDBNull(reader.GetOrdinal("NCUR")) ? 0 : reader.GetInt16(reader.GetOrdinal("NCUR"))),
                                NMOT = (reader.IsDBNull(reader.GetOrdinal("NMOT")) ? 0 : reader.GetInt16(reader.GetOrdinal("NMOT"))),
                                IACTREAL = (reader.IsDBNull(reader.GetOrdinal("IACTREAL")) ? "NULL" : reader.GetString(reader.GetOrdinal("IACTREAL"))),
                                NNIV = (reader.IsDBNull(reader.GetOrdinal("NNIV")) ? 0 : reader.GetInt16(reader.GetOrdinal("NNIV"))),
                                NPROF = (reader.IsDBNull(reader.GetOrdinal("NPROF")) ? 0 : reader.GetInt16(reader.GetOrdinal("NPROF"))),
                                NOTORGA = (reader.IsDBNull(reader.GetOrdinal("NOTORGA")) ? 0 : reader.GetInt16(reader.GetOrdinal("NOTORGA"))),
                                HRS = (reader.IsDBNull(reader.GetOrdinal("HRS")) ? 0 : (Single)reader.GetDouble(reader.GetOrdinal("HRS"))),
                                NDPAG = (reader.IsDBNull(reader.GetOrdinal("NDPAG")) ? 0 : reader.GetInt32(reader.GetOrdinal("NDPAG"))),
                                DDEPAG = (reader.IsDBNull(reader.GetOrdinal("DDEPAG")) ? "NULL" : reader.GetString(reader.GetOrdinal("DDEPAG"))),
                                NSUBPAG = (reader.IsDBNull(reader.GetOrdinal("NSUBPAG")) ? 0 : reader.GetInt32(reader.GetOrdinal("NSUBPAG"))),
                                DSUBPAG = (reader.IsDBNull(reader.GetOrdinal("DSUBPAG")) ? "NULL" : reader.GetString(reader.GetOrdinal("DSUBPAG"))),
                            });
                        }

                        reader.Dispose();
                    }

                    seguridad.RevocarSeguridad(odbConn);
                }
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, ex.Message);
            }
            finally
            {
                odbConn.Dispose();
            }

            var regreso = new BaseUsuarioOracleViewModel()
            {
                Encabezado = columns,
                Tipos = typesColumns,
                Contenido = valores
            };

            return regreso;
        }
        public async Task<InfoUsuarioOracleViewModel> ObtieneInfoUsuarioOracleAsync(string usuario)
        {
            //--Contenedor
            InfoUsuarioOracleViewModel info = null;
            //---------------------------------------
            var lsConexion = _configuration.GetConnectionString("OracleConnection");
            string consulta = string.Format("{0} {1} {2} {3}", 
                                "SELECT",
                                "PIDM,NOPER,SEXO,STATUS,CVELOGIN,NOMBRE,APAT,AMAT,NOMB,CORREO,LADA,TELEF_DEP,D_DIREC1,D_CP,D_CIUDAD,ESTADO,NTPE,TIPO_PER,NPUE,DPUE,NCAT,CATEGORIA,NDEP,DDEP,NZON,DZON",
                                "FROM DATPERS",
                                "WHERE CVELOGIN=:CVELOGIN");

            Permiso seguridad = new Permiso();
            OracleConnection odbConn = new OracleConnection(lsConexion);
            var valores = new List<InfoUsuarioGeneralOracleViewModel>();
            int verificarSeguridad = 1;
            //--
            try
            {
                odbConn.Open();

                verificarSeguridad = seguridad.VerificarSeguridad("HWIPDOC", odbConn);

                if (verificarSeguridad == 0)
                {
                    using (OracleCommand command = new OracleCommand(consulta, odbConn))
                    {
                        OracleParameter id = new OracleParameter("id", usuario);
                                        command.Parameters.Add(id);
                        OracleDataReader reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            valores.Add(new InfoUsuarioGeneralOracleViewModel()
                            {
                                NOPER = (reader.IsDBNull(reader.GetOrdinal("NOPER")) ? 0 : reader.GetInt32(reader.GetOrdinal("NOPER"))),
                                NOMBRE = (reader.IsDBNull(reader.GetOrdinal("NOMBRE")) ? string.Empty : reader.GetString(reader.GetOrdinal("NOMBRE"))),
                                SEXO = (reader.IsDBNull(reader.GetOrdinal("SEXO")) ? string.Empty : reader.GetString(reader.GetOrdinal("SEXO"))),
                                PIDM = (reader.IsDBNull(reader.GetOrdinal("PIDM")) ? 0 : reader.GetInt32(reader.GetOrdinal("PIDM"))),
                                ESTADO = (reader.IsDBNull(reader.GetOrdinal("ESTADO")) ? string.Empty : reader.GetString(reader.GetOrdinal("ESTADO"))),
                                NTPE = (reader.IsDBNull(reader.GetOrdinal("NTPE")) ? 0 : reader.GetInt16(reader.GetOrdinal("NTPE"))),
                                TIPO_PER = (reader.IsDBNull(reader.GetOrdinal("TIPO_PER")) ? string.Empty : reader.GetString(reader.GetOrdinal("TIPO_PER"))),
                                NPUE = (reader.IsDBNull(reader.GetOrdinal("NPUE")) ? 0 : reader.GetInt32(reader.GetOrdinal("NPUE"))),
                                DPUE = (reader.IsDBNull(reader.GetOrdinal("DPUE")) ? string.Empty : reader.GetString(reader.GetOrdinal("DPUE"))),
                                NCAT = (reader.IsDBNull(reader.GetOrdinal("NCAT")) ? 0 : reader.GetInt32(reader.GetOrdinal("NCAT"))),
                                CATEGORIA = (reader.IsDBNull(reader.GetOrdinal("CATEGORIA")) ? string.Empty : reader.GetString(reader.GetOrdinal("CATEGORIA"))),
                                NDEP = (reader.IsDBNull(reader.GetOrdinal("NDEP")) ? 0 : reader.GetInt32(reader.GetOrdinal("NDEP"))),
                                DDEP = (reader.IsDBNull(reader.GetOrdinal("DDEP")) ? string.Empty : reader.GetString(reader.GetOrdinal("DDEP"))),
                                CORREO = (reader.IsDBNull(reader.GetOrdinal("CORREO")) ? string.Empty : reader.GetString(reader.GetOrdinal("CORREO"))),
                                STATUS = (reader.IsDBNull(reader.GetOrdinal("STATUS")) ? string.Empty : reader.GetString(reader.GetOrdinal("STATUS"))),
                                LADA = (reader.IsDBNull(reader.GetOrdinal("LADA")) ? string.Empty : reader.GetString(reader.GetOrdinal("LADA"))),
                                TELEF_DEP = (reader.IsDBNull(reader.GetOrdinal("TELEF_DEP")) ? string.Empty : reader.GetString(reader.GetOrdinal("TELEF_DEP"))),
                                D_DIREC1 = (reader.IsDBNull(reader.GetOrdinal("D_DIREC1")) ? string.Empty : reader.GetString(reader.GetOrdinal("D_DIREC1"))),
                                D_CIUDAD = (reader.IsDBNull(reader.GetOrdinal("D_CIUDAD")) ? string.Empty : reader.GetString(reader.GetOrdinal("D_CIUDAD"))),
                                D_CP = (reader.IsDBNull(reader.GetOrdinal("D_CP")) ? string.Empty : reader.GetString(reader.GetOrdinal("D_CP"))),
                                NZON = (reader.IsDBNull(reader.GetOrdinal("NZON")) ? 0 : reader.GetInt16(reader.GetOrdinal("NZON"))),
                                DZON = (reader.IsDBNull(reader.GetOrdinal("DZON")) ? string.Empty : reader.GetString(reader.GetOrdinal("DZON"))),
                                APAT = (reader.IsDBNull(reader.GetOrdinal("APAT")) ? string.Empty : reader.GetString(reader.GetOrdinal("APAT"))),
                                AMAT = (reader.IsDBNull(reader.GetOrdinal("AMAT")) ? string.Empty : reader.GetString(reader.GetOrdinal("AMAT"))),
                                NOMB = (reader.IsDBNull(reader.GetOrdinal("NOMB")) ? string.Empty : reader.GetString(reader.GetOrdinal("NOMB"))),
                                CVELOGIN = (reader.IsDBNull(reader.GetOrdinal("CVELOGIN")) ? string.Empty : reader.GetString(reader.GetOrdinal("CVELOGIN")))
                            });
                        }

                        reader.Dispose();
                    }

                    seguridad.RevocarSeguridad(odbConn);
                    verificarSeguridad = 1;
                }
            }
            catch (Exception ex)
            {
                if (verificarSeguridad == 0) 
                {
                    seguridad.RevocarSeguridad(odbConn);
                    verificarSeguridad = 1;
                }

                _logger.Log(LogLevel.Error, ex.Message.ToString());
            }
            finally
            {
                if (verificarSeguridad == 0)
                {
                    seguridad.RevocarSeguridad(odbConn);
                }

                odbConn.Dispose();
            }
            //------
            if (valores.Count > 0)
            {
                //var info1 = valores.Select(x => x.NDEP.ToString()).Distinct().ToList();
                //var info2 = valores.Select(x => x.NPUE.ToString()).Distinct().ToList();
                //TextInfo myTI = _cultureEs.TextInfo;

                info = new InfoUsuarioOracleViewModel
                {
                    Usuario = valores.First().CVELOGIN.Trim(),
                    Nombre = string.Format("{0} {1} {2}", valores.First().NOMB, valores.First().APAT, valores.First().AMAT),
                    Correo = valores.First().CORREO.Trim(),
                    Puesto = valores.First().DPUE.Trim(),
                    //--
                    Areas = await _areaService.ObtenerAreasConSusHijasPorClaveAsync(valores.Select(x => x.NDEP.ToString()).Distinct().ToList())
                };
            }

            return info;
        }
    }
}

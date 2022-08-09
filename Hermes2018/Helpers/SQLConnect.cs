using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;


namespace Hermes2018 {
    public static class Extensions
    {
        public static Boolean NoEsVacio(this Object str)
        {
            bool lleno = false;
            if (DBNull.Value != str)
            {
                if (str.ToString() != "")
                    lleno = true;
            }
            return lleno;
        }


        public static String ToComilla(this String str)
        {
            return str.Replace("'", "");
        }

        public static Boolean TieneValor(this String str)
        {
            bool esVacio = true;
            if (str == null || str == "")
                esVacio = false;
            return esVacio;
        }



        public static String[] nombreDia = { "Lunes", "Martes", "Miercoles", "Jueves", "Viernes", "Sábado", "Domingo" };
        public static String[] nombreDia2 = { "Domingo", "Lunes", "Martes", "Miercoles", "Jueves", "Viernes", "Sábado" };
        public static String[] nombreMes = { "Ene", "Feb", "Mar", "Abr", "May", "Jun", "Jul", "Ago", "Sep", "Oct", "Nov", "Dic" };
        public static String[] nombreMesCompleto = { "Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre" };

        public static String ToDateHMS(this DateTime fecha)
        {
            return fecha.ToString("HH:mm:ss tt");
        }
        public static String ToDateDMA(this DateTime fecha)
        {
            return fecha.Day.ToString().PadLeft(2, '0') + "/" + nombreMes[fecha.Month - 1] + "/" + fecha.Year.ToString();
        }

        public static String ToDateDMAEspacio(this DateTime fecha)
        {
            return fecha.Day.ToString().PadLeft(2, '0') + " " + nombreMes[fecha.Month - 1].ToLower() + " " + fecha.Year.ToString();
        }

        public static String ToDateDMANum(this DateTime fecha)
        {
            return fecha.Day.ToString().PadLeft(2, '0') + "/" + fecha.Month.ToString().PadLeft(2, '0') + "/" + fecha.Year.ToString();
        }


        public static String ToDateDMAService(this String fecha)
        {
            string[] fe = fecha.Split('/');
            string mes = "";
            switch (fe[1].ToString())
            {
                case "Ene":
                    mes = "01";
                    break;
                case "Feb":
                    mes = "02";
                    break;
                case "Mar":
                    mes = "03";
                    break;
                case "Abr":
                    mes = "04";
                    break;
                case "May":
                    mes = "05";
                    break;
                case "Jun":
                    mes = "06";
                    break;
                case "Jul":
                    mes = "07";
                    break;
                case "Ago":
                    mes = "08";
                    break;
                case "Sep":
                    mes = "09";
                    break;
                case "Oct":
                    mes = "10";
                    break;
                case "Nov":
                    mes = "11";
                    break;
                case "Dic":
                    mes = "12";
                    break;
            }
            return fe[2] + "/" + mes + "/" + fe[0];
        }

        public static String ToDateDDMMAAAA(this DateTime fecha)
        {
            return fecha.Day.ToString().PadLeft(2, '0') + "/" + (fecha.Month).ToString().PadLeft(2, '0') + "/" + fecha.Year.ToString();
        }
        public static String ToDateYMD(this DateTime fecha)
        {
            return fecha.Year.ToString() + "-" + (fecha.Month).ToString().PadLeft(2, '0') + "-" + fecha.Day.ToString().PadLeft(2, '0');
        }

        public static String ToDateYMDDiagonal(this DateTime fecha)
        {
            return fecha.Year.ToString() + "/" + (fecha.Month).ToString().PadLeft(2, '0') + "/" + fecha.Day.ToString().PadLeft(2, '0');
        }


        public static String ToURLEncode(this String str)
        {
            return str.Replace("-", "____").Replace("/", "_").Replace("+", "__").Replace(" ", "-").Replace("'", "___").Replace(".", "--").Replace(":", "=").ToLower().ToLower();
        }

        public static String ToURLDecode(this String str)
        {
            return str.Replace("=", ":").Replace("--", ".").Replace("-", " ").Replace("____", "-").Replace("___", "'").Replace("__", "+").Replace("_", "/");
        }
        /// <summary>
        /// Método que convierte un datetime al formato 2015-12-25
        /// </summary>
        /// <param name="fecha"></param>
        /// <returns></returns>
        public static String ToDateAMD(this DateTime fecha)
        {
            return fecha.Year.ToString() + "-" + fecha.Month.ToString().PadLeft(2, '0') + "-" + fecha.Day.ToString().PadLeft(2, '0');
        }



        public static String ToDateAMDHMS(this DateTime fecha)
        {
            return fecha.Year.ToString() + "-" + fecha.Month.ToString().PadLeft(2, '0') + "-" + fecha.Day.ToString().PadLeft(2, '0') + "T" + fecha.ToString("HH:mm:ss");
        }
        public static String ToDateAMDHMSJunto(this DateTime fecha)
        {
            return fecha.Year.ToString() + fecha.Month.ToString().PadLeft(2, '0') + fecha.Day.ToString().PadLeft(2, '0') + "T" + fecha.ToString("HH:mm:ss");
        }

        public static String ToMMDDYYY(this DateTime fecha)
        {
            return fecha.Month.ToString().PadLeft(2, '0') + "-" + fecha.Day.ToString().PadLeft(2, '0') + "-" + fecha.Year.ToString();
        }

        public static String ToYMD(this DateTime fecha)
        {
            return fecha.Year.ToString() + "-" + fecha.Month.ToString().PadLeft(2, '0') + "-" + fecha.Day.ToString().PadLeft(2, '0');
        }


        public static String ToDateDMAHora(this DateTime fecha)
        {
            return fecha.Day.ToString().PadLeft(2, '0') + "/" + nombreMes[fecha.Month - 1] + "/" + fecha.Year.ToString() + " " + fecha.ToString("HH:mm");
        }


        public static String ToDateNombreArchivo(this DateTime fecha)
        {
            return fecha.Day.ToString().PadLeft(2, '0') + "_" + fecha.Month.ToString().PadLeft(2, '0') + "_" + fecha.Year.ToString() + "_" + fecha.ToString("HH_mm_ss");
        }


        public static String ToFechaNombre(this DateTime fecha)
        {
            return nombreDia2[Convert.ToInt32(fecha.DayOfWeek)] + " " + fecha.Day.ToString().PadLeft(2, '0') + " de " + nombreMesCompleto[fecha.Month - 1] + " de " + fecha.Year.ToString();
        }

        public static String ToDateDMA24Hora(this DateTime fecha)
        {
            return fecha.Year.ToString() + "-" + fecha.Month.ToString().PadLeft(2, '0') + "-" + fecha.Day.ToString().PadLeft(2, '0') + " " + fecha.ToString("HH:mm:ss");
        }

        public static String ToDateHM(this DateTime fecha)
        {
            return fecha.ToString("HH:mm");
        }

        public static String ToDate12Horas(this DateTime fecha)
        {
            return fecha.ToString("hh:mm tt");
        }

        public static String ToHM24Hora(this DateTime fecha)
        {
            return fecha.ToString("HH:mm:ss");
        }


        public static String ToDateDMA(this DateTime? fe)
        {
            DateTime fecha = Convert.ToDateTime(fe);
            return fecha.Day.ToString().PadLeft(2, '0') + "/" + nombreMes[fecha.Month - 1] + "/" + fecha.Year.ToString();
        }

        public static String ToDateEspaniol(this DateTime fecha)
        {
            return fecha.Day.ToString().PadLeft(2, '0') + " de " + nombreMes[fecha.Month - 1] + " de " + fecha.Year.ToString();
        }

        public static String ToDateEspaniolCompleto(this DateTime fecha)
        {
            return fecha.Day.ToString().PadLeft(2, '0') + " de " + nombreMesCompleto[fecha.Month - 1] + " de " + fecha.Year.ToString();
        }
        /// <summary>
        /// Convierte la cadena de fecha en formato 01/Ene/2014.
        /// </summary>
        /// <param name="fecha">Fecha en formato de cadena.</param>
        /// <returns></returns>
        public static String ToDMA(this String fecha)
        {
            DateTime f = Convert.ToDateTime(fecha);
            return f.Day.ToString().PadLeft(2, '0') + "/" + nombreMes[f.Month - 1] + "/" + f.Year.ToString();
        }

        public static String Precio(this Double precio)
        {
            return "$" + String.Format(new System.Globalization.CultureInfo("es-MX"), "{0:0,0.00}", precio); //precio.ToString("###,###,###.##")
        }


        public static String PrecioSinSigno(this Double precio)
        {
            return String.Format(new System.Globalization.CultureInfo("es-MX"), "{0:0,0.00}", precio); //precio.ToString("###,###,###.##")
        }

        public static Double PrecioAbsoluto(this String precio)
        {
            if (precio == "---")
                return 0;
            else
                return Convert.ToDouble(precio.Replace("$", "").Replace(",", ""));
        }

        public static String PrecioCompleto(this Double? precio)
        {
            if (precio == null)
                return "---";
            else
                return "$" + String.Format(new System.Globalization.CultureInfo("es-MX"), "{0:0,0.00}", precio); //precio.ToString("###,###,###.##");
        }

        public static String PrecioFormateado(this Double precio)
        {
            return "$" + String.Format(new System.Globalization.CultureInfo("es-MX"), "{0:0,0.00}", precio); //precio.ToString("###,###,###.##");
        }

        public static String UrlFriendly(this String url)//friendly URl in asp.net
        {

            string urlFiendly = url.Trim().ToLower();
            urlFiendly = urlFiendly.Replace("    ", "");
            urlFiendly = urlFiendly.Replace("  ", " ");
            urlFiendly = urlFiendly.Replace("  ", " ");
            urlFiendly = urlFiendly.Replace(" ", "-");
            urlFiendly = urlFiendly.Replace("ñ", "n");
            urlFiendly = urlFiendly.Replace("á", "a");
            urlFiendly = urlFiendly.Replace("é", "e");
            urlFiendly = urlFiendly.Replace("í", "i");
            urlFiendly = urlFiendly.Replace("ó", "o");
            urlFiendly = urlFiendly.Replace("ú", "u");
            urlFiendly = urlFiendly.Replace("á", "a");
            urlFiendly = urlFiendly.Replace("ü", "u");
            urlFiendly = urlFiendly.Replace("+", "-");
            urlFiendly = urlFiendly.Replace(".", "_");
            return urlFiendly;
        }

        public static String ReturnUrlFriendly(this String url)//friendly URl in asp.net
        {

            string urlFiendly = url.Trim().ToLower();
            urlFiendly = urlFiendly.Replace("_", ".");
            urlFiendly = urlFiendly.Replace("-", " ");

            return urlFiendly;
        }

        public static String UrlFriendlySKU(this String url)//friendly URl in asp.net
        {

            string urlFiendly = url.Trim().ToLower();
            urlFiendly = urlFiendly.Replace(".", "__");
            urlFiendly = urlFiendly.Replace("-", "_");
            return urlFiendly;
        }

        public static String ReturnSKU(this String url)//friendly URl in asp.net
        {

            string urlFiendly = url.Trim().ToLower();
            urlFiendly = urlFiendly.Replace("__", ".");
            urlFiendly = urlFiendly.Replace("_", "-");

            return urlFiendly;
        }
    }




}

namespace Hermes2018.Helpers
{
    public class RequestClientApi
    {

        public string Get(String urlApiREST)
        {
            var request = (HttpWebRequest)WebRequest.Create(urlApiREST);
            request.Method = "GET";
            request.ContentType = "application/json";
            request.Accept = "application/json";
            string returnJson = "";
            try
            {
                using (WebResponse response = request.GetResponse())
                {
                    using (Stream strReader = response.GetResponseStream())
                    {
                        if (strReader == null) returnJson = "";
                        else
                        {

                            using (StreamReader objReader = new StreamReader(strReader))
                                returnJson = objReader.ReadToEnd();
                        }
                    }
                }
            }
            catch (WebException ex)
            {
                //Utileria.escribeLog(ex + "");
            }
            return returnJson;
        }

        public string Post(String urlApiREST, string dataJsonParameters, string token)
        {

            var request = (HttpWebRequest)WebRequest.Create(urlApiREST);
            request.Method = "POST";
            request.ContentType = "application/json";
            if (token != "")
            {
                request.Headers.Add("Authorization", "Bearer " + token);
                request.Headers.Add("Access-Control-Allow-Origin", "*");
            }
            request.Accept = "application/json";


            request.Timeout = 2000;
            using (var streamWriter = new StreamWriter(request.GetRequestStream()))
            {
                streamWriter.Write(dataJsonParameters);
                streamWriter.Flush();
                streamWriter.Close();
            }
            string returnJson = "";
            try
            {
                using (WebResponse response = request.GetResponse())
                {
                    using (Stream strReader = response.GetResponseStream())
                    {
                        if (strReader == null) returnJson = "";
                        else
                        {
                            using (StreamReader objReader = new StreamReader(strReader))
                                returnJson = objReader.ReadToEnd();
                        }
                    }
                }
            }
            catch (WebException ex)
            {
                //Utileria.escribeLog(ex + "");
            }
            return returnJson;
        }

        public RequestClientApi() { }
    }
    public class SalidaSp
    {

        public Int32 Estado { get; set; }
        public String Resultado { get; set; }

        public SalidaSp()
        {
        }

    }

    public class LoginTP {

        public int iNumPer { get; set; }
        public string sTipPer { get; set; }

        public List<TipoPersonalCustom> TipoPersonal {
            get {
                List<TipoPersonalCustom> tipos = new List<TipoPersonalCustom>();
    
                string[] tiposP = sTipPer.Split(",");
                string[] tipoValue = null;
                TipoPersonalCustom tip = null;
                foreach (var data in tiposP) {
                    tipoValue = data.Split("-");
                    tip = new TipoPersonalCustom();
                    tip.Id = Convert.ToInt32(tipoValue[0].Trim());
                    tip.TipoPersonal = Convert.ToString(tipoValue[1].Trim()) ;
                    tipos.Add(tip);
                }
                return tipos;
            }
        }
        public LoginTP() { }
    }



    public class UsrToken {
        public String sTokenUV { get; set; }
        public String sTokenMS { get; set; }
        public String dtExpira { get; set; }
        public String sUsuario { get; set; }
        public String sError { get; set; }
        public String sClave { get; set; }
        public UsrToken() { }
    }
    public class LoginSPRH {
        public LoginTP UsrToken { get; set; }
        public LoginSPRH() { }
    }

    public class TipoPersonalCustom {

        public Int32 Id{get;set;}
        public String TipoPersonal { get; set; }
        public TipoPersonalCustom() { }
    }

    public class JsonLogin
    {
        public LoginTP oLoginTP { get; set; }
        public JsonLogin() { }
    }

    public class RestApiDSIA
    {
        public IConfigurationRoot GetConfiguration()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            return builder.Build();
        }
        public string ObtieneServMed(int numPer, int tipoPer)
        {
            var configuation = GetConfiguration();
            var dataBearer = configuation.GetSection("ConfigureCustom").GetSection("Bearer").Value;

            var jsonRaw = new
            {
                sNumPer = numPer.ToString(),
                sNumTipoPer = tipoPer.ToString(),
                sClave = configuation.GetSection("ConfigureCustom").GetSection("sClave").Value
            };
            return new RequestClientApi().Post(configuation.GetSection("ServicesAPIDSIA").GetSection("ObtieneServMed").Value, JsonConvert.SerializeObject(jsonRaw), dataBearer);
        }

        public string GetConnection() {
            var configuation = GetConfiguration();
            return configuation.GetSection("ConnectionStrings").GetSection("DefaultConnection").Value;

        }

        public string ObtieneServMedDep(int numPer, int tipoPer)
        {
            var configuation = GetConfiguration();
            var dataBearer = configuation.GetSection("ConfigureCustom").GetSection("Bearer").Value;

            var jsonRaw = new
            {
                sNumPer = numPer.ToString(),
                sNumTipoPer = tipoPer.ToString(),
                sClave = configuation.GetSection("ConfigureCustom").GetSection("sClave").Value
            };
            return new RequestClientApi().Post(configuation.GetSection("ServicesAPIDSIA").GetSection("ObtieneServMedDep").Value, JsonConvert.SerializeObject(jsonRaw), dataBearer);
        
        }

        public string ObtieneLogin2(string sUserId, string sPass)
        {
            var configuation = GetConfiguration();
            var dataBearer = configuation.GetSection("ConfigureCustom").GetSection("Bearer").Value;

            var jsonRaw = new
            {
                sUserId = sUserId,
                sPass = sPass,
                sClave = configuation.GetSection("ConfigureCustom").GetSection("sClaveLogin").Value
            };
            return new RequestClientApi().Post(configuation.GetSection("ServicesAPIDSIA").GetSection("Login2").Value, JsonConvert.SerializeObject(jsonRaw), dataBearer);
        }

        public int TipoPersonal { get; set; }

        public RestApiDSIA()
        {
        }

    }



    public class SearchCatalog
    {
        public string UsuarioId { get; set; }

        public int NumPagina { get; set; }
        public int Id { get; set; }
        public SearchCatalog()
        {
        }

    }

    public class FiltersConstancias
    {
        public int Pagina { get; set; }
        public int ConstanciaId { get; set; }

        public int EstadoId { get; set; }

        public int CampusId { get; set; }

        public int NoPersonal { get; set; }

        public int TipoPersonal { get; set; }

        public string Folio { get; set; }

        public string Dependencia { get; set; }

        public string FechaInicio { get; set; }
        public string FechaFinal { get; set; }
        public string Busqueda { get; set; }

        public FiltersConstancias()
        {
        }

    }

    public class CustomConstancias
    {
        public int NumPersonal { get; set; }

        public int TipoPersonal { get; set; }
        public string sCveLogin { get; set; }
        public string UserId { get; set; }
        public CustomConstancias()
        {
        }

    }
    public class SQLConnect
    {
        private SqlConnection conexion;

        public IConfigurationRoot GetConfiguration()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            return builder.Build();
        }

        public SQLConnect()
        {
            var configuation = GetConfiguration();
            conexion = new SqlConnection(configuation.GetSection("ConnectionStrings").GetSection("DefaultConnection").Value);
        }

        private System.Data.SqlClient.SqlParameter parametroResultado()
        {
            System.Data.SqlClient.SqlParameter a = new System.Data.SqlClient.SqlParameter();
            a.SqlDbType = SqlDbType.NVarChar;
            a.ParameterName = "@salida";
            a.Value = "0";
            a.Size = 2000;
            a.Direction = ParameterDirection.Output;
            return a;
        }

        private SqlCommand getSqlCommand(String sql)
        {
            return new SqlCommand(sql, conexion);
        }

        public SqlParameter estadoSalida()
        {
            SqlParameter estado_sp = new SqlParameter("@estado", SqlDbType.Int);
            estado_sp.Direction = ParameterDirection.Output;
            estado_sp.Value = 0;
            return estado_sp;
        }

        /// <summary>
        /// Método que ejecuta una sentencia SQL
        /// </summary>
        /// <param name="sql">Sentencia SQL que se ejecutará</param>
        /// <returns>Regresa los registros que coincida con los parámetros o la sentencia sql proporcionada</returns>

        public DataTable executeDataTable(String sql)
        {
            return this.executeDataTable(sql, new List<SqlParameter>());
        }

        /// <summary>
        /// Metódo que ejecuta una sentencia SQL o un Stored Procedure para obtener registros de la BD
        /// </summary>
        /// <param name="sql">Sentencia SQL o Stored Procedure</param>
        /// <param name="tipo">Tipo de <c>CommandType</c> que se ejecutara, puede ser in Text o un StoredProcedure</param>
        /// <param name="parametros">Lista de parámetros con el que se envía la sentencia SQL o el Stored Procedure</param>
        /// <returns>Regresa los registros que coincida con los parámetros o la sentencia sql proporcionada</returns>

        public DataTable executeDataTable(String sql, CommandType tipo, List<SqlParameter> parametros)
        {
            conexion.Open();
            SqlCommand cmd = this.getSqlCommand(sql);
            cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["CommandTimeoutBD"]);//3600
            cmd.CommandType = tipo;



            if (parametros != null)
            {
                foreach (SqlParameter param in parametros)
                    cmd.Parameters.Add(param);
            }
            //SqlDataReader reader = cmd.ExecuteReader();

            DataTable tabla = new DataTable();
            tabla.Load(cmd.ExecuteReader());
            cerrarConexion();
            return tabla;
        }

        /// <summary>
        /// Método que recibe 3 parámetros para obtener información de la BD
        /// </summary>
        /// <param name="sql">Sentencia SQL o nombre del procedimiento alamacenado que se ejecutará</param>
        /// <param name="tipo">Tipo de <c>CommandType</c> que debe ejecutar, ya sea un Texto o un StoredProcedure</param>
        /// <param name="parametros">Lista de parámetros que recibe la sentencia SQL o el Stored Procedure</param>
        /// <returns></returns>

        public DataSet executeDataSet(String sql, CommandType tipo, List<SqlParameter> parametros)
        {
            SqlCommand cmd = this.getSqlCommand(sql);
            cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["CommandTimeoutBD"]);//3600
            cmd.CommandType = tipo;


            foreach (SqlParameter param in parametros)
                cmd.Parameters.Add(param);


            DataSet dsRet = new DataSet();
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);

            adapter.Fill(dsRet);

            return dsRet;
        }

        /// <summary>
        /// Método que ejecuta una sentencia Sql con una lista de parámetros
        /// </summary>
        /// <param name="sql">Sentencia SQL que el método debe ejecutar</param>
        /// <param name="parametros">Lista de parámetros que recibe la sentencia SQL</param>
        /// <returns></returns>

        public DataTable executeDataTable(String sql, List<SqlParameter> parametros)
        {
            return this.executeDataTable(sql, CommandType.Text, parametros);
        }

        public String executeNonQueryCadena(string sql, CommandType tipo, List<SqlParameter> parametros)
        {
            conexion.Open();
            SqlCommand cmd = this.getSqlCommand(sql);
            cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["CommandTimeoutBD"]);//3600
            cmd.CommandType = tipo;

            if (parametros != null)
            {
                foreach (SqlParameter param in parametros)
                    cmd.Parameters.Add(param);
            }
            String resultado = String.Empty;
            try
            {
                cmd.ExecuteNonQuery();
                resultado = cmd.Parameters["@estado_sp"].Value.ToString();
            }
            catch (SqlException s)
            {
                resultado = String.Empty;
                throw s;
            }
            finally
            {
                cerrarConexion();
            }

            return resultado;
        }

        private SalidaSp executeNonQuery(string sql, CommandType tipo, List<SqlParameter> parametros, Boolean esInsercion)
        {
            conexion.Open();
            SqlCommand cmd = this.getSqlCommand(sql);
            cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["CommandTimeoutBD"]);//3600
            cmd.CommandType = tipo;
            SalidaSp salidaSp = null;
            if (parametros != null)
            {
                foreach (SqlParameter param in parametros)
                    cmd.Parameters.Add(param);

                if (esInsercion)
                    cmd.Parameters.Add(parametroResultado());
            }
            try
            {
                //cmd.ExecuteNonQuery();
                //resultado = Convert.ToInt32(cmd.Parameters["@estado_sp"].Value);
                salidaSp = new SalidaSp();
                cmd.ExecuteNonQuery();
                salidaSp.Estado = Convert.ToInt32(cmd.Parameters["@estado"].Value);
                if (esInsercion)
                    salidaSp.Resultado = Convert.ToString(cmd.Parameters["@salida"].Value);
                cmd.Dispose();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (this.conexion.State == ConnectionState.Open)
                    this.conexion.Close();
                if (cmd != null)
                    cmd.Dispose();
            }
            return salidaSp;
        }

        private void cerrarConexion()
        {
            //this.conexion.Dispose();
            this.conexion.Close();
        }

        public SalidaSp ExecuteNonQuery(string sql, CommandType tipo, List<SqlParameter> parametros, Boolean esInsercion = false)
        {
            return this.executeNonQuery(sql, tipo, parametros, esInsercion);
        }

        public String executeNonQueryCadena(string sql)
        {
            return this.executeNonQueryCadena(sql, CommandType.Text, null);
        }
    }

}

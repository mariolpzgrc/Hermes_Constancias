using Hermes2018.Data;
using Hermes2018.Helpers;
using Hermes2018.Models;
using Hermes2018.ModelsDBF;
using Hermes2018.Services.Interfaces;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using HER_Constancias = Hermes2018.Models.Constancia.HER_Constancias;
using HER_TipoPersonalConstancia = Hermes2018.Models.Constancia.HER_TipoPersonalConstancia;

namespace Hermes2018.Services
{

    public class ConstanciaService : IConstanciaService
    {
        private readonly ApplicationDbContext _bdContext;
        private readonly ILogger<ConstanciaService> _logger;

        public ConstanciaService(ApplicationDbContext context,
                                ILogger<ConstanciaService> logger)
        {
            _bdContext = context;
            _logger = logger;
        }

        public List<HER_TipoPersonalConstancia> Get_HER_TipoPersonalConstancia()
        {
            return new HER_TipoPersonalConstancia().Get_HER_TipoPersonalConstancia();
        }

        public List<HER_Constancias> Get_HER_Constancias(int tipoPersonal, string usuarioId)
        {
            return new HER_Constancias().Get_HER_Constancias(tipoPersonal, usuarioId);
        }

        public ListaPaginador<SolicitudConstanciaCustom> GET_FiltersConstancias(int constanciaId, string fechaI, string fechaT, int estadoId, string busqueda, int campusId, int noPersonal, string folio, string dependencia, int tipoPersonal, int pagina)
        {
            var user = new List<Models.HER_InfoUsuario>(_bdContext.HER_InfoUsuario);

            List<ModelsDBF.HER_SolicitudConstancia> costancias = new List<ModelsDBF.HER_SolicitudConstancia>();
            using (ModelsDBF.DBFContext _bd = new ModelsDBF.DBFContext())
            {
                costancias = _bd.HER_SolicitudConstancia.ToList();
                if (constanciaId > 0)
                    costancias = costancias.Where(x => x.ConstanciaId == constanciaId).ToList();


                if (fechaI.TieneValor() && fechaT.TieneValor())
                {
                    DateTime fI = Convert.ToDateTime(fechaI + " 00:00:00");
                    DateTime fT = Convert.ToDateTime(fechaT + " 23:59:59");
                    costancias = costancias.Where(x => x.FechaSolicitud >= fI && x.FechaSolicitud <= fT).ToList();
                }

                if (estadoId > 0)
                    costancias = costancias.Where(x => x.EstadoId == estadoId).ToList();
                if (campusId > 0)
                    costancias = costancias.Where(x => x.CampusId == campusId).ToList();
                if (folio.TieneValor())
                    costancias = costancias.Where(x => x.Folio == folio).ToList();
                if (noPersonal > 0)
                    costancias = costancias.Where(x => x.NoPersonal == noPersonal).ToList();
                if (dependencia.TieneValor())
                    costancias = costancias.Where(x => x.NombreDep.Contains(dependencia)).ToList();

                if (tipoPersonal > 0)
                    costancias = costancias.Where(x => x.TipoPersonal == tipoPersonal).ToList();
            }

            int[] idsConstancia = costancias.Select(x => x.Id).ToArray();

            Models.ListaPaginador<Models.SolicitudConstanciaCustom> listaPaginador = new Models.ListaPaginador<Models.SolicitudConstanciaCustom>();

            IEnumerable<SolicitudConstanciaCustom> _dataQuery;

            int totalRegistros = 0;

            using (ModelsDBF.DBFContext _bd = new ModelsDBF.DBFContext())
            {

                var _data_Full = (from b in _bd.HER_SolicitudConstancia
                                  join e in _bd.HER_EstadoConstancias on b.EstadoId equals e.Id
                                  join u in _bd.HER_Constancias on b.ConstanciaId equals u.Id
                                  join uu in user on b.UsuarioId equals uu.HER_UserName
                                  where (idsConstancia.Contains((int)b.Id))
                                  select new Models.SolicitudConstanciaCustom
                                  {
                                      Id = b.Id,
                                      ConstanciaId = b.ConstanciaId,
                                      UsuarioId = b.UsuarioId,
                                      NoPersonal = b.NoPersonal,
                                      FechaSolicitud = b.FechaSolicitud,
                                      EstadoId = b.EstadoId,
                                      Folio = b.Folio,
                                      Mensaje = b.Mensaje,
                                      NombreConstancia = u.Nombre,
                                      NombreEstado = e.Nombre,
                                      CveDep = b.CveDep,
                                      Dependencia = b.NombreDep,
                                      TipoPersonal = b.TipoPersonal,
                                      NombreUsuario = uu.HER_NombreCompleto,
                                      CampusId = b.CampusId,
                                      NombreCampus = b.NombreCampus,
                                      EstadosConstancia = (from e in _bd.HER_SolicitudConstanciaEstado where e.SolicitudConstanciaId == b.Id select e).ToList()
                                  }
                             ).ToList();
                totalRegistros = _data_Full.Count;

                _dataQuery = _data_Full.Skip((pagina - 1) * 10).ToList();

                listaPaginador.PaginaActual = pagina;

                listaPaginador.ElementosPagina = 10;
                listaPaginador.TotalElementos = totalRegistros;
                listaPaginador.Elementos = _dataQuery;
            }
            return listaPaginador;
        }
        public ReturnCRUD AddSolicitudConstancia(HER_SolicitudConstancia data)
        {
            Hermes2018.Models.ReturnCRUD salida = new Hermes2018.Models.ReturnCRUD(-1, "No es posible leer la información que se ingresa");
            try
            {
                using (ModelsDBF.DBFContext _bd = new ModelsDBF.DBFContext())
                {
                    if (data.Id > 0)
                    {
                        var dataFind = _bd.HER_SolicitudConstancia.Where(x => x.Id == data.Id).FirstOrDefault();
                        if (dataFind != null)
                        {
                            _bd.Update(dataFind);
                            _bd.SaveChanges();
                            salida = new Hermes2018.Models.ReturnCRUD(0, "");
                        }
                    }
                    else
                    {
                        data.Folio = new Models.General().ReturnFolio();

                        data.FechaSolicitud = DateTime.Now;
                        _bd.HER_SolicitudConstancia.Attach(data);
                        _bd.SaveChanges();
                        salida = new Hermes2018.Models.ReturnCRUD(0, data.Id.ToString());

                        ModelsDBF.HER_SolicitudConstanciaEstado estado = new ModelsDBF.HER_SolicitudConstanciaEstado();
                        estado.EstadoId = data.EstadoId;
                        estado.FechaHora = DateTime.Now;
                        estado.SolicitudConstanciaId = data.Id;
                        estado.UsuarioId = data.UsuarioId;
                        _bd.HER_SolicitudConstanciaEstado.Attach(estado);
                        _bd.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("ConstanciaService: " + ex.Message);
            }
            return salida;
        }

        public ReturnCRUD AddEstadoConstancias(HER_SolicitudConstanciaEstado data)
        {
            Hermes2018.Models.ReturnCRUD salida = new Hermes2018.Models.ReturnCRUD(-1, "No es posible leer la información que se ingresa");
            try
            {
                using (ModelsDBF.DBFContext _bd = new ModelsDBF.DBFContext())
                {
                    data.FechaHora = DateTime.Now;
                    _bd.HER_SolicitudConstanciaEstado.Attach(data);
                    _bd.SaveChanges();
                    salida = new Hermes2018.Models.ReturnCRUD(0, data.Id.ToString());
                    _bd.SaveChanges();
                    var dataFind = _bd.HER_SolicitudConstancia.Where(x => x.Id == data.SolicitudConstanciaId).FirstOrDefault();
                    if (dataFind != null)
                    {
                        dataFind.EstadoId = data.EstadoId;
                        _bd.Update(dataFind);
                        _bd.SaveChanges();
                        salida = new Hermes2018.Models.ReturnCRUD(0, "");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("ConstanciaService: " + ex.Message);
            }
            return salida;
        }

        public ConstanciasSolicitadasCustom GET_ConstanciaSolicitadaId(int id)
        {

            var user = new List<Models.HER_InfoUsuario>(_bdContext.HER_InfoUsuario);

            Models.ConstanciasSolicitadasCustom dataConstancia = new Models.ConstanciasSolicitadasCustom();


            using (ModelsDBF.DBFContext _bd = new ModelsDBF.DBFContext())
            {
                dataConstancia = (from b in _bd.HER_SolicitudConstancia
                                  join e in _bd.HER_EstadoConstancias on b.EstadoId equals e.Id
                                  join u in _bd.HER_Constancias on b.ConstanciaId equals u.Id
                                  join uu in user on b.UsuarioId equals uu.HER_UserName
                                  //join d in _bd.CDEPEN on b.CveDep equals d.CVEDEP
                                  where b.Id == id
                                  select new Models.ConstanciasSolicitadasCustom
                                  {
                                      Id = b.Id,
                                      ConstanciaId = b.ConstanciaId,
                                      UsuarioId = b.UsuarioId,
                                      NoPersonal = b.NoPersonal,
                                      FechaSolicitud = b.FechaSolicitud,
                                      EstadoId = b.EstadoId,
                                      Folio = b.Folio,
                                      Mensaje = b.Mensaje,
                                      NombreConstancia = u.Nombre,
                                      NombreEstado = e.Nombre,
                                      CveDep = b.CveDep,
                                      NombreDependencia = b.NombreDep,
                                      NombreUsuario = uu.HER_NombreCompleto,
                                      TipoPersonal = b.TipoPersonal,
                                      EstadosConstancia = (from e in _bd.HER_SolicitudConstanciaEstado where e.SolicitudConstanciaId == b.Id select e).ToList()

                                  }
                  ).FirstOrDefault();
            }
            return dataConstancia;
        }

        public ListaPaginador<ConstanciasSolicitadasCustom> GET_ConstanciasSolicitadas(int pagina)
        {

            Models.ListaPaginador<Models.ConstanciasSolicitadasCustom> listaPaginador = new Models.ListaPaginador<Models.ConstanciasSolicitadasCustom>();
            IEnumerable<Models.ConstanciasSolicitadasCustom> _dataQuery;

            int totalRegistros = 0;

            using (ModelsDBF.DBFContext _bd = new ModelsDBF.DBFContext())
            {
                var _data_Full = (from b in _bd.HER_SolicitudConstancia
                                  join e in _bd.HER_EstadoConstancias on b.EstadoId equals e.Id
                                  join u in _bd.HER_Constancias on b.ConstanciaId equals u.Id
                                  //join d in _bd.CDEPEN on b.CveDep equals d.CVEDEP
                                  select new Models.ConstanciasSolicitadasCustom
                                  {
                                      Id = b.Id,
                                      ConstanciaId = b.ConstanciaId,
                                      UsuarioId = b.UsuarioId,
                                      NoPersonal = b.NoPersonal,
                                      FechaSolicitud = b.FechaSolicitud,
                                      EstadoId = b.EstadoId,
                                      Folio = b.Folio,
                                      Mensaje = b.Mensaje,
                                      NombreConstancia = u.Nombre,
                                      NombreEstado = e.Nombre,
                                      CveDep = b.CveDep,
                                      NombreDependencia = b.NombreDep,
                                      TipoPersonal = b.TipoPersonal//,
                                      //EstadosConstancia = (from e in _bd.HER_SolicitudConstanciaEstado where e.SolicitudConstanciaId == b.Id select e).ToList()
                                  }
                             ).ToList();

                totalRegistros = _data_Full.Count;

                _dataQuery = _data_Full.Skip((pagina - 1) * 10).ToList();

                listaPaginador.PaginaActual = pagina;

                listaPaginador.ElementosPagina = 10;
                listaPaginador.TotalElementos = totalRegistros;
                listaPaginador.Elementos = _dataQuery;
            }
            return listaPaginador;
        }

        public ListaPaginador<SolicitudConstanciaCustom> GET_HER_SolicitudConstancia(string idUsuario, int pagina)
        {
            ListaPaginador<SolicitudConstanciaCustom> listaPaginador = new Models.ListaPaginador<Models.SolicitudConstanciaCustom>();

            IEnumerable<Models.SolicitudConstanciaCustom> _dataQuery;

            using (ModelsDBF.DBFContext _bd = new ModelsDBF.DBFContext())
            {

                _dataQuery = (from b in _bd.HER_SolicitudConstancia
                              join e in _bd.HER_EstadoConstancias on b.EstadoId equals e.Id
                              join u in _bd.HER_Constancias on b.ConstanciaId equals u.Id
                              where b.UsuarioId == idUsuario
                              select new Models.SolicitudConstanciaCustom
                              {
                                  Id = b.Id,
                                  ConstanciaId = b.ConstanciaId,
                                  UsuarioId = b.UsuarioId,
                                  NoPersonal = b.NoPersonal,
                                  FechaSolicitud = b.FechaSolicitud,
                                  EstadoId = b.EstadoId,
                                  Folio = b.Folio,
                                  Mensaje = b.Mensaje,
                                  NombreConstancia = u.Nombre,
                                  NombreEstado = e.Nombre,
                                  CveDep = b.CveDep,
                                  Dependencia = b.NombreDep,
                                  TipoPersonal = b.TipoPersonal,
                                  CampusId = b.CampusId,
                                  NombreCampus = b.NombreCampus,
                                  EstadosConstancia = (from e in _bd.HER_SolicitudConstanciaEstado where e.SolicitudConstanciaId == b.Id select e).ToList()

                              }
                             ).Skip((pagina - 1) * 10).ToList();

                listaPaginador.PaginaActual = pagina;

                listaPaginador.ElementosPagina = 10;
                listaPaginador.TotalElementos = _bd.HER_SolicitudConstancia.Where(x => x.UsuarioId == idUsuario).Count();
                listaPaginador.Elementos = _dataQuery;
            }
            return listaPaginador;
        }

        public List<oLsServMed> ObtieneServMed(int numPer, int tipoPer)
        {
            EncriptarJson _dataReturn = new EncriptarJson();
            JsonForDesencript _dataEncriptadaReturn = new JsonForDesencript();
            DesencriptarJson _rows = new DesencriptarJson();
            ServMedCustom _dataSMC = new ServMedCustom();
            List<oLsServMed> _data = new List<oLsServMed>();
            string jsonlimpio = "";
            var configuration = new RestApiDSIA().GetConfiguration();
            var dataBearer = configuration.GetSection("COnfigureCustom").GetSection("Bearer").Value;

            var jsonEncrip = new
            {
                sNumper = numPer.ToString(),
                sNumTipoPer = tipoPer.ToString(),
                sClave = configuration.GetSection("ConfigureCustom").GetSection("sClave").Value
            };
            var jsonRaw = new
            {
                sAesKey = configuration.GetSection("ConfigureCustom").GetSection("sClaveLogin").Value,
                sClave = configuration.GetSection("ConfigureCustom").GetSection("sClave").Value,
                sJson = JsonConvert.SerializeObject(jsonEncrip)
            };

            var salida = new RequestClientApi().Post(configuration.GetSection("ServicesAPIDSIA").GetSection("Encriptar").Value, JsonConvert.SerializeObject(jsonRaw), dataBearer);

            if (salida != "" && salida != null)
            {
                _dataReturn = JsonConvert.DeserializeObject<EncriptarJson>(salida);

                var jsonEncriptado = new
                {
                    sEncript = _dataReturn.sEncrip.ToString()
                };

                var salidaEncriptada = new RequestClientApi().Post(configuration.GetSection("ServicesAPIDSIA").GetSection("ObtieneServMed").Value, JsonConvert.SerializeObject(jsonEncriptado), dataBearer);

                if (salidaEncriptada != "" && salidaEncriptada != null)
                {
                    _dataEncriptadaReturn = JsonConvert.DeserializeObject<JsonForDesencript>(salidaEncriptada);

                    var jsonDesRaw = new
                    {
                        sAesKey = configuration.GetSection("ConfigureCustom").GetSection("sClaveLogin").Value,
                        sClave = configuration.GetSection("ConfigureCustom").GetSection("sClave").Value,
                        sEncrip = _dataEncriptadaReturn.sEncript.ToString()
                    };

                    var salidaDesencriptada = new RequestClientApi().Post(configuration.GetSection("ServicesAPIDSIA").GetSection("Desencriptar").Value, JsonConvert.SerializeObject(jsonDesRaw), dataBearer);

                    if (salidaDesencriptada != "" && salidaDesencriptada != null)
                    {
                        _rows = JsonConvert.DeserializeObject<DesencriptarJson>(salidaDesencriptada);
                        jsonlimpio = _rows.sJson.Replace(Environment.NewLine, "");
                        jsonlimpio = jsonlimpio.Replace("  ", String.Empty);
                        _dataSMC = JsonConvert.DeserializeObject<ServMedCustom>(jsonlimpio);
                        _data = _dataSMC.oLsServMed;
                    }
                }
            }
            return _data;
        }

        public List<oLsServMed> ObtieneServMedDep(int numPer, int tipoPer)
        {
            EncriptarJson _dataReturn = new EncriptarJson();
            JsonForDesencript _dataEncriptadaReturn = new JsonForDesencript();
            DesencriptarJson _rows = new DesencriptarJson();
            ServMedDepCustom _dataSMDC = new ServMedDepCustom();
            List<oLsServMed> _data = new List<oLsServMed>();
            string jsonlimpio = "";
            var configuration = new RestApiDSIA().GetConfiguration();
            var dataBearer = configuration.GetSection("COnfigureCustom").GetSection("Bearer").Value;
            var jsonEncrip = new
            {
                sNumper = numPer.ToString(),
                sNumTipoPer = tipoPer.ToString(),
                sClave = configuration.GetSection("ConfigureCustom").GetSection("sClave").Value
            };
            var jsonRaw = new
            {
                sAesKey = configuration.GetSection("ConfigureCustom").GetSection("sClaveLogin").Value,
                sClave = configuration.GetSection("ConfigureCustom").GetSection("sClave").Value,
                sJson = JsonConvert.SerializeObject(jsonEncrip)
            };

            var salida = new RequestClientApi().Post(configuration.GetSection("ServicesAPIDSIA").GetSection("Encriptar").Value, JsonConvert.SerializeObject(jsonRaw), dataBearer);

            if (salida != "" && salida != null)
            {
                _dataReturn = JsonConvert.DeserializeObject<EncriptarJson>(salida);

                var jsonEncriptado = new
                {
                    sEncript = _dataReturn.sEncrip.ToString()
                };

                var salidaEncriptada = new RequestClientApi().Post(configuration.GetSection("ServicesAPIDSIA").GetSection("ObtieneServMedDep").Value, JsonConvert.SerializeObject(jsonEncriptado), dataBearer);

                if (salidaEncriptada != "" && salidaEncriptada != null)
                {
                    _dataEncriptadaReturn = JsonConvert.DeserializeObject<JsonForDesencript>(salidaEncriptada);

                    var jsonDesRaw = new
                    {
                        sAesKey = configuration.GetSection("ConfigureCustom").GetSection("sClaveLogin").Value,
                        sClave = configuration.GetSection("ConfigureCustom").GetSection("sClave").Value,
                        sEncrip = _dataEncriptadaReturn.sEncript.ToString()
                    };

                    var salidaDesencriptada = new RequestClientApi().Post(configuration.GetSection("ServicesAPIDSIA").GetSection("Desencriptar").Value, JsonConvert.SerializeObject(jsonDesRaw), dataBearer);

                    if (salidaDesencriptada != "" && salidaDesencriptada != null)
                    {
                        _rows = JsonConvert.DeserializeObject<DesencriptarJson>(salidaDesencriptada);
                        jsonlimpio = _rows.sJson.Replace(Environment.NewLine, "");
                        jsonlimpio = jsonlimpio.Replace("  ", String.Empty);
                        _dataSMDC = JsonConvert.DeserializeObject<ServMedDepCustom>(jsonlimpio);
                        _data = _dataSMDC.oLsServMedDep;
                    }
                }
            }
            return _data;
        }

        public List<oLsIpe> ObtieneTrab_Perc(int numPer, int tipoPer)
        {
            EncriptarJson _dataReturn = new EncriptarJson();
            JsonForDesencript _dataEncriptadaReturn = new JsonForDesencript();
            DesencriptarJson _rows = new DesencriptarJson();
            TrabPercCustom _dataTPC = new TrabPercCustom();
            List<oLsIpe> _data = new List<oLsIpe>();
            string jsonlimpio = "";

            var configuration = new RestApiDSIA().GetConfiguration();
            var dataBearer = configuration.GetSection("ConfigureCustom").GetSection("Bearer").Value;

            var jsonEncrip = new
            {
                sNumper = numPer.ToString(),
                sNumTipoPer = tipoPer.ToString(),
                sClave = configuration.GetSection("ConfigureCustom").GetSection("sClave").Value
            };

            var jsonRaw = new
            {
                sAesKey = configuration.GetSection("ConfigureCustom").GetSection("sClaveLogin").Value,
                sClave = configuration.GetSection("ConfigureCustom").GetSection("sClave").Value,
                sJson = JsonConvert.SerializeObject(jsonEncrip)
            };

            var salida = new RequestClientApi().Post(configuration.GetSection("ServicesAPIDSIA").GetSection("Encriptar").Value, JsonConvert.SerializeObject(jsonRaw), dataBearer);

            if (salida != "" && salida != null)
            {
                _dataReturn = JsonConvert.DeserializeObject<EncriptarJson>(salida);

                var jsonEncriptado = new
                {
                    sEncript = _dataReturn.sEncrip.ToString()
                };

                var salidaEncriptada = new RequestClientApi().Post(configuration.GetSection("ServicesAPIDSIA").GetSection("ObtieneTrab_Perc").Value, JsonConvert.SerializeObject(jsonEncriptado), dataBearer);

                if (salidaEncriptada != "" && salidaEncriptada != null)
                {
                    _dataEncriptadaReturn = JsonConvert.DeserializeObject<JsonForDesencript>(salidaEncriptada);

                    var jsonDesRaw = new
                    {
                        sAesKey = configuration.GetSection("ConfigureCustom").GetSection("sClaveLogin").Value,
                        sClave = configuration.GetSection("ConfigureCustom").GetSection("sClave").Value,
                        sEncrip = _dataEncriptadaReturn.sEncript.ToString()
                    };

                    var salidaDesencriptada = new RequestClientApi().Post(configuration.GetSection("ServicesAPIDSIA").GetSection("Desencriptar").Value, JsonConvert.SerializeObject(jsonDesRaw), dataBearer);

                    if (salidaDesencriptada != "" && salidaDesencriptada != null)
                    {
                        _rows = JsonConvert.DeserializeObject<DesencriptarJson>(salidaDesencriptada);
                        jsonlimpio = _rows.sJson.Replace(Environment.NewLine, "");
                        jsonlimpio = jsonlimpio.Replace("  ", String.Empty);
                        _dataTPC = JsonConvert.DeserializeObject<TrabPercCustom>(jsonlimpio);
                        _data = _dataTPC.oLsTrabPerc;
                    }
                }
            }
            return _data;
        }

        public List<oLsIpe> ObtieneIpe(int numPer, int tipoPer)
        {
            EncriptarJson _dataReturn = new EncriptarJson();
            JsonForDesencript _dataEncriptadaReturn = new JsonForDesencript();
            DesencriptarJson _rows = new DesencriptarJson();
            IpeCustom _dataIPE = new IpeCustom();
            List<oLsIpe> _data = new List<oLsIpe>();
            string jsonlimpio = "";

            var configuration = new RestApiDSIA().GetConfiguration();
            var dataBearer = configuration.GetSection("ConfigureCustom").GetSection("Bearer").Value;

            var jsonEncrip = new
            {
                sNumper = numPer.ToString(),
                sNumTipoPer = tipoPer.ToString(),
                sClave = configuration.GetSection("ConfigureCustom").GetSection("sClave").Value
            };

            var jsonRaw = new
            {
                sAesKey = configuration.GetSection("ConfigureCustom").GetSection("sClaveLogin").Value,
                sClave = configuration.GetSection("ConfigureCustom").GetSection("sClave").Value,
                sJson = JsonConvert.SerializeObject(jsonEncrip)
            };

            var salida = new RequestClientApi().Post(configuration.GetSection("ServicesAPIDSIA").GetSection("Encriptar").Value, JsonConvert.SerializeObject(jsonRaw), dataBearer);

            if (salida != "" && salida != null)
            {
                _dataReturn = JsonConvert.DeserializeObject<EncriptarJson>(salida);

                var jsonEncriptado = new
                {
                    sEncript = _dataReturn.sEncrip.ToString()
                };

                var salidaEncriptada = new RequestClientApi().Post(configuration.GetSection("ServicesAPIDSIA").GetSection("ObtieneIpe").Value, JsonConvert.SerializeObject(jsonEncriptado), dataBearer);

                if (salidaEncriptada != "" && salidaEncriptada != null)
                {
                    _dataEncriptadaReturn = JsonConvert.DeserializeObject<JsonForDesencript>(salidaEncriptada);

                    var jsonDesRaw = new
                    {
                        sAesKey = configuration.GetSection("ConfigureCustom").GetSection("sClaveLogin").Value,
                        sClave = configuration.GetSection("ConfigureCustom").GetSection("sClave").Value,
                        sEncrip = _dataEncriptadaReturn.sEncript.ToString()
                    };

                    var salidaDesencriptada = new RequestClientApi().Post(configuration.GetSection("ServicesAPIDSIA").GetSection("Desencriptar").Value, JsonConvert.SerializeObject(jsonDesRaw), dataBearer);

                    if (salidaDesencriptada != "" && salidaDesencriptada != null)
                    {
                        _rows = JsonConvert.DeserializeObject<DesencriptarJson>(salidaDesencriptada);
                        jsonlimpio = _rows.sJson.Replace(Environment.NewLine, "");
                        jsonlimpio = jsonlimpio.Replace("  ", String.Empty);
                        _dataIPE = JsonConvert.DeserializeObject<IpeCustom>(jsonlimpio);
                        _data = _dataIPE.oLsIpe;
                    }
                }
            }
            return _data;
        }

        public List<oLsIpe> ObtieneMag(int numPer, int tipoPer)
        {
            EncriptarJson _dataReturn = new EncriptarJson();
            JsonForDesencript _dataEncriptadaReturn = new JsonForDesencript();
            DesencriptarJson _rows = new DesencriptarJson();
            MagCustom _dataMagC = new MagCustom();
            string jsonlimpio = "";
            List<oLsIpe> _data = new List<oLsIpe>();
            var configuration = new RestApiDSIA().GetConfiguration();
            var dataBearer = configuration.GetSection("ConfigureCustom").GetSection("Bearer").Value;

            var jsonEncrip = new
            {
                sNumper = numPer.ToString(),
                sNumTipoPer = tipoPer.ToString(),
                sClave = configuration.GetSection("ConfigureCustom").GetSection("sClave").Value
            };

            var jsonRaw = new
            {
                sAesKey = configuration.GetSection("ConfigureCustom").GetSection("sClaveLogin").Value,
                sClave = configuration.GetSection("ConfigureCustom").GetSection("sClave").Value,
                sJson = JsonConvert.SerializeObject(jsonEncrip)
            };

            var salida = new RequestClientApi().Post(configuration.GetSection("ServicesAPIDSIA").GetSection("Encriptar").Value, JsonConvert.SerializeObject(jsonRaw), dataBearer);

            if (salida != "" && salida != null)
            {
                _dataReturn = JsonConvert.DeserializeObject<EncriptarJson>(salida);

                var jsonEncriptado = new
                {
                    sEncript = _dataReturn.sEncrip.ToString()
                };

                var salidaEncriptada = new RequestClientApi().Post(configuration.GetSection("ServicesAPIDSIA").GetSection("ObtieneMag").Value, JsonConvert.SerializeObject(jsonEncriptado), dataBearer);

                if (salidaEncriptada != "" && salidaEncriptada != null)
                {
                    _dataEncriptadaReturn = JsonConvert.DeserializeObject<JsonForDesencript>(salidaEncriptada);

                    var jsonDesRaw = new
                    {
                        sAesKey = configuration.GetSection("ConfigureCustom").GetSection("sClaveLogin").Value,
                        sClave = configuration.GetSection("ConfigureCustom").GetSection("sClave").Value,
                        sEncrip = _dataEncriptadaReturn.sEncript.ToString()
                    };

                    var salidaDesencriptada = new RequestClientApi().Post(configuration.GetSection("ServicesAPIDSIA").GetSection("Desencriptar").Value, JsonConvert.SerializeObject(jsonDesRaw), dataBearer);

                    if (salidaDesencriptada != "" && salidaDesencriptada != null)
                    {
                        _rows = JsonConvert.DeserializeObject<DesencriptarJson>(salidaDesencriptada);
                        jsonlimpio = _rows.sJson.Replace(Environment.NewLine, "");
                        jsonlimpio = jsonlimpio.Replace("  ", String.Empty);
                        _dataMagC = JsonConvert.DeserializeObject<MagCustom>(jsonlimpio);
                        _data = _dataMagC.oLsMag;
                    }
                }
            }
            return _data;
        }

        public List<oLsOfiBajIPE> ObtieneBajaIpe(int numPer, int tipoPer)
        {
            EncriptarJson _dataReturn = new EncriptarJson();
            JsonForDesencript _dataEncriptadaReturn = new JsonForDesencript();
            DesencriptarJson _rows = new DesencriptarJson();
            OfiBajIPECustom _dataBIC = new OfiBajIPECustom();
            List<oLsOfiBajIPE> _data = new List<oLsOfiBajIPE>();
            string jsonlimpio = "";
            var configuration = new RestApiDSIA().GetConfiguration();
            var dataBearer = configuration.GetSection("ConfigureCustom").GetSection("Bearer").Value;

            var jsonEncrip = new
            {
                sNumper = numPer.ToString(),
                sNumTipoPer = tipoPer.ToString(),
                sClave = configuration.GetSection("ConfigureCustom").GetSection("sClave").Value
            };

            var jsonRaw = new
            {
                sAesKey = configuration.GetSection("ConfigureCustom").GetSection("sClaveLogin").Value,
                sClave = configuration.GetSection("ConfigureCustom").GetSection("sClave").Value,
                sJson = JsonConvert.SerializeObject(jsonEncrip)
            };

            var salida = new RequestClientApi().Post(configuration.GetSection("ServicesAPIDSIA").GetSection("Encriptar").Value, JsonConvert.SerializeObject(jsonRaw), dataBearer);

            if (salida != "" && salida != null)
            {
                _dataReturn = JsonConvert.DeserializeObject<EncriptarJson>(salida);

                var jsonEncriptado = new
                {
                    sEncript = _dataReturn.sEncrip.ToString()
                };

                var salidaEncriptada = new RequestClientApi().Post(configuration.GetSection("ServicesAPIDSIA").GetSection("ObtieneOfiBajIPE").Value, JsonConvert.SerializeObject(jsonEncriptado), dataBearer);

                if (salidaEncriptada != "" && salidaEncriptada != null)
                {
                    _dataEncriptadaReturn = JsonConvert.DeserializeObject<JsonForDesencript>(salidaEncriptada);

                    var jsonDesRaw = new
                    {
                        sAesKey = configuration.GetSection("ConfigureCustom").GetSection("sClaveLogin").Value,
                        sClave = configuration.GetSection("ConfigureCustom").GetSection("sClave").Value,
                        sEncrip = _dataEncriptadaReturn.sEncript.ToString()
                    };

                    var salidaDesencriptada = new RequestClientApi().Post(configuration.GetSection("ServicesAPIDSIA").GetSection("Desencriptar").Value, JsonConvert.SerializeObject(jsonDesRaw), dataBearer);

                    if (salidaDesencriptada != "" && salidaDesencriptada != null)
                    {
                        _rows = JsonConvert.DeserializeObject<DesencriptarJson>(salidaDesencriptada);
                        jsonlimpio = _rows.sJson.Replace(Environment.NewLine, "");
                        jsonlimpio = jsonlimpio.Replace("  ", String.Empty);
                        _dataBIC = JsonConvert.DeserializeObject<OfiBajIPECustom>(jsonlimpio);
                        _data = _dataBIC.oLsOfiBajIPE;
                    }
                }
            }
            return _data;
        }

        public List<oLsOfiBajIPE> ObtieneBajaMag(int numPer, int tipoPer)
        {
            EncriptarJson _dataReturn = new EncriptarJson();
            JsonForDesencript _dataEncriptadaReturn = new JsonForDesencript();
            DesencriptarJson _rows = new DesencriptarJson();
            OfiBajIPECustom _dataBMC = new OfiBajIPECustom();
            List<oLsOfiBajIPE> _data = new List<oLsOfiBajIPE>();
            string jsonlimpio = "";
            var configuration = new RestApiDSIA().GetConfiguration();
            var dataBearer = configuration.GetSection("ConfigureCustom").GetSection("Bearer").Value;

            var jsonEncrip = new
            {
                sNumper = numPer.ToString(),
                sNumTipoPer = tipoPer.ToString(),
                sClave = configuration.GetSection("ConfigureCustom").GetSection("sClave").Value
            };

            var jsonRaw = new
            {
                sAesKey = configuration.GetSection("ConfigureCustom").GetSection("sClaveLogin").Value,
                sClave = configuration.GetSection("ConfigureCustom").GetSection("sClave").Value,
                sJson = JsonConvert.SerializeObject(jsonEncrip)
            };

            var salida = new RequestClientApi().Post(configuration.GetSection("ServicesAPIDSIA").GetSection("Encriptar").Value, JsonConvert.SerializeObject(jsonRaw), dataBearer);
                        
            if (salida != "" && salida != null)
            {
                _dataReturn = JsonConvert.DeserializeObject<EncriptarJson>(salida);

                var jsonEncriptado = new
                {
                    sEncript = _dataReturn.sEncrip.ToString()
                };

                var salidaEncriptada = new RequestClientApi().Post(configuration.GetSection("ServicesAPIDSIA").GetSection("ObtieneOfiBajMAG").Value, JsonConvert.SerializeObject(jsonEncriptado), dataBearer);

                if (salidaEncriptada != "" && salidaEncriptada != null)
                {
                    _dataEncriptadaReturn = JsonConvert.DeserializeObject<JsonForDesencript>(salidaEncriptada);

                    var jsonDesRaw = new
                    {
                        sAesKey = configuration.GetSection("ConfigureCustom").GetSection("sClaveLogin").Value,
                        sClave = configuration.GetSection("ConfigureCustom").GetSection("sClave").Value,
                        sEncrip = _dataEncriptadaReturn.sEncript.ToString()
                    };

                    var salidaDesencriptada = new RequestClientApi().Post(configuration.GetSection("ServicesAPIDSIA").GetSection("Desencriptar").Value, JsonConvert.SerializeObject(jsonDesRaw), dataBearer);

                    if (salidaDesencriptada != "" && salidaDesencriptada != null)
                    {
                        _rows = JsonConvert.DeserializeObject<DesencriptarJson>(salidaDesencriptada);
                        jsonlimpio = _rows.sJson.Replace(Environment.NewLine, "");
                        jsonlimpio = jsonlimpio.Replace("  ", String.Empty);
                        _dataBMC = JsonConvert.DeserializeObject<OfiBajIPECustom>(jsonlimpio);
                        _data = _dataBMC.oLsOfiBajIPE;
                    }
                }
            }
            return _data;
        }

        public List<oLsVisa> ObtieneVISA(int numPer, int tipoPer)
        {
            EncriptarJson _dataReturn = new EncriptarJson();
            JsonForDesencript _dataEncriptadaReturn = new JsonForDesencript();
            DesencriptarJson _rows = new DesencriptarJson();
            VisaCustom _dataVS = new VisaCustom();
            List<oLsVisa> _data = new List<oLsVisa>();
            string jsonlimpio = "";

            var configuration = new RestApiDSIA().GetConfiguration();
            var dataBearer = configuration.GetSection("ConfigureCustom").GetSection("Bearer").Value;

            var jsonEncrip = new
            {
                sNumper = numPer.ToString(),
                sNumTipoPer = tipoPer.ToString(),
                sClave = configuration.GetSection("ConfigureCustom").GetSection("sClave").Value
            };

            var jsonRaw = new
            {
                sAesKey = configuration.GetSection("ConfigureCustom").GetSection("sClaveLogin").Value,
                sClave = configuration.GetSection("ConfigureCustom").GetSection("sClave").Value,
                sJson = JsonConvert.SerializeObject(jsonEncrip)
            };

            var salida = new RequestClientApi().Post(configuration.GetSection("ServicesAPIDSIA").GetSection("Encriptar").Value, JsonConvert.SerializeObject(jsonRaw), dataBearer);


            if (salida != "" && salida != null)
            {
                _dataReturn = JsonConvert.DeserializeObject<EncriptarJson>(salida);

                var jsonEncriptado = new
                {
                    sEncript = _dataReturn.sEncrip.ToString()
                };

                var salidaEncriptada = new RequestClientApi().Post(configuration.GetSection("ServicesAPIDSIA").GetSection("ObtieneVisa").Value, JsonConvert.SerializeObject(jsonEncriptado), dataBearer);

                if (salidaEncriptada != "" && salidaEncriptada != null)
                {
                    _dataEncriptadaReturn = JsonConvert.DeserializeObject<JsonForDesencript>(salidaEncriptada);

                    var jsonDesRaw = new
                    {
                        sAesKey = configuration.GetSection("ConfigureCustom").GetSection("sClaveLogin").Value,
                        sClave = configuration.GetSection("ConfigureCustom").GetSection("sClave").Value,
                        sEncrip = _dataEncriptadaReturn.sEncript.ToString()
                    };

                    var salidaDesencriptada = new RequestClientApi().Post(configuration.GetSection("ServicesAPIDSIA").GetSection("Desencriptar").Value, JsonConvert.SerializeObject(jsonDesRaw), dataBearer);

                    if (salidaDesencriptada != "" && salidaDesencriptada != null)
                    {
                        _rows = JsonConvert.DeserializeObject<DesencriptarJson>(salidaDesencriptada);
                        jsonlimpio = _rows.sJson.Replace(Environment.NewLine, "");
                        jsonlimpio = jsonlimpio.Replace("  ", String.Empty);
                        _dataVS = JsonConvert.DeserializeObject<VisaCustom>(jsonlimpio);
                        _data = _dataVS.oLsVisa;
                    }
                }
            }
            return _data;
        }

        public List<oLsVisa> ObtieneVISADep(int numPer, int tipoPer)
        {
            EncriptarJson _dataReturn = new EncriptarJson();
            JsonForDesencript _dataEncriptadaReturn = new JsonForDesencript();
            DesencriptarJson _rows = new DesencriptarJson();
            VisaDepCustom _dataVDC = new VisaDepCustom();
            List<oLsVisa> _data = new List<oLsVisa>();
            string jsonlimpio = "";
            var configuration = new RestApiDSIA().GetConfiguration();
            var dataBearer = configuration.GetSection("ConfigureCustom").GetSection("Bearer").Value;

            var jsonEncrip = new
            {
                sNumper = numPer.ToString(),
                sNumTipoPer = tipoPer.ToString(),
                sClave = configuration.GetSection("ConfigureCustom").GetSection("sClave").Value
            };

            var jsonRaw = new
            {
                sAesKey = configuration.GetSection("ConfigureCustom").GetSection("sClaveLogin").Value,
                sClave = configuration.GetSection("ConfigureCustom").GetSection("sClave").Value,
                sJson = JsonConvert.SerializeObject(jsonEncrip)
            };

            var salida = new RequestClientApi().Post(configuration.GetSection("ServicesAPIDSIA").GetSection("Encriptar").Value, JsonConvert.SerializeObject(jsonRaw), dataBearer);
                                    
            if (salida != "" && salida != null)
            {
                _dataReturn = JsonConvert.DeserializeObject<EncriptarJson>(salida);

                var jsonEncriptado = new
                {
                    sEncript = _dataReturn.sEncrip.ToString()
                };

                var salidaEncriptada = new RequestClientApi().Post(configuration.GetSection("ServicesAPIDSIA").GetSection("ObtieneVisaDep").Value, JsonConvert.SerializeObject(jsonEncriptado), dataBearer);

                if (salidaEncriptada != "" && salidaEncriptada != null)
                {
                    _dataEncriptadaReturn = JsonConvert.DeserializeObject<JsonForDesencript>(salidaEncriptada);

                    var jsonDesRaw = new
                    {
                        sAesKey = configuration.GetSection("ConfigureCustom").GetSection("sClaveLogin").Value,
                        sClave = configuration.GetSection("ConfigureCustom").GetSection("sClave").Value,
                        sEncrip = _dataEncriptadaReturn.sEncript.ToString()
                    };

                    var salidaDesencriptada = new RequestClientApi().Post(configuration.GetSection("ServicesAPIDSIA").GetSection("Desencriptar").Value, JsonConvert.SerializeObject(jsonDesRaw), dataBearer);

                    if (salidaDesencriptada != "" && salidaDesencriptada != null)
                    {
                        _rows = JsonConvert.DeserializeObject<DesencriptarJson>(salidaDesencriptada);
                        jsonlimpio = _rows.sJson.Replace(Environment.NewLine, "");
                        jsonlimpio = jsonlimpio.Replace("  ", String.Empty);
                        _dataVDC = JsonConvert.DeserializeObject<VisaDepCustom>(jsonlimpio);
                        _data = _dataVDC.oLsVisaDep;

                    }
                }
            }
            return _data;
        }

        public List<oLsPRODEP> ObtienePRODep(int numPer, int tipoPer)
        {
            EncriptarJson _dataReturn = new EncriptarJson();
            JsonForDesencript _dataEncriptadaReturn = new JsonForDesencript();
            DesencriptarJson _rows = new DesencriptarJson();
            ProdepCustom _dataprodep = new ProdepCustom();
            List<oLsPRODEP> _data = new List<oLsPRODEP>();
            string jsonlimpio = "";
            var configuration = new RestApiDSIA().GetConfiguration();
            var dataBearer = configuration.GetSection("ConfigureCustom").GetSection("Bearer").Value;

            var jsonEncrip = new
            {
                sNumper = numPer.ToString(),
                sNumTipoPer = tipoPer.ToString(),
                sClave = configuration.GetSection("ConfigureCustom").GetSection("sClave").Value
            };

            var jsonRaw = new
            {
                sAesKey = configuration.GetSection("ConfigureCustom").GetSection("sClaveLogin").Value,
                sClave = configuration.GetSection("ConfigureCustom").GetSection("sClave").Value,
                sJson = JsonConvert.SerializeObject(jsonEncrip)
            };

            var salida = new RequestClientApi().Post(configuration.GetSection("ServicesAPIDSIA").GetSection("Encriptar").Value, JsonConvert.SerializeObject(jsonRaw), dataBearer);

            if (salida != "" && salida != null)
            {
                _dataReturn = JsonConvert.DeserializeObject<EncriptarJson>(salida);

                var jsonEncriptado = new
                {
                    sEncript = _dataReturn.sEncrip.ToString()
                };

                var salidaEncriptada = new RequestClientApi().Post(configuration.GetSection("ServicesAPIDSIA").GetSection("ObtienePRODep").Value, JsonConvert.SerializeObject(jsonEncriptado), dataBearer);

                if (salidaEncriptada != "" && salidaEncriptada != null)
                {
                    _dataEncriptadaReturn = JsonConvert.DeserializeObject<JsonForDesencript>(salidaEncriptada);

                    var jsonDesRaw = new
                    {
                        sAesKey = configuration.GetSection("ConfigureCustom").GetSection("sClaveLogin").Value,
                        sClave = configuration.GetSection("ConfigureCustom").GetSection("sClave").Value,
                        sEncrip = _dataEncriptadaReturn.sEncript.ToString()
                    };

                    var salidaDesencriptada = new RequestClientApi().Post(configuration.GetSection("ServicesAPIDSIA").GetSection("Desencriptar").Value, JsonConvert.SerializeObject(jsonDesRaw), dataBearer);

                    if (salidaDesencriptada != "" && salidaDesencriptada != null)
                    {
                        _rows = JsonConvert.DeserializeObject<DesencriptarJson>(salidaDesencriptada);
                        jsonlimpio = _rows.sJson.Replace(Environment.NewLine, "");
                        jsonlimpio = jsonlimpio.Replace("  ", String.Empty);
                        _dataprodep = JsonConvert.DeserializeObject<ProdepCustom>(jsonlimpio);
                        _data = _dataprodep.oLsPRODEP;
                    }
                }
            }
            return _data;
        }

        public oLoginTP ObtieneCveLogin_TP(string sCveLogin)
        {
            EncriptarJson _dataReturn = new EncriptarJson();
            JsonForDesencript _dataEncriptadaReturn = new JsonForDesencript();
            DesencriptarJson _rows = new DesencriptarJson();
            LoginDataCustom _loginDataCustom = new LoginDataCustom();
            oLoginTP _data = new oLoginTP();
            var configuration = new RestApiDSIA().GetConfiguration();
            var dataBearer = configuration.GetSection("ConfigureCustom").GetSection("Bearer").Value;
            string jsonlimpio = "";

            var jsonForEncript = new
            {
                sCveLogin = sCveLogin,
                sClave = configuration.GetSection("ConfigureCustom").GetSection("sClave").Value
            };

            var jsonRaw = new
            {
                sAesKey = configuration.GetSection("ConfigureCustom").GetSection("sClaveLogin").Value,
                sClave = configuration.GetSection("ConfigureCustom").GetSection("sClave").Value,
                sJson = JsonConvert.SerializeObject(jsonForEncript)
            };

            var salida = new RequestClientApi().Post(configuration.GetSection("ServicesAPIDSIA").GetSection("Encriptar").Value, JsonConvert.SerializeObject(jsonRaw), dataBearer);

            if (salida != "" && salida != null)
            {
                _dataReturn = JsonConvert.DeserializeObject<EncriptarJson>(salida);
                var jsonEncriptado = new
                {
                    sEncript = _dataReturn.sEncrip.ToString()
                };

                var salidaEncriptada = new RequestClientApi().Post(configuration.GetSection("ServicesAPIDSIA").GetSection("ObtieneCveLogin_TP").Value, JsonConvert.SerializeObject(jsonEncriptado), dataBearer);

                if (salidaEncriptada != "" && salidaEncriptada != null)
                {
                    _dataEncriptadaReturn = JsonConvert.DeserializeObject<JsonForDesencript>(salidaEncriptada);
                    var jsonDesRaw = new
                    {
                        sAesKey = configuration.GetSection("ConfigureCustom").GetSection("sClaveLogin").Value,
                        sClave = configuration.GetSection("ConfigureCustom").GetSection("sClave").Value,
                        sEncrip = _dataEncriptadaReturn.sEncript.ToString()
                    };

                    var salidaDesencriptada = new RequestClientApi().Post(configuration.GetSection("ServicesAPIDSIA").GetSection("Desencriptar").Value, JsonConvert.SerializeObject(jsonDesRaw), dataBearer);

                    if (salidaDesencriptada != "" && salidaDesencriptada != null)
                    {
                        _rows = JsonConvert.DeserializeObject<DesencriptarJson>(salidaDesencriptada);
                        jsonlimpio = _rows.sJson.Replace(Environment.NewLine, "");
                        jsonlimpio = jsonlimpio.Replace(" ", String.Empty);
                        _loginDataCustom = JsonConvert.DeserializeObject<LoginDataCustom>(jsonlimpio);
                        _data = _loginDataCustom.oLoginTP;
                    }
                }
            }
            return _data;
        }
    }
}
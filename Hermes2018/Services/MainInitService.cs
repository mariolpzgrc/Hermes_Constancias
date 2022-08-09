using Hermes2018.Data;
using Hermes2018.Helpers;
using Hermes2018.Models;
using Hermes2018.Models.Anexo;
using Hermes2018.Models.Area;
using Hermes2018.Models.Calendario;
using Hermes2018.Models.Categoria;
using Hermes2018.Models.Configuracion;
using Hermes2018.Models.Documento;
using Hermes2018.Models.Rol;
using Hermes2018.Models.Tramite;
using Hermes2018.ModelsDBF;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Hermes2018.Services
{
    public class MainInitService : IMainInitService
    {
        private readonly UserManager<HER_Usuario> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _context;
        private readonly DBFContext _DBFContext;

        public MainInitService(
            UserManager<HER_Usuario> userManager,
            RoleManager<IdentityRole> roleManager,
            ApplicationDbContext context,
            DBFContext DBFContext)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
            _DBFContext = DBFContext;
        }

        public void Inicial()
        {
            //Roles
            if (_context.HER_Rol.Count() == 0)
            {
                var roles = new List<HER_Rol>()
                {
                    new HER_Rol { HER_Nombre = ConstRol.Rol1T },
                    new HER_Rol { HER_Nombre = ConstRol.Rol2T },
                    new HER_Rol { HER_Nombre = ConstRol.Rol3T },
                    new HER_Rol { HER_Nombre = ConstRol.Rol4T },
                    new HER_Rol { HER_Nombre = ConstRol.Rol5T },
                    new HER_Rol { HER_Nombre = ConstRol.Rol6T },
                    //new HER_Rol { HER_Nombre = ConstRol.Rol9T },
                    new HER_Rol { HER_Nombre = ConstRol.Rol7T },
                    new HER_Rol { HER_Nombre = ConstRol.Rol8T }
                };
                _context.HER_Rol.AddRange(roles);
                _context.SaveChanges();
            }

            //Regiones
            if (_context.HER_Region.Count() == 0)
            {
                var regiones = new List<HER_Region>()
               {
                    new HER_Region { HER_Nombre = ConstRegion.Region1T },
                    new HER_Region { HER_Nombre = ConstRegion.Region2T },
                    new HER_Region { HER_Nombre = ConstRegion.Region3T },
                    new HER_Region { HER_Nombre = ConstRegion.Region4T },
                    new HER_Region { HER_Nombre = ConstRegion.Region5T }
               };
                _context.HER_Region.AddRange(regiones);
                _context.SaveChanges();
            }

            //Areas
            if (_context.HER_Area.Count() == 0)
            {
                var pareasQuery = _DBFContext.PAREAS
                    .AsTracking()
                    .AsQueryable();

                var pareas = pareasQuery.ToList();

                foreach (var parea in pareas)
                {
                    _context.HER_Area.Add(new HER_Area
                    {
                        HER_Nombre = parea.Nombre,
                        HER_Clave = parea.Clave,
                        HER_DiasCompromiso = (int)parea.Dias,
                        HER_Direccion = parea.Direccion,
                        HER_Telefono = parea.Telefono,
                        HER_RegionId = (int)parea.Region_Id,
                        HER_Area_PadreId = (int?)parea.Area_Padre_Id,
                        HER_LogoId = null,
                        HER_Visible = true,
                        HER_Estado = ConstEstadoArea.EstadoN1,
                        HER_ExisteEnSIIU = (bool)parea.EsSIIU,
                    });
                    _context.SaveChanges();
                }
            }

            //Colores
            if (_context.HER_Color.Count() == 0)
            {
                var color = new HER_Color
                {
                    HER_Nombre = ConstColor.ColorT1,
                    HER_Codigo = ConstColor.ColorC1,
                    //HER_ColeccionId = configuracionColeccion.HER_ConfiguracionColeccionId
                };

                _context.HER_Color.Add(color);
                _context.SaveChanges();
            }

            //Extension
            if (_context.HER_Extension.Count() == 0)
            {
                var extensiones = new List<HER_Extension>
                {
                    new HER_Extension{HER_Nombre = ConstExtension.ExtensionT1, /*HER_ColeccionId = configuracionColeccion.HER_ConfiguracionColeccionId*/ },
                    new HER_Extension{HER_Nombre = ConstExtension.ExtensionT2, /*HER_ColeccionId = configuracionColeccion.HER_ConfiguracionColeccionId*/ },
                    new HER_Extension{HER_Nombre = ConstExtension.ExtensionT3, /*HER_ColeccionId = configuracionColeccion.HER_ConfiguracionColeccionId*/ },
                    new HER_Extension{HER_Nombre = ConstExtension.ExtensionT4, /*HER_ColeccionId = configuracionColeccion.HER_ConfiguracionColeccionId*/},
                    new HER_Extension{HER_Nombre = ConstExtension.ExtensionT5, /*HER_ColeccionId = configuracionColeccion.HER_ConfiguracionColeccionId*/ },
                    new HER_Extension{HER_Nombre = ConstExtension.ExtensionT6, /*HER_ColeccionId = configuracionColeccion.HER_ConfiguracionColeccionId*/ },
                    new HER_Extension{HER_Nombre = ConstExtension.ExtensionT7, /*HER_ColeccionId = configuracionColeccion.HER_ConfiguracionColeccionId*/ },
                    new HER_Extension{HER_Nombre = ConstExtension.ExtensionT8, /*HER_ColeccionId = configuracionColeccion.HER_ConfiguracionColeccionId*/ },
                    new HER_Extension{HER_Nombre = ConstExtension.ExtensionT9, /*HER_ColeccionId = configuracionColeccion.HER_ConfiguracionColeccionId*/ }
                }.AsEnumerable();

                _context.HER_Extension.AddRange(extensiones);
                _context.SaveChanges();
            }

            //Configuracion Coleccion
            if (_context.HER_AnexoGeneral.Count() == 0)
            {
                var provider = new FileExtensionContentTypeProvider();
                //--
                var path1 = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\images\lis_uv.png");
                var nombrefile1 = Path.GetFileName(path1);
                string tipofile1 = string.Empty;

                if (!provider.TryGetContentType(path1, out tipofile1))
                    tipofile1 = "application/octet-stream";

                //--
                var path2 = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\images\hermes_portada2.jpg");
                var nombrefile2 = Path.GetFileName(path2);
                var tipofile2 = string.Empty;

                if (!provider.TryGetContentType(path2, out tipofile2))
                    tipofile2 = "application/octet-stream";

                //--
                var logo_Institucion = new HER_AnexoGeneral
                {
                    HER_Nombre = nombrefile1,
                    HER_TipoContenido = tipofile1,
                    HER_RutaComplemento = @"images",
                    HER_TipoRegistro = ConstConfiguracionGeneral.TipoImagenN1
                };
                _context.HER_AnexoGeneral.Add(logo_Institucion);

                var imagen_Portada = new HER_AnexoGeneral
                {
                    HER_Nombre = nombrefile2,
                    HER_TipoContenido = tipofile2,
                    HER_RutaComplemento = @"images",
                    HER_TipoRegistro = ConstConfiguracionGeneral.TipoImagenN2
                };
                _context.HER_AnexoGeneral.Add(imagen_Portada);

                _context.SaveChanges();
            }

            //Configuración general
            if (_context.HER_Configuracion.Count() == 0)
            {
                var configuraciones = new List<HER_Configuracion>
                {
                    new HER_Configuracion{ HER_Propiedad = ConstConfiguracionGeneral.Propiedad1, HER_Valor = ConstConfiguracionGeneral.Valor1 },
                    new HER_Configuracion{ HER_Propiedad = ConstConfiguracionGeneral.Propiedad4, HER_Valor = ConstConfiguracionGeneral.Valor4 },
                    new HER_Configuracion{ HER_Propiedad = ConstConfiguracionGeneral.Propiedad5, HER_Valor = ConstConfiguracionGeneral.Valor5 },
                    new HER_Configuracion{ HER_Propiedad = ConstConfiguracionGeneral.Propiedad6, HER_Valor = ConstConfiguracionGeneral.Valor6 },
                    new HER_Configuracion{ HER_Propiedad = ConstConfiguracionGeneral.Propiedad7, HER_Valor = ConstConfiguracionGeneral.Valor7 },
                    new HER_Configuracion{ HER_Propiedad = ConstConfiguracionGeneral.Propiedad8, HER_Valor = ConstConfiguracionGeneral.Valor8 },
                    new HER_Configuracion{ HER_Propiedad = ConstConfiguracionGeneral.Propiedad9, HER_Valor = ConstConfiguracionGeneral.Valor9 },
                    new HER_Configuracion{ HER_Propiedad = ConstConfiguracionGeneral.Propiedad10, HER_Valor = ConstConfiguracionGeneral.Valor10  },
                    new HER_Configuracion{ HER_Propiedad = ConstConfiguracionGeneral.Propiedad11, HER_Valor = ConstConfiguracionGeneral.Valor11  },
                    new HER_Configuracion{ HER_Propiedad = ConstConfiguracionGeneral.Propiedad12, HER_Valor = ConstConfiguracionGeneral.Valor12  },
                    new HER_Configuracion{ HER_Propiedad = ConstConfiguracionGeneral.Propiedad13, HER_Valor = ConstConfiguracionGeneral.Valor13  },
                    new HER_Configuracion{ HER_Propiedad = ConstConfiguracionGeneral.Propiedad14, HER_Valor = ConstConfiguracionGeneral.Valor14  },
                    new HER_Configuracion{ HER_Propiedad = ConstConfiguracionGeneral.Propiedad15, HER_Valor = ConstConfiguracionGeneral.Valor15  }
                }.AsEnumerable();

                _context.HER_Configuracion.AddRange(configuraciones);
                _context.SaveChanges();
            }

            //Tipo Documento
            if (_context.HER_TipoDocumento.Count() == 0)
            {
                var tiposDocumento = new List<HER_TipoDocumento>()
                {
                    new HER_TipoDocumento{
                        HER_Nombre = ConstTipoDocumento.TipoDocumentoT1
                    }, 
                    new HER_TipoDocumento{
                        HER_Nombre = ConstTipoDocumento.TipoDocumentoT2
                    }
                };

                _context.HER_TipoDocumento.AddRange(tiposDocumento);
                _context.SaveChanges();
            }

            //Tipo Envio
            if (_context.HER_TipoEnvio.Count() == 0)
            {
                var tiposEnvio = new List<HER_TipoEnvio>()
                {
                    new HER_TipoEnvio{
                        HER_Nombre = ConstTipoEnvio.TipoEnvioT1
                    },
                    new HER_TipoEnvio{
                        HER_Nombre = ConstTipoEnvio.TipoEnvioT2
                    },
                    new HER_TipoEnvio{
                        HER_Nombre = ConstTipoEnvio.TipoEnvioT3
                    },
                    new HER_TipoEnvio{
                        HER_Nombre = ConstTipoEnvio.TipoEnvioT4
                    }
                };

                _context.HER_TipoEnvio.AddRange(tiposEnvio);
                _context.SaveChanges();
            }

            //Importancia Documento
            if (_context.HER_Importancia.Count() == 0)
            {
                var importancia = new List<HER_Importancia>()
                {
                    new HER_Importancia{
                        HER_Nombre = ConstImportancia.ImportanciaT1
                    },
                    new HER_Importancia{
                        HER_Nombre = ConstImportancia.ImportanciaT2
                    }
                };

                _context.HER_Importancia.AddRange(importancia);
                _context.SaveChanges();
            }

            //Estado documento
            if (_context.HER_EstadoDocumento.Count() == 0)
            {
                var estadoDocumento = new List<HER_EstadoDocumento>()
                {
                    new HER_EstadoDocumento{
                        HER_Nombre = ConstEstadoDocumento.EstadoDocumentoT1
                    },
                    new HER_EstadoDocumento{
                        HER_Nombre = ConstEstadoDocumento.EstadoDocumentoT2
                    },
                    //new HER_EstadoDocumento{
                    //    HER_Nombre = ConstEstadoDocumento.EstadoDocumentoT3
                    //},
                    //new HER_EstadoDocumento{
                    //    HER_Nombre = ConstEstadoDocumento.EstadoDocumentoT4
                    //}
                };

                _context.HER_EstadoDocumento.AddRange(estadoDocumento);
                _context.SaveChanges();
            }
            
            //Estado Envio
            if (_context.HER_EstadoEnvio.Count() == 0)
            {
                var estadoEnvio = new List<HER_EstadoEnvio>()
                {
                    new HER_EstadoEnvio{
                        HER_Nombre = ConstEstadoEnvio.EstadoEnvioT1
                    },
                    new HER_EstadoEnvio{
                        HER_Nombre = ConstEstadoEnvio.EstadoEnvioT2
                    },
                    new HER_EstadoEnvio{
                        HER_Nombre = ConstEstadoEnvio.EstadoEnvioT3
                    },
                    new HER_EstadoEnvio{
                        HER_Nombre = ConstEstadoEnvio.EstadoEnvioT4
                    },
                    new HER_EstadoEnvio{
                        HER_Nombre = ConstEstadoEnvio.EstadoEnvioT5
                    },
                    new HER_EstadoEnvio{
                        HER_Nombre = ConstEstadoEnvio.EstadoEnvioT6
                    },
                    new HER_EstadoEnvio{
                        HER_Nombre = ConstEstadoEnvio.EstadoEnvioT7
                    },
                    new HER_EstadoEnvio{
                        HER_Nombre = ConstEstadoEnvio.EstadoEnvioT8
                    }
                };

                _context.HER_EstadoEnvio.AddRange(estadoEnvio);
                _context.SaveChanges();
            }

            //Categoria
            if (_context.HER_Categoria.Count() == 0)
            {
                var categorias = new List<HER_Categoria>()
                {
                    new HER_Categoria{
                        HER_Nombre = ConstCategoria.CategoriaT1,
                        HER_Tipo = ConstTipoCategoria.TipoCategoriaN1,
                        HER_InfoUsuarioId = null
                    }
                };

                _context.HER_Categoria.AddRange(categorias);
                _context.SaveChanges();
            }

            //Calendario
            if (_context.HER_Calendario.Count() == 0)
            {
                var calendario = new HER_Calendario() {
                    HER_Titulo = "Días Festivos - Calendario 2019",
                    HER_Anio = 2019
                };
                _context.HER_Calendario.Add(calendario);

                var contenido = new List<HER_CalendarioContenido>() {
                    new HER_CalendarioContenido(){
                        HER_CalendarioId = calendario.HER_CalendarioId,
                        HER_Mes = 2,
                        HER_Dia = 4,
                        HER_Fecha = new DateTime(2019,2,4,0,0,0)
                    },
                    new HER_CalendarioContenido(){
                        HER_CalendarioId = calendario.HER_CalendarioId,
                        HER_Mes = 2,
                        HER_Dia = 19,
                        HER_Fecha = new DateTime(2019,2,19,0,0,0)
                    },
                    new HER_CalendarioContenido(){
                        HER_CalendarioId = calendario.HER_CalendarioId,
                        HER_Mes = 2,
                        HER_Dia = 20,
                        HER_Fecha = new DateTime(2019,2,20,0,0,0)
                    },
                    new HER_CalendarioContenido(){
                        HER_CalendarioId = calendario.HER_CalendarioId,
                        HER_Mes = 3,
                        HER_Dia = 4,
                        HER_Fecha = new DateTime(2019,3,4,0,0,0)
                    },
                    new HER_CalendarioContenido(){
                        HER_CalendarioId = calendario.HER_CalendarioId,
                        HER_Mes = 3,
                        HER_Dia = 5,
                        HER_Fecha = new DateTime(2019,3,5,0,0,0)
                    },
                    new HER_CalendarioContenido(){
                        HER_CalendarioId = calendario.HER_CalendarioId,
                        HER_Mes = 3,
                        HER_Dia = 6,
                        HER_Fecha = new DateTime(2019,3,6,0,0,0)
                    },
                    new HER_CalendarioContenido(){
                        HER_CalendarioId = calendario.HER_CalendarioId,
                        HER_Mes = 3,
                        HER_Dia = 18,
                        HER_Fecha = new DateTime(2019,3,18,0,0,0)
                    },
                    new HER_CalendarioContenido(){
                        HER_CalendarioId = calendario.HER_CalendarioId,
                        HER_Mes = 4,
                        HER_Dia = 15,
                        HER_Fecha = new DateTime(2019,4,15,0,0,0)
                    },
                    new HER_CalendarioContenido(){
                        HER_CalendarioId = calendario.HER_CalendarioId,
                        HER_Mes = 4,
                        HER_Dia = 16,
                        HER_Fecha = new DateTime(2019,4,16,0,0,0)
                    },
                    new HER_CalendarioContenido(){
                        HER_CalendarioId = calendario.HER_CalendarioId,
                        HER_Mes = 4,
                        HER_Dia = 17,
                        HER_Fecha = new DateTime(2019,4,17,0,0,0)
                    },
                    new HER_CalendarioContenido(){
                        HER_CalendarioId = calendario.HER_CalendarioId,
                        HER_Mes = 4,
                        HER_Dia = 18,
                        HER_Fecha = new DateTime(2019,4,18,0,0,0)
                    },
                    new HER_CalendarioContenido(){
                        HER_CalendarioId = calendario.HER_CalendarioId,
                        HER_Mes = 4,
                        HER_Dia = 19,
                        HER_Fecha = new DateTime(2019,4,19,0,0,0)
                    },
                    new HER_CalendarioContenido(){
                        HER_CalendarioId = calendario.HER_CalendarioId,
                        HER_Mes = 5,
                        HER_Dia = 1,
                        HER_Fecha = new DateTime(2019,5,1,0,0,0)
                    },
                    new HER_CalendarioContenido(){
                        HER_CalendarioId = calendario.HER_CalendarioId,
                        HER_Mes = 5,
                        HER_Dia = 10,
                        HER_Fecha = new DateTime(2019,5,10,0,0,0)
                    },
                    new HER_CalendarioContenido(){
                        HER_CalendarioId = calendario.HER_CalendarioId,
                        HER_Mes = 5,
                        HER_Dia = 15,
                        HER_Fecha = new DateTime(2019,5,15,0,0,0)
                    }
                }.AsEnumerable();

                _context.HER_CalendarioContenido.AddRange(contenido);
                _context.SaveChanges();
            }

            //Visibilidad Envio
            if (_context.HER_Visibilidad.Count()  == 0) 
            {
                var visibilidad = new List<HER_Visibilidad>()
                {
                    new HER_Visibilidad{
                        HER_Nombre = ConstVisibilidad.VisibilidadT1
                    },
                    new HER_Visibilidad{
                        HER_Nombre = ConstVisibilidad.VisibilidadT2
                    }
                };

                _context.HER_Visibilidad.AddRange(visibilidad);
                _context.SaveChanges();
            }
        }
        public async Task UsuariosAsync()
        {
            //Roles
            if (_context.Roles.Count() == 0) 
            {
                //Administrador General
                if (!await _roleManager.RoleExistsAsync(ConstRol.Rol1T))
                {
                    await _roleManager.CreateAsync(new IdentityRole(ConstRol.Rol1T));
                }
                //Administrador Regional Xalapa
                if (!await _roleManager.RoleExistsAsync(ConstRol.Rol2T))
                {
                    await _roleManager.CreateAsync(new IdentityRole(ConstRol.Rol2T));
                }
                //Administrador Regional Veracruz
                if (!await _roleManager.RoleExistsAsync(ConstRol.Rol3T))
                {
                    await _roleManager.CreateAsync(new IdentityRole(ConstRol.Rol3T));
                }
                //Administrador Regional Orizaba-Córdoba
                if (!await _roleManager.RoleExistsAsync(ConstRol.Rol4T))
                {
                    await _roleManager.CreateAsync(new IdentityRole(ConstRol.Rol4T));
                }
                //Administrador Regional Poza Rica-Tuxpan
                if (!await _roleManager.RoleExistsAsync(ConstRol.Rol5T))
                {
                    await _roleManager.CreateAsync(new IdentityRole(ConstRol.Rol5T));
                }
                //Administrador Regional Coatzacoalcos-Minatitlán
                if (!await _roleManager.RoleExistsAsync(ConstRol.Rol6T))
                {
                    await _roleManager.CreateAsync(new IdentityRole(ConstRol.Rol6T));
                }
                //Titular
                if (!await _roleManager.RoleExistsAsync(ConstRol.Rol7T))
                {
                    await _roleManager.CreateAsync(new IdentityRole(ConstRol.Rol7T));
                }
                //Usuario Normal
                if (!await _roleManager.RoleExistsAsync(ConstRol.Rol8T))
                {
                    await _roleManager.CreateAsync(new IdentityRole(ConstRol.Rol8T));
                }
            }
            //Usuarios
            if (_context.HER_Usuario.Count() == 0)
            {
                //Usuario:Administrador General
                var user = new HER_Usuario()
                {
                    SecurityStamp = "UV",
                    UserName = "hermes",
                    Email = "hermes@uv.mx",
                    HER_AceptoTerminos = false,
                    HER_Aprobado = true,
                    HER_FechaAprobado = DateTime.Now,
                    HER_NombreCompleto = "Hermes Administrador General"
                };
                var result = await _userManager.CreateAsync(user, string.Format("{0}{1}", ConstMasterKey.Key1, "hermes"));

                if (result.Succeeded)
                {
                    //Asociacion del usuario con el rol
                    await _userManager.AddToRoleAsync(user, ConstRol.Rol1T);
                }
                //-------------------------------------|
                //Usuario: Administrador Regional (Xalapa)
                var userRegional1 = new HER_Usuario()
                {
                    SecurityStamp = "UV",
                    UserName = "hermesxalapa",
                    Email = "hermesxalapa@uv.mx",
                    HER_AceptoTerminos = false,
                    HER_Aprobado = true,
                    HER_FechaAprobado = DateTime.Now,
                    HER_NombreCompleto = "Administrador Xalapa"
                };
                var resultReginal1 = await _userManager.CreateAsync(userRegional1, string.Format("{0}{1}", ConstMasterKey.Key1, "hermesxalapa"));

                if (resultReginal1.Succeeded)
                {
                    //Asociacion del usuario con el rol
                    await _userManager.AddToRoleAsync(userRegional1, ConstRol.Rol2T);
                }
                //-------------------------------------|
                //Usuario: Administrador Regional (Veracruz)
                var userRegional2 = new HER_Usuario
                {
                    SecurityStamp = "UV",
                    UserName = "hermesveracruz",
                    Email = "hermesveracruz@uv.mx",
                    HER_AceptoTerminos = false,
                    HER_Aprobado = true,
                    HER_FechaAprobado = DateTime.Now,
                    HER_NombreCompleto = "Administrador Veracruz"
                };
                var resultRegional2 = await _userManager.CreateAsync(userRegional2, string.Format("{0}{1}", ConstMasterKey.Key1, "hermesveracruz"));

                if (resultRegional2.Succeeded)
                {
                    //Asociacion del usuario con el rol
                    await _userManager.AddToRoleAsync(userRegional2, ConstRol.Rol3T);
                }
                //-------------------------------------|
                //Usuario: Administrador Regional (orizaba)
                var userRegional3 = new HER_Usuario
                {
                    SecurityStamp = "UV",
                    UserName = "hermesorizaba",
                    Email = "hermesorizaba@uv.mx",
                    HER_AceptoTerminos = false,
                    HER_Aprobado = true,
                    HER_FechaAprobado = DateTime.Now,
                    HER_NombreCompleto = "Administrador Orizaba"
                };
                var resultRegional3 = await _userManager.CreateAsync(userRegional3, string.Format("{0}{1}", ConstMasterKey.Key1, "hermesorizaba"));

                if (resultRegional3.Succeeded)
                {
                    //Asociacion del usuario con el rol
                    await _userManager.AddToRoleAsync(userRegional3, ConstRol.Rol4T);
                }
                //-------------------------------------|
                //Usuario: Administrador Regional (poza rica)
                var userRegional4 = new HER_Usuario
                {
                    SecurityStamp = "UV",
                    UserName = "hermespozarica",
                    Email = "hermespozarica@uv.mx",
                    HER_AceptoTerminos = false,
                    HER_Aprobado = true,
                    HER_FechaAprobado = DateTime.Now,
                    HER_NombreCompleto = "Administrador Poza Rica"
                };
                var resultRegional4 = await _userManager.CreateAsync(userRegional4, string.Format("{0}{1}", ConstMasterKey.Key1, "hermespozarica"));

                if (resultRegional4.Succeeded)
                {
                    //Asociacion del usuario con el rol
                    await _userManager.AddToRoleAsync(userRegional4, ConstRol.Rol5T);
                }
                //-------------------------------------|
                //Usuario: Administrador Regional (coatza)
                var userRegional5 = new HER_Usuario
                {
                    SecurityStamp = "UV",
                    UserName = "hermescoatza",
                    Email = "hermescoatza@uv.mx",
                    HER_AceptoTerminos = false,
                    HER_Aprobado = true,
                    HER_FechaAprobado = DateTime.Now,
                    HER_NombreCompleto = "Administrador Coatzacoalcos"
                };
                var resultRegional5 = await _userManager.CreateAsync(userRegional5, string.Format("{0}{1}", ConstMasterKey.Key1, "hermescoatza"));

                if (resultRegional5.Succeeded)
                {
                    //Asociacion del usuario con el rol
                    await _userManager.AddToRoleAsync(userRegional5, ConstRol.Rol6T);
                }

                //-------------------------------------|-----------------------------------|-----------------------------------|

                HER_Usuario usern;
                HER_InfoUsuario infon;
                HER_ConfiguracionUsuario confign;
                IdentityResult resultn;

                var pusuariosQuery = _DBFContext.PUSUARIOS
                    .AsTracking()
                    .AsQueryable();

                var pusuarios = pusuariosQuery
                    //.Take(1384) //Elementos a tomar
                    //.Skip(1384) //Ignorar
                    .ToList();

                foreach (var pusuario in pusuarios)
                {
                    usern = null;
                    infon = null;
                    resultn = null;
                    confign = null;

                    usern = new HER_Usuario()
                    {
                        SecurityStamp = "UV",
                        UserName = pusuario.SHGUSUARIO_OWNER,
                        Email = pusuario.CORREO,
                        HER_AceptoTerminos = false,
                        HER_Aprobado = true,
                        HER_FechaAprobado = DateTime.Now,
                        HER_NombreCompleto = pusuario.SHGUSUARIO_NOMBRE
                    };

                    resultn = await _userManager.CreateAsync(usern, string.Format("{0}{1}", ConstMasterKey.Key1, pusuario.SHGUSUARIO_OWNER));

                    if (resultn.Succeeded)
                    {
                        confign = new HER_ConfiguracionUsuario
                        {
                            HER_UsuarioId = usern.Id,
                            HER_Notificaciones = pusuario.SHGUSUARIO_ALERTA == 1,
                            HER_ElementosPorPagina = 25,
                            HER_ColorId = 1
                        };
                        _context.HER_ConfiguracionUsuario.Add(confign);

                        if (pusuario.TITULAR == 1)
                        {
                            await _userManager.AddToRoleAsync(usern, ConstRol.Rol7T);

                            infon = new HER_InfoUsuario()
                            {
                                HER_NombreCompleto = pusuario.SHGUSUARIO_NOMBRE,
                                HER_Correo = pusuario.CORREO,
                                HER_UserName = pusuario.SHGUSUARIO_OWNER,
                                HER_Activo = true,
                                HER_EstaEnReasignacion = false,
                                HER_EstaEnBajaDefinitiva = false,
                                HER_FechaRegistro = DateTime.Now,
                                HER_FechaActualizacion = DateTime.Now,
                                HER_EsUnico = true,
                                HER_RolNombre = ConstRol.Rol7T,
                                HER_UsuarioId = usern.Id,
                                HER_Titular = pusuario.SHGUSUARIO_OWNER,
                                HER_BandejaUsuario = pusuario.SHGUSUARIO_OWNER,
                                HER_BandejaPermiso = ConstDelegar.TipoN1,
                                HER_BandejaNombre = pusuario.SHGUSUARIO_NOMBRE,
                                HER_PermisoAA = true,
                                HER_Direccion = pusuario.DIRECCION,
                                HER_Telefono = pusuario.TELEFONO,
                                HER_AreaId = (int)pusuario.AREA_ID,
                                HER_Puesto = pusuario.PUESTO
                            };
                            _context.HER_InfoUsuario.Add(infon);
                        }
                        else
                        {
                            await _userManager.AddToRoleAsync(usern, ConstRol.Rol8T);

                            infon = new HER_InfoUsuario()
                            {
                                HER_NombreCompleto = pusuario.SHGUSUARIO_NOMBRE,
                                HER_Correo = pusuario.CORREO,
                                HER_UserName = pusuario.SHGUSUARIO_OWNER,
                                HER_Activo = true,
                                HER_EstaEnReasignacion = false,
                                HER_EstaEnBajaDefinitiva = false,
                                HER_FechaRegistro = DateTime.Now,
                                HER_FechaActualizacion = DateTime.Now,
                                HER_EsUnico = false,
                                HER_RolNombre = ConstRol.Rol8T,
                                HER_UsuarioId = usern.Id,
                                HER_Titular = pusuario.SHGUSUARIO_OWNER,
                                HER_BandejaUsuario = pusuario.SHGUSUARIO_OWNER,
                                HER_BandejaPermiso = ConstDelegar.TipoN1,
                                HER_BandejaNombre = pusuario.SHGUSUARIO_NOMBRE,
                                HER_PermisoAA = false,
                                HER_Direccion = pusuario.DIRECCION,
                                HER_Telefono = pusuario.TELEFONO,
                                HER_AreaId = (int)pusuario.AREA_ID,
                                HER_Puesto = pusuario.PUESTO
                            };
                            _context.HER_InfoUsuario.Add(infon);
                        }
                        _context.SaveChanges();
                    }
                }
            }
        }

        public void Tramites()
        {
            //Tramites
            if (_context.HER_Tramite.Count() == 0)
            {
                var tramites = new List<HER_Tramite>()
                {
                    new HER_Tramite { HER_Nombre = ConstTramite.TipoT1, HER_Dias = 0, HER_Descripcion = string.Empty, HER_FechaRegistro = DateTime.Now, HER_FechaActualizacion = DateTime.Now, HER_CreadorId = null, HER_Estado = ConstTramiteEstado.EstadoN1 },
                    new HER_Tramite { HER_Nombre = "Fondo rotatorio", 
                                      HER_Dias = 8, 
                                      HER_Descripcion = string.Empty, 
                                      HER_FechaRegistro = DateTime.Now, 
                                      HER_FechaActualizacion = DateTime.Now,
                                      HER_CreadorId = 22, //saf@uv.mx
                                      HER_Estado = ConstTramiteEstado.EstadoN1
                    },
                    new HER_Tramite { HER_Nombre = "Viáticos", HER_Dias = 4, HER_Descripcion = string.Empty, HER_FechaRegistro = DateTime.Now, HER_FechaActualizacion = DateTime.Now, HER_CreadorId = 22,HER_Estado = ConstTramiteEstado.EstadoN1 },
                    new HER_Tramite { HER_Nombre = "Requisión", HER_Dias = 9, HER_Descripcion = string.Empty, HER_FechaRegistro = DateTime.Now, HER_FechaActualizacion = DateTime.Now, HER_CreadorId = 22,HER_Estado = ConstTramiteEstado.EstadoN1 },
                    new HER_Tramite { HER_Nombre = "Movimiento de personal", HER_Dias = 3, HER_Descripcion = string.Empty, HER_FechaRegistro = DateTime.Now, HER_FechaActualizacion = DateTime.Now, HER_CreadorId = 22,HER_Estado = ConstTramiteEstado.EstadoN1 },
                    new HER_Tramite { HER_Nombre = "Transferencia presupuestal", HER_Dias = 7, HER_Descripcion = string.Empty, HER_FechaRegistro = DateTime.Now, HER_FechaActualizacion = DateTime.Now, HER_CreadorId = 22,HER_Estado = ConstTramiteEstado.EstadoN1 }
                };
                _context.HER_Tramite.AddRange(tramites);
                _context.SaveChanges();
            }
        }
    }
}

using Hermes2018.Data;
using Hermes2018.Helpers;
using Hermes2018.Models.Categoria;
using Hermes2018.Models.Documento;
using Hermes2018.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hermes2018.Services
{
    public class CategoriaService: ICategoriaService
    {
        private ApplicationDbContext _context;

        public CategoriaService(ApplicationDbContext context)
        {
            _context = context;
        }

        public bool ActualizarCategoria(ActualizarCategoriaViewModel actualizarCategoria)
        {
            int result = 0;
            var categoriaQuery = _context.HER_Categoria
                .Where(x => x.HER_CategoriaId == actualizarCategoria.CategoriaId
                         && x.HER_InfoUsuario.HER_UserName == actualizarCategoria.Usuario
                         && x.HER_InfoUsuario.HER_Activo == true)
                .AsQueryable();

            var categoria = categoriaQuery.FirstOrDefault();

            //Actualiza el nombre de la categoria
            categoria.HER_Nombre = actualizarCategoria.Nombre;

            _context.HER_Categoria.Update(categoria).State = EntityState.Modified;
            result = _context.SaveChanges();

            return result > 0 ? true : false;
        }
        public bool CategoriaEnUso(int categoriaId, string userName)
        {
            var enUsoDocumentoBaseQuery = _context.HER_CategoriaDocumentoBase
                .Where(x => x.HER_CategoriaId == categoriaId) //&& x.HER_Categoria.HER_InfoUsuario.HER_Activo == true
                .AsNoTracking()
                .AsQueryable();

            var enUsoDocumentoQuery = _context.HER_CategoriaDocumento
                .Where(x => x.HER_CategoriaId == categoriaId) //&& x.HER_Categoria.HER_InfoUsuario.HER_Activo == true
                .AsNoTracking()
                .AsQueryable();

            var enUsoRecepcionQuery = _context.HER_RecepcionCategoria
                .Where(x => x.HER_CategoriaId == categoriaId) //&& x.HER_Categoria.HER_InfoUsuario.HER_Activo == true
                .AsNoTracking()
                .AsQueryable();

            return (enUsoDocumentoBaseQuery.Any() || enUsoDocumentoQuery.Any() || enUsoRecepcionQuery.Any());
        }
        public bool EliminarCategoria(int categoriaId, string userName)
        {
            int result = 0;
            var categoriaQuery = _context.HER_Categoria
                .Where(x => x.HER_CategoriaId == categoriaId
                         && x.HER_InfoUsuario.HER_UserName == userName
                         && x.HER_InfoUsuario.HER_Activo == true)
                .AsQueryable();

            var categoria = categoriaQuery.FirstOrDefault();

            //Borrar la categoría
            _context.HER_Categoria.Remove(categoria);
            result = _context.SaveChanges();

            return result > 0 ? true : false;
        }
        public bool ExisteCategoria(int categoriaId, string userName)
        {
            var existeQuery = _context.HER_Categoria
                .Where(x => x.HER_CategoriaId == categoriaId
                         && x.HER_InfoUsuario.HER_UserName == userName
                         && x.HER_InfoUsuario.HER_Activo == true
                         && x.HER_Tipo == ConstTipoCategoria.TipoCategoriaN2)
                .AsNoTracking()
                .AsQueryable();

            return existeQuery.Any();
        }
        public bool ExisteCategoriaPorNombre(string nombreCategoria, string userName)
        {
            var existeQuery = _context.HER_Categoria
                .Where(x => x.HER_Nombre == nombreCategoria
                         && x.HER_InfoUsuario.HER_UserName == userName
                         && x.HER_InfoUsuario.HER_Activo == true
                         && x.HER_Tipo == ConstTipoCategoria.TipoCategoriaN2)
                .AsNoTracking()
                .AsQueryable();

            return existeQuery.Any();
        }
        public bool GuardarCategoria(NuevaCategoriaViewModel nuevaCategoria)
        {
            int result = 0;
            var nueva = new HER_Categoria()
            {
                HER_Nombre = nuevaCategoria.Nombre,
                HER_Tipo = nuevaCategoria.Tipo,
                HER_InfoUsuarioId = nuevaCategoria.InfoUsuarioId
            };

            _context.HER_Categoria.Add(nueva);
            result = _context.SaveChanges();

            return result > 0 ? true : false;
        }
        public HER_Categoria ObtenerCategoria(int categoriaId, string userName)
        {
            var categoriaQuery = _context.HER_Categoria
                .Where(x => x.HER_CategoriaId == categoriaId
                         && x.HER_InfoUsuario.HER_UserName == userName
                         && x.HER_InfoUsuario.HER_Activo == true)
                         .AsNoTracking()
                         .AsQueryable();

            return categoriaQuery.FirstOrDefault();
        }
        public List<HER_Categoria> ObtenerCategoriasUsuario(string userName)
        {
            var categoriasQuery = _context.HER_Categoria
                                .Where(x => x.HER_InfoUsuario.HER_UserName == userName
                                         && x.HER_InfoUsuario.HER_Activo == true
                                         && x.HER_Tipo == ConstTipoCategoria.TipoCategoriaN2)
                                .AsNoTracking()
                                .AsQueryable();

            return categoriasQuery.ToList();
        }
        //--
        public async Task<List<CategoriaViewModel>> ObtenerCategoriasAsync(string userName)
        {
            List<CategoriaViewModel> listadoCategorias = new List<CategoriaViewModel>();

            //Categoria General
            var categoriasGeneralesQuery = _context.HER_Categoria
                                .Where(x => x.HER_Tipo == ConstTipoCategoria.TipoCategoriaN1)
                                .AsNoTracking()
                                .Select(x => new CategoriaViewModel
                                {
                                    CategoriaId = x.HER_CategoriaId,
                                    Nombre = x.HER_Nombre
                                })
                                .AsQueryable();

            //Categorias del usuario
            var categoriasUsuarioQuery = _context.HER_Categoria
                .Where(x => x.HER_InfoUsuario.HER_UserName == userName
                        && x.HER_InfoUsuario.HER_Activo == true
                        && x.HER_Tipo == ConstTipoCategoria.TipoCategoriaN2)
                 .AsNoTracking()                 
                 .Select(x => new CategoriaViewModel
                 {
                     CategoriaId = x.HER_CategoriaId,
                     Nombre = x.HER_Nombre
                 })
                 .OrderBy(x => x.Nombre)
                .AsQueryable();

            var unionQuery = categoriasGeneralesQuery.Union(categoriasUsuarioQuery);

            return await unionQuery.ToListAsync();
        }
        public async Task<bool> ExisteCategoriaPorNombreAsync(string nombreCategoria, string userName)
        {
            var existeQuery = _context.HER_Categoria
                .Where(x => x.HER_Nombre == nombreCategoria
                         && x.HER_InfoUsuario.HER_UserName == userName
                         && x.HER_InfoUsuario.HER_Activo == true
                         && x.HER_Tipo == ConstTipoCategoria.TipoCategoriaN2)
                .AsNoTracking()
                .AsQueryable();

            return await existeQuery.AnyAsync();
        }
        public async Task<HER_Categoria> GuardarCategoriaAsync(NuevaCategoriaViewModel nuevaCategoria)
        {
            int result = 0;
            var nueva = new HER_Categoria()
            {
                HER_Nombre = nuevaCategoria.Nombre,
                HER_Tipo = nuevaCategoria.Tipo,
                HER_InfoUsuarioId = nuevaCategoria.InfoUsuarioId
            };

            await _context.HER_Categoria.AddAsync(nueva);
            result = await _context.SaveChangesAsync();

            if (result > 0)
            {
                return nueva;
            }
            else {
                return null;
            }
        }
        
        public async Task<List<CategoriaSeleccionadaViewModel>> ObtenerCategoriasSeleccionadasAsync(string folio, string userName)
        {
            List<CategoriaSeleccionadaViewModel> listadoCategorias = new List<CategoriaSeleccionadaViewModel>();
            
            var categoriasSeleccionadasQuery = _context.HER_CategoriaDocumentoBase
                .Where(x => x.HER_DocumentoBase.HER_Folio == folio)
                .Select(x => x.HER_Categoria.HER_CategoriaId)
                .AsQueryable();

            var categoriasSeleccionadas = await categoriasSeleccionadasQuery.ToListAsync();

            //Categoria General
            var categoriasGeneralesQuery = _context.HER_Categoria
                .Where(x => x.HER_Tipo == ConstTipoCategoria.TipoCategoriaN1)
                .Select(x => new CategoriaSeleccionadaViewModel {
                    CategoriaId = x.HER_CategoriaId,
                    Nombre = x.HER_Nombre,
                    Estado = categoriasSeleccionadas.Where(y => categoriasSeleccionadas.Contains(x.HER_CategoriaId)).Any()
                })
                .AsNoTracking()
                .AsQueryable();

            listadoCategorias.AddRange(await categoriasGeneralesQuery.ToListAsync());

            //Categorias del usuario
            var categoriasUsuarioQuery = _context.HER_Categoria
                .Where(x => x.HER_InfoUsuario.HER_UserName == userName
                        && x.HER_InfoUsuario.HER_Activo == true
                        && x.HER_Tipo == ConstTipoCategoria.TipoCategoriaN2)
                .AsNoTracking()
                .Select(x => new CategoriaSeleccionadaViewModel
                {
                    CategoriaId = x.HER_CategoriaId,
                    Nombre = x.HER_Nombre,
                    Estado = categoriasSeleccionadas.Where(y => categoriasSeleccionadas.Contains(x.HER_CategoriaId)).Any()
                })
                .AsQueryable();

            listadoCategorias.AddRange(await categoriasUsuarioQuery.ToListAsync());

            return listadoCategorias;
        }
        public async Task<List<CategoriaSeleccionadaViewModel>> ObtenerCategoriasSeleccionadasEnvioAsync(SolicitudCategoriaJsonModel solicitudCategoriaJson)
        {
            List<CategoriaSeleccionadaViewModel> listadoCategorias = new List<CategoriaSeleccionadaViewModel>();
            List<int> categoriasSeleccionadas = new List<int>();

            //[Todos los tipos de envio]
            //Categorias seleccionadas
            var documentoIdQuery = _context.HER_Envio
                    .Where(x => x.HER_EnvioId == solicitudCategoriaJson.EnvioId
                             && x.HER_Documento.HER_Folio == solicitudCategoriaJson.Folio)
                    .Select(x => x.HER_DocumentoId)
                    .AsQueryable();

            if (await documentoIdQuery.AnyAsync()) 
            {
                var documentoId = await documentoIdQuery.FirstOrDefaultAsync();

                var categoriasDocumentoQuery = _context.HER_CategoriaDocumento
                    .Where(x => x.HER_DocumentoId == documentoId)
                    .Select(x => x.HER_CategoriaId)                 
                    .AsQueryable();

                categoriasSeleccionadas = categoriasDocumentoQuery.ToList();
            }

            //---------------------------
            //Categoria General
            var categoriasGeneralesQuery = _context.HER_Categoria
                .Where(x => x.HER_Tipo == ConstTipoCategoria.TipoCategoriaN1)
                .AsNoTracking()
                .Select(x => new CategoriaSeleccionadaViewModel
                {
                    CategoriaId = x.HER_CategoriaId,
                    Nombre = x.HER_Nombre,
                    Estado = categoriasSeleccionadas.Where(y => categoriasSeleccionadas.Contains(x.HER_CategoriaId)).Any()
                })
                .AsQueryable();

            listadoCategorias.AddRange(await categoriasGeneralesQuery.ToListAsync());

            //Categorias del usuario
            var categoriasUsuarioQuery = _context.HER_Categoria
                .Where(x => x.HER_InfoUsuario.HER_UserName == solicitudCategoriaJson.Usuario
                        && x.HER_InfoUsuario.HER_Activo == true
                        && x.HER_Tipo == ConstTipoCategoria.TipoCategoriaN2)
                 .AsNoTracking()
                 .Select(x => new CategoriaSeleccionadaViewModel
                 {
                     CategoriaId = x.HER_CategoriaId,
                     Nombre = x.HER_Nombre,
                     Estado = categoriasSeleccionadas.Where(y => categoriasSeleccionadas.Contains(x.HER_CategoriaId)).Any()
                 })
                 .OrderBy(x => x.Nombre)
                 .AsQueryable();

            listadoCategorias.AddRange(await categoriasUsuarioQuery.ToListAsync());

            return listadoCategorias;
        }
        public async Task<List<CategoriaSeleccionadaViewModel>> ObtenerCategoriasSeleccionadasRecepcionAsync(SolicitudCategoriaJsonModel solicitudCategoriaJson)
        {
            List<CategoriaSeleccionadaViewModel> listadoCategorias = new List<CategoriaSeleccionadaViewModel>();
            List<int> categoriasSeleccionadas = new List<int>();
            IQueryable<int> categoriasSeleccionadasQuery;

            //[Todos los tipos de envio]
            //Categorias seleccionadas
            categoriasSeleccionadasQuery = _context.HER_RecepcionCategoria
                .Where(x => x.HER_Recepcion.HER_EnvioId == solicitudCategoriaJson.EnvioId
                         && x.HER_Recepcion.HER_Envio.HER_Documento.HER_Folio == solicitudCategoriaJson.Folio
                         && x.HER_Recepcion.HER_Para.HER_UserName == solicitudCategoriaJson.Usuario)
                .Select(x => x.HER_CategoriaId)
                .AsQueryable();

            categoriasSeleccionadas = await categoriasSeleccionadasQuery.ToListAsync();

            //--------------------------------------
            //Categoria General
            var categoriasGeneralesQuery = _context.HER_Categoria
                .Where(x => x.HER_Tipo == ConstTipoCategoria.TipoCategoriaN1)
                .AsNoTracking()
                .Select(x => new CategoriaSeleccionadaViewModel {
                    CategoriaId = x.HER_CategoriaId,
                    Nombre = x.HER_Nombre,
                    Estado = categoriasSeleccionadas.Where(y => categoriasSeleccionadas.Contains(x.HER_CategoriaId)).Any()
                })
                .AsQueryable();

            listadoCategorias.AddRange(await categoriasGeneralesQuery.ToListAsync());

            //Categorias del usuario
            var categoriasUsuarioQuery = _context.HER_Categoria
                .Where(x => x.HER_InfoUsuario.HER_UserName == solicitudCategoriaJson.Usuario
                        && x.HER_InfoUsuario.HER_Activo == true
                        && x.HER_Tipo == ConstTipoCategoria.TipoCategoriaN2)
                .AsNoTracking()
                .Select(x => new CategoriaSeleccionadaViewModel
                {
                    CategoriaId = x.HER_CategoriaId,
                    Nombre = x.HER_Nombre,
                    Estado = categoriasSeleccionadas.Where(y => categoriasSeleccionadas.Contains(x.HER_CategoriaId)).Any()
                })
                .OrderBy(x => x.Nombre)
                .AsQueryable();

            listadoCategorias.AddRange(await categoriasUsuarioQuery.ToListAsync());

            return listadoCategorias;
        }
        
        public async Task<bool> ActualizarRecepcionCategoriaAsync(ActualizacionCategoriaDocumentoJsonModel actualizacionJsonModel)
        {
            int result = 0;
            bool estado = false;
            //Categorias
            var categoriasNuevas = actualizacionJsonModel.Categorias.Split(',').ToList();

            //RecepcionId 
            var recepcionIdQuery = _context.HER_Recepcion
                .Where(x => x.HER_EnvioId == actualizacionJsonModel.EnvioId
                        && x.HER_Para.HER_UserName == actualizacionJsonModel.Usuario
                        && x.HER_Para.HER_Activo == true)
                .Select(x => x.HER_RecepcionId)
                .AsQueryable();

            var recepcionId = await recepcionIdQuery.FirstOrDefaultAsync();

            if (recepcionId > 0)
            {
                var categoriasActualesQuery = _context.HER_RecepcionCategoria
                    .Where(x => x.HER_RecepcionId == recepcionId)
                    .AsQueryable();

                var categoriasActuales = await categoriasActualesQuery.ToListAsync();

                _context.HER_RecepcionCategoria.RemoveRange(categoriasActuales);

                List<HER_RecepcionCategoria> listado = new List<HER_RecepcionCategoria>();
                foreach (var categoriaId in categoriasNuevas)
                {
                    listado.Add(new HER_RecepcionCategoria()
                    {
                        HER_CategoriaId = int.Parse(categoriaId),
                        HER_RecepcionId = recepcionId
                    });
                }

                await _context.HER_RecepcionCategoria.AddRangeAsync(listado);
                result = await _context.SaveChangesAsync();

                estado = result > 0 ? true : false;
            }

            return estado;
        }

        //[**CATEGORIAS**]
        public async Task<bool> GuardarCategoriasDocumentoAsync(List<string> listaCategorias, int documentoId)
        {
            //Crear asociación de categorias
            List<HER_CategoriaDocumento> listadoCategorias = new List<HER_CategoriaDocumento>();
            bool estado = false;
            //--

            if (listaCategorias.Count == 0)
            {
                var categoriaGeneralQuery = _context.HER_Categoria
                    .Where(x => x.HER_Nombre == ConstCategoria.CategoriaT1)
                    .Select(x => x.HER_CategoriaId)
                    .AsQueryable();

                var categoriaGeneral = categoriaGeneralQuery.FirstOrDefault();
                listaCategorias.Add(categoriaGeneral.ToString());
            }

            foreach (var categoriaId in listaCategorias)
            {
                listadoCategorias.Add(new HER_CategoriaDocumento()
                {
                    HER_CategoriaId = Int32.Parse(categoriaId),
                    HER_DocumentoId = documentoId
                });
            }

            _context.HER_CategoriaDocumento.AddRange(listadoCategorias);
            var result = await _context.SaveChangesAsync();
            estado = (result > 0) ? true : false;

            return estado;
        }
        public async Task<bool> GuardarCategoriasDocumentoAsync(int documentoId, int documentoBaseId)
        {
            //Crear asociación de categorias
            List<HER_CategoriaDocumento> listadoCategorias = new List<HER_CategoriaDocumento>();
            bool estado = false;

            //Buscamos el documento  
            var categoriasQuery = _context.HER_CategoriaDocumentoBase
                .Where(x => x.HER_DocumentoBase.HER_DocumentoBaseId == documentoBaseId)
                .Select(x => x.HER_CategoriaId)
                .AsQueryable();

            var categorias = await categoriasQuery.ToListAsync();

            if (categorias.Count > 0)
            {
                foreach (var categoriaId in categorias)
                {
                    listadoCategorias.Add(new HER_CategoriaDocumento()
                    {
                        HER_CategoriaId = categoriaId,
                        HER_DocumentoId = documentoId
                    });
                }

                _context.HER_CategoriaDocumento.AddRange(listadoCategorias);
                var result = await _context.SaveChangesAsync();

                estado = (result > 0) ? true : false;
            }

            return estado;
        }
        public async Task<bool> ActualizarCategoriasDocumentoAsync(List<CategoriaDocumentoViewModel> listCategoriasView, string folio)
        {
            var estado = false;

            //Buscamos el documento  
            var datosDocumentoQuery = _context.HER_CategoriaDocumento
                                .Where(x => x.HER_Documento.HER_Folio == folio)
                                .AsQueryable();

            var datosDocumento = await datosDocumentoQuery.ToListAsync();

            //Existe el documento
            if (datosDocumento.Count > 0)
            {
                //Hay categorias
                if (listCategoriasView.Count > 0)
                {
                    //Eliminacion de las categorias anteriormente registradas
                    _context.HER_CategoriaDocumento.RemoveRange(datosDocumento);

                    //Agregar las nuevas categorias
                    List<HER_CategoriaDocumento> listadoCategorias = new List<HER_CategoriaDocumento>();
                    //--
                    foreach (var categoriaView in listCategoriasView)
                    {
                        listadoCategorias.Add(new HER_CategoriaDocumento()
                        {
                            HER_CategoriaId = categoriaView.CategoriaId,
                            HER_DocumentoId = categoriaView.DocumentoId
                        });
                    }
                    //--
                    _context.HER_CategoriaDocumento.AddRange(listadoCategorias);
                    var result = await _context.SaveChangesAsync();

                    estado = (result > 0) ? true : false;
                }
            }

            return estado;
        }
        //--
        public async Task<bool> GuardarCategoriasDocumentoBaseAsync(List<string> listaCategorias, int documentoBaseId)
        {
            //Crear asociación de categorias
            List<HER_CategoriaDocumentoBase> listadoCategorias = new List<HER_CategoriaDocumentoBase>();
            bool estado = false;
            //--

            if (listaCategorias.Count == 0)
            {
                var categoriaGeneralQuery = _context.HER_Categoria
                    .Where(x => x.HER_Nombre == ConstCategoria.CategoriaT1)
                    .Select(x => x.HER_CategoriaId)
                    .AsQueryable();

                var categoriaGeneral = categoriaGeneralQuery.FirstOrDefault();
                listaCategorias.Add(categoriaGeneral.ToString());
            }

            foreach (var categoriaId in listaCategorias)
            {
                listadoCategorias.Add(new HER_CategoriaDocumentoBase()
                {
                    HER_CategoriaId = Int32.Parse(categoriaId),
                    HER_DocumentoBaseId = documentoBaseId
                });
            }

            _context.HER_CategoriaDocumentoBase.AddRange(listadoCategorias);
            var result = await _context.SaveChangesAsync();
            estado = (result > 0) ? true : false;

            return estado;
        }
        public async Task<bool> ActualizarCategoriasDocumentoBaseAsync(List<string> listaNuevasCategorias, int documentoBaseId)
        {
            var estado = false;
            var listaCategorias = listaNuevasCategorias.Select(x => int.Parse(x)).ToList();

            //Buscamos el documento  
            var datosQuery = _context.HER_CategoriaDocumentoBase
                .Where(x => x.HER_DocumentoBase.HER_DocumentoBaseId == documentoBaseId)
                .AsQueryable();

            var datos = await datosQuery.ToListAsync();
            
            //Existe el documento
            if (datos.Count > 0)
            {
                var categoriasRegistradas = datosQuery.Select(x => x.HER_CategoriaId).ToList();

                //Hay categorias nuevas
                if (listaCategorias.Count > 0)
                {
                    //Detecta cuando contiene las mismas categorias
                    var contiene = listaCategorias.All(categoriasRegistradas.Contains) && listaCategorias.Count == categoriasRegistradas.Count;

                    if (!contiene)
                    {
                        //Eliminacion de las categorias anteriormente registradas
                        _context.HER_CategoriaDocumentoBase.RemoveRange(datos);

                        //Agregar las nuevas categorias
                        List<HER_CategoriaDocumentoBase> listado = new List<HER_CategoriaDocumentoBase>();

                        foreach (var categoriaId in listaCategorias)
                        {
                            listado.Add(new HER_CategoriaDocumentoBase()
                            {
                                HER_CategoriaId = categoriaId,
                                HER_DocumentoBaseId = documentoBaseId
                            });
                        }

                        _context.HER_CategoriaDocumentoBase.AddRange(listado);
                        var result = await _context.SaveChangesAsync();

                        estado = (result > 0) ? true : false;
                    }
                }
            }

            return estado;
        }
        //--
        public async Task<bool> GuardarCategoriasDeDocumentoRespuestaAsync(List<string> listaCategorias, int documentoId)
        {
            //Crear asociación de categorias
            List<HER_CategoriaDocumento> listadoCategorias = new List<HER_CategoriaDocumento>();
            bool estado = false;
            //--

            if (listaCategorias.Count == 0)
            {
                var categoriaGeneralQuery = _context.HER_Categoria
                    .Where(x => x.HER_Nombre == ConstCategoria.CategoriaT1)
                    .Select(x => x.HER_CategoriaId)
                    .AsQueryable();

                var categoriaGeneral = categoriaGeneralQuery.FirstOrDefault();
                listaCategorias.Add(categoriaGeneral.ToString());
            }

            foreach (var categoriaId in listaCategorias)
            {
                listadoCategorias.Add(new HER_CategoriaDocumento()
                {
                    HER_CategoriaId = Int32.Parse(categoriaId),
                    HER_DocumentoId = documentoId
                });
            }

            _context.HER_CategoriaDocumento.AddRange(listadoCategorias);
            var result = await _context.SaveChangesAsync();
            estado = (result > 0) ? true : false;

            return estado;
        }
    }
}

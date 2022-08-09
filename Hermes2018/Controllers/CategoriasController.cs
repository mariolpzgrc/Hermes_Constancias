using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hermes2018.Data;
using Hermes2018.Helpers;
using Hermes2018.Models.Categoria;
using Hermes2018.Models.Documento;
using Hermes2018.Services;
using Hermes2018.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hermes2018.Controllers
{
    [Authorize(Roles = ConstRol.Rol7T + "," + ConstRol.Rol8T)]
    public class CategoriasController : Controller
    {
        private readonly IUsuarioClaimService _usuarioClaimService;
        private readonly IUsuarioService _usuarioService;
        private readonly ICategoriaService _categoriaService;

        public CategoriasController(IUsuarioClaimService usuarioClaimService,
                                    IUsuarioService usuarioService,
                                    ICategoriaService categoriaService)
        {
            _usuarioClaimService = usuarioClaimService;
            _usuarioService = usuarioService;
            _categoriaService = categoriaService;
        }

        public ActionResult Index()
        {
            //Información del usuario logueado
            var infoUsuarioClaims = _usuarioClaimService.ObtenerInfoUsuarioClaims(User);
            
            //Categorias del usuario
            List<HER_Categoria> categorias = _categoriaService.ObtenerCategoriasUsuario(infoUsuarioClaims.BandejaUsuario);

            ViewData["ActivaDelegacion"] = infoUsuarioClaims.ActivaDelegacion;
            ViewData["BandejaPermiso"] = infoUsuarioClaims.BandejaPermiso;
            return View(categorias);
        }

        public ActionResult Detalles(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //Información del usuario logueado
            var infoUsuarioClaims = _usuarioClaimService.ObtenerInfoUsuarioClaims(User);
            var existe = _categoriaService.ExisteCategoria((int)id, infoUsuarioClaims.BandejaUsuario);
            HER_Categoria categoria;

            if (existe)
            {
                categoria = _categoriaService.ObtenerCategoria((int)id, infoUsuarioClaims.BandejaUsuario);
            }
            else {
                return NotFound();
            }

            return View(categoria);
        }

        public ActionResult Crear()
        {
            var infoUsuarioClaims = _usuarioClaimService.ObtenerInfoUsuarioClaims(User);
            if (infoUsuarioClaims.ActivaDelegacion && infoUsuarioClaims.BandejaPermiso == ConstDelegar.TipoN2)
            {
                return RedirectToAction("Index", new { area = "", id = "" });
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Crear(CrearCategoriaViewModel categoriaView)
        {
            if (ModelState.IsValid)
            {
                //Información del usuario logueado
                var infoUsuarioClaims = _usuarioClaimService.ObtenerInfoUsuarioClaims(User);
                var usuarioId = await _usuarioService.ObtenerIdentificadorUsuarioAsync(infoUsuarioClaims.BandejaUsuario);

                //Valida si existe la categoria
                var existeCategoria = _categoriaService.ExisteCategoriaPorNombre(categoriaView.Nombre, infoUsuarioClaims.BandejaUsuario);
                
                if (!existeCategoria)
                {
                    var nueva = new NuevaCategoriaViewModel()
                    {
                        Nombre = categoriaView.Nombre,
                        Tipo = ConstTipoCategoria.TipoCategoriaN2,
                        InfoUsuarioId = usuarioId
                    };

                    var result = _categoriaService.GuardarCategoria(nueva);

                    if (result)
                    {
                        return RedirectToAction("Index");
                    }
                    else {
                        ModelState.AddModelError(string.Empty, "Ha ocurrido un error inténtelo más tarde.");
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "El nombre de la categoría que usted ha escrito ya se encuentra registrada.");
                }
            }

            return View(categoriaView);
        }

        public ActionResult Editar(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var infoUsuarioClaims = _usuarioClaimService.ObtenerInfoUsuarioClaims(User);
            if (infoUsuarioClaims.ActivaDelegacion && infoUsuarioClaims.BandejaPermiso == ConstDelegar.TipoN2)
            {
                return RedirectToAction("Index", new { area = "", id = "" });
            }

            //Categoria
            var existe = _categoriaService.ExisteCategoria((int)id, infoUsuarioClaims.BandejaUsuario);
            if (!existe) {
                return NotFound();
            }

            var categoria = _categoriaService.ObtenerCategoria((int)id, infoUsuarioClaims.BandejaUsuario);
            var categoriaView = new EditarCategoriaViewModel()
            {
                CategoriaId = categoria.HER_CategoriaId,
                Nombre = categoria.HER_Nombre
            };

            return View(categoriaView);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Editar(EditarCategoriaViewModel editarCategoriaView)
        {
            if (ModelState.IsValid)
            {
                var infoUsuarioClaims = _usuarioClaimService.ObtenerInfoUsuarioClaims(User);
                var existe = _categoriaService.ExisteCategoria(editarCategoriaView.CategoriaId, infoUsuarioClaims.BandejaUsuario);
                
                if(existe)
                {
                    //Valida si ya esta en uso
                    var enUso = _categoriaService.CategoriaEnUso(editarCategoriaView.CategoriaId, infoUsuarioClaims.BandejaUsuario);

                    if (!enUso)
                    {
                        var actualizacion = new ActualizarCategoriaViewModel() {
                            CategoriaId = editarCategoriaView.CategoriaId,
                            Nombre = editarCategoriaView.Nombre,
                            Usuario = infoUsuarioClaims.BandejaUsuario
                        };

                        var result = _categoriaService.ActualizarCategoria(actualizacion);
                        if (result)
                        {
                            return RedirectToAction(nameof(Index));
                        }
                        else {
                            ModelState.AddModelError(string.Empty, "Ha ocurrido un error inténtelo más tarde.");
                        }
                    }
                    else {
                        ModelState.AddModelError(string.Empty, "La categoría que usted desea editar, se encuentra relacionada con algun oficio.");
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "La categoría que usted desea editar, no se encuentra.");
                }
            }

            return View(editarCategoriaView);
        }

        public ActionResult Borrar(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var infoUsuarioClaims = _usuarioClaimService.ObtenerInfoUsuarioClaims(User);
            if (infoUsuarioClaims.ActivaDelegacion && infoUsuarioClaims.BandejaPermiso == ConstDelegar.TipoN2)
            {
                return RedirectToAction("Index", new { area = "", id = "" });
            }

            var existe = _categoriaService.ExisteCategoria((int)id, infoUsuarioClaims.BandejaUsuario);
            if (!existe)
            {
                return NotFound();
            }
            var categoria = _categoriaService.ObtenerCategoria((int)id, infoUsuarioClaims.BandejaUsuario);

            return View(categoria);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Borrar(int id)
        {
            var infoUsuarioClaims = _usuarioClaimService.ObtenerInfoUsuarioClaims(User);
            var existe = _categoriaService.ExisteCategoria(id, infoUsuarioClaims.BandejaUsuario);

            if (existe)
            {
                //Valida si ya esta en uso
                var enUso = _categoriaService.CategoriaEnUso(id, infoUsuarioClaims.BandejaUsuario);
                var categoria = _categoriaService.ObtenerCategoria(id, infoUsuarioClaims.BandejaUsuario);

                if (!enUso)
                {
                    var result = _categoriaService.EliminarCategoria(id, infoUsuarioClaims.BandejaUsuario);
                    if (result) {
                        return RedirectToAction("Index");
                    }
                    else{
                        ModelState.AddModelError(string.Empty, "Ha ocurrido un error inténtelo más tarde.");
                        return View(categoria);
                    }
                }
                else {
                    ModelState.AddModelError(string.Empty, "La categoría que usted desea borrar, se encuentra relacionada con algun oficio.");
                    return View(categoria);
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, "La categoría que usted desea borrar, no se encuentra.");
                return RedirectToAction(nameof(Borrar), new { id = id });
            }
        }
    }
}
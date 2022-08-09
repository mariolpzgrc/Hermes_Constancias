using Hermes2018.Data;
using Hermes2018.Helpers;
using Hermes2018.Models;
using Hermes2018.Services;
using Hermes2018.ViewComponentsModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Hermes2018.ViewComponents
{
	public class MenuDelegarViewComponent : ViewComponent
	{
        private readonly IUsuarioClaimService _usuarioClaimService;
        private readonly IUsuarioService _usuarioService;

        public MenuDelegarViewComponent(IUsuarioClaimService usuarioClaimService,
            IUsuarioService usuarioService)
		{
            _usuarioClaimService = usuarioClaimService;
            _usuarioService = usuarioService;

        }

		public async Task<IViewComponentResult> InvokeAsync(ClaimsPrincipal User, string Leyenda)
		{
			var infoUsuarioClaims = _usuarioClaimService.ObtenerInfoUsuarioClaims(User);

            var listadobandejas = new List<BandejasViewComponentModel>
            {
                //Usuario actual
                new BandejasViewComponentModel
                {
                    Usuario = infoUsuarioClaims.UserName,
                    NombreCompleto = string.Format("{0} ({1})", infoUsuarioClaims.FullName, infoUsuarioClaims.UserName),
                    Tipo = "Actual"
                }
            };

            if (!string.IsNullOrEmpty(infoUsuarioClaims.CuentaPersonal)) 
            {
                listadobandejas.Add(new BandejasViewComponentModel
                {
                    Usuario = infoUsuarioClaims.CuentaPersonal,
                    NombreCompleto = string.Format("{0} ({1})", infoUsuarioClaims.FullName, infoUsuarioClaims.CuentaPersonal),
                    Tipo = ConstDelegar.TipoTS2 // Cuenta Personal Revisor para los usuarios que tienen cuenta dependencias
                });
            }

            listadobandejas.AddRange(await _usuarioService.ObtenerUsuariosDelegadosAsync(infoUsuarioClaims.UserName));

            var viewcomponentmodel = new CambiarBandejaViewComponentModel()
			{
				Bandejas = listadobandejas,
				Usuario = infoUsuarioClaims.BandejaUsuario,
                Leyenda = Leyenda
            };

			return View(viewcomponentmodel);
		}
	}
}

using Hermes2018.Data;
using Hermes2018.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Hermes2018.Controllers.Api
{
    [Produces("application/json")]
    [Route("/api/v1/")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [DisableCors]
    public class ApiController : ControllerBase
    {
        /*                  NUEVO API HERMES 2018
         * Para crear nuevas rutas para la api, favor de tomar en cuenta la estrutura que se toma en los archivos dentro
         * de las carpetas correspondientes.
         * 
         * Esta sera la version 1.0 del Api.
         * 
         * La estructura de las rutas que se creen seran las siguientes:
         * 
         *      [dominio]/api/v1/[nombre del controlador]/[subnombre controlador]/[variables]
         *      
         * Ejemplos:
         *      /api/v1/extensiones/imgs
         *      /api/v1/users/ldap/jecastilla
         *      /api/v1/colors/
         * 
         * Consideraciones:
         *      -No usar nombres de funciones para las rutas(getnombres(), getvideos(), etc.)
         *      -Usar siempre sustantivos en plural para referirse a las entidades de las rutas(colors, oficios, extensioones, etc.)
         * 
         */

    }
}
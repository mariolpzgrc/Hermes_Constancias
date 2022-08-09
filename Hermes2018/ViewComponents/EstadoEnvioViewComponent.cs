using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hermes2018.ViewComponents
{
    public class EstadoEnvioViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(int estado)
        {
            return View(estado);
        }
    }
}

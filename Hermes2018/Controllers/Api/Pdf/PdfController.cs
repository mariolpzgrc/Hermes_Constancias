using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DinkToPdf;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using DinkToPdf.Contracts;
using System.IO;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Hermes2018.Controllers.Api.Pdf
{
    [Route("api/pdf")]
    [Authorize]
    [ApiController]
    public class ConstanciasController : Controller
    {
        private readonly IConverter _converter;

        public ConstanciasController(IConverter converter)
        {
            _converter = converter;
        }

        [HttpPost]
        public IActionResult GetPdf([FromBody]dynamic data)
        {
            string htmlString = data.htmlString;
            HtmlToPdfDocument doc = new HtmlToPdfDocument()
            {
                GlobalSettings = {
                    ColorMode = ColorMode.Color,
                    Orientation = Orientation.Portrait,
                    PaperSize = PaperKind.Letter,
                    Margins = new MarginSettings(10.0,10.0,30.0,0.0)
                },
                Objects = {
                    new ObjectSettings() {
                        PagesCount = true,
                        HtmlContent = htmlString,
                        WebSettings = { DefaultEncoding = "utf-8", UserStyleSheet = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "css", "pdfStyle.css") },
                        FooterSettings = { FontSize = 8,
                            //Right = areaSuperior + "\n" + area + "\n" + region + "\n\n" + "Página [page] de [toPage]",
                            Right =  "Página [page] de [toPage]",
                            Spacing = 20,
                            FontName= "Gill Sans MT"
                        }
                    }
                }
            };
            byte[] pdf = _converter.Convert(doc);
            return File(pdf, "application/pdf");
        }

    }
}

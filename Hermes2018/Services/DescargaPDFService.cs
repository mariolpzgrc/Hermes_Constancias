using Hermes2018.ViewModels;
using Microsoft.AspNetCore.Hosting;
using iTextSharp.text;
using iTextSharp.text.pdf;
using HtmlAgilityPack;
using iTextSharp.text.html.simpleparser;
using System;
using System.Linq;
using System.IO;
using Microsoft.Extensions.Logging;
using Hermes2018.Helpers;
using System.Globalization;

namespace Hermes2018.Services
{
    public class DescargarPDFService : IDescargarPDF
    {
        private readonly IDocumentoService _documentoService;
        private readonly IHostingEnvironment _environment;
        private readonly ILogger<DescargarPDFService> _logger;

        public DescargarPDFService(IDocumentoService documentoService, IHostingEnvironment environment, ILogger<DescargarPDFService> logger)
        {
            _documentoService = documentoService;
            _environment = environment;
            _logger = logger;
        }

        public byte[] DescargarPDF(DescargaPDFViewModel modelo, byte[] logo, string baseUrl, string usuario)
        {
            Font General = FontFactory.GetFont("Gill Sans MT", 10, BaseColor.DarkGray);
            Font GeneralItalic = FontFactory.GetFont("Gill Sans MT", 10, Font.ITALIC, BaseColor.DarkGray);
            Font HF = FontFactory.GetFont("Gill Sans MT", 10, BaseColor.DarkGray);
            Font HFBold = FontFactory.GetFont("Gill Sans MT", 10, Font.BOLD, BaseColor.DarkGray);
            Font Small = FontFactory.GetFont("Gill Sans MT", 8, BaseColor.DarkGray);
            Font SmallBold = FontFactory.GetFont("Gill Sans MT", 8, Font.BOLD, BaseColor.DarkGray);
            Font ExtraSmall = FontFactory.GetFont("Gill Sans MT", 7, BaseColor.DarkGray);

            var styles = new StyleSheet();
            // set the default font's properties
            styles.LoadTagStyle("table", "border", ".3");
            styles.LoadTagStyle("table", "width", "380");
            styles.LoadTagStyle("table", "style", "font-family:Gill Sans MT");
            styles.LoadTagStyle("table", "style", "font-size:10pt");
            styles.LoadTagStyle("caption", "color", "#303030");
            styles.LoadTagStyle("th", "style", "font-family:Gill Sans MT");
            styles.LoadTagStyle("th", "style", "font-size:8pt;font-weight:bold;");
            styles.LoadTagStyle("th", "style", "font-size:10pt");
            styles.LoadTagStyle("tr", "style", "font-size:10pt");
            styles.LoadTagStyle("td", "style", "font-family:Gill Sans MT");
            styles.LoadTagStyle("td span p div", "style", "font-size:10pt");
            styles.LoadTagStyle("span", "style", "font-family:Gill Sans MT");
            styles.LoadTagStyle("span", "style", "font-size:8pt");
            styles.LoadTagStyle("p", "style", "font-family:Gill Sans MT");
            styles.LoadTagStyle("p", "style", "font-size:10pt");
            styles.LoadTagStyle("body", "text-align", "right");
            styles.LoadTagStyle("body", "encoding", BaseFont.IDENTITY_H);
            styles.LoadTagStyle("body", "style", "font-family:Gill Sans MT");
            styles.LoadTagStyle("body", "style", "font-size:10pt");
            styles.LoadTagStyle("body", "color", "#303030");
            styles.LoadTagStyle("hr", "style", "width:100%");
            styles.LoadTagStyle("ul", "indent", "15");
            styles.LoadTagStyle("ol", "indent", "15");
            styles.LoadTagStyle("li", "style", "margin-left:2em");
            styles.LoadTagStyle("ul", "style", "padding-left:2em");
            styles.LoadTagStyle("ul", "style", "font-family:Gill Sans MT");
            styles.LoadTagStyle("ul", "style", "font-size:10pt");
            styles.LoadTagStyle("ol", "style", "font-family:Gill Sans MT");
            styles.LoadTagStyle("ol", "style", "font-size:10pt");
            styles.LoadTagStyle("li", "style", "font-family:Gill Sans MT");
            styles.LoadTagStyle("li", "style", "font-size:10pt");
            styles.LoadTagStyle("br", "style", "font-size:1em");

            using (var stream = new MemoryStream())
            {
                var document = new Document(PageSize.Letter);
                var writer = PdfWriter.GetInstance(document, stream);
                document.AddAuthor("UV");
                document.Open();


                if (!string.IsNullOrEmpty(modelo.Indicaciones))
                {
                    document.NewPage();
                    PdfPTable tableIndicaciones = new PdfPTable(4);
                    tableIndicaciones.TotalWidth = 550f;
                    tableIndicaciones.LockedWidth = true;
                    float[] widthsIndicaciones = new float[] { 14f, 8f, 30f, 30f };
                    tableIndicaciones.SetWidths(widthsIndicaciones);

                    PdfPCell cellVaciaInd = new PdfPCell(new Phrase(""));
                    cellVaciaInd.Colspan = 1;
                    cellVaciaInd.HorizontalAlignment = Element.ALIGN_RIGHT;
                    cellVaciaInd.Border = Rectangle.NO_BORDER;

                    PdfPCell cellineaInd = new PdfPCell(new Phrase(""));
                    cellineaInd.Colspan = 4;
                    cellineaInd.HorizontalAlignment = Element.ALIGN_RIGHT;
                    cellineaInd.Border = Rectangle.NO_BORDER;

                    // Logo
                    var fileLogoforInd = logo;
                    var pngLogoforInd = iTextSharp.text.Image.GetInstance(fileLogoforInd);
                    pngLogoforInd.ScaleAbsolute(120f, 100f);
      
                    PdfPCell celllogoIndicaciones = new PdfPCell(pngLogoforInd);
                    celllogoIndicaciones.Colspan = 4;
                    celllogoIndicaciones.HorizontalAlignment = Element.ALIGN_RIGHT;
                    celllogoIndicaciones.Border = Rectangle.NO_BORDER;
                    tableIndicaciones.AddCell(celllogoIndicaciones);

                    PdfPCell cellRegionIndicaciones = new PdfPCell(new Phrase("Región " + modelo.Region, SmallBold));
                    cellRegionIndicaciones.Colspan = 4;
                    cellRegionIndicaciones.HorizontalAlignment = Element.ALIGN_RIGHT;
                    cellRegionIndicaciones.Border = Rectangle.NO_BORDER;
                    tableIndicaciones.AddCell(cellRegionIndicaciones);

                    PdfPCell cellAsuntoIndicaciones = new PdfPCell(new Phrase(modelo.Asunto, Small));
                    cellAsuntoIndicaciones.Colspan = 4;
                    cellAsuntoIndicaciones.HorizontalAlignment = Element.ALIGN_RIGHT;
                    cellAsuntoIndicaciones.Border = Rectangle.NO_BORDER;
                    tableIndicaciones.AddCell(cellAsuntoIndicaciones);

                    DateTime fr = DateTime.Parse(modelo.FechaRecepcion);

                    PdfPCell cellFechaRecepcion = new PdfPCell(new Phrase(fr.ToString("dddd, dd MMMM yyyy", CultureInfo.CreateSpecificCulture("es-MX")), Small));
                    cellFechaRecepcion.Colspan = 4;
                    cellFechaRecepcion.HorizontalAlignment = Element.ALIGN_RIGHT;
                    cellFechaRecepcion.Border = Rectangle.NO_BORDER;
                    tableIndicaciones.AddCell(cellFechaRecepcion);

                    if (!string.IsNullOrEmpty(modelo.FechaCompromiso))
                    {
                        DateTime fc = DateTime.Parse(modelo.FechaCompromiso);

                        PdfPCell cellFechaCompromiso = new PdfPCell(new Phrase("Fecha Compromiso " + fc.ToString("dddd, dd MMMM yyyy", CultureInfo.CreateSpecificCulture("es-MX")), Small));
                        cellFechaCompromiso.Colspan = 4;
                        cellFechaCompromiso.HorizontalAlignment = Element.ALIGN_RIGHT;
                        cellFechaCompromiso.Border = Rectangle.NO_BORDER;
                        tableIndicaciones.AddCell(cellFechaCompromiso);
                    }

                    PdfPCell folioInd = new PdfPCell(new Phrase("Folio: " + modelo.Folio, HFBold));
                    folioInd.Colspan = 4;
                    folioInd.HorizontalAlignment = Element.ALIGN_RIGHT;
                    folioInd.Border = Rectangle.NO_BORDER;
                    tableIndicaciones.AddCell(folioInd);

                    Chunk deTitle = new Chunk("DE: ", HFBold);
                    Chunk deUsers = new Chunk(modelo.UsuariosDeActual.ToUpper(), General);

                    Paragraph de = new Paragraph();
                    de.Add(deTitle);
                    de.Add(deUsers);

                    tableIndicaciones.AddCell(cellineaInd);
                    PdfPCell cellDe = new PdfPCell(de);
                    cellDe.Colspan = 4;
                    cellDe.HorizontalAlignment = Element.ALIGN_LEFT;
                    cellDe.Border = Rectangle.NO_BORDER;
                    cellDe.PaddingLeft = 22f;
                    tableIndicaciones.AddCell(cellDe);

                    Chunk paraTitle = new Chunk("PARA: ", HFBold);
                    Chunk paraUsers = new Chunk(modelo.UsuariosParaActual.ToUpper(), General);

                    Paragraph para = new Paragraph();
                    para.Add(paraTitle);
                    para.Add(paraUsers);

                    tableIndicaciones.AddCell(cellineaInd);
                    PdfPCell cellPara = new PdfPCell(para);
                    cellPara.Colspan = 4;
                    cellPara.HorizontalAlignment = Element.ALIGN_LEFT;
                    cellPara.Border = Rectangle.NO_BORDER;
                    cellPara.PaddingLeft = 22f;
                    tableIndicaciones.AddCell(cellPara);

                    if (!string.IsNullOrEmpty(modelo.UsuariosParaCCActual))
                    {
                        Chunk ccTitle = new Chunk("CC: ", HFBold);
                        Chunk ccUsers = new Chunk(modelo.UsuariosParaCCActual.ToUpper(), General);

                        Paragraph cc = new Paragraph();
                        cc.Add(ccTitle);
                        cc.Add(ccUsers);

                        tableIndicaciones.AddCell(cellineaInd);
                        PdfPCell cellParaCC = new PdfPCell(cc);
                        cellParaCC.Colspan = 4;
                        cellParaCC.HorizontalAlignment = Element.ALIGN_LEFT;
                        cellParaCC.Border = Rectangle.NO_BORDER;
                        cellParaCC.PaddingLeft = 22f;
                        tableIndicaciones.AddCell(cellParaCC);
                    }

                    tableIndicaciones.AddCell(cellineaInd);
                    PdfPCell cellMdT = new PdfPCell(new Phrase("MENSAJE DE TEXTO: ", HFBold));
                    cellMdT.Colspan = 4;
                    cellMdT.HorizontalAlignment = Element.ALIGN_LEFT;
                    cellMdT.Border = Rectangle.NO_BORDER;
                    cellMdT.PaddingLeft = 22f;
                    tableIndicaciones.AddCell(cellMdT);
                    

                    tableIndicaciones.AddCell(cellineaInd);


                    string indicaciones = modelo.Indicaciones;
                    string tablaIndicaciones = "<table";
                    bool contieneTableIndicacions = indicaciones.Contains(tablaIndicaciones);

                    string saltoIndicaciones = "</p><p";
                    bool contieneSaltoIndicaciones = indicaciones.Contains(saltoIndicaciones);

                    if (contieneSaltoIndicaciones)
                    {
                        string saltoparrafoInd = "</p><br><p";
                        indicaciones = indicaciones.Replace(saltoIndicaciones, saltoparrafoInd);
                    }

                    if (contieneTableIndicacions)
                    {
                        string espaciotabla = "<p><br></p><table";
                        indicaciones = indicaciones.Replace(tablaIndicaciones, espaciotabla);
                    }

                    var tempIndicaciones = indicaciones.Replace("\"", "'");
                    var htmlIndicaciones = new HtmlDocument();
                    htmlIndicaciones.LoadHtml(@tempIndicaciones);

                    var newHtmlIndicaciones = htmlIndicaciones.DocumentNode.WriteTo();
                    var objIndicaciones = HtmlWorker.ParseToList(new StringReader(newHtmlIndicaciones), styles);

                    var cellCuerpoIndicaciones = new PdfPCell { };

                    foreach (IElement element in objIndicaciones)
                    {
                        cellCuerpoIndicaciones.AddElement(element);
                        cellCuerpoIndicaciones.Border = Rectangle.NO_BORDER;
                        cellCuerpoIndicaciones.PaddingLeft = 30f;
                        cellCuerpoIndicaciones.Colspan = 4;
                    }

                    tableIndicaciones.AddCell(cellCuerpoIndicaciones);

                    tableIndicaciones.AddCell(cellineaInd);                    

                    document.Add(tableIndicaciones);
                    document.NewPage();
                }
                PdfPTable table = new PdfPTable(4);
                table.TotalWidth = 550f;
                table.LockedWidth = true;
                float[] widths = new float[] { 14f, 8f, 30f, 30f };
                table.SetWidths(widths);

                PdfPCell cellvacia = new PdfPCell(new Phrase(""));
                cellvacia.Colspan = 1;
                cellvacia.HorizontalAlignment = Element.ALIGN_RIGHT;
                cellvacia.Border = Rectangle.NO_BORDER;

                PdfPCell celllinea = new PdfPCell(new Phrase(""));
                celllinea.Colspan = 4;
                celllinea.HorizontalAlignment = Element.ALIGN_RIGHT;
                celllinea.Border = Rectangle.NO_BORDER;

                // Logo
                var fileLogo = logo;
                var pngLogo = iTextSharp.text.Image.GetInstance(fileLogo);
                pngLogo.ScaleAbsolute(120f, 100f);

                //Direccion
                var direccionCompleta = "\n \n \nDirección: \n" + modelo.DeDireccion + "\n" + "\nTeléfono(s) \n" + modelo.DeTelefono + "\n" + "\nCorreo electrónico \n" + modelo.DeCorreo;

                PdfPCell celllogo = new PdfPCell(pngLogo);
                celllogo.Colspan = 4;
                celllogo.HorizontalAlignment = Element.ALIGN_RIGHT;
                celllogo.Border = Rectangle.NO_BORDER;
                table.AddCell(celllogo);

                PdfPCell cell1 = new PdfPCell(new Phrase(modelo.AreaSuperior, HFBold));
                cell1.Colspan = 4;
                cell1.HorizontalAlignment = Element.ALIGN_RIGHT;
                cell1.Border = Rectangle.NO_BORDER;
                table.AddCell(cell1);

                PdfPCell cell2 = new PdfPCell(new Phrase(modelo.Area, HF));
                cell2.Colspan = 4;
                cell2.HorizontalAlignment = Element.ALIGN_RIGHT;
                cell2.Border = Rectangle.NO_BORDER;
                table.AddCell(cell2);

                PdfPCell cell3 = new PdfPCell(new Phrase("Región " + modelo.Region, SmallBold));
                cell3.Colspan = 4;
                cell3.HorizontalAlignment = Element.ALIGN_RIGHT;
                cell3.Border = Rectangle.NO_BORDER;
                table.AddCell(cell3);

                PdfPCell cell4 = new PdfPCell(new Phrase(modelo.Asunto, Small));
                cell4.Colspan = 4;
                cell4.HorizontalAlignment = Element.ALIGN_RIGHT;
                cell4.Border = Rectangle.NO_BORDER;
                table.AddCell(cell4);

                if (!string.IsNullOrEmpty(modelo.NumeroInterno)) 
                {
                    PdfPCell cell41 = new PdfPCell(new Phrase(modelo.NumeroInterno, Small));
                    cell41.Colspan = 4;
                    cell41.HorizontalAlignment = Element.ALIGN_RIGHT;
                    cell41.Border = Rectangle.NO_BORDER;
                    table.AddCell(cell41);
                }

                PdfPCell cell5 = new PdfPCell(new Phrase(modelo.Fecha, Small));
                cell5.Colspan = 4;
                cell5.HorizontalAlignment = Element.ALIGN_RIGHT;
                cell5.Border = Rectangle.NO_BORDER;
                table.AddCell(cell5);

                PdfPCell folio = new PdfPCell(new Phrase("Folio: " + modelo.Folio, HFBold));
                folio.Colspan = 4;
                folio.HorizontalAlignment = Element.ALIGN_RIGHT;
                folio.Border = Rectangle.NO_BORDER;
                table.AddCell(folio);

                table.AddCell(celllinea);
                table.AddCell(celllinea);
                table.AddCell(celllinea);
                table.AddCell(celllinea);

                if (modelo.TipoUsuario == ConstTipoDestinatario.TipoDestinatarioN2)
                {
                    table.AddCell(celllinea);
                    table.AddCell(celllinea);

                    string ParaNombreEnviado = modelo.UsuariosParaCC.Replace(", ", "\n");

                    table.AddCell(cellvacia);
                    PdfPCell cellParaEnviado = new PdfPCell(new Phrase(ParaNombreEnviado.ToUpper(), General));
                    cellParaEnviado.Colspan = 4;
                    cellParaEnviado.HorizontalAlignment = Element.ALIGN_LEFT;
                    cellParaEnviado.Border = Rectangle.NO_BORDER;
                    cellParaEnviado.PaddingLeft = 10f;
                    table.AddCell(cellParaEnviado);
                    table.AddCell(cellvacia);
                    PdfPCell cell9 = new PdfPCell(new Phrase("UNIVERSIDAD VERACRUZANA", General));
                    cell9.Colspan = 4;
                    cell9.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell9.Border = Rectangle.NO_BORDER;
                    cell9.PaddingLeft = 10f;
                    table.AddCell(cell9);

                }
                else
                {
                    if ((modelo.ParaNombre != null))
                    {
                        table.AddCell(celllinea);
                        table.AddCell(celllinea);

                        table.AddCell(cellvacia);
                        PdfPCell cell6 = new PdfPCell(new Phrase(modelo.ParaNombre, General));
                        cell6.Colspan = 4;
                        cell6.HorizontalAlignment = Element.ALIGN_LEFT;
                        cell6.Border = Rectangle.NO_BORDER;
                        cell6.PaddingLeft = 10f;
                        table.AddCell(cell6);

                        table.AddCell(cellvacia);
                        PdfPCell cell7 = new PdfPCell(new Phrase(modelo.ParaPuesto, General));
                        cell7.Colspan = 4;
                        cell7.HorizontalAlignment = Element.ALIGN_LEFT;
                        cell7.Border = Rectangle.NO_BORDER;
                        cell7.PaddingLeft = 10f;
                        table.AddCell(cell7);

                        table.AddCell(cellvacia);
                        PdfPCell cell8 = new PdfPCell(new Phrase(modelo.ParaArea, General));
                        cell8.Colspan = 4;
                        cell8.HorizontalAlignment = Element.ALIGN_LEFT;
                        cell8.Border = Rectangle.NO_BORDER;
                        cell8.PaddingLeft = 10f;
                        table.AddCell(cell8);

                        table.AddCell(cellvacia);
                        PdfPCell cell9 = new PdfPCell(new Phrase("UNIVERSIDAD VERACRUZANA", General));
                        cell9.Colspan = 4;
                        cell9.HorizontalAlignment = Element.ALIGN_LEFT;
                        cell9.Border = Rectangle.NO_BORDER;
                        cell9.PaddingLeft = 10f;
                        table.AddCell(cell9);

                    }
                    else
                    {
                        table.AddCell(celllinea);
                        table.AddCell(celllinea);

                        string ParaNombreEnviado = modelo.ParaNombreEnviado.Replace(", ", "\n");

                        table.AddCell(cellvacia);
                        PdfPCell cellParaEnviado = new PdfPCell(new Phrase(ParaNombreEnviado.ToUpper(), General));
                        cellParaEnviado.Colspan = 4;
                        cellParaEnviado.HorizontalAlignment = Element.ALIGN_LEFT;
                        cellParaEnviado.Border = Rectangle.NO_BORDER;
                        cellParaEnviado.PaddingLeft = 10f;
                        table.AddCell(cellParaEnviado);
                    }
                }
                /*if (modelo.CCP != null)
                {
                    if (modelo.CCP.Any(str => str.Contains(usuario)))
                    {
                        table.AddCell(cellvacia);
                        PdfPCell celPSC = new PdfPCell(new Phrase("Para su conocimiento", GeneralItalic));
                        celPSC.Colspan = 4;
                        celPSC.HorizontalAlignment = Element.ALIGN_LEFT;
                        celPSC.Border = Rectangle.NO_BORDER;
                        celPSC.PaddingLeft = 10f;
                        table.AddCell(celPSC);
                    }
                }*/

                table.AddCell(celllinea);
                table.AddCell(celllinea);
                table.AddCell(celllinea);

                PdfPCell dir = new PdfPCell(new Phrase(direccionCompleta, ExtraSmall));
                dir.HorizontalAlignment = Element.ALIGN_RIGHT;
                dir.Border = Rectangle.NO_BORDER;
                table.AddCell(dir);

               
                //cuerpo             
                string cuerpo = modelo.Cuerpo;

                string tabla = "<table";
                bool contieneTable = cuerpo.Contains(tabla);

                string salto = "</p><p";
                bool contieneSalto = cuerpo.Contains(salto);

                string listas = "<li";
                bool contieneli = cuerpo.Contains(listas);

                if (contieneSalto)
                {
                    string saltoparrafo = "</p><br><p";
                    cuerpo = cuerpo.Replace(salto, saltoparrafo);
                }

                if (contieneTable)
                {
                    string espaciotabla = "<p><br></p><table";
                    cuerpo = cuerpo.Replace(tabla, espaciotabla);
                }

                var temp = cuerpo.Replace("\"", "'");
                var htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(@temp);

                string imagen = "<img";
                bool contieneImg = cuerpo.Contains(imagen);

                if (contieneImg)
                {
                    foreach (var node in htmlDoc.DocumentNode.SelectNodes("//img"))
                    {
                        var src = node.Attributes[@"src"].Value;

                        //if (src.StartsWith("/"))
                        //node.SetAttributeValue("src", baseUrl + src);
                        node.SetAttributeValue("src", src);
                        node.SetAttributeValue("width", "100");
                        node.SetAttributeValue("height", "100");

                    }
                }

                var newHtml = htmlDoc.DocumentNode.WriteTo();

                var objects = HtmlWorker.ParseToList(new StringReader(newHtml), styles);

                var cellCuerpo = new PdfPCell { };
                foreach (IElement element in objects)
                {
                    cellCuerpo.AddElement(element);
                    cellCuerpo.Border = Rectangle.NO_BORDER;
                    cellCuerpo.PaddingLeft = 17f;
                    cellCuerpo.Colspan = 4;
                }

                table.AddCell(cellCuerpo);

                table.AddCell(celllinea);
                table.AddCell(celllinea);
                table.AddCell(celllinea);
                table.AddCell(celllinea);
                table.AddCell(celllinea);

                table.AddCell(cellvacia);
                PdfPCell cell17 = new PdfPCell(new Phrase("A t e n t a m e n t e", SmallBold));
                cell17.Colspan = 3;
                cell17.HorizontalAlignment = Element.ALIGN_LEFT;
                cell17.Border = Rectangle.NO_BORDER;
                cell17.PaddingLeft = 10f;
                table.AddCell(cell17);

                table.AddCell(cellvacia);
                PdfPCell cell18 = new PdfPCell(new Phrase("Lis de Veracruz: Arte, Ciencia, Luz", Small));
                cell18.Colspan = 3;
                cell18.HorizontalAlignment = Element.ALIGN_LEFT;
                cell18.Border = Rectangle.NO_BORDER;
                cell18.PaddingLeft = 10f;
                table.AddCell(cell18);

                table.AddCell(celllinea);
                table.AddCell(celllinea);

                table.AddCell(cellvacia);
                PdfPCell cell19 = new PdfPCell(new Phrase(modelo.DeNombre, HF));
                cell19.Colspan = 3;
                cell19.HorizontalAlignment = Element.ALIGN_LEFT;
                cell19.Border = Rectangle.NO_BORDER;
                cell19.PaddingLeft = 10f;
                table.AddCell(cell19);

                table.AddCell(cellvacia);
                PdfPCell cell20 = new PdfPCell(new Phrase(modelo.DePuesto, Small));
                cell20.Colspan = 3;
                cell20.HorizontalAlignment = Element.ALIGN_LEFT;
                cell20.Border = Rectangle.NO_BORDER;
                cell20.PaddingLeft = 10f;
                table.AddCell(cell20);

                table.AddCell(cellvacia);
                PdfPCell cell21 = new PdfPCell(new Phrase(modelo.DeArea, HF));
                cell21.Colspan = 3;
                cell21.HorizontalAlignment = Element.ALIGN_LEFT;
                cell21.Border = Rectangle.NO_BORDER;
                cell21.PaddingLeft = 10f;
                table.AddCell(cell21);

                table.AddCell(cellvacia);
                PdfPCell cell22 = new PdfPCell(new Phrase("Universidad Veracruzana", HF));
                cell22.Colspan = 3;
                cell22.HorizontalAlignment = Element.ALIGN_LEFT;
                cell22.Border = Rectangle.NO_BORDER;
                cell22.PaddingLeft = 10f;
                table.AddCell(cell22);

                table.AddCell(celllinea);
                table.AddCell(celllinea);

                int index = 0;
                string copias = "";

                if (modelo.CCP != null)
                {
                    while (modelo.CCP.Count > index)
                    {
                        if (modelo.CCP[index] != "" && modelo.CCP[index] != " " && modelo.CCP[index] != null)
                        {
                            copias = "C.c.p. " + modelo.CCP[index];
                            table.AddCell(cellvacia);
                            PdfPCell cell23 = new PdfPCell(new Phrase(copias, ExtraSmall));
                            cell23.Colspan = 3;
                            cell23.HorizontalAlignment = Element.ALIGN_LEFT;
                            cell23.Border = Rectangle.NO_BORDER;
                            cell23.PaddingLeft = 10f;
                            table.AddCell(cell23);
                        }
                        index += 1;

                    }
                }

                table.AddCell(cellvacia);
                table.AddCell(celllinea);
                table.AddCell(celllinea);
                table.AddCell(celllinea);

                PdfPCell cell24 = new PdfPCell(new Phrase("R E C I B I D O - " + modelo.LeyendaRecibido + " hrs.", HFBold));
                cell24.Colspan = 4;
                cell24.HorizontalAlignment = Element.ALIGN_CENTER;
                cell24.Border = Rectangle.NO_BORDER;
                table.AddCell(cell24);

                table.AddCell(celllinea);
                table.AddCell(celllinea);
                table.AddCell(celllinea);
                table.AddCell(celllinea);

                //QR
                iTextSharp.text.Image gif = null;

                string base64string = modelo.CodigoQR;
                try
                {
                    //  Convert base64string to bytes array
                    Byte[] bytes = Convert.FromBase64String(base64string);
                    gif = iTextSharp.text.Image.GetInstance(bytes);
                    gif.ScaleAbsolute(100f, 100f);

                }
                catch (DocumentException dex)
                {
                    _logger.LogError("Error de documentación: " + dex.Message.ToString());
                }
                catch (IOException ioex)
                {
                    _logger.LogError("Error: " + ioex.Message.ToString());                
                }

                PdfPCell cellQR = new PdfPCell(gif);
                cellQR.Colspan = 4;
                cellQR.HorizontalAlignment = Element.ALIGN_RIGHT;
                cellQR.Border = Rectangle.NO_BORDER;
                table.AddCell(cellQR);

                table.SplitLate = false;
                table.SplitRows = true;

                document.Add(table);
                
                //writer.PageEmpty = false;

                document.Close();
                return stream.ToArray();
            }
        }   
    }   
}

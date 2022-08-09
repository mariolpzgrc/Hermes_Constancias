using Hermes2018.Services.Interfaces;
using Hermes2018.ViewModels;
using iTextSharp.text;
using iTextSharp.text.html;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Hermes2018.Services
{
    public class DescargarConstanciaPDFService : IDescargarConstanciaPDFService
    {
        private readonly IHostingEnvironment _env;
        private readonly ILogger<DescargarConstanciaPDFService> _logger;
        DateTime fechaExpedicion = DateTime.Today;

        public DescargarConstanciaPDFService(IHostingEnvironment env, ILogger<DescargarConstanciaPDFService> logger)
        {
            _env = env;
            _logger = logger;
        }

        public byte[] DescargarConstanciaNoDocente(ConstanciaViewModel modelo, int idTipoConstancia, string nombreConstancia, int idTipoPersonal)
        {
            //Definicion de las fuentes
            Font GeneralFooter = FontFactory.GetFont("Arial", 8, Font.NORMAL, BaseColor.Black);
            Font GeneralFoto = FontFactory.GetFont("Arial", 10, Font.ITALIC, BaseColor.Black);
            Font General = FontFactory.GetFont("Arial", 11, Font.NORMAL, BaseColor.Black);
            Font GeneralBold = FontFactory.GetFont("Arial", 11, Font.BOLD, BaseColor.Black);
            Font General12Bold = FontFactory.GetFont("Arial", 13, Font.BOLD, BaseColor.Black);
            Font General13Bold = FontFactory.GetFont("Arial", 13, Font.BOLD, BaseColor.Black);
            Font General14Bold = FontFactory.GetFont("Arial", 14, Font.BOLD, BaseColor.Black);

            // escudo
            var escudopath = Path.Combine(_env.WebRootPath, "images", "Escudo_UV.png");
            Image escudo = Image.GetInstance(escudopath);
            escudo.ScaleAbsolute(85f, 85f);

            var streamConstancia = new MemoryStream();
            using (streamConstancia)
            {
                var constancia = new Document(PageSize.Letter);
                var writer = PdfWriter.GetInstance(constancia, streamConstancia);
                constancia.AddAuthor("Universidad Veracruzana");
                try
                {
                    constancia.Open();

                    //Estructura del header
                    PdfPTable header = new PdfPTable(7);
                    header.TotalWidth = 550;
                    header.LockedWidth = true;
                    float[] widths = new float[] { 35f, 60f, 120f, 120f, 120f, 60f, 35f };
                    header.SetWidths(widths);

                    //Estructura de la correspondencia
                    PdfPTable correspondenciaConstancia = new PdfPTable(3);
                    correspondenciaConstancia.TotalWidth = 550f;
                    correspondenciaConstancia.LockedWidth = true;
                    float[] widthscc = new float[] { 45f, 460, 45f };
                    correspondenciaConstancia.SetWidths(widthscc);

                    //Estructura de la correspondencia
                    PdfPTable cuerpoConstancia = new PdfPTable(1);
                    cuerpoConstancia.TotalWidth = 550f;
                    cuerpoConstancia.LockedWidth = true;
                    float[] widthsc = new float[] { 550f };
                    cuerpoConstancia.SetWidths(widthsc);

                    //Estructura del cuerpo de la constancia
                    PdfPTable cuerpoSueldo = new PdfPTable(1);
                    cuerpoSueldo.TotalWidth = 550f;
                    cuerpoSueldo.LockedWidth = true;
                    float[] widthps = new float[] { 550f };
                    cuerpoSueldo.SetWidths(widthps);

                    //tabla de la constancia para docentes
                    PdfPTable tablaConstancia = new PdfPTable(9);
                    tablaConstancia.TotalWidth = 550f;
                    tablaConstancia.LockedWidth = true;
                    float[] widthsTablaconstancia = new float[] { 45f, 71.66f, 71.66f, 71.66f, 71.66f, 30, 71.66f, 71.66f, 45f };
                    tablaConstancia.SetWidths(widthsTablaconstancia);

                    //Estructura del cuerpo de la constancia
                    PdfPTable constanciafirma = new PdfPTable(7);
                    constanciafirma.TotalWidth = 550f;
                    constanciafirma.LockedWidth = true;
                    float[] widthf = new float[] { 45f, 60f, 113.33f, 113.33f, 113.33f, 60f, 45f };
                    constanciafirma.SetWidths(widthf);

                    //Estructura del footer
                    PdfPTable footer = new PdfPTable(7);
                    footer.TotalWidth = 550f;
                    footer.LockedWidth = true;
                    float[] widthsfooter = new float[] { 25f, 65f, 130f, 130f, 130f, 65f, 25f };
                    footer.SetWidths(widthsfooter);

                    //Header
                    var secretaria = "SECRETARÍA DE ADMINISTRACIÓN Y FINANZAS";
                    var direccion = "DIRECCIÓN GENERAL DE RECURSOS HUMANOS";
                    var direccionPersonal = "DIRECCIÓN DE PERSONAL";

                    //Variable del cuerpo
                    Chunk nombreTrabajador = new Chunk(modelo.sNombre, GeneralBold);
                    nombreTrabajador.SetUnderline(0.5f, -1.5f);
                    Chunk numeropersonal = new Chunk(modelo.sNumPer, GeneralBold);
                    numeropersonal.SetUnderline(0.5f, -1.5f);
                    Chunk fechaingreso = new Chunk(modelo.dFIngreso, General);
                    fechaingreso.SetUnderline(0.5f, -1.5f);
                    Chunk entidadDepedencia = new Chunk(modelo.sDescDep, General);
                    entidadDepedencia.SetUnderline(0.5f, -1.5f);
                    Chunk region = new Chunk(modelo.sDesRegion, General);
                    region.SetUnderline(0.5f, -1.5f);
                    Chunk tipoPersonal = new Chunk(modelo.sDesTPE, General);
                    tipoPersonal.SetUnderline(0.5f, -1.5f);
                    Chunk tipoContratacion = new Chunk(modelo.sDesCont, General);
                    tipoContratacion.SetUnderline(0.5f, -1.5f);
                    Chunk categoriaSueldo = new Chunk(modelo.sDescCat, General);
                    categoriaSueldo.SetUnderline(0.5f, -1.5f);
                    Chunk puesto = new Chunk(modelo.sDescPuesto, General);
                    puesto.SetUnderline(0.5f, -1.5f);
                    Chunk cantidadSueldo = new Chunk(modelo.sSuelPrest, General);
                    cantidadSueldo.SetUnderline(0.5f, -1.5f);
                    //Dependiente economico
                    Chunk relacionDepedientente = new Chunk(modelo.sParentesco, General);
                    relacionDepedientente.SetUnderline(0.5f, -1.5f);
                    Chunk nombreDepedienteEconomico = new Chunk(modelo.sNomDepen, General);
                    nombreDepedienteEconomico.SetUnderline(0.5f, -1.5f);

                    //Cuerpo formado
                    //Estructura del cuerpo 
                    Chunk cuerpo1raparte = new Chunk("De acuerdo a los documentos que se encuentran en el expediente personal, el (la) C. ", General);
                    Chunk cuerpo2daparte = new Chunk(", con número de personal ", General);
                    Chunk cuerpo3raparte = new Chunk(", ingresó el ", General);
                    Chunk cuerpo3rapartefoto = new Chunk(" cuya fotografía aparece al calce, ingresó el ", General);
                    Chunk cuerpo4taparte = new Chunk(", y actualmente presta sus servicios en esta Casa de Estudios como:", General);
                    Chunk cuerpo4taparteSM = new Chunk(", y actualmente recibe el beneficio de la prestación denominada “Servicio Médico” que brinda la institución, y presta sus servicios en esta Casa de Estudios como: ", General);

                    //Integracion del cuerpo
                    Paragraph cuerpo = new Paragraph();
                    cuerpo.Add(cuerpo1raparte);
                    cuerpo.Add(nombreTrabajador);
                    cuerpo.Add(cuerpo2daparte);
                    cuerpo.Add(numeropersonal);
                    if (idTipoConstancia != 9)
                    {
                        cuerpo.Add(cuerpo3raparte);
                    }
                    else
                    {
                        cuerpo.Add(cuerpo3rapartefoto);
                    }
                    cuerpo.Add(fechaingreso);
                    if (idTipoConstancia != 1)
                    {
                        cuerpo.Add(cuerpo4taparte);
                    }
                    else
                    {
                        cuerpo.Add(cuerpo4taparteSM);
                    }

                    //Cuerpo de sueldos 
                    Chunk cuerposueldo = new Chunk("Percibe en forma mensual de sueldo la cantidad de: ", General);

                    //PRODEP 
                    List list = new List(List.UNORDERED, 10f);
                    list.SetListSymbol("\u2022");
                    list.IndentationLeft = 30f;
                    list.Add(new ListItem(new Chunk("Es Tiempo Completo con esta categoría el: ", General)));

                    //Integracion del segundo cuerpo
                    Paragraph parrafoSueldo = new Paragraph();
                    parrafoSueldo.Add(cuerposueldo);
                    parrafoSueldo.Add(cantidadSueldo);

                    //Estructura del cuerpo del SM depediente economico
                    Chunk cuerpoSMDE1a = new Chunk("Solicita la presente para hacer constar que su ", General);
                    Chunk cuerpoSMDE2a = new Chunk(", quien(es) está(n) registrado(as) como su(s) dependiente(s) económico(s), recibe(n) el beneficio de la prestación denominada “Servicio Médico” que brinda la institución. ", General);

                    //Estrucutra del cuerpo de la visa depediente economico
                    Chunk cuerpovisade1 = new Chunk("Solicita la presente para trámite de VISA de su ", General);
                    Chunk cuerpovisade2 = new Chunk(", cuya fotografía aparece al calce, quien está registrado como su dependiente económico.", General);

                    //Integracion del parrafo del dependiente economico
                    Paragraph cuerpodepediente = new Paragraph();
                    if (idTipoConstancia == 2)
                    {
                        cuerpodepediente.Add(cuerpoSMDE1a);
                        cuerpodepediente.Add(relacionDepedientente);
                        cuerpodepediente.Add(nombreDepedienteEconomico);
                        cuerpodepediente.Add(cuerpoSMDE2a);
                    }
                    else if (idTipoConstancia == 10)
                    {
                        cuerpodepediente.Add(cuerpovisade1);
                        cuerpodepediente.Add(relacionDepedientente);
                        cuerpodepediente.Add(nombreDepedienteEconomico);
                        cuerpodepediente.Add(cuerpovisade2);
                    }

                    //Estructura de la despedida y firma
                    Chunk cuerpo21raparte = new Chunk("A petición de la parte interesada y para los efectos legales que a la misma convenga, se expide la presente ", General);
                    Chunk cuerpo22daparte = new Chunk("CONSTANCIA ", GeneralBold);
                    Chunk cuerpo23raparte = new Chunk("en la ciudad de Xalapa, Eqz., Veracruz al " + fechaExpedicion.ToString("D", CultureInfo.CreateSpecificCulture("es-MX")) + ".", General);

                    //Composicion del 2dfo parrafo
                    Paragraph parrafoFinal = new Paragraph();
                    parrafoFinal.Add(cuerpo21raparte);
                    parrafoFinal.Add(cuerpo22daparte);
                    parrafoFinal.Add(cuerpo23raparte);

                    //Header table
                    BaseColor headtableColor = WebColors.GetRgbColor("#D9D9D9");

                    //Header
                    //Header -padding left
                    PdfPCell cellpaddingleftlogo = new PdfPCell();
                    cellpaddingleftlogo.Rowspan = 5;
                    cellpaddingleftlogo.Colspan = 1;
                    cellpaddingleftlogo.HorizontalAlignment = Element.ALIGN_RIGHT;
                    cellpaddingleftlogo.Border = Rectangle.NO_BORDER;
                    header.AddCell(cellpaddingleftlogo);

                    //Header - escudo
                    PdfPCell cellescudo = new PdfPCell(escudo);
                    cellescudo.Rowspan = 5;
                    cellescudo.Colspan = 1;
                    cellescudo.HorizontalAlignment = Element.ALIGN_RIGHT;
                    cellescudo.Border = Rectangle.NO_BORDER;
                    header.AddCell(cellescudo);

                    PdfPCell cellSecretaria = new PdfPCell(new Phrase(secretaria, General14Bold));
                    cellSecretaria.Colspan = 4;
                    cellSecretaria.HorizontalAlignment = Element.ALIGN_CENTER;
                    cellSecretaria.Border = Rectangle.NO_BORDER;
                    header.AddCell(cellSecretaria);

                    PdfPCell cellpaddingrightlogo = new PdfPCell(new Phrase(""));
                    cellpaddingrightlogo.Colspan = 1;
                    cellpaddingrightlogo.HorizontalAlignment = Element.ALIGN_RIGHT;
                    cellpaddingrightlogo.Border = Rectangle.NO_BORDER;
                    header.AddCell(cellpaddingrightlogo);

                    PdfPCell celldireccion = new PdfPCell(new Phrase(direccion, General12Bold));
                    celldireccion.Colspan = 4;
                    celldireccion.HorizontalAlignment = Element.ALIGN_CENTER;
                    celldireccion.Border = Rectangle.NO_BORDER;
                    header.AddCell(celldireccion);

                    header.AddCell(cellpaddingrightlogo);

                    PdfPCell celldireccionPersonal = new PdfPCell(new Phrase(direccionPersonal, General12Bold));
                    celldireccionPersonal.Colspan = 4;
                    celldireccionPersonal.HorizontalAlignment = Element.ALIGN_CENTER;
                    celldireccionPersonal.Border = Rectangle.NO_BORDER;
                    header.AddCell(celldireccionPersonal);

                    header.AddCell(cellpaddingrightlogo);

                    PdfPCell cellvacia2 = new PdfPCell(new Phrase(""));
                    cellvacia2.Colspan = 4;
                    cellvacia2.HorizontalAlignment = Element.ALIGN_RIGHT;
                    cellvacia2.Border = Rectangle.NO_BORDER;
                    header.AddCell(cellvacia2);

                    header.AddCell(cellpaddingrightlogo);

                    PdfPCell celltipoConstancia = new PdfPCell(new Phrase(nombreConstancia, General13Bold));
                    celltipoConstancia.Colspan = 4;
                    celltipoConstancia.HorizontalAlignment = Element.ALIGN_CENTER;
                    celltipoConstancia.Border = Rectangle.NO_BORDER;
                    header.AddCell(celltipoConstancia);

                    header.AddCell(cellpaddingrightlogo);

                    //Correspondencia y Constancia
                    PdfPCell cellespaciado = new PdfPCell(new Phrase(""));
                    cellespaciado.Colspan = 3;
                    cellespaciado.FixedHeight = 20f;
                    cellespaciado.HorizontalAlignment = Element.ALIGN_RIGHT;
                    cellespaciado.Border = Rectangle.NO_BORDER;
                    correspondenciaConstancia.AddCell(cellespaciado);

                    PdfPCell cellcorrespondencia = new PdfPCell(new Phrase("A QUIEN CORRESPONDA:", GeneralBold));
                    cellcorrespondencia.Colspan = 3;
                    cellcorrespondencia.PaddingLeft = 45f;
                    cellcorrespondencia.PaddingRight = 45f;
                    cellcorrespondencia.HorizontalAlignment = Element.ALIGN_LEFT;
                    cellcorrespondencia.Border = Rectangle.NO_BORDER;
                    correspondenciaConstancia.AddCell(cellcorrespondencia);

                    correspondenciaConstancia.AddCell(cellespaciado);

                    PdfPCell cellsuscribe = new PdfPCell(new Phrase("El(La) que suscribe Director(a) de Personal de la Universidad Veracruzana en esta ciudad", General));
                    cellsuscribe.Colspan = 3;
                    cellsuscribe.PaddingLeft = 45f;
                    cellsuscribe.PaddingRight = 45f;
                    cellsuscribe.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                    cellsuscribe.Border = Rectangle.NO_BORDER;
                    correspondenciaConstancia.AddCell(cellsuscribe);

                    correspondenciaConstancia.AddCell(cellespaciado);

                    PdfPCell cell1 = new PdfPCell(new Phrase("HACE CONSTAR QUE", GeneralBold));
                    cell1.Colspan = 3;
                    cell1.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell1.Border = Rectangle.NO_BORDER;
                    correspondenciaConstancia.AddCell(cell1);

                    //Cuerpo primera parte
                    PdfPCell cellcuerpo = new PdfPCell(cuerpo);
                    cellcuerpo.Colspan = 1;
                    cellcuerpo.PaddingLeft = 45f;
                    cellcuerpo.PaddingRight = 45f;
                    cellcuerpo.SetLeading(15f, 1f);
                    cellcuerpo.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                    cellcuerpo.Border = Rectangle.NO_BORDER;
                    cuerpoConstancia.AddCell(cellcuerpo);

                    PdfPCell cellcuerpoespaciado = new PdfPCell();
                    cellcuerpoespaciado.Colspan = 1;
                    cellcuerpoespaciado.PaddingLeft = 45f;
                    cellcuerpoespaciado.PaddingRight = 45f;
                    cellcuerpoespaciado.SetLeading(15f, 1f);
                    cellcuerpoespaciado.FixedHeight = 15f;
                    cellcuerpoespaciado.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                    cellcuerpoespaciado.Border = Rectangle.NO_BORDER;
                    cuerpoConstancia.AddCell(cellcuerpoespaciado);

                    //Tabla de la constancia
                    PdfPCell cellpaddingleft = new PdfPCell(new Phrase(""));
                    cellpaddingleft.Colspan = 1;
                    cellpaddingleft.HorizontalAlignment = Element.ALIGN_CENTER;
                    cellpaddingleft.Border = Rectangle.NO_BORDER;
                    tablaConstancia.AddCell(cellpaddingleft);


                    PdfPCell cellheaderPeriodo = new PdfPCell(new Phrase("Periodo", GeneralFooter));
                    cellheaderPeriodo.Colspan = 1;
                    cellheaderPeriodo.HorizontalAlignment = Element.ALIGN_CENTER;
                    cellheaderPeriodo.BackgroundColor = headtableColor;
                    cellheaderPeriodo.Border = Rectangle.BOX;
                    tablaConstancia.AddCell(cellheaderPeriodo);

                    PdfPCell cellheaderPuesto = new PdfPCell(new Phrase("Puesto", GeneralFooter));
                    cellheaderPuesto.Colspan = 1;
                    cellheaderPuesto.HorizontalAlignment = Element.ALIGN_CENTER;
                    cellheaderPuesto.BackgroundColor = headtableColor;
                    cellheaderPuesto.Border = Rectangle.BOX;
                    tablaConstancia.AddCell(cellheaderPuesto);

                    PdfPCell cellheaderCategoria = new PdfPCell(new Phrase("Categoría", GeneralFooter));
                    cellheaderCategoria.Colspan = 1;
                    cellheaderCategoria.HorizontalAlignment = Element.ALIGN_CENTER;
                    cellheaderCategoria.BackgroundColor = headtableColor;
                    cellheaderCategoria.Border = Rectangle.BOX;
                    tablaConstancia.AddCell(cellheaderCategoria);

                    PdfPCell cellheaderTipoContratacion = new PdfPCell(new Phrase("Tipo Contratación", GeneralFooter));
                    cellheaderTipoContratacion.Colspan = 1;
                    cellheaderTipoContratacion.HorizontalAlignment = Element.ALIGN_CENTER;
                    cellheaderTipoContratacion.BackgroundColor = headtableColor;
                    cellheaderTipoContratacion.Border = Rectangle.BOX;
                    tablaConstancia.AddCell(cellheaderTipoContratacion);

                    PdfPCell cellheaderHoras = new PdfPCell(new Phrase("Horas", GeneralFooter));
                    cellheaderHoras.Colspan = 1;
                    cellheaderHoras.HorizontalAlignment = Element.ALIGN_CENTER;
                    cellheaderHoras.BackgroundColor = headtableColor;
                    cellheaderHoras.Border = Rectangle.BOX;
                    tablaConstancia.AddCell(cellheaderHoras);

                    PdfPCell cellheaderDepedencia = new PdfPCell(new Phrase("Dependencia Adscripción", GeneralFooter));
                    cellheaderDepedencia.Colspan = 1;
                    cellheaderDepedencia.HorizontalAlignment = Element.ALIGN_CENTER;
                    cellheaderDepedencia.BackgroundColor = headtableColor;
                    cellheaderDepedencia.Border = Rectangle.BOX;
                    tablaConstancia.AddCell(cellheaderDepedencia);

                    PdfPCell cellheaderUbicacion = new PdfPCell(new Phrase("Ubicación", GeneralFooter));
                    cellheaderUbicacion.Colspan = 1;
                    cellheaderUbicacion.HorizontalAlignment = Element.ALIGN_CENTER;
                    cellheaderUbicacion.BackgroundColor = headtableColor;
                    cellheaderUbicacion.Border = Rectangle.BOX;
                    tablaConstancia.AddCell(cellheaderUbicacion);

                    PdfPCell cellpaddingright = new PdfPCell(new Phrase(""));
                    cellpaddingright.Colspan = 1;
                    cellpaddingright.HorizontalAlignment = Element.ALIGN_CENTER;
                    cellpaddingright.Border = Rectangle.NO_BORDER;
                    tablaConstancia.AddCell(cellpaddingright);

                    //Cuerpo de la tabla
                    PdfPCell cellbodypaddingleft = new PdfPCell(new Phrase(""));
                    cellbodypaddingleft.Colspan = 1;
                    cellbodypaddingleft.HorizontalAlignment = Element.ALIGN_CENTER;
                    cellbodypaddingleft.Border = Rectangle.NO_BORDER;
                    cellbodypaddingleft.FixedHeight = 80f;
                    tablaConstancia.AddCell(cellbodypaddingleft);

                    PdfPCell cellbodyPeriodo = new PdfPCell(new Phrase("", GeneralFooter));
                    cellbodyPeriodo.Colspan = 1;
                    cellbodyPeriodo.HorizontalAlignment = Element.ALIGN_CENTER;
                    cellbodyPeriodo.Border = Rectangle.BOX;
                    cellbodyPeriodo.FixedHeight = 80f;
                    tablaConstancia.AddCell(cellbodyPeriodo);

                    PdfPCell cellbodyPuesto = new PdfPCell(new Phrase("", GeneralFooter));
                    cellbodyPuesto.Colspan = 1;
                    cellbodyPuesto.HorizontalAlignment = Element.ALIGN_CENTER;
                    cellbodyPuesto.Border = Rectangle.BOX;
                    cellbodyPuesto.FixedHeight = 80f;
                    tablaConstancia.AddCell(cellbodyPuesto);

                    PdfPCell cellbodyCategoria = new PdfPCell(new Phrase("", GeneralFooter));
                    cellbodyCategoria.Colspan = 1;
                    cellbodyCategoria.HorizontalAlignment = Element.ALIGN_CENTER;
                    cellbodyCategoria.Border = Rectangle.BOX;
                    cellbodyCategoria.FixedHeight = 80f;
                    tablaConstancia.AddCell(cellbodyCategoria);

                    PdfPCell cellbodyTipoContratacion = new PdfPCell(new Phrase("", GeneralFooter));
                    cellbodyTipoContratacion.Colspan = 1;
                    cellbodyTipoContratacion.HorizontalAlignment = Element.ALIGN_CENTER;
                    cellbodyTipoContratacion.Border = Rectangle.BOX;
                    cellbodyTipoContratacion.FixedHeight = 80f;
                    tablaConstancia.AddCell(cellbodyTipoContratacion);

                    PdfPCell cellbodyHoras = new PdfPCell(new Phrase("", GeneralFooter));
                    cellbodyHoras.Colspan = 1;
                    cellbodyHoras.HorizontalAlignment = Element.ALIGN_CENTER;
                    cellbodyHoras.Border = Rectangle.BOX;
                    cellbodyHoras.FixedHeight = 80f;
                    tablaConstancia.AddCell(cellbodyHoras);

                    PdfPCell cellbodyDepedencia = new PdfPCell(new Phrase(" ", GeneralFooter));
                    cellbodyDepedencia.Colspan = 1;
                    cellbodyDepedencia.HorizontalAlignment = Element.ALIGN_CENTER;
                    cellbodyDepedencia.Border = Rectangle.BOX;
                    cellbodyDepedencia.FixedHeight = 80f;
                    tablaConstancia.AddCell(cellbodyDepedencia);

                    PdfPCell cellbodyUbicacion = new PdfPCell(new Phrase("", GeneralFooter));
                    cellbodyUbicacion.Colspan = 1;
                    cellbodyUbicacion.HorizontalAlignment = Element.ALIGN_CENTER;
                    cellbodyUbicacion.Border = Rectangle.BOX;
                    cellbodyUbicacion.FixedHeight = 80f;
                    tablaConstancia.AddCell(cellbodyUbicacion);

                    PdfPCell cellbodypaddingright = new PdfPCell(new Phrase(""));
                    cellbodypaddingright.Colspan = 1;
                    cellbodypaddingright.HorizontalAlignment = Element.ALIGN_CENTER;
                    cellbodypaddingright.Border = Rectangle.NO_BORDER;
                    cellbodypaddingright.FixedHeight = 80f;
                    tablaConstancia.AddCell(cellbodypaddingright);

                    //Parrafo del sueldo
                    PdfPCell cellsueldoespacio = new PdfPCell();
                    cellsueldoespacio.Colspan = 1;
                    cellsueldoespacio.PaddingLeft = 45f;
                    cellsueldoespacio.PaddingRight = 45f;
                    cellsueldoespacio.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                    cellsueldoespacio.Border = Rectangle.NO_BORDER;
                    cuerpoSueldo.AddCell(cellsueldoespacio);

                    if (idTipoConstancia == 11)
                    {
                        PdfPCell cellsueldo = new PdfPCell();
                        cellsueldo.Colspan = 1;
                        cellsueldo.SetLeading(15f, 1f);
                        cellsueldo.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                        cellsueldo.PaddingLeft = 25f;
                        cellsueldo.PaddingRight = 25f;
                        cellsueldo.Border = Rectangle.NO_BORDER;
                        cellsueldo.AddElement(list);
                        cuerpoSueldo.AddCell(cellsueldo);
                    }
                    else
                    {
                        PdfPCell cellsueldo = new PdfPCell(parrafoSueldo);
                        cellsueldo.Colspan = 1;
                        cellsueldo.PaddingLeft = 45f;
                        cellsueldo.PaddingRight = 45f;
                        cellsueldo.SetLeading(15f, 1f);
                        cellsueldo.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                        cellsueldo.Border = Rectangle.NO_BORDER;
                        cuerpoSueldo.AddCell(cellsueldo);
                    }

                    cuerpoSueldo.AddCell(cellsueldoespacio);

                    PdfPCell celldependiente = new PdfPCell(cuerpodepediente);
                    celldependiente.Colspan = 1;
                    celldependiente.PaddingLeft = 45f;
                    celldependiente.PaddingRight = 45f;
                    celldependiente.SetLeading(15f, 1f);
                    celldependiente.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                    celldependiente.Border = Rectangle.NO_BORDER;
                    cuerpoSueldo.AddCell(celldependiente);

                    //Despedida y Firma
                    PdfPCell cellcuerpo2 = new PdfPCell(parrafoFinal);
                    cellcuerpo2.Colspan = 7;
                    cellcuerpo2.SetLeading(15f, 1f);
                    cellcuerpo2.PaddingLeft = 45f;
                    cellcuerpo2.PaddingRight = 45f;
                    cellcuerpo2.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                    cellcuerpo2.Border = Rectangle.NO_BORDER;
                    constanciafirma.AddCell(cellcuerpo2);

                    PdfPCell cellcuerpo2esp = new PdfPCell();
                    cellcuerpo2esp.Colspan = 7;
                    cellcuerpo2esp.SetLeading(15f, 1f);
                    cellcuerpo2esp.PaddingLeft = 45f;
                    cellcuerpo2esp.PaddingRight = 45f;
                    cellcuerpo2esp.FixedHeight = 20f;
                    cellcuerpo2esp.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                    cellcuerpo2esp.Border = Rectangle.NO_BORDER;
                    constanciafirma.AddCell(cellcuerpo2esp);

                    if (idTipoConstancia == 9 || idTipoConstancia == 10)
                    {
                        PdfPCell celldirectorpaddingleftfoto = new PdfPCell(new Phrase(""));
                        celldirectorpaddingleftfoto.Colspan = 1;
                        celldirectorpaddingleftfoto.FixedHeight = 75f;
                        celldirectorpaddingleftfoto.Border = Rectangle.NO_BORDER;
                        constanciafirma.AddCell(celldirectorpaddingleftfoto);

                        PdfPCell cellfoto = new PdfPCell(new Phrase("Fotografía", GeneralFoto));
                        cellfoto.Colspan = 1;
                        cellfoto.FixedHeight = 75f;
                        cellfoto.HorizontalAlignment = Element.ALIGN_CENTER;
                        cellfoto.VerticalAlignment = Element.ALIGN_CENTER;
                        cellfoto.Border = Rectangle.BOX;
                        constanciafirma.AddCell(cellfoto);
                    }
                    else
                    {
                        PdfPCell celldirectorpaddingleft = new PdfPCell(new Phrase(""));
                        celldirectorpaddingleft.Colspan = 2;
                        celldirectorpaddingleft.FixedHeight = 75f;
                        celldirectorpaddingleft.Border = Rectangle.NO_BORDER;
                        constanciafirma.AddCell(celldirectorpaddingleft);
                    }

                    PdfPCell celldirector = new PdfPCell(new Phrase("NOMBRE DEL DIRECTOR(A) DE PERSONAL)", GeneralBold));
                    celldirector.Colspan = 3;
                    celldirector.FixedHeight = 75f;
                    celldirector.VerticalAlignment = Element.ALIGN_BOTTOM;
                    celldirector.HorizontalAlignment = Element.ALIGN_CENTER;
                    celldirector.Border = Rectangle.NO_BORDER;
                    constanciafirma.AddCell(celldirector);

                    PdfPCell celldirectorpaddingright = new PdfPCell(new Phrase(""));
                    celldirectorpaddingright.Colspan = 2;
                    celldirectorpaddingright.FixedHeight = 75f;
                    celldirectorpaddingright.HorizontalAlignment = Element.ALIGN_CENTER;
                    celldirectorpaddingright.Border = Rectangle.NO_BORDER;
                    constanciafirma.AddCell(celldirectorpaddingright);

                    PdfPCell cellvacia = new PdfPCell(new Phrase(""));
                    cellvacia.Colspan = 7;
                    if (idTipoConstancia == 9)
                    {
                        cellvacia.FixedHeight = 60f;
                    }
                    else if (idTipoConstancia == 10)
                    {
                        cellvacia.FixedHeight = 20f;
                    }
                    else if (idTipoConstancia == 2)
                    {
                        cellvacia.FixedHeight = 5f;
                    }
                    else
                    {
                        cellvacia.FixedHeight = 60f;
                    }
                    cellvacia.HorizontalAlignment = Element.ALIGN_CENTER;
                    cellvacia.Border = Rectangle.NO_BORDER;
                    constanciafirma.AddCell(cellvacia);

                    //Footer
                    PdfPCell cellflup = new PdfPCell(new Phrase(""));
                    cellflup.Colspan = 7;
                    cellflup.HorizontalAlignment = Element.ALIGN_CENTER;
                    cellflup.Border = Rectangle.BOTTOM_BORDER;
                    cellflup.BorderWidthBottom = 0.5f;
                    footer.AddCell(cellflup);

                    PdfPCell cellfldown = new PdfPCell(new Phrase(""));
                    cellfldown.Colspan = 7;
                    cellfldown.HorizontalAlignment = Element.ALIGN_CENTER;
                    cellfldown.Border = Rectangle.BOTTOM_BORDER;
                    cellfldown.BorderWidthBottom = 0.5f;
                    footer.AddCell(cellfldown);

                    PdfPCell cellvaciafooter1 = new PdfPCell(new Phrase(""));
                    cellvaciafooter1.Colspan = 2;
                    cellvaciafooter1.HorizontalAlignment = Element.ALIGN_CENTER;
                    cellvaciafooter1.Border = Rectangle.NO_BORDER;
                    footer.AddCell(cellvaciafooter1);

                    PdfPCell cellDir = new PdfPCell(new Phrase("Edificio \"A\" de Rectoría, Planta Baja ", GeneralFooter));
                    cellDir.Colspan = 1;
                    cellDir.HorizontalAlignment = Element.ALIGN_CENTER;
                    cellDir.Border = Rectangle.NO_BORDER;
                    footer.AddCell(cellDir);

                    PdfPCell cellvaciafooter2 = new PdfPCell(new Phrase(""));
                    cellvaciafooter2.Colspan = 1;
                    cellvaciafooter2.HorizontalAlignment = Element.ALIGN_CENTER;
                    cellvaciafooter2.Border = Rectangle.NO_BORDER;
                    footer.AddCell(cellvaciafooter2);

                    PdfPCell cellTel = new PdfPCell(new Phrase("Teléfono 842-17-00 Ext. 11705", GeneralFooter));
                    cellTel.Colspan = 1;
                    cellTel.HorizontalAlignment = Element.ALIGN_CENTER;
                    cellTel.Border = Rectangle.NO_BORDER;
                    footer.AddCell(cellTel);

                    footer.AddCell(cellvaciafooter1);

                    constancia.Add(header);
                    constancia.Add(correspondenciaConstancia);
                    constancia.Add(cuerpoConstancia);
                    constancia.Add(tablaConstancia);
                    constancia.Add(cuerpoSueldo);
                    constancia.Add(constanciafirma);
                    constancia.Add(footer);
                }
                catch(Exception e)
                {
                    _logger.Log(LogLevel.Error, "Error catch: " + e.Message);
                }
                finally
                {
                    constancia.Close();
                }
            }
            return streamConstancia.ToArray();
        }

        public byte[] DescargarConstanciaDocente(ConstanciaViewModel modelo, int idTipoConstancia, string nombreConstancia, int idTipoPersonal)
        {
            //Definicion de las fuentes
            Font GeneralFooter = FontFactory.GetFont("Arial", 8, Font.NORMAL, BaseColor.Black);
            Font GeneralFoto = FontFactory.GetFont("Arial", 10, Font.ITALIC, BaseColor.Black);
            Font General = FontFactory.GetFont("Arial", 11, Font.NORMAL, BaseColor.Black);
            Font GeneralBold = FontFactory.GetFont("Arial", 11, Font.BOLD, BaseColor.Black);
            Font General12Bold = FontFactory.GetFont("Arial", 13, Font.BOLD, BaseColor.Black);
            Font General13Bold = FontFactory.GetFont("Arial", 13, Font.BOLD, BaseColor.Black);
            Font General14Bold = FontFactory.GetFont("Arial", 14, Font.BOLD, BaseColor.Black);

            // escudo
            var escudopath = Path.Combine(_env.WebRootPath, "images", "Escudo_UV.png");
            Image escudo = Image.GetInstance(escudopath);
            escudo.ScaleAbsolute(85f, 85f);

            var streamConstancia = new MemoryStream();
            using (streamConstancia)
            {
                var constancia = new Document(PageSize.Letter);
                var writer = PdfWriter.GetInstance(constancia, streamConstancia);
                constancia.AddAuthor("Universidad Veracruzana");
                try
                {
                    constancia.Open();

                    //Estructura del header
                    PdfPTable header = new PdfPTable(7);
                    header.TotalWidth = 550;
                    header.LockedWidth = true;
                    float[] widths = new float[] { 35f, 60f, 120f, 120f, 120f, 60f, 35f };
                    header.SetWidths(widths);

                    //Estructura de la correspondencia
                    PdfPTable correspondenciaConstancia = new PdfPTable(3);
                    correspondenciaConstancia.TotalWidth = 550f;
                    correspondenciaConstancia.LockedWidth = true;
                    float[] widthscc = new float[] { 45f, 460, 45f };
                    correspondenciaConstancia.SetWidths(widthscc);

                    //Estructura de la correspondencia
                    PdfPTable cuerpoConstancia = new PdfPTable(1);
                    cuerpoConstancia.TotalWidth = 550f;
                    cuerpoConstancia.LockedWidth = true;
                    float[] widthsc = new float[] { 550f };
                    cuerpoConstancia.SetWidths(widthsc);

                    //Estructura del cuerpo de la constancia
                    PdfPTable cuerpoSueldo = new PdfPTable(1);
                    cuerpoSueldo.TotalWidth = 550f;
                    cuerpoSueldo.LockedWidth = true;
                    float[] widthps = new float[] { 550f };
                    cuerpoSueldo.SetWidths(widthps);

                    //Tabla del horario
                    PdfPTable tablaConstancia = new PdfPTable(9);
                    tablaConstancia.TotalWidth = 550f;
                    tablaConstancia.LockedWidth = true;
                    float[] widthsTablaconstancia = new float[] { 45f, 65.71f, 65.71f, 65.71f, 65.71f, 65.71f, 65.71f, 65.71f, 45f };
                    tablaConstancia.SetWidths(widthsTablaconstancia);

                    //Estructura del cuerpo de la constancia
                    PdfPTable constanciafirma = new PdfPTable(7);
                    constanciafirma.TotalWidth = 550f;
                    constanciafirma.LockedWidth = true;
                    float[] widthf = new float[] { 45f, 60f, 113.33f, 113.33f, 113.33f, 60f, 45f };
                    constanciafirma.SetWidths(widthf);

                    //Estructura del footer
                    PdfPTable footer = new PdfPTable(7);
                    footer.TotalWidth = 550f;
                    footer.LockedWidth = true;
                    float[] widthsfooter = new float[] { 25f, 65f, 130f, 130f, 130f, 65f, 25f };
                    footer.SetWidths(widthsfooter);

                    //Header
                    var secretaria = "SECRETARÍA DE ADMINISTRACIÓN Y FINANZAS";
                    var direccion = "DIRECCIÓN GENERAL DE RECURSOS HUMANOS";
                    var direccionPersonal = "DIRECCIÓN DE PERSONAL";

                    //Destinario jubilacion    
                    var nombreJefeIPE = "MTRO. EFREN JIMENEZ ROJAS";
                    var departamento = "JEFE  DEL DEPTO. DE VIGENCIA DE DERECHOS";
                    var nombreInstituto = "DEL INSTITUTO DE PENSIONES DEL ESTADO (IPE)";

                    //Variable del cuerpo
                    Chunk nombreTrabajador = new Chunk(modelo.sNombre, GeneralBold);
                    nombreTrabajador.SetUnderline(0.5f, -1.5f);
                    Chunk numeropersonal = new Chunk(modelo.sNumPer, GeneralBold);
                    numeropersonal.SetUnderline(0.5f, -1.5f);
                    Chunk fechaingreso = new Chunk(modelo.dFIngreso, General);
                    fechaingreso.SetUnderline(0.5f, -1.5f);
                    Chunk entidadDepedencia = new Chunk(modelo.sDescDep, General);
                    entidadDepedencia.SetUnderline(0.5f, -1.5f);
                    Chunk region = new Chunk(modelo.sDesRegion, General);
                    region.SetUnderline(0.5f, -1.5f);
                    Chunk tipoPersonal = new Chunk(modelo.sDesTPE, General);
                    tipoPersonal.SetUnderline(0.5f, -1.5f);
                    Chunk tipoContratacion = new Chunk(modelo.sDesCont, General);
                    tipoContratacion.SetUnderline(0.5f, -1.5f);
                    Chunk categoriaSuledo = new Chunk(modelo.sDescCat, General);
                    categoriaSuledo.SetUnderline(0.5f, -1.5f);
                    Chunk puesto = new Chunk(modelo.sDescPuesto, General);
                    puesto.SetUnderline(0.5f, -1.5f);
                    Chunk cantidadSueldo = new Chunk(modelo.sSuelPrest, General);
                    cantidadSueldo.SetUnderline(0.5f, -1.5f);
                    //Dependiente economico
                    Chunk relacionDepedientente = new Chunk(modelo.sParentesco, General);
                    relacionDepedientente.SetUnderline(0.5f, -1.5f);
                    Chunk nombreDepedienteEconomico = new Chunk(modelo.sNomDepen, General);
                    nombreDepedienteEconomico.SetUnderline(0.5f, -1.5f);

                    //Cuerpo formado
                    //Estructura del cuerpo
                    Chunk cuerpo1raParte = new Chunk("De acuerdo a los documentos que se encuentran en el expediente personal, el (la) C. ", General);
                    Chunk cuerpo2daParte = new Chunk(", con número de personal ", General);
                    Chunk cuerpo3raParte = new Chunk(", ingresó el ", General);
                    Chunk cuerpo3rapartefoto = new Chunk(" cuya fotografía aparece al calce, ingresó el ", General);
                    Chunk cuerpo4taParte = new Chunk(", y actualmente presta sus servicios en esta Casa de Estudios en el (la) ", General);
                    Chunk cuerpo5taParte = new Chunk("de la región ", General);
                    Chunk cuerpo6taParte = new Chunk(", como ", General);
                    Chunk cuerpo7maParte = new Chunk(" con tipo de contratación ", General);
                    Chunk cuerpo8vaParte = new Chunk(" con la categoría ", General);
                    Chunk cuerpo9naParte = new Chunk(" y puesto de ", General);
                    Chunk cuerpo10mapartehorario = new Chunk(", cubriendo el siguiente horario:", General);
                    Chunk cuerpo10maParte = new Chunk(", percibe en forma mensual de sueldo la cantidad de ", General);
                    Chunk cuerpo10maPrestaciones = new Chunk(", percibe en forma mensual de sueldo más prestaciones la cantidad de ", General);

                    //Estructura del cuerpo del SM depediente economico
                    Chunk cuerpoSMDE1a = new Chunk(". Solicita la presente para hacer constar que su ", General);
                    Chunk cuerpoSMDE2a = new Chunk(", quien(es) está(n) registrado(as) como su(s) dependiente(s) económico(s), recibe(n) el beneficio de la prestación denominada “Servicio Médico” que brinda la institución. ", General);

                    //Estrucutra del cuerpo de la visa depediente economico
                    Chunk cuerpovisade1 = new Chunk(". Solicita la presente para trámite de VISA de su ", General);
                    Chunk cuerpovisade2 = new Chunk(", cuya fotografía aparece al calce, quien está registrado como su dependiente económico.", General);

                    //Jubilacion ATM
                    Chunk amtsueldomensual = new Chunk(", percibe en forma mensual un sueldo de: ", General);
                    Chunk amtsueldoreconocimiento = new Chunk(" y un reconocimiento de antigüedad de: ", General);
                    Chunk amtsueldototal = new Chunk(" haciendo un total de: ", General);

                    //Integración del cuerpo
                    Paragraph cuerpoCompuesto = new Paragraph();
                    cuerpoCompuesto.Add(cuerpo1raParte);
                    cuerpoCompuesto.Add(nombreTrabajador);
                    cuerpoCompuesto.Add(cuerpo2daParte);
                    cuerpoCompuesto.Add(numeropersonal);
                    if (idTipoConstancia == 9)
                    {
                        cuerpoCompuesto.Add(cuerpo3rapartefoto);
                    }
                    else
                    {
                        cuerpoCompuesto.Add(cuerpo3raParte);
                    }
                    cuerpoCompuesto.Add(fechaingreso);
                    cuerpoCompuesto.Add(cuerpo4taParte);
                    cuerpoCompuesto.Add(entidadDepedencia);
                    cuerpoCompuesto.Add(cuerpo5taParte);
                    cuerpoCompuesto.Add(region);
                    cuerpoCompuesto.Add(cuerpo6taParte);
                    cuerpoCompuesto.Add(tipoPersonal);
                    cuerpoCompuesto.Add(cuerpo7maParte);
                    cuerpoCompuesto.Add(tipoContratacion);
                    cuerpoCompuesto.Add(cuerpo8vaParte);
                    cuerpoCompuesto.Add(categoriaSuledo);
                    cuerpoCompuesto.Add(cuerpo9naParte);
                    cuerpoCompuesto.Add(puesto);
                    if (idTipoConstancia == 2)
                    {
                        cuerpoCompuesto.Add(cuerpoSMDE1a);
                        cuerpoCompuesto.Add(relacionDepedientente);
                        cuerpoCompuesto.Add(nombreDepedienteEconomico);
                        cuerpoCompuesto.Add(cuerpoSMDE2a);
                    }
                    else if (idTipoConstancia == 3)
                    {
                        cuerpoCompuesto.Add(cuerpo10maPrestaciones);
                        cuerpoCompuesto.Add(cantidadSueldo + ".");
                    }
                    else if (idTipoConstancia == 4)
                    {
                        cuerpoCompuesto.Add(cuerpo10mapartehorario);
                    }
                    else if (idTipoConstancia == 9)
                    {
                        cuerpoCompuesto.Add(cuerpo10maPrestaciones);
                        cuerpoCompuesto.Add(cantidadSueldo + ".");
                    }
                    else if (idTipoConstancia == 10)
                    {
                        cuerpoCompuesto.Add(cuerpo10maPrestaciones);
                        cuerpoCompuesto.Add(cantidadSueldo);
                        cuerpoCompuesto.Add(cuerpovisade1);
                        cuerpoCompuesto.Add(relacionDepedientente);
                        cuerpoCompuesto.Add(nombreDepedienteEconomico);
                        cuerpoCompuesto.Add(cuerpovisade2);
                    }
                    else if (idTipoConstancia == 14 && idTipoPersonal != 3)
                    {
                        cuerpoCompuesto.Add(amtsueldomensual);
                        cuerpoCompuesto.Add(cantidadSueldo);
                        cuerpoCompuesto.Add(amtsueldoreconocimiento);
                        cuerpoCompuesto.Add(cantidadSueldo);
                        cuerpoCompuesto.Add(amtsueldototal);
                        cuerpoCompuesto.Add(cantidadSueldo);
                    }
                    else if (idTipoConstancia == 14 && idTipoPersonal == 3)
                    {
                        cuerpoCompuesto.Add(cuerpo10maParte);
                        cuerpoCompuesto.Add(cantidadSueldo);
                        cuerpoCompuesto.Add(cuerpo10mapartehorario);
                    }
                    else
                    {
                        cuerpoCompuesto.Add(cuerpo10maParte);
                        cuerpoCompuesto.Add(cantidadSueldo + ".");
                    }

                    //PRODEP 
                    List list = new List(List.UNORDERED, 10f);
                    list.SetListSymbol("\u2022");
                    list.IndentationLeft = 30f;
                    list.Add(new ListItem(new Chunk("Es Tiempo Completo con esta categoría el: ", General)));

                    //Estructura de la despedida y firma
                    Chunk cuerpo21raparte = new Chunk("A petición de la parte interesada y para los efectos legales que a la misma convenga, se expide la presente ", General);
                    Chunk cuerpo22daparte = new Chunk("CONSTANCIA ", GeneralBold);
                    Chunk cuerpo23raparte = new Chunk("en la ciudad de Xalapa, Eqz., Veracruz al " + fechaExpedicion.ToString("D", CultureInfo.CreateSpecificCulture("es-MX")) + ".", General);

                    //Composicion del 2dfo parrafo
                    Paragraph parrafoFinal = new Paragraph();
                    parrafoFinal.Add(cuerpo21raparte);
                    parrafoFinal.Add(cuerpo22daparte);
                    parrafoFinal.Add(cuerpo23raparte);

                    //Header table
                    BaseColor headtableColor = WebColors.GetRgbColor("#D9D9D9");

                    //Header
                    //Header -padding left
                    PdfPCell cellpaddingleftlogo = new PdfPCell();
                    cellpaddingleftlogo.Rowspan = 5;
                    cellpaddingleftlogo.Colspan = 1;
                    cellpaddingleftlogo.HorizontalAlignment = Element.ALIGN_RIGHT;
                    cellpaddingleftlogo.Border = Rectangle.NO_BORDER;
                    header.AddCell(cellpaddingleftlogo);

                    //Header - escudo
                    PdfPCell cellescudo = new PdfPCell(escudo);
                    cellescudo.Rowspan = 5;
                    cellescudo.Colspan = 1;
                    cellescudo.HorizontalAlignment = Element.ALIGN_RIGHT;
                    cellescudo.Border = Rectangle.NO_BORDER;
                    header.AddCell(cellescudo);

                    PdfPCell cellSecretaria = new PdfPCell(new Phrase(secretaria, General14Bold));
                    cellSecretaria.Colspan = 4;
                    cellSecretaria.HorizontalAlignment = Element.ALIGN_CENTER;
                    cellSecretaria.Border = Rectangle.NO_BORDER;
                    header.AddCell(cellSecretaria);

                    PdfPCell cellpaddingrightlogo = new PdfPCell(new Phrase(""));
                    cellpaddingrightlogo.Colspan = 1;
                    cellpaddingrightlogo.HorizontalAlignment = Element.ALIGN_RIGHT;
                    cellpaddingrightlogo.Border = Rectangle.NO_BORDER;
                    header.AddCell(cellpaddingrightlogo);

                    PdfPCell celldireccion = new PdfPCell(new Phrase(direccion, General12Bold));
                    celldireccion.Colspan = 4;
                    celldireccion.HorizontalAlignment = Element.ALIGN_CENTER;
                    celldireccion.Border = Rectangle.NO_BORDER;
                    header.AddCell(celldireccion);

                    header.AddCell(cellpaddingrightlogo);

                    PdfPCell celldireccionPersonal = new PdfPCell(new Phrase(direccionPersonal, General12Bold));
                    celldireccionPersonal.Colspan = 4;
                    celldireccionPersonal.HorizontalAlignment = Element.ALIGN_CENTER;
                    celldireccionPersonal.Border = Rectangle.NO_BORDER;
                    header.AddCell(celldireccionPersonal);

                    header.AddCell(cellpaddingrightlogo);

                    PdfPCell cellvacia2 = new PdfPCell(new Phrase(""));
                    cellvacia2.Colspan = 4;
                    cellvacia2.HorizontalAlignment = Element.ALIGN_RIGHT;
                    cellvacia2.Border = Rectangle.NO_BORDER;
                    header.AddCell(cellvacia2);

                    header.AddCell(cellpaddingrightlogo);

                    PdfPCell celltipoConstancia = new PdfPCell(new Phrase(nombreConstancia, General13Bold));
                    celltipoConstancia.Colspan = 4;
                    celltipoConstancia.HorizontalAlignment = Element.ALIGN_CENTER;
                    celltipoConstancia.Border = Rectangle.NO_BORDER;
                    header.AddCell(celltipoConstancia);

                    header.AddCell(cellpaddingrightlogo);

                    //Correspondencia y Constancia
                    PdfPCell cellespaciado = new PdfPCell(new Phrase(""));
                    cellespaciado.Colspan = 3;
                    cellespaciado.FixedHeight = 20f;
                    cellespaciado.HorizontalAlignment = Element.ALIGN_RIGHT;
                    cellespaciado.Border = Rectangle.NO_BORDER;
                    correspondenciaConstancia.AddCell(cellespaciado);

                    if (idTipoConstancia == 14)
                    {
                        PdfPCell cellcorrespondenciaJefe = new PdfPCell(new Phrase(nombreJefeIPE, GeneralBold));
                        cellcorrespondenciaJefe.Colspan = 3;
                        cellcorrespondenciaJefe.PaddingLeft = 45f;
                        cellcorrespondenciaJefe.PaddingRight = 45f;
                        cellcorrespondenciaJefe.HorizontalAlignment = Element.ALIGN_LEFT;
                        cellcorrespondenciaJefe.Border = Rectangle.NO_BORDER;
                        correspondenciaConstancia.AddCell(cellcorrespondenciaJefe);

                        PdfPCell celljefe = new PdfPCell(new Phrase(departamento, GeneralBold));
                        celljefe.Colspan = 3;
                        celljefe.PaddingLeft = 45f;
                        celljefe.PaddingRight = 45f;
                        celljefe.HorizontalAlignment = Element.ALIGN_LEFT;
                        celljefe.Border = Rectangle.NO_BORDER;
                        correspondenciaConstancia.AddCell(celljefe);

                        PdfPCell cellinstituto = new PdfPCell(new Phrase(nombreInstituto, GeneralBold));
                        cellinstituto.Colspan = 3;
                        cellinstituto.PaddingLeft = 45f;
                        cellinstituto.PaddingRight = 45f;
                        cellinstituto.HorizontalAlignment = Element.ALIGN_LEFT;
                        cellinstituto.Border = Rectangle.NO_BORDER;
                        correspondenciaConstancia.AddCell(cellinstituto);

                        PdfPCell cellPresente = new PdfPCell(new Phrase("P R E S E N T E ", GeneralBold));
                        cellPresente.Colspan = 6;
                        cellPresente.PaddingLeft = 45f;
                        cellPresente.PaddingRight = 45f;
                        cellPresente.HorizontalAlignment = Element.ALIGN_LEFT;
                        cellPresente.Border = Rectangle.NO_BORDER;
                        correspondenciaConstancia.AddCell(cellPresente);
                    }
                    else
                    {
                        PdfPCell cellcorrespondencia = new PdfPCell(new Phrase("A QUIEN CORRESPONDA:", GeneralBold));
                        cellcorrespondencia.Colspan = 3;
                        cellcorrespondencia.PaddingLeft = 45f;
                        cellcorrespondencia.PaddingRight = 45f;
                        cellcorrespondencia.HorizontalAlignment = Element.ALIGN_LEFT;
                        cellcorrespondencia.Border = Rectangle.NO_BORDER;
                        correspondenciaConstancia.AddCell(cellcorrespondencia);
                    }
                    correspondenciaConstancia.AddCell(cellespaciado);

                    PdfPCell cellsuscribe = new PdfPCell(new Phrase("El(La) que suscribe Director(a) de Personal de la Universidad Veracruzana en esta ciudad", General));
                    cellsuscribe.Colspan = 3;
                    cellsuscribe.PaddingLeft = 45f;
                    cellsuscribe.PaddingRight = 45f;
                    cellsuscribe.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                    cellsuscribe.Border = Rectangle.NO_BORDER;
                    correspondenciaConstancia.AddCell(cellsuscribe);

                    correspondenciaConstancia.AddCell(cellespaciado);

                    PdfPCell cell1 = new PdfPCell(new Phrase("HACE CONSTAR QUE", GeneralBold));
                    cell1.Colspan = 3;
                    cell1.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell1.Border = Rectangle.NO_BORDER;
                    correspondenciaConstancia.AddCell(cell1);

                    //Cuerpo primera parte
                    PdfPCell cellcuerpo = new PdfPCell(cuerpoCompuesto);
                    cellcuerpo.Colspan = 1;
                    cellcuerpo.PaddingLeft = 45f;
                    cellcuerpo.PaddingRight = 45f;
                    cellcuerpo.SetLeading(15f, 1f);
                    cellcuerpo.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                    cellcuerpo.Border = Rectangle.NO_BORDER;
                    cuerpoConstancia.AddCell(cellcuerpo);

                    PdfPCell cellcuerpoespaciado = new PdfPCell();
                    cellcuerpoespaciado.Colspan = 1;
                    cellcuerpoespaciado.PaddingLeft = 45f;
                    cellcuerpoespaciado.PaddingRight = 45f;
                    cellcuerpoespaciado.SetLeading(15f, 1f);
                    cellcuerpoespaciado.FixedHeight = 25f;
                    cellcuerpoespaciado.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                    cellcuerpoespaciado.Border = Rectangle.NO_BORDER;
                    cuerpoConstancia.AddCell(cellcuerpoespaciado);

                    //Tabla del horario de la constancia
                    PdfPCell cellpaddingleft = new PdfPCell(new Phrase(""));
                    cellpaddingleft.Colspan = 1;
                    cellpaddingleft.HorizontalAlignment = Element.ALIGN_CENTER;
                    cellpaddingleft.Border = Rectangle.NO_BORDER;
                    tablaConstancia.AddCell(cellpaddingleft);

                    PdfPCell cellheaderPuesto = new PdfPCell(new Phrase("Puesto", General));
                    cellheaderPuesto.Colspan = 7;
                    cellheaderPuesto.HorizontalAlignment = Element.ALIGN_CENTER;
                    cellheaderPuesto.Border = Rectangle.BOX;
                    tablaConstancia.AddCell(cellheaderPuesto);

                    PdfPCell cellpaddingright = new PdfPCell(new Phrase(""));
                    cellpaddingright.Colspan = 1;
                    cellpaddingright.HorizontalAlignment = Element.ALIGN_CENTER;
                    cellpaddingright.Border = Rectangle.NO_BORDER;
                    tablaConstancia.AddCell(cellpaddingright);

                    tablaConstancia.AddCell(cellpaddingleft);

                    PdfPCell cellheaderLunes = new PdfPCell(new Phrase("LUNES", General));
                    cellheaderLunes.Colspan = 1;
                    cellheaderLunes.HorizontalAlignment = Element.ALIGN_CENTER;
                    cellheaderLunes.Border = Rectangle.BOX;
                    tablaConstancia.AddCell(cellheaderLunes);

                    PdfPCell cellheaderMartes = new PdfPCell(new Phrase("MARTES", General));
                    cellheaderMartes.Colspan = 1;
                    cellheaderMartes.HorizontalAlignment = Element.ALIGN_CENTER;
                    cellheaderMartes.Border = Rectangle.BOX;
                    tablaConstancia.AddCell(cellheaderMartes);

                    PdfPCell cellheaderMier = new PdfPCell(new Phrase("MIÉRCOLES", General));
                    cellheaderMier.Colspan = 1;
                    cellheaderMier.HorizontalAlignment = Element.ALIGN_CENTER;
                    cellheaderMier.Border = Rectangle.BOX;
                    tablaConstancia.AddCell(cellheaderMier);

                    PdfPCell cellheaderJueves = new PdfPCell(new Phrase("JUEVES", General));
                    cellheaderJueves.Colspan = 1;
                    cellheaderJueves.HorizontalAlignment = Element.ALIGN_CENTER;
                    cellheaderJueves.Border = Rectangle.BOX;
                    tablaConstancia.AddCell(cellheaderJueves);

                    PdfPCell cellheaderViernes = new PdfPCell(new Phrase("VIERNES", General));
                    cellheaderViernes.Colspan = 1;
                    cellheaderViernes.HorizontalAlignment = Element.ALIGN_CENTER;
                    cellheaderViernes.Border = Rectangle.BOX;
                    tablaConstancia.AddCell(cellheaderViernes);

                    PdfPCell cellheaderSabado = new PdfPCell(new Phrase("SÁBADO", General));
                    cellheaderSabado.Colspan = 1;
                    cellheaderSabado.HorizontalAlignment = Element.ALIGN_CENTER;
                    cellheaderSabado.Border = Rectangle.BOX;
                    tablaConstancia.AddCell(cellheaderSabado);

                    PdfPCell cellheaderDomingo = new PdfPCell(new Phrase("DOMINGO", General));
                    cellheaderDomingo.Colspan = 1;
                    cellheaderDomingo.HorizontalAlignment = Element.ALIGN_CENTER;
                    cellheaderDomingo.Border = Rectangle.BOX;
                    tablaConstancia.AddCell(cellheaderDomingo);

                    tablaConstancia.AddCell(cellpaddingright);

                    PdfPCell cellpaddingbodyleft = new PdfPCell(new Phrase(""));
                    cellpaddingbodyleft.Colspan = 1;
                    cellpaddingbodyleft.HorizontalAlignment = Element.ALIGN_CENTER;
                    cellpaddingbodyleft.Border = Rectangle.NO_BORDER;
                    cellpaddingbodyleft.FixedHeight = 40f;
                    tablaConstancia.AddCell(cellpaddingbodyleft);

                    PdfPCell cellbodyLunes = new PdfPCell(new Phrase("", General));
                    cellbodyLunes.Colspan = 1;
                    cellbodyLunes.HorizontalAlignment = Element.ALIGN_CENTER;
                    cellbodyLunes.Border = Rectangle.BOX;
                    cellbodyLunes.FixedHeight = 40f;
                    tablaConstancia.AddCell(cellbodyLunes);

                    PdfPCell cellhbodyMartes = new PdfPCell(new Phrase("", General));
                    cellhbodyMartes.Colspan = 1;
                    cellhbodyMartes.HorizontalAlignment = Element.ALIGN_CENTER;
                    cellhbodyMartes.Border = Rectangle.BOX;
                    cellhbodyMartes.FixedHeight = 40f;
                    tablaConstancia.AddCell(cellhbodyMartes);

                    PdfPCell cellbodyMier = new PdfPCell(new Phrase("", General));
                    cellbodyMier.Colspan = 1;
                    cellbodyMier.HorizontalAlignment = Element.ALIGN_CENTER;
                    cellbodyMier.Border = Rectangle.BOX;
                    cellbodyMier.FixedHeight = 40f;
                    tablaConstancia.AddCell(cellbodyMier);

                    PdfPCell cellbodyJueves = new PdfPCell(new Phrase("", General));
                    cellbodyJueves.Colspan = 1;
                    cellbodyJueves.HorizontalAlignment = Element.ALIGN_CENTER;
                    cellbodyJueves.Border = Rectangle.BOX;
                    cellbodyJueves.FixedHeight = 40f;
                    tablaConstancia.AddCell(cellbodyJueves);

                    PdfPCell cellbodyViernes = new PdfPCell(new Phrase("", General));
                    cellbodyViernes.Colspan = 1;
                    cellbodyViernes.HorizontalAlignment = Element.ALIGN_CENTER;
                    cellbodyViernes.Border = Rectangle.BOX;
                    cellbodyViernes.FixedHeight = 40f;
                    tablaConstancia.AddCell(cellbodyViernes);

                    PdfPCell cellbodySabado = new PdfPCell(new Phrase("", General));
                    cellbodySabado.Colspan = 1;
                    cellbodySabado.HorizontalAlignment = Element.ALIGN_CENTER;
                    cellbodySabado.Border = Rectangle.BOX;
                    cellbodySabado.FixedHeight = 40f;
                    tablaConstancia.AddCell(cellbodySabado);

                    PdfPCell cellbodyDomingo = new PdfPCell(new Phrase("", General));
                    cellbodyDomingo.Colspan = 1;
                    cellbodyDomingo.HorizontalAlignment = Element.ALIGN_CENTER;
                    cellbodyDomingo.Border = Rectangle.BOX;
                    cellbodyDomingo.FixedHeight = 40f;
                    tablaConstancia.AddCell(cellbodyDomingo);

                    PdfPCell cellpaddingbodyright = new PdfPCell(new Phrase(""));
                    cellpaddingbodyright.Colspan = 1;
                    cellpaddingbodyright.HorizontalAlignment = Element.ALIGN_CENTER;
                    cellpaddingbodyright.Border = Rectangle.NO_BORDER;
                    cellpaddingbodyright.FixedHeight = 40f;
                    tablaConstancia.AddCell(cellpaddingbodyright);

                    //Despedida y Firma
                    PdfPCell cellcuerpo2 = new PdfPCell(parrafoFinal);
                    cellcuerpo2.Colspan = 7;
                    cellcuerpo2.SetLeading(15f, 1f);
                    cellcuerpo2.PaddingLeft = 45f;
                    cellcuerpo2.PaddingRight = 45f;
                    cellcuerpo2.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                    cellcuerpo2.Border = Rectangle.NO_BORDER;
                    constanciafirma.AddCell(cellcuerpo2);

                    PdfPCell cellcuerpo2esp = new PdfPCell();
                    cellcuerpo2esp.Colspan = 7;
                    cellcuerpo2esp.SetLeading(15f, 1f);
                    cellcuerpo2esp.PaddingLeft = 45f;
                    cellcuerpo2esp.PaddingRight = 45f;
                    cellcuerpo2esp.FixedHeight = 40f;
                    cellcuerpo2esp.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                    cellcuerpo2esp.Border = Rectangle.NO_BORDER;
                    constanciafirma.AddCell(cellcuerpo2esp);

                    if (idTipoConstancia == 9 || idTipoConstancia == 10)
                    {
                        PdfPCell celldirectorpaddingleftfoto = new PdfPCell(new Phrase(""));
                        celldirectorpaddingleftfoto.Colspan = 1;
                        celldirectorpaddingleftfoto.FixedHeight = 75f;
                        celldirectorpaddingleftfoto.Border = Rectangle.NO_BORDER;
                        constanciafirma.AddCell(celldirectorpaddingleftfoto);

                        PdfPCell cellfoto = new PdfPCell(new Phrase("Fotografía", GeneralFoto));
                        cellfoto.Colspan = 1;
                        cellfoto.FixedHeight = 75f;
                        cellfoto.HorizontalAlignment = Element.ALIGN_CENTER;
                        cellfoto.VerticalAlignment = Element.ALIGN_CENTER;
                        cellfoto.Border = Rectangle.BOX;
                        constanciafirma.AddCell(cellfoto);
                    }
                    else
                    {
                        PdfPCell celldirectorpaddingleft = new PdfPCell(new Phrase(""));
                        celldirectorpaddingleft.Colspan = 2;
                        celldirectorpaddingleft.FixedHeight = 75f;
                        celldirectorpaddingleft.Border = Rectangle.NO_BORDER;
                        constanciafirma.AddCell(celldirectorpaddingleft);
                    }

                    PdfPCell celldirector = new PdfPCell(new Phrase("NOMBRE DEL DIRECTOR(A) DE PERSONAL)", GeneralBold));
                    celldirector.Colspan = 3;
                    celldirector.FixedHeight = 75f;
                    celldirector.VerticalAlignment = Element.ALIGN_BOTTOM;
                    celldirector.HorizontalAlignment = Element.ALIGN_CENTER;
                    celldirector.Border = Rectangle.NO_BORDER;
                    constanciafirma.AddCell(celldirector);

                    PdfPCell celldirectorpaddingright = new PdfPCell(new Phrase(""));
                    celldirectorpaddingright.Colspan = 2;
                    celldirectorpaddingright.FixedHeight = 75f;
                    celldirectorpaddingright.HorizontalAlignment = Element.ALIGN_CENTER;
                    celldirectorpaddingright.Border = Rectangle.NO_BORDER;
                    constanciafirma.AddCell(celldirectorpaddingright);

                    PdfPCell cellvacia = new PdfPCell(new Phrase(""));
                    cellvacia.Colspan = 7;
                    if (idTipoConstancia == 4)
                    {
                        cellvacia.FixedHeight = 90f;
                    }
                    else if (idTipoConstancia == 14 && idTipoPersonal != 3)
                    {
                        cellvacia.FixedHeight = 100f;
                    }
                    else if (idTipoConstancia == 14 && idTipoPersonal == 3)
                    {
                        cellvacia.FixedHeight = 45f;
                    }
                    else
                    {
                        cellvacia.FixedHeight = 150f;
                    }
                    cellvacia.HorizontalAlignment = Element.ALIGN_CENTER;
                    cellvacia.Border = Rectangle.NO_BORDER;
                    constanciafirma.AddCell(cellvacia);

                    //Footer
                    PdfPCell cellflup = new PdfPCell(new Phrase(""));
                    cellflup.Colspan = 7;
                    cellflup.HorizontalAlignment = Element.ALIGN_CENTER;
                    cellflup.Border = Rectangle.BOTTOM_BORDER;
                    cellflup.BorderWidthBottom = 0.5f;
                    footer.AddCell(cellflup);

                    PdfPCell cellfldown = new PdfPCell(new Phrase(""));
                    cellfldown.Colspan = 7;
                    cellfldown.HorizontalAlignment = Element.ALIGN_CENTER;
                    cellfldown.Border = Rectangle.BOTTOM_BORDER;
                    cellfldown.BorderWidthBottom = 0.5f;
                    footer.AddCell(cellfldown);

                    PdfPCell cellvaciafooter1 = new PdfPCell(new Phrase(""));
                    cellvaciafooter1.Colspan = 2;
                    cellvaciafooter1.HorizontalAlignment = Element.ALIGN_CENTER;
                    cellvaciafooter1.Border = Rectangle.NO_BORDER;
                    footer.AddCell(cellvaciafooter1);

                    PdfPCell cellDir = new PdfPCell(new Phrase("Edificio \"A\" de Rectoría, Planta Baja ", GeneralFooter));
                    cellDir.Colspan = 1;
                    cellDir.HorizontalAlignment = Element.ALIGN_CENTER;
                    cellDir.Border = Rectangle.NO_BORDER;
                    footer.AddCell(cellDir);

                    PdfPCell cellvaciafooter2 = new PdfPCell(new Phrase(""));
                    cellvaciafooter2.Colspan = 1;
                    cellvaciafooter2.HorizontalAlignment = Element.ALIGN_CENTER;
                    cellvaciafooter2.Border = Rectangle.NO_BORDER;
                    footer.AddCell(cellvaciafooter2);

                    PdfPCell cellTel = new PdfPCell(new Phrase("Teléfono 842-17-00 Ext. 11705", GeneralFooter));
                    cellTel.Colspan = 1;
                    cellTel.HorizontalAlignment = Element.ALIGN_CENTER;
                    cellTel.Border = Rectangle.NO_BORDER;
                    footer.AddCell(cellTel);

                    footer.AddCell(cellvaciafooter1);

                    constancia.Add(header);
                    constancia.Add(correspondenciaConstancia);
                    constancia.Add(cuerpoConstancia);
                    if (idTipoConstancia == 4 || (idTipoConstancia == 14 && idTipoPersonal == 3))
                    {
                        constancia.Add(tablaConstancia);
                    }
                    constancia.Add(constanciafirma);
                    constancia.Add(footer);
                }
                catch (Exception e)
                {
                    _logger.Log(LogLevel.Error, "Error catch: " + e.Message);
                }
                finally
                {
                    constancia.Close();
                }
            }
            return streamConstancia.ToArray();
        }

        public byte[] DescargarOficioBajaIPEMagisterio()
        {
            //Definicion de las fuentes
            Font GeneralFooter = FontFactory.GetFont("Arial", 8, Font.NORMAL, BaseColor.Black);
            Font GeneralFoto = FontFactory.GetFont("Arial", 10, Font.ITALIC, BaseColor.Black);
            Font General = FontFactory.GetFont("Arial", 11, Font.NORMAL, BaseColor.Black);
            Font GeneralBold = FontFactory.GetFont("Arial", 11, Font.BOLD, BaseColor.Black);
            Font General12Bold = FontFactory.GetFont("Arial", 13, Font.BOLD, BaseColor.Black);
            Font General13Bold = FontFactory.GetFont("Arial", 13, Font.BOLD, BaseColor.Black);
            Font General14Bold = FontFactory.GetFont("Arial", 14, Font.BOLD, BaseColor.Black);

            // Escudo
            var escudopath = Path.Combine(_env.WebRootPath, "images", "Escudo_UV.png");
            Image escudo = Image.GetInstance(escudopath);
            escudo.ScaleAbsolute(85f, 85f);

            var streamConstancia = new MemoryStream();
            using (streamConstancia)
            {
                var oficio = new Document(PageSize.Letter);
                var writer = PdfWriter.GetInstance(oficio, streamConstancia);
                oficio.AddAuthor("Universidad Veracruzana");
                try
                {
                    oficio.Open();

                    //Estructura de la constancia del header
                    PdfPTable estructuraHeader = new PdfPTable(7);
                    estructuraHeader.TotalWidth = 550f;
                    estructuraHeader.LockedWidth = true;
                    float[] widths = new float[] { 25f, 65f, 130f, 130f, 130f, 65f, 25f };
                    estructuraHeader.SetWidths(widths);

                    //Estructura del cuerpo
                    PdfPTable estructuraCuerpo = new PdfPTable(1);
                    estructuraCuerpo.TotalWidth = 550f;
                    estructuraCuerpo.LockedWidth = true;
                    float[] width = new float[] { 550f };
                    estructuraCuerpo.SetWidths(width);

                    //Estructura de la constancia del footer
                    PdfPTable estructuraFooter = new PdfPTable(7);
                    estructuraFooter.TotalWidth = 550f;
                    estructuraFooter.LockedWidth = true;
                    float[] widthsF = new float[] { 25f, 65f, 130f, 130f, 130f, 65f, 25f };
                    estructuraFooter.SetWidths(widthsF);

                    //Header
                    var direccion = "Dirección General de Recursos Humanos";
                    var subdireccion = "Dirección de Personal";

                    //Comparacion de fechas
                    string stringFechaBaja = "2022-01-21T00:00:00";
                    var parseDate = DateTime.Parse(stringFechaBaja);

                    int comparacionFecha = DateTime.Compare(DateTime.Today, parseDate);

                    //Cuerpo
                    Chunk stringCuerpo1a = new Chunk("El (la) C.", General);
                    Chunk stringCuerpo2a = new Chunk(" en el (la) ", General);
                    Chunk stringCuerpo3a = new Chunk(" en ", General);
                    Chunk stringCuerpo41a = new Chunk(", causó  baja en su plaza el ", General);
                    Chunk stringCuerpo42a = new Chunk(", causará baja en su plaza el ", General);
                    Chunk stringCuerpo5a = new Chunk(", por Jubilación.", General);

                    //Variables del oficio 
                    Chunk nombreTrabajador = new Chunk("", GeneralBold);
                    nombreTrabajador.SetUnderline(0.5f, -1.5f);
                    Chunk categoria = new Chunk("", General);
                    categoria.SetUnderline(0.5f, -1.5f);
                    Chunk puesto = new Chunk("", General);
                    puesto.SetUnderline(0.5f, -1.5f);
                    Chunk entidadDependencia = new Chunk("", General);
                    entidadDependencia.SetUnderline(0.5f, -1.5f);
                    Chunk region = new Chunk("", General);
                    region.SetUnderline(0.5f, -1.5f);
                    Chunk fechaBaja = new Chunk("", General);
                    fechaBaja.SetUnderline(0.5f, -1.5f);

                    //Composicion del mensaje
                    Paragraph cuerpo = new Paragraph();
                    cuerpo.Add(stringCuerpo1a);
                    cuerpo.Add(nombreTrabajador);
                    cuerpo.Add(stringCuerpo2a);
                    cuerpo.Add(categoria);
                    cuerpo.Add(puesto);
                    cuerpo.Add(stringCuerpo3a);
                    cuerpo.Add(entidadDependencia);
                    if (comparacionFecha >= 0)
                    {
                        cuerpo.Add(stringCuerpo41a);
                    }
                    else
                    {
                        cuerpo.Add(stringCuerpo42a);
                    }
                    cuerpo.Add(fechaBaja);
                    cuerpo.Add(stringCuerpo5a);

                    var stringConocimiento = "Lo anterior lo hago de su conocimiento para los trámites a que haya lugar.";

                    PdfPCell cellpadingescudoleft = new PdfPCell();
                    cellpadingescudoleft.Colspan = 3;
                    cellpadingescudoleft.HorizontalAlignment = Element.ALIGN_CENTER;
                    cellpadingescudoleft.Border = Rectangle.NO_BORDER;
                    cellpadingescudoleft.FixedHeight = 90f;
                    estructuraHeader.AddCell(cellpadingescudoleft);

                    PdfPCell cellescudo = new PdfPCell(escudo);
                    cellescudo.Colspan = 1;
                    cellescudo.HorizontalAlignment = Element.ALIGN_CENTER;
                    cellescudo.FixedHeight = 90f;
                    cellescudo.Border = Rectangle.BOTTOM_BORDER;
                    estructuraHeader.AddCell(cellescudo);

                    PdfPCell cellpadingescudoright = new PdfPCell();
                    cellpadingescudoright.Colspan = 3;
                    cellpadingescudoright.HorizontalAlignment = Element.ALIGN_CENTER;
                    cellpadingescudoright.Border = Rectangle.NO_BORDER;
                    cellpadingescudoright.FixedHeight = 90f;
                    estructuraHeader.AddCell(cellpadingescudoright);

                    PdfPCell cellDireccionRH = new PdfPCell(new Phrase(direccion, General14Bold));
                    cellDireccionRH.Colspan = 7;
                    cellDireccionRH.HorizontalAlignment = Element.ALIGN_CENTER;
                    cellDireccionRH.Border = Rectangle.NO_BORDER;
                    estructuraHeader.AddCell(cellDireccionRH);

                    PdfPCell cellDireccion = new PdfPCell(new Phrase(subdireccion, General13Bold));
                    cellDireccion.Colspan = 7;
                    cellDireccion.HorizontalAlignment = Element.ALIGN_CENTER;
                    cellDireccion.Border = Rectangle.NO_BORDER;
                    estructuraHeader.AddCell(cellDireccion);

                    PdfPCell cellvacia1 = new PdfPCell(new Phrase(""));
                    cellvacia1.Colspan = 7;
                    cellvacia1.FixedHeight = 20f;
                    cellvacia1.HorizontalAlignment = Element.ALIGN_RIGHT;
                    cellvacia1.Border = Rectangle.NO_BORDER;
                    estructuraCuerpo.AddCell(cellvacia1);

                    PdfPCell celcorrespondencia = new PdfPCell(new Phrase("A QUIEN CORRESPONDA:", GeneralBold));
                    celcorrespondencia.Colspan = 1;
                    celcorrespondencia.PaddingLeft = 45f;
                    celcorrespondencia.PaddingRight = 45f;
                    celcorrespondencia.HorizontalAlignment = Element.ALIGN_LEFT;
                    celcorrespondencia.Border = Rectangle.NO_BORDER;
                    estructuraCuerpo.AddCell(celcorrespondencia);

                    PdfPCell cellvacia2 = new PdfPCell(new Phrase(""));
                    cellvacia2.Colspan = 1;
                    cellvacia2.HorizontalAlignment = Element.ALIGN_RIGHT;
                    cellvacia2.FixedHeight = 60f;
                    cellvacia2.Border = Rectangle.NO_BORDER;
                    estructuraCuerpo.AddCell(cellvacia2);

                    PdfPCell cellsuscribe = new PdfPCell(new Phrase("Por este medio comunico a usted que:", General));
                    cellsuscribe.Colspan = 1;
                    cellsuscribe.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                    cellsuscribe.PaddingLeft = 45f;
                    cellsuscribe.PaddingRight = 45f;
                    cellsuscribe.Border = Rectangle.NO_BORDER;
                    estructuraCuerpo.AddCell(cellsuscribe);

                    PdfPCell cellcuerpo = new PdfPCell(cuerpo);
                    cellcuerpo.Colspan = 1;
                    cellcuerpo.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                    cellcuerpo.SetLeading(20f, 0f);
                    cellcuerpo.PaddingLeft = 45f;
                    cellcuerpo.PaddingRight = 45f;
                    cellcuerpo.Border = Rectangle.NO_BORDER;
                    estructuraCuerpo.AddCell(cellcuerpo);

                    PdfPCell cellvacia3 = new PdfPCell(new Phrase(""));
                    cellvacia3.Colspan = 1;
                    cellvacia3.HorizontalAlignment = Element.ALIGN_RIGHT;
                    cellvacia3.FixedHeight = 35f;
                    cellvacia3.Border = Rectangle.NO_BORDER;
                    estructuraCuerpo.AddCell(cellvacia3);

                    PdfPCell cellcuerpo2 = new PdfPCell(new Paragraph(stringConocimiento, General));
                    cellcuerpo2.Colspan = 1;
                    cellcuerpo2.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                    cellcuerpo2.SetLeading(20f, 0f);
                    cellcuerpo2.PaddingLeft = 45f;
                    cellcuerpo2.PaddingRight = 45f;
                    cellcuerpo2.Border = Rectangle.NO_BORDER;
                    estructuraCuerpo.AddCell(cellcuerpo2);

                    PdfPCell cellvacia4 = new PdfPCell(new Phrase(""));
                    cellvacia4.Colspan = 1;
                    cellvacia4.HorizontalAlignment = Element.ALIGN_RIGHT;
                    cellvacia4.FixedHeight = 90f;
                    cellvacia4.Border = Rectangle.NO_BORDER;
                    estructuraCuerpo.AddCell(cellvacia4);

                    PdfPCell cellatt = new PdfPCell(new Phrase("A T E N T A M E N T E", General));
                    cellatt.Colspan = 1;
                    cellatt.HorizontalAlignment = Element.ALIGN_CENTER;
                    cellatt.PaddingLeft = 45f;
                    cellatt.PaddingRight = 45f;
                    cellatt.Border = Rectangle.NO_BORDER;
                    estructuraCuerpo.AddCell(cellatt);

                    PdfPCell cellslogan = new PdfPCell(new Phrase("\"LIS DE VERACRUZ: ARTE, CIENCIA Y LUZ\"", General));
                    cellslogan.Colspan = 1;
                    cellslogan.HorizontalAlignment = Element.ALIGN_CENTER;
                    cellslogan.PaddingLeft = 45f;
                    cellslogan.PaddingRight = 45f;
                    cellslogan.Border = Rectangle.NO_BORDER;
                    estructuraCuerpo.AddCell(cellslogan);

                    PdfPCell cellslugarfecha = new PdfPCell(new Phrase("Xalapa-Ez., Ver. " + fechaExpedicion.ToString("D", CultureInfo.CreateSpecificCulture("es-MX")), General));
                    cellslugarfecha.Colspan = 1;
                    cellslugarfecha.HorizontalAlignment = Element.ALIGN_CENTER;
                    cellslugarfecha.PaddingLeft = 45f;
                    cellslugarfecha.PaddingRight = 45f;
                    cellslugarfecha.Border = Rectangle.NO_BORDER;
                    estructuraCuerpo.AddCell(cellslugarfecha);

                    PdfPCell cellvacia5 = new PdfPCell(new Phrase(""));
                    cellvacia5.Colspan = 7;
                    cellvacia5.HorizontalAlignment = Element.ALIGN_RIGHT;
                    cellvacia5.FixedHeight = 100f;
                    cellvacia5.Border = Rectangle.NO_BORDER;
                    estructuraCuerpo.AddCell(cellvacia5);

                    PdfPCell cellvacia6 = new PdfPCell(new Phrase(""));
                    cellvacia6.Colspan = 1;
                    cellvacia6.Border = Rectangle.NO_BORDER;
                    estructuraCuerpo.AddCell(cellvacia6);

                    PdfPCell celldirector = new PdfPCell(new Phrase("NOMBRE DEL DIRECTOR(A) DE PERSONAL \n DIRECTORA DE PERSONAL", GeneralBold));
                    celldirector.Colspan = 1;
                    celldirector.PaddingLeft = 45f;
                    celldirector.PaddingRight = 45f;
                    celldirector.VerticalAlignment = Element.ALIGN_BOTTOM;
                    celldirector.HorizontalAlignment = Element.ALIGN_CENTER;
                    celldirector.Border = Rectangle.NO_BORDER;
                    estructuraCuerpo.AddCell(celldirector);

                    PdfPCell cellvacia7 = new PdfPCell(new Phrase(""));
                    cellvacia7.Colspan = 1;
                    cellvacia7.HorizontalAlignment = Element.ALIGN_CENTER;
                    cellvacia7.Border = Rectangle.NO_BORDER;
                    estructuraCuerpo.AddCell(cellvacia7);

                    PdfPCell cellvacia8 = new PdfPCell(new Phrase(""));
                    cellvacia8.Colspan = 7;
                    cellvacia8.FixedHeight = 85f;
                    cellvacia8.HorizontalAlignment = Element.ALIGN_CENTER;
                    cellvacia8.Border = Rectangle.NO_BORDER;
                    estructuraCuerpo.AddCell(cellvacia8);

                    PdfPCell cellfldown = new PdfPCell(new Phrase(""));
                    cellfldown.Colspan = 7;
                    cellfldown.PaddingLeft = 45f;
                    cellfldown.PaddingRight = 45f;
                    cellfldown.HorizontalAlignment = Element.ALIGN_CENTER;
                    cellfldown.Border = Rectangle.BOTTOM_BORDER;
                    cellfldown.BorderWidthBottom = 0.5f;
                    estructuraFooter.AddCell(cellfldown);

                    PdfPCell cellvacia9 = new PdfPCell(new Phrase(""));
                    cellvacia9.Colspan = 2;
                    cellvacia9.HorizontalAlignment = Element.ALIGN_CENTER;
                    cellvacia9.Border = Rectangle.NO_BORDER;
                    estructuraFooter.AddCell(cellvacia9);

                    PdfPCell cellDir = new PdfPCell(new Phrase("Edificio \"A\" de Rectoría, Planta Baja ", GeneralFooter));
                    cellDir.Colspan = 1;
                    cellDir.HorizontalAlignment = Element.ALIGN_CENTER;
                    cellDir.Border = Rectangle.NO_BORDER;
                    estructuraFooter.AddCell(cellDir);

                    PdfPCell cellvacia10 = new PdfPCell(new Phrase(""));
                    cellvacia10.Colspan = 1;
                    cellvacia10.HorizontalAlignment = Element.ALIGN_CENTER;
                    cellvacia10.Border = Rectangle.NO_BORDER;
                    estructuraFooter.AddCell(cellvacia10);

                    PdfPCell cellTel = new PdfPCell(new Phrase("Teléfono 842-17-00 Ext. 11705", GeneralFooter));
                    cellTel.Colspan = 1;
                    cellTel.HorizontalAlignment = Element.ALIGN_CENTER;
                    cellTel.Border = Rectangle.NO_BORDER;
                    estructuraFooter.AddCell(cellTel);
                    estructuraFooter.AddCell(cellvacia9);

                    oficio.Add(estructuraHeader);
                    oficio.Add(estructuraCuerpo);
                    oficio.Add(estructuraFooter);
                }
                catch (Exception e)
                {
                    _logger.Log(LogLevel.Error, "Error catch: " + e.Message);
                }
                finally
                {
                    oficio.Close();
                }
            }
            return streamConstancia.ToArray();
        }
    }
}

using Hermes2018.Data;
using Hermes2018.Helpers;
using Hermes2018.ViewModels;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Threading.Tasks;

namespace Hermes2018.Services
{
    public class MailService : IMailService
    {
        private readonly IConfiguracionService _configuracionService;
        private readonly IHostingEnvironment _environment;
        private readonly ILogger<MailService> _logger;

        public MailService(IConfiguracionService configuracionService,
            IHostingEnvironment environment,
            ILogger<MailService> logger)
        {
            _configuracionService = configuracionService;
            _environment = environment;
            _logger = logger;
        }

        //Envio correo
        public ResumenInfoDestinatariosOrigenViewModel Envio { get; set; }
        public async Task<bool> EnviarCorreo(string[] para, string[] ccp, string[] anexos, string asunto, string cuerpo)
        {
            var configuracion = await _configuracionService.ObtenerInfoConfiguracionEmailAsync();
            bool resultado = false;
            //--
            using (var cliente = new SmtpClient(configuracion.HER_ServidorSMTP, configuracion.HER_PuertoServidorSMTP))
            {
                try
                {
                    cliente.EnableSsl = true;
                    cliente.DeliveryMethod = SmtpDeliveryMethod.Network;
                    cliente.UseDefaultCredentials = false;
                    cliente.Credentials = new System.Net.NetworkCredential(configuracion.HER_CorreoNotificador, configuracion.HER_ContraseniaCorreoNotificador);
                    //--
                    var mensaje = new MailMessage
                    {
                        From = new MailAddress(configuracion.HER_CorreoNotificador),
                        Subject = string.Format("[HERMES v2] {0}", asunto),
                        Body = cuerpo,
                        BodyEncoding = System.Text.Encoding.UTF8,
                        SubjectEncoding = System.Text.Encoding.UTF8,
                        HeadersEncoding = System.Text.Encoding.UTF8,
                        IsBodyHtml = true
                    };
                    //--
                    //Aqui se va registrando los correos a quien van dirigido (para)
                    foreach (string p in para)
                    {
                        mensaje.To.Add(p);
                    }
                    //--
                    //aqui usuarios con copia (para)
                    if (ccp != null)
                    {
                        foreach (string c in ccp)
                        {
                            mensaje.CC.Add(c);
                        }
                    }

                    //--
                   //await cliente.SendMailAsync(mensaje); //++ //aqui hace el envio del correo electronico


                    resultado = true;
                }
                catch (Exception ex)
                {
                    _logger.Log(LogLevel.Error, ex.Message);

                    cliente.Dispose();
                    resultado = false;
                }
            }
            //await Task.CompletedTask;

            return resultado;
        }
        public async Task<bool> EnviarCorreoEspecialEnvio(ResumenEnvioDocumentoCorreoViewModel modelo, string plantilla)
        {
            var configuracion = await _configuracionService.ObtenerInfoConfiguracionEmailAsync();
            bool resultado = false;
            //--
            using (var cliente = new SmtpClient(configuracion.HER_ServidorSMTP, configuracion.HER_PuertoServidorSMTP))
            {
                try
                {
                    cliente.EnableSsl = true;
                    cliente.DeliveryMethod = SmtpDeliveryMethod.Network;
                    cliente.Timeout = 100;
                    cliente.UseDefaultCredentials = false;
                    cliente.Credentials = new System.Net.NetworkCredential(configuracion.HER_CorreoNotificador, configuracion.HER_ContraseniaCorreoNotificador);
                    //--

                    foreach (var para in modelo.Origen_UsuarioPara)
                    {
                        if (para.EstaActivaNotificacion)
                        {
                            using (var mensaje = new MailMessage())
                            {
                                mensaje.From = new MailAddress(configuracion.HER_CorreoNotificador);
                                mensaje.Subject = string.Format("[HERMES v2] {0}", modelo.Asunto);

                                //Se agrega el tag <imagen>
                                var cuerpo = modelo.Cuerpo;
                                var htmlDoc = new HtmlDocument();
                                htmlDoc.LoadHtml(cuerpo);

                                string imagen = "<img";
                                bool contieneImg = cuerpo.Contains(imagen);

                                if (contieneImg)
                                {
                                    foreach (var node in htmlDoc.DocumentNode.SelectNodes("//img"))
                                    {
                                        node.ParentNode.ReplaceChild(HtmlTextNode.CreateNode(node.InnerText + "Imagen"), node);
                                    }
                                }
                                var NuevoCuerpo = htmlDoc.DocumentNode.WriteTo();

                                mensaje.Body = string.Format(plantilla,
                                    modelo.Origen_UsuarioCreador,
                                    modelo.Folio,
                                    modelo.TipoDocumento,
                                    string.Format("{0} ({1})", modelo.Origen_UsuarioDe.NombreCompleto, modelo.Origen_UsuarioDe.Correo),
                                    string.Join(", ", modelo.Origen_UsuarioPara.Select(x => string.Format("{0} ({1})", x.NombreCompleto, x.Correo))),
                                    string.Join(", ", modelo.Origen_UsuarioCcp.Select(x => string.Format("{0} ({1})", x.NombreCompleto, x.Correo))),
                                    modelo.Asunto,
                                    para.NombreCompleto.ToUpper(),
                                    para.Puesto.ToUpper(),
                                    para.Area.ToUpper(),
                                    "P R E S E N T E",
                                    //modelo.Cuerpo,
                                    NuevoCuerpo,
                                    modelo.Origen_UsuarioDe.NombreCompleto,
                                    modelo.Origen_UsuarioDe.Puesto,
                                    modelo.Origen_UsuarioDe.Area);

                                mensaje.BodyEncoding = System.Text.Encoding.UTF8;
                                mensaje.SubjectEncoding = System.Text.Encoding.UTF8;
                                mensaje.HeadersEncoding = System.Text.Encoding.UTF8;
                                mensaje.IsBodyHtml = true;
                                //--
                                mensaje.To.Add(para.Correo);

                                if (modelo.Origen_Anexos.Count > 0)
                                {
                                    Attachment anexo;
                                    foreach (var a in modelo.Origen_Anexos)
                                    {
                                        anexo = new Attachment(a, MediaTypeNames.Application.Octet);

                                        mensaje.Attachments.Add(anexo);
                                    }
                                }
                                //--
                               //await cliente.SendMailAsync(mensaje); //++ //aqui se manda correo electronico (falta deshabilitar)

                            }
                        }
                    }
                    //--
                    foreach (var ccp in modelo.Origen_UsuarioCcp)
                    {
                        if (ccp.EstaActivaNotificacion)
                        {
                            using (var mensaje = new MailMessage())
                            {
                                mensaje.From = new MailAddress(configuracion.HER_CorreoNotificador);
                                mensaje.Subject = string.Format("[HERMES v2] {0}", modelo.Asunto);

                                //Se agrega el tag <imagen>
                                var cuerpo = modelo.Cuerpo;
                                var htmlDoc = new HtmlDocument();
                                htmlDoc.LoadHtml(cuerpo);

                                string imagen = "<img";
                                bool contieneImg = cuerpo.Contains(imagen);

                                if (contieneImg)
                                {
                                    foreach (var node in htmlDoc.DocumentNode.SelectNodes("//img"))
                                    {
                                        node.ParentNode.ReplaceChild(HtmlTextNode.CreateNode(node.InnerText + "Imagen"), node);
                                    }
                                }
                                var NuevoCuerpo = htmlDoc.DocumentNode.WriteTo();

                                mensaje.Body = string.Format(plantilla,
                                    modelo.Origen_UsuarioCreador,
                                    modelo.Folio,
                                    modelo.TipoDocumento,
                                    string.Format("{0} ({1})", modelo.Origen_UsuarioDe.NombreCompleto, modelo.Origen_UsuarioDe.Correo),
                                    string.Join(", ", modelo.Origen_UsuarioPara.Select(x => string.Format("{0} ({1})", x.NombreCompleto, x.Correo))),
                                    string.Join(", ", modelo.Origen_UsuarioCcp.Select(x => string.Format("{0} ({1})", x.NombreCompleto, x.Correo))),
                                    modelo.Asunto,
                                    ccp.NombreCompleto.ToUpper(),
                                    ccp.Puesto.ToUpper(),
                                    ccp.Area.ToUpper(),
                                    "\"PARA SU CONOCIMIENTO\"",
                                    //modelo.Cuerpo,
                                    NuevoCuerpo,
                                    modelo.Origen_UsuarioDe.NombreCompleto,
                                    modelo.Origen_UsuarioDe.Puesto,
                                    modelo.Origen_UsuarioDe.Area);

                                mensaje.BodyEncoding = System.Text.Encoding.UTF8;
                                mensaje.SubjectEncoding = System.Text.Encoding.UTF8;
                                mensaje.HeadersEncoding = System.Text.Encoding.UTF8;
                                mensaje.IsBodyHtml = true;
                                //--
                                mensaje.To.Add(ccp.Correo);

                                //--
                                if (modelo.Origen_Anexos.Count > 0)
                                {
                                    Attachment anexo;
                                    foreach (var a in modelo.Origen_Anexos)
                                    {
                                        anexo = new Attachment(a, MediaTypeNames.Application.Octet);

                                        mensaje.Attachments.Add(anexo);
                                    }
                                }
                                //--
                               //await cliente.SendMailAsync(mensaje); //++ //aqui tambien se hace la accion de mandar correo

                            }
                        }
                    }

                    resultado = true;
                }
                catch (Exception ex)
                {
                    _logger.Log(LogLevel.Error, ex.Message);
                    cliente.Dispose();
                    resultado = false;
                }
            }
            //await Task.CompletedTask;

            return resultado;
        }
        public async Task<bool> EnviarCorreoEspecialTurnar(ResumenTurnarDocumentoCorreoViewModel modelo, string plantilla)
        {
            var configuracion = await _configuracionService.ObtenerInfoConfiguracionEmailAsync();
            bool resultado = false;
            //--
            using (var cliente = new SmtpClient(configuracion.HER_ServidorSMTP, configuracion.HER_PuertoServidorSMTP))
            {
                try
                {
                    cliente.EnableSsl = true;
                    cliente.DeliveryMethod = SmtpDeliveryMethod.Network;
                    cliente.Timeout = 100;
                    cliente.UseDefaultCredentials = false;
                    cliente.Credentials = new System.Net.NetworkCredential(configuracion.HER_CorreoNotificador, configuracion.HER_ContraseniaCorreoNotificador);
                    //--
                    foreach (var actualPara in modelo.Actual_UsuarioPara)
                    {
                        if (actualPara.EstaActivaNotificacion)
                        {
                            using (var mensaje = new MailMessage())
                            {
                                mensaje.From = new MailAddress(configuracion.HER_CorreoNotificador);
                                mensaje.Subject = string.Format("[HERMES v2] {0}", modelo.Actual_AsuntoEnvio); //asunto

                                //Se agrega el tag <imagen>
                                var cuerpo = modelo.Cuerpo;
                                var htmlDoc = new HtmlDocument();
                                htmlDoc.LoadHtml(cuerpo);

                                string imagen = "<img";
                                bool contieneImg = cuerpo.Contains(imagen);

                                if (contieneImg)
                                {
                                    foreach (var node in htmlDoc.DocumentNode.SelectNodes("//img"))
                                    {
                                        node.ParentNode.ReplaceChild(HtmlTextNode.CreateNode(node.InnerText + "Imagen"), node);
                                    }
                                }
                                var NuevoCuerpo = htmlDoc.DocumentNode.WriteTo();

                                mensaje.Body = string.Format(plantilla,
                                    modelo.Origen_UsuarioCreador,
                                    string.Format("{0} ({1})", modelo.Actual_UsuarioDe.NombreCompleto, modelo.Actual_UsuarioDe.Correo),
                                    string.Join(", ", modelo.Actual_UsuarioPara.Select(x => string.Format("{0} ({1})", x.NombreCompleto, x.Correo))),
                                    string.Join(", ", modelo.Actual_UsuarioCcp.Select(x => string.Format("{0} ({1})", x.NombreCompleto, x.Correo))),
                                    modelo.Actual_Indicaciones,
                                    modelo.Folio,
                                    modelo.TipoDocumento,
                                    string.Format("{0} ({1})", modelo.Origen_UsuarioDe.NombreCompleto, modelo.Origen_UsuarioDe.Correo),
                                    string.Join(", ", modelo.Origen_UsuarioPara.Select(x => string.Format("{0} ({1})", x.NombreCompleto, x.Correo))),
                                    string.Join(", ", modelo.Origen_UsuarioCcp.Select(x => string.Format("{0} ({1})", x.NombreCompleto, x.Correo))),
                                    modelo.Asunto,
                                    string.Join(", ", modelo.Origen_Anexos),
                                    modelo.Origen_UsuarioParaBase.NombreCompleto.ToUpper(),
                                    modelo.Origen_UsuarioParaBase.Puesto.ToUpper(),
                                    modelo.Origen_UsuarioParaBase.Area.ToUpper(),
                                    "P R E S E N T E",
                                    //modelo.Cuerpo,
                                    NuevoCuerpo,
                                    modelo.Origen_UsuarioDe.NombreCompleto,
                                    modelo.Origen_UsuarioDe.Puesto,
                                    modelo.Origen_UsuarioDe.Area);

                                mensaje.BodyEncoding = System.Text.Encoding.UTF8;
                                //mensaje.SubjectEncoding = System.Text.Encoding.UTF8;
                                mensaje.HeadersEncoding = System.Text.Encoding.UTF8;
                                mensaje.IsBodyHtml = true;
                                //--
                                mensaje.To.Add(actualPara.Correo);

                                //--
                                if (modelo.Actual_Anexos.Count > 0)
                                {
                                    Attachment anexo;
                                    foreach (var a in modelo.Actual_Anexos)
                                    {
                                        anexo = new Attachment(a, MediaTypeNames.Application.Octet);

                                        mensaje.Attachments.Add(anexo);
                                    }
                                }
                                //--
                               //await cliente.SendMailAsync(mensaje); //++                               
                            }
                        }
                    }
                    //--
                    foreach (var actualPara in modelo.Actual_UsuarioCcp)
                    {
                        if (actualPara.EstaActivaNotificacion)
                        {
                            using (var mensaje = new MailMessage())
                            {
                                mensaje.From = new MailAddress(configuracion.HER_CorreoNotificador);
                                mensaje.Subject = string.Format("[HERMES v2] {0}", modelo.Actual_AsuntoEnvio); //asunto

                                //Se agrega el tag <imagen>
                                var cuerpo = modelo.Cuerpo;
                                var htmlDoc = new HtmlDocument();
                                htmlDoc.LoadHtml(cuerpo);

                                string imagen = "<img";
                                bool contieneImg = cuerpo.Contains(imagen);

                                if (contieneImg)
                                {
                                    foreach (var node in htmlDoc.DocumentNode.SelectNodes("//img"))
                                    {
                                        node.ParentNode.ReplaceChild(HtmlTextNode.CreateNode(node.InnerText + "Imagen"), node);
                                    }
                                }
                                var NuevoCuerpo = htmlDoc.DocumentNode.WriteTo();

                                mensaje.Body = string.Format(plantilla,
                                    modelo.Origen_UsuarioCreador,
                                    string.Format("{0} ({1})", modelo.Actual_UsuarioDe.NombreCompleto, modelo.Actual_UsuarioDe.Correo),
                                    string.Join(", ", modelo.Actual_UsuarioPara.Select(x => string.Format("{0} ({1})", x.NombreCompleto, x.Correo))),
                                    string.Join(", ", modelo.Actual_UsuarioCcp.Select(x => string.Format("{0} ({1})", x.NombreCompleto, x.Correo))),
                                    modelo.Actual_Indicaciones,
                                    modelo.Folio,
                                    modelo.TipoDocumento,
                                    string.Format("{0} ({1})", modelo.Origen_UsuarioDe.NombreCompleto, modelo.Origen_UsuarioDe.Correo),
                                    string.Join(", ", modelo.Origen_UsuarioPara.Select(x => string.Format("{0} ({1})", x.NombreCompleto, x.Correo))),
                                    string.Join(", ", modelo.Origen_UsuarioCcp.Select(x => string.Format("{0} ({1})", x.NombreCompleto, x.Correo))),
                                    modelo.Asunto,
                                    string.Join(", ", modelo.Origen_Anexos),
                                    modelo.Origen_UsuarioParaBase.NombreCompleto.ToUpper(),
                                    modelo.Origen_UsuarioParaBase.Puesto.ToUpper(),
                                    modelo.Origen_UsuarioParaBase.Area.ToUpper(),
                                    "\"PARA SU CONOCIMIENTO\"",
                                    //modelo.Cuerpo,
                                    NuevoCuerpo,
                                    modelo.Origen_UsuarioDe.NombreCompleto,
                                    modelo.Origen_UsuarioDe.Puesto,
                                    modelo.Origen_UsuarioDe.Area);

                                mensaje.BodyEncoding = System.Text.Encoding.UTF8;
                                //mensaje.SubjectEncoding = System.Text.Encoding.UTF8;
                                mensaje.HeadersEncoding = System.Text.Encoding.UTF8;
                                mensaje.IsBodyHtml = true;
                                //--
                                mensaje.To.Add(actualPara.Correo);

                                if (modelo.Actual_Anexos.Count > 0)
                                {
                                    Attachment anexo;
                                    foreach (var a in modelo.Actual_Anexos)
                                    {
                                        anexo = new Attachment(a, MediaTypeNames.Application.Octet);

                                        mensaje.Attachments.Add(anexo);
                                    }
                                }
                                //--
                               //await cliente.SendMailAsync(mensaje); //++

                            }
                        }
                    }

                    resultado = true;
                }
                catch (Exception ex)
                {
                    _logger.Log(LogLevel.Error, ex.Message);
                    cliente.Dispose();
                    resultado = false;
                }
            }
            //await Task.CompletedTask;

            return resultado;
        }
        public async Task<bool> EnviarCorreoEspecialTurnarEspecial(ResumenTurnarEspecialDocumentoCorreoViewModel modelo, string plantilla)
        {
            var configuracion = await _configuracionService.ObtenerInfoConfiguracionEmailAsync();
            bool resultado = false;
            //--
            using (var cliente = new SmtpClient(configuracion.HER_ServidorSMTP, configuracion.HER_PuertoServidorSMTP))
            {
                try
                {
                    cliente.EnableSsl = true;
                    cliente.DeliveryMethod = SmtpDeliveryMethod.Network;
                    cliente.Timeout = 100;
                    cliente.UseDefaultCredentials = false;
                    cliente.Credentials = new System.Net.NetworkCredential(configuracion.HER_CorreoNotificador, configuracion.HER_ContraseniaCorreoNotificador);
                    //--
                    foreach (var actualPara in modelo.Actual_UsuarioPara)
                    {
                        if (actualPara.EstaActivaNotificacion)
                        {
                            using (var mensaje = new MailMessage())
                            {
                                mensaje.From = new MailAddress(configuracion.HER_CorreoNotificador);
                                mensaje.Subject = string.Format("[HERMES v2] {0}", modelo.Actual_AsuntoEnvio); //asunto

                                //Se agrega el tag <imagen>
                                var cuerpo = modelo.Cuerpo;
                                var htmlDoc = new HtmlDocument();
                                htmlDoc.LoadHtml(cuerpo);

                                string imagen = "<img";
                                bool contieneImg = cuerpo.Contains(imagen);

                                if (contieneImg)
                                {
                                    foreach (var node in htmlDoc.DocumentNode.SelectNodes("//img"))
                                    {
                                        node.ParentNode.ReplaceChild(HtmlTextNode.CreateNode(node.InnerText + "Imagen"), node);
                                    }
                                }
                                var NuevoCuerpo = htmlDoc.DocumentNode.WriteTo();

                                mensaje.Body = string.Format(plantilla,
                                    modelo.Origen_UsuarioCreador,
                                    string.Format("{0} ({1})", modelo.Actual_UsuarioDe.NombreCompleto, modelo.Actual_UsuarioDe.Correo),
                                    string.Join(", ", modelo.Actual_UsuarioPara.Select(x => string.Format("{0} ({1})", x.NombreCompleto, x.Correo))),
                                    string.Join(", ", modelo.Actual_UsuarioCcp.Select(x => string.Format("{0} ({1})", x.NombreCompleto, x.Correo))),
                                    modelo.Actual_Indicaciones,
                                    modelo.Folio,
                                    modelo.TipoDocumento,
                                    string.Format("{0} ({1})", modelo.Origen_UsuarioDe.NombreCompleto, modelo.Origen_UsuarioDe.Correo),
                                    string.Join(", ", modelo.Origen_UsuarioPara.Select(x => string.Format("{0} ({1})", x.NombreCompleto, x.Correo))),
                                    string.Join(", ", modelo.Origen_UsuarioCcp.Select(x => string.Format("{0} ({1})", x.NombreCompleto, x.Correo))),
                                    modelo.Asunto,
                                    string.Join(", ", modelo.Origen_Anexos),
                                    modelo.Origen_UsuarioParaBase.NombreCompleto.ToUpper(),
                                    modelo.Origen_UsuarioParaBase.Puesto.ToUpper(),
                                    modelo.Origen_UsuarioParaBase.Area.ToUpper(),
                                    "P R E S E N T E",
                                    //modelo.Cuerpo,
                                    NuevoCuerpo,
                                    modelo.Origen_UsuarioDe.NombreCompleto,
                                    modelo.Origen_UsuarioDe.Puesto,
                                    modelo.Origen_UsuarioDe.Area);

                                mensaje.BodyEncoding = System.Text.Encoding.UTF8;
                                //mensaje.SubjectEncoding = System.Text.Encoding.UTF8;
                                mensaje.HeadersEncoding = System.Text.Encoding.UTF8;
                                mensaje.IsBodyHtml = true;
                                //--
                                mensaje.To.Add(actualPara.Correo);

                                //--
                                if (modelo.Actual_Anexos.Count > 0)
                                {
                                    Attachment anexo;
                                    foreach (var a in modelo.Actual_Anexos)
                                    {
                                        anexo = new Attachment(a, MediaTypeNames.Application.Octet);

                                        mensaje.Attachments.Add(anexo);
                                    }
                                }
                                //--
                                //await cliente.SendMailAsync(mensaje); //++                               
                            }
                        }
                    }
                    //--
                    foreach (var actualPara in modelo.Actual_UsuarioCcp)
                    {
                        if (actualPara.EstaActivaNotificacion)
                        {
                            using (var mensaje = new MailMessage())
                            {
                                mensaje.From = new MailAddress(configuracion.HER_CorreoNotificador);
                                mensaje.Subject = string.Format("[HERMES v2] {0}", modelo.Actual_AsuntoEnvio); //asunto

                                //Se agrega el tag <imagen>
                                var cuerpo = modelo.Cuerpo;
                                var htmlDoc = new HtmlDocument();
                                htmlDoc.LoadHtml(cuerpo);

                                string imagen = "<img";
                                bool contieneImg = cuerpo.Contains(imagen);

                                if (contieneImg)
                                {
                                    foreach (var node in htmlDoc.DocumentNode.SelectNodes("//img"))
                                    {
                                        node.ParentNode.ReplaceChild(HtmlTextNode.CreateNode(node.InnerText + "Imagen"), node);
                                    }
                                }
                                var NuevoCuerpo = htmlDoc.DocumentNode.WriteTo();

                                mensaje.Body = string.Format(plantilla,
                                    modelo.Origen_UsuarioCreador,
                                    string.Format("{0} ({1})", modelo.Actual_UsuarioDe.NombreCompleto, modelo.Actual_UsuarioDe.Correo),
                                    string.Join(", ", modelo.Actual_UsuarioPara.Select(x => string.Format("{0} ({1})", x.NombreCompleto, x.Correo))),
                                    string.Join(", ", modelo.Actual_UsuarioCcp.Select(x => string.Format("{0} ({1})", x.NombreCompleto, x.Correo))),
                                    modelo.Actual_Indicaciones,
                                    modelo.Folio,
                                    modelo.TipoDocumento,
                                    string.Format("{0} ({1})", modelo.Origen_UsuarioDe.NombreCompleto, modelo.Origen_UsuarioDe.Correo),
                                    string.Join(", ", modelo.Origen_UsuarioPara.Select(x => string.Format("{0} ({1})", x.NombreCompleto, x.Correo))),
                                    string.Join(", ", modelo.Origen_UsuarioCcp.Select(x => string.Format("{0} ({1})", x.NombreCompleto, x.Correo))),
                                    modelo.Asunto,
                                    string.Join(", ", modelo.Origen_Anexos),
                                    modelo.Origen_UsuarioParaBase.NombreCompleto.ToUpper(),
                                    modelo.Origen_UsuarioParaBase.Puesto.ToUpper(),
                                    modelo.Origen_UsuarioParaBase.Area.ToUpper(),
                                    "\"PARA SU CONOCIMIENTO\"",
                                    //modelo.Cuerpo,
                                    NuevoCuerpo,
                                    modelo.Origen_UsuarioDe.NombreCompleto,
                                    modelo.Origen_UsuarioDe.Puesto,
                                    modelo.Origen_UsuarioDe.Area);

                                mensaje.BodyEncoding = System.Text.Encoding.UTF8;
                                //mensaje.SubjectEncoding = System.Text.Encoding.UTF8;
                                mensaje.HeadersEncoding = System.Text.Encoding.UTF8;
                                mensaje.IsBodyHtml = true;
                                //--
                                mensaje.To.Add(actualPara.Correo);

                                if (modelo.Actual_Anexos.Count > 0)
                                {
                                    Attachment anexo;
                                    foreach (var a in modelo.Actual_Anexos)
                                    {
                                        anexo = new Attachment(a, MediaTypeNames.Application.Octet);

                                        mensaje.Attachments.Add(anexo);
                                    }
                                }
                                //--
                                //await cliente.SendMailAsync(mensaje); //++

                            }
                        }
                    }

                    resultado = true;
                }
                catch (Exception ex)
                {
                    _logger.Log(LogLevel.Error, ex.Message);
                    cliente.Dispose();
                    resultado = false;
                }
            }
            //await Task.CompletedTask;

            return resultado;
        }
        public async Task<bool> EnviarCorreoEspecialResponder(ResumenResponderDocumentoCorreoViewModel modelo, string plantilla)
        {
            var configuracion = await _configuracionService.ObtenerInfoConfiguracionEmailAsync();
            bool resultado = false;
            //--
            using (var cliente = new SmtpClient(configuracion.HER_ServidorSMTP, configuracion.HER_PuertoServidorSMTP))
            {
                try
                {
                    cliente.EnableSsl = true;
                    cliente.DeliveryMethod = SmtpDeliveryMethod.Network;
                    cliente.Timeout = 100;
                    cliente.UseDefaultCredentials = false;
                    cliente.Credentials = new System.Net.NetworkCredential(configuracion.HER_CorreoNotificador, configuracion.HER_ContraseniaCorreoNotificador);
                    //--

                    foreach (var actualPara in modelo.Origen_UsuarioPara)
                    {
                        if (actualPara.EstaActivaNotificacion)
                        {
                            using (var mensaje = new MailMessage())
                            {
                                mensaje.From = new MailAddress(configuracion.HER_CorreoNotificador);
                                mensaje.Subject = string.Format("[HERMES v2] {0}", modelo.Asunto); //asunto

                                //Se agrega el tag <imagen>
                                var cuerpo = modelo.Cuerpo;
                                var htmlDoc = new HtmlDocument();
                                htmlDoc.LoadHtml(cuerpo);

                                string imagen = "<img";
                                bool contieneImg = cuerpo.Contains(imagen);

                                if (contieneImg)
                                {
                                    foreach (var node in htmlDoc.DocumentNode.SelectNodes("//img"))
                                    {
                                        node.ParentNode.ReplaceChild(HtmlTextNode.CreateNode(node.InnerText + "Imagen"), node);
                                    }
                                }
                                var NuevoCuerpo = htmlDoc.DocumentNode.WriteTo();

                                mensaje.Body = string.Format(plantilla,
                                    modelo.Origen_UsuarioCreador,
                                    modelo.Folio,
                                    modelo.TipoDocumento,
                                    string.Format("{0} ({1})", modelo.Origen_UsuarioDe.NombreCompleto, modelo.Origen_UsuarioDe.Correo),
                                    string.Join(", ", modelo.Origen_UsuarioPara.Select(x => string.Format("{0} ({1})", x.NombreCompleto, x.Correo))),
                                    string.Join(", ", modelo.Origen_UsuarioCcp.Select(x => string.Format("{0} ({1})", x.NombreCompleto, x.Correo))),
                                    modelo.Asunto,
                                    actualPara.NombreCompleto.ToUpper(),
                                    actualPara.Puesto.ToUpper(),
                                    actualPara.Area.ToUpper(),
                                    "P R E S E N T E",
                                    //modelo.Cuerpo,
                                    NuevoCuerpo,
                                    modelo.Origen_UsuarioDe.NombreCompleto,
                                    modelo.Origen_UsuarioDe.Puesto,
                                    modelo.Origen_UsuarioDe.Area);

                                mensaje.BodyEncoding = System.Text.Encoding.UTF8;
                                //mensaje.SubjectEncoding = System.Text.Encoding.UTF8;
                                mensaje.HeadersEncoding = System.Text.Encoding.UTF8;
                                mensaje.IsBodyHtml = true;
                                //--
                                mensaje.To.Add(actualPara.Correo);

                                if (modelo.Origen_Anexos.Count > 0)
                                {
                                    Attachment anexo;
                                    foreach (var a in modelo.Origen_Anexos)
                                    {
                                        anexo = new Attachment(a, MediaTypeNames.Application.Octet);

                                        mensaje.Attachments.Add(anexo);
                                    }
                                }
                                //--
                               //await cliente.SendMailAsync(mensaje); //++

                            }
                        }
                    }

                    foreach (var actualPara in modelo.Origen_UsuarioCcp)
                    {
                        if (actualPara.EstaActivaNotificacion)
                        {
                            using (var mensaje = new MailMessage())
                            {
                                mensaje.From = new MailAddress(configuracion.HER_CorreoNotificador);
                                mensaje.Subject = string.Format("[HERMES v2] {0}", modelo.Asunto); //asunto

                                //Se agrega el tag <imagen>
                                var cuerpo = modelo.Cuerpo;
                                var htmlDoc = new HtmlDocument();
                                htmlDoc.LoadHtml(cuerpo);

                                string imagen = "<img";
                                bool contieneImg = cuerpo.Contains(imagen);

                                if (contieneImg)
                                {
                                    foreach (var node in htmlDoc.DocumentNode.SelectNodes("//img"))
                                    {
                                        node.ParentNode.ReplaceChild(HtmlTextNode.CreateNode(node.InnerText + "Imagen"), node);
                                    }
                                }
                                var NuevoCuerpo = htmlDoc.DocumentNode.WriteTo();

                                mensaje.Body = string.Format(plantilla,
                                    modelo.Origen_UsuarioCreador,
                                    modelo.Folio,
                                    modelo.TipoDocumento,
                                    string.Format("{0} ({1})", modelo.Origen_UsuarioDe.NombreCompleto, modelo.Origen_UsuarioDe.Correo),
                                    string.Join(", ", modelo.Origen_UsuarioPara.Select(x => string.Format("{0} ({1})", x.NombreCompleto, x.Correo))),
                                    string.Join(", ", modelo.Origen_UsuarioCcp.Select(x => string.Format("{0} ({1})", x.NombreCompleto, x.Correo))),
                                    modelo.Asunto,
                                    actualPara.NombreCompleto.ToUpper(),
                                    actualPara.Puesto.ToUpper(),
                                    actualPara.Area.ToUpper(),
                                    "\"PARA SU CONOCIMIENTO\"",
                                    //modelo.Cuerpo,
                                    NuevoCuerpo,
                                    modelo.Origen_UsuarioDe.NombreCompleto,
                                    modelo.Origen_UsuarioDe.Puesto,
                                    modelo.Origen_UsuarioDe.Area);

                                mensaje.BodyEncoding = System.Text.Encoding.UTF8;
                                //mensaje.SubjectEncoding = System.Text.Encoding.UTF8;
                                mensaje.HeadersEncoding = System.Text.Encoding.UTF8;
                                mensaje.IsBodyHtml = true;
                                //--
                                mensaje.To.Add(actualPara.Correo);

                                if (modelo.Origen_Anexos.Count > 0)
                                {
                                    Attachment anexo;
                                    foreach (var a in modelo.Origen_Anexos)
                                    {
                                        anexo = new Attachment(a, MediaTypeNames.Application.Octet);

                                        mensaje.Attachments.Add(anexo);
                                    }
                                }
                                //--
                               //await cliente.SendMailAsync(mensaje); //++

                            }
                        }
                    }

                    resultado = true;
                }
                catch (Exception ex)
                {
                    _logger.Log(LogLevel.Error, ex.Message);
                    cliente.Dispose();
                    resultado = false;
                }
            }
            //await Task.CompletedTask;

            return resultado;
        }
        public async Task EnviarCorreoEspecialLectura(string usuario, string correo, bool archivos, ResumenEnvioDocumentoCorreoViewModel modelo)
        {
            var configuracion = await _configuracionService.ObtenerInfoConfiguracionEmailAsync();
            string correoU = usuario + "@uv.mx";

            bool existe1 = modelo.Origen_UsuarioDe.Correo == correoU;
            bool existe2 = modelo.Origen_UsuarioPara.Where(x => x.Correo == correoU).Any();
            bool existe3 = modelo.Origen_UsuarioCcp.Where(x => x.Correo == correoU).Any();

            using (var cliente = new SmtpClient(configuracion.HER_ServidorSMTP, configuracion.HER_PuertoServidorSMTP))
            {
                cliente.EnableSsl = true;
                cliente.DeliveryMethod = SmtpDeliveryMethod.Network;
                cliente.Timeout = 100;
                cliente.UseDefaultCredentials = false;
                cliente.Credentials = new System.Net.NetworkCredential(configuracion.HER_CorreoNotificador, configuracion.HER_ContraseniaCorreoNotificador);

                if (existe1)
                {

                    var plantilla =
                   $"<div style=\"font-family:Segoe UI; font-size: 13px; \">" +
                       "<p style=\"font-weight: bold; margin-bottom:0; \"> Sistema de Administración y Seguimiento de Correspondencia (HERMES)</p>" +
                   "</div>" +
                   "<div style=\"font-family:Segoe UI; font-size: 12px; \">" +
                       "<p>Ha recibido un documento desde la cuenta <strong>{0}</strong> con la siguiente información: </p>" +
                       "<p style=\"margin: 0; \"><strong>No.: </strong> {1} </p>" +
                       "<p style=\"margin: 0; \"><strong>Tipo de documento: </strong> {2} </p>" +
                       "<p style=\"margin: 0; \"><strong>De: </strong> {3} </p>" +
                       "<p style=\"margin: 0; \"><strong>Para: </strong> {4} </p>" +
                       "<p style=\"margin: 0; \"><strong>CCP: </strong> {5} </p>" +
                       "<p style=\"margin: 0; \"><strong>Asunto: </strong> {6} </p>" +
                       "<div style=\"border: 0.05em solid #000; margin-top: 1em; padding: 0.7em;\">" +
                           "<div style=\"padding: 1em 1em 1em 0; \">" +
                               "{7}" +
                           "</div>" +
                           "<p style=\"margin: 0; \">A t e n t a m e n t e</p>" +
                           "<p style=\"margin: 0; \">\"Lis de Veracruz: Arte, Ciencia, Luz\"</p>" +
                           "<br />" +
                           "<p style=\"margin: 0; \">{8}</p> " +
                           "<p style=\"margin: 0; \">{9}</p> " +
                           "<p style=\"margin: 0; \">{10}</p>" +
                           "<p style=\"margin: 0; \">Universidad Veracruzana</p>" +
                       "</div>" +
                   "</div>" +
                   "<div style=\"font-family:Segoe UI; font-size: 10px; \">" +
                       "<p style=\"margin-top: 0.5em; margin-bottom: 0.5em; \">-----------------------------------------------------------------</p>" +
                       "<p style=\"margin-top: 0.5em; margin-bottom: 0em; \">Mensaje enviado automáticamente por el Sistema de Administración y Seguimiento de Correspondencia (HERMES) - <a href=\"https://hermes.uv.mx/\" target=\"_blank\">https://hermes.uv.mx/</a></p>" +
                       "<p style=\"margin: 0; \"><strong>Favor de no responder al remitente de este mensaje.</strong></p>" +
                   "</div>";

                    //Se agrega el tag <imagen>

                    var cuerpo = "";
                    if ((modelo.TipoEnvioId == ConstTipoEnvio.TipoEnvioN2) || (modelo.TipoEnvioId == ConstTipoEnvio.TipoEnvioN5) || (modelo.TipoEnvioId == ConstTipoEnvio.TipoEnvioN1 && modelo.EsReenvio && !string.IsNullOrEmpty(modelo.Indicaciones))) 
                    {
                        cuerpo = modelo.Indicaciones;
                    }
                    else
                    {
                        cuerpo = modelo.Cuerpo;
                    }

                    //= modelo.Cuerpo;
                    var htmlDoc = new HtmlDocument();
                    htmlDoc.LoadHtml(cuerpo);

                    string imagen = "<img";
                    bool contieneImg = cuerpo.Contains(imagen);

                    if (contieneImg)
                    {
                        foreach (var node in htmlDoc.DocumentNode.SelectNodes("//img"))
                        {
                            node.ParentNode.ReplaceChild(HtmlTextNode.CreateNode(node.InnerText + "Imagen"), node);
                        }
                    }
                    var NuevoCuerpo = htmlDoc.DocumentNode.WriteTo();

                    var body = string.Format(plantilla,
                                modelo.Origen_UsuarioCreador,
                                modelo.Folio,
                                modelo.TipoDocumento,
                                string.Format("{0} ({1})", modelo.Origen_UsuarioDe.NombreCompleto, modelo.Origen_UsuarioDe.Correo),
                                string.Join(", ", modelo.Origen_UsuarioPara.Select(x => string.Format("{0} ({1})", x.NombreCompleto, x.Correo))),
                                string.Join(", ", modelo.Origen_UsuarioCcp.Select(x => string.Format("{0} ({1})", x.NombreCompleto, x.Correo))),
                                modelo.Asunto,
                                //modelo.Cuerpo,
                                NuevoCuerpo,
                                modelo.Origen_UsuarioDe.NombreCompleto,
                                modelo.Origen_UsuarioDe.Puesto,
                                modelo.Origen_UsuarioDe.Area);

                    using (var mensaje = new MailMessage())
                    {
                        mensaje.From = new MailAddress(configuracion.HER_CorreoNotificador);
                        mensaje.Subject = string.Format("[HERMES v2] {0}", modelo.Asunto);
                        mensaje.Body = body;

                        mensaje.BodyEncoding = System.Text.Encoding.UTF8;
                        //mensaje.SubjectEncoding = System.Text.Encoding.UTF8;
                        mensaje.HeadersEncoding = System.Text.Encoding.UTF8;
                        mensaje.IsBodyHtml = true;
                        //--
                        mensaje.To.Add(correo);
                        if (archivos)
                        {
                            if (modelo.Origen_Anexos.Count > 0)
                            {
                                Attachment anexo;
                                foreach (var a in modelo.Origen_Anexos)
                                {
                                    anexo = new Attachment(Path.Combine(_environment.WebRootPath, a), MediaTypeNames.Application.Octet);

                                    mensaje.Attachments.Add(anexo);
                                }
                            }
                        }
                        //--
                       //await cliente.SendMailAsync(mensaje); //++

                    }
                }
                else if (existe2)
                {
                    var plantilla =
                   $"<div style=\"font-family:Segoe UI; font-size: 13px; \">" +
                       "<p style=\"font-weight: bold; margin-bottom:0; \"> Sistema de Administración y Seguimiento de Correspondencia (HERMES)</p>" +
                   "</div>" +
                   "<div style=\"font-family:Segoe UI; font-size: 12px; \">" +
                       "<p>Ha recibido un documento con la siguiente información: </p>" +
                       "<p style=\"margin: 0; \"><strong>No.: </strong> {0} </p>" +
                       "<p style=\"margin: 0; \"><strong>Tipo de documento: </strong> {1} </p>" +
                       "<p style=\"margin: 0; \"><strong>De: </strong> {2} </p>" +
                       "<p style=\"margin: 0; \"><strong>Para: </strong> {3} </p>" +
                       "<p style=\"margin: 0; \"><strong>CCP: </strong> {4} </p>" +
                       "<p style=\"margin: 0; \"><strong>Asunto: </strong> {5} </p>" +
                       "<div style=\"border: 0.05em solid #000; margin-top: 1em; padding: 0.7em;\">" +
                           "<p style=\"margin: 0; \"><strong>{6}</strong></p>" +
                           "<p style=\"margin: 0; \"><strong>{7}</strong></p>" +
                           "<p style=\"margin: 0; \"><strong>{8}</strong></p>" +
                           "<p style=\"margin: 0; \">{9}</p>" +
                           "<div style=\"padding: 1em 1em 1em 0; \">" +
                               "{10}" +
                           "</div>" +
                           "<p style=\"margin: 0; \">A t e n t a m e n t e</p>" +
                           "<p style=\"margin: 0; \">\"Lis de Veracruz: Arte, Ciencia, Luz\"</p>" +
                           "<br />" +
                           "<p style=\"margin: 0; \">{11}</p> " +
                           "<p style=\"margin: 0; \">{12}</p> " +
                           "<p style=\"margin: 0; \">{13}</p>" +
                           "<p style=\"margin: 0; \">Universidad Veracruzana</p>" +
                       "</div>" +
                   "</div>" +
                   "<div style=\"font-family:Segoe UI; font-size: 10px; \">" +
                       "<p style=\"margin-top: 0.5em; margin-bottom: 0.5em; \">-----------------------------------------------------------------</p>" +
                       "<p style=\"margin-top: 0.5em; margin-bottom: 0em; \">Mensaje enviado automáticamente por el Sistema de Administración y Seguimiento de Correspondencia (HERMES) - <a href=\"https://hermes.uv.mx/\" target=\"_blank\">https://hermes.uv.mx/</a></p>" +
                       "<p style=\"margin: 0; \"><strong>Favor de no responder al remitente de este mensaje.</strong></p>" +
                   "</div>";

                    //Se agrega el tag <imagen>
                    var cuerpo = "";
                    if ((modelo.TipoEnvioId == ConstTipoEnvio.TipoEnvioN2) || (modelo.TipoEnvioId == ConstTipoEnvio.TipoEnvioN5) || (modelo.TipoEnvioId == ConstTipoEnvio.TipoEnvioN1 && modelo.EsReenvio && !string.IsNullOrEmpty(modelo.Indicaciones)))
                    {
                        cuerpo = modelo.Indicaciones;
                    }
                    else
                    {
                        cuerpo = modelo.Cuerpo;
                    }
                    var htmlDoc = new HtmlDocument();
                    htmlDoc.LoadHtml(cuerpo);

                    string imagen = "<img";
                    bool contieneImg = cuerpo.Contains(imagen);

                    if (contieneImg)
                    {
                        foreach (var node in htmlDoc.DocumentNode.SelectNodes("//img"))
                        {
                            node.ParentNode.ReplaceChild(HtmlTextNode.CreateNode(node.InnerText + "Imagen"), node);
                        }
                    }
                    var NuevoCuerpo = htmlDoc.DocumentNode.WriteTo();

                    var info = modelo.Origen_UsuarioPara.Where(x => x.Correo == correoU).FirstOrDefault();
                    //var info = modelo.Origen_UsuarioPara.Where(x => x.Correo == correo).FirstOrDefault();

                    var body = string.Format(plantilla,
                            modelo.Folio,
                            modelo.TipoDocumento,
                            string.Format("{0} ({1})", modelo.Origen_UsuarioDe.NombreCompleto, modelo.Origen_UsuarioDe.Correo),
                            string.Join(", ", modelo.Origen_UsuarioPara.Select(x => string.Format("{0} ({1})", x.NombreCompleto, x.Correo))),
                            string.Join(", ", modelo.Origen_UsuarioCcp.Select(x => string.Format("{0} ({1})", x.NombreCompleto, x.Correo))),
                            modelo.Asunto,
                            info.NombreCompleto.ToUpper(),
                            info.Puesto.ToUpper(),
                            info.Area.ToUpper(),
                            "P R E S E N T E",
                            //modelo.Cuerpo,
                            NuevoCuerpo,
                            modelo.Origen_UsuarioDe.NombreCompleto,
                            modelo.Origen_UsuarioDe.Puesto,
                            modelo.Origen_UsuarioDe.Area);

                    using (var mensaje = new MailMessage())
                    {

                        mensaje.From = new MailAddress(configuracion.HER_CorreoNotificador);
                        mensaje.Subject = string.Format("[HERMES v2] {0}", modelo.Asunto);
                        mensaje.Body = body;

                        mensaje.BodyEncoding = System.Text.Encoding.UTF8;
                        //mensaje.SubjectEncoding = System.Text.Encoding.UTF8;
                        mensaje.HeadersEncoding = System.Text.Encoding.UTF8;
                        mensaje.IsBodyHtml = true;
                        //--
                        mensaje.To.Add(correo);

                        if (archivos)
                        {
                            if (modelo.Origen_Anexos.Count > 0)
                            {
                                Attachment anexo;
                                foreach (var a in modelo.Origen_Anexos)
                                {
                                    anexo = new Attachment(Path.Combine(_environment.WebRootPath, a), MediaTypeNames.Application.Octet);

                                    mensaje.Attachments.Add(anexo);
                                }
                            }
                        }
                        //--
                       //await cliente.SendMailAsync(mensaje); //++
                    }
                }
                else if (existe3)
                {
                    var plantilla =
                   $"<div style=\"font-family:Segoe UI; font-size: 13px; \">" +
                       "<p style=\"font-weight: bold; margin-bottom:0; \"> Sistema de Administración y Seguimiento de Correspondencia (HERMES)</p>" +
                   "</div>" +
                   "<div style=\"font-family:Segoe UI; font-size: 12px; \">" +
                       "<p>Ha recibido un documento con la siguiente información: </p>" +
                       "<p style=\"margin: 0; \"><strong>No.: </strong> {0} </p>" +
                       "<p style=\"margin: 0; \"><strong>Tipo de documento: </strong> {1} </p>" +
                       "<p style=\"margin: 0; \"><strong>De: </strong> {2} </p>" +
                       "<p style=\"margin: 0; \"><strong>Para: </strong> {3} </p>" +
                       "<p style=\"margin: 0; \"><strong>CCP: </strong> {4} </p>" +
                       "<p style=\"margin: 0; \"><strong>Asunto: </strong> {5} </p>" +
                       "<div style=\"border: 0.05em solid #000; margin-top: 1em; padding: 0.7em;\">" +
                           "<p style=\"margin: 0; \"><strong>{6}</strong></p>" +
                           "<p style=\"margin: 0; \"><strong>{7}</strong></p>" +
                           "<p style=\"margin: 0; \"><strong>{8}</strong></p>" +
                           "<p style=\"margin: 0; \">{9}</p>" +
                           "<p style=\"margin: 0; \"><i>{10}</i></p>" +
                           "<div style=\"padding: 1em 1em 1em 0; \">" +
                               "{11}" +
                           "</div>" +
                           "<p style=\"margin: 0; \">A t e n t a m e n t e</p>" +
                           "<p style=\"margin: 0; \">\"Lis de Veracruz: Arte, Ciencia, Luz\"</p>" +
                           "<br />" +
                           "<p style=\"margin: 0; \">{12}</p> " +
                           "<p style=\"margin: 0; \">{13}</p> " +
                           "<p style=\"margin: 0; \">{14}</p>" +
                           "<p style=\"margin: 0; \">Universidad Veracruzana</p>" +
                       "</div>" +
                   "</div>" +
                   "<div style=\"font-family:Segoe UI; font-size: 10px; \">" +
                       "<p style=\"margin-top: 0.5em; margin-bottom: 0.5em; \">-----------------------------------------------------------------</p>" +
                       "<p style=\"margin-top: 0.5em; margin-bottom: 0em; \">Mensaje enviado automáticamente por el Sistema de Administración y Seguimiento de Correspondencia (HERMES) - <a href=\"https://hermes.uv.mx/\" target=\"_blank\">https://hermes.uv.mx/</a></p>" +
                       "<p style=\"margin: 0; \"><strong>Favor de no responder al remitente de este mensaje.</strong></p>" +
                   "</div>";

                    //Se agrega el tag <imagen>
                    var cuerpo = "";
                    if ((modelo.TipoEnvioId == ConstTipoEnvio.TipoEnvioN2) || (modelo.TipoEnvioId == ConstTipoEnvio.TipoEnvioN5) || (modelo.TipoEnvioId == ConstTipoEnvio.TipoEnvioN1 && modelo.EsReenvio && !string.IsNullOrEmpty(modelo.Indicaciones)))
                    {
                        cuerpo = modelo.Indicaciones;
                    }
                    else
                    {
                        cuerpo = modelo.Cuerpo;
                    }
                    var htmlDoc = new HtmlDocument();
                    htmlDoc.LoadHtml(cuerpo);

                    string imagen = "<img";
                    bool contieneImg = cuerpo.Contains(imagen);

                    if (contieneImg)
                    {
                        foreach (var node in htmlDoc.DocumentNode.SelectNodes("//img"))
                        {
                            node.ParentNode.ReplaceChild(HtmlTextNode.CreateNode(node.InnerText + "Imagen"), node);
                        }
                    }
                    var NuevoCuerpo = htmlDoc.DocumentNode.WriteTo();

                    var info = modelo.Origen_UsuarioCcp.Where(x => x.Correo == correoU).FirstOrDefault();
                    //var info = modelo.Origen_UsuarioCcp.Where(x => x.Correo == correo).FirstOrDefault();

                    var body = string.Format(plantilla,
                            modelo.Folio,
                            modelo.TipoDocumento,
                            string.Format("{0} ({1})", modelo.Origen_UsuarioDe.NombreCompleto, modelo.Origen_UsuarioDe.Correo),
                            string.Join(", ", modelo.Origen_UsuarioPara.Select(x => string.Format("{0} ({1})", x.NombreCompleto, x.Correo))),
                            string.Join(", ", modelo.Origen_UsuarioCcp.Select(x => string.Format("{0} ({1})", x.NombreCompleto, x.Correo))),
                            modelo.Asunto,
                            info.NombreCompleto.ToUpper(),
                            info.Puesto.ToUpper(),
                            info.Area.ToUpper(),
                            "P R E S E N T E",
                            "Para su conocimiento",
                            NuevoCuerpo,
                            modelo.Origen_UsuarioDe.NombreCompleto,
                            modelo.Origen_UsuarioDe.Puesto,
                            modelo.Origen_UsuarioDe.Area);

                    using (var mensaje = new MailMessage())
                    {

                        mensaje.From = new MailAddress(configuracion.HER_CorreoNotificador);
                        mensaje.Subject = string.Format("[HERMES v2] {0}", modelo.Asunto);
                        mensaje.Body = body;

                        mensaje.BodyEncoding = System.Text.Encoding.UTF8;
                        //mensaje.SubjectEncoding = System.Text.Encoding.UTF8;
                        mensaje.HeadersEncoding = System.Text.Encoding.UTF8;
                        mensaje.IsBodyHtml = true;
                        //--
                        mensaje.To.Add(correo);

                        if (archivos)
                        {
                            if (modelo.Origen_Anexos.Count > 0)
                            {
                                Attachment anexo;
                                foreach (var a in modelo.Origen_Anexos)
                                {
                                    anexo = new Attachment(Path.Combine(_environment.WebRootPath, a), MediaTypeNames.Application.Octet);

                                    mensaje.Attachments.Add(anexo);
                                }
                            }
                        }
                        //--
                       //await cliente.SendMailAsync(mensaje); //++                      
                    }
                }
            }
        }
    }
}
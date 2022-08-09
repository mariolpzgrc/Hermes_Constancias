using Hermes2018.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hermes2018.Services
{
    public interface IMailService
    {
        Task<bool> EnviarCorreo(string[] para, string[] ccp, string[] anexos, string asunto, string cuerpo);
        Task<bool> EnviarCorreoEspecialEnvio(ResumenEnvioDocumentoCorreoViewModel modelo, string plantilla);
        Task<bool> EnviarCorreoEspecialTurnar(ResumenTurnarDocumentoCorreoViewModel modelo, string plantilla);
        Task<bool> EnviarCorreoEspecialTurnarEspecial(ResumenTurnarEspecialDocumentoCorreoViewModel modelo, string plantilla);

        Task<bool> EnviarCorreoEspecialResponder(ResumenResponderDocumentoCorreoViewModel modelo, string plantilla);

        //Envío de oficio al correo - Lectura
        Task EnviarCorreoEspecialLectura(string usuario, string correo, bool archivos, ResumenEnvioDocumentoCorreoViewModel modelo);
    }
}

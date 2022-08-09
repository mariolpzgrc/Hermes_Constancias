using Hermes2018.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hermes2018.Services.Interfaces
{
    public interface IDescargarConstanciaPDFService
    {
        byte[] DescargarConstanciaDocente(ConstanciaViewModel  modelo, int idTipoConstancia, string nombreConstancia, int idTipoPersonal);
        byte[] DescargarConstanciaNoDocente(ConstanciaViewModel modelo, int idTipoConstancia, string nombreConstancia, int idTipoPersonal);
        byte[] DescargarOficioBajaIPEMagisterio();
    }
}

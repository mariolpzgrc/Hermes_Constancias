using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hermes2018.ViewModels
{
    public class CorreoViewModel
    {        
            //Origen
            //Oficio
            public string Folio { get; set; }
            public string Asunto { get; set; }
            public string NoInterno { get; set; }
            public string Fecha { get; set; }
            public string Cuerpo { get; set; }
            public string Importancia { get; set; }
            public string TipoDocumento { get; set; }

            //Usuario De Origen
            public ResumenInfoRemitenteOrigenViewModel Origen_UsuarioDe { get; set; }
            //Usuario Para Origen
            public List<ResumenInfoDestinatariosOrigenViewModel> Origen_UsuarioPara { get; set; }
            //Usuario CCP Origen
            public List<ResumenInfoDestinatariosOrigenViewModel> Origen_UsuarioCcp { get; set; }
            //Nombre de quien lo ha creado/ ya sea el titular o el delegado
            public string Origen_NombreCreador { get; set; }
            public string Origen_UsuarioCreador { get; set; }
            public string Origen_NombreTitular { get; set; }
            public string Origen_UsuarioTitular { get; set; }
            //Anexos Origen
            public List<string> Origen_Anexos { get; set; }

    }
}

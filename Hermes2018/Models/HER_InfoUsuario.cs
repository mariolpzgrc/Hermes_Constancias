using Hermes2018.Models.Area;
using Hermes2018.Models.Categoria;
using Hermes2018.Models.Configuracion;
using Hermes2018.Models.Carpeta;
using Hermes2018.Models.Grupo;
using Hermes2018.Models.Documento;
using Hermes2018.Models.Plantilla;
using Hermes2018.Models.Rol;
using Hermes2018.Models.Servicio;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Hermes2018.Models.Tramite;

namespace Hermes2018.Models
{
    public class HER_InfoUsuario
    {
        [Key]
        public int HER_InfoUsuarioId { get; set; }

        public string HER_NombreCompleto { get; set; }
        public string HER_Correo { get; set; }
        public string HER_NumeroPersonal { get; set; }
        public string HER_TipoPersonal { get; set; }
        public string HER_UserName { get; set; }
        public string HER_Direccion { get; set; }
        public string HER_Telefono { get; set; }
        public DateTime HER_FechaRegistro { get; set; }
        public DateTime HER_FechaActualizacion { get; set; }
        public bool HER_Activo { get; set; }
        public bool HER_EstaEnReasignacion { get; set; }
        public bool HER_EstaEnBajaDefinitiva { get; set; }
        public string HER_Titular { get; set; }
        public string HER_RolNombre { get; set; }
        //---*
        public string HER_Puesto { get; set; }
        public bool HER_EsUnico { get; set; } //Para el puesto
        //---*
        public string HER_BandejaUsuario { get; set; } //Nombre del usuario de la bandeja actual
        public int HER_BandejaPermiso { get; set; } //Nombre del permiso del usuario de la bandeja actual
        public string HER_BandejaNombre { get; set; } //Nombre completo del usuario de la bandeja actual
        public bool HER_PermisoAA { get; set; } //Permisos de administrador del área (P-AA)
        //---*

        public int HER_AreaId { get; set; }
        [ForeignKey("HER_AreaId")]
        public HER_Area HER_Area { get; set; }
        
        public string HER_UsuarioId { get; set; }
        [ForeignKey("HER_UsuarioId")]
        public HER_Usuario HER_Usuario { get; set; }

        [InverseProperty("HER_Titular")]
        public IEnumerable<HER_Delegar> HER_Delegados { get; set; }

        [InverseProperty("HER_Delegado")]
        public IEnumerable<HER_Delegar> HER_Titulares { get; set; }
        
        //--Documento
        [InverseProperty("HER_DocumentoTitular")]
        public IEnumerable<HER_Documento> HER_DocTitular { get; set; }

        [InverseProperty("HER_DocumentoCreador")]
        public IEnumerable<HER_Documento> HER_DocCreador { get; set; }
        
        //--Documento Base
        [InverseProperty("HER_DocumentoBaseTitular")]
        public IEnumerable<HER_DocumentoBase> HER_DocBaseTitular { get; set; }

        [InverseProperty("HER_DocumentoBaseCreador")]
        public IEnumerable<HER_DocumentoBase> HER_DocBaseCreador { get; set; }

        //--Envio_Revision
        [InverseProperty("HER_RevisionRemitente")]
        public HER_EnvioRevision HER_RevRemitente { get; set; }

        [InverseProperty("HER_RevisionDestinatario")]
        public HER_EnvioRevision HER_RevDestinatario { get; set; }
        
        //--Envio
        [InverseProperty("HER_De")]
        public HER_Envio HER_EnvioUsuarioDe { get; set; }

        [InverseProperty("HER_UsuarioEnvia")]
        public HER_Envio HER_EnvioUsuarioEnvia { get; set; }

        //--Recepcion
        public HER_Recepcion HER_RecepcionUsuarioPara { get; set; }
        public IEnumerable<HER_Grupo> HER_Grupos { get; set; }
        public IEnumerable<HER_GrupoIntegrante> HER_GrupoIntegrantes { get; set; }
        public IEnumerable<HER_ServicioIntegrante> HER_ServicioIntegrantes { get; set; }
        public IEnumerable<HER_Categoria> HER_Categorias { get; set; }
        public IEnumerable<HER_Carpeta> HER_Carpetas { get; set; }
        public IEnumerable<HER_Plantilla> HER_Plantillas { get; set; }
        //--
        public IEnumerable<HER_Destinatario> HER_Destinatarios { get; set; }
        //--
        public IEnumerable<HER_Tramite> HER_Tramites { get; set; }

        //Historial correo
        public IEnumerable<HER_HistorialCorreo> HER_HistorialCorreo { get; set; }
    }
}

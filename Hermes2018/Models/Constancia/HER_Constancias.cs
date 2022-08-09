using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using Hermes2018.Helpers;
namespace Hermes2018.Models.Constancia
{
    public class HER_Constancias
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public int DiasAtencion { get; set; }

        public bool Visible { get; set; }

        public bool SolicitudActiva { get; set; }

        public List<HER_Constancias> Get_HER_Constancias(int tipoPersonal, string usuarioId)
        {
            SQLConnect con = new SQLConnect();
            List<SqlParameter> pars = new List<SqlParameter>();
            DataTable datos = con.executeDataTable("SELECT c.*,(SELECT ISNULL(COUNT(Id),0) FROM HER_SolicitudConstancia WHERE ConstanciaId=c.Id AND UsuarioId='"+ usuarioId + "' AND TipoPersonal=" + tipoPersonal + " AND EstadoId NOT IN(2,5,6,7,8)) AS ExisteSolicitud FROM HER_Constancias  c WHERE c.Id IN (SELECT HER_ConstanciaId FROM HER_ConstanciaTipoPersonal WHERE HER_TipoPersonal=" + tipoPersonal+")  and c.Id NOT IN (4,13,14)", CommandType.Text, null);
            HER_Constancias info = null;
            List<HER_Constancias> informacion = new List<HER_Constancias>();
            foreach (DataRow registro in datos.Rows)
            {
                info = new HER_Constancias();
                if (DBNull.Value != registro["Id"])
                    info.Id = Convert.ToInt32(registro["Id"]);
                if (DBNull.Value != registro["Nombre"])
                    info.Nombre = Convert.ToString(registro["Nombre"]);
                if (DBNull.Value != registro["DiasAtencion"])
                    info.DiasAtencion = Convert.ToInt32(registro["DiasAtencion"]);
                if (DBNull.Value != registro["Visible"])
                    info.Visible = Convert.ToBoolean(registro["Visible"]);

                SolicitudActiva = false;
                if (DBNull.Value != registro["ExisteSolicitud"])
                    info.SolicitudActiva = Convert.ToInt32(registro["ExisteSolicitud"]) > 0 ? true : false;
                informacion.Add(info);
            }
            return informacion;
        }

        public HER_Constancias() { }
    }
}

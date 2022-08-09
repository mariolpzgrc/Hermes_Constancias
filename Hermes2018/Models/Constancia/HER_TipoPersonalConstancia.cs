using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Hermes2018.Helpers;

namespace Hermes2018.Models.Constancia
{
    public class HER_TipoPersonalConstancia
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public List<HER_TipoPersonalConstancia> Get_HER_TipoPersonalConstancia()
        {
            SQLConnect con = new SQLConnect();
            List<SqlParameter> pars = new List<SqlParameter>();
            DataTable datos = con.executeDataTable("SELECT * FROM HER_TipoPersonalConstancia", CommandType.Text, null);
            HER_TipoPersonalConstancia info = null;
            List<HER_TipoPersonalConstancia> informacion = new List<HER_TipoPersonalConstancia>();
            foreach (DataRow registro in datos.Rows)
            {
                info = new HER_TipoPersonalConstancia();
                if (DBNull.Value != registro["Id"])
                    info.Id = Convert.ToInt32(registro["Id"]);
                if (DBNull.Value != registro["Nombre"])
                    info.Nombre = Convert.ToString(registro["Nombre"]);
                informacion.Add(info);
            }
            return informacion;
        }

        public HER_TipoPersonalConstancia() { }
    }
}
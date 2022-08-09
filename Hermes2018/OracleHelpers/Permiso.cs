using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Hermes2018.OracleHelpers
{
    public class Permiso
    {
        public int VerificarSeguridad(object pagina, object conexionacad)
        {
            string str1 = "";
            string str2 = "1.0";
            int num1 = 12345678;
            int num2 = 87651234;
            string Left;
            int num3;
            try
            {
                OracleCommand oracleDbCommand = new OracleCommand();
                oracleDbCommand.Connection = (OracleConnection)conexionacad;
                oracleDbCommand.CommandType = CommandType.StoredProcedure;
                oracleDbCommand.CommandText = "G$_SECURITY.G$_VERIFY_PASSWORD1_PRD";

                OracleParameter oracleDbParameter1 = new OracleParameter(nameof(pagina), OracleDbType.Char, 30);
                OracleParameter oracleDbParameter2 = new OracleParameter("Version", OracleDbType.Char, 10);
                OracleParameter oracleDbParameter3 = new OracleParameter("Password", OracleDbType.Varchar2, 30);
                OracleParameter oracleDbParameter4 = new OracleParameter("rol", OracleDbType.Varchar2, 30);

                oracleDbParameter1.Direction = ParameterDirection.Input;
                oracleDbParameter2.Direction = ParameterDirection.Input;
                oracleDbParameter3.Direction = ParameterDirection.InputOutput;
                oracleDbParameter4.Direction = ParameterDirection.Output;
                oracleDbParameter1.Value = RuntimeHelpers.GetObjectValue(pagina);
                oracleDbParameter2.Value = (object)str2;

                oracleDbCommand.Parameters.Add(oracleDbParameter1);
                oracleDbCommand.Parameters.Add(oracleDbParameter2);
                oracleDbCommand.Parameters.Add(oracleDbParameter3);
                oracleDbCommand.Parameters.Add(oracleDbParameter4);
                oracleDbCommand.ExecuteNonQuery();

                Left = oracleDbCommand.Parameters["Password"].Value.ToString().Trim();
                oracleDbCommand.Dispose();

                if (string.Compare(Left, "INSECURED", false) == 0)
                {
                    num3 = Conversions.ToInteger(Left);
                    goto label_14;
                }
            }
            catch (Exception ex)
            {
                ProjectData.SetProjectError(ex);
                str1 = string.Format("{0}{1}", "El usuario: no tiene permisos : ", pagina);
                num3 = 1;
                ProjectData.ClearProjectError();
                goto label_14;
            }

            OracleCommand oracleDbCommand1 = new OracleCommand();
            string str3;
            try
            {
                oracleDbCommand1.Connection = (OracleConnection)conexionacad;
                oracleDbCommand1.CommandText = "G$_SECURITY.G$_DECRYPT_FNC";
                oracleDbCommand1.CommandType = CommandType.StoredProcedure;

                OracleParameter oracleDbParameter1 = new OracleParameter("Password", OracleDbType.Varchar2, 30);
                OracleParameter oracleDbParameter2 = new OracleParameter("Password2", OracleDbType.Varchar2, 30);
                OracleParameter oracleDbParameter3 = new OracleParameter("seed3", OracleDbType.Int32, 10);

                oracleDbParameter1.Direction = ParameterDirection.ReturnValue;
                oracleDbParameter2.Direction = ParameterDirection.Input;
                oracleDbParameter3.Direction = ParameterDirection.Input;
                oracleDbParameter2.Value = (object)Left;
                oracleDbParameter3.Value = (object)num2;

                oracleDbCommand1.Parameters.Add(oracleDbParameter1);
                oracleDbCommand1.Parameters.Add(oracleDbParameter2);
                oracleDbCommand1.Parameters.Add(oracleDbParameter3);
                oracleDbCommand1.ExecuteNonQuery();

                str3 = oracleDbCommand1.Parameters["Password"].Value.ToString().Trim();
            }
            catch (Exception ex)
            {
                ProjectData.SetProjectError(ex);
                str1 = "Error al momento de ejecutar la segunda fase del proceso, verifique que cuenta con los permisos correspondientes";
                num3 = 2;
                ProjectData.ClearProjectError();
                goto label_14;
            }

            string str4;
            string str5;
            try
            {
                OracleCommand oracleDbCommand2 = new OracleCommand();
                oracleDbCommand2.Connection = (OracleConnection)conexionacad;
                oracleDbCommand2.CommandText = "G$_SECURITY.G$_VERIFY_PASSWORD1_PRD";
                oracleDbCommand2.CommandType = CommandType.StoredProcedure;

                OracleParameter oracleDbParameter1 = new OracleParameter("Objeto", OracleDbType.Char, 30);
                OracleParameter oracleDbParameter2 = new OracleParameter("Version", OracleDbType.Char, 10);
                OracleParameter oracleDbParameter3 = new OracleParameter("Password", OracleDbType.Varchar2, 30);
                OracleParameter oracleDbParameter4 = new OracleParameter("Rol", OracleDbType.Varchar2, 30);

                oracleDbParameter1.Direction = ParameterDirection.Input;
                oracleDbParameter2.Direction = ParameterDirection.Input;
                oracleDbParameter3.Direction = ParameterDirection.InputOutput;
                oracleDbParameter4.Direction = ParameterDirection.Output;
                oracleDbParameter1.Value = RuntimeHelpers.GetObjectValue(pagina);
                oracleDbParameter2.Value = (object)str2;
                oracleDbParameter3.Value = (object)str3;

                oracleDbCommand2.Parameters.Add(oracleDbParameter1);
                oracleDbCommand2.Parameters.Add(oracleDbParameter2);
                oracleDbCommand2.Parameters.Add(oracleDbParameter3);
                oracleDbCommand2.Parameters.Add(oracleDbParameter4);
                oracleDbCommand2.ExecuteNonQuery();

                str4 = oracleDbCommand2.Parameters["Password"].Value.ToString().Trim();
                str5 = oracleDbCommand2.Parameters["Rol"].Value.ToString().Trim();
                oracleDbCommand2.Dispose();
            }
            catch (Exception ex)
            {
                ProjectData.SetProjectError(ex);
                str1 = "Error al momento de ejecutar la tercera fase del proceso, verifique que cuenta con los permisos correspondientes";
                num3 = 2;
                ProjectData.ClearProjectError();
                goto label_14;
            }

            string str6;
            try
            {
                oracleDbCommand1.Parameters["Password2"].Value = (object)str4;
                oracleDbCommand1.Parameters["seed3"].Value = (object)num1;
                oracleDbCommand1.ExecuteNonQuery();

                str6 = "\"" + oracleDbCommand1.Parameters["password"].Value.ToString().Trim() + "\"";
                oracleDbCommand1.Dispose();
            }
            catch (Exception ex)
            {
                ProjectData.SetProjectError(ex);
                str1 = "error en el 4 paso " + ex.Message.ToString();// Information.Err().Description;
                num3 = 3;
                ProjectData.ClearProjectError();
                goto label_14;
            }

            try
            {
                OracleCommand oracleDbCommand2 = new OracleCommand();
                string str7 = str5 + " IDENTIFIED BY " + str6;
                oracleDbCommand2.Connection = (OracleConnection)conexionacad;
                oracleDbCommand2.CommandText = "DBMS_SESSION.SET_ROLE";
                oracleDbCommand2.CommandType = CommandType.StoredProcedure;
                oracleDbCommand2.Parameters.Add(new OracleParameter("setrol", OracleDbType.Varchar2, str7.Length)
                {
                    Direction = ParameterDirection.Input,
                    Value = (object)(str7.Trim())
                });
                oracleDbCommand2.ExecuteNonQuery();
                oracleDbCommand2.Dispose();
            }
            catch (Exception ex)
            {
                ProjectData.SetProjectError(ex);
                str1 = "Error en el quinto paso : " + ex.Message.ToString(); //Information.Err().Description;
                num3 = 4;
                ProjectData.ClearProjectError();
                goto label_14;
            }

            str1 = ""; //Strings.Trim 
            return 0;

            label_14:
            return num3;
        }
        public object RevocarSeguridad(object conexionacad)
        {
            string str1 = "NONE";
            string str2 = "";
            try
            {
                OracleCommand oracleDbCommand = new OracleCommand();
                oracleDbCommand.Connection = (OracleConnection)conexionacad;
                oracleDbCommand.CommandType = CommandType.StoredProcedure;
                oracleDbCommand.CommandText = "DBMS_SESSION.SET_ROLE";
                oracleDbCommand.Parameters.Add(new OracleParameter("RevocarPermiso", OracleDbType.Varchar2, str1.Length)
                {
                    Direction = ParameterDirection.Input,
                    Value = (object)str1
                });
                oracleDbCommand.ExecuteNonQuery();
                oracleDbCommand.Dispose();
            }
            catch (Exception ex)
            {
                ProjectData.SetProjectError(ex);
                str2 = "error al revocar " + ex.Message;
                ProjectData.ClearProjectError();
            }
            return (object)str2;
        }
    }
}

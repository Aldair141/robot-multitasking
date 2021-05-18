using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using ServicioWinSUNAT.Modelo;

namespace ServicioWinSUNAT.Servicio
{
    public class InsertarDataDA
    {
        SqlConnection cnx = null;

        public InsertarDataDA()
        {
                cnx = ConexionDA.GetConnection;
        }

        public Tb_ClienteBE Get(Tb_ClienteBE entidad)
        {
            try
            {
                if (cnx.State != ConnectionState.Open)
                    cnx.Close();

                SqlCommand cmd = new SqlCommand("tb_cliente_SelectByNmruc", cnx);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;

                cmd.Parameters.Add("@nmruc", SqlDbType.Char, 11).Value = entidad.nmruc;

                cnx.Open();

                SqlDataReader lector = cmd.ExecuteReader();

                if (lector.Read())
                {
                    Tb_ClienteBE objCliente = new Tb_ClienteBE();
                    objCliente.ctacte = lector.GetString(0);
                    objCliente.ctactename = lector.GetString(1);
                    objCliente.docuidentid = lector.GetString(2);
                    objCliente.nmruc = lector.GetString(3);

                    return objCliente;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (cnx != null)
                    cnx.Close();
            }
        }

        public bool Create(Tb_ClienteBE value)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("gspTbCliente_INSERT", cnx);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;

                cmd.Parameters.Add("@ctactename", SqlDbType.Char, 200).Value = value.ctactename;
                cmd.Parameters.Add("@nmruc", SqlDbType.Char, 11).Value = value.nmruc;
                cmd.Parameters.Add("@docuidentid", SqlDbType.Char).Value = value.docuidentid;
                cmd.Parameters.Add("@direc", SqlDbType.VarChar, 100).Value = value.direc;
                cmd.Parameters.Add("@rubroid", SqlDbType.Char, 2).Value = "00";

                cnx.Open();

                return (cmd.ExecuteNonQuery() > 0);
            }
            catch(Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (cnx != null)
                    cnx.Close();
            }
        }
    }
}
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

        public TbClienteSUNAT Get(string ruc)
        {
            try
            {
                if (cnx.State != ConnectionState.Open)
                    cnx.Close();

                //SqlCommand cmd = new SqlCommand("tb_cliente_SelectByNmruc", cnx);
                SqlCommand cmd = new SqlCommand("RSUtil.ClienteSUNAT_GET", cnx);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;

                //cmd.Parameters.Add("@nmruc", SqlDbType.Char, 11).Value = entidad.nmruc;
                cmd.Parameters.Add("@ruc", SqlDbType.VarChar, 11).Value = ruc;

                cnx.Open();

                SqlDataReader lector = cmd.ExecuteReader();

                if (lector.Read())
                {

                    TbClienteSUNAT clienteSUNAT = new TbClienteSUNAT();
                    clienteSUNAT.ruc = lector.GetString(0);
                    clienteSUNAT.razonSocial = lector.GetString(1);
                    clienteSUNAT.estadoContribuyente = lector.GetString(2);
                    clienteSUNAT.condicionDomicilio = lector.GetString(3);
                    clienteSUNAT.ubigeo = lector.GetString(4);
                    clienteSUNAT.tipoVia = lector.GetString(5);
                    clienteSUNAT.nombreVia = lector.GetString(6);
                    clienteSUNAT.codigoZona = lector.GetString(7);
                    clienteSUNAT.tipoZona = lector.GetString(8);
                    clienteSUNAT.numero = lector.GetString(9);
                    clienteSUNAT.interior = lector.GetString(10);
                    clienteSUNAT.lote = lector.GetString(11);
                    clienteSUNAT.departamento = lector.GetString(12);
                    clienteSUNAT.manzana = lector.GetString(13);
                    clienteSUNAT.kilometro = lector.GetString(14);
                    return clienteSUNAT;
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

        public bool Create(TbClienteSUNAT value)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("RSUtil.ClienteSUNAT_INSERT", cnx);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;

                cmd.Parameters.Add("@ruc", SqlDbType.VarChar, 11).Value = value.ruc;
                cmd.Parameters.Add("@razonSocial", SqlDbType.VarChar, 150).Value = value.razonSocial;
                cmd.Parameters.Add("@estadoContribuyente", SqlDbType.VarChar, 50).Value = value.estadoContribuyente;
                cmd.Parameters.Add("@condicionDomicilio", SqlDbType.VarChar, 50).Value = value.condicionDomicilio;
                cmd.Parameters.Add("@ubigeo", SqlDbType.VarChar, 20).Value = value.ubigeo;
                cmd.Parameters.Add("@tipoVia", SqlDbType.VarChar, 20).Value = value.tipoVia;
                cmd.Parameters.Add("@nombreVia", SqlDbType.VarChar, 60).Value = value.nombreVia;
                cmd.Parameters.Add("@codigoZona", SqlDbType.VarChar, 20).Value = value.codigoZona;
                cmd.Parameters.Add("@tipoZona", SqlDbType.VarChar, 60).Value = value.tipoZona;
                cmd.Parameters.Add("@numero", SqlDbType.VarChar, 60).Value = value.numero;
                cmd.Parameters.Add("@interior", SqlDbType.VarChar, 60).Value = value.interior;
                cmd.Parameters.Add("@lote", SqlDbType.VarChar, 60).Value = value.lote;
                cmd.Parameters.Add("@departamento", SqlDbType.VarChar, 60).Value = value.departamento;
                cmd.Parameters.Add("@manzana", SqlDbType.VarChar, 60).Value = value.manzana;
                cmd.Parameters.Add("@kilometro", SqlDbType.VarChar, 60).Value = value.kilometro;

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
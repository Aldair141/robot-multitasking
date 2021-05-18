using ServicioWinSUNAT.Modelo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicioWinSUNAT.Servicio
{
    public class TaskDA
    {
        SqlConnection cnx = null;
        public TaskDA()
        {
            cnx = ConexionDA.GetConnection;
        }
        public List<Tb_Task_SchedulerBE> Get(Tb_Task_SchedulerBE objProceso)
        {
            try
            {
                if (cnx.State != ConnectionState.Open)
                    cnx.Close();

                SqlCommand cmd = new SqlCommand("tb_task_scheduler_SELECT", cnx);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;

                cmd.Parameters.Add("@hour", SqlDbType.Time).Value = objProceso.TaskHour;

                cnx.Open();

                SqlDataReader lector = cmd.ExecuteReader();
                List<Tb_Task_SchedulerBE> lista = null;
                Tb_Task_SchedulerBE objTask = null;

                if (lector.HasRows)
                {
                    lista = new List<Tb_Task_SchedulerBE>();

                    while (lector.Read())
                    {
                        objTask = new Tb_Task_SchedulerBE();
                        objTask.TaskID = lector.GetInt32(0);
                        objTask.TaskName = lector.GetString(1);
                        objTask.TaskDate = lector.GetDateTime(2);
                        objTask.TaskHour = lector.GetTimeSpan(3);
                        objTask.TaskLastDate = lector.GetDateTime(4);
                        objTask.TaskStatus = lector.GetBoolean(5);

                        lista.Add(objTask);
                    }
                }

                return lista;
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
    }
}
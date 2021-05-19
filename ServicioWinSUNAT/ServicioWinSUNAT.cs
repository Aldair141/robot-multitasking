using ServicioWinSUNAT.Modelo;
using ServicioWinSUNAT.Servicio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;

namespace ServicioWinSUNAT
{
    public partial class ServicioWinSUNAT : ServiceBase
    {
        System.Timers.Timer tm;
        InsertarDataDA dataDA;
        TaskDA taskDA;

        //Tarea SUNAT
        ThreadStart tareaSUNAT;
        Thread hiloSUNAT;
        bool procesandoSUNAT;
        public ServicioWinSUNAT()
        {
            InitializeComponent();
            eventosSistema = new EventLog();

            if (!EventLog.SourceExists("ServicioWinSUNAT"))
            {
                EventLog.CreateEventSource("ServicioWinSUNAT", "Application");
            }

            eventosSistema.Source = "ServicioWinSUNAT";
            eventosSistema.Log = "Application";
        }

        protected override void OnStart(string[] args)
        {
            taskDA = new TaskDA();
            dataDA = new InsertarDataDA();
            
            tm = new System.Timers.Timer();
            tm.Interval = 60000;
            tm.Elapsed += new System.Timers.ElapsedEventHandler(ProcesamientoGeneral);
            tm.Start();

            eventosSistema.WriteEntry("Servicio SUNAT iniciado.");

            tareaSUNAT = new ThreadStart(InsertarDataSUNAT);
            hiloSUNAT = new Thread(tareaSUNAT);
        }

        private void ProcesamientoGeneral(object sender, System.Timers.ElapsedEventArgs e)
        {
            List<Tb_Task_SchedulerBE> listaTareas = taskDA.Get(new Tb_Task_SchedulerBE { TaskHour = DateTime.Now.TimeOfDay });
            if (listaTareas != null)
            {
                foreach (Tb_Task_SchedulerBE item in listaTareas)
                {
                    if (item.TaskName == "SUNAT_DATA")
                    {
                        if (hiloSUNAT.ThreadState != System.Threading.ThreadState.Running)
                        {
                            hiloSUNAT.Start();
                        }
                    }
                    if (item.TaskName == "IMPRIMIR_TEXTO")
                    {
                        eventosSistema.WriteEntry("Imprimiendo texto.");
                    }
                }
            }
        }

        private void InsertarDataSUNAT()
        {
            string[] registrosSunat = File.ReadAllLines("C:\\padron_reducido_ruc.txt");
            TbClienteSUNAT clienteSUNAT;

            int insertados = 0;
            int noinsertados = 0;
            int existentes = 0;

            if (registrosSunat.Length > 1) //Sin contar la cabecera
            {
                procesandoSUNAT = true;
                //for (int i = 1; i < registrosSunat.Length; i++)
                for (int i = 1; i < 10; i++)
                {
                    if (!procesandoSUNAT)
                    {
                        eventosSistema.WriteEntry("Se detuvo el proceso de grabación a SUNAT");
                        break;
                    }

                    string[] lineContent = registrosSunat[i].Split('|');

                    try
                    {
                        if (dataDA.Get(lineContent[0].Trim()) == null)
                        {
                            clienteSUNAT = new TbClienteSUNAT();
                            clienteSUNAT.ruc = lineContent[0];
                            clienteSUNAT.razonSocial = lineContent[1];
                            clienteSUNAT.estadoContribuyente = lineContent[2];
                            clienteSUNAT.condicionDomicilio = lineContent[3];
                            clienteSUNAT.ubigeo = lineContent[4];
                            clienteSUNAT.tipoVia = lineContent[5];
                            clienteSUNAT.nombreVia = lineContent[6];
                            clienteSUNAT.codigoZona = lineContent[7];
                            clienteSUNAT.tipoZona = lineContent[8];
                            clienteSUNAT.numero = lineContent[9];
                            clienteSUNAT.interior = lineContent[10];
                            clienteSUNAT.lote = lineContent[11];
                            clienteSUNAT.departamento = lineContent[12];
                            clienteSUNAT.manzana = lineContent[13];
                            clienteSUNAT.kilometro = lineContent[14];

                            if (dataDA.Create(clienteSUNAT))
                            {
                                insertados++;
                            }
                            else
                            {
                                noinsertados++;
                            }
                        }
                        else
                        {
                            existentes++;
                        }
                    }
                    catch (Exception ex)
                    {
                        eventosSistema.WriteEntry($"Error: {ex.Message}");
                    }

                    

                    //tb_ClienteBE = new Tb_ClienteBE();
                    //tb_ClienteBE.nmruc = datos[0];
                    //tb_ClienteBE.ctactename = datos[1];
                    //tb_ClienteBE.docuidentid = "6";
                    //tb_ClienteBE.rubroid = "00";

                    //try
                    //{
                    //    if (dataDA.Get(tb_ClienteBE) == null)
                    //    {
                    //        if (dataDA.Create(tb_ClienteBE))
                    //        {
                    //            insertados++;
                    //        }
                    //        else
                    //        {
                    //            noinsertados++;
                    //        }
                    //    }
                    //    else
                    //    {
                    //        existentes++;
                    //    }
                    //}
                    //catch (Exception ex)
                    //{
                    //    eventosSistema.WriteEntry($"Error: {ex.Message}");
                    //}
                }
            }
            eventosSistema.WriteEntry($"{insertados} registros insertados.\n{noinsertados} registros no se pudieron insertar.\n{existentes} registros ya existen.");
        }

        protected override void OnStop()
        {
            procesandoSUNAT = false;
            eventosSistema.WriteEntry("Servicio SUNAT detenido.");
        }
    }
}

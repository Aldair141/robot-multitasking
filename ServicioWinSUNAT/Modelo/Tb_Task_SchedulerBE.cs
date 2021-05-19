using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicioWinSUNAT.Modelo
{
    public class Tb_Task_SchedulerBE
    {
        public int TaskID { get; set; }
        public string TaskName { get; set; }
        public DateTime TaskDate { get; set; }
        public TimeSpan TaskHour { get; set; }
        public Nullable<DateTime> TaskLastDate { get; set; }
        public bool TaskStatus { get; set; }
        public int TaskNextDay { get; set; }
    }
}
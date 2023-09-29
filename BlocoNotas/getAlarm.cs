using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BlocoNotas
{
    public class getAlarm
    {
        private string time;
        private Thread alarmThread;
        private volatile bool stopAlarm = false; // Variável de sinalização

        public getAlarm(string time)
        {
            this.time = time;
        }

        public void Start()
        {
            alarmThread = new Thread(StartAlarm);
            alarmThread.Start();
        }

        public void Remove()
        {
            stopAlarm = true; // Sinalize para o thread que ele deve parar
            if (alarmThread != null && alarmThread.IsAlive)
            {
                alarmThread.Join(); // Aguarde até que o thread seja encerrado
            }
        }

        private void StartAlarm()
        {
            if (DateTime.TryParseExact(time, "HH:mm:ss", null, System.Globalization.DateTimeStyles.None, out DateTime targetTime))
            {
                while (!stopAlarm && DateTime.Now < targetTime)
                {
                    // Aguarde até que o horário especificado seja atingido
                    Thread.Sleep(1000);
                }

                if (!stopAlarm)
                {
                    MessageBox.Show("Hora especificada atingida!");
                }
            }
            else
            {
                MessageBox.Show("Formato de hora inválido. Use HH:mm:ss.");
            }
        }
    }

    public class AlarmManager
    {
        private List<getAlarm> activeAlarms = new List<getAlarm>();

        public void AddAlarm(string time)
        {
            getAlarm alarm = new getAlarm(time);
            alarm.Start();
            activeAlarms.Add(alarm);
        }

        public void RemoveAllAlarms()
        {
            foreach (var alarm in activeAlarms)
            {
                alarm.Remove();
            }
            activeAlarms.Clear();
        }
    }
}

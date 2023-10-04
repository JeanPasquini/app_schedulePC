using System;
using System.Collections.Generic;
using System.IO;
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
        private volatile bool stopAlarm = false;
        private string nomeArquivo;

        public getAlarm(string time, string nomeArquivo)
        {
            this.time = time;
            this.nomeArquivo = nomeArquivo;
        }

        public void Start()
        {
            alarmThread = new Thread(StartAlarm);
            alarmThread.Start();
        }

        public void Remove()
        {
            stopAlarm = true;
            if (alarmThread != null && alarmThread.IsAlive)
            {
                alarmThread.Join();
            }
        }

        private void StartAlarm()
        {
            if (DateTime.TryParseExact(time, "dd/MM/yyyy HH:mm:ss", null, System.Globalization.DateTimeStyles.None, out DateTime targetTime))
            {
                while (!stopAlarm && DateTime.Now < targetTime)
                {
                    Thread.Sleep(1000);
                }

                if (!stopAlarm)
                {
                    string conteudoDoArquivo = LerConteudoDoArquivo(nomeArquivo);
                    string titulo = "Task: " + Path.GetFileNameWithoutExtension(nomeArquivo);
                    MessageBoxIcon icone = MessageBoxIcon.Information;

                    MessageBox.Show(conteudoDoArquivo, titulo, MessageBoxButtons.OK, icone);
                }
            }
            else
            {
                MessageBox.Show("Formato de data e hora inválido. Use dd/MM/yyyy HH:mm:ss.");
            }
        }

        private string LerConteudoDoArquivo(string nomeDoArquivo)
        {
            if (File.Exists(caminhoArquivoTxt(nomeDoArquivo)))
            {
                return File.ReadAllText(caminhoArquivoTxt(nomeDoArquivo));
            }
            else
            {
                return string.Empty;
            }
        }

        private string caminhoArquivoTxt(string nomeDoArquivo)
        {
            string diretorioDoApp = AppDomain.CurrentDomain.BaseDirectory;
            string subpastaTxt = "txt";
            string caminhoDaSubpastaTxt = Path.Combine(diretorioDoApp, subpastaTxt);
            string caminhoDoArquivo = Path.Combine(caminhoDaSubpastaTxt, nomeDoArquivo);

            if (File.Exists(caminhoDoArquivo))
            {
                return caminhoDoArquivo;
            }
            else
            {
                return string.Empty;
            }
        }
    }

    public class AlarmManager
    {
        private List<getAlarm> activeAlarms = new List<getAlarm>();
        private string nomeArquivo;

        public void AddAlarm(string time, string nomeArquivo)
        {
            this.nomeArquivo = nomeArquivo;
            getAlarm alarm = new getAlarm(time, nomeArquivo);
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

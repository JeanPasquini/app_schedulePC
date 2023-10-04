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
    public class Alarme
    {
        public DateTime HoraAlarme { get; set; }
        public DateTime DataAlarme { get; set; }
        public string Mensagem { get; set; }
    }
}

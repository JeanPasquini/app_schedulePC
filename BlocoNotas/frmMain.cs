using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BlocoNotas
{
    public partial class frmMain : Form
    {
        public string alarm;
        public string gridTitulo;
        static AlarmManager globalAlarmManager = new AlarmManager();

        public frmMain()
        {
            InitializeComponent();
        }

        private void Form1_Load_1(object sender, EventArgs e)
        {
            atualizarGrid();
            atualizaAlarm();
        }

        public void atualizaAlarm()
        {
            globalAlarmManager.RemoveAllAlarms();

            string diretorioDoApp = AppDomain.CurrentDomain.BaseDirectory;
            string subpastaConfigs = "configs";
            string caminhoDaSubpastaConfigs = Path.Combine(diretorioDoApp, subpastaConfigs);
            DirectoryInfo dirInfo = new DirectoryInfo(caminhoDaSubpastaConfigs);
            FileInfo[] arquivos = dirInfo.GetFiles("*.txt");

            foreach (FileInfo arquivo in arquivos)
            {
                string formato = "HH:mm:ss";

                if (DateTime.TryParseExact(getSpecificCodeString(arquivo.Name), formato, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime horaDaString))
                {
                    DateTime horaAtual = DateTime.Now;
                    if (horaDaString > horaAtual)
                    {
                        globalAlarmManager.AddAlarm(getSpecificCodeString(arquivo.Name));
                    }
                }
            }
        }

        private void atualizarGrid()
        {
            dataGridView2.Rows.Clear();

            string diretorioDoApp = AppDomain.CurrentDomain.BaseDirectory;
            DirectoryInfo dirInfo = new DirectoryInfo(diretorioDoApp);
            FileInfo[] arquivos = dirInfo.GetFiles("*.txt");

            foreach (FileInfo arquivo in arquivos)
            {
                string arquivoFormatado = System.IO.Path.GetFileNameWithoutExtension(arquivo.Name);
                dataGridView2.Rows.Add(arquivoFormatado, arquivo.CreationTime);
            }
        }

        private void dataGridView2_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                string nomeDoArquivo = dataGridView2.Rows[e.RowIndex].Cells[0].Value.ToString() + ".txt";
                getSpecificCode("config"+nomeDoArquivo);

                txtTitulo.Text = dataGridView2.Rows[e.RowIndex].Cells[0].Value.ToString();
                txtConteudo.Text = LerConteudoDoArquivo(nomeDoArquivo);
                txtAlarme.Text = alarm;
            }
        }

        private string LerConteudoDoArquivo(string nomeDoArquivo)
        {
            if (File.Exists(caminhoArquivo(nomeDoArquivo)))
            {
                return File.ReadAllText(caminhoArquivo(nomeDoArquivo));
            }
            else
            {
                return string.Empty;
            }
        }

        private string caminhoArquivo(string nomeDoArquivo)
        {
            string diretorioDoApp = AppDomain.CurrentDomain.BaseDirectory;
            string caminhoDoArquivo = Path.Combine(diretorioDoApp, nomeDoArquivo);

            if (File.Exists(caminhoDoArquivo))
            {
                return caminhoDoArquivo;
            }
            else
            {
                return string.Empty;
            }
        }

        private string caminhoArquivoConfig(string nomeDoArquivo)
        {
            string diretorioDoApp = AppDomain.CurrentDomain.BaseDirectory;
            string subpastaConfigs = "configs";
            string caminhoDaSubpastaConfigs = Path.Combine(diretorioDoApp, subpastaConfigs);
            string caminhoDoArquivo = Path.Combine(caminhoDaSubpastaConfigs, nomeDoArquivo);

            if (File.Exists(caminhoDoArquivo))
            {
                return caminhoDoArquivo;
            }
            else
            {
                return string.Empty;
            }
        }

        private void btnCriarArquivo_Click(object sender, EventArgs e)
        {
            frmCadastroNota frm = new frmCadastroNota(txtTitulo.Text, txtConteudo.Text, txtAlarme.Text);
            frm.ShowDialog();
            atualizarGrid();
            atualizaAlarm();
        }

        private string getSpecificCodeString(string nomeDoArquivo)
        {
            string keyword = "alarme";
            string caminho = caminhoArquivoConfig(nomeDoArquivo);
            try
            {
                string fileContent = File.ReadAllText(caminho);

                int keywordIndex = fileContent.IndexOf(keyword);

                if (keywordIndex != -1)
                {
                    string information = fileContent.Substring(keywordIndex + keyword.Length);

                    return alarm = information;
                }
                else
                {
                    return "nothing";
                }
            }
            catch (Exception ex)
            {
                return "erro";
            }
        }

        private void getSpecificCode(string nomeDoArquivo)
        {
            string keyword = "alarme";
            string caminho = caminhoArquivoConfig(nomeDoArquivo);
            try
            {
                string fileContent = File.ReadAllText(caminho);

                int keywordIndex = fileContent.IndexOf(keyword);

                if (keywordIndex != -1)
                {
                    string information = fileContent.Substring(keywordIndex + keyword.Length);

                    alarm = information;
                }
                else
                {
                    Console.WriteLine("Palavra-chave não encontrada no arquivo.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ocorreu um erro: {ex.Message}");
            }
        }

        private void btnExcluir_Click(object sender, EventArgs e)
        {

            try
            {
                if (File.Exists(caminhoArquivo(gridTitulo + ".txt")) && File.Exists(caminhoArquivoConfig("config" + gridTitulo + ".txt")))
                {
                    File.Delete(caminhoArquivo(gridTitulo + ".txt"));
                    File.Delete(caminhoArquivoConfig("config" + gridTitulo + ".txt"));
                    MessageBox.Show("Arquivo excluído com sucesso.");

                    gridTitulo = null;
                }
                else
                {
                    MessageBox.Show("O arquivo não existe.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocorreu um erro ao excluir o arquivo: " + ex.Message);
            }

            atualizarGrid();
        }

        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Certifique-se de que o clique foi em uma célula válida (não no cabeçalho)
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                // Obtém o valor da célula clicada na primeira coluna
                DataGridViewCell cell = dataGridView2.Rows[e.RowIndex].Cells[0];
                gridTitulo = cell.Value.ToString();

                // Agora 'valorSelecionado' contém o valor da célula clicada na primeira coluna
            }
        }
    }
}

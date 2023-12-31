﻿using System;
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
        public string data;
        public string hora;
        public string gridTitulo;
        static AlarmManager globalAlarmManager = new AlarmManager();
        private NotifyIcon notifyIcon;

        public frmMain()
        {
            InitializeComponent();
            dataGridView2.Columns[0].Width = 90;
            dataGridView2.Columns[1].Width = 80;
            InitializeNotifyIcon();
        }

        private void InitializeNotifyIcon()
        {
            notifyIcon = new NotifyIcon();
            notifyIcon.Icon = System.Drawing.Icon.ExtractAssociatedIcon(Application.ExecutablePath);
            notifyIcon.Text = "Schedule";
            notifyIcon.DoubleClick += new EventHandler(NotifyIcon_DoubleClick);
            ContextMenu contextMenu = new ContextMenu();
            contextMenu.MenuItems.Add("Abrir", NotifyIcon_Open);
            contextMenu.MenuItems.Add("Fechar", NotifyIcon_Close);
            notifyIcon.ContextMenu = contextMenu;
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
                string formato = "dd/MM/yyyy HH:mm:ss";
                string nomeArquivoSemConfig = arquivo.Name.Replace("config", "");

                if (DateTime.TryParseExact(getSpecificCodeString(arquivo.Name), formato, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime horaDaString))
                {
                    DateTime horaAtual = DateTime.Now;
                    if (horaDaString > horaAtual)
                    {
                        globalAlarmManager.AddAlarm(getSpecificCodeString(arquivo.Name), nomeArquivoSemConfig);
                    }
                }
            }
        }

        private void atualizarGrid()
        {
            dataGridView2.Rows.Clear();

            string diretorioDoApp = AppDomain.CurrentDomain.BaseDirectory;
            string subpastaTxt = "txt";
            string caminhoDaSubpastaTxt = Path.Combine(diretorioDoApp, subpastaTxt);

            string subpastaConfigs = "configs";
            string caminhoDaSubpastaConfigs = Path.Combine(diretorioDoApp, subpastaConfigs);

            

            DirectoryInfo dirInfo = new DirectoryInfo(caminhoDaSubpastaTxt);
            FileInfo[] arquivos = dirInfo.GetFiles("*.txt");

            foreach (FileInfo arquivo in arquivos)
            {
                getSpecificDates("config"+arquivo.Name);

                string arquivoFormatado = System.IO.Path.GetFileNameWithoutExtension(arquivo.Name);
                string horaMinutosCriacao = arquivo.CreationTime.ToString("HH:mm:ss"); // Formato de hora e minutos
                dataGridView2.Rows.Add(arquivoFormatado, data, hora);
            }

            txtData.Text = null;
            txtHora.Text = null;
            txtTitulo.Text = null;
            txtConteudo.Text = null;
        }

        private string LerConteudoDoArquivo(string nomeDoArquivo, bool txtConfig)
        {
            if (File.Exists(caminhoArquivoTxt(nomeDoArquivo)) && txtConfig == false)
            {
                return File.ReadAllText(caminhoArquivoTxt(nomeDoArquivo));
            }
            else if(File.Exists(caminhoArquivoConfig(nomeDoArquivo)) && txtConfig == true)
            {
                return File.ReadAllText(caminhoArquivoConfig(nomeDoArquivo));
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
            frmCadastroNota frm = new frmCadastroNota("", "", "", "");
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
                if (File.Exists(caminhoArquivoTxt(gridTitulo + ".txt")) && File.Exists(caminhoArquivoConfig("config" + gridTitulo + ".txt")))
                {
                    DialogResult resultado = MessageBox.Show("Tem certeza de que deseja excluir o arquivo: "+ gridTitulo +"?", "Confirmação", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (resultado == DialogResult.Yes)
                    {
                        File.Delete(caminhoArquivoTxt(gridTitulo + ".txt"));
                        File.Delete(caminhoArquivoConfig("config" + gridTitulo + ".txt"));
                        MessageBox.Show("Arquivo excluído com sucesso.");
                        gridTitulo = null;
                    }
                    else
                    {
                        MessageBox.Show("A exclusão do arquivo foi cancelada.");
                    }
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
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                DataGridViewCell cell = dataGridView2.Rows[e.RowIndex].Cells[0];
                if (cell.Value != null && cell.Value != DBNull.Value)
                {
                    gridTitulo = cell.Value.ToString();
                    string nomeDoArquivo = dataGridView2.Rows[e.RowIndex].Cells[0].Value.ToString() + ".txt";
                    string nome = dataGridView2.Rows[e.RowIndex].Cells[0].Value.ToString();
                    string nomeDoArquivoConfig = "config" + nomeDoArquivo;

                    getSpecificCode(nomeDoArquivoConfig);
                    getSpecificDates(nomeDoArquivoConfig);


                    txtTitulo.Text = nome;
                    txtConteudo.Text = LerConteudoDoArquivo(nomeDoArquivo, false);
                    txtData.Text = data;
                    txtHora.Text = hora;
                }
                else
                {
                    MessageBox.Show("Selecione um arquivo existente");
                }
            }
        }

        private void txtEditar_Click(object sender, EventArgs e)
        {
            if (txtTitulo.Text != "" && txtConteudo.Text != "")
            {
                frmCadastroNota frm = new frmCadastroNota(txtTitulo.Text, txtConteudo.Text, txtData.Text, txtHora.Text);
                frm.ShowDialog();
                atualizarGrid();
                atualizaAlarm();
            }
            else
            {
                MessageBox.Show("Selecione um arquivo para editar");
            }
        }

        private void NotifyIcon_DoubleClick(object sender, EventArgs e)
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
            notifyIcon.Visible = false;
        }

        private void NotifyIcon_Open(object sender, EventArgs e)
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
            notifyIcon.Visible = false;
        }

        private void NotifyIcon_Close(object sender, EventArgs e)
        {
            // Implemente a lógica para fechar o programa quando o usuário clicar em "Fechar" no menu de contexto do ícone na bandeja do sistema.
            Application.Exit();
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            if (this.WindowState == FormWindowState.Minimized)
            {
                this.Hide();
                notifyIcon.Visible = true;
            }
        }

        private void dataGridView2_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                DataGridViewCell cell = dataGridView2.Rows[e.RowIndex].Cells[0];
                if (cell.Value != null && cell.Value != DBNull.Value)
                {
                    gridTitulo = cell.Value.ToString();
                    string nomeDoArquivo = dataGridView2.Rows[e.RowIndex].Cells[0].Value.ToString() + ".txt";
                    string nome = dataGridView2.Rows[e.RowIndex].Cells[0].Value.ToString();
                    string nomeDoArquivoConfig = "config" + nomeDoArquivo;

                    getSpecificCode(nomeDoArquivoConfig);

                    txtTitulo.Text = nome;
                    txtConteudo.Text = LerConteudoDoArquivo(nomeDoArquivo, false);

                    if (txtTitulo.Text != "" && txtConteudo.Text != "")
                    {
                        frmCadastroNota frm = new frmCadastroNota(txtTitulo.Text, txtConteudo.Text, txtData.Text, txtHora.Text);
                        frm.ShowDialog();
                        atualizarGrid();
                        atualizaAlarm();
                    }
                    else
                    {
                        MessageBox.Show("Selecione um arquivo para editar");
                    }
                }
                else
                {
                    MessageBox.Show("Selecione um arquivo existente");
                }
            }
        }

        private void getSpecificDates(string configFileName)
        {
            // Suponha que 'alarm' é uma variável que armazena o valor completo da data e hora
            string alarm = LerConteudoDoArquivo(configFileName, true);

            alarm = alarm.Replace("alarme", "").Trim(); // Remove a palavra e remove espaços extras

            // Se a data e hora estão juntas, encontre o espaço em branco que separa a data da hora
            int spaceIndex = alarm.IndexOf(' ');

            if (spaceIndex >= 0)
            {
                // Agora, você pode separar a data e a hora usando a posição do espaço
                string data = alarm.Substring(0, spaceIndex); // Pega a parte da data
                string hora = alarm.Substring(spaceIndex + 1); // Pega a parte da hora

                // Agora você pode definir 'data' e 'hora' nas áreas apropriadas da sua interface do usuário
                // Exemplo:
                this.data = data;
                this.hora = hora;
            }
        }
    }
}

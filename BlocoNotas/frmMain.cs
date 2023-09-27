using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BlocoNotas
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        private void Form1_Load_1(object sender, EventArgs e)
        {
            // Define o diretório "txts" a partir do diretório onde o aplicativo está localizado

            atualizarGrid();
        }

        private void atualizarGrid()
        {
            dataGridView2.Rows.Clear();

            string diretorioDoApp = AppDomain.CurrentDomain.BaseDirectory;
            DirectoryInfo dirInfo = new DirectoryInfo(diretorioDoApp);
            FileInfo[] arquivos = dirInfo.GetFiles("*.txt");

            foreach (FileInfo arquivo in arquivos)
            {
                dataGridView2.Rows.Add(arquivo.Name, arquivo.CreationTime);
            }
        }

        private void dataGridView2_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            // Verifica se a célula clicada não é o cabeçalho da coluna
            if (e.RowIndex >= 0)
            {
                // Obtém o nome do arquivo e o conteúdo da célula selecionada
                string nomeDoArquivo = dataGridView2.Rows[e.RowIndex].Cells[0].Value.ToString(); // A primeira coluna (índice 0) contém o nome do arquivo
                string conteudoDoArquivo = LerConteudoDoArquivo(nomeDoArquivo);

                // Carrega o título e o conteúdo nas TextBoxes
                txtTitulo.Text = nomeDoArquivo;
                txtConteudo.Text = conteudoDoArquivo;
            }
        }

        // Função para ler o conteúdo do arquivo com base no nome do arquivo
        private string LerConteudoDoArquivo(string nomeDoArquivo)
        {
            // Construa o caminho completo do arquivo com base no diretório "txts"
            string diretorioDoApp = AppDomain.CurrentDomain.BaseDirectory;
            string caminhoDoArquivo = Path.Combine(diretorioDoApp, nomeDoArquivo);

            // Verifique se o arquivo existe
            if (File.Exists(caminhoDoArquivo))
            {
                // Leia o conteúdo do arquivo e retorne-o como uma string
                return File.ReadAllText(caminhoDoArquivo);
            }
            else
            {
                return string.Empty; // Arquivo não encontrado
            }
        }

        private void btnCriarArquivo_Click(object sender, EventArgs e)
        {
            frmCadastroNota frm = new frmCadastroNota(txtTitulo.Text, txtConteudo.Text);
            frm.ShowDialog();
            atualizarGrid();
        }
    }
}

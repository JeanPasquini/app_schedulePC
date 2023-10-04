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
    public partial class frmCadastroNota : Form
    {

        public string titulo;
        public string conteudo;
        public string alarme;
        public frmCadastroNota(string titulo, string conteudo, string alarme)
        {
            this.titulo = titulo;
            this.conteudo = conteudo;
            this.alarme = alarme;
            InitializeComponent();
        }

        private void salvar()
        {
            string diretorioDoApp = AppDomain.CurrentDomain.BaseDirectory;
            

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Arquivos de texto (*.txt)|*.txt|Todos os arquivos (*.*)|*.*";
            saveFileDialog.FileName = txtTitulo.Text;

            string subpastaTxts = "txt"; // Nome da subpasta que você deseja acessar
            string caminhoDaSubpastaTxts = Path.Combine(diretorioDoApp, subpastaTxts);

            saveFileDialog.InitialDirectory = diretorioDoApp;

            string filePath = Path.Combine(caminhoDaSubpastaTxts, txtTitulo.Text + ".txt");

            if (!filePath.EndsWith(".txt", StringComparison.OrdinalIgnoreCase))
            {
                filePath += ".txt";
            }

            File.WriteAllText(filePath, txtConteudo.Text);

            MessageBox.Show("Arquivo atualizado com sucesso.");

            // Config

            string subpastaConfigs = "configs"; // Nome da subpasta que você deseja acessar
            string caminhoDaSubpastaConfigs = Path.Combine(diretorioDoApp, subpastaConfigs);

            SaveFileDialog saveFileDialogConfig = new SaveFileDialog();
            saveFileDialogConfig.Filter = "Arquivos de texto (*.txt)|*.txt|Todos os arquivos (*.*)|*.*";
            saveFileDialogConfig.FileName = "config" + txtTitulo.Text;

            string filePathConfig = Path.Combine(caminhoDaSubpastaConfigs, "config" + txtTitulo.Text + ".txt");

            File.WriteAllText(filePathConfig, "alarme" + txtAlarme.Text);  
        }

        private void btnCriarArquivotxt_Click(object sender, EventArgs e)
        {
            string diretorioDoApp = AppDomain.CurrentDomain.BaseDirectory;
            string subpastaTxt = "txt"; // Nome da subpasta que você deseja acessar
            string subpastaConfig = "configs"; // Nome da subpasta que você deseja acessar
            string caminhoDaSubpastaTxts = Path.Combine(diretorioDoApp, subpastaTxt);
            string caminhoDaSubpastaConfig = Path.Combine(diretorioDoApp, subpastaConfig);

            //editar
            if (titulo != "" && conteudo != "")
            {
                string caminhoDoArquivo = Path.Combine(caminhoDaSubpastaTxts, titulo + ".txt");
                string caminhoDoArquivoConfig = Path.Combine(caminhoDaSubpastaConfig, "config" + titulo + ".txt");

                if (File.Exists(caminhoDoArquivo))
                {
                    DialogResult result = MessageBox.Show("O arquivo já existe. Deseja substituí-lo?", "Aviso", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                    if (result == DialogResult.Yes)
                    {
                        File.Delete(caminhoDoArquivo);
                        File.Delete(caminhoDoArquivoConfig);
                        salvar();
                    }
                    else
                    {
                        MessageBox.Show("Nenhuma alteração foi feita.");
                    }
                }
                else
                {
                    File.WriteAllText(caminhoDoArquivo, txtConteudo.Text);
                    MessageBox.Show("Arquivo criado com sucesso.");
                }
            }
            //criar novo
            else
            {
                salvar();
            }

            this.Close();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmCadastroNota_Load(object sender, EventArgs e)
        {
            txtConteudo.Text = conteudo;
            txtTitulo.Text = titulo;
            txtAlarme.Text = alarme;
        }

        private void txtTitulo_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Lista de caracteres inválidos em nomes de arquivo
            char[] invalidChars = Path.GetInvalidFileNameChars();

            // Verifique se o caractere digitado está na lista de caracteres inválidos (exceto o caractere de backspace)
            if (invalidChars.Contains(e.KeyChar) && e.KeyChar != '\b')
            {
                e.Handled = true; // Impede que o caractere seja inserido na TextBox

                // Exibe um aviso
                MessageBox.Show("Os seguintes caracteres são inválidos: <>:\"/\\|?*", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}

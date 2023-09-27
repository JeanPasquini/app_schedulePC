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
        public frmCadastroNota(string titulo, string conteudo)
        {
            this.titulo = titulo;
            this.conteudo = conteudo;
            InitializeComponent();

            
        }

        private void salvar()
        {
            string diretorioDoApp = AppDomain.CurrentDomain.BaseDirectory;

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Arquivos de texto (*.txt)|*.txt|Todos os arquivos (*.*)|*.*";
            saveFileDialog.FileName = txtTitulo.Text;

            // Define o diretório inicial como "txts" a partir do diretório onde o aplicativo está localizado


            saveFileDialog.InitialDirectory = diretorioDoApp;

            string filePath = saveFileDialog.FileName;

            if (!filePath.EndsWith(".txt", StringComparison.OrdinalIgnoreCase))
            {
                filePath += ".txt";
            }

            File.WriteAllText(filePath, txtConteudo.Text);

            MessageBox.Show("Arquivo atualizado com sucesso.");
        }

        private void btnCriarArquivotxt_Click(object sender, EventArgs e)
        {
            string diretorioDoApp = AppDomain.CurrentDomain.BaseDirectory;

            //editar
            if (titulo != "" && conteudo != "")
            {
                string caminhoDoArquivo = Path.Combine(diretorioDoApp, titulo);

                if (File.Exists(caminhoDoArquivo))
                {
                    DialogResult result = MessageBox.Show("O arquivo já existe. Deseja substituí-lo?", "Aviso", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                    if (result == DialogResult.Yes)
                    {
                        File.Delete(caminhoDoArquivo);
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
        }
    }
}

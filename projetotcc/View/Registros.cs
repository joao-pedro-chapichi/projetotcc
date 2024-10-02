using projetotcc.Controles_De_Usuario;
using projetotcc.Controller;
using projetotcc.Model;
using projetotcc.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace projetotcc.View
{
    public partial class Registros : Form
    {
        //registros
        public MenuLateral menuLateral;

        public Registros()
        {
            InitializeComponent();
            menuLateral = new MenuLateral(this);
            this.Controls.Add(menuLateral);
            menuLateral.Dock = DockStyle.Left;
            Redimensionar();
            comboBox1.SelectedIndex = 2;
            comboAtividade.SelectedIndex = 2;
        }

        #region NAVEGAÇÃO
        // Carregar formulário
        private void CarregarForm_form(object sender, EventArgs e)
        {
            labelNome.Focus();
        }

        // Fechar formulário
        private void pbFechar_form(object sender, EventArgs e)
        {
            UtilsClasse.ConfirmacaoFechar(this);
        }

        // Minimizar formulário
        private void pbMinimizar_form(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        // Voltar para formulário Gerenciamento
        private void pbVoltarGen_form(object sender, EventArgs e)
        {
            //utilizando o metodo(de forma estatica, não precisa instanciar) para fechar o form atual e abri o proximo
            Gerenciamento gerenciamento = new Gerenciamento();
            UtilsClasse.FecharEAbrirProximoForm(this, gerenciamento);
        }

        // Imprimir registros
        private void pbImprimir_form(object sender, EventArgs e)
        {
            // Ainda sem ação
        }
        #endregion

        private async void btnPesquisar_Click(object sender, EventArgs e)
        {
            await AtualizarRegistros();
        }

        private void Redimensionar()
        {
            UtilsClasse.RedimensionarLabel(this, labelTopo, 0.04f);

            float newSize = this.Width * 0.01f; // Ajusta o tamanho da fonte com base na largura do formulário
            dataGridView1.DefaultCellStyle.Font = new Font("Arial", newSize);
            UtilsClasse.AjustarFonteDataGridView(dataGridView1, 0.015f);
        }

        private async Task AtualizarRegistros()
        {
            dataGridView1.DataSource = null;
            try
            {
                ModelRegistro modelRegistro = new ModelRegistro();

                modelRegistro.DataInicio = dateTimePicker1.Value.Date;
                modelRegistro.DataFim = dateTimePicker2.Value.Date;

                // Verifica se o textBox1 não está vazio antes de buscar o ID
                if (!string.IsNullOrEmpty(labelNome.Text))
                {
                    modelRegistro.Id = await ControllerColaborador.PesquisarCodigoPorNome(labelNome.Text);
                }

                // Verifica se a combobox não está vazia ou nula
                if (!string.IsNullOrEmpty(comboBox1.SelectedItem?.ToString()) && comboBox1.SelectedItem.ToString() != "TODOS")
                {
                    modelRegistro.Acao = comboBox1.SelectedItem?.ToString().ToLower();
                    MessageBox.Show("a");
                }

                try
                {
                    string atividade = comboAtividade.SelectedItem.ToString();
                    DataTable dataTable = new DataTable();
                    if(comboAtividade.SelectedItem.ToString() == "TODOS")
                    {
                        dataTable = await ControllerRegistro.PesquisaRegistro(modelRegistro);
                    }
                    else
                    {
                        dataTable = await ControllerRegistro.PesquisaRegistro(modelRegistro, atividade.ToUpper());
                    }
                    

                    if (dataTable != null && dataTable.Rows.Count > 0)
                    {
                        // Adiciona a fonte de dados à DataGridView
                        dataGridView1.DataSource = dataTable;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Erro ao pesquisar registros: {ex.Message}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro inesperado: {ex.Message}");
            }
        }

        private async void checkUsuariosInativos_CheckedChanged(object sender, EventArgs e)
        {
            if (checkUsuariosInativos.Checked)
            {
                await AtualizarRegistros();
            }
            else
            {
                dataGridView1.DataSource = null;
            }
        }

        private void btnCriarRelatorio_Click(object sender, EventArgs e)
        {
            CriarRelatorio criarRelatorio = new CriarRelatorio();
            UtilsClasse.FecharEAbrirProximoForm(this, criarRelatorio);
        }

        private void txbNome_ValidacaoTecla(object sender, KeyPressEventArgs e)
        {
            // Lista de caracteres acentuados permitidos
            char[] allowedChars = { 'é', 'è', 'ê', 'ë', 'à', 'â', 'ä', 'á', 'ò', 'ô', 'ö', 'ó', 'ù', 'û', 'ü', 'ú', ' ' };

            // Verifica se a tecla pressionada é uma letra, uma letra acentuada específica ou tecla de controle
            if (!char.IsLetter(e.KeyChar) && !char.IsControl(e.KeyChar) && !Array.Exists(allowedChars, c => c == e.KeyChar))
            {
                e.Handled = true; // Impede a entrada de qualquer caractere que não seja letra, letra acentuada específica ou tecla de controle
            }
        }

        // Outros eventos como CarregarForm_form, pbFechar_form, pbMinimizar_form, etc., podem continuar conforme você já os implementou
    }
}

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
        public Registros()
        {
            InitializeComponent();
        }

        #region NAVEGAÇÃO
        // Carregar formulário
        private void CarregarForm_form(object sender, EventArgs e)
        {
            textBox1.Focus();
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

        private async Task AtualizarRegistros()
        {
            try
            {
                ModelRegistro modelRegistro = new ModelRegistro();

                modelRegistro.DataInicio = dateTimePicker1.Value.Date;
                modelRegistro.DataFim = dateTimePicker2.Value.Date;

                // Verifica se o textBox1 não está vazio antes de buscar o ID
                if (!string.IsNullOrEmpty(textBox1.Text))
                {
                    modelRegistro.Id = await ControllerColaborador.PesquisarCodigoPorNome(textBox1.Text);
                }

                // Verifica se a combobox não está vazia ou nula
                if (!string.IsNullOrEmpty(comboBox1.SelectedItem?.ToString()))
                {
                    modelRegistro.Acao = comboBox1.SelectedItem?.ToString().ToLower();
                }

                try
                {
                    // Chamar o método de pesquisa de registro no Controller
                    DataTable dataTable = await ControllerRegistro.PesquisaRegistro(modelRegistro);

                    if (dataTable != null && dataTable.Rows.Count > 0)
                    {
                        // Adiciona a fonte de dados à DataGridView
                        dataGridView1.DataSource = dataTable;
                    }
                    else
                    {
                        MessageBox.Show("Não foram encontrados registros.");
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


        private void button2_Click(object sender, EventArgs e)
        {
            CriarRelatorio criarRelatorio = new CriarRelatorio();
            UtilsClasse.FecharEAbrirProximoForm(this, criarRelatorio);
        }

        // Outros eventos como CarregarForm_form, pbFechar_form, pbMinimizar_form, etc., podem continuar conforme você já os implementou
    }
}

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

        private string _idSelecionado;
        private string _horaSelecionada;
        private string _dataSelecionada;
        private string _acaoSelecionada;
        private string _nomeFuncionario;

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
                modelRegistro.Id = await ControllerColaborador.PesquisarCodigoPorNome(textBox1.Text);


                modelRegistro.DataInicio = dateTimePicker1.Value;
                modelRegistro.DataFim = dateTimePicker2.Value;




                modelRegistro.Acao = comboBox1.SelectedItem?.ToString().ToLower();

                try
                {
                    // Chamar o método de pesquisa de registro no Controller
                    DataTable dataTable = await ControllerRegistro.PesquisaRegistro(modelRegistro);


                    if (dataTable != null && dataTable.Rows.Count > 0)
                    {
                        // Adiciona a fonte de dados à DataGridView
                        dataGridView1.DataSource = dataTable;
                        dataGridView1.Columns["id"].HeaderText = "Id de Sistema";
                        dataGridView1.Columns["hora"].HeaderText = "Horário";
                        dataGridView1.Columns["data"].HeaderText = "Data";
                        dataGridView1.Columns["acao"].HeaderText = "Ação";
                        // Deseleciona a primeira linha
                    }
                    else
                    {
                        MessageBox.Show("Não foram encontrados registros.");
                    }

                    // Aqui você pode atualizar sua interface com os resultados, se necessário
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

        private void dgvVerificar_celula(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

                // Supondo que as colunas são ID, Nome, Cargo, Email
                string id = row.Cells["id"].Value.ToString();
                string hora = row.Cells["hora"].Value.ToString();
                string data = row.Cells["data"].Value.ToString();
                string acao = row.Cells["acao"].Value.ToString().ToUpper();
                string nome = textBox1.Text.ToUpper();

                // Armazena os dados em variáveis de instância (opcional)
                _idSelecionado = id;
                _horaSelecionada = hora;
                _dataSelecionada = data;
                _acaoSelecionada = acao;
                _nomeFuncionario = nome;
            }
        }

        // Gerar pdf dos registros
        private void pbImprimir_form(object sender, EventArgs e)
        {
            try
            {
                // Verifica se há um funcionário selecionado
                if (!string.IsNullOrEmpty(_idSelecionado))
                {
                    // Instancia a classe PdfGenerator
                    ControllerPDF pdfGenerator = new ControllerPDF();

                    // Chama o método para gerar o PDF com os dados do funcionário selecionado
                    pdfGenerator.GerarPdf(dataGridView1, _nomeFuncionario, _idSelecionado);

                    MessageBox.Show("PDF gerado com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Por favor, selecione um funcionário na tabela.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao gerar o PDF: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }
}

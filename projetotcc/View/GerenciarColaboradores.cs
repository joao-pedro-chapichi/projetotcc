using projetotcc.Controller;
using projetotcc.Model;
using projetotcc.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace projetotcc.View
{
    public partial class GerenciarColaboradores : Form
    {
        public GerenciarColaboradores()
        {
            InitializeComponent();
            AtualizarDados();
            dataGridView1.CellContentClick += dataGridView1_CellContentClick;
            ModelFuncionario mFun = new ModelFuncionario();
        }

        #region NAVEGAÇÃO
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

        // Voltar ao formulário gerenciamento
        private void pbVoltarGen_form(object sender, EventArgs e)
        {
            //utilizando o metodo(de forma estatica, não precisa instanciar) para fechar o form atual e abri o proximo
            Gerenciamento gerenciamento = new Gerenciamento();
            UtilsClasse.FecharEAbrirProximoForm(this, gerenciamento);

        }

        // Cadastrar novo colaborador
        private void cadastrarCol_form(object sender, EventArgs e)
        {
            //utilizando o metodo(de forma estatica, não precisa instanciar) para fechar o form atual e abri o proximo
            CadastrarColaborador cadastrarColaborador = new CadastrarColaborador();
            UtilsClasse.FecharEAbrirProximoForm(this, cadastrarColaborador);
        }

        public async void AtualizarDados()
        {
            dataGridView1.Columns.Clear();
            try
            {
                string nome = txtNome.Text;
                string codigo = txtCodigo.Text;
                DataTable dataTable = await ControllerColaborador.buscasFuncionarios(nome, codigo);
                PreencherDataGrindView(dataTable);
            }
            catch
            {
                return;
            }
        }

        private void PreencherDataGrindView(DataTable dataTable)
        {
            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                dataGridView1.DataSource = dataTable;

                if (dataGridView1.Rows.Count > 0)
                {
                    dataGridView1.Rows[0].Selected = false; // Desseleciona a primeira linha
                }

                // Verifica se as colunas de botões EXCLUIR e EDITAR já existem
                bool deleteButtonExists = false;
                bool editButtonExists = false;

                foreach (DataGridViewColumn column in dataGridView1.Columns)
                {
                    if (column.HeaderText == "EXCLUIR")
                    {
                        deleteButtonExists = true;
                    }
                    else if (column.HeaderText == "EDITAR")
                    {
                        editButtonExists = true;
                    }
                }

                // Se as colunas de botões não existirem, adiciona-as
                if (!deleteButtonExists)
                {
                    DataGridViewButtonColumn deleteButtonColumn = new DataGridViewButtonColumn();
                    deleteButtonColumn.HeaderText = "EXCLUIR";
                    deleteButtonColumn.Text = "EXCLUIR";
                    deleteButtonColumn.UseColumnTextForButtonValue = true;
                    dataGridView1.Columns.Add(deleteButtonColumn);
                }

                if (!editButtonExists)
                {
                    DataGridViewButtonColumn editButtonColumn = new DataGridViewButtonColumn();
                    editButtonColumn.HeaderText = "EDITAR";
                    editButtonColumn.Text = "EDITAR";
                    editButtonColumn.UseColumnTextForButtonValue = true;
                    dataGridView1.Columns.Add(editButtonColumn);

                }
            }
        }
        #endregion

        private async void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0) // Verifica se o clique ocorreu em uma célula válida
            {
                if (dataGridView1.Columns[e.ColumnIndex] is DataGridViewButtonColumn) // Verifica se o clique ocorreu em uma coluna de botão
                {
                    if (dataGridView1.Columns[e.ColumnIndex].HeaderText == "EXCLUIR") // Verifica se o botão clicado é o de EXCLUIR
                    {
                        int codigo = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["id_funcionario"].Value.ToString());

                        DialogResult result = MessageBox.Show("Deseja continuar?", "Confirmação", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                        if (result == DialogResult.Yes)
                        {
                            await ControllerColaborador.ExcluirFuncionario(codigo);
                            AtualizarDados();

                        }
                    }
                    //else if (dataGridView1.Columns[e.ColumnIndex].HeaderText == "EDITAR") // Verifica se o botão clicado é o de EDITAR
                    //{
                    //    string nome = dataGridView1.Rows[e.RowIndex].Cells["nome"].Value.ToString();
                    //    string cpf = dataGridView1.Rows[e.RowIndex].Cells["cpf"].Value.ToString();
                    //    string celular = dataGridView1.Rows[e.RowIndex].Cells["celular"].Value.ToString();
                    //    string habilitacao = dataGridView1.Rows[e.RowIndex].Cells["habilitacao"].Value.ToString();

                    //    EditarClienteForms editarCliente = new EditarClienteForms(nomeUser, nome, cpf, celular, habilitacao);
                    //    suport.FecharEAbrirProximoForm(this, editarCliente);
                    //}
                }
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            AtualizarDados();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            AtualizarDados();
        }
    }
}

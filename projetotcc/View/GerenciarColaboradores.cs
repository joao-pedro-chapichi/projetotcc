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
            InitializeComponent(); // Inicializa os componentes do formulário
            AtualizarDados(); // Atualiza os dados do DataGridView ao inicializar o formulário
            dataGridView1.CellContentClick += dataGridView1_CellContentClick; // Adiciona um evento para o clique nas células do DataGridView
            ModelFuncionario mFun = new ModelFuncionario(); // Instancia um novo objeto ModelFuncionario (não utilizado no construtor)
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
            dataGridView1.Columns.Clear(); // Limpa todas as colunas do DataGridView
            try
            {
                string nome = txtNome.Text; // Obtém o texto do campo txtNome
                string codigo = txtCodigo.Text; // Obtém o texto do campo txtCodigo
                DataTable dataTable = await ControllerColaborador.buscasFuncionarios(nome, codigo); // Busca os funcionários com base nos filtros
                PreencherDataGrindView(dataTable); // Preenche o DataGridView com os dados retornados
            }
            catch
            {
                return; // Em caso de erro, apenas retorna sem fazer nada
            }
        }

        private void PreencherDataGrindView(DataTable dataTable)
        {
            if (dataTable != null && dataTable.Rows.Count > 0) // Verifica se o DataTable possui dados
            {
                dataGridView1.DataSource = dataTable; // Define a fonte de dados do DataGridView

                if (dataGridView1.Rows.Count > 0)
                {
                    dataGridView1.Rows[0].Selected = false; // Desseleciona a primeira linha, se existir
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

        // Evento de clique no conteúdo da célula do DataGridView
        private async void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0) // Verifica se o clique ocorreu em uma célula válida
            {
                if (dataGridView1.Columns[e.ColumnIndex] is DataGridViewButtonColumn) // Verifica se o clique ocorreu em uma coluna de botão
                {
                    if (dataGridView1.Columns[e.ColumnIndex].HeaderText == "EXCLUIR") // Verifica se o botão clicado é o de EXCLUIR
                    {
                        int codigo = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["id_funcionario"].Value.ToString()); // Obtém o código do funcionário da linha clicada
                        string nome = dataGridView1.Rows[e.RowIndex].Cells["nome"].Value.ToString();

                        DialogResult result = MessageBox.Show("Deseja continuar?", "Confirmação", MessageBoxButtons.YesNo, MessageBoxIcon.Question); // Exibe uma caixa de diálogo para confirmação

                        if (result == DialogResult.Yes)
                        {
                            

                            DialogResult resultado = MessageBox.Show("Deseja excluir os registros do funcionario?", "Confirmação", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                            if(resultado == DialogResult.Yes)
                            {
                                int cod = await ControllerColaborador.PesquisarCodigoPorNome(nome);
                                MessageBox.Show(cod.ToString());
                                string res =  await ControllerRegistro.ExcluirRegistros(cod);
                                MessageBox.Show(res);
                            }
                            string ress = await ControllerColaborador.ExcluirFuncionario(codigo); // Exclui o funcionário de forma assíncrona
                            MessageBox.Show(ress);
                            AtualizarDados(); // Atualiza os dados do DataGridView após a exclusão
                        }
                    }
                    else if (dataGridView1.Columns[e.ColumnIndex].HeaderText == "EDITAR") // Verifica se o botão clicado é o de EDITAR
                    {
                        int id_fun = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["id_funcionario"].Value.ToString()); // Obtém o código do funcionário da linha clicada
                        string nome_fun = dataGridView1.Rows[e.RowIndex].Cells["nome"].Value.ToString(); // Obtém o nome do funcionário da linha clicada

                        AlterarColaborador editarCliente = new AlterarColaborador(id_fun, nome_fun); // Cria uma nova instância do formulário AlterarColaborador
                        UtilsClasse.FecharEAbrirProximoForm(this, editarCliente); // Utiliza um método utilitário para fechar o formulário atual e abrir o próximo
                    }
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

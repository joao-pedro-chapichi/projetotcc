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

        public void AtualizarDados()
        {
            ModelFuncionario modelFuncionario = new ModelFuncionario();
            dataGridView1.Columns.Clear();

            string nome = textBox1.Text;
            int codigo;

            // Verifica se o valor do código é um inteiro válido
            if (!int.TryParse(textBox2.Text, out codigo))
            {
                MessageBox.Show("Por favor, insira um valor válido para o código.");
                return;
            }

            // Definindo os campos e valores para pesquisa
            Dictionary<string, object> camposValores = new Dictionary<string, object>();
            camposValores.Add("Nome", nome);
            camposValores.Add("Id_funcionario", codigo);

            // Chamando a função Listar() do controlador para buscar os dados da tabela
            DataTable dataTable = ControllerAll.Listar("funcionario", new string[] { "*" }, camposValores.Keys.ToArray(), camposValores.Values.ToArray());

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

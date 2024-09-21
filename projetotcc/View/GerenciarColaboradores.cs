﻿using projetotcc.Controles_De_Usuario;
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
        public MenuLateral menuLateral;

        public GerenciarColaboradores()
        {
            InitializeComponent(); // Inicializa os componentes do formulário
            menuLateral = new MenuLateral(this);
            this.Controls.Add(menuLateral);
            menuLateral.Dock = DockStyle.Left;
            Redimensionar();
            AtualizarDados(); // Atualiza os dados do DataGridView ao inicializar o formulário
            dataGridView1.CellContentClick += dataGridView1_CellContentClick; // Adiciona um evento para o clique nas células do DataGridView
            ModelFuncionario mFun = new ModelFuncionario(); // Instancia um novo objeto ModelFuncionario (não utilizado no construtor)

        }

        private void Redimensionar()
        {
            UtilsClasse.RedimensionarLabel(this, labelTopo, 0.04f);
            UtilsClasse.RedimensionarLabel(this, labelNome, 0.015f);
            UtilsClasse.RedimensionarLabel(this, labelCodigo, 0.015f);
            float newSize = this.Width * 0.01f; // Ajusta o tamanho da fonte com base na largura do formulário
            dataGridView1.DefaultCellStyle.Font = new Font("Arial", newSize);
            UtilsClasse.AjustarFonteDataGridView(dataGridView1, 0.015f);
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
            cadastrarColaborador.Show();
        }

        #endregion

        #region CODIGOS

        private void PreencherDataGrindView(DataTable dataTable)
        {
            if (dataTable != null && dataTable.Rows.Count > 0) // Verifica se o DataTable possui dados
            {
                dataGridView1.DataSource = dataTable; // Define a fonte de dados do DataGridView
            }
        }

        public async void AtualizarDados()
        {
            try
            {
                string nome = txtNome.Text; // Obtém o texto do campo txtNome
                string codigo = txtCodigo.Text; // Obtém o texto do campo txtCodigo
                string cpf = txtCPF.Text;
                DataTable dataTable = new DataTable();
                if (checkPesquisaTotal.Checked)
                {
                   dataTable  = await ControllerColaborador.buscasFuncionarios(nome, codigo, cpf); // Busca os funcionários com base nos filtros
                }
                else
                {
                    dataTable = await ControllerColaborador.buscasFuncionarios(nome, codigo, cpf, "ativo");
                }
                PreencherDataGrindView(dataTable); // Preenche o DataGridView com os dados retornados
            }
            catch
            {
                return; // Em caso de erro, apenas retorna sem fazer nada
            }
        }

        // Evento de clique no conteúdo da célula do DataGridView
        private async void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return; // Se o clique não foi em uma célula válida, retorna

            var clickedColumn = dataGridView1.Columns[e.ColumnIndex]; // Obtém a coluna clicada

            if (clickedColumn is DataGridViewButtonColumn) // Verifica se é uma coluna de botão
            {
                string headerText = clickedColumn.HeaderText;

                int idFuncionario = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["id_funcionario"].Value);
                string nomeFuncionario = dataGridView1.Rows[e.RowIndex].Cells["nome"].Value.ToString();
                string cpfFuncionario = dataGridView1.Rows[e.RowIndex].Cells["cpf"].Value.ToString();

                if (headerText == "ALTERAR ESTADO")
                {
                    await AlterarEstadoFuncionario(idFuncionario); // Chama método assíncrono para alterar o estado
                }
                else if (headerText == "EDITAR")
                {
                    EditarFuncionario(idFuncionario, nomeFuncionario, cpfFuncionario); // Chama método para editar funcionário
                }
            }
        }

        // Método para alterar o estado do funcionário
        private async Task AlterarEstadoFuncionario(int idFuncionario)
        {
            DialogResult result = MessageBox.Show("Deseja continuar?", "Confirmação", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                string resposta = await ControllerColaborador.InativarFuncionario(idFuncionario); // Chama a função assíncrona
                MessageBox.Show(resposta);
                AtualizarDados(); // Atualiza os dados no DataGridView
            }
        }

        // Método para abrir o formulário de edição do funcionário
        private void EditarFuncionario(int idFuncionario, string nomeFuncionario, string cpf)
        {
            AlterarColaborador formEditar = new AlterarColaborador(idFuncionario, nomeFuncionario, cpf); // Cria o form de edição
            formEditar.Show(); // Abre o novo form
        }


        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            AtualizarDados();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            AtualizarDados();
        }

        #endregion

        private void checkPesquisaTotal_CheckedChanged(object sender, EventArgs e)
        {
            AtualizarDados();
        }

        private void GerenciarColaboradores_SizeChanged(object sender, EventArgs e)
        {
            Redimensionar();
        }

        private void txtCPF_TextChanged(object sender, EventArgs e)
        {
            AtualizarDados();
        }
    }
}

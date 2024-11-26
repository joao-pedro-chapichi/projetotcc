using projetotcc.Controles_De_Usuario;
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

        private void GerenciarColaboradores_Load(object sender, EventArgs e)
        {
            AtualizarDados();
        }

        private void Redimensionar()
        {
            UtilsClasse.RedimensionarLabel(this, labelTopo, 0.04f);
            UtilsClasse.RedimensionarLabel(this, labelNome, 0.01f);
            UtilsClasse.RedimensionarLabel(this, labelCodigo, 0.01f);
            UtilsClasse.RedimensionarLabel(this, labelCpf, 0.01f);
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



        #endregion

        #region CODIGOS

        // Cadastrar novo colaborador
        private void cadastrarCol_form(object sender, EventArgs e)
        {
            //utilizando o metodo(de forma estatica, não precisa instanciar) para fechar o form atual e abri o proximo
            CadastrarColaborador cadastrarColaborador = new CadastrarColaborador();
            cadastrarColaborador.ShowDialog();
        }

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
                    dataTable = await ControllerColaborador.buscasFuncionarios(nome, codigo, cpf, "ATIVO");
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

                int idFuncionario = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["CODIGO"].Value);
                string nomeFuncionario = dataGridView1.Rows[e.RowIndex].Cells["NOME"].Value.ToString();
                string cpfFuncionario = dataGridView1.Rows[e.RowIndex].Cells["CPF"].Value.ToString();

                if (headerText == "ESTADO")
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
            DialogResult result = MessageBox.Show("Deseja alterar o estado de atividade do Colaborador?", "Confirmação", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

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
            formEditar.ShowDialog(); // Abre o novo form
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

        private void lbLkComopesquisarcolaborador(object sender, EventArgs e)
        {
            MessageBox.Show("Digite nos campos acima:\nNome: <nome do colaborador igual ao cadastrado>\nCodigo: <codigo do colaborador>\nCPF: <cpf do colaborador cadastrado>\nAs pesquisas são independentes, caso esqueça uma informação, preencha o restante. Para uma pesquisa mais precisa, preencha todos os campos.", "Como pesquisar um colaborador?", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void validacaoTecla_Nome(object sender, KeyPressEventArgs e)
        {
            // Lista de caracteres acentuados permitidos
            char[] allowedChars = { 'é', 'è', 'ê', 'ë', 'à', 'â', 'ä', 'á', 'ò', 'ô', 'ö', 'ó', 'ù', 'û', 'ü', 'ú', ' ' };

            // Verifica se a tecla pressionada é uma letra, uma letra acentuada específica ou tecla de controle
            if (!char.IsLetter(e.KeyChar) && !char.IsControl(e.KeyChar) && !Array.Exists(allowedChars, c => c == e.KeyChar))
            {
                e.Handled = true; // Impede a entrada de qualquer caractere que não seja letra, letra acentuada específica ou tecla de controle
            }
        }

        private void validacaoTecla_Codigo(object sender, KeyPressEventArgs e)
        {
            // Verifica se a tecla pressionada é um dígito ou a tecla de controle (como backspace)
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true; // Impede a entrada de qualquer caractere que não seja número
            }
        }

        private void validacaoTecla_cpf(object sender, KeyPressEventArgs e)
        {
            // Verifica se a tecla pressionada é um dígito ou a tecla de controle (como backspace)
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true; // Impede a entrada de qualquer caractere que não seja número
            }
        }

        private void btnAtualizar_Click(object sender, EventArgs e)
        {
            AtualizarDados();
        }
    }
}

using projetotcc.View;
using projetotcc.Utils;
using System;
using System.Windows.Forms;
using projetotcc.Controller;
using projetotcc.Model;
using System.Threading.Tasks;
using System.Data;

namespace projetotcc
{
    public partial class Inicial : Form
    {
        #region VARIÁVEIS E CAMPOS PRIVADOS
        private bool aguardandoConfirmacao = false; // Indica se o sistema está aguardando confirmação
        private Timer timer; // Timer usado para controlar o tempo de espera
        #endregion

        #region CONSTRUTOR
        public Inicial()
        {
            InitializeComponent();
            InitializeTimer(); // Inicializa o timer
            AtualizarRegistrosDeHoje().ConfigureAwait(false); // Atualiza os registros do dia
        }
        #endregion

        #region TIMER
        // Evento chamado a cada intervalo do Timer
        private void Timer_Tick(object sender, EventArgs e)
        {
            if (aguardandoConfirmacao)
            {
                // Reativa os controles
                foreach (Control control in this.Controls)
                {
                    control.Enabled = true;
                }

                // Para o timer
                timer.Stop();

                // Limpa o textbox e foca nele novamente
                txbcodigo_inicial.Text = "";
                txbcodigo_inicial.Focus();

                aguardandoConfirmacao = false; // Reseta a flag de confirmação
            }
        }

        // Inicializa o Timer com um intervalo de 1 segundo
        private void InitializeTimer()
        {
            timer = new Timer();
            timer.Interval = 1000; // Intervalo definido em milissegundos
            timer.Tick += Timer_Tick;
        }
        #endregion

        #region NAVEGAÇÃO
        // Evento para fechar o formulário
        private void pbFechar_form(object sender, EventArgs e)
        {
            UtilsClasse.ConfirmacaoFechar(this);
        }

        // Evento para abrir o formulário de gerenciamento
        private void pbAbrir_gerenciamento(object sender, EventArgs e)
        {
            Gerenciamento gerenciamento = new Gerenciamento();
            UtilsClasse.FecharEAbrirProximoForm(this, gerenciamento);
        }

        // Evento para focar no campo de código ao carregar o formulário
        private void carregarForm_form(object sender, EventArgs e)
        {
            txbcodigo_inicial.Focus();
        }
        #endregion

        #region REGISTRO DE PONTO ELETRÔNICO
        // Evento para registrar o ponto eletrônico ao pressionar Enter
        private async void pontoEletronico(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                // Verifica se o campo está vazio ou contém um valor inválido
                if (string.IsNullOrWhiteSpace(txbcodigo_inicial.Text))
                {
                    Console.Beep(2000, 500); // Emite um som de erro
                    return;
                }

                if (!long.TryParse(txbcodigo_inicial.Text, out long codigo))
                {
                    Console.Beep(2000, 500); // Emite um som de erro
                    txbcodigo_inicial.Text = "";
                    txbcodigo_inicial.Focus();
                    return;
                }

                aguardandoConfirmacao = true; // Sinaliza que está aguardando confirmação

                long id = await ControllerRegistro.BuscarCodigoPorCodigoDeBarras(codigo);
                bool existencia = await ControllerRegistro.VerificarExistencia(id);

                if (existencia)
                {
                    string nome = await ControllerRegistro.BuscarNomeFuncionario(id);

                    var mFunc = new ModelFuncionario
                    {
                        ID = id,
                        Nome = nome,
                        Id_funcionario = codigo
                    };

                    string success = await ControllerRegistro.CriarRegistro(mFunc);
                    await AtualizarRegistrosDeHoje();

                    // Desativa os controles durante a confirmação
                    foreach (Control control in this.Controls)
                    {
                        control.Enabled = false;
                    }

                    // Emite um som de confirmação
                    Console.Beep(1000, 500);
                    Console.Beep(1000, 500);
                    Console.Beep(1000, 500);

                    timer.Start(); // Inicia o timer para reativar os controles
                }
                else
                {
                    Console.Beep(2000, 500); // Emite um som de erro
                    txbcodigo_inicial.Text = "";
                    timer.Start();
                    txbcodigo_inicial.Focus();
                }
            }
        }
        #endregion

        #region ATUALIZAÇÃO DOS REGISTROS
        // Atualiza a tabela com os registros do dia
        private async Task AtualizarRegistrosDeHoje()
        {
            try
            {
                DataTable dataTable = await ControllerRegistro.PesquisaRegistroHoje();

                if (dataTable.Rows.Count == 0)
                {
                    // Adiciona uma linha com a mensagem "Nenhum registro referente a Hoje"
                    dataGridView1.Columns.Add("Mensagem", "Aviso");
                    dataGridView1.Rows.Add("Nenhum registro referente a Hoje");
                }
                else
                {
                    // Limpa as colunas e linhas anteriores se houver registros
                    if (dataGridView1.Columns.Count == 1)
                    {
                        dataGridView1.Rows.Clear();
                        dataGridView1.Columns.Clear();
                    }

                    // Atualiza o DataGridView com os dados
                    dataGridView1.DataSource = dataTable;
                    dataGridView1.Columns[0].HeaderText = "Horário";
                    dataGridView1.Columns[1].HeaderText = "Data";
                    dataGridView1.Columns[2].HeaderText = "Funcionário";
                    dataGridView1.Columns[3].HeaderText = "Código";
                    dataGridView1.Columns[4].HeaderText = "Ação";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao pesquisar registros: {ex.Message}");
            }
        }
        #endregion

        #region EVENTOS DO FORMULÁRIO
        // Evento ao carregar o formulário, foca no campo de código e adiciona o evento KeyDown
        private void Inicial_Load(object sender, EventArgs e)
        {
            txbcodigo_inicial.KeyDown += pontoEletronico;
            txbcodigo_inicial.Focus();
        }

        // Evento para restringir o input do campo de código a apenas números
        private void txbcodigo_inicial_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true; // Impede a entrada de qualquer caractere que não seja número
            }
        }
        #endregion
    }
}

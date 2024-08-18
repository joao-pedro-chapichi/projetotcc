using projetotcc.View;
using projetotcc.Utils;
using System;
using System.Windows.Forms;
using projetotcc.Controller;
using projetotcc.Model;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Data;

namespace projetotcc
{
    public partial class Inicial : Form
    {
        private bool aguardandoConfirmacao = false;
        private Timer timer;

        public Inicial()
        {
            InitializeComponent();
            InitializeTimer();
            AtualizarRegistrosDeHoje().ConfigureAwait(false);
            
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (aguardandoConfirmacao)
            {
                // Re-enable controls
                foreach (Control control in this.Controls)
                {
                    control.Enabled = true;
                }

                // Stop the timer
                timer.Stop();

                // Clear textbox and set focus
                txbcodigo_inicial.Text = "";
                txbcodigo_inicial.Focus();

                aguardandoConfirmacao = false; // Reset the flag
            }
        }

        private void InitializeTimer()
        {
            timer = new Timer();
            timer.Interval = 1000; // Defina o intervalo em milissegundos (3 segundos neste caso)
            timer.Tick += Timer_Tick;
        }

        // Aqui estão os eventos para fechar o formulario, abrir novos e outros
        #region NAVEGAÇÃO
        private void pbFechar_form(object sender, EventArgs e)
        {
            UtilsClasse.ConfirmacaoFechar(this);
        }

        private void pbAbrir_gerenciamento(object sender, EventArgs e)
        {
            Gerenciamento gerenciamento = new Gerenciamento();
            UtilsClasse.FecharEAbrirProximoForm(this, gerenciamento);
        }

        private void carregarForm_form(object sender, EventArgs e)
        {
            txbcodigo_inicial.Focus();
        }
        #endregion

        private async void pontoEletronico(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                // Handling empty or invalid input
                if (string.IsNullOrWhiteSpace(txbcodigo_inicial.Text))
                {
                    MessageBox.Show("Por favor, insira um código.");
                    return;
                }

                if (!long.TryParse(txbcodigo_inicial.Text, out long codigo))
                {
                    MessageBox.Show("Por favor, insira um número válido.");
                    txbcodigo_inicial.Text = "";
                    txbcodigo_inicial.Focus();
                    return;
                }

                aguardandoConfirmacao = true;

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

                    // Disable controls
                    foreach (Control control in this.Controls)
                    {
                        control.Enabled = false;
                    }

                    // Start the timer
                    timer.Start();
                }
                else
                {
                    MessageBox.Show("Colaborador não Encontrado!");
                    txbcodigo_inicial.Text = "";
                    timer.Start();
                    txbcodigo_inicial.Focus();

                }
            }
        }

        private async Task AtualizarRegistrosDeHoje()
        {
            try
            {
                try
                {
                    // Chamar o método de pesquisa de registro no Controller
                    DataTable dataTable = await ControllerRegistro.PesquisaRegistroHoje();


                    if (dataTable.Rows.Count == 0)
                    {
                        // Adiciona uma linha com a mensagem "Nenhum registro referente a Hoje"
                        dataGridView1.Columns.Add("Mensagem", "Aviso");
                        dataGridView1.Rows.Add("Nenhum registro referente a Hoje");
                    }
                    else
                    {
                        if (dataGridView1.Columns.Count == 1)
                        {
                            dataGridView1.Rows.Clear();
                            dataGridView1.Columns.Clear();
                        }
            
                        dataGridView1.DataSource = dataTable;
                        dataGridView1.Columns[0].HeaderText = "HORÁRIO";
                        dataGridView1.Columns[1].HeaderText = "DATA";
                        dataGridView1.Columns[2].HeaderText = "FUNCIONARIO";
                        dataGridView1.Columns[3].HeaderText = "AÇÃO";

                        foreach(var item in dataTable.Rows)
                        {
                            
                        }
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

        private void Inicial_Load(object sender, EventArgs e)
        {
            txbcodigo_inicial.KeyDown += pontoEletronico;
            txbcodigo_inicial.Focus();
        }
    }
}

using projetotcc.View;
using projetotcc.Utils;
using System;
using System.Windows.Forms;
using projetotcc.Controller;
using projetotcc.Model;

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
            timer.Interval = 3000; // Defina o intervalo em milissegundos (3 segundos neste caso)
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
                    txbcodigo_inicial.Focus();
                }
            }
        }

        private void Inicial_Load(object sender, EventArgs e)
        {
            txbcodigo_inicial.KeyDown += pontoEletronico;
            txbcodigo_inicial.Focus();
        }
    }
}

using projetotcc.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace projetotcc.View
{
    public partial class Gerenciamento : Form
    {
        private Form forms;
        private Form formAnterior;

        public Gerenciamento(Form forms, Form formAnterior)
        {
            this.formAnterior = formAnterior;
            this.forms = forms;
            InitializeComponent();
            Timer timer = new Timer();
            timer.Interval = 10;
            timer.Tick += horaData_tick;
            timer.Start();
        }

        #region NAVEGAÇÃO E GERAIS
        // Eventos ao carregar formulário
        private void frmCarregar_form(object sender, EventArgs e)
        {
            // Ainda sem ação
        }

        // Fechar formulário
        private void pbFechar_form(object sender, EventArgs e)
        {
            UtilsClasse utils = new UtilsClasse();
            utils.confirmacaoFechar(this);
        }

        // Minimizar formulário
        private void pbMinimizar_form(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        // Voltar para form do ponto
        private void pbVoltarPonto_form(object sender, EventArgs e)
        {
            this.Hide();
            var Inicial = new Inicial(this, null);
            Inicial.Closed += (s, args) => this.Close();
            Inicial.Show();
        }


        // Abrir formulário - Codigo de Barras
        private void pbAbrirCodBar_form(object sender, EventArgs e)
        {
            this.Hide();
            var CodigoBarras = new CodigoBarras(this, null);
            CodigoBarras.Closed += (s, args) => this.Close();
            CodigoBarras.Show();
        }

        // Abrir formulário - Registros
        private void pbAbrirRegis_form(object sender, EventArgs e)
        {
            this.Hide();
            var Registros = new Registros(this, null);
            Registros.Closed += (s, args) => this.Close();
            Registros.Show();
        }

        // Abrir formulário - Gerenciar Colaboradores
        private void pbAbrirGenCol_form(object sender, EventArgs e)
        {
            this.Hide();
            var GerenciarColaboradores = new GerenciarColaboradores(this, null);
            GerenciarColaboradores.Closed += (s, args) => this.Close();
            GerenciarColaboradores.Show();
        }

        private void horaData_tick(object sender, EventArgs e)
        {
            DateTime horaDataUtc = DateTime.UtcNow;
            TimeZoneInfo brasiliaHoraData = TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time");
            DateTime brasiliaAtual = TimeZoneInfo.ConvertTimeFromUtc(horaDataUtc, brasiliaHoraData);
            label1.Text = brasiliaAtual.ToString("HH:mm:ss - dd/MM/yyyy");
        }
        #endregion


    }
}

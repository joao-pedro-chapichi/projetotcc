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
        public Gerenciamento()
        {
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
            UtilsClasse.ConfirmacaoFechar(this);
        }

        // Minimizar formulário
        private void pbMinimizar_form(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        // Voltar para form do ponto
        private void pbVoltarPonto_form(object sender, EventArgs e)
        {
            //utilizando o metodo(de forma estatica, não precisa instanciar) para fechar o form atual e abri o proximo
            Inicial inicial = new Inicial();    
            UtilsClasse.FecharEAbrirProximoForm(this, inicial);
        }


        // Abrir formulário - Codigo de Barras
        private void pbAbrirCodBar_form(object sender, EventArgs e)
        {
            //utilizando o metodo(de forma estatica, não precisa instanciar) para fechar o form atual e abri o proximo
            CodigoBarras codigoBarras = new CodigoBarras();
            UtilsClasse.FecharEAbrirProximoForm(this, codigoBarras);
        }

        // Abrir formulário - Registros
        private void pbAbrirRegis_form(object sender, EventArgs e)
        {
            //utilizando o metodo(de forma estatica, não precisa instanciar) para fechar o form atual e abri o proximo
            Registros registros = new Registros();
            UtilsClasse.FecharEAbrirProximoForm(this, registros);
        }

        // Abrir formulário - Gerenciar Colaboradores
        private void pbAbrirGenCol_form(object sender, EventArgs e)
        {
            //utilizando o metodo(de forma estatica, não precisa instanciar) para fechar o form atual e abri o proximo
            GerenciarColaboradores gerenciarColaboradores = new GerenciarColaboradores();
            UtilsClasse.FecharEAbrirProximoForm(this, gerenciarColaboradores);
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

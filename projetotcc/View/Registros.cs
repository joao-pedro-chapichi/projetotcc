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
    public partial class Registros : Form
    {
        private Form forms;
        private Form formAnterior;

        public Registros(Form forms, Form formAnterior)
        {
            this.forms = forms;
            this.formAnterior = formAnterior;
            InitializeComponent();
        }

        #region NAVEGAÇÃO
        // Carregar formulário
        private void CarregarForm_form(object sender, EventArgs e)
        {
            textBox1.Focus();
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

        // Voltar para formulário Gerenciamento
        private void pbVoltarGen_form(object sender, EventArgs e)
        {
            this.Hide();
            var Gerenciamento = new Gerenciamento(this, null);
            Gerenciamento.Closed += (s, args) => this.Close();
            Gerenciamento.Show();
        }

        // Imprimir registros
        private void pbImprimir_form(object sender, EventArgs e)
        {
            // Ainda sem ação
        }
        #endregion
    }
}

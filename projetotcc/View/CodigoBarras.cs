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
    public partial class CodigoBarras : Form
    {
        private Form forms;
        private Form formAnterior;

        public CodigoBarras(Form forms, Form formAnterior)
        {
            this.forms = forms;
            this.formAnterior = formAnterior;
            InitializeComponent();
        }

        #region NAVEGAÇÃO
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

        // Voltar para gerenciamento
        private void pbVoltarGen_form(object sender, EventArgs e)
        {
            this.Hide();
            var Gerenciamento = new Gerenciamento(this, null);
            Gerenciamento.Closed += (s, args) => this.Close();
            Gerenciamento.Show();
        }

        // Salvar código de barras
        private void pbSalvarCod_form(object sender, EventArgs e)
        {
            // Ainda sem ação
        }

        // Confirmar código de barras da textbox
        private void pbConfirmCod_form(object sender, EventArgs e)
        {
            // Ainda sem ação
        }
        #endregion
    }
}

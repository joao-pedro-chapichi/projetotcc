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
    public partial class AlterarColaborador : Form
    {
        private Form forms;
        private Form formAnterior;

        public AlterarColaborador(Form forms, Form formAnterior)
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

        // Voltar ao gerenciar colaboradores
        private void pbVoltarCol_form(object sender, EventArgs e)
        {
            this.Hide();
            var GerenciarColaboradores = new GerenciarColaboradores(this, null);
            GerenciarColaboradores.Closed += (s, args) => this.Close();
            GerenciarColaboradores.Show();
        }

        // FInalizar alteração de colaboradores
        private void finalizarAlt_form(object sender, EventArgs e)
        {
            // Ainda sem ação
        }
        #endregion
    }
}

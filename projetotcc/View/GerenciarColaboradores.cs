using projetotcc.Controller;
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
        private Form forms;
        private Form formAnterior;

        public GerenciarColaboradores(Form forms, Form formAnterior)
        {
            this.formAnterior = formAnterior;
            this.forms = forms;
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

        // Voltar ao formulário gerenciamento
        private void pbVoltarGen_form(object sender, EventArgs e)
        {
            this.Hide();
            var Gerenciamento = new Gerenciamento(this, null);
            Gerenciamento.Closed += (s, args) => this.Close();
            Gerenciamento.Show();
        }

        // Cadastrar novo colaborador
        private void cadastrarCol_form(object sender, EventArgs e)
        {
            this.Hide();
            var CadastrarColaborador = new CadastrarColaborador(this, null);
            CadastrarColaborador.Closed += (s, args) => this.Close();
            CadastrarColaborador.Show();
        }
        #endregion  
    }
}

using projetotcc.View;
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


namespace projetotcc
{
    public partial class Inicial : Form
    {
        private Form forms;
        private Form formAnterior;

        public Inicial(Form forms, Form formAnterior)
        {
            this.forms = forms;
            this.formAnterior = formAnterior;
            InitializeComponent();
        }

        // Aqui estão os eventos para fechar o formulario, abrir novos e outros
        #region NAVEGAÇÃO
        // Fechar Formulário - Form1
        private void pbFechar_form(object sender, EventArgs e)
        {
            UtilsClasse utils = new UtilsClasse();
            utils.confirmacaoFechar(this);
        }

        // Abrir formulário de gerenciamento
        private void pbAbrir_gerenciamento(object sender, EventArgs e)
        {
            this.Hide();
            var Gerenciamento = new Gerenciamento(this, null);
            Gerenciamento.Closed += (s, args) => this.Close();
            Gerenciamento.Show();
        }

        // Carregar formulário com o foco na textbox
        private void carregarForm_form(object sender, EventArgs e)
        {
            //o maior fodase possivel
            txbcodigo_inicial.Focus();
        }
        #endregion
    }
}
    
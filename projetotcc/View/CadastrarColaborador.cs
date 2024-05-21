using projetotcc.Controller;
using projetotcc.Model;
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
    public partial class CadastrarColaborador : Form
    {
        private Form forms;
        private Form formAnterior;

        public CadastrarColaborador(Form forms, Form formAnterior)
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

        // Finalizar cadastro de colaborador
        private void finalizarCad_form(object sender, EventArgs e)
        {
            string[] campos = new string[] { "id_funcionario", "nome"};

            ModelFuncionario modelFuncionario = new ModelFuncionario();

            
            modelFuncionario.Id = int.Parse(textCodigo.Text);
            modelFuncionario.Nome = textNome.Text;

            object[] array = ControllerAll.CriarArray(modelFuncionario);

            string res = ControllerAll.Cadastrar("funcionario", campos, array);
            MessageBox.Show(res);
        }
        #endregion
    }
}

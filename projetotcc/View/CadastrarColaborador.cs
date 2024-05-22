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
        public CadastrarColaborador()
        {
            InitializeComponent();
        }

        #region NAVEGAÇÃO
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

        // Voltar ao gerenciar colaboradores
        private void pbVoltarCol_form(object sender, EventArgs e)
        {
            //utilizando o metodo(de forma estatica, não precisa instanciar) para fechar o form atual e abri o proximo
            GerenciarColaboradores gerenciarColaboradores = new GerenciarColaboradores();
            UtilsClasse.FecharEAbrirProximoForm(this, gerenciarColaboradores);
        }

        // Finalizar cadastro de colaborador
        private void finalizarCad_form(object sender, EventArgs e)
        {
            //fazendo verificação de erros
            ModelFuncionario modelFuncionario = new ModelFuncionario();
            try
            {
                //adicionando os valores na classe do Funcionario
                modelFuncionario.Id_funcionario = int.Parse(textCodigo.Text);
                modelFuncionario.Nome = textNome.Text;
                //criando a resposta ao mesmo tempo que usa o metodo cadastrar
                //esse "funcionario" é a tabela que voce vai cadastrar, ao lado é a model 
                string res = ControllerAll.Cadastrar("funcionario", modelFuncionario);
                //Mostrando a resposta caso der certo
                MessageBox.Show(res);
            }
            catch (Exception ex)
            {
                //se der errado mostra o pq
                MessageBox.Show(ex.Message);
            }
            //apaga os valores das textbox
            textNome.Text = "";
            textCodigo.Text = "";
        }
        #endregion
    }
}

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
        public Inicial()
        {
            InitializeComponent();
        }

        // Aqui estão os eventos para fechar o formulario, abrir novos e outros
        #region NAVEGAÇÃO
        // Fechar Formulário - Form1
        private void pbFechar_form(object sender, EventArgs e)
        {
            UtilsClasse.ConfirmacaoFechar(this);
        }

        // Abrir formulário de gerenciamento
        private void pbAbrir_gerenciamento(object sender, EventArgs e)
        {
            //utilizando o metodo(de forma estatica, não precisa instanciar) para fechar o form atual e abri o proximo
            Gerenciamento gerenciamento = new Gerenciamento();
            UtilsClasse.FecharEAbrirProximoForm(this, gerenciamento);
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
    
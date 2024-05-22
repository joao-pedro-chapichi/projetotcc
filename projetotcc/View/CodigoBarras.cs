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
        public CodigoBarras()
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

        // Voltar para gerenciamento
        private void pbVoltarGen_form(object sender, EventArgs e)
        {
            //utilizando o metodo(de forma estatica, não precisa instanciar) para fechar o form atual e abri o proximo
            Gerenciamento gerenciamento = new Gerenciamento();
            UtilsClasse.FecharEAbrirProximoForm(this, gerenciamento);
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

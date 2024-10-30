using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using iTextSharp.text;
using iTextSharp.text.pdf;
using GenCode128;
using projetotcc.Utils; // Supondo que você tem UtilsClasse aí
using projetotcc.Controles_De_Usuario;
using projetotcc.Controller;

namespace projetotcc.View
{
    public partial class CodigoBarras : Form
    {
        public int altura = 4;
        public MenuLateral menuLateral;

        public CodigoBarras()
        {
            InitializeComponent();
            menuLateral = new MenuLateral(this);
            this.Controls.Add(menuLateral);
            menuLateral.Dock = DockStyle.Left;
        }

        private void btnGerar_Click(object sender, EventArgs e)
        {
            if (int.TryParse(txtCodigo.Text, out int codigo))
            {
                ControllerCarteirinha controllerCarteirinha = new ControllerCarteirinha();

                
                if (controllerCarteirinha.GerarCarteirinha(codigo, out string nome))
                {
                    return;
                }
                else
                {
                    MessageBox.Show("Código não encontrado. Verifique se a pessoa está cadastrada.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Código inválido. Por favor, insira um número inteiro.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
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
            // Utilizando o método (de forma estática, não precisa instanciar) para fechar o form atual e abrir o próximo
            Gerenciamento gerenciamento = new Gerenciamento();
            UtilsClasse.FecharEAbrirProximoForm(this, gerenciamento);
        }

        #endregion
    }
}

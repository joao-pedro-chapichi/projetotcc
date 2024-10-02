using GenCode128;
using projetotcc.Controles_De_Usuario;
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
        public int altura = 4;

        public MenuLateral menuLateral;


        public CodigoBarras()
        {
            InitializeComponent();
            menuLateral = new MenuLateral(this);
            this.Controls.Add(menuLateral);
            menuLateral.Dock = DockStyle.Left;
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

        private void btnGerar_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtCodigo.Text) || !string.IsNullOrWhiteSpace(txtCodigo.Text))
            {
                try
                {
                    Image codigoBarras = Code128Rendering.MakeBarcodeImage(txtCodigo.Text, altura, true);
                    picCodigoBarras.Image = codigoBarras;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, ex.Message, this.Text);
                }
            }
            else
            {
                MessageBox.Show("Preencha todos os campos e tente novamente.", "AVISO!");
            }
        }

        //METODO DE COPIAR, INUTIL NO MOMENTO

        //private void btnCopiar_Click(object sender, EventArgs e)
        //{
        //    if (picCodigoBarras.Image != null)
        //    {
        //        // Converte a imagem da PictureBox em um objeto Bitmap
        //        Bitmap bmp = new Bitmap(picCodigoBarras.Image);

        //        // Coloca o objeto Bitmap na área de transferência
        //        Clipboard.SetImage(bmp);

        //        MessageBox.Show("Codigo copiado para a área de transferência!");
        //    }
        //}

        private void btnImprimir_Click(object sender, EventArgs e)
        {
            this.printDocument1.Print();
        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            using (var g = e.Graphics)
            {
                using (var fnt = new Font("Courier New", 16))
                {
                    g.DrawImage(this.picCodigoBarras.Image, 30, 50);

                    var caption = txtCodigo.Text;
                    g.DrawString(caption, fnt, Brushes.Black, 340, 200);
                }
            }
        }

        #endregion
    }
}

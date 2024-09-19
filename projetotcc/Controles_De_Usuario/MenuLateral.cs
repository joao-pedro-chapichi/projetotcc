using projetotcc.Utils;
using projetotcc.View;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace projetotcc.Controles_De_Usuario
{
    public partial class MenuLateral : UserControl
    {
        Form form;

        public MenuLateral(Form form)
        {
            InitializeComponent();
            this.form = form;
            Redimensionar();
        }

        public static void FecharEAbrirProximoForm(Form formAtual, Form proximoForm)
        {
            formAtual.Hide();
            proximoForm.Show();
            proximoForm.FormClosed += (s, args) => formAtual.Close();
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            Gerenciamento gerenciamento = new Gerenciamento();
            FecharEAbrirProximoForm(form, gerenciamento);
        }

        private void MenuLateral_SizeChanged(object sender, EventArgs e)
        {

        }

        private void Redimensionar()
        {
            UtilsClasse.RedimensionarLabel(form, labelRegistros, 0.02f);
            UtilsClasse.RedimensionarLabel(form, labelFuncionarios, 0.02f);
            UtilsClasse.RedimensionarLabel(form, labelCodigoBarras, 0.02f);

        }

        private void irParaRegistros(object sender, EventArgs e)
        {
            Registros registros = new Registros();
            FecharEAbrirProximoForm(form, registros);
        }

        private void irParaFuncionarios(object sender, EventArgs e)
        {
            GerenciarColaboradores gerenciarColaboradores = new GerenciarColaboradores();
            FecharEAbrirProximoForm(form, gerenciarColaboradores);
        }

        private void irParaCodigoDeBarras(object sender, EventArgs e)
        {

        }

        private void btnVoltar(object sender, EventArgs e)
        {
            Inicial inicial = new Inicial();
            FecharEAbrirProximoForm(form, inicial);
        }
    }
}

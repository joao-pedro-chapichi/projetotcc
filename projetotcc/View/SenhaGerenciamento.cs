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
    public partial class SenhaGerenciamento : Form
    {
        public SenhaGerenciamento()
        {
            InitializeComponent();
        }

        private async void verificarSenha_click(object sender, EventArgs e)
        {
            string senhaGen = "1234";
            

            if (textBox1.Text == senhaGen)
            {
                label2.Visible = true;
                label3.Visible = true;
                label2.Text = "SENHA CORRETA!";
                label3.Text = "ACESSO LIBERADO.";
                label2.ForeColor = Color.Green;
                label3.ForeColor = Color.Green;
                await Task.Delay(2000);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                label2.Visible = true;
                label3.Visible = true;
                label2.Text = "SENHA INCORRETA!";
                label3.Text = "ACESSO NEGADO.";
                label2.ForeColor = Color.Red;
                label3.ForeColor = Color.Red;
                

            }
        }

        private void carregarForm_load(object sender, EventArgs e)
        {
            label2.Visible = false;
            label3.Visible = false;
        }

        private async void acionarVerificacaoSenha_enter(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                string senhaGen = "1234";

                if (textBox1.Text == senhaGen)
                {
                    label2.Visible = true;
                    label3.Visible = true;
                    label2.Text = "SENHA CORRETA!";
                    label3.Text = "ACESSO LIBERADO.";
                    label2.ForeColor = Color.Green;
                    label3.ForeColor = Color.Green;
                    await Task.Delay(2000);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    textBox1.Text = " ";
                    label2.Visible = true;
                    label3.Visible = true;
                    label2.Text = "SENHA INCORRETA!";
                    label3.Text = "ACESSO NEGADO.";
                    label2.ForeColor = Color.Red;
                    label3.ForeColor = Color.Red;

                    textBox1.Clear();
                    textBox1.Focus();
                    UtilsClasse.SimularBackspace();
                }
            }
        }
    }
}

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
using static System.Runtime.CompilerServices.RuntimeHelpers;

namespace projetotcc.View
{
    public partial class CadastrarColaborador : Form
    {

        private Timer timer;

        public CadastrarColaborador()
        {
            InitializeComponent();
            Redimensionar();
            timer = new Timer();
            timer.Interval = 500;
            timer.Tick += cadCol_Tick_Form;
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
            this.Close();
        }

        // Finalizar cadastro de colaborador
        private async void finalizarCad_form(object sender, EventArgs e)
        {
            ModelFuncionario modelFunc = new ModelFuncionario();

            // Validação para garantir que o campo codigo e nome não fiquem vazios no cadastro
            if (string.IsNullOrEmpty(textCodigo.Text) || string.IsNullOrWhiteSpace(textCodigo.Text))
            {
                MessageBox.Show("Preencha todos os campos antes de continuar.", "AVISO!");
            }
            else
            {
                modelFunc.Nome = textNome.Text;
                modelFunc.Cpf = textCPF.Text;

                if (string.IsNullOrEmpty(textNome.Text) || string.IsNullOrWhiteSpace(textNome.Text))
                {
                    MessageBox.Show("Preencha todos os campos antes de continuar.", "AVISO!");
                }
                else
                {
                    modelFunc.Id_funcionario = Convert.ToInt32(textCodigo.Text);
                    await ControllerColaborador.cadastrarFuncionario(modelFunc);
                    int proximoCodigo = await ControllerColaborador.ObterUltimoCodigoFuncionario() + 1;
                    textCodigo.Text = proximoCodigo.ToString();
                    textNome.Text = "";
                    textCPF.Text = "";
                }
            }
        }
        #endregion

        #region Validação de Teclas
        private void txbNome_ValidacaoTecla(object sender, KeyPressEventArgs e)
        {
            // Lista de caracteres acentuados permitidos
            char[] allowedChars = { 'é', 'è', 'ê', 'ë', 'à', 'â', 'ä', 'á', 'ò', 'ô', 'ö', 'ó', 'ù', 'û', 'ü', 'ú', ' ' };

            // Verifica se a tecla pressionada é uma letra, uma letra acentuada específica ou tecla de controle
            if (!char.IsLetter(e.KeyChar) && !char.IsControl(e.KeyChar) && !Array.Exists(allowedChars, c => c == e.KeyChar))
            {
                e.Handled = true; // Impede a entrada de qualquer caractere que não seja letra, letra acentuada específica ou tecla de controle
            }

        }

        private void txbCodigo_ValidacaoTecla(object sender, KeyPressEventArgs e)
        {
            // Verifica se a tecla pressionada é um dígito ou a tecla de controle (como backspace)
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true; // Impede a entrada de qualquer caractere que não seja número
            }
        }

        private void txbCPF_ValidacaoTecla(object sender, KeyPressEventArgs e)
        {
            // Verifica se a tecla pressionada é um dígito ou a tecla de controle (como backspace)
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true; // Impede a entrada de qualquer caractere que não seja número
            }
        }
        #endregion

        private void CadastrarColaborador_SizeChanged(object sender, EventArgs e)
        {
            Redimensionar();
        }

        private void Redimensionar()
        {
            UtilsClasse.RedimensionarLabel(this, labelCadastrar, 0.04f);
            UtilsClasse.RedimensionarLabel(this, labelNome, 0.02f);
            UtilsClasse.RedimensionarLabel(this, labelCodigo, 0.02f);
            UtilsClasse.RedimensionarLabel(this, labelCfp, 0.02f);
        }

        private void carregarCadColaborador_Form(object sender, EventArgs e)
        {
            timer.Start();
        }

        private async void cadCol_Tick_Form(object sender, EventArgs e)
        {
            timer.Stop();
            int proximoCodigo = await ControllerColaborador.ObterUltimoCodigoFuncionario() + 1;
            textCodigo.Text = proximoCodigo.ToString();
        }
    }
}

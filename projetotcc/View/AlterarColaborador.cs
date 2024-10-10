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
    public partial class AlterarColaborador : Form
    {
        // Declaração de variáveis globais da classe
        int id_funcionario; // ID do funcionário que será alterado
        string nome;        // Nome do funcionário que será alterado
        int id;             // ID obtido através de uma busca assíncrona

        // Construtor da classe AlterarColaborador
        public AlterarColaborador(int id_funcionario, string nome, string cpf)
        {
            InitializeComponent();  // Inicializa os componentes da interface gráfica

            // Atribui os parâmetros recebidos às variáveis globais
            this.nome = nome;

            // Define o texto do campo txtNome com o nome do funcionário
            textNome.Text = nome;

            textCPF.Text = cpf;

            // Define o texto do campo txtCodigo com o ID do funcionário, convertendo-o para string
            textCodigo.Text = id_funcionario.ToString();
        }

        // Método que será chamado quando o formulário for carregado de forma assíncrona
        private async void AlterarColaborador_LoadAsync(object sender, EventArgs e)
        {
            // Chama o método PesquisarCodigoPorNome do ControllerColaborador para obter o ID
            // do funcionário baseado no nome e espera o resultado
            id = await ControllerColaborador.PesquisarCodigoPorNome(nome);
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

        // FInalizar alteração de colaboradores
        private async void finalizarAlt_formAsync(object sender, EventArgs e)
        {
            // Verifica se o campo 'Nome' não está vazio
            if (string.IsNullOrEmpty(textNome.Text) || string.IsNullOrWhiteSpace(textNome.Text))
            {
                MessageBox.Show("Preencha o campo Nome antes de continuar.", "AVISO!");
                return;
            }

            // Verifica se o campo 'Código' não está vazio
            if (string.IsNullOrEmpty(textCodigo.Text) || string.IsNullOrWhiteSpace(textCodigo.Text))
            {
                MessageBox.Show("Preencha o campo Código antes de continuar.", "AVISO!");
                return;
            }

            // Validação do CPF
            if (string.IsNullOrWhiteSpace(textCPF.Text))
            {
                MessageBox.Show("O CPF não pode ser vazio!", "ERRO!");
                return;
            }

            // Verifica se o CPF contém apenas números e tem 11 dígitos
            if (!textCPF.Text.All(char.IsDigit) || textCPF.Text.Length != 11)
            {
                MessageBox.Show("O CPF deve conter exatamente 11 números!", "ERRO!");
                return;
            }

            // Verifica se o CPF é válido
            if (!UtilsClasse.ValidarCpf(textCPF.Text))
            {
                MessageBox.Show("O CPF informado é inválido!", "ERRO!");
                return;
            }

            try
            {
                // Instancia a Classe
                ModelFuncionario mFun = new ModelFuncionario();
                // Atribui os valores da classe
                mFun.Nome = textNome.Text;
                mFun.Id_funcionario = Convert.ToInt32(textCodigo.Text);
                mFun.Cpf = textCPF.Text;

                // Chama o método estático assíncrono para alterar os dados
                string res = await ControllerColaborador.AlterarDados(mFun, id);

                MessageBox.Show(res);
            }
            catch (Exception ex)
            {
                // Mostra o Erro caso a alteração não dê certo
                MessageBox.Show(ex.Message);
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
        #endregion

        private void txbCPF_ValidacaoTecla(object sender, KeyPressEventArgs e)
        {
            // Verifica se a tecla pressionada é um dígito ou a tecla de controle (como backspace)
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true; // Impede a entrada de qualquer caractere que não seja número
            }
        }

        private void Redimensionar()
        {
            UtilsClasse.RedimensionarLabel(this, labelaAlterar, 0.04f);
            UtilsClasse.RedimensionarLabel(this, labelNome, 0.02f);
            UtilsClasse.RedimensionarLabel(this, labelCodigo, 0.02f);
            UtilsClasse.RedimensionarLabel(this, labelCfp, 0.02f);
        }
    }
}

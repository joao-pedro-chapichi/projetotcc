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

            // Define o texto dos campos com os valores recebidos
            textNome.Text = nome;
            textCodigo.Text = id_funcionario.ToString();
            textCPF.Text = cpf;
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
            // Utilizando o método para fechar o form atual e abrir o próximo
            GerenciarColaboradores gerenciarColaboradores = new GerenciarColaboradores();
            UtilsClasse.FecharEAbrirProximoForm(this, gerenciarColaboradores);
        }

        // Finalizar alteração de colaboradores
        private async void finalizarAlt_formAsync(object sender, EventArgs e)
        {
            // Verifica se os campos não estão vazios
            if (string.IsNullOrEmpty(textNome.Text) || string.IsNullOrWhiteSpace(textNome.Text))
            {
                MessageBox.Show("Preencha todos os campos antes de continuar.", "AVISO!");
                return;
            }
            if (string.IsNullOrEmpty(textCodigo.Text) || string.IsNullOrWhiteSpace(textCodigo.Text))
            {
                MessageBox.Show("Preencha todos os campos antes de continuar.", "AVISO!");
                return;
            }
            if (string.IsNullOrEmpty(textCPF.Text) || string.IsNullOrWhiteSpace(textCPF.Text))
            {
                MessageBox.Show("Preencha o campo CPF antes de continuar.", "AVISO!");
                return;
            }

            try
            {
                // Instancia a Classe ModelFuncionario
                ModelFuncionario mFun = new ModelFuncionario
                {
                    Nome = textNome.Text,
                    Id_funcionario = Convert.ToInt32(textCodigo.Text),
                    Cpf = textCPF.Text
                };

                // Chama o método estático assíncrono de alterar os dados, usando a classe e o ID do funcionário
                string res = await ControllerColaborador.AlterarDados(mFun, id);

                MessageBox.Show(res);
            }
            catch (Exception ex)
            {
                // Mostra o erro caso a alteração não dê certo
                MessageBox.Show(ex.Message);
            }
        }

        #endregion

        #region Validação de Teclas
        private void txbNome_ValidacaoTecla(object sender, KeyPressEventArgs e)
        {
            // Lista de caracteres acentuados permitidos
            char[] allowedChars = { 'é', 'è', 'ê', 'ë', 'à', 'â', 'ä', 'á', 'ò', 'ô', 'ö', 'ó', 'ù', 'û', 'ü', 'ú' };

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
    }
}

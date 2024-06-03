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
        public AlterarColaborador(int id_funcionario, string nome)
        {
            InitializeComponent();  // Inicializa os componentes da interface gráfica

            // Atribui os parâmetros recebidos às variáveis globais
            this.nome = nome;

            // Define o texto do campo txtNome com o nome do funcionário
            txtNome.Text = nome;

            // Define o texto do campo txtCodigo com o ID do funcionário, convertendo-o para string
            txtCodigo.Text = id_funcionario.ToString();
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
            //utilizando o metodo(de forma estatica, não precisa instanciar) para fechar o form atual e abri o proximo
            GerenciarColaboradores gerenciarColaboradores = new GerenciarColaboradores();
            UtilsClasse.FecharEAbrirProximoForm(this, gerenciarColaboradores);
        }

        // FInalizar alteração de colaboradores
        private async void finalizarAlt_formAsync(object sender, EventArgs e)
        {
           //Verifica se o campo não está vazio
            if (string.IsNullOrEmpty(txtNome.Text))
            {
                MessageBox.Show("Valor invalido no campo nome!");
                return;
            }//Verifica se o campo não está vazio
            if (string.IsNullOrEmpty(txtCodigo.Text))
            {
                MessageBox.Show("Valor invalido no campo codigo!");
                return;
            }//Tenta iniciar a alteração
            try
            {   //Instancia a Classe
                ModelFuncionario mFun = new ModelFuncionario();
                //Atribui os valores da classe
                mFun.Nome = txtNome.Text;
                mFun.Id_funcionario = Convert.ToInt32(txtCodigo.Text);
                //Chama o metodo static assincrono de alterar os dados, usando a classe e o id do funcionario
                string res = await ControllerColaborador.AlterarDados(mFun, id);

                MessageBox.Show(res);
            }catch (Exception ex)
            {//Mostra o Erro caso a alteração não de certo
                MessageBox.Show(ex.Message);
            }
            
        }
        #endregion


    }
}

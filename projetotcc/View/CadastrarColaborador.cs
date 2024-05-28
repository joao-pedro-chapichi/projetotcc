﻿using projetotcc.Controller;
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
    public partial class CadastrarColaborador : Form
    {
        public CadastrarColaborador()
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

        // Voltar ao gerenciar colaboradores
        private void pbVoltarCol_form(object sender, EventArgs e)
        {
            //utilizando o metodo(de forma estatica, não precisa instanciar) para fechar o form atual e abri o proximo
            GerenciarColaboradores gerenciarColaboradores = new GerenciarColaboradores();
            UtilsClasse.FecharEAbrirProximoForm(this, gerenciarColaboradores);
        }

        // Finalizar cadastro de colaborador
        private void finalizarCad_form(object sender, EventArgs e)
        {
            // Chamando as classes ControllerColaborador e ModelFuncionario
            ControllerColaborador controllerCol = new ControllerColaborador();
            ModelFuncionario modelFunc = new ModelFuncionario();

            // Inserindo os valores da textBox nos atributos (set nome e set id)
            modelFunc.Nome = textNome.Text;
            modelFunc.Id_funcionario = Convert.ToInt32(textCodigo.Text);

            /* Chamando o metodo cadastrarFuncionario e passando como parametro a classe
               ModelFuncionario (Explicado na classe ControllerColaborador) */
            controllerCol.cadastrarFuncionario(modelFunc);

            // Limpando a textBox após cadastrar o funcionário
            textCodigo.Text = "";
            textNome.Text = "";
        }
        #endregion
    }
}

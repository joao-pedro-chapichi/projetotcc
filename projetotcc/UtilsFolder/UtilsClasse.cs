using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using projetotcc.View;

namespace projetotcc.Utils
{
    public static class UtilsClasse
    {   
        #region UTILIDADES
        // Fechar os formulários com confirmação
        public static void ConfirmacaoFechar(Form formAtual)
        {
            DialogResult res = MessageBox.Show("Tem certeza que deseja encerrar o sistema?", "Aviso!", MessageBoxButtons.OKCancel);
            if (res == DialogResult.OK)
            {
                formAtual.Close();
            }
        }

        //metodo que fecha o form atual e abre o proximo
        public static void FecharEAbrirProximoForm(Form formAtual, Form proximoForm)
        {
            formAtual.Hide();
            proximoForm.Show();
            proximoForm.FormClosed += (s, args) => formAtual.Close();
        }

        public static void RedimensionarLabel(Form form, Label label, float tamanho)
        {
            float newSize = form.Width * tamanho; // 5% da largura do formulário
            label.Font = new Font(label.Font.FontFamily, newSize, label.Font.Style);
            label.Left = (form.ClientSize.Width - label.Width) / 2;
            label.Top = (form.ClientSize.Height - label.Height) / 2;
        }

        public static void AjustarFonteDataGridView(DataGridView dataGridView, float proporcaoBase)
        {
            float novoTamanhoFonte = dataGridView.Width * proporcaoBase;

            // Ajusta a fonte das células (registros)
            dataGridView.RowsDefaultCellStyle.Font = new Font(dataGridView.Font.FontFamily, novoTamanhoFonte);

            // Ajusta a fonte do cabeçalho das colunas
            dataGridView.ColumnHeadersDefaultCellStyle.Font = new Font(dataGridView.Font.FontFamily, novoTamanhoFonte);

            // Caso o `RowHeadersDefaultCellStyle` também precise ser ajustado
            dataGridView.RowHeadersDefaultCellStyle.Font = new Font(dataGridView.Font.FontFamily, novoTamanhoFonte);
        }

        //Validacao de CPF 
        public static bool ValidarCpf(string cpf)
        {
            // Algoritmo de validação do CPF com dígito verificador
            // Remove qualquer pontuação do CPF
            cpf = cpf.Trim().Replace(".", "").Replace("-", "");

            if (cpf.Length != 11 || cpf.All(c => c == cpf[0])) // Verifica se todos os dígitos são iguais
                return false;

            int[] multiplicador1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            string tempCpf;
            string digito;
            int soma, resto;

            tempCpf = cpf.Substring(0, 9);
            soma = 0;

            for (int i = 0; i < 9; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];

            resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;

            digito = resto.ToString();
            tempCpf = tempCpf + digito;
            soma = 0;

            for (int i = 0; i < 10; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];

            resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;

            digito = digito + resto.ToString();

            return cpf.EndsWith(digito);
        }

        #endregion
    }
}

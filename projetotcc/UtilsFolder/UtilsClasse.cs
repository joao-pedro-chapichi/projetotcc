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

        #endregion
    }
}

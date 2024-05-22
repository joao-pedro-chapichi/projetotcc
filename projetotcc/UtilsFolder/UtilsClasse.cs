using System;
using System.Collections.Generic;
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
        #endregion
    }
}

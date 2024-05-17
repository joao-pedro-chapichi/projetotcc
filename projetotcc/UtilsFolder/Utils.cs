using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using projetotcc.View;

namespace projetotcc.Utils
{
    public class UtilsClasse
    {
        Form form;

        // Fechar os formulários com confirmação
        public void confirmacaoFechar(Form formAtual)
        {
            form = formAtual;
            DialogResult res = MessageBox.Show("Tem certeza que deseja encerrar o sistema?", "Aviso!", MessageBoxButtons.OKCancel);
            if (res == DialogResult.OK)
            {
                form.Close();
            }
        }
    }
}

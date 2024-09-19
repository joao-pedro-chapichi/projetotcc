using projetotcc.Controles_De_Usuario;
using projetotcc.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace projetotcc.View
{
    public partial class Gerenciamento : Form
    {
        public MenuLateral menuLateral;

        public Gerenciamento()
        {
            InitializeComponent();
            menuLateral = new MenuLateral(this);
            this.Controls.Add(menuLateral);
            menuLateral.Dock = DockStyle.Left;
            Redimensionar();
        }

        private void Gerenciamento_SizeChanged(object sender, EventArgs e)
        {
            Redimensionar();
        }

        private void Redimensionar()
        {
            UtilsClasse.RedimensionarLabel(this, labelHome1, 0.04f);
            UtilsClasse.RedimensionarLabel(this, labelHome2, 0.02f);
        }

        #region HOME
        // Eventos ao carregar formulário
        private void frmCarregar_form(object sender, EventArgs e)
        {
        }

        private void horaData_tick(object sender, EventArgs e)
        {
            //DateTime horaDataUtc = DateTime.UtcNow;
            //TimeZoneInfo brasiliaHoraData = TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time");
            //DateTime brasiliaAtual = TimeZoneInfo.ConvertTimeFromUtc(horaDataUtc, brasiliaHoraData);
            //label1.Text = brasiliaAtual.ToString("HH:mm:ss - dd/MM/yyyy");
        }

        #endregion

        
    }
}

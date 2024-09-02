using projetotcc.Controller;
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
    public partial class CriarRelatorio : Form
    {
        public CriarRelatorio()
        {
            InitializeComponent();
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            Registros registros = new Registros();
            UtilsClasse.FecharEAbrirProximoForm(this, registros);
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            try
            { 

                DataTable res = await ControllerRelatorio.SomarHorasEDias(dateInicio.Value.Date, dateFim.Value.Date);
                dataGridView1.DataSource = res;
            }
            catch (Exception ex)
            {
                // Tratar exceções e informar o usuário
                MessageBox.Show($"Ocorreu um erro: {ex.Message}");
            }
        }

    }
}

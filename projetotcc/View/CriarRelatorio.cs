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
            radioButtonSimples.Checked = true;
            campoCPF.Visible = false;
            txtCPF.Visible = false;
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
                if(radioButtonDetalhado.Checked)
                {
                    if (string.IsNullOrEmpty(campoCPF.Text))
                    {
                        MessageBox.Show("O CPF está Nulo!!", "ERRO ERRO ERRO");
                        return;
                    }

                    DataTable res = await ControllerRelatorio.GerarRelatorioDetalhado(dateInicio.Value.Date, dateFim.Value.Date, campoCPF.Text);
                    dataGridView1.DataSource = res;

                }
                else
                {
                    DataTable res = await ControllerRelatorio.SomarHorasEDias(dateInicio.Value.Date, dateFim.Value.Date);
                    dataGridView1.DataSource = res;
                }
            }
            catch (Exception ex)
            {
                
                MessageBox.Show($"Ocorreu um erro: {ex.Message}");
            }
        }

        private void gerarPDF_click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow != null)
            {
                DataGridViewRow linhaSelecionada = dataGridView1.CurrentRow;

                
                string dataInicial = dateInicio.Value.ToString("dd/MM/yyyy");
                string dataFinal = dateFim.Value.ToString("dd/MM/yyyy");

                
                if (radioButtonSimples.Checked)
                {
                    ControllerPDF controllerPDF = new ControllerPDF();
                    controllerPDF.GerarRelatorioSimples(dataGridView1, dataInicial, dataFinal);
                }
                else 
                {
                    string cpfUsuario = campoCPF.Text.Replace(".", "").Replace("-", "").Trim();
                    string nomeFuncionario;
                    int idFuncionario;
                    

                    
                    if (ControllerColaborador.verificarExistenciaParaRelatorioDetalhadoCPF(cpfUsuario, out nomeFuncionario, out idFuncionario))
                    {
                        
                        ControllerPDF controllerPDF = new ControllerPDF();
                        controllerPDF.GerarRelatorioDetalhado(dataGridView1, nomeFuncionario, idFuncionario.ToString());
                    }
                    else
                    {
                        MessageBox.Show("Funcionário não encontrado. Verifique o CPF informado.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Selecione uma célula antes de gerar o relatório.");
            }
        }


        private void radioButtonSimples_CheckedChanged(object sender, EventArgs e)
        {
            radioButtonDetalhado.Checked = false;
        }

        private void radioButtonDetalhado_CheckedChanged(object sender, EventArgs e)
        {
            radioButtonSimples.Checked = false;

        }

        private void MostrarCPF()
        {
            try
            {
                if(radioButtonDetalhado.Checked)
                {
                    campoCPF.Visible = true;
                    txtCPF.Visible = true;
                    return;
                }
                else
                {
                    campoCPF.Visible = false;
                    txtCPF.Visible = false;
                    return;
                }
            }catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void radioButtonSimples_CheckedChanged_1(object sender, EventArgs e)
        {
            MostrarCPF();
        }

        private void radioButtonDetalhado_CheckedChanged_1(object sender, EventArgs e)
        {
            MostrarCPF();
        }
    }
}

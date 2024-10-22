using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using iTextSharp.text;
using iTextSharp.text.pdf;
using GenCode128;
using projetotcc.Utils; // Supondo que você tem UtilsClasse aí
using projetotcc.Controles_De_Usuario;

namespace projetotcc.View
{
    public partial class CodigoBarras : Form
    {
        public int altura = 4;
        public MenuLateral menuLateral;

        public CodigoBarras()
        {
            InitializeComponent();
            menuLateral = new MenuLateral(this);
            this.Controls.Add(menuLateral);
            menuLateral.Dock = DockStyle.Left;
        }

        private void buttonGeneratePDF_Click(object sender, EventArgs e)
        {
            if (!ValidarCampos())
            {
                MessageBox.Show("Preencha todos os campos corretamente.");
                return;
            }

            GerarPDF();
        }

        private bool ValidarCampos()
        {
            // Verifica se os campos de nome e código estão preenchidos e se a imagem foi gerada
            return !string.IsNullOrWhiteSpace(textBoxName.Text) &&
                   !string.IsNullOrWhiteSpace(txtCodigo.Text) &&
                   picCodigoBarras.Image != null;
        }

        private void GerarPDF()
        {
            string nome = textBoxName.Text;
            string codigoBarras = txtCodigo.Text;

            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "PDF files (*.pdf)|*.pdf",
                Title = "Salvar PDF"
            };

            if (saveFileDialog.ShowDialog() != DialogResult.OK) return;

            string caminhoPDF = saveFileDialog.FileName;

            // Cria o documento PDF
            Document document = new Document();
            try
            {
                // Cria o writer que escreve no PDF
                PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(caminhoPDF, FileMode.Create));
                document.Open();

                // Adiciona o nome ao PDF
                document.Add(new Paragraph("Nome: " + nome));

                // Adiciona o código de barras ao PDF
                var code128 = new Barcode128
                {
                    CodeType = Barcode.CODE128,
                    Code = codigoBarras
                };

                // Cria a imagem do código de barras com o DirectContent do PdfWriter
                PdfContentByte cb = writer.DirectContent;
                iTextSharp.text.Image barcodeImage = code128.CreateImageWithBarcode(cb, null, null);
                document.Add(barcodeImage);

                // Adiciona a imagem da PictureBox ao PDF
                using (MemoryStream stream = new MemoryStream())
                {
                    System.Drawing.Image pictureBoxImage = picCodigoBarras.Image;
                    pictureBoxImage.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                    iTextSharp.text.Image pdfImage = iTextSharp.text.Image.GetInstance(stream.ToArray());
                    pdfImage.ScaleToFit(200f, 200f); // Ajusta o tamanho da imagem (opcional)
                    document.Add(pdfImage);
                }

                MessageBox.Show("PDF gerado com sucesso!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao gerar o PDF: " + ex.Message);
            }
            finally
            {
                document.Close();
            }
        }


        private void btnGerar_Click(object sender, EventArgs e)
        {
            // Caminho da imagem base
            string caminhoImagem = "../Resources/codigoDeBarras/Cartao";

            // Verifica se a imagem existe
            if (!File.Exists(caminhoImagem))
            {
                MessageBox.Show("Imagem base não encontrada.");
                return;
            }

            // Captura o nome e o código de barras das TextBox
            string nome = textBoxName.Text;
            string codigoBarras = txtCodigo.Text;

            try
            {
                // Carrega a imagem base
                Bitmap bitmap = new Bitmap(caminhoImagem);

                // Cria um objeto Graphics para desenhar na imagem
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    // Define a fonte e a cor para o texto
                    System.Drawing.Font fonte = new System.Drawing.Font("Arial", 12, FontStyle.Bold);
                    Brush corTexto = Brushes.Black;

                    // Desenha o nome na posição adequada (ajuste as coordenadas conforme necessário)
                    g.DrawString(nome, fonte, corTexto, new PointF(30, 20));

                    // Desenha o código na posição adequada (ajuste as coordenadas conforme necessário)
                    g.DrawString(codigoBarras, fonte, corTexto, new PointF(30, 80));

                    // Gera o código de barras como uma imagem
                    System.Drawing.Image imagemCodigoBarras = Code128Rendering.MakeBarcodeImage(codigoBarras, altura, true);

                    // Define a posição onde o código de barras será desenhado (ajuste conforme necessário)
                    Point posicaoCodigoBarras = new Point(300, 150);
                    g.DrawImage(imagemCodigoBarras, posicaoCodigoBarras);
                }

                // Atualiza a PictureBox com o bitmap editado
                picCodigoBarras.Image = bitmap;

                MessageBox.Show("Cartão gerado com sucesso!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao gerar o cartão: " + ex.Message);
            }
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

        // Voltar para gerenciamento
        private void pbVoltarGen_form(object sender, EventArgs e)
        {
            // Utilizando o método (de forma estática, não precisa instanciar) para fechar o form atual e abrir o próximo
            Gerenciamento gerenciamento = new Gerenciamento();
            UtilsClasse.FecharEAbrirProximoForm(this, gerenciamento);
        }

        private void btnImprimir_Click(object sender, EventArgs e)
        {
            this.printDocument1.Print();
        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            using (var g = e.Graphics)
            {
                using (var fnt = new System.Drawing.Font("Courier New", 16))
                {
                    g.DrawImage(this.picCodigoBarras.Image, 30, 50);

                    var caption = txtCodigo.Text;
                    g.DrawString(caption, fnt, Brushes.Black, 340, 200);
                }
            }
        }

        #endregion
    }
}

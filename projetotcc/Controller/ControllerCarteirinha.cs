using iTextSharp.text.pdf;
using iTextSharp.text;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace projetotcc.Controller
{
    public class ControllerCarteirinha
    {
        public void LayoutCarteirinha(string nome, int codigo)
        {

            using (var saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.FileName = $"Carteirinha-{nome}.pdf";
                saveFileDialog.Filter = "PDF files (*.pdf)|*.pdf";
                saveFileDialog.Title = "Salvar Carteirinha";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        using (Document document = new Document(new Rectangle(200, 300)))
                        {
                            PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(saveFileDialog.FileName, FileMode.Create));
                            document.Open();

                            
                            string avatarPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\Resources\Icones", "avatar-carteirinha.png");

                            
                            if (File.Exists(avatarPath))
                            {
                                Image avatar = Image.GetInstance(avatarPath);
                                avatar.ScaleToFit(80f, 80f);
                                avatar.Alignment = Element.ALIGN_CENTER;
                                document.Add(avatar);
                                document.Add(new Paragraph("\n"));
                            }
                            else
                            {
                                document.Add(new Paragraph($"Avatar não encontrado em: {avatarPath}", FontFactory.GetFont("Arial", 14, Font.ITALIC)));
                            }

                            
                            document.Add(new Paragraph("Carteirinha", FontFactory.GetFont("Arial", 18, Font.BOLD))
                            {
                                Alignment = Element.ALIGN_CENTER
                            });

                            
                            document.Add(new Paragraph($"Nome: {nome}", FontFactory.GetFont("Arial", 12))
                            {
                                Alignment = Element.ALIGN_CENTER 
                            });

                            
                            try
                            {
                                Barcode39 barcode = new Barcode39
                                {

                                    Code = codigo.ToString(),
                                };

                                Image barcodeImage = barcode.CreateImageWithBarcode(writer.DirectContent, BaseColor.BLACK, BaseColor.BLACK);
                                barcodeImage.ScaleToFit(180f, 60f);
                                barcodeImage.Alignment = Element.ALIGN_CENTER;
                                document.Add(barcodeImage);
                            }
                            catch (Exception ex)
                            {
                                document.Add(new Paragraph($"Erro ao gerar código de barras: {ex.Message}", FontFactory.GetFont("Arial", 14, Font.ITALIC)));
                            }


                            document.Close();
                        }
                        MessageBox.Show($"A carteirinha do {nome} foi criada com sucesso!", "Sucesso!", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Erro ao gerar carteirinha: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Operação cancelada.", "Cancelado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        public bool GerarCarteirinha(int codigo, out string nome)
        {
            
            bool existe = ControllerColaborador.verificarExistenciaParaCarteirinha(codigo, out nome);

            if (existe)
            {
                
                LayoutCarteirinha(nome, codigo);
            }

            return existe;
        }
    }
}


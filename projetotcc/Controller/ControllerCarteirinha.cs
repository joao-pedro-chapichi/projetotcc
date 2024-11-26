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

                            PdfPTable table = new PdfPTable(1);
                            table.WidthPercentage = 130;

                            PdfPCell cell = new PdfPCell();
                            cell.Border = Rectangle.BOX;
                            cell.BorderColor = BaseColor.BLACK;
                            cell.Padding = 10f;

                            string avatarPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\Resources\Icones", "avatar-carteirinha.png");

                            if (File.Exists(avatarPath))
                            {
                                Image avatar = Image.GetInstance(avatarPath);
                                avatar.ScaleToFit(80f, 80f);
                                avatar.Alignment = Element.ALIGN_CENTER;
                                cell.AddElement(avatar);
                                cell.AddElement(new Paragraph("\n"));
                            }
                            else
                            {
                                cell.AddElement(new Paragraph($"Avatar não encontrado em: {avatarPath}", FontFactory.GetFont("Arial", 14, Font.ITALIC)));
                            }

                            cell.AddElement(new Paragraph("Carteirinha", FontFactory.GetFont("Arial", 15, Font.BOLD))
                            {
                                Alignment = Element.ALIGN_CENTER
                            });

                            cell.AddElement(new Paragraph($"{nome}", FontFactory.GetFont("Arial", 10))
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
                                barcodeImage.ScaleToFit(140f, 40f);
                                barcodeImage.Alignment = Element.ALIGN_CENTER;
                                cell.AddElement(barcodeImage);
                            }
                            catch (Exception ex)
                            {
                                cell.AddElement(new Paragraph($"Erro ao gerar código de barras: {ex.Message}", FontFactory.GetFont("Arial", 14, Font.ITALIC)));
                            }

                            table.AddCell(cell);
                            document.Add(table);
                            document.Close();
                        }
                        MessageBox.Show($"A carteirinha do(a) {nome} foi criada com sucesso!", "Sucesso!", MessageBoxButtons.OK, MessageBoxIcon.Information);
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


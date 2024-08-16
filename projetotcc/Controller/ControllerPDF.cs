using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;
using iTextSharp;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;
using static iTextSharp.text.pdf.codec.TiffWriter;

namespace projetotcc.Controller
{
    public class ControllerPDF
    {
        
        public void GerarPdf(DataGridView dataGridView, string nome, string id)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "PDF Files (*.pdf)|*.pdf";
            saveFileDialog.Title = "Salvar PDF";
            saveFileDialog.FileName = $"Relatório-{nome}.pdf";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                string caminhoArquivo = saveFileDialog.FileName;

                Document doc = new Document(PageSize.A4, 20, 20, 30, 30); // Margens: esquerda, direita, superior, inferior
                PdfWriter.GetInstance(doc, new FileStream(caminhoArquivo, FileMode.Create));

                doc.Open();

                // Fontes
                Font titleFont = FontFactory.GetFont("Arial", 14, Font.BOLD, BaseColor.BLACK);
                Font headerFont = FontFactory.GetFont("Arial", 10, Font.BOLD, BaseColor.BLACK);
                Font bodyFont = FontFactory.GetFont("Arial", 9, Font.NORMAL, BaseColor.BLACK);

                // Título
                Paragraph title = new Paragraph("Folha de Ponto", titleFont)
                {
                    Alignment = Element.ALIGN_CENTER,
                    SpacingAfter = 20f
                };
                doc.Add(title);

                // Informações básicas da folha de ponto
                PdfPTable infoTable = new PdfPTable(2);
                infoTable.WidthPercentage = 100;
                infoTable.SpacingAfter = 20f;

                PdfPCell cell1 = new PdfPCell(new Phrase($"Funcionário: {nome}\nCargo: (Tavlez tenha)\nMês: (Provavelmente vai ter)", bodyFont));
                cell1.Border = PdfPCell.NO_BORDER;
                infoTable.AddCell(cell1);

                PdfPCell cell2 = new PdfPCell(new Phrase($"ID: {id}\nDepartamento: (Improvavel que a gente coloque)\nGestor: (Talvez tenha)", bodyFont))
                {
                    HorizontalAlignment = Element.ALIGN_RIGHT
                };
                cell2.Border = PdfPCell.NO_BORDER;
                infoTable.AddCell(cell2);

                doc.Add(infoTable);

                // Tabela de pontos
                PdfPTable table = new PdfPTable(4);
                table.WidthPercentage = 100;

                // Definindo as larguras relativas das colunas
                float[] widths = new float[] { 1f, 3f, 3f, 3f };
                table.SetWidths(widths);

                // Adicionando cabeçalhos
                string[] headers = { "ID", "Data", "Hora", "Ação" };
                foreach (string header in headers)
                {
                    PdfPCell headerCell = new PdfPCell(new Phrase(header, headerFont))
                    {
                        BackgroundColor = BaseColor.LIGHT_GRAY,
                        HorizontalAlignment = Element.ALIGN_CENTER
                    };
                    table.AddCell(headerCell);
                }

                // Adicionando dados à tabela com base no DataGridView
                foreach (DataGridViewRow row in dataGridView.Rows)
                {
                    if (!row.IsNewRow) // Evita adicionar a linha de novos registros (em edição)
                    {
                        foreach (DataGridViewCell cell in row.Cells)
                        {
                            PdfPCell pdfCell = new PdfPCell(new Phrase(cell.Value?.ToString() ?? string.Empty, bodyFont))
                            {
                                HorizontalAlignment = Element.ALIGN_CENTER,
                                FixedHeight = 15f  // Ajuste a altura da célula
                            };
                            table.AddCell(pdfCell);
                        }
                    }
                }

                doc.Add(table);

                // Rodapé com assinatura
                Paragraph assinatura = new Paragraph("\n\nAssinatura do Funcionário: _______________________________", bodyFont)
                {
                    Alignment = Element.ALIGN_LEFT,
                    SpacingBefore = 30f
                };
                doc.Add(assinatura);

                doc.Close();
            
            }
        }

        
    }
}

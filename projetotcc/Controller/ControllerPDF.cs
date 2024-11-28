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
        #region Relatório Detalhado
        public void GerarRelatorioDetalhado(DataGridView dataGridView, string nome, string id)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "PDF Files (*.pdf)|*.pdf";
            saveFileDialog.Title = "Salvar PDF";
            saveFileDialog.FileName = $"Relatório-{nome}.pdf";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                string caminhoArquivo = saveFileDialog.FileName;

                Document doc = new Document(PageSize.A4, 20, 20, 30, 30);
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

                
                PdfPTable infoTable = new PdfPTable(2);
                infoTable.WidthPercentage = 100;
                infoTable.SpacingAfter = 20f;

                PdfPCell cell1 = new PdfPCell(new Phrase($"Funcionário: {nome}", bodyFont));
                cell1.Border = PdfPCell.NO_BORDER;
                infoTable.AddCell(cell1);

                PdfPCell cell2 = new PdfPCell(new Phrase($"Código: {id}", bodyFont))
                {
                    HorizontalAlignment = Element.ALIGN_RIGHT
                };
                cell2.Border = PdfPCell.NO_BORDER;
                infoTable.AddCell(cell2);

                doc.Add(infoTable);

                
                PdfPTable table = new PdfPTable(5);
                table.WidthPercentage = 100;

                
                float[] widths = new float[] { 1f, 3f, 3f, 2f, 2f };
                table.SetWidths(widths);

                
                string[] headers = { "TOTAL HORAS", "HORARIO INICIO", "HORARIO FIM", "DATA", "NOME" };
                foreach (string header in headers)
                {
                    PdfPCell headerCell = new PdfPCell(new Phrase(header, headerFont))
                    {
                        BackgroundColor = BaseColor.LIGHT_GRAY,
                        HorizontalAlignment = Element.ALIGN_CENTER
                    };
                    table.AddCell(headerCell);
                }

                
                foreach (DataGridViewRow row in dataGridView.Rows)
                {
                    if (!row.IsNewRow)
                    {
                        foreach (DataGridViewCell cell in row.Cells)
                        {
                            PdfPCell pdfCell = new PdfPCell(new Phrase(cell.Value?.ToString() ?? string.Empty, bodyFont))
                            {
                                HorizontalAlignment = Element.ALIGN_CENTER,
                                FixedHeight = 15f 
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
        #endregion
        public void GerarRelatorioSimples(DataGridView dataGridView, string dataInicial, string dataFinal)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "PDF Files (*.pdf)|*.pdf";
            saveFileDialog.Title = "Salvar PDF";
            saveFileDialog.FileName = "Relatório-Geral.pdf";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                string caminhoArquivo = saveFileDialog.FileName;

                Document doc = new Document(PageSize.A4, 20, 20, 30, 30); 
                PdfWriter.GetInstance(doc, new FileStream(caminhoArquivo, FileMode.Create));

                doc.Open();

                
                Font titleFont = FontFactory.GetFont("Arial", 14, Font.BOLD, BaseColor.BLACK);
                Font headerFont = FontFactory.GetFont("Arial", 10, Font.BOLD, BaseColor.BLACK);
                Font bodyFont = FontFactory.GetFont("Arial", 9, Font.NORMAL, BaseColor.BLACK);

                
                Paragraph title = new Paragraph($"Relatório Geral - {dataInicial} à {dataFinal}", titleFont)
                {
                    Alignment = Element.ALIGN_CENTER,
                    SpacingAfter = 20f
                };
                doc.Add(title);

                
                PdfPTable tabela = new PdfPTable(4)
                {
                    WidthPercentage = 100 
                };

                
                tabela.SetWidths(new float[] { 1, 1, 1.5f, 1 });

                
                string[] cabecalhos = { "NOME", "DIAS PRESENTES", "HORAS TRABALHADAS", "ASSINATURA" };
                foreach (string cabecalho in cabecalhos)
                {
                    PdfPCell celula = new PdfPCell(new Phrase(cabecalho, headerFont))
                    {
                        BackgroundColor = BaseColor.LIGHT_GRAY,
                        HorizontalAlignment = Element.ALIGN_CENTER,
                        Padding = 5
                    };
                    tabela.AddCell(celula);
                }

                
                foreach (DataGridViewRow linha in dataGridView.Rows)
                {
                    if (!linha.IsNewRow)
                    {

                        tabela.AddCell(new PdfPCell(new Phrase(linha.Cells["NOME"].Value?.ToString(), bodyFont))
                        {
                            HorizontalAlignment = Element.ALIGN_CENTER
                        });
                        tabela.AddCell(new PdfPCell(new Phrase(linha.Cells["DIAS PRESENTES"].Value?.ToString(), bodyFont))
                        {
                            HorizontalAlignment = Element.ALIGN_CENTER
                        });
                        tabela.AddCell(new PdfPCell(new Phrase(linha.Cells["HORAS TRABALHADAS"].Value?.ToString(), bodyFont))
                        {
                            HorizontalAlignment = Element.ALIGN_CENTER
                        });


                        PdfPCell celulaAssinatura = new PdfPCell(new Phrase(" ", bodyFont))
                        {
                            FixedHeight = 20f, 
                            BorderWidth = 1,
                            HorizontalAlignment = Element.ALIGN_CENTER
                        };
                        tabela.AddCell(celulaAssinatura);
                    }
                }

                
                doc.Add(tabela);

                
                doc.Close();

                MessageBox.Show("Relatório gerado com sucesso em: " + caminhoArquivo);
            }
        }

        public void GerarRelatoriu(DataGridView dataGridView, string nome, string id)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "PDF Files (*.pdf)|*.pdf";
            saveFileDialog.Title = "Salvar PDF";
            saveFileDialog.FileName = $"Relatório-{nome}.pdf";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                string caminhoArquivo = saveFileDialog.FileName;

                Document doc = new Document(PageSize.A4, 20, 20, 30, 30);
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


                PdfPTable table = new PdfPTable(5);
                table.WidthPercentage = 100;


                float[] widths = new float[] { 1f, 3f, 3f, 2f, 2f };
                table.SetWidths(widths);


                string[] headers = { "TOTAL HORAS", "HORARIO INICIO", "HORARIO FIM", "DATA", "NOME" };
                foreach (string header in headers)
                {
                    PdfPCell headerCell = new PdfPCell(new Phrase(header, headerFont))
                    {
                        BackgroundColor = BaseColor.LIGHT_GRAY,
                        HorizontalAlignment = Element.ALIGN_CENTER
                    };
                    table.AddCell(headerCell);
                }


                foreach (DataGridViewRow row in dataGridView.Rows)
                {
                    if (!row.IsNewRow)
                    {
                        foreach (DataGridViewCell cell in row.Cells)
                        {
                            PdfPCell pdfCell = new PdfPCell(new Phrase(cell.Value?.ToString() ?? string.Empty, bodyFont))
                            {
                                HorizontalAlignment = Element.ALIGN_CENTER,
                                FixedHeight = 15f
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

        public void AjustarColunasParaImpressao(DataGridView dgv)
        {
            foreach (DataGridViewColumn column in dgv.Columns)
            {
                column.Width = Math.Max(column.Width, 30); // Ajuste a largura mínima como achar melhor
            }
        }
    }
}
    

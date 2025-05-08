using System;
using System.Data;
using System.Windows.Forms;
using MySql.Data.MySqlClient; 
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace JFT
{
    public partial class Transact : Form
    {
        public Transact()
        {
            InitializeComponent();
        }

        
        private string connectionString = "Server=127.0.0.1;Database=jft;Uid=root;Pwd=122996;";

        private void btn_GenReport_Click(object sender, EventArgs e)
        {
            
            LoadDataToGrid();

            
            string folderPath = @"D:\ISKOL BOKOL\tird year gaw\2nd sem\JFT REPORT"; 
            Directory.CreateDirectory(folderPath);
            string filePath = Path.Combine(folderPath, $"Report_{DateTime.Now:yyyyMMdd_HHmmss}.pdf");

            
            ExportGridToPDF(filePath);

            MessageBox.Show("PDF report generated at:\n" + filePath, "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void LoadDataToGrid()
        {
            string query = "SELECT username, product_id, quantity, price, order_date FROM orders;"; 
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                MySqlDataAdapter adapter = new MySqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                dataGridView1.DataSource = dt;
            }
        }

        private void ExportGridToPDF(string filePath)
        {
            Document doc = new Document(PageSize.A4, 10f, 10f, 10f, 10f);

            try
            {
                PdfWriter.GetInstance(doc, new FileStream(filePath, FileMode.Create));
                doc.Open();

                
                string logoPath = @"D:\ISKOL BOKOL\tird year gaw\2nd sem\JFT REPORT\jft.png";
                if (File.Exists(logoPath))
                {
                    iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(logoPath);
                    logo.ScaleToFit(100f, 100f);
                    logo.Alignment = Element.ALIGN_CENTER;
                    doc.Add(logo);
                }


                Paragraph title = new Paragraph("JFT - Transaction Report", FontFactory.GetFont("Arial", 16, iTextSharp.text.Font.BOLD));
                title.Alignment = Element.ALIGN_CENTER;
                doc.Add(title);

                doc.Add(new Paragraph("Generated: " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
                doc.Add(new Paragraph(" "));

                
                PdfPTable table = new PdfPTable(dataGridView1.Columns.Count);
                table.WidthPercentage = 100;

                
                foreach (DataGridViewColumn column in dataGridView1.Columns)
                {
                    PdfPCell headerCell = new PdfPCell(new Phrase(column.HeaderText));
                    headerCell.BackgroundColor = BaseColor.LIGHT_GRAY;
                    table.AddCell(headerCell);
                }

                
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    if (!row.IsNewRow)
                    {
                        foreach (DataGridViewCell cell in row.Cells)
                        {
                            table.AddCell(cell.Value?.ToString() ?? "");
                        }
                    }
                }

                doc.Add(table);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error generating PDF: " + ex.Message);
            }
            finally
            {
                doc.Close();
            }
        }
    }
}

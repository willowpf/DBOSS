using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace JFT
{
    public partial class ReportForm : Form
    {
        private string connectionString = "Server=127.0.0.1;Database=jft;Uid=root;Pwd=122996;";

        public ReportForm()
        {
            InitializeComponent();
        }

        private void btnSubmitReport_Click(object sender, EventArgs e)
        {
            string description = txtDescription.Text.Trim();

            if (string.IsNullOrEmpty(description))
            {
                MessageBox.Show("Please enter a description of your problem.");
                return;
            }

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "INSERT INTO reports (username, description) VALUES (@username, @description)";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@username", Form2.LoggedInUsername);
                        cmd.Parameters.AddWithValue("@description", description);
                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Report submitted successfully. Thank you!");
                this.Close(); // Close the form after submitting
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to submit report: " + ex.Message);
            }
        }
    }
}


using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient; // Add this line to include MySQL support
using BCrypt.Net; // Add this line to include BCrypt support

namespace JFT
{
    public partial class Form3 : Form
    {
        private string connectionString = "Server=127.0.0.1;Database=jft;Uid=root;Pwd=122996;";

        public Form3()
        {
            InitializeComponent();
            this.textBox2.PasswordChar = '*';
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Retrieve input from text boxes
            string username = textBox1.Text;
            string password = textBox2.Text;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please enter both username and password.");
                return;
            }

            // Hash the password
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();

                    // Check if the username already exists
                    string checkQuery = "SELECT COUNT(*) FROM accs WHERE username = @username";
                    using (MySqlCommand checkCmd = new MySqlCommand(checkQuery, conn))
                    {
                        checkCmd.Parameters.AddWithValue("@username", username);
                        int userExists = Convert.ToInt32(checkCmd.ExecuteScalar());

                        if (userExists > 0)
                        {
                            MessageBox.Show("Username already exists. Choose another one.");
                            return;
                        }
                    }

                    // Insert the new credentials into the database
                    string query = "INSERT INTO accs (username, password) VALUES (@username, @password)";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@username", username);
                        cmd.Parameters.AddWithValue("@password", hashedPassword);

                        int result = cmd.ExecuteNonQuery();

                        if (result > 0)
                        {
                            MessageBox.Show("Signup Successful!");
                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show("Signup Failed.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }
    }
}

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
using BCrypt.Net;

namespace JFT
{
    public partial class Form2 : Form
    {
        private string connectionString = "Server=127.0.0.1;Database=jft;Uid=root;Pwd=122996;";

        public Form2()
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

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();

                    // Check if the username exists and retrieve the hashed password and salt
                    string query = "SELECT password, salt FROM accs WHERE username = @username";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@username", username);
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string storedHashedPassword = reader["password"].ToString();
                                string storedSalt = reader["salt"].ToString();
                                string saltedEnteredPassword = password + storedSalt;

                                // Verify the entered password against the stored hashed password
                                bool isPasswordValid = BCrypt.Net.BCrypt.Verify(saltedEnteredPassword, storedHashedPassword);

                                if (isPasswordValid)
                                {
                                    MessageBox.Show("Login Successful!");

                                    
                                    MainMenu mainMenu = new MainMenu();
                                    mainMenu.ShowDialog();
                                    this.Hide();
                                    // MAIN APP HERE
                                }
                                else
                                {
                                    MessageBox.Show("Invalid username or password.");
                                }
                            }
                            else
                            {
                                MessageBox.Show("Invalid username or password.");
                            }
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

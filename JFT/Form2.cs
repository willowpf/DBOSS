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
        public static string LoggedInUsername;
        private void button1_Click(object sender, EventArgs e)
        {
            string username = textBox1.Text.Trim();
            string password = textBox2.Text;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please enter both username and password.");
                return;
            }

                
            if (username == "admin1" && password == "admin123")
            {
                MessageBox.Show("Admin login successful!");
                MainMenu adminPage = new MainMenu(); // Admin page
                adminPage.ShowDialog();
                this.Close();
                return;
            }

            
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
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

                                bool isPasswordValid = BCrypt.Net.BCrypt.Verify(saltedEnteredPassword, storedHashedPassword);

                                if (isPasswordValid)
                                {
                                    MessageBox.Show("User login successful!");
                                    Form userPage = new UserPage(); 
                                    userPage.ShowDialog();
                                    this.Close();
                                    Form2.LoggedInUsername = username; 

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



        private void btnForgotPassword_Click(object sender, EventArgs e)
        {
            ChangePass changePassForm = new ChangePass();
            changePassForm.ShowDialog();
        }
    }
}

using System;
using System.Data;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using BCrypt.Net;

namespace JFT
{
    public partial class Form3 : Form
    {
        private string connectionString = "Server=127.0.0.1;Database=jft;Uid=root;Pwd=122996;";

        public Form3()
        {
            InitializeComponent();
            textBox2.PasswordChar = '*'; // Password textbox
            textBox3.PasswordChar = '*'; // Confirm password textbox
            textBox4.PasswordChar = '*'; // Security answer textbox

            // Populate the ComboBox with security questions
            comboBox1.Items.Add("What was your first pet's name?");
            comboBox1.Items.Add("What is your favorite book?");
            comboBox1.SelectedIndex = 0; // Default selection
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string username = textBox1.Text.Trim();
            string password = textBox2.Text;
            string confirmPassword = textBox3.Text;
            string securityQuestion = comboBox1.SelectedItem?.ToString();
            string securityAnswer = textBox4.Text.Trim();

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) ||
                string.IsNullOrEmpty(confirmPassword) || string.IsNullOrEmpty(securityQuestion) ||
                string.IsNullOrEmpty(securityAnswer))
            {
                MessageBox.Show("Please fill in all fields.");
                return;
            }

            if (password != confirmPassword)
            {
                MessageBox.Show("Passwords do not match. Please try again.");
                return;
            }

            
            string passwordPattern = @"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[\W_])[A-Za-z\d\W_]{14,}$";

            if (!Regex.IsMatch(password, passwordPattern))
            {
                MessageBox.Show("Password must be at least 14 characters long, include uppercase, lowercase, numbers, and special characters.");
                return;
            }

            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password); // Hash without extra salt
            string hashedSecurityAnswer = BCrypt.Net.BCrypt.HashPassword(securityAnswer.ToLower()); 

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();

                    // Check if username already exists
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

                    // Insert user into the database
                    string query = "INSERT INTO accs (username, password, security_question, security_answer) VALUES (@username, @password, @securityQuestion, @securityAnswer)";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@username", username);
                        cmd.Parameters.AddWithValue("@password", hashedPassword);
                        cmd.Parameters.AddWithValue("@securityQuestion", securityQuestion);
                        cmd.Parameters.AddWithValue("@securityAnswer", hashedSecurityAnswer);
                        

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

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
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
            this.textBox2.PasswordChar = '*'; //Password textvox
            this.textBox3.PasswordChar = '*'; // Confirm password textbox
            this.textBox4.PasswordChar = '*'; //Answer of sec. question

            // Populate the ComboBox with security questions
            comboBox1.Items.Add("What is your mother's maiden name?");
            comboBox1.Items.Add("What was your first pet's name?");
            comboBox1.Items.Add("What is the name of your favorite teacher?");
            comboBox1.Items.Add("What was the name of your first school?");
            comboBox1.Items.Add("What is your favorite book?");

            comboBox1.SelectedIndex = 0; // Set default selected item
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string username = textBox1.Text;
            string password = textBox2.Text;
            string confirmPassword = textBox3.Text; // Confirm password input
            string securityQuestion = comboBox1.SelectedItem?.ToString(); // Get selected security question
            string securityAnswer = textBox4.Text; // Get security answer input

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(confirmPassword) || string.IsNullOrEmpty(securityQuestion) || string.IsNullOrEmpty(securityAnswer))
            {
                MessageBox.Show("Please fill in all fields, including security question and answer.");
                return;
            }

            if (password != confirmPassword)
            {
                MessageBox.Show("Passwords do not match. Please try again.");
                return;
            }

            string passwordPattern = @"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{14,}$";
            if (!Regex.IsMatch(password, passwordPattern))
            {
                MessageBox.Show("Password must be at least 14 characters long, include upper and lower case letters, numbers, and special characters.");
                return;
            }

            string salt = BCrypt.Net.BCrypt.GenerateSalt();
            string saltedPassword = password + salt;
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(saltedPassword);
            string hashedSecurityAnswer = BCrypt.Net.BCrypt.HashPassword(securityAnswer);

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();

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

                    string query = "INSERT INTO accs (username, password, salt, security_question, security_answer) VALUES (@username, @password, @salt, @securityQuestion, @securityAnswer)";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@username", username);
                        cmd.Parameters.AddWithValue("@password", hashedPassword);
                        cmd.Parameters.AddWithValue("@salt", salt);
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

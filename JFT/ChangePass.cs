using System;
using System.Data;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using BCrypt.Net;
using System.Security.Cryptography;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace JFT
{
    public partial class ChangePass : Form
    {
        private string connectionString = "Server=127.0.0.1;Database=jft;Uid=root;Pwd=122996;";
        private string generatedOTP;
        private DateTime otpGeneratedTime;
        private string storedHashedAnswer;
        private string retrievedUsername;

        public ChangePass()
        {
            InitializeComponent();
            comboBox1.Items.AddRange(new string[]
            {
                "What is your mother's maiden name?",
                "What was your first pet's name?",
                "What is the name of your favorite teacher?",
                "What was the name of your first school?",
                "What is your favorite book?"
            });
            comboBox1.SelectedIndex = 0;
        }

        private void btnVerifySecurityQuestion_Click(object sender, EventArgs e)
        {
            string username = textBoxUsername.Text;
            string securityAnswer = textBoxSecurityAnswer.Text;

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT security_question, security_answer FROM accs WHERE username = @username";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@username", username);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string storedQuestion = reader["security_question"].ToString();
                            storedHashedAnswer = reader["security_answer"].ToString();

                            // Display the security question to the user
                            labelSecurityQuestion.Text = storedQuestion;
                            labelSecurityQuestion.Visible = true;

                            if (BCrypt.Net.BCrypt.Verify(securityAnswer, storedHashedAnswer))
                            {
                                MessageBox.Show("User verified.");
                                GenerateOTP();
                            }
                            else
                            {
                                MessageBox.Show("Incorrect security answer.");
                            }
                        }
                        else
                        {
                            MessageBox.Show("User not found.");
                        }
                    }
                }
            }
        }


        private void GenerateOTP()
        {
            Random rand = new Random();
            generatedOTP = rand.Next(100000, 999999).ToString();
            otpGeneratedTime = DateTime.Now;

            MessageBox.Show("Your OTP is: " + generatedOTP);
        }

        private void btnVerifyOTP_Click(object sender, EventArgs e)
        {
            string enteredOTP = textBoxOTP.Text;
            if (enteredOTP == generatedOTP && (DateTime.Now - otpGeneratedTime).TotalMinutes <= 2)
            {
                MessageBox.Show("OTP verified. You may now reset your password.");
                btnSendOTP.Visible = true;
            }
            else
            {
                MessageBox.Show("Invalid or expired OTP.");
            }
        }

        private void btnResetPassword_Click(object sender, EventArgs e)
        {
            string newPassword = textBoxNewPassword.Text;
            string confirmPassword = textBoxConfirmPassword.Text;

            if (newPassword != confirmPassword)
            {
                MessageBox.Show("Passwords do not match.");
                return;
            }

            string passwordPattern = @"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{14,}$";
            if (!Regex.IsMatch(newPassword, passwordPattern))
            {
                MessageBox.Show("Password must meet security requirements.");
                return;
            }

            string salt = BCrypt.Net.BCrypt.GenerateSalt();
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(newPassword + salt);

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = "UPDATE accs SET password = @password, salt = @salt WHERE username = @username";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@password", hashedPassword);
                    cmd.Parameters.AddWithValue("@salt", salt);
                    cmd.Parameters.AddWithValue("@username", retrievedUsername);

                    int result = cmd.ExecuteNonQuery();
                    if (result > 0)
                    {
                        MessageBox.Show("Password reset successful!");
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Password reset failed.");
                    }
                }
            }
        }
    }
}

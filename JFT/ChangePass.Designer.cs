namespace JFT
{
    partial class ChangePass
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.labelSecurityQuestion = new System.Windows.Forms.Label();
            this.textBoxNewPassword = new System.Windows.Forms.TextBox();
            this.textBoxConfirmPassword = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.textBoxSecurityAnswer = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.textBoxOTP = new System.Windows.Forms.TextBox();
            this.btnSendOTP = new System.Windows.Forms.Button();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxUsername = new System.Windows.Forms.TextBox();
            this.btnVerifySecurityQuestion = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(128, 232);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(97, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "New Password";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(110, 260);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(145, 16);
            this.label2.TabIndex = 1;
            this.label2.Text = "Confrim New Password";
            // 
            // labelSecurityQuestion
            // 
            this.labelSecurityQuestion.AutoSize = true;
            this.labelSecurityQuestion.Location = new System.Drawing.Point(155, 129);
            this.labelSecurityQuestion.Name = "labelSecurityQuestion";
            this.labelSecurityQuestion.Size = new System.Drawing.Size(111, 16);
            this.labelSecurityQuestion.TabIndex = 2;
            this.labelSecurityQuestion.Text = "Security Question";
            this.labelSecurityQuestion.EnabledChanged += new System.EventHandler(this.btnVerifySecurityQuestion_Click);
            // 
            // textBoxNewPassword
            // 
            this.textBoxNewPassword.Location = new System.Drawing.Point(291, 226);
            this.textBoxNewPassword.Name = "textBoxNewPassword";
            this.textBoxNewPassword.Size = new System.Drawing.Size(160, 22);
            this.textBoxNewPassword.TabIndex = 3;
            // 
            // textBoxConfirmPassword
            // 
            this.textBoxConfirmPassword.Location = new System.Drawing.Point(291, 254);
            this.textBoxConfirmPassword.Name = "textBoxConfirmPassword";
            this.textBoxConfirmPassword.Size = new System.Drawing.Size(160, 22);
            this.textBoxConfirmPassword.TabIndex = 4;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(174, 171);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(51, 16);
            this.label4.TabIndex = 5;
            this.label4.Text = "Answer";
            // 
            // textBoxSecurityAnswer
            // 
            this.textBoxSecurityAnswer.Location = new System.Drawing.Point(291, 171);
            this.textBoxSecurityAnswer.Name = "textBoxSecurityAnswer";
            this.textBoxSecurityAnswer.Size = new System.Drawing.Size(160, 22);
            this.textBoxSecurityAnswer.TabIndex = 6;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(275, 337);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(160, 91);
            this.button1.TabIndex = 8;
            this.button1.Text = "Change Password";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.btnResetPassword_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(155, 289);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(35, 16);
            this.label5.TabIndex = 9;
            this.label5.Text = "OTP";
            // 
            // textBoxOTP
            // 
            this.textBoxOTP.Location = new System.Drawing.Point(291, 286);
            this.textBoxOTP.Name = "textBoxOTP";
            this.textBoxOTP.Size = new System.Drawing.Size(160, 22);
            this.textBoxOTP.TabIndex = 10;
            // 
            // btnSendOTP
            // 
            this.btnSendOTP.Location = new System.Drawing.Point(475, 274);
            this.btnSendOTP.Name = "btnSendOTP";
            this.btnSendOTP.Size = new System.Drawing.Size(175, 47);
            this.btnSendOTP.TabIndex = 11;
            this.btnSendOTP.Text = "Verify OTP";
            this.btnSendOTP.UseVisualStyleBackColor = true;
            this.btnSendOTP.Click += new System.EventHandler(this.btnVerifyOTP_Click);
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(291, 129);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(160, 24);
            this.comboBox1.TabIndex = 12;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(174, 82);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(79, 16);
            this.label3.TabIndex = 13;
            this.label3.Text = "User Name:";
            // 
            // textBoxUsername
            // 
            this.textBoxUsername.Location = new System.Drawing.Point(291, 76);
            this.textBoxUsername.Name = "textBoxUsername";
            this.textBoxUsername.Size = new System.Drawing.Size(160, 22);
            this.textBoxUsername.TabIndex = 14;
            // 
            // btnVerifySecurityQuestion
            // 
            this.btnVerifySecurityQuestion.Location = new System.Drawing.Point(475, 108);
            this.btnVerifySecurityQuestion.Name = "btnVerifySecurityQuestion";
            this.btnVerifySecurityQuestion.Size = new System.Drawing.Size(152, 58);
            this.btnVerifySecurityQuestion.TabIndex = 15;
            this.btnVerifySecurityQuestion.Text = "Verify User";
            this.btnVerifySecurityQuestion.UseVisualStyleBackColor = true;
            this.btnVerifySecurityQuestion.Click += new System.EventHandler(this.btnVerifySecurityQuestion_Click);
            // 
            // ChangePass
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btnVerifySecurityQuestion);
            this.Controls.Add(this.textBoxUsername);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.btnSendOTP);
            this.Controls.Add(this.textBoxOTP);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.textBoxSecurityAnswer);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.textBoxConfirmPassword);
            this.Controls.Add(this.textBoxNewPassword);
            this.Controls.Add(this.labelSecurityQuestion);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "ChangePass";
            this.Text = "Change Password";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label labelSecurityQuestion;
        private System.Windows.Forms.TextBox textBoxNewPassword;
        private System.Windows.Forms.TextBox textBoxConfirmPassword;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBoxSecurityAnswer;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBoxOTP;
        private System.Windows.Forms.Button btnSendOTP;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxUsername;
        private System.Windows.Forms.Button btnVerifySecurityQuestion;
    }
}
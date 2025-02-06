using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient; // Add this line to include MySQL support

namespace JFT
{
    public partial class Form1 : Form
    {
        // Define the connection string as a class-level variable
        private string connectionString = "Server=127.0.0.1;Database=jft;Uid=root;Pwd=122996;";

        public Form1()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form3 form3 = new Form3();
            form3.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2();
            form2.ShowDialog();
        }

       
    }
}

﻿using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace JFT
{
    public partial class UserPage : Form
    {
        private string connectionString = "Server=127.0.0.1;Database=jft;Uid=root;Pwd=122996;";
        private DataTable orderTable;

        public UserPage()
        {
            InitializeComponent();
            LoadProducts();
            InitializeOrderTable();

            txtProductID.ReadOnly = true;
            txtProductName.ReadOnly = true;
            txtPrice.ReadOnly = true;
            numericQuantity.Minimum = 1;
            numericQuantity.Maximum = 100;
        }

        private void LoadProducts()
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT * FROM products";
                MySqlDataAdapter adapter = new MySqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                dataGridViewProducts.DataSource = dt;
            }
        }

        private void InitializeOrderTable()
        {
            orderTable = new DataTable();
            orderTable.Columns.Add("ProductID");
            orderTable.Columns.Add("ProductName");
            orderTable.Columns.Add("Price", typeof(decimal));
            orderTable.Columns.Add("Quantity", typeof(int));
            orderTable.Columns.Add("Total", typeof(decimal), "Price * Quantity");

            dataGridViewOrders.DataSource = orderTable;
        }

        private void dataGridViewProducts_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dataGridViewProducts.Rows[e.RowIndex].Cells["product_id"].Value != null)
            {
                DataGridViewRow row = dataGridViewProducts.Rows[e.RowIndex];

                txtProductID.Text = row.Cells["product_id"].Value.ToString();
                txtProductName.Text = row.Cells["product_name"].Value.ToString();
                txtPrice.Text = row.Cells["price"].Value.ToString();

                string imagePath = row.Cells["image_path"].Value.ToString();
                if (!string.IsNullOrEmpty(imagePath) && File.Exists(imagePath))
                {
                    pictureBoxProduct.Image = Image.FromFile(imagePath);
                    pictureBoxProduct.SizeMode = PictureBoxSizeMode.Zoom;
                }
                else
                {
                    pictureBoxProduct.Image = null;
                }
            }
        }

        private void btnOrder_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtProductID.Text))
            {
                MessageBox.Show("Please select a product.");
                return;
            }

            int quantity = (int)numericQuantity.Value;
            decimal price = decimal.Parse(txtPrice.Text);

            
            foreach (DataRow row in orderTable.Rows)
            {
                if (row["ProductID"].ToString() == txtProductID.Text)
                {
                    row["Quantity"] = quantity;
                    return;
                }
            }

            orderTable.Rows.Add(txtProductID.Text, txtProductName.Text, price, quantity);
        }

        private void btnDeleteOrder_Click(object sender, EventArgs e)
        {
            if (dataGridViewOrders.SelectedRows.Count > 0)
            {
                foreach (DataGridViewRow row in dataGridViewOrders.SelectedRows)
                {
                    dataGridViewOrders.Rows.RemoveAt(row.Index);
                }
            }
            else
            {
                MessageBox.Show("Please select a row to delete.");
            }
        }

        private void btnUpdateOrder_Click(object sender, EventArgs e)
        {
            if (dataGridViewOrders.SelectedRows.Count > 0)
            {
                var row = dataGridViewOrders.SelectedRows[0];
                int quantity = (int)numericQuantity.Value;
                row.Cells["Quantity"].Value = quantity;
            }
            else
            {
                MessageBox.Show("Please select a row to update.");
            }
        }

        private void btnCheckout_Click(object sender, EventArgs e)
        {
            if (orderTable.Rows.Count == 0)
            {
                MessageBox.Show("You must add at least one product before checking out.");
                return;
            }

            int totalQuantity = 0;
            decimal totalPrice = 0;

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                using (MySqlTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        foreach (DataRow row in orderTable.Rows)
                        {
                            int productId = Convert.ToInt32(row["ProductID"]);
                            int quantity = Convert.ToInt32(row["Quantity"]);
                            decimal price = Convert.ToDecimal(row["Price"]);

                            string insertQuery = @"INSERT INTO orders (username, product_id, quantity, price) 
                                           VALUES (@username, @product_id, @quantity, @price)";
                            using (MySqlCommand cmd = new MySqlCommand(insertQuery, conn, transaction))
                            {
                                cmd.Parameters.AddWithValue("@username", Form2.LoggedInUsername);
                                cmd.Parameters.AddWithValue("@product_id", productId);
                                cmd.Parameters.AddWithValue("@quantity", quantity);
                                cmd.Parameters.AddWithValue("@price", price);
                                cmd.ExecuteNonQuery();
                            }

                            totalQuantity += quantity;
                            totalPrice += price * quantity;
                        }

                        transaction.Commit();
                        MessageBox.Show($"Checkout complete!\nTotal Items: {totalQuantity}\nTotal Price: ₱{totalPrice}", "Success");
                        orderTable.Clear();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        MessageBox.Show("Checkout failed: " + ex.Message);
                    }
                }
            }
        }


        private void btn_Report_Click(object sender, EventArgs e)
        {
            ReportForm reports = new ReportForm();
            reports.ShowDialog();
        }
    }
}

using System;
using System.Data;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Drawing; // Required for Image handling

namespace JFT
{
    public partial class MainMenu : Form
    {
        private string connectionString = "Server=127.0.0.1;Database=jft;Uid=root;Pwd=122996;";

        public MainMenu()
        {
            InitializeComponent();
            LoadProducts();
            LoadColorOptions();
            LoadSizeOptions();
        }

        // Load predefined colors into ComboBox
        private void LoadColorOptions()
        {
            cmbColor.Items.AddRange(new string[] { "Red", "Blue", "Black", "White", "Green", "Yellow" });
            cmbColor.SelectedIndex = 0;
        }

        private void LoadSizeOptions()
        {
            cmbSize.Items.AddRange(new string[] { "Small", "Medium", "Large", "X-Large" });
            cmbSize.SelectedIndex = 0;
        }

        // Load Products into DataGridView
        private void LoadProducts()
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT * FROM products";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    dataGridViewProducts.DataSource = dt;
                }
            }
        }

        // Browse and Select an Image
        private void btnBrowseImage_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif";
                openFileDialog.Title = "Select an Image";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    txtImagePath.Text = openFileDialog.FileName;
                    pictureBoxProduct.Image = Image.FromFile(openFileDialog.FileName); // Display image
                }
            }
        }

        // Add a New Product
        private void btnAddProduct_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtProductName.Text) || cmbSize.SelectedItem == null ||
                cmbColor.SelectedItem == null || string.IsNullOrWhiteSpace(txtPrice.Text) ||
                string.IsNullOrWhiteSpace(txtStock.Text) || string.IsNullOrWhiteSpace(txtImagePath.Text))
            {
                MessageBox.Show("Please fill all fields and select an image.");
                return;
            }

            string productName = txtProductName.Text;
            string size = cmbSize.SelectedItem.ToString();
            string color = cmbColor.SelectedItem.ToString();
            decimal price = Convert.ToDecimal(txtPrice.Text);
            int stock = Convert.ToInt32(txtStock.Text);
            string imagePath = txtImagePath.Text;

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = "INSERT INTO products (product_name, size, color, price, stock, image_path) " +
                               "VALUES (@product_name, @size, @color, @price, @stock, @image_path)";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@product_name", productName);
                    cmd.Parameters.AddWithValue("@size", size);
                    cmd.Parameters.AddWithValue("@color", color);
                    cmd.Parameters.AddWithValue("@price", price);
                    cmd.Parameters.AddWithValue("@stock", stock);
                    cmd.Parameters.AddWithValue("@image_path", imagePath);

                    cmd.ExecuteNonQuery();
                }
            }

            MessageBox.Show("Product added successfully!");
            LoadProducts();
        }

        // Update a Product
        private void btnUpdateProduct_Click(object sender, EventArgs e)
        {
            if (dataGridViewProducts.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a product to update.");
                return;
            }

            int productId = Convert.ToInt32(txtProductID.Text);
            string productName = txtProductName.Text;
            string size = cmbSize.SelectedItem.ToString();
            string color = cmbColor.SelectedItem.ToString();
            decimal price = Convert.ToDecimal(txtPrice.Text);
            int stock = Convert.ToInt32(txtStock.Text);
            string imagePath = txtImagePath.Text;

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();

                // Check if the product_id exists
                string checkQuery = "SELECT COUNT(*) FROM products WHERE product_id = @product_id";
                using (MySqlCommand checkCmd = new MySqlCommand(checkQuery, conn))
                {
                    checkCmd.Parameters.AddWithValue("@product_id", productId);
                    int count = Convert.ToInt32(checkCmd.ExecuteScalar());

                    // If product_id doesn't exist, find the lowest available ID
                    if (count == 0)
                    {
                        string getNewIdQuery = "SELECT MIN(t1.product_id + 1) " +
                                               "FROM products t1 " +
                                               "WHERE NOT EXISTS (SELECT 1 FROM products t2 WHERE t2.product_id = t1.product_id + 1)";
                        using (MySqlCommand newIdCmd = new MySqlCommand(getNewIdQuery, conn))
                        {
                            object newIdResult = newIdCmd.ExecuteScalar();
                            if (newIdResult != DBNull.Value)
                            {
                                productId = Convert.ToInt32(newIdResult);
                            }
                        }
                    }
                }

                // Update product details
                string query = "UPDATE products SET product_name = @product_name, size = @size, color = @color, " +
                               "price = @price, stock = @stock, image_path = @image_path WHERE product_id = @product_id";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@product_name", productName);
                    cmd.Parameters.AddWithValue("@size", size);
                    cmd.Parameters.AddWithValue("@color", color);
                    cmd.Parameters.AddWithValue("@price", price);
                    cmd.Parameters.AddWithValue("@stock", stock);
                    cmd.Parameters.AddWithValue("@image_path", imagePath);
                    cmd.Parameters.AddWithValue("@product_id", productId);

                    cmd.ExecuteNonQuery();
                }
            }

            MessageBox.Show("Product updated successfully!");
            LoadProducts();
        }


        // Delete a Product
        private void btnDeleteProduct_Click(object sender, EventArgs e)
        {
            if (dataGridViewProducts.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a product to delete.");
                return;
            }

            int productId = Convert.ToInt32(txtProductID.Text);

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = "DELETE FROM products WHERE product_id = @product_id";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@product_id", productId);
                    cmd.ExecuteNonQuery();
                }
            }

            MessageBox.Show("Product deleted successfully!");
            LoadProducts();
        }

        // Select a Product from DataGridView
        private void dataGridViewProducts_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dataGridViewProducts.Rows[e.RowIndex].Cells["product_id"].Value != null)
            {
                DataGridViewRow row = dataGridViewProducts.Rows[e.RowIndex];

                // Populate text fields with selected row data, handling null values safely
                txtProductID.Text = row.Cells["product_id"].Value?.ToString() ?? "";
                txtProductName.Text = row.Cells["product_name"].Value?.ToString() ?? "";
                txtPrice.Text = row.Cells["price"].Value?.ToString() ?? "";
                txtStock.Text = row.Cells["stock"].Value?.ToString() ?? "";
                txtImagePath.Text = row.Cells["image_path"].Value?.ToString() ?? "";

                // Handle Size selection
                string sizeValue = row.Cells["size"].Value?.ToString() ?? "";
                if (cmbSize.Items.Contains(sizeValue))
                {
                    cmbSize.SelectedItem = sizeValue;
                }
                else
                {
                    cmbSize.SelectedIndex = 0; // Default selection if size is invalid
                }

                // Handle Color selection
                string colorValue = row.Cells["color"].Value?.ToString() ?? "";
                if (cmbColor.Items.Contains(colorValue))
                {
                    cmbColor.SelectedItem = colorValue;
                }
                else
                {
                    cmbColor.SelectedIndex = 0; // Default selection if color is invalid
                }

                // Load and display the image if the file exists
                if (!string.IsNullOrEmpty(txtImagePath.Text) && System.IO.File.Exists(txtImagePath.Text))
                {
                    pictureBoxProduct.Image = Image.FromFile(txtImagePath.Text);
                }
                else
                {
                    pictureBoxProduct.Image = null; // Clear image if path is invalid
                }
            }
        }


        private void btnClearFields_Click(object sender, EventArgs e)
        {
            txtProductID.Clear();
            txtProductName.Clear();
            txtPrice.Clear();
            txtStock.Clear();
            txtImagePath.Clear();

            cmbSize.SelectedIndex = 0;
            cmbColor.SelectedIndex = 0;

            pictureBoxProduct.Image = null;
        }

        // Open Change Password Form
        private void button1_Click(object sender, EventArgs e)
        {
            ChangePass changePass = new ChangePass();
            changePass.ShowDialog();
        }

       
    }
}

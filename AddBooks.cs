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
using System.IO;

namespace LibraryManagementSystem
{
    public partial class AddBooks : UserControl
    {
        // MySQL connection string
        MySqlConnection con = new MySqlConnection(@"server=localhost;user=root;password=;database=library");

        // Constructor
        public AddBooks()
        {
            InitializeComponent();
        }

        // Method to refresh the displayed books data
        public async void refreshData()
        {
            if (InvokeRequired)
            {
                Invoke((MethodInvoker)refreshData);
                return;
            }

            await displayBooksAsync();
        }

        // Method to display books from the database asynchronously
        public async Task displayBooksAsync()
        {
            DataAddBooks dab = new DataAddBooks();
            List<DataAddBooks> listData = await Task.Run(() => dab.addBooksData());
            dataGridView1.DataSource = listData;
        }

        // Global variable to hold the image path
        private string imagePath;

        // Import image button click event
        private void addBooks_importBtn_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog dialog = new OpenFileDialog();
                dialog.Filter = "Image Files (*.jpg; *.png)|*.jpg;*.png";

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    imagePath = dialog.FileName;
                    addBooks_picture.ImageLocation = imagePath;  // Display the image
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Button to add a book
        private async void addBooks_addBtn_Click(object sender, EventArgs e)
        {
            // Check if required fields are empty
            if (addBooks_picture.Image == null || string.IsNullOrEmpty(addBooks_bookTitle.Text) || string.IsNullOrEmpty(addBooks_author.Text) || addBooks_published.Value == null || string.IsNullOrEmpty(addBooks_status.Text))
            {
                MessageBox.Show("Please fill all blank fields", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Validate that the 'status' is one of the allowed values: 'Available', 'Issued', or 'Reserved'
            if (addBooks_status.Text != "Available" && addBooks_status.Text != "Issued" && addBooks_status.Text != "Reserved")
            {
                MessageBox.Show("Please select a valid status: Available, Issued, or Reserved.", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                DateTime today = DateTime.Today;
                con.Open();

                // Prepare the insert statement
                string insertData = "INSERT INTO books (book_title, author, published_date, status, image, date_insert) " +
                                    "VALUES(@bookTitle, @author, @published_date, @status, @image, @dateInsert)";

                // Create the path for the image
                string path = Path.Combine(@"C:\Users\40737385\Desktop\Library-Management-System-using-CSharp-main\LibraryManagementSystem\LibraryManagementSystem",
                                            addBooks_bookTitle.Text.Trim() + addBooks_author.Text.Trim() + ".jpg");

                string directoryPath = Path.GetDirectoryName(path);
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);  // Create directory if it doesn't exist
                }

                File.Copy(addBooks_picture.ImageLocation, path, true);  // Copy the image to the folder

                // Insert the book data into the database
                using (MySqlCommand cmd = new MySqlCommand(insertData, con))
                {
                    cmd.Parameters.AddWithValue("@bookTitle", addBooks_bookTitle.Text.Trim());
                    cmd.Parameters.AddWithValue("@author", addBooks_author.Text.Trim());
                    cmd.Parameters.AddWithValue("@published_date", addBooks_published.Value);
                    cmd.Parameters.AddWithValue("@status", addBooks_status.Text.Trim());  // Insert the status correctly
                    cmd.Parameters.AddWithValue("@image", path);
                    cmd.Parameters.AddWithValue("@dateInsert", today);

                    await cmd.ExecuteNonQueryAsync();  // Execute the insert query asynchronously

                    // Refresh the data grid view
                    await displayBooksAsync();

                    MessageBox.Show("Added successfully!", "Information Message", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Clear fields after adding the book
                    clearFields();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                con.Close();
            }
        }

        // Method to clear the fields after a book is added
        private void clearFields()
        {
            addBooks_bookTitle.Text = "";
            addBooks_author.Text = "";
            addBooks_picture.Image = null;
            addBooks_status.SelectedIndex = -1;
        }

        // Handle when a row in the dataGridView is clicked
        private int bookID = 0;
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                bookID = (int)row.Cells[0].Value;
                addBooks_bookTitle.Text = row.Cells[1].Value.ToString();
                addBooks_author.Text = row.Cells[2].Value.ToString();
                addBooks_published.Text = row.Cells[3].Value.ToString();

                string imagePath = row.Cells[4].Value.ToString();
                if (imagePath != null || imagePath.Length >= 1)
                {
                    addBooks_picture.Image = Image.FromFile(imagePath);
                }
                else
                {
                    addBooks_picture.Image = null;
                }
                addBooks_status.Text = row.Cells[5].Value.ToString();
            }
        }

        // Button to clear the fields
        private void addBooks_clearBtn_Click(object sender, EventArgs e)
        {
            clearFields();
        }

        // Button to update a book
        private async void addBooks_updateBtn_Click(object sender, EventArgs e)
        {
            if (addBooks_picture.Image == null || string.IsNullOrEmpty(addBooks_bookTitle.Text) || string.IsNullOrEmpty(addBooks_author.Text) || addBooks_published.Value == null || string.IsNullOrEmpty(addBooks_status.Text))
            {
                MessageBox.Show("Please select item first", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (con.State != ConnectionState.Open)
            {
                DialogResult check = MessageBox.Show("Are you sure you want to UPDATE Book ID:" + bookID + "?", "Confirmation Message", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (check == DialogResult.Yes)
                {
                    try
                    {
                        con.Open();
                        DateTime today = DateTime.Today;

                        string updateData = "UPDATE books SET book_title = @bookTitle, author = @author, published_date = @published, status = @status, date_update = @dateUpdate WHERE id = @id";

                        using (MySqlCommand cmd = new MySqlCommand(updateData, con))
                        {
                            cmd.Parameters.AddWithValue("@bookTitle", addBooks_bookTitle.Text.Trim());
                            cmd.Parameters.AddWithValue("@author", addBooks_author.Text.Trim());
                            cmd.Parameters.AddWithValue("@published", addBooks_published.Value);
                            cmd.Parameters.AddWithValue("@status", addBooks_status.Text.Trim());
                            cmd.Parameters.AddWithValue("@dateUpdate", today);
                            cmd.Parameters.AddWithValue("@id", bookID);

                            await cmd.ExecuteNonQueryAsync();  // Execute the update asynchronously

                            await displayBooksAsync();  // Refresh the data

                            MessageBox.Show("Updated successfully!", "Information Message", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            clearFields();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error: " + ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    finally
                    {
                        con.Close();
                    }
                }
                else
                {
                    MessageBox.Show("Cancelled.", "Information Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        // Button to delete a book
        private async void addBooks_deleteBtn_Click(object sender, EventArgs e)
        {
            if (addBooks_picture.Image == null || string.IsNullOrEmpty(addBooks_bookTitle.Text) || string.IsNullOrEmpty(addBooks_author.Text) || addBooks_published.Value == null || string.IsNullOrEmpty(addBooks_status.Text))
            {
                MessageBox.Show("Please select item first", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (con.State != ConnectionState.Open)
            {
                DialogResult check = MessageBox.Show("Are you sure you want to DELETE Book ID:" + bookID + "?", "Confirmation Message", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (check == DialogResult.Yes)
                {
                    try
                    {
                        con.Open();
                        DateTime today = DateTime.Today;

                        string updateData = "UPDATE books SET date_delete = @dateDelete WHERE id = @id";

                        using (MySqlCommand cmd = new MySqlCommand(updateData, con))
                        {
                            cmd.Parameters.AddWithValue("@dateDelete", today);
                            cmd.Parameters.AddWithValue("@id", bookID);

                            await cmd.ExecuteNonQueryAsync();  // Execute the delete asynchronously

                            await displayBooksAsync();  // Refresh the data

                            MessageBox.Show("Deleted successfully!", "Information Message", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            clearFields();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error: " + ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    finally
                    {
                        con.Close();
                    }
                }
                else
                {
                    MessageBox.Show("Cancelled.", "Information Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }
    }
}

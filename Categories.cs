using System;
using System.Text;
using System.Data;
using System.Collections.Generic;   //Import namesapce to use function
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using Terminal.Gui;
using Org.BouncyCastle.Asn1.X509;
using Mysqlx;

public class Categories
{
    public int CategoryID { get; set; }
    public string CategoryName { get; set; } 
    public string CategoryDescription { get; set; }

    public static List<Categories> ListCategories = new List<Categories>();
    
    public static Categories cate = new Categories();
    public static SuperAdmin superadmin = new SuperAdmin();
    public static Customers cus = new Customers();
    public static Cart userCart = new Cart();
    public static Orders order = new Orders();

    public static string connectionString = Configuration.ConnectionString;

    // Method to load categories from the database
    static List<Categories> LoadCategory(string connectionString)
    {
        List<Categories> ListCategory = new List<Categories>();

        // Establish connection to the database
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {   
            string query = "SELECT * FROM categories"; 
            MySqlCommand command = new MySqlCommand(query, connection);
            connection.Open();
            MySqlDataReader read = command.ExecuteReader();
            while (read.Read())
            {
                Categories c = new Categories();
                // Load category properties from the database
                c.CategoryID = read.GetInt32("category_id");
                c.CategoryName = read.GetString("category_name");
                c.CategoryDescription = read.GetString("category_description");

                ListCategory.Add(c);
            }
        }
        return ListCategory;
    }
    
    public void AddCategory()
    {
        Categories cg = new Categories();
        var top = Application.Top;
        var addCategorytWin = new Window("Add Category")
        {
            X = 0,
            Y = 0,
            Width = Dim.Fill(),
            Height = Dim.Fill()
        };
        top.Add(addCategorytWin);
        addCategorytWin.FocusNext();

        // Labels and text fields for inputting category details
        var CategoryNameLabel = new Label("Category Name:")
        {
            X = 2,
            Y = 2
        };
        var CategoryNameField = new TextField("")
        {
            X = 18,
            Y = 2,
            Width = 100
        };

        var CategoryDescriptionLabel = new Label("Description:")
        {
            X = 2,
            Y = 4
        };
        var CategoryDescriptionField = new TextField("")
        {
            X = 18,
            Y = 4,
            Width = 100
        };

        // Button to save the new category
        var saveButton = new Button("Save")
        {
            X = Pos.Center(),
            Y = 14
        };
        saveButton.Clicked += () =>
        {
            // Check if any fields are empty
            if (string.IsNullOrWhiteSpace(CategoryNameField.Text.ToString()) || 
                string.IsNullOrWhiteSpace(CategoryDescriptionField.Text.ToString()))
            {
                MessageBox.ErrorQuery("Error", "All fields must be filled.", "OK");
                return;
            }

            try
            {
                // Assign input values to the category object
                cg.CategoryName = CategoryNameField.Text.ToString();
                cg.CategoryDescription = CategoryDescriptionField.Text.ToString();

                // Insert the new category into the database
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "INSERT INTO categories (category_name, category_description) VALUES (@CategoryName, @CategoryDescription)";
                    MySqlCommand command = new MySqlCommand(query, connection);
                    command.Parameters.AddWithValue("@CategoryName", cg.CategoryName);
                    command.Parameters.AddWithValue("@CategoryDescription", cg.CategoryDescription);

                    command.ExecuteNonQuery();
                }

                // Add the new category to the static list
                ListCategories.Add(cg);

                // Show success message and return to superadmin menu
                MessageBox.Query("Success", "Successfully added new category!", "OK");
                top.Remove(addCategorytWin);
                superadmin.SuperAdminMenu();
            }
            catch 
            {
                // Display a generic error message if an exception occurs
                MessageBox.ErrorQuery("Error", "An error occurred while adding the category. Please try again later.", "OK");
            }
        };

        // Button to close the window
        var closeButton = new Button("Close")
        {
            X = Pos.Center(),
            Y = Pos.Bottom(saveButton) + 1
        };
        closeButton.Clicked += () =>
        {
            // Prompt confirmation before closing the window
            bool confirmed = MessageBox.Query("Confirm", "Are you sure you want to close?", "Yes", "No") == 0;
            if (confirmed)
            {
                top.Remove(addCategorytWin);
                superadmin.SuperAdminMenu(); // Return to superadmin menu
            }
        };

        // Add labels, text fields, and buttons to the window
        addCategorytWin.Add(CategoryNameLabel, CategoryNameField, CategoryDescriptionLabel, CategoryDescriptionField,
                            saveButton, closeButton);
}
    public void DeleteCategory(int CategoryID)
    {
        try
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = @"UPDATE categories
                                SET active = FALSE 
                                WHERE category_id = @CategoryID;
                                UPDATE products
                                SET is_active = FALSE
                                WHERE product_categorry_id = @CategoryID";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@CategoryID", CategoryID);
                command.ExecuteNonQuery();
            }
            MessageBox.Query("Success", "Cateogry successfully marked as inactive!", "OK");
        }
        catch 
        {
            MessageBox.ErrorQuery("Error", "An error occurred while deleting the category. Please try again later.", "OK");
        }
    }
    public void DisplayCategories(string role)
    {
        // Load the list of categories from the database using the connection string.
        List<Categories> categoriesList = LoadCategory(connectionString);
        
        // Get the top-level window of the application.
        var top = Application.Top;

        // Create a new window titled "Display Categories" with specific dimensions.
        var displayCategoryWindow = new Window("Display Categories")
        {
            X = 0,
            Y = 0,
            Width = Dim.Fill(),
            Height = Dim.Fill()
        };

        // Add the display category window to the top-level window.
        top.Add(displayCategoryWindow);

        // Move focus to the next UI element in the window.
        displayCategoryWindow.FocusNext();

        // Establish a connection to the database.
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            // Define the SQL query to retrieve category data.
            string query = @"SELECT 
                                cate.category_id, 
                                cate.category_name, 
                                cate.category_description
                            FROM categories cate 
                            WHERE active = TRUE";

            // Create a command object to execute the query.
            MySqlCommand command = new MySqlCommand(query, connection);
            
            // Open the connection to the database.
            connection.Open();
            
            // Execute the query and obtain a data reader to read the results.
            MySqlDataReader reader = command.ExecuteReader();

            // Initialize the row offset for displaying data rows.
            int rowOffset = 1;

            // Read the data row by row from the data reader.
            while (reader.Read())
            {
                // Retrieve category details from the current row.
                int categoryId = Convert.ToInt32(reader["category_id"]);
                string categoryName = reader["category_name"].ToString();
                string categoryDescription = reader["category_description"].ToString();

                // Create a button for each category.
                var categoryButton = new Button(categoryName)
                {
                    X = 1,
                    Y = rowOffset
                };
                categoryButton.Clicked += () => 
                {
                    // Define the action to be taken when the category button is clicked.
                    top.Remove(displayCategoryWindow);
                    DisplayProducts(categoryId, categoryName);
                };
                var deleteCategoryButton = new Button(categoryName)
                {
                    X = 1,
                    Y = rowOffset
                };
                deleteCategoryButton.Clicked += () =>
                { 
                    bool confirmed = MessageBox.Query("Comfirm", "Are you sure want to disable this category", "Yes", "No") == 0;
                    if (confirmed)
                    {
                        try
                        {
                        cate.DeleteCategory(categoryId);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.ErrorQuery("Error", ex.Message, "OK");
                        }
                    }
                };

                if(role == "user")
                {    
                    categoryButton.Visible = true;
                    deleteCategoryButton.Visible = false;
                }
                else if(role == "superadmin")
                {
                    categoryButton.Visible = false;
                    deleteCategoryButton.Visible = true;
                }
                // Add the category button to the window.
                displayCategoryWindow.Add(categoryButton, deleteCategoryButton);

                // Increment the row offset for the next data row.
                rowOffset += 2;
            }

            // Close the data reader.
            reader.Close();
        }

        // Create a button labeled "Close" to close the display category window.
        var btnClose = new Button("Close")
        {
            X = Pos.Center(),
            Y = Pos.Percent(100) - 1
        };

        // Define the action to be taken when the close button is clicked.
        btnClose.Clicked += () =>
        {
            bool confirmed = MessageBox.Query("Confirm", "Are you sure you want to close?", "Yes", "No") == 0;
            if (confirmed)
            {
                // Remove the display category window and go back to the menu.
                top.Remove(displayCategoryWindow);
                switch(role)
                {
                    case "user":
                    cus.UserMenu();
                    break;
                    case "superadmin":
                    superadmin.SuperAdminMenu();
                    break;
                }
            }
        };

        // Add the close button to the display category window.
        displayCategoryWindow.Add(btnClose);
    }

    public void DisplayProducts(int categoryId, string categoryName)
    {
        // Get the top-level window of the application.
        var top = Application.Top;

        // Create a new window titled "Products in {categoryName}" with specific dimensions.
        var productsWindow = new Window($"Products in {categoryName}")
        {
            X = 0,
            Y = 0,
            Width = Dim.Fill(),
            Height = Dim.Fill()
        };

        // Create a ScrollView to hold the products.
        var scrollView = new ScrollView()
        {
            X = 0,
            Y = 0,
            Width = Dim.Fill(),
            Height = Dim.Fill(),
            ContentSize = new Size(0, 0) // Kích thước nội dung sẽ được thiết lập sau
        };

        productsWindow.Add(scrollView);
        top.Add(productsWindow); // Add the products window to the top-level window

        var productContainer = new View()
        {
            X = 0,
            Y = 0,
            Width = Dim.Fill(),
            Height = Dim.Fill()
        };

        scrollView.Add(productContainer);

        int row = 0;
        int col = 0;
        int colCount = 2; // Số cột trong ScrollView
        int colWidth = 40; // Chiều rộng của mỗi cửa sổ sản phẩm
        int rowHeight = 12; // Chiều cao của mỗi cửa sổ sản phẩm
        int margin = 2; // Lề giữa các sản phẩm

        // Connect to the database and retrieve products for the selected category.
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            string query = @"SELECT 
                                p.product_id,
                                p.product_name,
                                p.product_brand,
                                p.product_price
                            FROM products p
                            WHERE p.product_category_id = @CategoryId";
            MySqlCommand command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@CategoryId", categoryId); // Bind category ID parameter
            connection.Open();
            MySqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                // Construct the product details string
                int productID = reader.GetInt32("product_id");
                string productName = reader["product_name"].ToString();
                string productBrand = reader["product_brand"].ToString();
                decimal productPrice = reader.GetDecimal("product_price");

                var productWindow = new Window($"Product {row * colCount + col + 1}")
                {
                    X = col * (colWidth + margin),
                    Y = row * (rowHeight + margin),
                    Width = colWidth,
                    Height = rowHeight
                };

                // Create labels for displaying product details
                var productNameLabel = new Label($"Name: {productName}")
                {
                    X = 1,
                    Y = 1,
                    Width = Dim.Fill(),
                    TextAlignment = TextAlignment.Left // Left text alignment
                };
                var productBrandLabel = new Label($"Description: {productBrand}")
                {
                    X = 1,
                    Y = Pos.Bottom(productNameLabel) + 1,
                    Width = Dim.Fill(),
                    TextAlignment = TextAlignment.Left // Left text alignment
                };
                var productPriceLabel = new Label($"Price: {productPrice:C}")
                {
                    X = 1,
                    Y = Pos.Bottom(productBrandLabel) + 1,
                    Width = Dim.Fill(),
                    TextAlignment = TextAlignment.Left // Left text alignment
                };
                var addButton = new Button("Add to Cart")
                {
                    X = 1,
                    Y = 7
                };
                addButton.Clicked += () =>
                {
                    try
                    {
                        userCart.AddToCart(productID);
                        MessageBox.Query("Success", "Product successfully added to cart!", "OK");
                    }
                    catch
                    {
                        MessageBox.ErrorQuery("Error", "An error occurred while adding the product to the cart. Please try again later.", "OK");
                    }
                };

                var orderButton = new Button("Order")
                {
                    X = Pos.Right(addButton) + 2,
                    Y = 7
                };

                orderButton.Clicked += () =>
                {
                    try
                    {
                        top.Remove(productsWindow);
                        order.OrderProduct(productID, productName, productPrice, "category");
                        MessageBox.Query("Success", "Order placed successfully!", "OK");
                    }
                    catch
                    {
                        MessageBox.ErrorQuery("Error", "An error occurred while placing the order. Please try again later.", "OK");
                    }
                };
                // Add labels to the product window
                productWindow.Add(productNameLabel, productBrandLabel, productPriceLabel, addButton);
                productContainer.Add(productWindow);

                col++;
                if (col >= colCount)
                {
                    col = 0;
                    row++;
                }
            }

            // Cập nhật kích thước nội dung của ScrollView
            scrollView.ContentSize = new Size((colWidth + margin) * colCount, (row + 1) * (rowHeight + margin));

            // Close the data reader.
            reader.Close();
        }

        // Create a button labeled "Close" to close the products window.
        var btnClose = new Button("Close")
        {
            X = Pos.Center(),
            Y = Pos.Bottom(scrollView) - 1
        };

        // Define the action to be taken when the close button is clicked.
        btnClose.Clicked += () =>
        {
            // Remove the products window and go back to the categories window.
            top.Remove(productsWindow);
            cate.DisplayCategories("user");
        };

        // Add the close button to the products window.
        productsWindow.Add(btnClose);
    }
}

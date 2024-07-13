using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;                      //Import namesapce to use function
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using Terminal.Gui;
using Mysqlx.Crud;


public class Products
{
    // Properties representing details of a product
    public int ProductID { get; set; }
    public string ProductName { get; set; }
    public int ProductStockQuantity { get; set; }
    public decimal ProductPrice { get; set; }
    public int ProductCategoryID { get; set; }
    public string ProductBrand { get; set; }

    // Static field holding database connection string
    public static string connectionString = Configuration.ConnectionString;

    // Static list to store instances of Products
    public static List<Products> ListProducts = new List<Products>();

    // Static instances of various application components
    public static Products pd = new Products();
    public static Program program = new Program();
    public static Users user = new Users();
    public static Admin admin = new Admin();
    public static SuperAdmin superadmin = new SuperAdmin();
    public static Cart userCart = new Cart();
    public static Customers customer = new Customers();
    public static Orders order = new Orders();

    static List<Products> LoadProducts(string connectionString)
    {
        using(MySqlConnection connectionproduct = new MySqlConnection(connectionString))
        {
            string queryproduct = @"SELECT product_id FROM products";
            MySqlCommand commandproduct = new MySqlCommand(queryproduct, connectionproduct);
            connectionproduct.Open();
            MySqlDataReader read = commandproduct.ExecuteReader();
            while (read.Read())
            {
                pd.ProductID = read.GetInt32("product_id");
                pd.ProductName = read.GetString("product_name");
                pd.ProductPrice = read.GetDecimal("product_price");
                pd.ProductCategoryID = read.GetInt32("product_category_id");
                pd.ProductBrand = read.GetString("product_band");
                ListProducts.Add(pd);
            }
        }
        return ListProducts;
    }

    public void DisplayProduct(string role)
    {
        // List to hold string arrays of product details
        List<string[]> products = new List<string[]>();
        
        // Get the top-level application instance
        var top = Application.Top;

        // Create a window for displaying product list
        var displayProductWindow = new Window("Product List")
        {
            X = 0,
            Y = 0,
            Width = Dim.Fill(),
            Height = Dim.Fill()
        };
        top.Add(displayProductWindow);
        displayProductWindow.FocusNext();

        // Connect to the database and retrieve product information
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            string query = @"SELECT 
                                p.product_id,
                                p.product_name, 
                                p.product_stock_quantity, 
                                p.product_price, 
                                c.category_name, 
                                p.product_brand
                            FROM products p
                            INNER JOIN categories c ON p.product_category_id = c.category_id;";
            MySqlCommand command = new MySqlCommand(query, connection);
            connection.Open();
            MySqlDataReader reader = command.ExecuteReader();
            
            // Array for column headers and their display settings
            var columnDisplayListProduct = new string[]
            {
                "Product's name", "Stock quantity", "Price", "Category's name", "Brand",  "Action"
            };

            int columnWidth = 20; // Set column width

            // Display column headers based on role
            for (int i = 0; i < columnDisplayListProduct.Length; i++)
            {
                displayProductWindow.Add(new Label(columnDisplayListProduct[i])
                {
                    X = i * columnWidth,
                    Y = 0,
                    Width = columnWidth,
                    Height = 1,
                    Visible = !(role == "admin" && ( columnDisplayListProduct[i] == "Action"))
                });
            }

            int row = 1;
            // Iterate through each product and display its details
            while (reader.Read())
            {
                int productID = int.Parse(reader["product_id"].ToString());
                string productName = reader["product_name"].ToString();
                decimal productprice = decimal.Parse(reader["product_price"].ToString());
                int productQuantity = reader.GetInt32("product_stock_quantity");
                // Labels for each product detail
                var productLabel = new Label($"{reader["product_name"]}")
                {
                    X = 0,
                    Y = row,
                    Width = columnWidth,
                    Height = 1,
                    TextAlignment = TextAlignment.Left // Left text if it exceeds column width
                };
                
                var stockQuantityLabel = new Label(productQuantity > 0 ? productQuantity.ToString() : "Sold out")
                {
                    X = 1 * columnWidth,
                    Y = row,
                    Width = columnWidth,
                    Height = 1,
                    TextAlignment = TextAlignment.Left // Left text if it exceeds column width
                };
                var priceLabel = new Label($"{reader["product_price"]}")
                {
                    X = 2 * columnWidth,
                    Y = row,
                    Width = columnWidth,
                    Height = 1,
                    TextAlignment = TextAlignment.Left // Left text if it exceeds column width
                };
                var categoryLabel = new Label($"{reader["category_name"]}")
                {
                    X = 3 * columnWidth,
                    Y = row,
                    Width = columnWidth,
                    Height = 1,
                    TextAlignment = TextAlignment.Left // Left text if it exceeds column width
                };
                var brandLabel = new Label($"{reader["product_brand"]}")
                {
                    X = 4 * columnWidth,
                    Y = row,
                    Width = columnWidth,
                    Height = 1,
                    TextAlignment = TextAlignment.Left // Left text if it exceeds column width
                };

                var addButton = new Button("Add to Cart")
                {
                    X = 5 * columnWidth,
                    Y = row,
                    Width = columnWidth,
                    Height = 1
                };
                addButton.Clicked += () =>
                {
                    try
                    {
                        userCart.AddToCart(productID);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.ErrorQuery("Error", ex.Message, "OK");
                    }
                };
                var orderbutton = new Button("Order")
                {
                    X = 6 * columnWidth,
                    Y = row,
                    Width = columnWidth,
                    Height = 1,
                };
                orderbutton.Clicked += () =>
                {
                    try
                    {
                        top.Remove(displayProductWindow);
                        order.OrderProduct(productID, productName, ProductPrice, "display");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.ErrorQuery("Error", ex.Message, "OK");
                    }
                };

                // Add labels, text field, and button to the display window
                displayProductWindow.Add(productLabel, stockQuantityLabel, priceLabel, categoryLabel, brandLabel, addButton, orderbutton);
                row++;

                // Set visibility based on user role
                if (role == "user")
                {
                    addButton.Visible = true;
                    orderbutton.Visible = true;
                }
                else if (role == "admin")
                {
                    addButton.Visible = false;
                    orderbutton.Visible = false;
                }
            }

            // 'Back' button to return to respective menus
            var btnBack = new Button("Back")
            {
                X = Pos.Center(),
                Y = Pos.Percent(100) - 1
            };
            btnBack.Clicked += () =>
            {
                top.Remove(displayProductWindow);
                switch (role)
                {
                    case "user":
                        customer.UserMenu();
                        break;
                    case "admin":
                        admin.AdminMenu();
                        break;
                }
            };

            displayProductWindow.Add(btnBack);
        }
    }
    public void AddProduct()
    {
        Products pd = new Products(); // Create a new instance of Products
        var top = Application.Top;
        var addProductWin = new Window("Add Product")
        {
            X = 0,
            Y = 0,
            Width = Dim.Fill(),
            Height = Dim.Fill()
        };
        top.Add(addProductWin);
        addProductWin.FocusNext();

        // Labels and text fields for inputting product details
        var productNameLabel = new Label("Product Name:")
        {
            X = 2,
            Y = 2
        };
        var productNameField = new TextField("")
        {
            X = 18,
            Y = 2,
            Width = 100
        };

        var productStockQuantityLabel = new Label("Stock Quantity:")
        {
            X = 2,
            Y = 4
        };
        var productStockQuantityField = new TextField("")
        {
            X = 18,
            Y = 4,
            Width = 100
        };

        var productCategoryIDLabel = new Label("Category ID:")
        {
            X = 2,
            Y = 6
        };
        var productCategoryIDField = new TextField("")
        {
            X = 18,
            Y = 6,
            Width = 100
        };

        var productPriceLabel = new Label("Price:")
        {
            X = 2,
            Y = 8
        };
        var productPriceField = new TextField("")
        {
            X = 18,
            Y = 8,
            Width = 100
        };

        var productBrandLabel = new Label("Brand:")
        {
            X = 2,
            Y = 12
        };
        var productBrandField = new TextField("")
        {
            X = 18,
            Y = 12,
            Width = 100
        };

        // Button to save the new product
        var saveButton = new Button("Save")
        {
            X = Pos.Center(),
            Y = 14
        };
        saveButton.Clicked += () =>
        {
            try
            {
                // Assign input values to the product object
                pd.ProductName = productNameField.Text.ToString();
                pd.ProductStockQuantity = int.Parse(productStockQuantityField.Text.ToString());
                pd.ProductCategoryID = int.Parse(productCategoryIDField.Text.ToString());
                pd.ProductPrice = decimal.Parse(productPriceField.Text.ToString());
                pd.ProductBrand = productBrandField.Text.ToString();

                // Insert the new product into the database
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "INSERT INTO products (product_name, product_stock_quantity, product_category_id, product_price, product_brand) VALUES (@ProductName, @ProductStockQuantity, @ProductCategoryID, @ProductPrice, @ProductBrand)";
                    MySqlCommand command = new MySqlCommand(query, connection);
                    command.Parameters.AddWithValue("@ProductName", pd.ProductName);
                    command.Parameters.AddWithValue("@ProductStockQuantity", pd.ProductStockQuantity);
                    command.Parameters.AddWithValue("@ProductCategoryID", pd.ProductCategoryID);
                    command.Parameters.AddWithValue("@ProductPrice", pd.ProductPrice);
                    command.Parameters.AddWithValue("@ProductBrand", pd.ProductBrand);
                    command.ExecuteNonQuery();
                }

                // Add the new product to the static list
                ListProducts.Add(pd);

                // Show success message and return to admin menu
                MessageBox.Query("Success", "Successfully added new product!", "OK");
                top.Remove(addProductWin);
                admin.AdminMenu();
            }
            catch (Exception ex)
            {
                // Display error message if an exception occurs
                MessageBox.ErrorQuery("Error", ex.Message, "OK");
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
                top.Remove(addProductWin);
                admin.AdminMenu(); // Return to admin menu
            }
        };

        // Add labels, text fields, and buttons to the window
        addProductWin.Add(productNameLabel, productNameField, productStockQuantityLabel, productStockQuantityField,
                        productCategoryIDLabel, productCategoryIDField, productPriceLabel, productPriceField,
                        productBrandLabel, productBrandField,
                        saveButton, closeButton);
    }
    public void EditProductInformations()
    {
        var top = Application.Top;
        var editProductWin = new Window("Edit Product Information")
        {
            X = 0,
            Y = 0,
            Width = Dim.Fill(),
            Height = Dim.Fill()
        };
        top.Add(editProductWin);

        // Label and field for finding a product by name
        var findProductLabel = new Label("Find Product Name:")
        {
            X = 2,
            Y = 2
        };

        var findProductField = new TextField("")
        {
            X = 18,
            Y = Pos.Top(findProductLabel),
            Width = 100
        };

        // Button to confirm the search
        var confirmButton = new Button("Confirm")
        {
            X = Pos.Right(findProductField) + 2,
            Y = Pos.Top(findProductLabel)
        };

        // List view to display search results
        var productListView = new ListView(new List<string>())
        {
            X = 2,
            Y = Pos.Bottom(findProductLabel) + 2,
            Width = 100,
            Height = 5,
            Visible = false
        };

        // Labels and fields for editing product information
        var editProductNameLabel = new Label("Product Name:")
        {
            X = 2,
            Y = Pos.Bottom(productListView) + 1
        };

        var editProductNameField = new TextField("")
        {
            X = 18,
            Y = Pos.Top(editProductNameLabel),
            Width = 100
        };

        var editProductStockQuantityLabel = new Label("Stock Quantity:")
        {
            X = 2,
            Y = Pos.Bottom(editProductNameField) + 1
        };

        var editProductStockQuantityField = new TextField("")
        {
            X = 18,
            Y = Pos.Top(editProductStockQuantityLabel),
            Width = 100
        };

        var editProductCategoryIDLabel = new Label("Category ID:")
        {
            X = 2,
            Y = Pos.Bottom(editProductStockQuantityField) + 1
        };

        var editProductCategoryIDField = new TextField("")
        {
            X = 18,
            Y = Pos.Top(editProductCategoryIDLabel),
            Width = 100
        };

        var editProductPriceLabel = new Label("Price:")
        {
            X = 2,
            Y = Pos.Bottom(editProductCategoryIDField) + 1
        };

        var editProductPriceField = new TextField("")
        {
            X = 18,
            Y = Pos.Top(editProductPriceLabel),
            Width = 100
        };

        var editProductBrandLabel = new Label("Brand:")
        {
            X = 2,
            Y = Pos.Bottom(editProductPriceField) + 1
        };

        var editProductBrandField = new TextField("")
        {
            X = 18,
            Y = Pos.Top(editProductBrandLabel),
            Width = 100
        };

        // Button to save edited product information
        var saveButton = new Button("Save")
        {
            X = Pos.Center(),
            Y = Pos.Bottom(editProductBrandField) + 2
        };
        saveButton.Clicked += () =>
        {
            try
            {
                // Update the product object with edited values
                pd.ProductName = editProductNameField.Text.ToString();
                pd.ProductStockQuantity = int.Parse(editProductStockQuantityField.Text.ToString());
                pd.ProductCategoryID = int.Parse(editProductCategoryIDField.Text.ToString());
                pd.ProductPrice = decimal.Parse(editProductPriceField.Text.ToString());
                pd.ProductBrand = editProductBrandField.Text.ToString();

                // Update the product in the database
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "UPDATE products SET product_name = @ProductName, product_stock_quantity = @ProductStockQuantity, product_price = @ProductPrice, product_category_id = @ProductCategoryID, product_brand = @ProductBrand WHERE product_id = @ProductID";
                    MySqlCommand command = new MySqlCommand(query, connection);
                    command.Parameters.AddWithValue("@ProductID", pd.ProductID);
                    command.Parameters.AddWithValue("@ProductName", pd.ProductName);
                    command.Parameters.AddWithValue("@ProductStockQuantity", pd.ProductStockQuantity);
                    command.Parameters.AddWithValue("@ProductPrice", pd.ProductPrice);
                    command.Parameters.AddWithValue("@ProductCategoryID", pd.ProductCategoryID);
                    command.Parameters.AddWithValue("@ProductBrand", pd.ProductBrand);
                    command.ExecuteNonQuery();
                }

                // Display success message and return to admin menu
                MessageBox.Query("Success", "Successfully edited product!", "OK");
                top.Remove(editProductWin);
                pd.EditProductInformations();
            }
            catch (Exception ex)
            {
                // Display error message if an exception occurs
                MessageBox.ErrorQuery("Error", ex.Message, "OK");
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
                top.Remove(editProductWin);
                admin.AdminMenu(); // Return to admin menu
            }
        };

        // Action for confirming the product search
        confirmButton.Clicked += () =>
        {
            try
            {
                string productName = findProductField.Text.ToString();
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    string query = @"SELECT 
                                        product_id,
                                        product_name, 
                                        product_stock_quantity, 
                                        product_price, 
                                        product_category_id, 
                                        product_brand
                                    FROM products
                                    WHERE product_name LIKE @ProductName";
                    MySqlCommand command = new MySqlCommand(query, connection);
                    command.Parameters.AddWithValue("@ProductName", "%" + productName + "%");
                    connection.Open();
                    MySqlDataReader reader = command.ExecuteReader();

                    var productList = new List<string>();
                    var productMap = new Dictionary<int, Products>();

                    while (reader.Read())
                    {
                        Products product = new Products
                        {
                            ProductID = reader.GetInt32("product_id"),
                            ProductName = reader.GetString("product_name"),
                            ProductStockQuantity = reader.GetInt32("product_stock_quantity"),
                            ProductPrice = reader.GetDecimal("product_price"),
                            ProductCategoryID = reader.GetInt32("product_category_id"),
                            ProductBrand = reader.GetString("product_brand")
                        };

                        productList.Add($"{product.ProductID} - {product.ProductName}");
                        productMap[product.ProductID] = product;
                    }

                    if (productList.Count > 0)
                    {
                        // Display the list of found products and allow selection
                        productListView.SetSource(productList);
                        productListView.Visible = true;
                        productListView.OpenSelectedItem += (args) =>
                        {
                            int selectedProductID = int.Parse(productList[args.Item].Split(' ')[0]);
                            pd = productMap[selectedProductID]; // Assign selected product to pd for editing

                            // Hide search components and show edit components with selected product's details
                            findProductLabel.Visible = false;
                            findProductField.Visible = false;
                            confirmButton.Visible = false;
                            productListView.Visible = false;

                            editProductNameField.Text = pd.ProductName;
                            editProductStockQuantityField.Text = pd.ProductStockQuantity.ToString();
                            editProductCategoryIDField.Text = pd.ProductCategoryID.ToString();
                            editProductPriceField.Text = pd.ProductPrice.ToString();
                            editProductBrandField.Text = pd.ProductBrand;

                            // Make edit fields visible
                            editProductNameLabel.Visible = true;
                            editProductNameField.Visible = true;
                            editProductStockQuantityLabel.Visible = true;
                            editProductStockQuantityField.Visible = true;
                            editProductCategoryIDLabel.Visible = true;
                            editProductCategoryIDField.Visible = true;
                            editProductPriceLabel.Visible = true;
                            editProductPriceField.Visible = true;
                            editProductBrandLabel.Visible = true;
                            editProductBrandField.Visible = true;
                            saveButton.Visible = true; // Show the save button
                        };
                    }
                    else
                    {
                        MessageBox.ErrorQuery("Error", "Product not found!", "OK");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.ErrorQuery("Error", ex.Message, "OK");
            }
        };

        // Initial visibility settings
        productListView.Visible = false;
        editProductNameLabel.Visible = false;
        editProductNameField.Visible = false;
        editProductStockQuantityLabel.Visible = false;
        editProductStockQuantityField.Visible = false;
        editProductCategoryIDLabel.Visible = false;
        editProductCategoryIDField.Visible = false;
        editProductPriceLabel.Visible = false;
        editProductPriceField.Visible = false;
        editProductBrandLabel.Visible = false;
        editProductBrandField.Visible = false;
        saveButton.Visible = false;

        // Add all components to the window
        editProductWin.Add(findProductLabel, findProductField, confirmButton, productListView,
                        editProductNameLabel, editProductNameField,
                        editProductStockQuantityLabel, editProductStockQuantityField,
                        editProductCategoryIDLabel, editProductCategoryIDField,
                        editProductPriceLabel, editProductPriceField,
                        editProductBrandLabel, editProductBrandField,
                        saveButton, closeButton);
    }
    public void DeleteProduct()
    {
        ListProducts =LoadProducts(connectionString);
        var top = Application.Top;
        var deleteProductWin = new Window("Delete Product")
        {
            X = 0,
            Y = 0,
            Width = Dim.Fill(),
            Height = Dim.Fill()
        };
        top.Add(deleteProductWin);

        // Label and field for entering the product ID
        var productIDLabel = new Label("Product ID:")
        {
            X = 2,
            Y = 2
        };
        var productIDField = new TextField("")
        {
            X = Pos.Right(productIDLabel) + 1,
            Y = 2,
            Width = 100
        };

        // Button to delete the product
        var deleteButton = new Button("Delete")
        {
            X = Pos.Center(),
            Y = 4
        };
        deleteButton.Clicked += () =>
        {
            try
            {   
                
                // Validate and parse the product ID from the text field
                    if (int.TryParse(productIDField.Text.ToString(), out int productID))
                    {
                        
                        // Find the product in the list based on the ID
                        Products pd = ListProducts.Find(s => s.ProductID == productID);

                        if (pd != null)
                        {
                            // Delete the product from the database
                            using (MySqlConnection connection = new MySqlConnection(connectionString))
                            {
                                connection.Open();
                                string query = "DELETE FROM products WHERE product_id = @ProductID";
                                MySqlCommand command = new MySqlCommand(query, connection);
                                command.Parameters.AddWithValue("@ProductID", productID);
                                command.ExecuteNonQuery();
                            }

                            // Remove the product from the application list
                            ListProducts.Remove(pd);

                            // Display success message and return to admin menu
                            MessageBox.Query("Success", "Successfully deleted product!", "OK");
                            top.Remove(deleteProductWin);
                            pd.DeleteProduct();
                        }
                        else
                        {
                            // Display error if product not found
                            MessageBox.ErrorQuery("Error", "Product not found!", "OK");
                        }
                    }
                    else
                    {
                        // Display error for invalid product ID format
                        MessageBox.ErrorQuery("Error", "Invalid Product ID!", "OK");
                    }
                
            }
            catch (Exception ex)
            {
                // Display error for any exception during deletion process
                MessageBox.ErrorQuery("Error", ex.Message, "OK");
            }
        };

        // Button to close the window
        var closeButton = new Button("Close")
        {
            X = Pos.Center(),
            Y = Pos.Bottom(deleteButton) + 1
        };
        closeButton.Clicked += () =>
        {
            // Prompt confirmation before closing the window
            bool confirmed = MessageBox.Query("Confirm", "Are you sure you want to close?", "Yes", "No") == 0;
            if (confirmed)
            {
                top.Remove(deleteProductWin);
                admin.AdminMenu(); // Return to admin menu
            }
        };

        // Add all components to the window
        deleteProductWin.Add(productIDLabel, productIDField, deleteButton, closeButton);
    }

    public void FindProduct(string role)
    {
        var top = Application.Top;
        var findProductWin = new Window("Find Product")
        {
            X = 0,
            Y = 0,
            Width = Dim.Fill(),
            Height = Dim.Fill()
        };
        top.Add(findProductWin);

        // Label and field for entering the product name
        var productNameLabel = new Label("Product name:")
        {
            X = 2,
            Y = 2
        };
        var productNameField = new TextField("")
        {
            X = Pos.Right(productNameLabel) + 1,
            Y = 2,
            Width = 100
        };

        // Button to find the product
        var findButton = new Button("Find")
        {
            X = Pos.Center(),
            Y = 4
        };

        // Array of column headers for product information
        var columnDisplayListProduct = new string[]
        {
            "Product's Name", "Stock Quantity", "Price", "Category ID", "Brand", "Image"
        };

        // Add column headers to the window
        for (int i = 0; i < columnDisplayListProduct.Length; i++)
        {
            findProductWin.Add(new Label(columnDisplayListProduct[i])
            {
                X = i * 20,
                Y = 6,
                Width = 20,
                Height = 1
            });
        }

        // Event handler for the Find button
        findButton.Clicked += () =>
        {
            try
            {
                string productName = productNameField.Text.ToString();
                Products product = null;

                // Query to retrieve product information based on product name
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    string query = @"SELECT 
                                        p.product_id,
                                        p.product_name, 
                                        p.product_stock_quantity, 
                                        p.product_price, 
                                        c.category_name, 
                                        p.product_brand
                                    FROM products p
                                    INNER JOIN categories c ON p.product_category_id = c.category_id
                                    WHERE p.product_name Like @ProductName";
                    MySqlCommand command = new MySqlCommand(query, connection);
                    command.Parameters.AddWithValue("@ProductName", "%" + productName + "%");
                    connection.Open();
                    MySqlDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        // Populate product object with retrieved data
                        product = new Products
                        {
                            ProductName = reader["product_name"].ToString(),
                            ProductStockQuantity = int.Parse(reader["product_stock_quantity"].ToString()),
                            ProductPrice = decimal.Parse(reader["product_price"].ToString()),
                            ProductCategoryID = int.Parse(reader["category_name"].ToString()),
                            ProductBrand = reader["product_brand"].ToString(),
                        };
                    }
                    else
                    {
                        // Display error if product not found
                        MessageBox.ErrorQuery("Error", "Product not found!", "OK");
                        return;
                    }
                }

                // Display product information in the window
                findProductWin.Add(new Label(product.ProductName)
                {
                    X = 0,
                    Y = 7,
                    Width = 20,
                    Height = 1
                });
                findProductWin.Add(new Label(product.ProductStockQuantity.ToString())
                {
                    X = 20,
                    Y = 7,
                    Width = 20,
                    Height = 1
                });
                findProductWin.Add(new Label(product.ProductPrice.ToString("C"))
                {
                    X = 60,
                    Y = 7,
                    Width = 20,
                    Height = 1
                });
                findProductWin.Add(new Label(product.ProductCategoryID.ToString())
                {
                    X = 80,
                    Y = 7,
                    Width = 20,
                    Height = 1
                });
                findProductWin.Add(new Label(product.ProductBrand)
                {
                    X = 100,
                    Y = 7,
                    Width = 20,
                    Height = 1
                });
                
            }
            catch (Exception ex)
            {
                // Display error message for any exception
                MessageBox.ErrorQuery("Error", ex.Message, "OK");
            }
        };

        // Button to close the window
        var closeButton = new Button("Close")
        {
            X = Pos.Center(),
            Y = Pos.Percent(100) - 1
        };

        // Event handler for the Close button
        closeButton.Clicked += () =>
        {
            top.Remove(findProductWin);

            // Return to respective menu based on user role
            switch(role)
            {
                case "user":
                    customer.UserMenu();
                    break;
                case "admin":
                    admin.AdminMenu();
                    break;
                case "superadmin":
                    superadmin.SuperAdminMenu();
                    break;
            }
        };

        // Add components to the window
        findProductWin.Add(productNameLabel, productNameField, findButton, closeButton);
    }
}
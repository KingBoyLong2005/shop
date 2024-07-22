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
    public string ProductCategoryName { get; set; }
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
            string queryproduct = @"SELECT * FROM products WHERE is_active = TRUE";
            MySqlCommand commandproduct = new MySqlCommand(queryproduct, connectionproduct);
            connectionproduct.Open();
            MySqlDataReader read = commandproduct.ExecuteReader();
            while (read.Read())
            {
                pd.ProductID = read.GetInt32("product_id");
                pd.ProductName = read.GetString("product_name");
                pd.ProductPrice = read.GetDecimal("product_price");
                pd.ProductCategoryID = read.GetInt32("product_category_id");
                pd.ProductBrand = read.GetString("product_brand");
                ListProducts.Add(pd);
            }
        }
        return ListProducts;
    }

    public void DisplayProduct(string role)
    {
        var top = Application.Top;

        // Tạo cửa sổ chính
        var displayWindow = new Window("Product List")
        {
            X = 0,
            Y = 0,
            Width = Dim.Fill(),
            Height = Dim.Fill()
        };

        var scrollView = new ScrollView()
        {
            X = 0,
            Y = 0,
            Width = Dim.Fill(),
            Height = Dim.Fill(),
            ContentSize = new Size(0, 0) // Kích thước nội dung sẽ được thiết lập sau
        };

        displayWindow.Add(scrollView);
        top.Add(displayWindow);

        // Tạo một View để chứa tất cả các cửa sổ sản phẩm
        var productContainer = new View()
        {
            X = 0,
            Y = 0,
            Width = Dim.Fill(),
            Height = Dim.Fill()
        };

        scrollView.Add(productContainer);

        // Connect to the database and retrieve product information
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            string query = @"SELECT 
                                p.product_id,
                                p.product_name, 
                                p.product_stock_quantity, 
                                p.product_price, 
                                c.category_name,
                                p.product_category_id,
                                p.product_brand
                            FROM products p
                            INNER JOIN categories c ON p.product_category_id = c.category_id
                            WHERE is_active = TRUE;";
            MySqlCommand command = new MySqlCommand(query, connection);
            connection.Open();
            MySqlDataReader reader = command.ExecuteReader();

            int colCount = 6; // Number of columns per row
            int colWidth = 30; // Width of each product window
            int rowHeight = 12; // Height of each product window
            int margin = 2; // Margin between products

            int col = 0;
            int row = 0;

            while (reader.Read())
            {
                int productID = int.Parse(reader["product_id"].ToString());
                string productName = reader["product_name"].ToString();
                decimal productPrice = decimal.Parse(reader["product_price"].ToString());
                int productQuantity = reader.GetInt32("product_stock_quantity");
                int categoryID = int.Parse(reader["product_category_id"].ToString());
                string categoryName = reader["category_name"].ToString();
                string productBrand = reader["product_brand"].ToString();

                var productWindow = new Window($"{productName}")
                {
                    X = col * (colWidth + margin),
                    Y = row * (rowHeight + margin),
                    Width = colWidth,
                    Height = rowHeight
                };

                var nameLabel = new Label($"Name: {productName}")
                {
                    X = 1,
                    Y = 1,
                    Width = Dim.Fill()
                };
                var quantityLabel = new Label($"Stock: {productQuantity}")
                {
                    X = 1,
                    Y = 2,
                    Width = Dim.Fill()
                };
                var priceLabel = new Label($"Price: {productPrice} $")
                {
                    X = 1,
                    Y = 3,
                    Width = Dim.Fill()
                };
                var categoryLabel = new Label($"Category: {categoryName}")
                {
                    X = 1,
                    Y = 4,
                    Width = Dim.Fill()
                };
                var brandLabel = new Label($"Brand: {productBrand}")
                {
                    X = 1,
                    Y = 5,
                    Width = Dim.Fill()
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
                    }
                    catch
                    {
                        MessageBox.ErrorQuery("Error", "An error occurred while adding the product to the cart.", "OK");
                    }
                };

                var editproductButton = new Button("Edit")
                {
                    X = 1,
                    Y = 7
                };
                editproductButton.Clicked += () =>
                {
                    try
                    {
                        top.Remove(displayWindow);
                        pd.EditProductInformations(role, productID, productName, productQuantity,productPrice, categoryID, productBrand);
                    }
                    catch
                    {
                        MessageBox.ErrorQuery("Error", "An error occurred while editing the product information.", "OK");
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
                        top.Remove(displayWindow);
                        order.OrderProduct(productID, productName, productPrice, "display");
                    }
                    catch
                    {
                        MessageBox.ErrorQuery("Error", "An error occurred while processing the order.", "OK");
                    }
                };

                var blockproductButton = new Button("Disable")
                {
                    X = Pos.Right(editproductButton) + 2,
                    Y = 7
                };
                blockproductButton.Clicked += () =>
                {
                    try
                    {
                        bool confirmed = MessageBox.Query("Confirm", "Are you sure you want to disable product?", "Yes", "No") == 0;
                        if (confirmed)
                        {
                        pd.DeleteProduct(productID);
                        }
                    }
                    catch 
                    {
                        MessageBox.ErrorQuery("Error", "An error occurred while disabling the product.","OK");
                    }
                };
                var OrderForCustomerbutton = new Button("Order For Customer")
                {
                    X = 1,
                    Y = Pos.Bottom(orderButton) + 1,
                };
                    OrderForCustomerbutton.Clicked += () =>
                    {
                        try
                        {
                            top.Remove(displayWindow);
                            order.OrderProductForCustomer(productID, productName, productPrice, role);
                        }
                        catch 
                        {
                            MessageBox.ErrorQuery("Error", "An error occurred while processing the order for the customer.", "OK");
                        }
                    };

                productWindow.Add(nameLabel, quantityLabel, priceLabel, categoryLabel, brandLabel, addButton, orderButton, editproductButton, OrderForCustomerbutton);
                productContainer.Add(productWindow);

                col++;
                if (col >= colCount)
                {
                    col = 0;
                    row++;
                }
                if (role == "user")
                {
                    editproductButton.Visible = false;
                    blockproductButton.Visible = false;
                    OrderForCustomerbutton.Visible = false;
                    addButton.Visible = true;
                    orderButton.Visible = true;
                }
                else if (role == "admin")
                {
                    editproductButton.Visible = true;
                    blockproductButton.Visible = true;
                    OrderForCustomerbutton.Visible = true;
                    addButton.Visible = false;
                    orderButton.Visible = false;
                }
                else if( role == "superadmin")
                {
                    editproductButton.Visible = true;
                    blockproductButton.Visible = true;
                    OrderForCustomerbutton.Visible = true;
                    addButton.Visible = false;
                    orderButton.Visible = false;
                }
            }

            reader.Close();

            // Cập nhật kích thước nội dung của ScrollView
            scrollView.ContentSize = new Size((colWidth + margin) * colCount, (row + 1) * (rowHeight + margin));
        }

        // Đặt nút Back ở dưới cùng
        var backButton = new Button("Back")
        {
            X = Pos.Center(),
            Y = Pos.Top(displayWindow) 
        };
        backButton.Clicked += () =>
        {
            top.Remove(displayWindow);
            switch (role)
            {
                case "user":
                    // Gọi menu user
                    customer.UserMenu();
                    break;
                case "admin":
                    // Gọi menu admin
                    admin.AdminMenu();
                    break;
                case "superadmin":
                    superadmin.SuperAdminMenu();
                    break;
            }
        };

        displayWindow.Add(backButton);
    }
    public void AddProduct()
    {
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
            Y = 10
        };
        var productBrandField = new TextField("")
        {
            X = 18,
            Y = 10,
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
                // Validate that all fields are filled
                if (string.IsNullOrWhiteSpace(productNameField.Text.ToString()) ||
                    string.IsNullOrWhiteSpace(productStockQuantityField.Text.ToString()) ||
                    string.IsNullOrWhiteSpace(productCategoryIDField.Text.ToString()) ||
                    string.IsNullOrWhiteSpace(productPriceField.Text.ToString()) ||
                    string.IsNullOrWhiteSpace(productBrandField.Text.ToString()))
                {
                    MessageBox.ErrorQuery("Error", "All fields must be filled.", "OK");
                    return;
                }

                // Validate numeric fields
                if (!int.TryParse(productStockQuantityField.Text.ToString(), out int stockQuantity))
                {
                    MessageBox.ErrorQuery("Error", "Stock Quantity must be a valid number.", "OK");
                    return;
                }

                if (!int.TryParse(productCategoryIDField.Text.ToString(), out int categoryID))
                {
                    MessageBox.ErrorQuery("Error", "Category ID must be a valid number.", "OK");
                    return;
                }

                if (!decimal.TryParse(productPriceField.Text.ToString(), out decimal price))
                {
                    MessageBox.ErrorQuery("Error", "Price must be a valid decimal number.", "OK");
                    return;
                }

                // Assign input values to the product object
                pd.ProductName = productNameField.Text.ToString();
                pd.ProductStockQuantity = stockQuantity;
                pd.ProductCategoryID = categoryID;
                pd.ProductPrice = price;
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
            catch
            {
                // Display error message if an exception occurs
                MessageBox.ErrorQuery("Error", "An error occurred while adding the new product. Please try again.", "OK");
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
    public void EditProductInformations(string role, int productID, string productName, int quantity, decimal price, int categoryID, string brand)
    {
        var top = Application.Top;

        // Tạo cửa sổ để chỉnh sửa thông tin sản phẩm
        var editProductWin = new Window("Edit Product Information")
        {
            X = 0,
            Y = 0,
            Width = Dim.Fill(),
            Height = Dim.Fill()
        };
        top.Add(editProductWin);

        // Labels and fields for editing product information
        var editProductNameLabel = new Label("Product Name:")
        {
            X = 2,
            Y = 2
        };

        var editProductNameField = new TextField(productName) // Khởi tạo với giá trị hiện tại
        {
            X = 20,
            Y = 2,
            Width = 40
        };

        var editProductStockQuantityLabel = new Label("Stock Quantity:")
        {
            X = 2,
            Y = 4
        };

        var editProductStockQuantityField = new TextField(quantity.ToString()) // Khởi tạo với giá trị hiện tại
        {
            X = 20,
            Y = 4,
            Width = 40
        };

        var editProductCategoryIDLabel = new Label("Category ID:")
        {
            X = 2,
            Y = 6
        };

        var editProductCategoryIDField = new TextField(categoryID.ToString()) // Khởi tạo với giá trị hiện tại
        {
            X = 20,
            Y = 6,
            Width = 40
        };

        var editProductPriceLabel = new Label("Price:")
        {
            X = 2,
            Y = 8
        };

        var editProductPriceField = new TextField(price.ToString("F2")) // Khởi tạo với giá trị hiện tại
        {
            X = 20,
            Y = 8,
            Width = 40
        };

        var editProductBrandLabel = new Label("Brand:")
        {
            X = 2,
            Y = 10
        };

        var editProductBrandField = new TextField(brand) // Khởi tạo với giá trị hiện tại
        {
            X = 20,
            Y = 10,
            Width = 40
        };

        // Button to save edited product information
        var saveButton = new Button("Save")
        {
            X = 20,
            Y = 12
        };
        saveButton.Clicked += () =>
        {
            try
            {
                // Lấy thông tin nhập từ người dùng
                var productNameText = editProductNameField.Text.ToString();
                var stockQuantityText = editProductStockQuantityField.Text.ToString();
                var categoryIDText = editProductCategoryIDField.Text.ToString();
                var priceText = editProductPriceField.Text.ToString();
                var brandText = editProductBrandField.Text.ToString();

                // Kiểm tra và cập nhật thông tin sản phẩm
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    string query = @"UPDATE products 
                                    SET product_name = @ProductName, 
                                        product_stock_quantity = @ProductStockQuantity, 
                                        product_price = @ProductPrice, 
                                        product_category_id = @ProductCategoryID, 
                                        product_brand = @ProductBrand 
                                    WHERE product_id = @ProductID";
                    MySqlCommand command = new MySqlCommand(query, connection);
                    command.Parameters.AddWithValue("@ProductID", productID);
                    command.Parameters.AddWithValue("@ProductName", productNameText);
                    command.Parameters.AddWithValue("@ProductStockQuantity", int.Parse(stockQuantityText));
                    command.Parameters.AddWithValue("@ProductPrice", decimal.Parse(priceText));
                    command.Parameters.AddWithValue("@ProductCategoryID", int.Parse(categoryIDText));
                    command.Parameters.AddWithValue("@ProductBrand", brandText);
                    command.ExecuteNonQuery();
                }

                // Hiển thị thông báo thành công và đóng cửa sổ chỉnh sửa
                MessageBox.Query("Success", "Successfully edited product!", "OK");
                top.Remove(editProductWin);
                switch (role)
                {
                    case "admin":
                        DisplayProduct("admin");
                        break;
                    case "superadmin":
                        DisplayProduct("superadmin");
                        break;
                }
            }
            catch
            {
                // Hiển thị thông báo lỗi nếu có lỗi xảy ra
                MessageBox.ErrorQuery("Error", "An error occurred while editing the product. Please try again.", "OK");
            }
        };

        // Button to close the window
        var closeButton = new Button("Close")
        {
            X = 20,
            Y = Pos.Bottom(saveButton) + 1
        };
        closeButton.Clicked += () =>
        {
            // Hiển thị thông báo xác nhận trước khi đóng cửa sổ
            bool confirmed = MessageBox.Query("Confirm", "Are you sure you want to close?", "Yes", "No") == 0;
            if (confirmed)
            {
                top.Remove(editProductWin);
                switch (role)
                {
                    case "admin":
                        DisplayProduct("admin");
                        break;
                    case "superadmin":
                        DisplayProduct("superadmin");
                        break;
                }
            }
        };

        // Thêm tất cả các thành phần vào cửa sổ
        editProductWin.Add(editProductNameLabel, editProductNameField,
                        editProductStockQuantityLabel, editProductStockQuantityField,
                        editProductCategoryIDLabel, editProductCategoryIDField,
                        editProductPriceLabel, editProductPriceField,
                        editProductBrandLabel, editProductBrandField,
                        saveButton, closeButton);
    }
    public void DeleteProduct(int productID)
    {
        try
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = @"UPDATE products 
                                SET is_active = FALSE 
                                WHERE product_id = @ProductID";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@ProductID", productID);
                command.ExecuteNonQuery();
            }

            MessageBox.Query("Success", "Product successfully marked as inactive!", "OK");
        }
        catch
        {
            MessageBox.ErrorQuery("Error", "An error occurred while marking the product as inactive. Please try again.", "OK");
        }
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

        // Event handler for the Find button
        findButton.Clicked += () =>
        {
            try
            {
                string productName = productNameField.Text.ToString();

                // Check if the product name field is empty
                if (string.IsNullOrWhiteSpace(productName))
                {
                    MessageBox.ErrorQuery("Error", "You must enter the product name you want to find.", "OK");
                    return;
                }
                top.Remove(findProductWin);

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
                                    WHERE p.product_name Like @ProductName AND is_active = TRUE";
                    MySqlCommand command = new MySqlCommand(query, connection);
                    command.Parameters.AddWithValue("@ProductName", "%" + productName + "%");
                    connection.Open();
                    MySqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        // Populate product object with retrieved data
                        var product = new Products
                        {
                            ProductName = reader["product_name"].ToString(),
                            ProductStockQuantity = int.Parse(reader["product_stock_quantity"].ToString()),
                            ProductPrice = decimal.Parse(reader["product_price"].ToString()),
                            ProductCategoryName = reader["category_name"].ToString(),
                            ProductBrand = reader["product_brand"].ToString(),
                        };

                        ListProducts.Add(product);
                    }

                    reader.Close();
                }

                if (ListProducts.Count == 0)
                {
                    // Display error if no products found
                    MessageBox.ErrorQuery("Error", "Product not found!", "OK");
                    return;
                }
                else
                {

                    // Create a new window to display the products
                    var productsWindow = new Window("Products")
                    {
                        X = 0,
                        Y = 0,
                        Width = Dim.Fill(),
                        Height = Dim.Fill()
                    };

                    // Create a ScrollView to hold the products
                    var scrollView = new ScrollView()
                    {
                        X = 0,
                        Y = 0,
                        Width = Dim.Fill(),
                        Height = Dim.Fill(),
                        ContentSize = new Size(0, 0) // Kích thước nội dung sẽ được thiết lập sau
                    };

                    productsWindow.Add(scrollView);
                    top.Add(productsWindow);

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

                    foreach (var product in ListProducts)
                    {
                        var productWindow = new Window($"Product {row * colCount + col + 1}")
                        {
                            X = col * (colWidth + margin),
                            Y = row * (rowHeight + margin),
                            Width = colWidth,
                            Height = rowHeight
                        };

                        // Create labels for displaying product details
                        var productNameLabel = new Label($"Name: {product.ProductName}")
                        {
                            X = 1,
                            Y = 1,
                            Width = Dim.Fill(),
                            TextAlignment = TextAlignment.Left // Left text alignment
                        };
                        var productStockQuantityLabel = new Label($"Stock: {product.ProductStockQuantity}")
                        {
                            X = 1,
                            Y = Pos.Bottom(productNameLabel) + 1,
                            Width = Dim.Fill(),
                            TextAlignment = TextAlignment.Left // Left text alignment
                        };
                        var productPriceLabel = new Label($"Price: {product.ProductPrice:C}")
                        {
                            X = 1,
                            Y = Pos.Bottom(productStockQuantityLabel) + 1,
                            Width = Dim.Fill(),
                            TextAlignment = TextAlignment.Left // Left text alignment
                        };
                        var productCategoryLabel = new Label($"Category: {product.ProductCategoryName}")
                        {
                            X = 1,
                            Y = Pos.Bottom(productPriceLabel) + 1,
                            Width = Dim.Fill(),
                            TextAlignment = TextAlignment.Left // Left text alignment
                        };
                        var productBrandLabel = new Label($"Brand: {product.ProductBrand}")
                        {
                            X = 1,
                            Y = Pos.Bottom(productCategoryLabel) + 1,
                            Width = Dim.Fill(),
                            TextAlignment = TextAlignment.Left // Left text alignment
                        };

                        // Add labels to the product window
                        productWindow.Add(productNameLabel, productStockQuantityLabel, productPriceLabel, productCategoryLabel, productBrandLabel);
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

                    // Create a button labeled "Close" to close the products window
                    var btnClose = new Button("Close")
                    {
                        X = Pos.Center(),
                        Y = 1 
                    };

                    // Define the action to be taken when the close button is clicked
                    btnClose.Clicked += () =>
                    {
                        ListProducts.Clear();
                        // Remove the products window
                        top.Remove(productsWindow);
                        switch (role)
                        {
                            case "user":
                                pd.FindProduct("user");
                                break;
                            case "admin":
                                pd.FindProduct("admin");
                                break;
                            case "superadmin":
                                pd.FindProduct("superadmin");
                                break;
                        }
                    };

                    // Add the close button to the products window
                    productsWindow.Add(btnClose);
                }

            }
            catch
            {
                // Display error message for any exception
                MessageBox.ErrorQuery("Error", "An error occurred while searching for products. Please try again.", "OK");
            }
        };

        // Button to close the find product window
        var closeButton = new Button("Back")
        {
            X = Pos.Center(),
            Y = Pos.Percent(100) - 1
        };

        // Event handler for the Close button
        closeButton.Clicked += () =>
        {
            ListProducts.Clear();
            top.Remove(findProductWin);

            // Return to respective menu based on user role
            switch (role)
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
using System;
using System.Text;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Security.Cryptography.X509Certificates;
using Terminal.Gui;
using Google.Protobuf;
using Org.BouncyCastle.Asn1.Tsp;
using System.Security.Cryptography;
using Mysqlx.Resultset;


class Program
{
    static Window mainMenu;
    static Window productMenu;
    static Window customerMenu;
    static Window displayProductWindow;
    static Window addProductWin;
    static Window editProductWin;
    static Window deleteProductWin;
    static Window findProductWin;
    static Window displayCustomerWindow;
    static Window addCustomerWin;
    public static string connectionString;
    public static int currentCustomerID = -1;
    public static List<Products> ListProducts = new List<Products>();
    public static List<Categories> ListCategories = new List<Categories>();
    public static List<Users> ListUsers = new List<Users>();
    public static List<Customers> ListCustomers = new List<Customers>();
    public static List<Cart> ListCarts = new List<Cart>();
    public static Cart userCart = new Cart();

    static Program()
    {
        // Hỏi mật khẩu từ người dùng
        Console.Write("Enter the database password: ");
       string password = Console.ReadLine();

        // Gốc chuỗi kết nối với placeholder @pass
        string baseConnectionString = "Server=localhost;Database=shop;Uid=root;Pwd=@pass";

        // Thay thế @pass bằng mật khẩu thực
        connectionString = baseConnectionString.Replace("@pass", password);
    }

    static List<Products> LoadProducts(string connectionString)
    {
        List<Products> ListProduct = new List<Products>();

        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {   
            string query = "SELECT * FROM products"; 
            MySqlCommand command = new MySqlCommand(query, connection);
            connection.Open();
            MySqlDataReader read = command.ExecuteReader();
            while (read.Read())
            {
                Products sp = new Products();
                // Nạp các thuộc tính 
                sp.ProductID = read.GetInt32("product_id");
                sp.ProductName = read.GetString("product_name");
                sp.ProductDescription = read.GetString("product_description");
                sp.ProductPrice = read.GetDecimal("product_price");
                sp.ProductStockQuantity = read.GetInt32("product_stock_quantity");
                sp.ProductBrand = read.GetString("product_brand");
                sp.ProductCategoryID = read.GetInt32("product_category_id");


                ListProduct.Add(sp);
            }
        }
        return ListProduct;
    }

    static List<Categories> LoadCategory(string connectionString)
    {
        List<Categories> ListCategory = new List<Categories>();

        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {   
            string query = "SELECT * FROM categories"; 
            MySqlCommand command = new MySqlCommand(query, connection);
            connection.Open();
            MySqlDataReader read = command.ExecuteReader();
            while (read.Read())
            {
                Categories c = new Categories();
                // Nạp các thuộc tính 
                c.CategoryID = read.GetInt32("category_id");
                c.CategoryName = read.GetString("category_name");
                c.CategoryDescription = read.GetString("category_description");

                ListCategory.Add(c);
            }
        }
        return ListCategory;
    }

    static List<Customers> LoadCustomers(string connectionString)
    {
        List<Customers> ListCustomers = new List<Customers>();

        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {   
            string query = "SELECT * FROM customers"; 
            MySqlCommand command = new MySqlCommand(query, connection);
            connection.Open();
            MySqlDataReader read = command.ExecuteReader();
            while (read.Read())
            {
                Customers cus = new Customers();
                // Nạp các thuộc tính 
                cus.CustomerID = read.GetInt32("customer_id");
                cus.CustomerName = read.GetString("customer_name");
                cus.CustomerPhone = read.GetString("customer_phone_number");
                cus.CustomerAddress = read.GetString("customer_address");
                cus.CustomerEmail = read.GetString("customer_email");
                cus.CustomerGender = read.GetString("customer_gender");
                cus.CustomerDateOfBirth = read.GetDateTime("customer_date_of_birth");
                cus.CustomerCount = read.GetInt32("customer_count");
                cus.CustomerTotalSpent = read.GetDecimal("custommer_totalspent");

                ListCustomers.Add(cus);
            }
        }
        return ListCustomers;
    }
    static List<Cart> LoadCarts(string connectionString)
    {
        List<Cart> ListCarts = new List<Cart>();

        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {   
            string query = "SELECT * FROM cart"; 
            MySqlCommand command = new MySqlCommand(query, connection);
            connection.Open();
            MySqlDataReader read = command.ExecuteReader();
            while (read.Read())
            {
                Cart gh = new Cart();
                // Nạp các thuộc tính 
                gh.CartID = read.GetInt32("cart_id");
                gh.CartCustomerID = read.GetInt32("cart_customer_id");
                gh.CartProductID = read.GetInt32("cart_product_id");
                gh.CartQuantity = read.GetInt32("cart_quantity");
                gh.CartOrderID = read.GetInt32("cart_order_id");
                gh.CartProductPrice = read.GetDecimal("cart_product_price");
                gh.CartOrderPrice = read.GetDecimal("cart_order_price");
                gh.CartTotalProduct = read.GetInt32("cart_total_product");


                ListCarts.Add(gh);
            }
        }
        return ListCarts;
    }

    static void Main()
    {
        Application.Init();
        Colors.Base.Normal = Application.Driver.MakeAttribute(Color.BrightGreen, Color.Black);
        Colors.Base.Focus = Application.Driver.MakeAttribute(Color.White, Color.DarkGray);
        
        Application.Init();
        AddCustomer();
        Application.Run();
    }

static void MainMenu()
    {
        var top = Application.Top;

        mainMenu = new Window("Main Menu")
        {
            X = 0,
            Y = 0,
            Width = Dim.Fill(),
            Height = Dim.Fill()
        };
        top.Add(mainMenu);

        var btnCustomer = new Button("Customers")
        {
            X = 2,
            Y = 2,
        };
        btnCustomer.Clicked += () =>
        {
            try
            {
                // Xử lý sự kiện cho nút Customers
            }
            catch (Exception ex)
            {
                MessageBox.ErrorQuery("Error", ex.Message, "OK");
            }
        };

        var btnProduct = new Button("Products")
        {
            X = 2,
            Y = 3
        };
        btnProduct.Clicked += () =>
        {
            try
            {
                top.Remove(mainMenu);
                ProductMenu();
            }
            catch (Exception ex)
            {
                MessageBox.ErrorQuery("Error", ex.Message, "OK");
            }
        };

        var btnCategories = new Button("Categories")
        {
            X = 2,
            Y = 4
        };
        btnCategories.Clicked += () =>
        {
            try
            {
                // Xử lý sự kiện cho nút Categories
            }
            catch (Exception ex)
            {
                MessageBox.ErrorQuery("Error", ex.Message, "OK");
            }
        };

        var btnOrder = new Button("Orders")
        {
            X = 2,
            Y = 5,
        };
        btnOrder.Clicked += () =>
        {
            try
            {
                // Xử lý sự kiện cho nút Orders
            }
            catch (Exception ex)
            {
                MessageBox.ErrorQuery("Error", ex.Message, "OK");
            }
        };

        var btnClose = new Button("Close")
        {
            X = Pos.Center(),
            Y = Pos.Percent(100) - 1
        };
        btnClose.Clicked += () =>
        {
            Application.RequestStop();
        };

        mainMenu.Add(btnCustomer, btnProduct, btnCategories, btnOrder, btnClose);
    }

    static void ProductMenu()
    {
        var top = Application.Top;

        productMenu = new Window("Product Menu")
        {
            X = 0,
            Y = 0,
            Width = Dim.Fill(),
            Height = Dim.Fill()
        };
        top.Add(productMenu);
        productMenu.FocusNext();

        var btnDisplayProduct = new Button("Display Product")
        {
            X = 2,
            Y = 2
        };
        btnDisplayProduct.Clicked += () =>
        {
            try
            {
                top.Remove(productMenu);
                DisplayProduct();
            }
            catch (Exception ex)
            {
                MessageBox.ErrorQuery("Error", ex.Message, "OK");
            }
        };

        var btnAddProduct = new Button("Add Product")
        {
            X = 2,
            Y = 3
        };
        btnAddProduct.Clicked += () =>
        {
            try
            {
                top.Remove(productMenu);
                AddProduct();
            }
            catch (Exception ex)
            {
                MessageBox.ErrorQuery("Error", ex.Message, "OK");
            }
        };

        var btnFindProduct = new Button("Find Product")
        {
            X = 2,
            Y = 4
        };
        btnFindProduct.Clicked += () =>
        {
            try
            {
                top.Remove(productMenu);
                FindProduct();
            }
            catch (Exception ex)
            {
                MessageBox.ErrorQuery("Error", ex.Message, "OK");
            }
        };

        var btnDeleteProduct = new Button("Delete Product")
        {
            X = 2,
            Y = 5
        };
        btnDeleteProduct.Clicked += () =>
        {
            try
            {
                top.Remove(productMenu);
                DeleteProduct();
            }
            catch (Exception ex)
            {
                MessageBox.ErrorQuery("Error", ex.Message, "OK");
            }
        };

        var btnEditProduct = new Button("Edit Product")
        {
            X = 2,
            Y = 6
        };
        btnEditProduct.Clicked += () =>
        {
            try
            {
                top.Remove(productMenu);
                EditProductInformations();
            }
            catch (Exception ex)
            {
                MessageBox.ErrorQuery("Error", ex.Message, "OK");
            }
        };

        var btnClose = new Button("Close")
        {
            X = Pos.Center(),
            Y = Pos.Percent(100) - 1
        };
        btnClose.Clicked += () =>
        {
            top.Remove(productMenu);
            MainMenu();
        };

        productMenu.Add(btnDisplayProduct, btnAddProduct, btnFindProduct, btnDeleteProduct, btnEditProduct, btnClose);
    }

   static void DisplayProduct()
{
    var top = Application.Top;
    displayProductWindow = new Window()
    {
        Title = "Product List",
        X = 0,
        Y = 0,
        Width = Dim.Fill(),
        Height = Dim.Fill()
    };
    top.Add(displayProductWindow);

    using (MySqlConnection connection = new MySqlConnection(connectionString))
    {
        string query = @"SELECT 
                            p.product_id,
                            p.product_name, 
                            p.product_stock_quantity, 
                            p.product_description, 
                            p.product_price, 
                            c.category_name, 
                            p.product_brand
                        FROM products p
                        INNER JOIN categories c ON p.product_category_id = c.category_id;";
        MySqlCommand command = new MySqlCommand(query, connection);
        connection.Open();
        MySqlDataReader read = command.ExecuteReader();
        var columnDisplayListProduct = new string[]
        {
            "Product's name", "Stock quantity", "Description", "Price", "Category's name", "Brand", "Quantity", "Action"
        };

        // Hiển thị tiêu đề cột
        for (int i = 0; i < columnDisplayListProduct.Length; i++)
        {
            displayProductWindow.Add(new Label(columnDisplayListProduct[i])
            {
                X = i * 15,
                Y = 0,
                Width = 15,
                Height = 1
            });
        }

        int row = 1;
        while (read.Read())
        {
            int productID = int.Parse(read["product_id"].ToString());

            var productLabel = new Label($"{read["product_name"]}")
            {
                X = 0,
                Y = row
            };
            var stockQuantityLabel = new Label($"{read["product_stock_quantity"]}")
            {
                X = 15,
                Y = row
            };
            var descriptionLabel = new Label($"{read["product_description"]}")
            {
                X = 30,
                Y = row
            };
            var priceLabel = new Label($"{read["product_price"]}")
            {
                X = 45,
                Y = row
            };
            var categoryLabel = new Label($"{read["category_name"]}")
            {
                X = 60,
                Y = row
            };
            var brandLabel = new Label($"{read["product_brand"]}")
            {
                X = 75,
                Y = row
            };

            var textQuantity = new TextField()
            {
                X = 90,
                Y = row,
                Width = 10
            };

            var addButton = new Button("Add to Cart")
            {
                X = 105,
                Y = row
            };
            addButton.Clicked += () =>
            {
                int quantity = int.Parse(textQuantity.Text.ToString());
                AddToCart(productID, quantity);
            };

            displayProductWindow.Add(productLabel, stockQuantityLabel, descriptionLabel, priceLabel, categoryLabel, brandLabel, textQuantity, addButton);
            row++;
        }
    }

    var btnBack = new Button("Back")
    {
        X = 2,
        Y = Pos.AnchorEnd(2) ,
    };
    btnBack.Clicked += () =>
    {
        top.Remove(displayProductWindow);
        ProductMenu();
    };

    displayProductWindow.Add(btnBack);
}
    static void AddProduct()
{
    Products sp = new Products();
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

    // Tạo các nhãn và trường nhập liệu
    var productNameLabel = new Label("Product Name:")
    {
        X = 2,
        Y = 2
    };
    var productNameField = new TextField("")
    {
        X = Pos.Right(productNameLabel) + 1,
        Y = 2,
        Width = Dim.Fill() - 4
    };

    var productStockQuantityLabel = new Label("Stock Quantity:")
    {
        X = 2,
        Y = 4
    };
    var productStockQuantityField = new TextField("")
    {
        X = Pos.Right(productStockQuantityLabel) + 1,
        Y = 4,
        Width = Dim.Fill() - 4
    };

    var productCategoryIDLabel = new Label("Category ID:")
    {
        X = 2,
        Y = 6
    };
    var productCategoryIDField = new TextField("")
    {
        X = Pos.Right(productCategoryIDLabel) + 1,
        Y = 6,
        Width = Dim.Fill() - 4
    };

    var productPriceLabel = new Label("Price:")
    {
        X = 2,
        Y = 8
    };
    var productPriceField = new TextField("")
    {
        X = Pos.Right(productPriceLabel) + 1,
        Y = 8,
        Width = Dim.Fill() - 4
    };

    var productDescriptionLabel = new Label("Description:")
    {
        X = 2,
        Y = 10
    };
    var productDescriptionField = new TextField("")
    {
        X = Pos.Right(productDescriptionLabel) + 1,
        Y = 10,
        Width = Dim.Fill() - 4
    };

    var productBrandLabel = new Label("Brand:")
    {
        X = 2,
        Y = 12
    };
    var productBrandField = new TextField("")
    {
        X = Pos.Right(productBrandLabel) + 1,
        Y = 12,
        Width = Dim.Fill() - 4
    };

    var saveButton = new Button("Save")
    {
        X = Pos.Center(),
        Y = 14
    };
    saveButton.Clicked += () =>
    {
        try
        {
            sp.ProductName = productNameField.Text.ToString();
            sp.ProductStockQuantity = int.Parse(productStockQuantityField.Text.ToString());
            sp.ProductCategoryID = int.Parse(productCategoryIDField.Text.ToString());
            sp.ProductPrice = decimal.Parse(productPriceField.Text.ToString());
            sp.ProductDescription = productDescriptionField.Text.ToString();
            sp.ProductBrand = productBrandField.Text.ToString();

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = "INSERT INTO products (product_name, product_stock_quantity, product_category_id, product_price, product_description, product_brand) VALUES (@ProductName, @ProductStockQuantity, @ProductCategoryID, @ProductPrice, @ProductDescription, @ProductBrand)";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@ProductName", sp.ProductName);
                command.Parameters.AddWithValue("@ProductStockQuantity", sp.ProductStockQuantity);
                command.Parameters.AddWithValue("@ProductCategoryID", sp.ProductCategoryID);
                command.Parameters.AddWithValue("@ProductPrice", sp.ProductPrice);
                command.Parameters.AddWithValue("@ProductDescription", sp.ProductDescription);
                command.Parameters.AddWithValue("@ProductBrand", sp.ProductBrand);
                command.ExecuteNonQuery();
            }

            ListProducts.Add(sp);
            MessageBox.Query("Success", "Successfully added new product!", "OK");
            top.Remove(addProductWin);
            ProductMenu();
        }
        catch (Exception ex)
        {
            MessageBox.ErrorQuery("Error", ex.Message, "OK");
        }
    };

    var closeButton = new Button("Close")
    {
        X = Pos.Center(),
        Y = Pos.Bottom(saveButton) + 1
    };
    closeButton.Clicked += () =>
    {
        top.Remove(addProductWin);
        ProductMenu();
    };

    addProductWin.Add(productNameLabel, productNameField, productStockQuantityLabel, productStockQuantityField,
                      productCategoryIDLabel, productCategoryIDField, productPriceLabel, productPriceField,
                      productDescriptionLabel, productDescriptionField, productBrandLabel, productBrandField,
                      saveButton, closeButton);

}
    static void EditProductInformations()
{
    Products sp = new Products();
    ListProducts = LoadProducts(connectionString);
    var top = Application.Top;
    var editProductWin = new Window("Edit Product Information")
    {
        X = 0,
        Y = 0,
        Width = Dim.Fill(),
        Height = Dim.Fill()
    };
    top.Add(editProductWin);

    var findProductIDLabel = new Label("Find Product ID:")
    {
        X = 2,
        Y = 2
    };

    var findProductIDField = new TextField("")
    {
        X = Pos.Right(findProductIDLabel) + 1,
        Y = 2,
        Width = Dim.Fill() - 4
    };

    var editProductNameLabel = new Label("Product Name:")
    {
        X = 2,
        Y = 4
    };

    var editProductNameField = new TextField("")
    {
        X = Pos.Right(editProductNameLabel) + 1,
        Y = 4,
        Width = Dim.Fill() - 4
    };

    var editProductStockQuantityLabel = new Label("Stock Quantity:")
    {
        X = 2,
        Y = 6
    };

    var editProductStockQuantityField = new TextField("")
    {
        X = Pos.Right(editProductStockQuantityLabel) + 1,
        Y = 6,
        Width = Dim.Fill() - 4
    };

    var editProductCategoryIDLabel = new Label("Category ID:")
    {
        X = 2,
        Y = 8
    };

    var editProductCategoryIDField = new TextField("")
    {
        X = Pos.Right(editProductCategoryIDLabel) + 1,
        Y = 8,
        Width = Dim.Fill() - 4
    };

    var editProductPriceLabel = new Label("Price:")
    {
        X = 2,
        Y = 10
    };
    var editProductPriceField = new TextField("")
    {
        X = Pos.Right(editProductPriceLabel) + 1,
        Y = 10,
        Width = Dim.Fill() - 4
    };

    var editProductDescriptionLabel = new Label("Description:")
    {
        X = 2,
        Y = 12
    };

    var editProductDescriptionField = new TextField("")
    {
        X = Pos.Right(editProductDescriptionLabel) + 1,
        Y = 12,
        Width = Dim.Fill() - 4
    };

    var editProductBrandLabel = new Label("Brand:")
    {
        X = 2,
        Y = 14
    };

    var editProductBrandField = new TextField("")
    {
        X = Pos.Right(editProductBrandLabel) + 1,
        Y = 14,
        Width = Dim.Fill() - 4
    };

    var saveButton = new Button("Save")
    {
        X = Pos.Center(),
        Y = 16
    };
    saveButton.Clicked += () =>
    {
        try
        {
            sp.ProductName = editProductNameField.Text.ToString();
            sp.ProductStockQuantity = int.Parse(editProductStockQuantityField.Text.ToString());
            sp.ProductCategoryID = int.Parse(editProductCategoryIDField.Text.ToString());
            sp.ProductPrice = decimal.Parse(editProductPriceField.Text.ToString());
            sp.ProductDescription = editProductDescriptionField.Text.ToString();
            sp.ProductBrand = editProductBrandField.Text.ToString();

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = "UPDATE products SET product_name = @ProductName, product_stock_quantity = @ProductStockQuantity, product_description = @ProductDescription, product_price = @ProductPrice, product_category_id = @ProductCategoryID, product_brand = @ProductBrand WHERE product_id = @ProductID";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@ProductID", sp.ProductID);
                command.Parameters.AddWithValue("@ProductName", sp.ProductName);
                command.Parameters.AddWithValue("@ProductStockQuantity", sp.ProductStockQuantity);
                command.Parameters.AddWithValue("@ProductDescription", sp.ProductDescription);
                command.Parameters.AddWithValue("@ProductPrice", sp.ProductPrice);
                command.Parameters.AddWithValue("@ProductCategoryID", sp.ProductCategoryID);
                command.Parameters.AddWithValue("@ProductBrand", sp.ProductBrand);
                command.ExecuteNonQuery();
            }

            MessageBox.Query("Success", "Successfully edited product!", "OK");
            top.Remove(editProductWin);
            ProductMenu();
        }
        catch (Exception ex)
        {
            MessageBox.ErrorQuery("Error", ex.Message, "OK");
        }
    };

    var confirmButton = new Button("Confirm")
    {
        X = Pos.Bottom(findProductIDField) + 1,
        Y = 2
    };
    confirmButton.Clicked += () =>
    {
        try
        {
            if (int.TryParse(findProductIDField.Text.ToString(), out int productID))
            {
                var product = ListProducts.FirstOrDefault(p => p.ProductID == productID);
                if (product != null)
                {
                    sp = product;
                    findProductIDLabel.Visible = false;
                    findProductIDField.Visible = false;
                    confirmButton.Visible = false;

                    editProductNameLabel.Visible = true;
                    editProductNameField.Visible = true;
                    editProductStockQuantityLabel.Visible = true;
                    editProductStockQuantityField.Visible = true;
                    editProductCategoryIDLabel.Visible = true;
                    editProductCategoryIDField.Visible = true;
                    editProductPriceLabel.Visible = true;
                    editProductPriceField.Visible = true;
                    editProductDescriptionLabel.Visible = true;
                    editProductDescriptionField.Visible = true;
                    editProductBrandLabel.Visible = true;
                    editProductBrandField.Visible = true;
                    saveButton.Visible = true;
                }
                else
                {
                    MessageBox.ErrorQuery("Error", "Product not found!", "OK");
                }
            }
            else
            {
                MessageBox.ErrorQuery("Error", "Invalid Product ID!", "OK");
            }
        }
        catch (Exception ex)
        {
            MessageBox.ErrorQuery("Error", ex.Message, "OK");
        }
    };

    var closeButton = new Button("Close")
    {
        X = Pos.Center(),
        Y = Pos.Bottom(saveButton) + 1
    };
    closeButton.Clicked += () =>
    {
        top.Remove(editProductWin);
        ProductMenu();
    };

    // Initial visibility
    editProductNameLabel.Visible = false;
    editProductNameField.Visible = false;
    editProductStockQuantityLabel.Visible = false;
    editProductStockQuantityField.Visible = false;
    editProductCategoryIDLabel.Visible = false;
    editProductCategoryIDField.Visible = false;
    editProductPriceLabel.Visible = false;
    editProductPriceField.Visible = false;
    editProductDescriptionLabel.Visible = false;
    editProductDescriptionField.Visible = false;
    editProductBrandLabel.Visible = false;
    editProductBrandField.Visible = false;
    saveButton.Visible = false;

    editProductWin.Add(findProductIDLabel, findProductIDField, confirmButton,
                       editProductNameLabel, editProductNameField,
                       editProductStockQuantityLabel, editProductStockQuantityField,
                       editProductCategoryIDLabel, editProductCategoryIDField,
                       editProductPriceLabel, editProductPriceField,
                       editProductDescriptionLabel, editProductDescriptionField,
                       editProductBrandLabel, editProductBrandField,
                       saveButton, closeButton);

}


    static void DeleteProduct()
{
    ListProducts = LoadProducts(connectionString);
    var top = Application.Top;
    var deleteProductWin = new Window("Delete Product")
    {
        X = 0,
        Y = 0,
        Width = Dim.Fill(),
        Height = Dim.Fill()
    };
    top.Add(deleteProductWin);

    var productIDLabel = new Label("Product ID:")
    {
        X = 2,
        Y = 2
    };
    var productIDField = new TextField("")
    {
        X = Pos.Right(productIDLabel) + 1,
        Y = 2,
        Width = Dim.Fill() - 4
    };

    var deleteButton = new Button("Delete")
    {
        X = Pos.Center(),
        Y = 4
    };
    deleteButton.Clicked += () =>
    {
        try
        {

            if (int.TryParse(productIDField.Text.ToString(), out int productID))
            {
                Products sp = ListProducts.Find(s => s.ProductID == productID);

                if (sp != null)
                {
                    using (MySqlConnection connection = new MySqlConnection(connectionString))
                    {
                        connection.Open();
                        string query = "DELETE FROM products WHERE product_id = @ProductID";
                        MySqlCommand command = new MySqlCommand(query, connection);
                        command.Parameters.AddWithValue("@ProductID", sp.ProductID);
                        command.ExecuteNonQuery();
                    }
                    ListProducts.Remove(sp);
                    MessageBox.Query("Success", "Successfully deleted product!", "OK");
                    top.Remove(deleteProductWin);
                    ProductMenu();
                }
                else
                {
                    MessageBox.ErrorQuery("Error", "Product not found!", "OK");
                }
            }
            else
            {
                MessageBox.ErrorQuery("Error", "Invalid Product ID!", "OK");
            }
        }
        catch (Exception ex)
        {
            MessageBox.ErrorQuery("Error", ex.Message, "OK");
        }
    };

    var closeButton = new Button("Close")
    {
        X = Pos.Center(),
        Y = Pos.Bottom(deleteButton) + 1
    };
    closeButton.Clicked += () =>
    {
        Application.RequestStop();
        top.Remove(deleteProductWin);
        ProductMenu();
    };

    deleteProductWin.Add(productIDLabel, productIDField, deleteButton, closeButton);

}


   static void FindProduct()
{
    ListProducts = LoadProducts(connectionString);
    var top = Application.Top;
    var findProductWin = new Window("Find Product")
    {
        X = 0,
        Y = 0,
        Width = Dim.Fill(),
        Height = Dim.Fill()
    };
    top.Add(findProductWin);

    var productIDLabel = new Label("Product ID:")
    {
        X = 2,
        Y = 2
    };
    var productIDField = new TextField("")
    {
        X = Pos.Right(productIDLabel) + 1,
        Y = 2,
        Width = Dim.Fill() - 4
    };

    var findButton = new Button("Find")
    {
        X = Pos.Center(),
        Y = 4
    };
    findButton.Clicked += () =>
    {
        try
        {
            int productID = int.Parse(productIDField.Text.ToString());
            List<Products> matchedProducts = ListProducts.FindAll(s => s.ProductID == productID);

            if (matchedProducts.Count > 0)
            {
                StringBuilder message = new StringBuilder();
                foreach (var product in matchedProducts)
                {
                    message.AppendLine($"Name: {product.ProductName}");
                    message.AppendLine($"Stock Quantity: {product.ProductStockQuantity}");
                    message.AppendLine($"Category ID: {product.ProductCategoryID}");
                    message.AppendLine($"Price: {product.ProductPrice}");
                    message.AppendLine($"Description: {product.ProductDescription}");
                    message.AppendLine($"Brand: {product.ProductBrand}");
                    message.AppendLine(); // Add a blank line between products
                }

                MessageBox.Query($"Products Matching ID {productID}", message.ToString(), "OK");
            }
            else
            {
                MessageBox.ErrorQuery("Error", "Product not found!", "OK");
            }
        }
        catch (Exception ex)
        {
            MessageBox.ErrorQuery("Error", ex.Message, "OK");
        }
    };

    var closeButton = new Button("Close")
    {
        X = Pos.Center(),
        Y = Pos.Bottom(findButton) + 1
    };
    closeButton.Clicked += () =>
    {
        Application.RequestStop();
    };
    findProductWin.Add(productIDLabel, productIDField, findButton, closeButton);

}

/*static void FindProduct()
{
    ListProducts = LoadProducts(connectionString);
    var top = Application.Top;
    var findProductWin = new Window("Find Product")
    {
        X = 0,
        Y = 0,
        Width = Dim.Fill(),
        Height = Dim.Fill()
    };
    top.Add(findProductWin);

    var productNameLabel = new Label("Product Name:")
    {
        X = 2,
        Y = 2
    };
    var productNameField = new TextField("")
    {
        X = Pos.Right(productNameLabel) + 1,
        Y = 2,
        Width = Dim.Fill() - 4
    };

    var findButton = new Button("Find")
    {
        X = Pos.Center(),
        Y = 4
    };
    findButton.Clicked += () =>
    {
        try
        {
            string productName = productNameField.Text.ToString();
            List<Products> matchedProducts = ListProducts.FindAll(s => s.ProductName.Equals(productName, StringComparison.OrdinalIgnoreCase));

            if (matchedProducts.Count > 0)
            {
                StringBuilder message = new StringBuilder();
                foreach (var product in matchedProducts)
                {
                    message.AppendLine($"Name: {product.ProductName}");
                    message.AppendLine($"Stock Quantity: {product.ProductStockQuantity}");
                    message.AppendLine($"Category ID: {product.ProductCategoryID}");
                    message.AppendLine($"Price: {product.ProductPrice}");
                    message.AppendLine($"Description: {product.ProductDescription}");
                    message.AppendLine($"Brand: {product.ProductBrand}");
                    message.AppendLine(); // Add a blank line between products
                }

                MessageBox.Query($"Products Matching {productName}", message.ToString(), "OK");
            }
            else
            {
                MessageBox.ErrorQuery("Error", "Product not found!", "OK");
            }
        }
        catch (Exception ex)
        {
            MessageBox.ErrorQuery("Error", ex.Message, "OK");
        }
    };

    var closeButton = new Button("Close")
    {
        X = Pos.Center(),
        Y = Pos.Bottom(findButton) + 1
    };
    closeButton.Clicked += () =>
    {
        top.Remove(findProductWin);
        ProductMenu();
    };
    findProductWin.Add(productNameLabel, productNameField, findButton, closeButton);

}
*/
  static void Login()
    {
        var top = Application.Top;
        var loginWin = new Window()
        {
            Title = "Login",
            X = 0,
            Y = 0,
            Width = Dim.Fill(),
            Height = Dim.Fill()
        };
        top.Add(loginWin);

        var usernameLabel = new Label("Username:")
        {
            X = 2,
            Y = 2
        };
        var usernameField = new TextField("")
        {
            X = Pos.Right(usernameLabel) + 1,
            Y = 2,
            Width = Dim.Fill() - 4
        };

        var passwordLabel = new Label("Password:")
        {
            X = 2,
            Y = 4
        };
        var passwordField = new TextField("")
        {
            Secret = true,
            X = Pos.Right(passwordLabel) + 1,
            Y = 4,
            Width = Dim.Fill() - 4
        };

        var loginButton = new Button("Login")
        {
            X = Pos.Center(),
            Y = 6
        };
        loginButton.Clicked += () =>
        {
            string username = usernameField.Text.ToString();
            string password = passwordField.Text.ToString();
            bool isAuthenticated = false;
            string role = "";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT customer_id, password_hash, role FROM users WHERE username = @Username";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@Username", username);

                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    string storedHash = reader.GetString("password_hash");
                    if (password == storedHash)
                    {
                        isAuthenticated = true;
                        role = reader.GetString("role");
                        currentCustomerID = reader.GetInt32("customer_id"); // Lưu giữ customerID sau khi đăng nhập
                    }
                }
            }

            if (isAuthenticated)
            {
                MessageBox.Query("Success", $"Welcome {role}!", "OK");
                top.Remove(loginWin);
                switch (role)
                {
                    case "user":
                        UserMenu();
                        break;
                    case "admin":
                        AdminMenu();
                        break;
                    case "superadmin":
                        SuperAdminMenu();
                        break;
                }
            }
            else
            {
                MessageBox.ErrorQuery("Error", "Invalid username or password!", "OK");
            }
        };

        var closeButton = new Button("Close")
        {
            X = Pos.Center(),
            Y = Pos.Bottom(loginButton) + 1
        };
        closeButton.Clicked += () =>
        {
            top.Remove(loginWin);
            MainMenu();
        };

        loginWin.Add(usernameLabel, usernameField, passwordLabel, passwordField, loginButton, closeButton);
    }

    static void RegisterUser()
    {
        Register("user");
    }

    static void RegisterAdmin()
    {
        Register("admin");
    }

    static void RegisterSuperAdmin()
    {
        Register("superadmin");
    }

    static void Register(string role)
    {
        Users  us = new Users();
        Customers cus = new Customers();
        var top = Application.Top;
        var registerWin = new Window()
        {
            Title = $"Register {role}",
            X = 0,
            Y = 0,
            Width = Dim.Fill(),
            Height = Dim.Fill()
        };
        top.Add(registerWin);

        var usernameLabel = new Label("Username:")
        {
            X = 2,
            Y = 2
        };
        var usernameField = new TextField("")
        {
            X = Pos.Right(usernameLabel) + 1,
            Y = 2,
            Width = Dim.Fill() - 4
        };

        var passwordLabel = new Label("Password:")
        {
            X = 2,
            Y = 4
        };
        var passwordField = new TextField("")
        {
            Secret = true,
            X = Pos.Right(passwordLabel) + 1,
            Y = 4,
            Width = Dim.Fill() - 4
        };
        var CustomerNameLabel = new Label("Name: ")
        {
            X = 2,
            Y = 6
        };
        var CustomerNameField = new TextField("")
        {
            X = Pos.Right(CustomerNameLabel) + 1,
            Y  = 6,
            Width = Dim.Fill() - 4
        };
        var CustomerPhoneNumberLabel = new Label("Phone number: ")
        {
            X = 2,
            Y = 8
        };
        var CustomerPhoneNumberField = new TextField("")
        {
            X = Pos.Right(CustomerPhoneNumberLabel) + 1,
            Y  = 8,
            Width = Dim.Fill() - 4
        };
        var CustomerAddressLabel = new Label("Address: ")
        {
            X = 2,
            Y = 10
        };
        var CustomerAddressField = new TextField("")
        {
            X = Pos.Right(CustomerAddressLabel) + 1,
            Y  = 10,
            Width = Dim.Fill() - 4
        };
        var CustomerEmailLabel = new Label("Email: ")
        {
            X = 2,
            Y = 12
        };
        var CustomerEmailField = new TextField("")
        {
            X = Pos.Right(CustomerEmailLabel) + 1,
            Y  = 12,
            Width = Dim.Fill() - 4
        };
        var CustomerGenderLabel = new Label("Gender: ")
        {
            X = 2,
            Y = 14
        };
        var CustomerGenderField = new TextField("")
        {
            X = Pos.Right(CustomerGenderLabel) + 1,
            Y  = 14,
            Width = Dim.Fill() - 4
        };
        var CustomerDateOfBirthLabel = new Label("Date of birth (YYYY-MM-DD): ")
        {
            X = 2,
            Y = 16
        };
        var CustomerDateOfBirthField = new TextField("")
        {
            X = Pos.Right(CustomerDateOfBirthLabel) + 1,
            Y  = 16,
            Width = Dim.Fill() - 4
        };

        var registerButton = new Button("Register")
        {
            X = Pos.Center(),
            Y = Pos.Bottom(CustomerDateOfBirthField),
        };
        registerButton.Clicked += () =>
        {
            us.Username = usernameField.Text.ToString();
            us.PasswordHash = passwordField.Text.ToString();
            cus.CustomerName = CustomerNameField.Text.ToString();
            cus.CustomerPhone = CustomerPhoneNumberField.Text.ToString();
            cus.CustomerAddress = CustomerAddressField.Text.ToString();
            cus.CustomerEmail = CustomerEmailField.Text.ToString();
            cus.CustomerGender = CustomerGenderField.Text.ToString();
            
            DateTime customerDateOfBirth;
            if (!DateTime.TryParse(CustomerDateOfBirthField.Text.ToString(), out customerDateOfBirth))
            {
                MessageBox.ErrorQuery("Error", "Invalid date format. Please use YYYY-MM-DD.", "OK");
                return;
            }
            cus.CustomerDateOfBirth = customerDateOfBirth;

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string customerquery ="INSERT INTO customers (customer_name, customer_phone_number, customer_address, customer_email, customer_gender, customer_dateofbirth, customer_count, customer_totalspent)"+
                                    "VALUES (@customername, @customerphonenumber, @customeraddress, @customeremail, @customergender, @customerdateofbirth, 0, 0)";
                MySqlCommand customercommand = new MySqlCommand(customerquery, connection);
                customercommand.Parameters.AddWithValue("@customername", cus.CustomerName);
                customercommand.Parameters.AddWithValue("@customerphonenumber", cus.CustomerPhone);
                customercommand.Parameters.AddWithValue("@customeraddress",cus.CustomerAddress);
                customercommand.Parameters.AddWithValue("@customeremail", cus.CustomerEmail);
                customercommand.Parameters.AddWithValue("@customergender", cus.CustomerGender);
                customercommand.Parameters.AddWithValue("@customerdateofbirth", cus.CustomerDateOfBirth);
                customercommand.ExecuteNonQuery();

                long customerId = customercommand.LastInsertedId;
                string userquery = "INSERT INTO users (username, password_hash, role, customer_id) VALUES (@Username, @PasswordHash, @Role, @CustomerId)";
                MySqlCommand usercommand = new MySqlCommand(userquery, connection);
                usercommand.Parameters.AddWithValue("@Username", us.Username);
                usercommand.Parameters.AddWithValue("@PasswordHash", us.PasswordHash);
                usercommand.Parameters.AddWithValue("@Role", role);
                usercommand.Parameters.AddWithValue("@CustomerId", customerId);

                try
                {
                    usercommand.ExecuteNonQuery();
                    ListUsers.Add(us);
                    ListCustomers.Add(cus);
                    MessageBox.Query("Success", "Registration successful!", "OK");
                    Application.RequestStop();
                }
                catch (Exception ex)
                {
                    MessageBox.ErrorQuery("Error", ex.Message, "OK");
                }
            }
        };

        var closeButton = new Button("Close")
        {
            X = Pos.Center(),
            Y = Pos.Bottom(registerButton) + 1
        };
        closeButton.Clicked += () =>
        {
            top.Remove(registerWin);
            MainMenu();
        };

        registerWin.Add(usernameLabel, usernameField, passwordLabel, passwordField,
                        CustomerNameLabel, CustomerNameField, CustomerPhoneNumberLabel,
                        CustomerPhoneNumberField, CustomerAddressLabel, CustomerAddressField,
                        CustomerEmailLabel, CustomerEmailField, CustomerGenderLabel, CustomerGenderField,
                        CustomerDateOfBirthLabel, CustomerDateOfBirthField, registerButton, closeButton);

    }

   static void UserMenu()
    {
        var top = Application.Top;
        var userMenu = new Window()
        {
            Title = "User Menu",
            X = 0,
            Y = 0,
            Width = Dim.Fill(),
            Height = Dim.Fill()
        };
        top.Add(userMenu);

        var btnViewProducts = new Button("View Products")
        {
            X = 2,
            Y = 2,
        };
        btnViewProducts.Clicked += () =>
        {
            try
            {
                top.Remove(userMenu);
                ProductMenu();
            }
            catch (Exception ex)
            {
                MessageBox.ErrorQuery("Error", ex.Message, "OK");
            }
        };

        var btnViewCart = new Button("View Cart")
        {
            X = 2,
            Y = 3,
        };
        btnViewCart.Clicked += () =>
        {
            try
            {
                DisplayCart();
            }
            catch (Exception ex)
            {
                MessageBox.ErrorQuery("Error", ex.Message, "OK");
            }
        };

        var btnLogout = new Button("Logout")
        {
            X = 2,
            Y = 4,
        };
        btnLogout.Clicked += () =>
        {
            top.Remove(userMenu);
            Login();
        };

        userMenu.Add(btnViewProducts, btnViewCart, btnLogout);
    }
    static void AdminMenu()
    {
        var top = Application.Top;
        var adminMenu = new Window()
        {
            Title = "Admin Menu",
            X = 0,
            Y = 0,
            Width = Dim.Fill(),
            Height = Dim.Fill()
        };
        top.Add(adminMenu);

        var closeButton = new Button("Close")
        {
            X = Pos.Center(),
            Y = Pos.Percent(100) - 3
        };
        closeButton.Clicked += () => 
        {
            top.Remove(adminMenu);
            MainMenu();
        };
        adminMenu.Add(closeButton);
    }

    static void SuperAdminMenu()
    {
        var top = Application.Top;
        var superAdminMenu = new Window()
        {
            Title = "Super Admin Menu",
            X = 0,
            Y = 0,
            Width = Dim.Fill(),
            Height = Dim.Fill()
        };
        top.Add(superAdminMenu);

        var closeButton = new Button("Close")
        {
            X = Pos.Center(),
            Y = Pos.Percent(100) - 3
        };
        closeButton.Clicked += () => 
        {
            top.Remove(superAdminMenu);
            MainMenu();
        };
        superAdminMenu.Add(closeButton);

    }

    static void CustomerMenu()
    {
        var top = Application.Top;
        customerMenu = new Window()
        {
            Title = "Customer Menu",
            X = 0,
            Y = 0,
            Width = Dim.Fill(),
            Height = Dim.Fill()
        };
        top.Add(customerMenu);
        customerMenu.FocusNext();
        var btnDisplayCustomer = new Button("Display Customer")
        {
            X =2,
            Y = 2
        };
        btnDisplayCustomer.Clicked += () =>
        {
            try
            {
                top.Remove(customerMenu);
            }
            catch (Exception ex)
            {
                MessageBox.ErrorQuery("Error", ex.Message, "OK");
            }
        };

        var btnAddCustomer = new Button("Add Customer")
        {
            X =2,
            Y = 4
        };
        btnAddCustomer.Clicked += () =>
        {
            try
            {
                top.Remove(customerMenu);
            }
            catch (Exception ex)
            {
                MessageBox.ErrorQuery("Error", ex.Message, "OK");
            }
        };

        var btnEditCustomer = new Button("Edit Customer:")
        {
            X =2,
            Y = 6
        };
        btnEditCustomer.Clicked += () =>
        {
            try
            {
                top.Remove(customerMenu);
                EditCustomer();
            }
            catch (Exception ex)
            {
                MessageBox.ErrorQuery("Error", ex.Message, "OK");
            }
        };

        var btnDeleteCustomer = new Button("Delete Customer:")
        {
            X =2,
            Y = 8
        };
        btnDeleteCustomer.Clicked += () =>
        {
            try
            {
                top.Remove(customerMenu);
                DeleteCustomer();
            }
            catch (Exception ex)
            {
                MessageBox.ErrorQuery("Error", ex.Message, "OK");
            }
        };

        var btnClose = new Button("Close")
        {
            X = Pos.Center(),
            Y = Pos.Percent(100) - 3
        };

        customerMenu.Add(btnDisplayCustomer, btnAddCustomer, btnEditCustomer, btnDeleteCustomer, btnClose);

    }
 
    static void EditCustomer()
    {
        Customers kh = new Customers();
        ListCustomers = LoadCustomers(connectionString);

        var top = Application.Top;
        var editCustomerWin = new Window("Edit Customer")
        {
            X = 0,
            Y = 0,
            Width = Dim.Fill() - 4,
            Height = Dim.Fill() - 4
        };
        top.Add(editCustomerWin);

        var findCustomerIDLabel = new Label("Find Customer ID:")
        {
            X = 1,
            Y = 1
        };

        var findCustomerIDField = new TextField("")
        {
            X = Pos.Right(findCustomerIDLabel) + 1,
            Y = 1,
            Width = Dim.Fill() - 4
        };

        var editCustomerNameLabel = new Label("Customer Name:")
        {
            X = 1,
            Y = 3
        };

        var editCustomerNameField = new TextField("")
        {
            X = Pos.Right(editCustomerNameLabel) + 1,
            Y = 3,
            Width = Dim.Fill() - 4
        };

        var editCustomerPhoneLabel = new Label("Customer Phone:")
        {
            X = 1,
            Y = 5
        };
        var editCustomerPhoneField = new TextField("")
        {
            X = Pos.Right(editCustomerPhoneLabel) + 1,
            Y = 5,
            Width = Dim.Fill() - 4
        };

        var editCustomerAddressLabel = new Label("Customer Address:")
        {
            X = 1,
            Y = 7
        };

        var editCustomerAddressField = new TextField("")
        {
            X = Pos.Right(editCustomerAddressLabel) + 1,
            Y = 7,
            Width = Dim.Fill() - 4
        };

        var editCustomerEmailLabel = new Label("Customer Email:")
        {
            X = 1,
            Y = 9
        };

        var editCustomerEmailField = new TextField("")
        {
            X = Pos.Right(editCustomerEmailLabel) + 1,
            Y = 9,
            Width = Dim.Fill() - 4
        };

        var editCustomerGenderLabel = new Label("Customer Gender:")
        {
            X = 1,
            Y = 11
        };
        var editCustomerGenderField = new TextField("")
        {
            X = Pos.Right(editCustomerGenderLabel) + 1,
            Y = 11,
            Width = Dim.Fill() - 4
        };

        var editCustomerDateOfBirthLabel = new Label("Customer Date of Birth:")
        {
            X = 1,
            Y = 13
        };
        var editCustomerDateOfBirthField = new TextField("")
        {
            X = Pos.Right(editCustomerDateOfBirthLabel) + 1,
            Y = 13,
            Width = Dim.Fill() - 4
        };

        
        var saveButton = new Button("Save")
        {
            X = Pos.Center(),
            Y = 19
        };
        saveButton.Clicked += () =>
        {
            try
            {
                kh.CustomerID = int.Parse(findCustomerIDField.Text.ToString());
                kh.CustomerName = editCustomerNameField.Text.ToString();
                kh.CustomerPhone = editCustomerPhoneField.Text.ToString();
                kh.CustomerAddress = editCustomerAddressField.Text.ToString();
                kh.CustomerEmail = editCustomerEmailField.Text.ToString();
                kh.CustomerGender = editCustomerGenderField.Text.ToString();
                kh.CustomerDateOfBirth = DateTime.Parse(editCustomerDateOfBirthField.Text.ToString());
                

                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "UPDATE customers SET customer_name = @CustomerName, customer_phone_number = @CustomerPhone, customer_address = @CustomerAddress, customer_email = @CustomerEmail, customer_gender = @CustomerGender, customer_dateofbirth = @CustomerDateOfBirth WHERE customer_id = @CustomerID";
                    MySqlCommand command = new MySqlCommand(query, connection);
                    command.Parameters.AddWithValue("@CustomerID", kh.CustomerID);
                    command.Parameters.AddWithValue("@CustomerName", kh.CustomerName);
                    command.Parameters.AddWithValue("@CustomerPhone", kh.CustomerPhone);
                    command.Parameters.AddWithValue("@CustomerAddress", kh.CustomerAddress);
                    command.Parameters.AddWithValue("@CustomerEmail", kh.CustomerEmail);
                    command.Parameters.AddWithValue("@CustomerGender", kh.CustomerGender);
                    command.Parameters.AddWithValue("@CustomerDateOfBirth", kh.CustomerDateOfBirth);

                    
                    command.ExecuteNonQuery();
                }
                MessageBox.Query("Success", "Customer has been updated", "OK");
                top.Remove(editCustomerWin);
                CustomerMenu();
            }
            catch (Exception ex)
            {
                MessageBox.ErrorQuery("Error", ex.Message, "OK");
            }
        };

        var confirmButton = new Button("Confirm")
        {
            X = Pos.Bottom(findCustomerIDField) +1,
            Y= 2
        };
        confirmButton.Clicked += () =>
        {
            try
            {
                if(int.TryParse(findCustomerIDField.Text.ToString(), out int customerID))
                {
                    var customer = ListCustomers.FirstOrDefault(k => k.CustomerID == customerID);
                    if (customer!= null)
                    {
                        findCustomerIDLabel.Visible = false;
                        findCustomerIDField.Visible = false;
                        confirmButton.Visible = false;

                        editCustomerNameLabel.Visible = true;
                        editCustomerNameField.Visible = true;
                        editCustomerPhoneLabel.Visible = true;
                        editCustomerPhoneField.Visible = true;
                        editCustomerAddressLabel.Visible = true;
                        editCustomerAddressField.Visible = true;
                        editCustomerEmailLabel.Visible = true;
                        editCustomerEmailField.Visible = true;
                        editCustomerGenderLabel.Visible = true;
                        editCustomerGenderField.Visible = true;
                        editCustomerDateOfBirthLabel.Visible = true;
                        editCustomerDateOfBirthField.Visible = true;
                    
                        saveButton.Visible = true;

                    }
                    else
                    {
                        MessageBox.ErrorQuery("Error", "Customer not found", "OK");
                    }
                }
                else
                {
                    MessageBox.ErrorQuery("Error", "Invalid Customer ID", "OK");
                }
            }
            catch (Exception ex)
            {
                MessageBox.ErrorQuery("Error", ex.Message, "OK");
            }
        };
        var closeButton = new Button("Close")
        {
            X = Pos.Center(),
            Y = Pos.Bottom(saveButton) + 1
        };
        closeButton.Clicked += () =>
        {
            top.Remove(editCustomerWin);
            CustomerMenu();
        };

        editCustomerNameLabel.Visible = false;
        editCustomerNameField.Visible = false;
        editCustomerPhoneLabel.Visible = false;
        editCustomerPhoneField.Visible = false;
        editCustomerAddressLabel.Visible = false;
        editCustomerAddressField.Visible = false;
        editCustomerEmailLabel.Visible = false;
        editCustomerEmailField.Visible = false;
        editCustomerGenderLabel.Visible = false;
        editCustomerGenderField.Visible = false;
        editCustomerDateOfBirthLabel.Visible = false;;
        editCustomerDateOfBirthField.Visible = false;;
        
        saveButton.Visible = false;


        editCustomerWin.Add(findCustomerIDLabel,findCustomerIDField,confirmButton,editCustomerNameLabel,editCustomerNameField,editCustomerPhoneLabel,
                            editCustomerPhoneField,editCustomerAddressLabel,editCustomerAddressField,editCustomerEmailLabel,editCustomerEmailField,
                            editCustomerGenderLabel,editCustomerGenderField,editCustomerDateOfBirthLabel,editCustomerDateOfBirthField,saveButton);
    }
    static void DeleteCustomer()
    {
        ListCustomers = LoadCustomers(connectionString);
        var top = Application.Top;
        var deleteCustomerWin = new Window("Delete Customer")
        {
            X = 0,
            Y = 0,
            Width = Dim.Fill() - 4,
            Height = Dim.Fill() - 4
        };
        top.Add(deleteCustomerWin);

        var customerIDLabel = new Label("Customer ID:")
        {
            X = 1,
            Y = 1
        };

        var customerIDField = new TextField("")
        {
            X = Pos.Right(customerIDLabel) + 1,
            Y = 1,
            Width = Dim.Fill() - 4
        };

        var deleteButton = new Button("Delete")
        {
            X = Pos.Center(),
            Y = Pos.Bottom(customerIDField) + 1
        };

        deleteButton.Clicked += () =>
        {
            try
            {

                if(int.TryParse(customerIDField.Text.ToString(), out int customerID))
                {
                    Customers kh = ListCustomers.Find(k => k.CustomerID == customerID);

                    if (kh!= null)
                    {
                        using (MySqlConnection connection = new MySqlConnection(connectionString))
                        {
                            connection.Open();
                            string customerquery = "DELETE FROM customers WHERE customer_id = @customerid";
                            MySqlCommand customercommand = new MySqlCommand(customerquery, connection);
                            customercommand.Parameters.AddWithValue("@customerid", kh.CustomerID);
                            customercommand.ExecuteNonQuery();
                        }
                        ListCustomers.Remove(kh);
                        MessageBox.Query("Success", "Successfully deleted customer!", "OK");
                        top.Remove(deleteCustomerWin);
                        CustomerMenu();
                    }
                    else
                    {
                        MessageBox.ErrorQuery("Error", "Customer not found!", "OK");
                    }
                }
                else
                {
                    MessageBox.ErrorQuery("Error", "Invalid customer ID!", "OK");
                }
            }
            catch (Exception ex)
            {
                MessageBox.ErrorQuery("Error", ex.Message, "OK");
            }

    };

    var closeButton = new Button("Close")
    {
        X = Pos.Center(),
        Y = Pos.Bottom(deleteButton) + 1
    };
    closeButton.Clicked += () =>
    {
        top.Remove(deleteCustomerWin);
        CustomerMenu();
    };

    deleteCustomerWin.Add(customerIDLabel, customerIDField, deleteButton, closeButton);
    }
    static void DisplayCustomers()
    {
        List<Customers> ListCustomers = LoadCustomers(connectionString);
        List<string[]> customers = new List<string[]>();
        var top = Application.Top;

        displayCustomerWindow = new Window("Display Customers")
        {
            X = 0,
            Y = 0,
            Width = Dim.Fill(),
            Height = Dim.Fill()
        };
        top.Add(displayCustomerWindow);
        displayCustomerWindow.FocusNext();

        using (MySqlConnection connection = new MySqlConnection(connectionString))
    {
        string query = @"SELECT 
                            cus.customer_name, 
                            cus.customer_phone_number, 
                            cus.customer_address, 
                            cus.customer_email, 
                            cus.customer_gender, 
                            cus.customer_dateofbirth,
                            cus.customer_count,
                            cus.customer_totalspent
                        FROM customers cus";
        MySqlCommand command = new MySqlCommand(query, connection);
        connection.Open();
        MySqlDataReader read = command.ExecuteReader();
        var columnDisplayListCustomer = new string[]
        {
            "Customer's name", "Phone number", "Address", "Email", "Gender", "Date of birth", "Order count", "Total spent"
        };
        while (read.Read())
        {
            customers.Add(new string[]{
                read["customer_name"].ToString(),
                read["customer_phone_number"].ToString(),
                read["customer_address"].ToString(),
                read["customer_email"].ToString(),
                read["customer_gender"].ToString(),
                read["customer_dateofbirth"].ToString(),
                read["customer_count"].ToString(),
                read["customer_totalspent"].ToString()

            });
        }
        for (int i = 0; i < columnDisplayListCustomer.Length; i++)
        {
            displayCustomerWindow.Add(new Label(columnDisplayListCustomer[i])
            {
                X = i * 15,
                Y = 0,
                Width = 15,
                Height = 1
            });
        }
        for (int i = 0; i < customers.Count; i++)
        {
            for (int j = 0; j < customers[i].Length; j++)
            {
                displayCustomerWindow.Add(new Label(customers[i][j])
                {
                    X = j * 15,
                    Y = i + 1,
                    Width = 15,
                    Height = 1
                });
            }
        }
    }

        var btnClose = new Button("Close")
        {
            X = Pos.Center(),
            Y = Pos.Percent(100) - 1
        };
        btnClose.Clicked += () =>
        {
            top.Remove(displayCustomerWindow);
            CustomerMenu();
        };

        displayCustomerWindow.Add(btnClose);
    }
    static void AddCustomer()
    {
        Customers cus = new Customers();
        var top = Application.Top;
        addCustomerWin = new Window("Add Customer")
        {
            X = 0,
            Y = 0,
            Width = Dim.Fill(),
            Height = Dim.Fill()
        };
        top.Add(addCustomerWin);
        addCustomerWin.FocusNext();

        var customerNameLabel = new Label("Customer Name: ")
        {
            X = 2,
            Y = 2
        };

        var customerNameField = new TextField("")
        {
            X = Pos.Right(customerNameLabel) + 1,
            Y = 2,
            Width = Dim.Fill() - 4
        };

        var customerAddressLabel = new Label("Customer Address: ")
        {
            X = 2,
            Y = 4
        };

        var customerAddressField = new TextField("")
        {
            X = Pos.Right(customerAddressLabel) + 1,
            Y = 4,
            Width = Dim.Fill() - 4
        };

        var customerPhoneNumberLabel = new Label("Customer Phone: ")
        {
            X = 2,
            Y = 4
        };

        var customerPhoneNumberField = new TextField("")
        {
            X = Pos.Right(customerPhoneNumberLabel) + 1,
            Y = 4,
            Width = Dim.Fill() - 4
        };

        var customerEmailLabel = new Label("Customer Email: ")
        {
            X = 2,
            Y = 4
        };

        var customerEmailField = new TextField("")
        {
            X = Pos.Right(customerEmailLabel) + 1,
            Y = 4,
            Width = Dim.Fill() - 4
        };

        var customerGenderLabel = new Label("Customer Gender: ")
        {
            X = 2,
            Y = 4
        };

        var customerGenderField = new TextField("")
        {
            X = Pos.Right(customerGenderLabel) + 1,
            Y = 4,
            Width = Dim.Fill() - 4
        };

        var customerDateOfBirthLabel = new Label("Customer Date Of Birth: ")
        {
            X = 2,
            Y = 4
        };

        var customerDateOfBirthField = new TextField("")
        {
            X = Pos.Right(customerDateOfBirthLabel) + 1,
            Y = 4,
            Width = Dim.Fill() - 4
        };

        var saveButton = new Button("Save")
        {
            X = Pos.Center(),
            Y = 14
        };
        saveButton.Clicked += () =>
        {
            try
            {
                cus.CustomerName = customerNameField.Text.ToString();
                cus.CustomerAddress = customerAddressField.Text.ToString();
                cus.CustomerPhone = customerPhoneNumberField.Text.ToString();
                cus.CustomerEmail = customerEmailField.Text.ToString();
                cus.CustomerGender = customerGenderField.Text.ToString();
                DateTime customerDateOfBirth;
                if (!DateTime.TryParse(customerDateOfBirthField.Text.ToString(), out customerDateOfBirth))
                {
MessageBox.ErrorQuery("Error", "Invalid date format. Please use YYYY-MM-DD.", "OK");
                    return;
                }
                cus.CustomerDateOfBirth = customerDateOfBirth;

                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "INSERT INTO customers (customer_name, customer_address, customer_phone_number, customer_email, customer_gender, customer_dateofbirth, customer_count, customer_totalspent) VALUES (@CustomerName, @CustomerAddress, @CustomerPhone, @CustomerEmail, @CustomerGender, @CustomerDateOfBirth)";
                    MySqlCommand command = new MySqlCommand(query, connection);
                    command.Parameters.AddWithValue("@ProductName", cus.CustomerName);
                    command.Parameters.AddWithValue("@CustomerAddress", cus.CustomerAddress);
                    command.Parameters.AddWithValue("@CustomerPhoneNumber", cus.CustomerPhone);
                    command.Parameters.AddWithValue("@CustomerEmail", cus.CustomerEmail);
                    command.Parameters.AddWithValue("@CustomerGender", cus.CustomerGender);
                    command.Parameters.AddWithValue("@CustomerDateOfBirth", cus.CustomerDateOfBirth);
                    command.ExecuteNonQuery();
                }

                ListCustomers.Add(cus);
                MessageBox.Query("Success", "Successfully added new customer infomration!");
                top.Remove(addCustomerWin);
                CustomerMenu();
            }
            catch (Exception ex)
            {
                MessageBox.ErrorQuery("Error", ex.Message, "OK");
            }
        };

        var closeButton = new Button("Close")
        {
            X = Pos.Center(),
            Y = Pos.Bottom(saveButton) + 1
        };
        closeButton.Clicked += () =>
        {
            top.Remove(addCustomerWin);
            CustomerMenu();
        };

        addCustomerWin.Add(customerNameLabel, customerAddressLabel, customerPhoneNumberLabel,
                            customerEmailLabel, customerGenderLabel, customerDateOfBirthLabel);
    }
// Cart
static void DisplayCart()
    {
        var top = Application.Top;

        var cartWindow = new Window("Cart")
        {
            X = 0,
            Y = 0,
            Width = Dim.Fill(),
            Height = Dim.Fill()
        };
        top.Add(cartWindow);

        int row = 1;

        // Kết nối cơ sở dữ liệu để lấy thông tin giỏ hàng của khách hàng
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            string query = @"SELECT 
                                p.product_id,
                                p.product_name, 
                                p.product_price, 
                                c.cart_quantity 
                            FROM cart c
                            INNER JOIN products p ON c.cart_product_id = p.product_id
                            WHERE c.cart_customer_id = @CustomerID";
            MySqlCommand command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@CustomerID", currentCustomerID);
            connection.Open();
            MySqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                int productID = reader.GetInt32("product_id");
                string productName = reader["product_name"].ToString();
                decimal productPrice = reader.GetDecimal("product_price");
                int quantity = reader.GetInt32("cart_quantity");

                var productLabel = new Label($"{productName} - ${productPrice} x {quantity}")
                {
                    X = 1,
                    Y = row
                };
                var removeButton = new Button("Remove")
                {
                    X = Pos.Right(productLabel) + 1,
                    Y = row
                };
                removeButton.Clicked += () =>
                {
                    RemoveItemFromCart(productID);
                    top.Remove(cartWindow);
                    DisplayCart();
                };

                cartWindow.Add(productLabel, removeButton);
                row++;
            }
        }

        var btnBack = new Button("Back")
        {
            X = 2,
            Y = row + 1
        };
        btnBack.Clicked += () =>
        {
            top.Remove(cartWindow);
            MainMenu();
        };

        cartWindow.Add(btnBack);
    }
    static void RemoveItemFromCart(int productID)
    {
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            string query = "DELETE FROM cart WHERE cart_customer_id = @CustomerID AND cart_product_id = @ProductID";
            MySqlCommand command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@CustomerID", currentCustomerID);
            command.Parameters.AddWithValue("@ProductID", productID);
            connection.Open();
            command.ExecuteNonQuery();
        }
    }
    static void AddToCart(int productID, int quantityProduct)
{
    ListProducts = LoadProducts(connectionString);
    Products sp = ListProducts.FirstOrDefault(p => p.ProductID == productID);
    if (sp != null)
    {
        userCart.AddItem(sp, quantityProduct);
        using(MySqlConnection connection = new MySqlConnection(connectionString))
        {
            connection.Open();
            // Tạm thời bỏ cart_order_id
            string query = "INSERT INTO cart (cart_customer_id, cart_product_id, cart_quantity, cart_product_price, cart_order_price, cart_total_products)" +
                        "VALUES(@CartCustomerID, @CartProductID, @CartQuantity, 0, 0, 0)";
            MySqlCommand command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@CartProductID", productID);
            command.Parameters.AddWithValue("@CartQuantity", quantityProduct);
            command.Parameters.AddWithValue("@CartCustomerID", currentCustomerID);

            try
            {
                command.ExecuteNonQuery();
                ListCarts.Add(userCart);
                MessageBox.Query("Success", "Product added to cart.", "OK");
            }
            catch (Exception ex)
            {
                MessageBox.ErrorQuery("Error", ex.Message, "OK");
            }
        }
    }
    else
    {
        MessageBox.ErrorQuery("Error", "Product not found.", "OK");
    }
}

}
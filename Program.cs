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


class Program
{
    static Window mainMenu;
    static Window productMenu;
    static Window displayProductWindow;
    static Window addProductWin;
    static Window editProductWin;
    static Window deleteProductWin;
    static Window findProductWin;
    public static string connectionString;
    public static List<Products> ListProducts = new List<Products>();
    public static List<Categories> ListCategories = new List<Categories>();
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

    static void Main()
    {
        Application.Init();
        Colors.Base.Normal = Application.Driver.MakeAttribute(Color.BrightGreen, Color.Black);
        Colors.Base.Focus = Application.Driver.MakeAttribute(Color.White, Color.DarkGray);
        
        Application.Init();
        Login();
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
        List<Products> ListProducts = LoadProducts(connectionString);
        ListCategories = LoadCategory(connectionString);
        List<string[]> products = new List<string[]>();
        var top = Application.Top;

        displayProductWindow = new Window("Display Products")
        {
            X = 0,
            Y = 0,
            Width = Dim.Fill(),
            Height = Dim.Fill()
        };
        top.Add(displayProductWindow);
        displayProductWindow.FocusNext();

        using (MySqlConnection connection = new MySqlConnection(connectionString))
    {
        string query = @"SELECT 
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
            "Product's name", "Stock quantity", "Description", "Price", "Category's name", "Brand"
        };
        while (read.Read())
        {
            products.Add(new string[]{
                read["product_name"].ToString(),
                read["product_stock_quantity"].ToString(),
                read["product_description"].ToString(),
                read["product_price"].ToString(),
                read["category_name"].ToString(),
                read["product_brand"].ToString()
            });
        }
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
        for (int i = 0; i < products.Count; i++)
        {
            for (int j = 0; j < products[i].Length; j++)
            {
                displayProductWindow.Add(new Label(products[i][j])
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
            top.Remove(displayProductWindow);
            ProductMenu();
        };

        displayProductWindow.Add(btnClose);
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

                    editProductNameField.Text = sp.ProductName;
                    editProductStockQuantityField.Text = sp.ProductStockQuantity.ToString();
                    editProductCategoryIDField.Text = sp.ProductCategoryID.ToString();
                    editProductPriceField.Text = sp.ProductPrice.ToString();
                    editProductDescriptionField.Text = sp.ProductDescription;
                    editProductBrandField.Text = sp.ProductBrand;

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
                string query = "SELECT username, password_hash, role FROM users WHERE username = @Username";
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

        var registerButton = new Button("Register")
        {
            X = Pos.Center(),
            Y = 6
        };
        registerButton.Clicked += () =>
        {
            string username = usernameField.Text.ToString();
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(passwordField.Text.ToString());

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = $"INSERT INTO users (username, password_hash, role) VALUES (@Username, @PasswordHash, @Role)";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@Username", username);
                command.Parameters.AddWithValue("@PasswordHash", passwordHash);
                command.Parameters.AddWithValue("@Role", role);
                try
                {
                    command.ExecuteNonQuery();
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

        registerWin.Add(usernameLabel, usernameField, passwordLabel, passwordField, registerButton, closeButton);
        Application.Run(registerWin);
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

        var closeButton = new Button("Close")
        {
            X = Pos.Center(),
            Y = Pos.Percent(100) - 3
        };
        closeButton.Clicked += () => 
        {
            top.Remove(userMenu);
            MainMenu();
        };
        userMenu.Add(closeButton);

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


}
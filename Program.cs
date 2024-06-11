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
    static Window MenuMain;
    static Window menuProduct;
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
        
        Editproductinformations(ListProducts);
    }

    static void MainMenu()
    {
        Application.Init();

        MenuMain = new Window("Main Menu")
        {
            X = 0,
            Y = 0,
            Width = Dim.Fill(),
            Height = Dim.Fill()
        }; 

        var btnCustomer = new Button("Cutomers")
        {
            X = 2,
            Y = 2,
        };
        btnCustomer.Clicked += () =>
        {
            try
            {

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
            MenuMain.Dispose();
            Application.Run(/*quay lại menu chính*/);    
        };
        Application.Run();
    }

    static void ProductMenu()
    {
        Application.Init();

        var top = Application.Top;

        // Cửa sổ chính
        menuProduct = new Window("Main Menu")
        {
            X = 0,
            Y = 0,
            Width = Dim.Fill(),
            Height = Dim.Fill()
        };
        top.Add(menuProduct);

        // Tạo nút Display
        var btnDisplayProduct = new Button("Display Product")
        {
            X = 2,
            Y = 2 
        };
        btnDisplayProduct.Clicked += () =>
        {
            try
            {
                DisplayProduct(ListProducts);
            }
            catch (Exception ex)
            {
                MessageBox.ErrorQuery("Error", ex.Message, "OK");
            }
        };
        var btnAddProduct = new Button("Add product")
        {
            X = 2,
            Y = 3
        };
        btnAddProduct.Clicked += () =>
        {
            try
            {
                
            }
            catch (Exception ex)
            {
                MessageBox.ErrorQuery("Error",ex.Message,"OK");
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
                
            }
            catch (Exception ex)
            {
                MessageBox.ErrorQuery("Error",ex.Message,"OK");
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
                
            }
            catch (Exception ex)
            {
                MessageBox.ErrorQuery("Error",ex.Message,"OK");
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
                
            }
            catch (Exception ex)
            {
                MessageBox.ErrorQuery("Error",ex.Message,"OK");
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
            menuProduct.Dispose();
            Application.Run(MenuMain);    
        };

        // Thêm nút Display vào bảng 
        menuProduct.Add(btnDisplayProduct, btnAddProduct,btnFindProduct, btnDeleteProduct, btnEditProduct, btnClose);

        // Chạy ứng dụng
        Application.Run();
        }
    

    static void DisplayProduct(List<Products> ListProducts)
    {
        ListProducts = LoadProducts(connectionString);
        ListCategories = LoadCategory(connectionString);
        List<string[]> products = new List<string[]>();
        var DisplayListProduct = new Window()
        {
            Title = "List's Product",
            X = 0,
            Y = 0,
            Width = Dim.Fill(),
            Height = Dim.Fill()
        };
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
                products.Add( new string[]{
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
                DisplayListProduct.Add(new Label(columnDisplayListProduct[i])
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
                    DisplayListProduct.Add(new Label(products[i][j])
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
            Application.RequestStop();
            DisplayListProduct.Dispose();
            Application.Run(menuProduct);    
        };
        Application.Top.Add(DisplayListProduct,btnClose);
        Application.Run(DisplayListProduct);
    }

 
   static void AddProduct(List<Products> ListProducts)
    {
        Products sp = new Products();
        var addProductWin = new Window("Add Product")
        {
            X = 0,
            Y = 0,
            Width = Dim.Fill(),
            Height = Dim.Fill() 
        };

        // Tạo các nhãn và trường nhập liệu
        var productNameLabel = new Label("Product name:")
        {
            X = 1,
            Y = 1
        };
        var productNameField = new TextField("")
        {
            X = 1,
            Y = 2,
            Width = Dim.Fill() - 2
        };

        var productStockQuantityLabel = new Label("Product stock quantity:")
        {
            X = 1,
            Y = 4
        };
        var productStockQuantityField = new TextField("")
        {
            X = 1,
            Y = 5,
            Width = Dim.Fill() - 2
        };

        var productCategoryIDLabel = new Label("Product category ID:")
        {
            X = 1,
            Y = 7
        };
        var productCategoryIDField = new TextField("")
        {
            X = 1,
            Y = 8,
            Width = Dim.Fill() - 2
        };

        var productPriceLabel = new Label("Product price:")
        {
            X = 1,
            Y = 10
        };
        var productPriceField = new TextField("")
        {
            X = 1,
            Y = 11,
            Width = Dim.Fill() - 2
        };

        var productDescriptionLabel = new Label("Product description:")
        {
            X = 1,
            Y = 13
        };
        var productDescriptionField = new TextField("")
        {
            X = 1,
            Y = 14,
            Width = Dim.Fill() - 2
        };

        var productBrandLabel = new Label("Product brand:")
        {
            X = 1,
            Y = 16
        };
        var productBrandField = new TextField("")
        {
            X = 1,
            Y = 17,
            Width = Dim.Fill() - 2
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
                Application.RequestStop();
            }
            catch (Exception ex)
            {
                MessageBox.ErrorQuery("Error", ex.Message, "OK");
            }
        };

    addProductWin.Add(productNameLabel, productNameField, productStockQuantityLabel, productStockQuantityField, productCategoryIDLabel, productCategoryIDField,
                      productPriceLabel, productPriceField, productDescriptionLabel, productDescriptionField,
                      productBrandLabel, productBrandField, saveButton);

    Application.Run(addProductWin);
}

    static void Editproductinformations(List<Products> ListProducts)
    {
        Products sp = new Products();
        ListProducts = LoadProducts(connectionString);

        var Editproductinformations = new Window("Edit Productinformations")
        {
            X = 0,
            Y = 0,
            Width = Dim.Fill(),
            Height = Dim.Fill()
        };

        var FindproductIDLabel = new Label("Find Product ID:")
        {
            X = 1,
            Y = 1
        };

        var FindproductIDField = new TextField("")
        {
            X = Pos.Right(FindproductIDLabel) + 1,
            Y = 1,
            Width = Dim.Fill() - 3
        };

        var EditproductnameLabel = new Label("Product Name:")
        {
            X = 1,
            Y = 3
        };

        var EditproductnameField = new TextField("")
        {
            X = Pos.Right(EditproductnameLabel) + 1,
            Y = 3,
            Width = Dim.Fill() - 3
        };

        var EditproductstockquantityLabel = new Label("Stock Quantity:")
        {
            X = 1,
            Y = 5
        };

        var EditproductstockquantityField = new TextField("")
        {
            X = Pos.Right(EditproductstockquantityLabel) +1,
            Y = 5,
            Width = Dim.Fill() - 3
        };

        var EditproductcategoryIDLabel = new Label("Product Category:")
        {
            X = 1,
            Y = 7
        };

        var EditproductcategoryIDField = new TextField("")
        {
            X = Pos.Right(EditproductcategoryIDLabel) +1,
            Y = 7,
            Width = Dim.Fill() - 3
        };

        var EditproductpriceLabel = new Label("Product Price:")
        {
            X = 1,
            Y = 9
        };
        var EditproductpriceField = new TextField("")
        {
            X = Pos.Right(EditproductpriceLabel) +1,
            Y = 9,
            Width = Dim.Fill() - 3
        };

        var EditproductdescriptionLabel = new Label("Product Description:")
        {
            X = 1,
            Y = 11
        };

        var EditproductdescriptionField = new TextField("")
        {
            X = Pos.Right(EditproductdescriptionLabel) +1,
            Y = 11,
            Width = Dim.Fill() - 3
        };

        var EditproductbrandLabel = new Label("Product Brand:")
        {
            X = 1,
            Y = 13
        };

        var EditproductbrandField = new TextField("")
        {
            X = Pos.Bottom(EditproductbrandLabel) +1,
            Y = 13,
            Width = Dim.Fill() - 3
        };
        EditproductnameLabel.Visible = false; 
        EditproductnameField.Visible = false; 
        EditproductstockquantityLabel.Visible = false; 
        EditproductstockquantityField.Visible = false; 
        EditproductcategoryIDLabel.Visible = false; 
        EditproductcategoryIDField.Visible = false; 
        EditproductpriceLabel.Visible = false; 
        EditproductpriceField.Visible = false; 
        EditproductdescriptionLabel.Visible = false; 
        EditproductdescriptionField.Visible = false; 
        EditproductbrandLabel.Visible = false; 
        EditproductbrandField.Visible = false;

        var saveButton = new Button("Save")
        {
            X = Pos.Center(),
            Y = 15
        };
        saveButton.Clicked += () =>
        {
            try
            {
                        // Update the product properties
                        sp.ProductName = EditproductnameField.Text.ToString();
                        sp.ProductStockQuantity = int.Parse(EditproductstockquantityField.Text.ToString());
                        sp.ProductDescription = EditproductdescriptionField.Text.ToString();
                        sp.ProductPrice = decimal.Parse(EditproductpriceField.Text.ToString());
                        sp.ProductCategoryID = int.Parse(EditproductcategoryIDField.Text.ToString());
                        sp.ProductBrand = EditproductbrandField.Text.ToString();

                        // Update the product in the database
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
                        ListProducts.Add(sp);

                        // Reload the list of products from the database
                        ListProducts = LoadProducts(connectionString);

                        MessageBox.Query("Success", "Successfully edited product!", "OK");
                        Application.RequestStop();
                    
                
            }
            catch (Exception ex)
            {
                MessageBox.ErrorQuery("Error", ex.Message, "OK");
            }
        };
        saveButton.Visible = false;
        var btncomfirm = new Button("Comfirm")
        {
            X = Pos.Bottom(saveButton),
            Y = 15
        };
        btncomfirm.Clicked += () =>
        {
            try
            {
                if (int.TryParse(FindproductIDField.Text.ToString(), out int productID))
                {
                    FindproductIDLabel.Visible = false; 
                    FindproductIDField.Visible = false;
                    btncomfirm.Visible = false;
                    EditproductnameLabel.Visible = true; 
                    EditproductnameField.Visible = true; 
                    EditproductstockquantityLabel.Visible = true; 
                    EditproductstockquantityField.Visible = true; 
                    EditproductcategoryIDLabel.Visible = true; 
                    EditproductcategoryIDField.Visible = true; 
                    EditproductpriceLabel.Visible = true; 
                    EditproductpriceField.Visible = true; 
                    EditproductdescriptionLabel.Visible = true; 
                    EditproductdescriptionField.Visible = true; 
                    EditproductbrandLabel.Visible = true; 
                    EditproductbrandField.Visible = true;
                    saveButton.Visible = true;
                    
                }
            }
            catch (Exception ex)
            {
                MessageBox.ErrorQuery("Error", ex.Message, "OK");
            }
        };

    Editproductinformations.Add(FindproductIDLabel, FindproductIDField, btncomfirm , EditproductnameLabel, EditproductnameField , EditproductstockquantityLabel , EditproductstockquantityField, EditproductcategoryIDLabel, EditproductcategoryIDField, EditproductpriceLabel, EditproductpriceField, EditproductdescriptionLabel, EditproductdescriptionField, EditproductbrandLabel, EditproductbrandField, saveButton);

    Application.Run(Editproductinformations);
}




    static void Deleteproduct(List<Products> ListProducts)
    {
        ListProducts = LoadProducts(connectionString);
        Console.Clear();
        Console.WriteLine("Enter the correct product code:");
        int ProductID = int.Parse(Console.ReadLine());
        Products sp = ListProducts.Find(s => s.ProductID == ProductID);

        if (sp!= null)
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
            Console.WriteLine("Successfully deleted product!");
        }
        else
        {
            Console.WriteLine("Product not found!");
        }
    }

    static void Findproduct (List<Products> ListProducts)
    {
        ListProducts = LoadProducts(connectionString);
        Console.Clear();
        Console.WriteLine("Enter the correct product code:");
        int ProductID = int.Parse(Console.ReadLine());
        Products sp = ListProducts.Find(s => s.ProductID == ProductID);
        if (sp!= null)
        {
            Console.WriteLine("Product name: " + sp.ProductName);
            Console.WriteLine("Product category ID: " + sp.ProductCategoryID);
            Console.WriteLine("Product price: " + sp.ProductPrice);
            Console.WriteLine("Product description: " + sp.ProductDescription);
            Console.WriteLine("Product brand: " + sp.ProductBrand);

        }else{
            Console.WriteLine("Product not found!");
        }
    }

}
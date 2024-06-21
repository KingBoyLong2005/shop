using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using Spectre.Console;
using Terminal.Gui;


public class Products
{
    public int ProductID { get; set; }
    public string ProductName { get; set;}
    public int ProductStockQuantity { get; set; }
    public string ProductDescription { get; set; }
    public decimal ProductPrice { get; set; }
    public int ProductCategoryID { get; set; }
    public string ProductBrand { get; set; }
    public static string connectionString = Configuration.ConnectionString;
    public static List<Products> ListProducts = new List<Products>();
    public static Products pd = new Products();

    

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

    public void ProductMenu()
    {
        var top = Application.Top;

        var productMenu = new Window("Product Menu")
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
            Program program = new Program();
            top.Remove(productMenu);
            program.MainMenu();
        };

        productMenu.Add(btnDisplayProduct, btnAddProduct, btnFindProduct, btnDeleteProduct, btnEditProduct, btnClose);
    }

    static void DisplayProduct()
    {
        List<string[]> products = new List<string[]>();
        var top = Application.Top;

        var displayProductWindow = new Window("Product List")
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
            MySqlDataReader reader = command.ExecuteReader();
            var columnDisplayListProduct = new string[]
            {
                "Product's name", "Stock quantity", "Description", "Price", "Category's name", "Brand"
            };

            int columnWidth = 20; // Increase column width

            // Display column headers
            for (int i = 0; i < columnDisplayListProduct.Length; i++)
            {
                displayProductWindow.Add(new Label(columnDisplayListProduct[i])
                {
                    X = i * columnWidth,
                    Y = 0,
                    Width = columnWidth,
                    Height = 1
                });
            }

            int row = 1;
            while (reader.Read())
            {
                var productLabel = new Label($"{reader["product_name"]}")
                {
                    X = 0,
                    Y = row,
                    Width = columnWidth
                };
                var stockQuantityLabel = new Label($"{reader["product_stock_quantity"]}")
                {
                    X = 1 * columnWidth,
                    Y = row,
                    Width = columnWidth
                };
                var descriptionLabel = new Label($"{reader["product_description"]}")
                {
                    X = 2 * columnWidth,
                    Y = row,
                    Width = columnWidth
                };
                var priceLabel = new Label($"{reader["product_price"]}")
                {
                    X = 3 * columnWidth,
                    Y = row,
                    Width = columnWidth
                };
                var categoryLabel = new Label($"{reader["category_name"]}")
                {
                    X = 4 * columnWidth,
                    Y = row,
                    Width = columnWidth
                };
                var brandLabel = new Label($"{reader["product_brand"]}")
                {
                    X = 5 * columnWidth,
                    Y = row,
                    Width = columnWidth
                };

                displayProductWindow.Add(productLabel, stockQuantityLabel, descriptionLabel, priceLabel, categoryLabel, brandLabel);
                row++;
            }
        }

        var btnBack = new Button("Back")
        {
            X = Pos.Center(),
            Y = Pos.Percent(100) - 1
        };
        btnBack.Clicked += () =>
        {
            top.Remove(displayProductWindow);
            pd.ProductMenu();
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
            pd.ProductMenu();
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
        pd.ProductMenu();
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

    var confirmButton = new Button("Confirm")
    {
        X = Pos.Right(findProductIDField) + 1,
        Y = 2
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
            pd.ProductMenu();
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
        pd.ProductMenu();
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
                    pd.ProductMenu();
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
        pd.ProductMenu();
    };

    deleteProductWin.Add(productIDLabel, productIDField, deleteButton, closeButton);

}

    static void FindProduct()
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

    var productNameLabel = new Label("Product name:")
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

    var columnDisplayListProduct = new string[]
    {
        "Product's Name", "Stock Quantity", "Description", "Price", "Category ID", "Brand", "Image"
    };

    // Add column headers
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

    findButton.Clicked += () =>
    {
        try
        {
            string productName = productNameField.Text.ToString();
            Products product = null;

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string query = @"SELECT 
                                    p.product_name, 
                                    p.product_stock_quantity, 
                                    p.product_description, 
                                    p.product_price, 
                                    p.product_category_id, 
                                    p.product_brand
                                FROM products p
                                WHERE p.product_name Like @ProductName";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@ProductName", "%" + productName + "%");
                connection.Open();
                MySqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    product = new Products
                    {
                        ProductName = reader["product_name"].ToString(),
                        ProductStockQuantity = int.Parse(reader["product_stock_quantity"].ToString()),
                        ProductDescription = reader["product_description"].ToString(),
                        ProductPrice = decimal.Parse(reader["product_price"].ToString()),
                        ProductCategoryID = int.Parse(reader["product_category_id"].ToString()),
                        ProductBrand = reader["product_brand"].ToString(),
                    };
                }
                else
                {
                    MessageBox.ErrorQuery("Error", "Product not found!", "OK");
                    return;
                }
            }

            // Display product information
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
            findProductWin.Add(new Label(product.ProductDescription)
            {
                X = 40,
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
            MessageBox.ErrorQuery("Error", ex.Message, "OK");
        }
    };

    var closeButton = new Button("Close")
    {
        X = Pos.Center(),
        Y = Pos.Percent(100) - 1
    };

    closeButton.Clicked += () =>
    {
        top.Remove(findProductWin);
        pd.ProductMenu();
    };

    findProductWin.Add(productNameLabel, productNameField, findButton, closeButton);
}

}

using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
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
    public static Program program = new Program();
    public static Users user = new Users();
    public static Admin admin = new Admin();
    public static SuperAdmin superadmin = new SuperAdmin();
    public static Cart userCart = new Cart();
    public static Customers customer = new Customers();

    

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
                Products pd = new Products();
                // Nạp các thuộc tính 
                pd.ProductID = read.GetInt32("product_id");
                pd.ProductName = read.GetString("product_name");
                pd.ProductDescription = read.GetString("product_description");
                pd.ProductPrice = read.GetDecimal("product_price");
                pd.ProductStockQuantity = read.GetInt32("product_stock_quantity");
                pd.ProductBrand = read.GetString("product_brand");
                pd.ProductCategoryID = read.GetInt32("product_category_id");


                ListProduct.Add(pd);
            }
        }
        return ListProduct;
    }


    public void DisplayProduct(string role)
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
                int productID = int.Parse(reader["product_id"].ToString());

            var productLabel = new Label($"{reader["product_name"]}")
            {
                X = 0,
                Y = row
            };
            var stockQuantityLabel = new Label($"{reader["product_stock_quantity"]}")
            {
                X = 15,
                Y = row
            };
            var descriptionLabel = new Label($"{reader["product_description"]}")
            {
                X = 30,
                Y = row
            };
            var priceLabel = new Label($"{reader["product_price"]}")
            {
                X = 45,
                Y = row
            };
            var categoryLabel = new Label($"{reader["category_name"]}")
            {
                X = 60,
                Y = row
            };
            var brandLabel = new Label($"{reader["product_brand"]}")
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
                userCart.AddToCart(productID, quantity);
            };

            displayProductWindow.Add(productLabel, stockQuantityLabel, descriptionLabel, priceLabel, categoryLabel, brandLabel, textQuantity, addButton);
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
            switch(role)
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
    public void AddProduct()
    {
    Products pd = new Products();
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
            pd.ProductName = productNameField.Text.ToString();
            pd.ProductStockQuantity = int.Parse(productStockQuantityField.Text.ToString());
            pd.ProductCategoryID = int.Parse(productCategoryIDField.Text.ToString());
            pd.ProductPrice = decimal.Parse(productPriceField.Text.ToString());
            pd.ProductDescription = productDescriptionField.Text.ToString();
            pd.ProductBrand = productBrandField.Text.ToString();

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = "INSERT INTO products (product_name, product_stock_quantity, product_category_id, product_price, product_description, product_brand) VALUES (@ProductName, @ProductStockQuantity, @ProductCategoryID, @ProductPrice, @ProductDescription, @ProductBrand)";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@ProductName", pd.ProductName);
                command.Parameters.AddWithValue("@ProductStockQuantity", pd.ProductStockQuantity);
                command.Parameters.AddWithValue("@ProductCategoryID", pd.ProductCategoryID);
                command.Parameters.AddWithValue("@ProductPrice", pd.ProductPrice);
                command.Parameters.AddWithValue("@ProductDescription", pd.ProductDescription);
                command.Parameters.AddWithValue("@ProductBrand", pd.ProductBrand);
                command.ExecuteNonQuery();
            }

            ListProducts.Add(pd);
            MessageBox.Query("Success", "Successfully added new product!", "OK");
            top.Remove(addProductWin);
            admin.AdminMenu();
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
        bool confirmed = MessageBox.Query("Confirm", "Are you sure you want to close?", "Yes", "No") == 0;
        if (confirmed)
        {
            top.Remove(addProductWin);
            admin.AdminMenu();
        }
    };

    addProductWin.Add(productNameLabel, productNameField, productStockQuantityLabel, productStockQuantityField,
                      productCategoryIDLabel, productCategoryIDField, productPriceLabel, productPriceField,
                      productDescriptionLabel, productDescriptionField, productBrandLabel, productBrandField,
                      saveButton, closeButton);

}
  public void EditProductInformations()
{
    Products pd = new Products();
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

    var findProductLabel = new Label("Find Product Name:")
    {
        X = 2,
        Y = 2
    };

    var findProductField = new TextField("")
    {
        X = Pos.Right(findProductLabel) + 1,
        Y = 2,
        Width = Dim.Fill() - 4
    };

    var confirmButton = new Button("Confirm")
    {
        X = Pos.Right(findProductField) + 1,
        Y = 2
    };

    var productListView = new ListView(new List<string>())
    {
        X = 2,
        Y = 4,
        Width = Dim.Fill() - 4,
        Height = 5,
        Visible = false
    };

    var editProductNameLabel = new Label("Product Name:")
    {
        X = 2,
        Y = Pos.Bottom(productListView) + 1
    };

    var editProductNameField = new TextField("")
    {
        X = Pos.Right(editProductNameLabel) + 1,
        Y = Pos.Bottom(productListView) + 1,
        Width = Dim.Fill() - 4
    };

    var editProductStockQuantityLabel = new Label("Stock Quantity:")
    {
        X = 2,
        Y = Pos.Bottom(editProductNameField) + 1
    };

    var editProductStockQuantityField = new TextField("")
    {
        X = Pos.Right(editProductStockQuantityLabel) + 1,
        Y = Pos.Bottom(editProductNameField) + 1,
        Width = Dim.Fill() - 4
    };

    var editProductCategoryIDLabel = new Label("Category ID:")
    {
        X = 2,
        Y = Pos.Bottom(editProductStockQuantityField) + 1
    };

    var editProductCategoryIDField = new TextField("")
    {
        X = Pos.Right(editProductCategoryIDLabel) + 1,
        Y = Pos.Bottom(editProductStockQuantityField) + 1,
        Width = Dim.Fill() - 4
    };

    var editProductPriceLabel = new Label("Price:")
    {
        X = 2,
        Y = Pos.Bottom(editProductCategoryIDField) + 1
    };

    var editProductPriceField = new TextField("")
    {
        X = Pos.Right(editProductPriceLabel) + 1,
        Y = Pos.Bottom(editProductCategoryIDField) + 1,
        Width = Dim.Fill() - 4
    };

    var editProductDescriptionLabel = new Label("Description:")
    {
        X = 2,
        Y = Pos.Bottom(editProductPriceField) + 1
    };

    var editProductDescriptionField = new TextField("")
    {
        X = Pos.Right(editProductDescriptionLabel) + 1,
        Y = Pos.Bottom(editProductPriceField) + 1,
        Width = Dim.Fill() - 4
    };

    var editProductBrandLabel = new Label("Brand:")
    {
        X = 2,
        Y = Pos.Bottom(editProductDescriptionField) + 1
    };

    var editProductBrandField = new TextField("")
    {
        X = Pos.Right(editProductBrandLabel) + 1,
        Y = Pos.Bottom(editProductDescriptionField) + 1,
        Width = Dim.Fill() - 4
    };

    var saveButton = new Button("Save")
    {
        X = Pos.Center(),
        Y = Pos.Bottom(editProductBrandField) + 2
    };
    saveButton.Clicked += () =>
    {
        try
        {
            pd.ProductName = editProductNameField.Text.ToString();
            pd.ProductStockQuantity = int.Parse(editProductStockQuantityField.Text.ToString());
            pd.ProductCategoryID = int.Parse(editProductCategoryIDField.Text.ToString());
            pd.ProductPrice = decimal.Parse(editProductPriceField.Text.ToString());
            pd.ProductDescription = editProductDescriptionField.Text.ToString();
            pd.ProductBrand = editProductBrandField.Text.ToString();

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = "UPDATE products SET product_name = @ProductName, product_stock_quantity = @ProductStockQuantity, product_description = @ProductDescription, product_price = @ProductPrice, product_category_id = @ProductCategoryID, product_brand = @ProductBrand WHERE product_id = @ProductID";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@ProductID", pd.ProductID);
                command.Parameters.AddWithValue("@ProductName", pd.ProductName);
                command.Parameters.AddWithValue("@ProductStockQuantity", pd.ProductStockQuantity);
                command.Parameters.AddWithValue("@ProductDescription", pd.ProductDescription);
                command.Parameters.AddWithValue("@ProductPrice", pd.ProductPrice);
                command.Parameters.AddWithValue("@ProductCategoryID", pd.ProductCategoryID);
                command.Parameters.AddWithValue("@ProductBrand", pd.ProductBrand);
                command.ExecuteNonQuery();
            }

            MessageBox.Query("Success", "Successfully edited product!", "OK");
            top.Remove(editProductWin);
            admin.AdminMenu();
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
        bool confirmed = MessageBox.Query("Confirm", "Are you sure you want to close?", "Yes", "No") == 0;
        if (confirmed)
        {
            top.Remove(editProductWin);
            admin.AdminMenu();
        }
    };

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
                                    product_description, 
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
                        ProductDescription = reader.GetString("product_description"),
                        ProductPrice = reader.GetDecimal("product_price"),
                        ProductCategoryID = reader.GetInt32("product_category_id"),
                        ProductBrand = reader.GetString("product_brand")
                    };

                    productList.Add($"{product.ProductID} - {product.ProductName}");
                    productMap[product.ProductID] = product;
                }

                if (productList.Count > 0)
                {
                    productListView.SetSource(productList);
                    productListView.Visible = true;
                    productListView.OpenSelectedItem += (args) =>
                    {
                        int selectedProductID = int.Parse(productList[args.Item].Split(' ')[0]);
                        pd = productMap[selectedProductID];
                        findProductLabel.Visible = false;
                        findProductField.Visible = false;
                        confirmButton.Visible = false;
                        productListView.Visible = false;

                        editProductNameField.Text = pd.ProductName;
                        editProductStockQuantityField.Text = pd.ProductStockQuantity.ToString();
                        editProductCategoryIDField.Text = pd.ProductCategoryID.ToString();
                        editProductPriceField.Text = pd.ProductPrice.ToString();
                        editProductDescriptionField.Text = pd.ProductDescription;
                        editProductBrandField.Text = pd.ProductBrand;

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

    // Initial visibility
    productListView.Visible = false;
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

    editProductWin.Add(findProductLabel, findProductField, confirmButton, productListView,
                       editProductNameLabel, editProductNameField,
                       editProductStockQuantityLabel, editProductStockQuantityField,
                       editProductCategoryIDLabel, editProductCategoryIDField,
                       editProductPriceLabel, editProductPriceField,
                       editProductDescriptionLabel, editProductDescriptionField,
                       editProductBrandLabel, editProductBrandField,
                       saveButton, closeButton);
}
  public void DeleteProduct()
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
                Products pd = ListProducts.Find(s => s.ProductID == productID);

                if (pd != null)
                {
                    using (MySqlConnection connection = new MySqlConnection(connectionString))
                    {
                        connection.Open();
                        string query = "DELETE FROM products WHERE product_id = @ProductID";
                        MySqlCommand command = new MySqlCommand(query, connection);
                        command.Parameters.AddWithValue("@ProductID", pd.ProductID);
                        command.ExecuteNonQuery();
                    }
                    ListProducts.Remove(pd);
                    MessageBox.Query("Success", "Successfully deleted product!", "OK");
                    top.Remove(deleteProductWin);
                    admin.AdminMenu();
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
        bool confirmed = MessageBox.Query("Confirm", "Are you sure you want to close?", "Yes", "No") == 0;
        if (confirmed)
        {
        top.Remove(deleteProductWin);
        admin.AdminMenu();
        }
    };

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
        switch(role)
        {
            case "user":
            customer.UserMenu();
            break;
            case "admin":
            admin.AdminMenu();
            break;
        }
    };

    findProductWin.Add(productNameLabel, productNameField, findButton, closeButton);
}

}

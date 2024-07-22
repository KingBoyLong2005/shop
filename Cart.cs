using System;
using System.Text;
using System.Data;
using System.Collections.Generic;
using System.Linq;                                      //Import namesapce to use function
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Security.Cryptography.X509Certificates;
using Terminal.Gui;
using Mysqlx.Crud;

public class Cart
{
    public int CartID { get; set; }
    public int CartCustomerID { get; set; }
    public int CartProductID { get; set; }
    public int CartQuantity { get; set; }
    public int CartOrderID { get; set; }
    public decimal CartProductPrice { get; set; }
    public decimal CartOrderPrice { get; set; }
    public int CartTotalProduct { get; set; }


    public static string connectionString = Configuration.ConnectionString;

    public static Customers customer = new Customers();
    public static Cart userCart = new Cart();
    public static Orders order = new Orders();

    public static List<CartItem> CartItems { get; set; } = new List<CartItem>();
    public static List<Cart> ListCarts = new List<Cart>();
    public static List<Products> ListProducts = new List<Products>();

    // Load products from the database
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
                // Load product properties
                sp.ProductID = read.GetInt32("product_id");
                sp.ProductName = read.GetString("product_name");
                sp.ProductPrice = read.GetDecimal("product_price");
                sp.ProductStockQuantity = read.GetInt32("product_stock_quantity");
                sp.ProductBrand = read.GetString("product_brand");
                sp.ProductCategoryID = read.GetInt32("product_category_id");

                ListProduct.Add(sp);
            }
        }
        return ListProduct;
    }

    // Add an item to the shopping cart
    public void AddItem(Products product)
    {
        var cartItem = CartItems.FirstOrDefault(c => c.Product.ProductID == product.ProductID);
        
            CartItems.Add(new CartItem { Product = product});
        
    }
    // Remove an item from the shopping cart
    public void RemoveItem(int productID)
    {
        var cartItem = CartItems.FirstOrDefault(c => c.Product.ProductID == productID);
        if (cartItem != null)
        {
            CartItems.Remove(cartItem);
        }
    }
    // Display the shopping cart contents
    public void DisplayCart()
    {
        var top = Application.Top;

        // Tạo cửa sổ chính
        var cartWindow = new Window("Cart")
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

        cartWindow.Add(scrollView);
        top.Add(cartWindow);

        // Tạo một View để chứa tất cả các sản phẩm trong giỏ hàng
        var cartContainer = new View()
        {
            X = 0,
            Y = 0,
            Width = Dim.Fill(),
            Height = Dim.Fill()
        };

        scrollView.Add(cartContainer);

        int row = 1;

        // Connect to the database to retrieve the customer's cart information
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            string query = @"SELECT 
                                p.product_id,
                                p.product_name, 
                                p.product_price,
                                p.product_brand
                            FROM cart c
                            INNER JOIN products p ON c.cart_product_id = p.product_id
                            WHERE c.cart_customer_id = @CustomerID AND active = TRUE";
            MySqlCommand command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@CustomerID", SessionData.Instance.CurrentCustomerID);
            connection.Open();
            MySqlDataReader reader = command.ExecuteReader();

            int colCount = 1; // Number of columns per row
            int colWidth = 30; // Width of each product window
            int rowHeight = 9; // Height of each product window
            int margin = 2; // Margin between products

            int col = 0;

            while (reader.Read())
            {
                int productID = reader.GetInt32("product_id");
                string productName = reader["product_name"].ToString();
                decimal productPrice = reader.GetDecimal("product_price");
                string productbrand = reader["product_brand"].ToString();

                var productWindow = new Window($"{productName}")
                {
                    X = col * (colWidth + margin),
                    Y = row * (rowHeight + margin),
                    Width = colWidth,
                    Height = rowHeight
                };

                var productLabel = new Label($"Name: {productName}")
                {
                    X = 1,
                    Y = 1,
                    Width = Dim.Fill()
                };
                var priceLabel = new Label($"Price: {productPrice:C}")
                {
                    X = 1,
                    Y = 2,
                    Width = Dim.Fill()
                };
                var brandlabel = new Label($"Brand: {productbrand}")
                {
                    X = 1,
                    Y = 3,
                    Width = Dim.Fill()
                };
                var removeButton = new Button("Remove")
                {
                    X = 1,
                    Y = 5
                };
                var orderButton = new Button("Order")
                {
                    X = Pos.Right(removeButton) + 2,
                    Y = 5
                };

                // Handle click events for removing and ordering products
                removeButton.Clicked += () =>
                {
                    try
                    {
                        RemoveItemFromCart(productID);
                        top.Remove(cartWindow);
                        DisplayCart();
                    }
                    catch
                    {
                        // Display a generic error message if the remove operation fails
                        MessageBox.ErrorQuery("Error", "An unexpected error occurred while removing the item from the cart. Please try again later.", "OK");
                    }
                };

                orderButton.Clicked += () =>
                {
                    try
                    {
                        top.Remove(cartWindow);
                        order.OrderProduct(productID, productName, productPrice, "cart");
                    }
                    catch
                    {
                        // Display a generic error message if the order operation fails
                        MessageBox.ErrorQuery("Error", "An unexpected error occurred while placing the order. Please try again later.", "OK");
                    }
                };
                productWindow.Add(productLabel, priceLabel, brandlabel, removeButton, orderButton);
                cartContainer.Add(productWindow);

                col++;
                if (col >= colCount)
                {
                    col = 0;
                    row++;
                }
            }

            reader.Close();

            // Cập nhật kích thước nội dung của ScrollView
            scrollView.ContentSize = new Size((colWidth + margin) * colCount, (row + 1) * (rowHeight + margin));
        }

        // Đặt nút Back ở dưới cùng
        var btnBack = new Button("Back")
        {
            X = Pos.Center(),
            Y = Pos.Top(scrollView) + 1,
        };
        btnBack.Clicked += () =>
        {
            top.Remove(cartWindow);
            customer.UserMenu();
        };

        cartWindow.Add(btnBack);
    }
    // Remove an item from the database cart and update the UI
    public void RemoveItemFromCart(int productID)
    {
        userCart.RemoveItem(productID);
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            string query = "UPDATE cart SET active = FALSE WHERE cart_customer_id = @CustomerID AND cart_product_id = @ProductID";
            MySqlCommand command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@CustomerID", SessionData.Instance.CurrentCustomerID);
            command.Parameters.AddWithValue("@ProductID", productID);
            connection.Open();
            command.ExecuteNonQuery();
        }
    }

    // Add a product with specified quantity to the shopping cart and database cart
    public void AddToCart(int productID)
    {
        ListProducts = LoadProducts(connectionString);
        Products sp = ListProducts.FirstOrDefault(p => p.ProductID == productID);
        if (sp != null)
        {
            userCart.AddItem(sp);
            using(MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = "INSERT INTO cart (cart_customer_id, cart_product_id, cart_product_price, cart_order_price, cart_total_products)" +
                            "VALUES(@CartCustomerID, @CartProductID, 0, 0, 0)";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@CartProductID", productID);
                command.Parameters.AddWithValue("@CartCustomerID", SessionData.Instance.CurrentCustomerID);

                try
                {
                    command.ExecuteNonQuery();
                    ListCarts.Add(userCart);
                    MessageBox.Query("Success", "Product added to cart.", "OK");
                }
                catch 
                {
                    MessageBox.ErrorQuery("Error", "An unexpected error occurred while adding the product to the cart. Please try again later.", "OK");
                }
            }
        }
        else
        {
            MessageBox.ErrorQuery("Error", "Product not found.", "OK");
        }
    }

}
public class CartItem
{
    public Products Product { get; set; }
    
}

using System;
using System.Text;
using System.Data;
using System.Collections.Generic;
using System.Linq;
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
    public static int currentCustomerID = SessionData.Instance.CurrentCustomerID;
    public static List<Cart> ListCarts = new List<Cart>();
    public static List<Products> ListProducts = new List<Products>();
    public static Program program = new Program();

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

    

    public static List<CartItem> CartItems { get; set; } = new List<CartItem>();

    public void AddItem(Products product, int quantity)
    {
        var cartItem = CartItems.FirstOrDefault(c => c.Product.ProductID == product.ProductID);
        if (cartItem != null)
        {
            cartItem.Quantity += quantity;
        }
        else
        {
            CartItems.Add(new CartItem { Product = product, Quantity = quantity });
        }
    }

    public void RemoveItem(int productID)
    {
        var cartItem = CartItems.FirstOrDefault(c => c.Product.ProductID == productID);
        if (cartItem != null)
        {
            CartItems.Remove(cartItem);
        }
    }
   public void DisplayCart(string role)
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
            var orderButton = new Button("Order")
            {
                X = Pos.Right(removeButton) + 2,
                Y = row
            };

            removeButton.Clicked += () =>
            {
                RemoveItemFromCart(productID);
                top.Remove(cartWindow);
                DisplayCart(role);
            };

            orderButton.Clicked += () =>
            {
                top.Remove(cartWindow);
                order.OrderProduct(productID, productName, productPrice);
            };

            cartWindow.Add(productLabel, removeButton, orderButton);
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
        customer.UserMenu();
    };

    cartWindow.Add(btnBack);
}
    public void RemoveItemFromCart(int productID)
    {
        userCart.RemoveItem(productID);
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
    public void AddToCart(int productID, int quantityProduct)
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


public class CartItem
{
    public Products Product { get; set; }
    public int Quantity { get; set; }
    
}

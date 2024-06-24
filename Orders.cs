using System;
using System.Text;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Security.Cryptography.X509Certificates;
using Terminal.Gui;
public class Orders
{
    public int OrderID { get; set; }
    public int OrderCustomerID { get; set; }
    public Decimal OrderTotalPrice { get; set; }
    public DateTime OrderDate { get; set; }
    public DateTime OrderDeliveryDate { get; set; }
    public string OrderPaymentMethod { get; set; }
    public DateTime OrderPaymentDate { get; set; }
    public string OrderStatus { get; set; }
    public string OrderAddress { get; set; }
    
    public static string connectionString = Configuration.ConnectionString;
    public static Products pd = new Products();
    public static Orders order = new Orders();
    public static int currentCustomerID = SessionData.Instance.CurrentCustomerID;

    public void DisplayProductToOrder()
    {
        List<Products> products = new List<Products>();
        var top = Application.Top;

        var DisplayProductToOrderWin = new Window("Product List")
        {
            X = 0,
            Y = 0,
            Width = Dim.Fill(),
            Height = Dim.Fill()
        };
        top.Add(DisplayProductToOrderWin);
        DisplayProductToOrderWin.FocusNext();

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
            var columnDisplayListProductToOrder = new string[]
            {
                "Product's name", "Stock quantity", "Description", "Price", "Category's name", "Brand"
            };

            int columnWidth = 20; // Increase column width

            // Display column headers
            for (int i = 0; i < columnDisplayListProductToOrder.Length; i++)
            {
                DisplayProductToOrderWin.Add(new Label(columnDisplayListProductToOrder[i])
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
                int productID = reader.GetInt32("product_id");
                string productName = reader["product_name"].ToString();
                decimal productPrice = reader.GetDecimal("product_price");

                var productLabel = new Label($"{productName}")
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
                var priceLabel = new Label($"{productPrice}")
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
                var orderButton = new Button("Order")
                {
                    X = 6 * columnWidth,
                    Y = row
                };
                orderButton.Clicked += () =>
                {
                    top.Remove(DisplayProductToOrderWin);
                    OrderProduct(productID, productName, productPrice);
                };

                DisplayProductToOrderWin.Add(productLabel, stockQuantityLabel, descriptionLabel, priceLabel, categoryLabel, brandLabel, orderButton);
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
            top.Remove(DisplayProductToOrderWin);
            pd.ProductMenu();
        };

        DisplayProductToOrderWin.Add(btnBack);
    }

    static void OrderProduct(int productID, string productName, decimal productPrice)
    {
        var top = Application.Top;
        var orderWindow = new Window("Order Product")
        {
            X = 0,
            Y = 0,
            Width = Dim.Fill(),
            Height = Dim.Fill()
        };
        top.Add(orderWindow);

        var lblProductName = new Label($"Product: {productName}")
        {
            X = 1,
            Y = 1
        };
        orderWindow.Add(lblProductName);

        var lblQuantity = new Label("Quantity:")
        {
            X = 1,
            Y = 3
        };
        var txtQuantity = new TextField("")
        {
            X = Pos.Right(lblQuantity) + 1,
            Y = 3,
            Width = 20
        };
        orderWindow.Add(lblQuantity, txtQuantity);

        var lblDeliveryAddress = new Label("Delivery Address:")
        {
            X = 1,
            Y = 5
        };
        var txtDeliveryAddress = new TextField("")
        {
            X = Pos.Right(lblDeliveryAddress) + 1,
            Y = 5,
            Width = 40
        };
        orderWindow.Add(lblDeliveryAddress, txtDeliveryAddress);

        var lblPaymentMethod = new Label("Payment Method:")
        {
            X = 1,
            Y = 7
        };
        var txtPaymentMethod = new TextField("")
        {
            X = Pos.Right(lblPaymentMethod) + 1,
            Y = 7,
            Width = 20
        };
        orderWindow.Add(lblPaymentMethod, txtPaymentMethod);

        var btnSubmitOrder = new Button("Submit Order")
        {
            X = 1,
            Y = 10
        };
        btnSubmitOrder.Clicked += () =>
        {
            int quantity;
            if (int.TryParse(txtQuantity.Text.ToString(), out quantity) && quantity > 0)
            {
                PlaceOrderDirectly(productID, quantity, txtDeliveryAddress.Text.ToString(), txtPaymentMethod.Text.ToString());
                top.Remove(orderWindow);
            }
            else
            {
                MessageBox.ErrorQuery("Error", "Invalid quantity.", "OK");
            }
        };

        var btnCancel = new Button("Cancel")
        {
            X = Pos.Right(btnSubmitOrder) + 2,
            Y = 10
        };
        btnCancel.Clicked += () =>
        {
            top.Remove(orderWindow);
            order.DisplayProductToOrder();
        };

        orderWindow.Add(btnSubmitOrder, btnCancel);
    }

    static void PlaceOrderDirectly(int productID, int quantity, string deliveryAddress, string paymentMethod)
    {
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            connection.Open();
            string query = @"INSERT INTO orders (order_customer_id, order_product_id, order_quantity, order_date, order_delivery_date, order_payment_method, order_status, order_delivery_address) 
                             VALUES (@CustomerID, @ProductID, @Quantity, @OrderDate, @DeliveryDate, @PaymentMethod, @OrderStatus, @DeliveryAddress);

                            UPDATE `orders` o JOIN `products` p ON o.order_product_id = p.product_id SET o.order_total_price = o.order_quantity * p.product_price;";
            MySqlCommand command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@CustomerID", currentCustomerID);
            command.Parameters.AddWithValue("@ProductID", productID);
            command.Parameters.AddWithValue("@Quantity", quantity);
            command.Parameters.AddWithValue("@OrderDate", DateTime.Now);
            command.Parameters.AddWithValue("@DeliveryDate", DateTime.Now.AddDays(7)); // Giả sử thời gian giao hàng là 7 ngày
            command.Parameters.AddWithValue("@PaymentMethod", paymentMethod);
            command.Parameters.AddWithValue("@OrderStatus", "Pending");
            command.Parameters.AddWithValue("@DeliveryAddress", deliveryAddress);

            try
            {
                command.ExecuteNonQuery();
                MessageBox.Query("Success", "Order placed successfully.", "OK");
            }
            catch (Exception ex)
            {
                MessageBox.ErrorQuery("Error", ex.Message, "OK");
            }
        }
    }

    }

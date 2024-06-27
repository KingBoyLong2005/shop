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
    public static Cart cart= new Cart();
    public static Users user = new Users();
    public static Program program = new Program();
    public static Customers customer = new Customers();
    public static Admin admin = new Admin();
    public static SuperAdmin superadmin = new SuperAdmin();
    public static int currentCustomerID = SessionData.Instance.CurrentCustomerID;
    public static List<Products> ListProducts = new List<Products>();
    public static List<Customers> ListCustomers = new List<Customers>();

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
                cus.CustomerDateOfBirth = read.GetDateTime("customer_dateofbirth");
                cus.CustomerCount = read.GetInt32("customer_count");
                cus.CustomerTotalSpent = read.GetDecimal("customer_totalspent");

                ListCustomers.Add(cus);
            }
        }
        return ListCustomers;
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

        var btnClose = new Button("Close")
        {
            X = Pos.Center(),
            Y = Pos.Percent(100) - 1
        };
        btnClose.Clicked += () =>
        {
            top.Remove(DisplayProductToOrderWin);
            customer.UserMenu();
        };

        DisplayProductToOrderWin.Add(btnClose);
    }

    
    public void DisplayMyOrder()
    {
        Application.Init();

        // Tạo cửa sổ chính để hiển thị đơn hàng
        var top = Application.Top;
        var myOrderWindow = new Window("My Orders")
        {
            X = 0,
            Y = 0,
            Width = Dim.Fill(),
            Height = Dim.Fill()
        };
        top.Add(myOrderWindow);
        int yPosition = 1;

        // Truy vấn cơ sở dữ liệu để lấy các đơn hàng của người dùng hiện tại
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            
            string query = @"SELECT 
                                o.order_quantity,
                                o.order_date,
                                o.order_delivery_date,
                                o.order_payment_method,
                                o.order_status,
                                o.order_total_price,
                                p.product_name
                            FROM orders o
                            JOIN products p ON o.order_product_id = p.product_id
                            WHERE o.order_customer_id = @CustomerID";
            MySqlCommand command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@CustomerID", currentCustomerID);
            connection.Open();
            MySqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                string orderDetails =$"Product: {reader["product_name"]}, " +
                                    $"Quantity: {reader["order_quantity"]}, " +
                                    $"Order Date: {reader["order_date"]}, " +
                                    $"Delivery Date: {reader["order_delivery_date"]}, " +
                                    $"Payment: {reader["order_payment_method"]}, " +
                                    $"Status: {reader["order_status"]}, " +
                                    $"Total: {reader["order_total_price"]}";
                
                var orderLabel = new Label(orderDetails)
                {
                    X = 1,
                    Y = yPosition,
                    Width = Dim.Fill()
                };
                myOrderWindow.Add(orderLabel);
                yPosition += 2;
            }
        }

        var CloseButton = new Button("Close")
        {
            X = Pos.Center(),
            Y = yPosition + 1,
        };
        CloseButton.Clicked += () =>
        {
            top.Remove(myOrderWindow);
            customer.UserMenu(); // Quay lại menu chính
        };
        myOrderWindow.Add(CloseButton);

    }
    

    public void OrderProduct(int productID, string productName, decimal productPrice)
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
                cart.RemoveItemFromCart(productID);
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
            bool confirmed = MessageBox.Query("Confirm", "Are you sure you want to close?", "Yes", "No") == 0;
            if (confirmed)
            {
                top.Remove(orderWindow);
                order.DisplayProductToOrder();
            }
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
    public void DisplayProductToOrderForCustomer()
    {
        List<Products> products = new List<Products>();
        var top = Application.Top;

        var DisplayProductToOrderForCustomerWin = new Window("Product List")
        {
            X = 0,
            Y = 0,
            Width = Dim.Fill(),
            Height = Dim.Fill()
        };
        top.Add(DisplayProductToOrderForCustomerWin);
        DisplayProductToOrderForCustomerWin.FocusNext();

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
                DisplayProductToOrderForCustomerWin.Add(new Label(columnDisplayListProductToOrder[i])
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
                    top.Remove(DisplayProductToOrderForCustomerWin);
                    OrderProductForCustomer(productID, productName, productPrice);
                };

                DisplayProductToOrderForCustomerWin.Add(productLabel, stockQuantityLabel, descriptionLabel, priceLabel, categoryLabel, brandLabel, orderButton);
                row++;
            }
        }

        var btnClose = new Button("Close")
        {
            X = Pos.Center(),
            Y = Pos.Percent(100) - 1
        };
        btnClose.Clicked += () =>
        {
            top.Remove(DisplayProductToOrderForCustomerWin);
            admin.AdminMenu();
        };

        DisplayProductToOrderForCustomerWin.Add(btnClose);
    }
    static void OrderProductForCustomer(int productID, string productName, decimal productPrice)
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

        var lblCustomerID = new Label("Customer ID:")
        {
            X = 1,
            Y = 5
        };
        var txtCustomerID = new TextField("")
        {
            X = Pos.Right(lblCustomerID) + 1,
            Y = 5,
            Width = 20
        };
        orderWindow.Add(lblCustomerID, txtCustomerID);

        var lblQuantity = new Label("Quantity:")
        {
            X = 1,
            Y = 7
        };
        var txtQuantity = new TextField("")
        {
            X = Pos.Right(lblQuantity) + 1,
            Y = 7,
            Width = 20
        };
        orderWindow.Add(lblQuantity, txtQuantity);
        lblQuantity.Visible = true;
        txtQuantity.Visible = true;

        var lblDeliveryAddress = new Label("Delivery Address:")
        {
            X = 1,
            Y = 9
        };
        var txtDeliveryAddress = new TextField("")
        {
            X = Pos.Right(lblDeliveryAddress) + 1,
            Y = 9,
            Width = 40
        };
        orderWindow.Add(lblDeliveryAddress, txtDeliveryAddress);
        lblDeliveryAddress.Visible = true;
        txtDeliveryAddress.Visible = true;

        var lblPaymentMethod = new Label("Payment Method:")
        {
            X = 1,
            Y = 11
        };
        var txtPaymentMethod = new TextField("")
        {
            X = Pos.Right(lblPaymentMethod) + 1,
            Y = 11,
            Width = 20
        };
        lblPaymentMethod.Visible = true;
        txtPaymentMethod.Visible = true;
        orderWindow.Add(lblPaymentMethod, txtPaymentMethod);
        var ConfirmCustomer = new Button("Comfirm")
        {
            X = Pos.Right(txtCustomerID) + 1,
            Y = 5
        };
        ConfirmCustomer.Clicked += () =>
        {
            int customerID;
            if (int.TryParse(txtCustomerID.Text.ToString(), out customerID))
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT * FROM customers WHERE customer_id = @CustomerID";
                    MySqlCommand command = new MySqlCommand(query, connection);
                    command.Parameters.AddWithValue("@CustomerID", customerID);
                    int customerExists = Convert.ToInt32(command.ExecuteScalar());
                    if (customerExists != null)
                    {
                        lblQuantity.Visible = false;
                        txtQuantity.Visible = false;
                        lblDeliveryAddress.Visible = false;
                        txtDeliveryAddress.Visible = false;
                        lblPaymentMethod.Visible = false;
                        txtPaymentMethod.Visible = false;
                    }
                    else
                    {
                        MessageBox.ErrorQuery("Error", "Customer ID does not exist.", "OK");
                    }
                }
            }
            
            else
            {
                MessageBox.ErrorQuery("Error", "Invalid Customer ID.", "OK");
            }

        };

        var btnSubmitOrder = new Button("Submit Order")
        {
            X = 1,
            Y = 14
        };
        btnSubmitOrder.Clicked += () =>
        {
            int quantity;
            if (int.TryParse(txtQuantity.Text.ToString(), out quantity) && quantity > 0)
            {
                PlaceOrderForCustomer(int.Parse(txtCustomerID.Text.ToString()), productID, quantity, txtDeliveryAddress.Text.ToString(), txtPaymentMethod.Text.ToString());
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
            Y = 14
        };
        btnCancel.Clicked += () =>
        {
            bool confirmed = MessageBox.Query("Confirm", "Are you sure you want to close?", "Yes", "No") == 0;
            if (confirmed)
            {
                top.Remove(orderWindow);
                order.DisplayProductToOrder();
            }
        };

        orderWindow.Add(ConfirmCustomer, btnSubmitOrder, btnCancel);
    }
    static void PlaceOrderForCustomer(int customerID, int productID, int quantity, string deliveryAddress, string paymentMethod)
    {
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            connection.Open();
            string query = @"INSERT INTO orders (order_customer_id, order_product_id, order_quantity, order_date, order_delivery_date, order_payment_method, order_status, order_delivery_address) 
                            VALUES (@CustomerID, @ProductID, @Quantity, @OrderDate, @DeliveryDate, @PaymentMethod, @OrderStatus, @DeliveryAddress);

                            UPDATE `orders` o JOIN `products` p ON o.order_product_id = p.product_id SET o.order_total_price = o.order_quantity * p.product_price;";
            MySqlCommand command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@CustomerID", customerID);
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

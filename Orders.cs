using System;
using System.Text;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;                                   //Import namesapce to use function
using System.Security.Cryptography.X509Certificates;
using Terminal.Gui;
public class Orders
{
    public int OrderID { get; set; }
    public int OrderCustomerID { get; set; }
    public Decimal OrderTotalPrice { get; set; }
    public string OrderPaymentMethod { get; set; }
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
    public static Categories cate = new Categories();
    
    public static List<Products> ListProducts = new List<Products>();
    public static List<Customers> ListCustomers = new List<Customers>();

    static List<Customers> LoadCustomers(string connectionString)
    {
        // Initialize a list to store customers
        List<Customers> ListCustomers = new List<Customers>();

        // Establish connection to the database using MySqlConnection
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {   
            // SQL query to select all customers
            string query = "SELECT * FROM customers"; 
            
            // MySqlCommand to execute the query using the connection
            MySqlCommand command = new MySqlCommand(query, connection);
            
            // Open the database connection
            connection.Open();
            
            // Execute the query and retrieve data using ExecuteReader
            MySqlDataReader read = command.ExecuteReader();
            
            // Iterate through the results
            while (read.Read())
            {
                // Create a new Customers object to hold current customer data
                Customers cus = new Customers();
                
                // Populate Customers object properties from database fields
                cus.CustomerID = read.GetInt32("customer_id");
                cus.CustomerName = read.GetString("customer_name");
                cus.CustomerPhone = read.GetString("customer_phone_number");
                cus.CustomerAddress = read.GetString("customer_address");
                cus.CustomerEmail = read.GetString("customer_email");
                cus.CustomerGender = read.GetString("customer_gender");
                cus.CustomerDateOfBirth = read.GetDateTime("customer_dateofbirth");
                cus.CustomerCount = read.GetInt32("customer_count");
                cus.CustomerTotalSpent = read.GetDecimal("customer_totalspent");

                // Add the populated Customers object to the ListCustomers list
                ListCustomers.Add(cus);
            }
        }
        
        // Return the populated list of Customers objects
        return ListCustomers;
    }

    static List<Products> LoadProducts(string connectionString)
    {
        // Initialize a list to store products
        List<Products> ListProduct = new List<Products>();

        // Establish connection to the database using MySqlConnection
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {   
            // SQL query to select all products
            string query = "SELECT * FROM products"; 
            
            // MySqlCommand to execute the query using the connection
            MySqlCommand command = new MySqlCommand(query, connection);
            
            // Open the database connection
            connection.Open();
            
            // Execute the query and retrieve data using ExecuteReader
            MySqlDataReader read = command.ExecuteReader();
            
            // Iterate through the results
            while (read.Read())
            {
                // Create a new Products object to hold current product data
                Products pd = new Products();
                
                // Populate Products object properties from database fields
                pd.ProductID = read.GetInt32("product_id");
                pd.ProductName = read.GetString("product_name");
                pd.ProductPrice = read.GetDecimal("product_price");
                pd.ProductStockQuantity = read.GetInt32("product_stock_quantity");
                pd.ProductBrand = read.GetString("product_brand");
                pd.ProductCategoryID = read.GetInt32("product_category_id");
                
                // Add the populated Products object to the ListProduct list
                ListProduct.Add(pd);
            }
        }
        
        // Return the populated list of Products objects
        return ListProduct;
    }


    public void DisplayMyOrder()
    {
        var top = Application.Top; // Get the top-level window

        var myOrderWindow = new Window("My Orders")
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

        myOrderWindow.Add(scrollView);
        top.Add(myOrderWindow); // Add the main window to the application

        int row = 0;
        int col = 0;
        int colCount = 2; // Số cột trong ScrollView
        int colWidth = 40; // Chiều rộng của mỗi cửa sổ sản phẩm
        int rowHeight = 12; // Chiều cao của mỗi cửa sổ sản phẩm
        int margin = 2; // Lề giữa các sản phẩm

        // Connect to the database and retrieve orders for the current customer
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            string query = @"SELECT 
                                o.order_quantity,
                                o.order_payment_method,
                                o.order_status,
                                o.order_total_price,
                                p.product_name
                            FROM orders o
                            JOIN products p ON o.order_product_id = p.product_id
                            WHERE o.order_customer_id = @CustomerID";
            MySqlCommand command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@CustomerID", SessionData.Instance.CurrentCustomerID); // Bind current customer ID parameter
            connection.Open();
            MySqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                // Construct the order details string
                string productName = reader["product_name"].ToString();
                int productQuantity = reader.GetInt32("order_quantity");
                string payment = reader["order_payment_method"].ToString();
                string status = reader["order_status"].ToString();
                decimal totalPrice = reader.GetDecimal("order_total_price");

                var orderWindow = new Window($"Order {row * colCount + col + 1}")
                {
                    X = col * (colWidth + margin),
                    Y = row * (rowHeight + margin),
                    Width = colWidth,
                    Height = rowHeight
                };

                // Create labels for displaying order details
                var productNameLabel = new Label($"Product Name: {productName}")
                {
                    X = 1,
                    Y = 1,
                    Width = Dim.Fill(),
                    TextAlignment = TextAlignment.Left // Left text alignment
                };
                var productQuantityLabel = new Label($"Quantity: {productQuantity}")
                {
                    X = 1,
                    Y = Pos.Bottom(productNameLabel) + 1,
                    Width = Dim.Fill(),
                    TextAlignment = TextAlignment.Left // Left text alignment
                };
                var productPaymentLabel = new Label($"Payment: {payment}")
                {
                    X = 1,
                    Y = Pos.Bottom(productQuantityLabel) + 1,
                    Width = Dim.Fill(),
                    TextAlignment = TextAlignment.Left // Left text alignment
                };
                var productStatusLabel = new Label($"Status: {status}")
                {
                    X = 1,
                    Y = Pos.Bottom(productPaymentLabel) + 1,
                    Width = Dim.Fill(),
                    TextAlignment = TextAlignment.Left // Left text alignment
                };
                var productTotalPriceLabel = new Label($"Total Price: {totalPrice:C}")
                {
                    X = 1,
                    Y = Pos.Bottom(productStatusLabel) + 1,
                    Width = Dim.Fill(),
                    TextAlignment = TextAlignment.Left // Left text alignment
                };

                // Add labels to the order window
                orderWindow.Add(productNameLabel, productQuantityLabel, productPaymentLabel, productStatusLabel, productTotalPriceLabel);
                scrollView.Add(orderWindow);

                col++;
                if (col >= colCount)
                {
                    col = 0;
                    row++;
                }
            }
        }

        // Cập nhật kích thước nội dung của ScrollView
        scrollView.ContentSize = new Size((colWidth + margin) * colCount, (row + 1) * (rowHeight + margin));

        // Create a Close button to exit the order display window
        var closeButton = new Button("Close")
        {
            X = Pos.Center(),
            Y = Pos.Bottom(scrollView) - 1
        };
        closeButton.Clicked += () =>
        {
            top.Remove(myOrderWindow); // Remove the order window from the top application
            customer.UserMenu(); // Return to the main menu
        };

        myOrderWindow.Add(closeButton); // Add the Close button to the order window
    }

    public void OrderProduct(int productID, string productName, decimal productPrice, string window)
    {
        var top = Application.Top;
        Application.Init();
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
            X = Pos.Center(),
            Y = Pos.Percent(10)
        };
        orderWindow.Add(lblProductName);

        var lblQuantity = new Label("Quantity:")
        {
            X = Pos.Center() - 15,
            Y = Pos.Percent(25)
        };
        var txtQuantity = new TextField("")
        {
            X = Pos.Center() + 2,
            Y = Pos.Percent(25),
            Width = 20
        };
        orderWindow.Add(lblQuantity, txtQuantity);

        var lblDeliveryAddress = new Label("Delivery Address:")
        {
            X = Pos.Center() - 17,
            Y = Pos.Percent(40)
        };
        var txtDeliveryAddress = new TextField("")
        {
            X = Pos.Center() + 2,
            Y = Pos.Percent(40),
            Width = 40
        };
        orderWindow.Add(lblDeliveryAddress, txtDeliveryAddress);

        var lblPaymentMethod = new Label("Payment Method:")
        {
            X = Pos.Center() - 15,
            Y = Pos.Percent(55)
        };
        var txtPaymentMethod = new TextField("")
        {
            X = Pos.Center() + 2,
            Y = Pos.Percent(55),
            Width = 20
        };
        orderWindow.Add(lblPaymentMethod, txtPaymentMethod);

        var btnSubmitOrder = new Button("Submit Order")
        {
            X = Pos.Center() - 10,
            Y = Pos.Percent(70)
        };
        btnSubmitOrder.Clicked += () =>
        {
            if (string.IsNullOrWhiteSpace(txtQuantity.Text.ToString()) ||
                string.IsNullOrWhiteSpace(txtDeliveryAddress.Text.ToString()) ||
                string.IsNullOrWhiteSpace(txtPaymentMethod.Text.ToString()))
            {
                MessageBox.ErrorQuery("Error", "All fields must be filled.", "OK");
                return;
            }

            if (int.TryParse(txtQuantity.Text.ToString(), out int quantity) && quantity > 0)
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT product_stock_quantity FROM products WHERE product_id = @productID";
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@productID", productID);
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                int stockQuantity = reader.GetInt32("product_stock_quantity");
                                if (quantity <= stockQuantity)
                                {
                                    cart.RemoveItemFromCart(productID);
                                    PlaceOrderDirectly(productID, quantity, txtDeliveryAddress.Text.ToString(), txtPaymentMethod.Text.ToString());
                                    top.Remove(orderWindow);
                                    switch (window)
                                    {
                                        case "display":
                                            pd.DisplayProduct("user");
                                            break;
                                        case "cart":
                                            cart.DisplayCart();
                                            break;
                                        case "category":
                                            cate.DisplayCategories("superadmins");
                                            break;
                                    }
                                }
                                else
                                {
                                    MessageBox.ErrorQuery("Error", "Not enough product in stock.", "OK");
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                MessageBox.ErrorQuery("Error", "Invalid quantity.", "OK");
            }
        };

        var btnCancel = new Button("Cancel")
        {
            X = Pos.Right(btnSubmitOrder) + 2,
            Y = Pos.Percent(70)
        };
        btnCancel.Clicked += () =>
        {
            bool confirmed = MessageBox.Query("Confirm", "Are you sure you want to close?", "Yes", "No") == 0;
            if (confirmed)
            {
                top.Remove(orderWindow);
                switch (window)
                {
                    case "display":
                        pd.DisplayProduct("user");
                        break;
                    case "cart":
                        cart.DisplayCart();
                        break;
                    case "category":
                        cate.DisplayCategories("superadmins");
                        break;
                }
            }
        };

        orderWindow.Add(btnSubmitOrder, btnCancel);
    }
    static void PlaceOrderDirectly(int productID, int quantity, string deliveryAddress, string paymentMethod)
    {
        try
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                
                // SQL query to insert a new order and update the order total price
                string query = @"
                    INSERT INTO orders 
                        (order_customer_id, order_product_id, order_quantity, order_payment_method, order_status, order_delivery_address) 
                    VALUES 
                        (@CustomerID, @ProductID, @Quantity, @PaymentMethod, @OrderStatus, @DeliveryAddress);

                    UPDATE orders o 
                    JOIN products p ON o.order_product_id = p.product_id 
                    SET 
                        o.order_total_price = o.order_quantity * p.product_price
                    WHERE 
                        o.order_product_id = @ProductID AND o.order_customer_id = @CustomerID AND o.order_quantity = @Quantity;";

                MySqlCommand command = new MySqlCommand(query, connection);

                // Parameters for the INSERT INTO orders statement
                command.Parameters.AddWithValue("@CustomerID", SessionData.Instance.CurrentCustomerID);
                command.Parameters.AddWithValue("@ProductID", productID);
                command.Parameters.AddWithValue("@Quantity", quantity);
                command.Parameters.AddWithValue("@PaymentMethod", paymentMethod);
                command.Parameters.AddWithValue("@OrderStatus", "Pending");
                command.Parameters.AddWithValue("@DeliveryAddress", deliveryAddress);

                // Execute the INSERT INTO and UPDATE queries
                command.ExecuteNonQuery();
                MessageBox.Query("Success", "Order placed successfully.", "OK");
            }
        }
        catch (MySqlException)
        {
            // Handle specific SQL exceptions
            MessageBox.ErrorQuery("Database Error", "An error occurred while accessing the database. Please try again later.", "OK");
        }
        catch (Exception)
        {
            // Handle all other exceptions
            MessageBox.ErrorQuery("Error", "An unexpected error occurred while placing the order. Please try again later.", "OK");
        }
    }

   public void OrderProductForCustomer(int productID, string productName, decimal productPrice, string role)
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
            X = Pos.Center(),
            Y = 1
        };
        orderWindow.Add(lblProductName);

        var lblCustomerID = new Label("Customer ID:")
        {
            X = Pos.Center() - 10,
            Y = 5
        };
        var txtCustomerID = new TextField("")
        {
            X = Pos.Center() + 5,
            Y = 5,
            Width = 20
        };
        orderWindow.Add(lblCustomerID, txtCustomerID);

        var lblQuantity = new Label("Quantity:")
        {
            X = Pos.Center() - 10,
            Y = 7
        };
        var txtQuantity = new TextField("")
        {
            X = Pos.Center() + 5,
            Y = 7,
            Width = 20
        };
        orderWindow.Add(lblQuantity, txtQuantity);

        var lblDeliveryAddress = new Label("Delivery Address:")
        {
            X = Pos.Center() - 14,
            Y = 9
        };
        var txtDeliveryAddress = new TextField("")
        {
            X = Pos.Center() + 7,
            Y = 9,
            Width = 40
        };
        orderWindow.Add(lblDeliveryAddress, txtDeliveryAddress);

        var lblPaymentMethod = new Label("Payment Method:")
        {
            X = Pos.Center() - 14,
            Y = 11
        };
        var txtPaymentMethod = new TextField("")
        {
            X = Pos.Center() + 7,
            Y = 11,
            Width = 20
        };
        orderWindow.Add(lblPaymentMethod, txtPaymentMethod);

        var ConfirmCustomer = new Button("Confirm")
        {
            X = Pos.Center() + 15,
            Y = 5
        };
        ConfirmCustomer.Clicked += () =>
        {
            if (string.IsNullOrWhiteSpace(txtCustomerID.Text.ToString()))
            {
                MessageBox.ErrorQuery("Error", "Customer ID cannot be empty.", "OK");
                return;
            }

            if (int.TryParse(txtCustomerID.Text.ToString(), out int customerID))
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT * FROM customers WHERE customer_id = @CustomerID";
                    MySqlCommand command = new MySqlCommand(query, connection);
                    command.Parameters.AddWithValue("@CustomerID", customerID);
                    object customerExists = command.ExecuteScalar();
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
            X = Pos.Center() - 10,
            Y = 14
        };
        btnSubmitOrder.Clicked += () =>
        {
            if (string.IsNullOrWhiteSpace(txtQuantity.Text.ToString()) ||
                string.IsNullOrWhiteSpace(txtDeliveryAddress.Text.ToString()) ||
                string.IsNullOrWhiteSpace(txtPaymentMethod.Text.ToString()))
            {
                MessageBox.ErrorQuery("Error", "All fields must be filled.", "OK");
                return;
            }

            if (int.TryParse(txtQuantity.Text.ToString(), out int quantity) && quantity > 0)
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT product_stock_quantity FROM products WHERE product_id = @productID";
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@productID", productID);
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                int stockQuantity = reader.GetInt32("product_stock_quantity");
                                if (quantity <= stockQuantity)
                                {
                                    cart.RemoveItemFromCart(productID);
                                    PlaceOrderDirectly(productID, quantity, txtDeliveryAddress.Text.ToString(), txtPaymentMethod.Text.ToString());
                                    top.Remove(orderWindow);
                                    switch (role)
                                    {
                                        case "user":
                                            pd.DisplayProduct("user");
                                            break;
                                        case "superadmins":
                                            cate.DisplayCategories("superadmins");
                                            break;
                                    }
                                }
                                else
                                {
                                    MessageBox.ErrorQuery("Error", "Not enough product in stock.", "OK");
                                }
                            }
                        }
                    }
                }
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
                switch (role)
                {
                    case "user":
                        pd.DisplayProduct("user");
                        break;
                    case "superadmins":
                        cate.DisplayCategories("superadmins");
                        break;
                }
            }
        };

        orderWindow.Add(btnSubmitOrder, btnCancel, ConfirmCustomer);
    }

    static void PlaceOrderForCustomer(int customerID, int productID, int quantity, string deliveryAddress, string paymentMethod)
    {
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            connection.Open();
            
            // Insert query to add the order details
            string insertQuery = @"
                INSERT INTO orders 
                    (order_customer_id, order_product_id, order_quantity, order_payment_method, order_status, order_delivery_address) 
                VALUES 
                    (@CustomerID, @ProductID, @Quantity, @PaymentMethod, @OrderStatus, @DeliveryAddress);";

            MySqlCommand insertCommand = new MySqlCommand(insertQuery, connection);
            insertCommand.Parameters.AddWithValue("@CustomerID", customerID);
            insertCommand.Parameters.AddWithValue("@ProductID", productID);
            insertCommand.Parameters.AddWithValue("@Quantity", quantity);
            insertCommand.Parameters.AddWithValue("@PaymentMethod", paymentMethod);
            insertCommand.Parameters.AddWithValue("@OrderStatus", "Pending");
            insertCommand.Parameters.AddWithValue("@DeliveryAddress", deliveryAddress);

            try
            {
                // Execute the INSERT command
                insertCommand.ExecuteNonQuery();

                // Update query to calculate and update the order total price based on product price
                string updateQuery = @"
                    UPDATE orders o 
                    JOIN products p ON o.order_product_id = p.product_id 
                    SET o.order_total_price = o.order_quantity * p.product_price
                    WHERE o.order_customer_id = @CustomerID AND o.order_product_id = @ProductID AND o.order_quantity = @Quantity;";

                MySqlCommand updateCommand = new MySqlCommand(updateQuery, connection);
                updateCommand.Parameters.AddWithValue("@CustomerID", customerID);
                updateCommand.Parameters.AddWithValue("@ProductID", productID);
                updateCommand.Parameters.AddWithValue("@Quantity", quantity);

                // Execute the UPDATE command
                updateCommand.ExecuteNonQuery();

                MessageBox.Query("Success", "Order placed successfully.", "OK");
            }
            catch (MySqlException)
            {
                // Handle specific SQL exceptions
                MessageBox.ErrorQuery("Database Error", "An error occurred while accessing the database. Please try again later.", "OK");
            }
            catch (Exception)
            {
                // Handle all other exceptions
                MessageBox.ErrorQuery("Error", "An unexpected error occurred while placing the order. Please try again later.", "OK");
            }
        }
    }
    public void UpdateStatus()
    {
        Application.Init();
        var top = Application.Top;

        var updatestatus = new Window("Order Status Updater")
        {
            X = 0,
            Y = 1,
            Width = Dim.Fill(),
            Height = Dim.Fill() - 1
        };
        top.Add(updatestatus);

        var menu = new MenuBar(new MenuBarItem[]
        {
            new MenuBarItem("_File", new MenuItem[]
            {
                new MenuItem("_Quit", "", () => { if (Quit()) top.Running = false; })
            })
        });
        top.Add(menu);

        var lblOrderId = new Label("Order ID:")
        {
            X = 2,
            Y = 2
        };
        var txtOrderId = new TextField("")
        {
            X = 11,
            Y = 2,
            Width = 40
        };
        var btnUpdateStatus = new Button("Update Status")
        {
            X = 2,
            Y = Pos.Bottom(lblOrderId) + 2
        };
        var btnClose = new Button("Close")
        {
            X = Pos.Center(),
            Y = Pos.Percent(100) + 1
        };
        btnClose.Clicked += () =>
        {
            bool confirmed = MessageBox.Query("Confirm", "Are you sure you want to close?", "Yes", "No") == 0;
            if (confirmed)
            {
                top.Remove(updatestatus);
                admin.AdminMenu();
            }
        };

        btnUpdateStatus.Clicked += () => UpdateOrderStatus(txtOrderId.Text.ToString());

        updatestatus.Add(lblOrderId, txtOrderId, btnUpdateStatus, btnClose);
    }
    private static bool Quit()
    {
        return MessageBox.Query(50, 7, "Quit", "Are you sure you want to quit?", "Yes", "No") == 0;
    }

    private static void UpdateOrderStatus(string orderIdText)
    {
        int orderId;
        if (!int.TryParse(orderIdText, out orderId))
        {
            MessageBox.ErrorQuery("Error", "Invalid Order ID.", "OK");
            return;
        }

        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            connection.Open();

            string selectQuery = "SELECT * FROM orders WHERE order_id = @OrderID";
            MySqlCommand selectCommand = new MySqlCommand(selectQuery, connection);
            selectCommand.Parameters.AddWithValue("@OrderID", orderId);

            using (MySqlDataReader reader = selectCommand.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    reader.Close();
                    string updateQuery = "UPDATE orders SET order_status = 'Successful delivery' WHERE order_id = @OrderID";
                    MySqlCommand updateCommand = new MySqlCommand(updateQuery, connection);
                    updateCommand.Parameters.AddWithValue("@OrderID", orderId);

                    try
                    {
                        updateCommand.ExecuteNonQuery();
                        MessageBox.Query("Success", "Order status updated to 'Successful delivery'.", "OK");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.ErrorQuery("Error", "An error occurred while updating the order status. Please try again later.", "OK");
                    }
                }
                else
                {
                    MessageBox.ErrorQuery("Error", "Order not found.", "OK");
                }
            }
        }
    }
}

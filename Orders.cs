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
    public DateTime OrderDate { get; set; }
    public DateTime OrderDeliveryDate { get; set; }
    public string OrderPaymentMethod { get; set; }
    public DateTime OrderPaymentDate { get; set; }
    public string OrderStatus { get; set; }
    public string OrderAddress { get; set; }

    
    public static string connectionString = Configuration.ConnectionString;
    public static int currentCustomerID = SessionData.Instance.CurrentCustomerID;

    public static Products pd = new Products();
    public static Orders order = new Orders();
    public static Cart cart= new Cart();
    public static Users user = new Users();
    public static Program program = new Program();
    public static Customers customer = new Customers();
    public static Admin admin = new Admin();
    public static SuperAdmin superadmin = new SuperAdmin();
    
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

    public void DisplayProductToOrder()
    {
        List<Products> products = new List<Products>(); // Initialize a list to hold products
        var top = Application.Top;

        // Create a new window for displaying products
        var DisplayProductToOrderWin = new Window("Product List")
        {
            X = 0,
            Y = 0,
            Width = Dim.Fill(),
            Height = Dim.Fill()
        };
        top.Add(DisplayProductToOrderWin); // Add window to the top application
        DisplayProductToOrderWin.FocusNext(); // Focus on the window

        // Connect to the database and retrieve product information
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

            // Define column headers for product display
            var columnDisplayListProductToOrder = new string[]
            {
                "Product Name", "Stock Quantity", "Description", "Price", "Category", "Brand"
            };

            int columnWidth = 20; // Width for each column

            // Display column headers in the window
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

            int row = 1; // Starting row for displaying products

            // Read through each product retrieved from the database
            while (reader.Read())
            {
                int productID = reader.GetInt32("product_id");
                string productName = reader["product_name"].ToString();
                decimal productPrice = reader.GetDecimal("product_price");

                // Create labels for each product attribute to display
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
                
                // Button to order the product
                var orderButton = new Button("Order")
                {
                    X = 6 * columnWidth,
                    Y = row
                };
                orderButton.Clicked += () =>
                {
                    try
                    {
                        // Remove the product display window and initiate the order process
                        top.Remove(DisplayProductToOrderWin);
                        OrderProduct(productID, productName, productPrice);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.ErrorQuery("Error", ex.Message, "OK");
                    }
                };

                // Add labels and order button to the product display window
                DisplayProductToOrderWin.Add(productLabel, stockQuantityLabel, descriptionLabel, 
                                            priceLabel, categoryLabel, brandLabel, orderButton);
                
                row++; // Move to the next row for the next product
            }
        }

        // Close button to exit the product display window
        var btnClose = new Button("Close")
        {
            X = Pos.Center(),
            Y = Pos.Percent(100) - 1
        };
        btnClose.Clicked += () =>
        {
            top.Remove(DisplayProductToOrderWin); // Remove the window from the top application
            customer.UserMenu(); // Return to the user menu
        };

        // Add close button to the product display window
        DisplayProductToOrderWin.Add(btnClose);
    }

    public void DisplayMyOrder()
    {
        Application.Init(); // Initialize the Terminal.Gui application

        var top = Application.Top; // Get the top-level window
        var myOrderWindow = new Window("My Orders")
        {
            X = 0,
            Y = 0,
            Width = Dim.Fill(),
            Height = Dim.Fill()
        };
        top.Add(myOrderWindow); // Add the main window to the application
        int yPosition = 1; // Starting Y position for displaying orders

        // Connect to the database and retrieve orders for the current customer
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
            command.Parameters.AddWithValue("@CustomerID", currentCustomerID); // Bind current customer ID parameter
            connection.Open();
            MySqlDataReader reader = command.ExecuteReader();

            // Read through each order retrieved from the database
            while (reader.Read())
            {
                // Construct the order details string
                string orderDetails = $"Product: {reader["product_name"]}, " +
                                    $"Quantity: {reader["order_quantity"]}, " +
                                    $"Order Date: {reader["order_date"]}, " +
                                    $"Delivery Date: {reader["order_delivery_date"]}, " +
                                    $"Payment: {reader["order_payment_method"]}, " +
                                    $"Status: {reader["order_status"]}, " +
                                    $"Total: {reader["order_total_price"]}";

                // Create a label for displaying order details
                var orderLabel = new Label(orderDetails)
                {
                    X = 1,
                    Y = yPosition,
                    Width = Dim.Fill()
                };
                myOrderWindow.Add(orderLabel); // Add the label to the order window
                yPosition += 2; // Increment Y position for the next order
            }
        }

        // Create a Close button to exit the order display window
        var closeButton = new Button("Close")
        {
            X = Pos.Center(),
            Y = yPosition + 1, // Position below the last order display
        };
        closeButton.Clicked += () =>
        {
            top.Remove(myOrderWindow); // Remove the order window from the top application
            customer.UserMenu(); // Return to the main menu
        };
        myOrderWindow.Add(closeButton); // Add the Close button to the order window
    }

    public void OrderProduct(int productID, string productName, decimal productPrice)
    {
        var top = Application.Top; // Get the top-level application window
        var orderWindow = new Window("Order Product")
        {
            X = 0,
            Y = 0,
            Width = Dim.Fill(),
            Height = Dim.Fill()
        };
        top.Add(orderWindow); // Add the order window to the top-level application

        // Label to display the selected product's name
        var lblProductName = new Label($"Product: {productName}")
        {
            X = 1,
            Y = 1
        };
        orderWindow.Add(lblProductName); // Add the product name label to the order window

        // Label and text field for entering the quantity to order
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
        orderWindow.Add(lblQuantity, txtQuantity); // Add quantity label and text field to the order window

        // Label and text field for entering the delivery address
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
        orderWindow.Add(lblDeliveryAddress, txtDeliveryAddress); // Add delivery address label and text field to the order window

        // Label and text field for selecting the payment method
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
        orderWindow.Add(lblPaymentMethod, txtPaymentMethod); // Add payment method label and text field to the order window

        // Button to submit the order
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
                // Remove the ordered item from the cart
                cart.RemoveItemFromCart(productID);

                // Place the order
                PlaceOrderDirectly(productID, quantity, txtDeliveryAddress.Text.ToString(), txtPaymentMethod.Text.ToString());

                // Close the order window and display the product list again
                top.Remove(orderWindow);
                DisplayProductToOrder();
            }
            else
            {
                MessageBox.ErrorQuery("Error", "Invalid quantity.", "OK");
            }
        };

        // Button to cancel the order
        var btnCancel = new Button("Cancel")
        {
            X = Pos.Right(btnSubmitOrder) + 2,
            Y = 10
        };
        btnCancel.Clicked += () =>
        {
            // Prompt for confirmation before closing the order window
            bool confirmed = MessageBox.Query("Confirm", "Are you sure you want to close?", "Yes", "No") == 0;
            if (confirmed)
            {
                // Close the order window and display the product list again
                top.Remove(orderWindow);
                DisplayProductToOrder();
            }
        };

        orderWindow.Add(btnSubmitOrder, btnCancel); // Add submit and cancel buttons to the order window
    }

    static void PlaceOrderDirectly(int productID, int quantity, string deliveryAddress, string paymentMethod)
    {
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            connection.Open();
            
            // SQL query to insert a new order and update the order total price
            string query = @"INSERT INTO orders 
                                (order_customer_id, order_product_id, order_quantity, order_date, order_delivery_date, order_payment_method, order_status, order_delivery_address) 
                            VALUES 
                                (@CustomerID, @ProductID, @Quantity, @OrderDate, @DeliveryDate, @PaymentMethod, @OrderStatus, @DeliveryAddress);

                            UPDATE `orders` o 
                            JOIN `products` p ON o.order_product_id = p.product_id 
                            SET 
                                o.order_total_price = o.order_quantity * p.product_price;";

            MySqlCommand command = new MySqlCommand(query, connection);

            // Parameters for the INSERT INTO orders statement
            command.Parameters.AddWithValue("@CustomerID", currentCustomerID);  // Using static currentCustomerID
            command.Parameters.AddWithValue("@ProductID", productID);
            command.Parameters.AddWithValue("@Quantity", quantity);
            command.Parameters.AddWithValue("@OrderDate", DateTime.Now);
            command.Parameters.AddWithValue("@DeliveryDate", DateTime.Now.AddDays(7)); // Assuming delivery time is 7 days from now
            command.Parameters.AddWithValue("@PaymentMethod", paymentMethod);
            command.Parameters.AddWithValue("@OrderStatus", "Pending");
            command.Parameters.AddWithValue("@DeliveryAddress", deliveryAddress);

            try
            {
                // Execute the INSERT INTO and UPDATE queries
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

        // Create a window to display product list
        var DisplayProductToOrderForCustomerWin = new Window("Product List")
        {
            X = 0,
            Y = 0,
            Width = Dim.Fill(),
            Height = Dim.Fill()
        };
        top.Add(DisplayProductToOrderForCustomerWin);
        DisplayProductToOrderForCustomerWin.FocusNext();

        // Connect to the database and retrieve product information
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

            // Define column headers
            var columnDisplayListProductToOrder = new string[]
            {
                "Product's name", "Stock quantity", "Description", "Price", "Category's name", "Brand"
            };

            int columnWidth = 20; // Column width

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
                // Read product details from the database
                int productID = reader.GetInt32("product_id");
                string productName = reader["product_name"].ToString();
                decimal productPrice = reader.GetDecimal("product_price");

                // Create labels for each product attribute
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

                // Button to order the product
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

                // Add labels and button to the window
                DisplayProductToOrderForCustomerWin.Add(productLabel, stockQuantityLabel, descriptionLabel, priceLabel, categoryLabel, brandLabel, orderButton);
                row++;
            }
        }

        // Close button to exit the product display window
        var btnClose = new Button("Close")
        {
            X = Pos.Center(),
            Y = Pos.Percent(100) - 1
        };
        btnClose.Clicked += () =>
        {
            top.Remove(DisplayProductToOrderForCustomerWin);
            admin.AdminMenu(); // Navigate back to admin menu (adjust as needed)
        };

        // Add close button to the window
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
        orderWindow.Add(lblPaymentMethod, txtPaymentMethod);

        var ConfirmCustomer = new Button("Confirm")
        {
            X = Pos.Right(txtCustomerID) + 1,
            Y = 5
        };
        ConfirmCustomer.Clicked += () =>
        {
            int customerID;
            if (int.TryParse(txtCustomerID.Text.ToString(), out customerID))
            {
                // Check if customer exists in the database
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT * FROM customers WHERE customer_id = @CustomerID";
                    MySqlCommand command = new MySqlCommand(query, connection);
                    command.Parameters.AddWithValue("@CustomerID", customerID);
                    object customerExists = command.ExecuteScalar();
                    if (customerExists != null)
                    {
                        // Hide unnecessary fields after customer confirmation
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

            // Insert query to add the order details
            string insertQuery = @"INSERT INTO orders 
                                    (order_customer_id, order_product_id, order_quantity, order_date, order_delivery_date, order_payment_method, order_status, order_delivery_address) 
                                VALUES 
                                    (@CustomerID, @ProductID, @Quantity, @OrderDate, @DeliveryDate, @PaymentMethod, @OrderStatus, @DeliveryAddress);";

            MySqlCommand insertCommand = new MySqlCommand(insertQuery, connection);
            insertCommand.Parameters.AddWithValue("@CustomerID", customerID);
            insertCommand.Parameters.AddWithValue("@ProductID", productID);
            insertCommand.Parameters.AddWithValue("@Quantity", quantity);
            insertCommand.Parameters.AddWithValue("@OrderDate", DateTime.Now);
            insertCommand.Parameters.AddWithValue("@DeliveryDate", DateTime.Now.AddDays(7)); // Assuming delivery time is 7 days
            insertCommand.Parameters.AddWithValue("@PaymentMethod", paymentMethod);
            insertCommand.Parameters.AddWithValue("@OrderStatus", "Pending");
            insertCommand.Parameters.AddWithValue("@DeliveryAddress", deliveryAddress);

            try
            {
                insertCommand.ExecuteNonQuery();

                // Update query to calculate and update the order total price based on product price
                string updateQuery = @"UPDATE orders o 
                                    JOIN products p ON o.order_product_id = p.product_id 
                                    SET o.order_total_price = o.order_quantity * p.product_price;";

                MySqlCommand updateCommand = new MySqlCommand(updateQuery, connection);
                updateCommand.ExecuteNonQuery();

                MessageBox.Query("Success", "Order placed successfully.", "OK");
            }
            catch (Exception ex)
            {
                MessageBox.ErrorQuery("Error", ex.Message, "OK");
            }
        }
    }
}

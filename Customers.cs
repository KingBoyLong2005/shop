using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;                      //Import namesapce to use function
using System.Threading.Tasks;
using MySql.Data.MySqlClient;  
using Terminal.Gui;
using System.Security.Cryptography.X509Certificates;

public class Customers
{
    // Define properties of the Customers class
    public int CustomerID { get; set; } // Unique identifier for the customer
    public string CustomerName { get; set; } // Name of the customer
    public string CustomerPhone { get; set; } // Phone number of the customer
    public string CustomerAddress { get; set; } // Address of the customer
    public string CustomerEmail { get; set; } // Email address of the customer
    public string CustomerGender { get; set; } // Gender of the customer
    public DateTime CustomerDateOfBirth { get; set; } // Date of birth of the customer
    public int CustomerCount { get; set; } // Number of purchases made by the customer
    public decimal CustomerTotalSpent { get; set; } // Total amount spent by the customer

    // Static instances of various classes
    public static Users user = new Users();
    public static Admin admin = new Admin();
    public static SuperAdmin superadmin = new SuperAdmin();
    public static Cart userCart = new Cart();
    public static Products pd = new Products();
    public static Orders order = new Orders();
    public static Program program = new Program();
    public static Customers cus = new Customers();
    public static Categories cate = new Categories();


    // Connection string for the database, retrieved from the configuration
    public static string connectionString = Configuration.ConnectionString;

    // Initialize lists to store users and customers
    public static List<Users> ListUsers = new List<Users>();
    public static List<Customers> ListCustomers = new List<Customers>();

    // Static method to load customers from the database
    static List<Customers> LoadCustomers(string connectionString)
    {
        List<Customers> ListCustomers = new List<Customers>();

        // Using statement ensures the database connection is properly closed after use
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {   
            // SQL query to select all customers from the database
            string query = "SELECT * FROM customers WHERE active = TRUE"; 
            MySqlCommand command = new MySqlCommand(query, connection);

            // Open the database connection
            connection.Open();

            // Execute the query and get a data reader
            MySqlDataReader read = command.ExecuteReader();

            // Read the data row by row
            while (read.Read())
            {
                Customers cus = new Customers();

                // Load properties of the Customers class from the database
                cus.CustomerID = read.GetInt32("customer_id");
                cus.CustomerName = read.GetString("customer_name");
                cus.CustomerPhone = read.GetString("customer_phone_number");
                cus.CustomerAddress = read.GetString("customer_address");
                cus.CustomerEmail = read.GetString("customer_email");
                cus.CustomerGender = read.GetString("customer_gender");
                cus.CustomerDateOfBirth = read.GetDateTime("customer_dateofbirth");
                cus.CustomerCount = read.GetInt32("customer_count");
                cus.CustomerTotalSpent = read.GetDecimal("customer_totalspent");

                // Add the customer to the list
                ListCustomers.Add(cus);
            }
        }
        // Return the list of customers
        return ListCustomers;
    }

    public void AddCustomer()
    {
        // Initialize user and customer objects
        Users us = new Users();
        Customers cus = new Customers();

        // Create the main window for the registration form
        var top = Application.Top;
        var registerWin = new Window()
        {
            Title = $"Register for customer",
            X = 0,
            Y = 0,
            Width = Dim.Fill(),
            Height = Dim.Fill()
        };
        top.Add(registerWin);

        // Define and position the input fields for user and customer details
            var usernameLabel = new Label("Username:")
            {
                X = 2,
                Y = 2
            };
            var usernameField = new TextField("")
            {
                X = 31,
                Y = 2,
                Width = 100
            };

            var passwordLabel = new Label("Password:")
            {
                X = 2,
                Y = 4
            };
            var passwordField = new TextField("")
            {
                Secret = true,
                X = 31,
                Y = 4,
                Width = 100
            };
            var CustomerNameLabel = new Label("Name: ")
            {
                X = 2,
                Y = 6
            };
            var CustomerNameField = new TextField("")
            {
                X = 31,
                Y  = 6,
                Width = 100
            };
            var CustomerPhoneNumberLabel = new Label("Phone number: ")
            {
                X = 2,
                Y = 8
            };
            var CustomerPhoneNumberField = new TextField("")
            {
                X = 31,
                Y  = 8,
                Width = 100
            };
            var CustomerAddressLabel = new Label("Address: ")
            {
                X = 2,
                Y = 10
            };
            var CustomerAddressField = new TextField("")
            {
                X = 31,
                Y  = 10,
                Width = 100
            };
            var CustomerEmailLabel = new Label("Email: ")
            {
                X = 2,
                Y = 12
            };
            var CustomerEmailField = new TextField("")
            {
                X = 31,
                Y  = 12,
                Width = 100
            };
            var CustomerGenderLabel = new Label("Gender: ")
            {
                X = 2,
                Y = 14
            };
            var CustomerGenderField = new TextField("")
            {
                X = 31,
                Y  = 14,
                Width = 100
            };
            var CustomerDateOfBirthLabel = new Label("Date of birth (YYYY-MM-DD): ")
            {
                X = 2,
                Y = 16
            };
            var CustomerDateOfBirthField = new TextField("")
            {
                X = 31,
                Y  = 16,
                Width = 100
            };

            var registerButton = new Button("Register")
            {
                X = Pos.Center(),
                Y = Pos.Bottom(CustomerDateOfBirthField),
            };

            // Define the click event for the "Register" button
            registerButton.Clicked += () =>
            {
                // Assign input values to user and customer objects
                us.Username = usernameField.Text.ToString();
                us.PasswordHash = passwordField.Text.ToString();
                cus.CustomerName = CustomerNameField.Text.ToString();
                cus.CustomerPhone = CustomerPhoneNumberField.Text.ToString();
                cus.CustomerAddress = CustomerAddressField.Text.ToString();
                cus.CustomerEmail = CustomerEmailField.Text.ToString();
                cus.CustomerGender = CustomerGenderField.Text.ToString();
                
                // Validate and assign the date of birth
                DateTime customerDateOfBirth;
                if (!DateTime.TryParse(CustomerDateOfBirthField.Text.ToString(), out customerDateOfBirth))
                {
                    MessageBox.ErrorQuery("Error", "Invalid date format. Please use YYYY-MM-DD.", "OK");
                    return;
                }
                cus.CustomerDateOfBirth = customerDateOfBirth;

                // Insert data into the database
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    MySqlTransaction transaction = connection.BeginTransaction();
                    try
                    {
                        // Check if the username already exists
                        string checkUserQuery = "SELECT COUNT(*) FROM users WHERE username = @Username";
                        MySqlCommand checkUserCommand = new MySqlCommand(checkUserQuery, connection, transaction);
                        checkUserCommand.Parameters.AddWithValue("@Username", us.Username);
                        int userCount = Convert.ToInt32(checkUserCommand.ExecuteScalar());

                        if (userCount > 0)
                        {
                            // Username is duplicated
                            MessageBox.ErrorQuery("Error", "Username is duplicated. Please choose a different username.", "OK");
                            return;
                        }

                        // Insert customer data
                        string customerQuery = "INSERT INTO customers (customer_name, customer_phone_number, customer_address, customer_email, customer_gender, customer_dateofbirth, customer_count, customer_totalspent)" +
                                            "VALUES (@customername, @customerphonenumber, @customeraddress, @customeremail, @customergender, @customerdateofbirth, 0, 0)";
                        MySqlCommand customerCommand = new MySqlCommand(customerQuery, connection, transaction);
                        customerCommand.Parameters.AddWithValue("@customername", cus.CustomerName);
                        customerCommand.Parameters.AddWithValue("@customerphonenumber", cus.CustomerPhone);
                        customerCommand.Parameters.AddWithValue("@customeraddress", cus.CustomerAddress);
                        customerCommand.Parameters.AddWithValue("@customeremail", cus.CustomerEmail);
                        customerCommand.Parameters.AddWithValue("@customergender", cus.CustomerGender);
                        customerCommand.Parameters.AddWithValue("@customerdateofbirth", cus.CustomerDateOfBirth);
                        customerCommand.ExecuteNonQuery();
                        
                        // Insert user data linked to the customer
                        long customerId = customerCommand.LastInsertedId;
                        string userQuery = "INSERT INTO users (username, password_hash, role, user_customer_id) VALUES (@Username, @PasswordHash, 'user', @CustomerId)";
                        MySqlCommand userCommand = new MySqlCommand(userQuery, connection, transaction);
                        userCommand.Parameters.AddWithValue("@Username", us.Username);
                        userCommand.Parameters.AddWithValue("@PasswordHash", us.PasswordHash);
                        userCommand.Parameters.AddWithValue("@CustomerId", customerId);
                        userCommand.ExecuteNonQuery();

                        // Commit the transaction
                        transaction.Commit();

                        // Add user and customer to the lists and show success message
                        ListUsers.Add(us);
                        ListCustomers.Add(cus);
                        MessageBox.Query("Success", "Registration successful!", "OK");
                        
                        // Close the registration window and go back to admin menu
                        top.Remove(registerWin);
                        admin.AdminMenu();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        MessageBox.ErrorQuery("Error", ex.Message, "OK");
                    }
                }
            };


            // Define and position the "Close" button
            var closeButton = new Button("Close")
            {
                X = Pos.Center(),
                Y = Pos.Bottom(registerButton) + 1
            };
            closeButton.Clicked += () =>
            {
                top.Remove(registerWin);
                admin.AdminMenu();
            };

            // Add all controls to the registration window
            registerWin.Add(usernameLabel, usernameField, passwordLabel, passwordField,
                            CustomerNameLabel, CustomerNameField, CustomerPhoneNumberLabel,
                            CustomerPhoneNumberField, CustomerAddressLabel, CustomerAddressField,
                            CustomerEmailLabel, CustomerEmailField, CustomerGenderLabel, CustomerGenderField,
                            CustomerDateOfBirthLabel, CustomerDateOfBirthField, registerButton, closeButton);

        
        }
    public void EditCustomer(string role, int customerID, string customerName, string customerPhone, string customerAddress, string customerEmail, string customerGender, DateTime customerDateOfBirth, string username, string password)
    {
        var top = Application.Top;

        // Create a new window for editing customer details
        var editCustomerWin = new Window("Edit Customer")
        {
            X = 0,
            Y = 0,
            Width = Dim.Fill(),
            Height = Dim.Fill()
        };
        top.Add(editCustomerWin);

        // Labels and fields for editing customer information
        var editCustomerNameLabel = new Label("Customer Name:")
        {
            X = 2,
            Y = 2
        };
        var editCustomerNameField = new TextField(customerName)
        {
            X = 18,
            Y = Pos.Right(editCustomerNameLabel),
            Width = 100
        };

        var editCustomerPhoneLabel = new Label("Customer Phone:")
        {
            X = 2,
            Y = Pos.Bottom(editCustomerNameLabel) + 1
        };
        var editCustomerPhoneField = new TextField(customerPhone)
        {
            X = 18,
            Y = Pos.Right(editCustomerPhoneLabel),
            Width = 100
        };

        var editCustomerAddressLabel = new Label("Customer Address:")
        {
            X = 2,
            Y = Pos.Bottom(editCustomerPhoneLabel) + 1
        };
        var editCustomerAddressField = new TextField(customerAddress)
        {
            X = 18,
            Y = Pos.Right(editCustomerAddressLabel),
            Width = 100
        };

        var editCustomerEmailLabel = new Label("Customer Email:")
        {
            X = 2,
            Y = Pos.Bottom(editCustomerAddressLabel) + 1
        };
        var editCustomerEmailField = new TextField(customerEmail)
        {
            X = 18,
            Y = Pos.Right(editCustomerEmailLabel),
            Width = 100
        };

        var editCustomerGenderLabel = new Label("Customer Gender:")
        {
            X = 2,
            Y = Pos.Bottom(editCustomerEmailLabel) + 1
        };
        var editCustomerGenderField = new TextField(customerGender)
        {
            X = 18,
            Y = Pos.Right(editCustomerGenderLabel),
            Width = 100
        };

        var editCustomerDateOfBirthLabel = new Label("Customer Date of Birth:")
        {
            X = 2,
            Y = Pos.Bottom(editCustomerGenderLabel) + 1
        };
        var editCustomerDateOfBirthField = new TextField(customerDateOfBirth.ToString("yyyy-MM-dd"))
        {
            X = 18,
            Y = Pos.Right(editCustomerDateOfBirthLabel),
            Width = 100
        };
        var usernameLabel = new Label("Username: ")
        {
            X = 2,
            Y = Pos.Bottom(editCustomerDateOfBirthLabel) + 1
        };
        var usernamefield = new TextField(username)
        {
            X = 18,
            Y = Pos.Right(usernameLabel),
        };
        var passLabel = new Label("Password: ")
        {
            X = 2,
            Y = Pos.Bottom(usernameLabel),
        };
        var passfield = new TextField(password)
        {
            X = 18,
            Y = Pos.Right(passLabel),
        };

        // Button to save edited customer information
        var saveButton = new Button("Save")
        {
            X = Pos.Center(),
            Y = Pos.Bottom(passfield) + 2
        };
        saveButton.Clicked += () =>
        {
            try
            {
                // Update the customer object with data from the text fields
                var updatedCustomerName = editCustomerNameField.Text.ToString();
                var updatedCustomerPhone = editCustomerPhoneField.Text.ToString();
                var updatedCustomerAddress = editCustomerAddressField.Text.ToString();
                var updatedCustomerEmail = editCustomerEmailField.Text.ToString();
                var updatedCustomerGender = editCustomerGenderField.Text.ToString();
                var updatedCustomerDateOfBirth = DateTime.Parse(editCustomerDateOfBirthField.Text.ToString());
                var updateusername = usernamefield.Text.ToString();
                var updatepass = passfield.Text.ToString();

                // Open a connection to the database and execute the update query
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    string query = @"UPDATE customers 
                                    SET customer_name = @CustomerName, 
                                        customer_phone_number = @CustomerPhone, 
                                        customer_address = @CustomerAddress, 
                                        customer_email = @CustomerEmail, 
                                        customer_gender = @CustomerGender, 
                                        customer_dateofbirth = @CustomerDateOfBirth 
                                    WHERE customer_id = @CustomerID;
                                    UPDATE users
                                    SET username = @UserName,
                                        password_hash = @Pass,
                                    WHERE user_customer_id = @CustomerID";
                    MySqlCommand command = new MySqlCommand(query, connection);
                    command.Parameters.AddWithValue("@CustomerID", customerID);
                    command.Parameters.AddWithValue("@CustomerName", updatedCustomerName);
                    command.Parameters.AddWithValue("@CustomerPhone", updatedCustomerPhone);
                    command.Parameters.AddWithValue("@CustomerAddress", updatedCustomerAddress);
                    command.Parameters.AddWithValue("@CustomerEmail", updatedCustomerEmail);
                    command.Parameters.AddWithValue("@CustomerGender", updatedCustomerGender);
                    command.Parameters.AddWithValue("@CustomerDateOfBirth", updatedCustomerDateOfBirth);
                    command.Parameters.AddWithValue("@UserName",updateusername);
                    command.Parameters.AddWithValue("@Pass",updatepass);

                    command.ExecuteNonQuery();
                }

                // Display a success message and close the window
                MessageBox.Query("Success", "Customer information has been updated!", "OK");
                top.Remove(editCustomerWin);
                // Refresh the customer list or do something else after saving
                // Example: cus.LoadCustomerList();
            }
            catch (Exception ex)
            {
                // Display an error message if the update fails
                MessageBox.ErrorQuery("Error", ex.Message, "OK");
            }
        };

        // Button to close the window
        var closeButton = new Button("Close")
        {
            X = Pos.Center(),
            Y = Pos.Bottom(saveButton) + 1
        };
        closeButton.Clicked += () =>
        {
            // Confirm before closing the window
            bool confirmed = MessageBox.Query("Confirm", "Are you sure you want to close?", "Yes", "No") == 0;
            if (confirmed)
            {
                top.Remove(editCustomerWin);
                switch (role)
                {
                    case "admin":
                    DisplayCustomers("admin");
                    break;
                    case "superadmin":
                    DisplayCustomers("superadmin");
                    break;
                }
            }
        };

        // Add all controls to the edit customer window
        editCustomerWin.Add(editCustomerNameLabel, editCustomerNameField,
                            editCustomerPhoneLabel, editCustomerPhoneField,
                            editCustomerAddressLabel, editCustomerAddressField,
                            editCustomerEmailLabel, editCustomerEmailField,
                            editCustomerGenderLabel, editCustomerGenderField,
                            editCustomerDateOfBirthLabel, editCustomerDateOfBirthField,
                            saveButton, closeButton);
    }

    public void DeleteCustomer(int customerID)
    {
        try
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = @"UPDATE customer
                                SET active = FALSE 
                                WHERE customer_id = @CustomerID;
                                UPDATE users
                                SET active = FALSE
                                WHERE user_customer_id = @CustomerID";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@CustomerID", customerID);
                command.ExecuteNonQuery();
            }
            MessageBox.Query("Success", "Customer successfully marked as inactive!", "OK");
        }
        catch (Exception ex)
        {
            MessageBox.ErrorQuery("Error", ex.Message, "OK");
        }
    }

    public void DisplayCustomers(string role)
    {
        var top = Application.Top;

        // Tạo cửa sổ chính
        var displayWindow = new Window("Customer List")
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

        displayWindow.Add(scrollView);
        top.Add(displayWindow);

        // Tạo một View để chứa tất cả các cửa sổ khách hàng
        var customerContainer = new View()
        {
            X = 0,
            Y = 0,
            Width = Dim.Fill(),
            Height = Dim.Fill()
        };

        scrollView.Add(customerContainer);

        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            string query = @"SELECT 
                                c.customer_id, 
                                c.customer_name, 
                                c.customer_phone_number, 
                                c.customer_address, 
                                c.customer_email, 
                                c.customer_gender, 
                                c.customer_dateofbirth,
                                c.customer_count,
                                c.customer_totalspent,
                                u.username,
								u.password_hash
                            FROM customers c
                            inner join users u on c.customer_id = u.user_customer_id
                            WHERE c.active = TRUE AND u.active =TRUE;";

            MySqlCommand command = new MySqlCommand(query, connection);
            connection.Open();
            MySqlDataReader reader = command.ExecuteReader();

            int colCount = 3; // Number of columns per row
            int colWidth = 40; // Width of each customer window
            int rowHeight = 15; // Height of each customer window
            int margin = 2; // Margin between customers

            int col = 0;
            int row = 0;

            while (reader.Read())
            {
                int customerId = int.Parse(reader["customer_id"].ToString());
                string customerName = reader["customer_name"].ToString();
                string customerPhone = reader["customer_phone_number"].ToString();
                string customerAddress = reader["customer_address"].ToString();
                string customerEmail = reader["customer_email"].ToString();
                string customerGender = reader["customer_gender"].ToString();
                DateTime customerDateOfBirth = DateTime.Parse(reader["customer_dateofbirth"].ToString());
                int orderCount = int.Parse(reader["customer_count"].ToString());
                decimal totalSpent = decimal.Parse(reader["customer_totalspent"].ToString());
                string username = reader["username"].ToString();
                string password = reader["password_hash"].ToString();

                var customerWindow = new Window($"{customerName}")
                {
                    X = col * (colWidth + margin),
                    Y = row * (rowHeight + margin),
                    Width = colWidth,
                    Height = rowHeight
                };

                var nameLabel = new Label($"Name: {customerName}")
                {
                    X = 1,
                    Y = 1,
                    Width = Dim.Fill()
                };
                var phoneLabel = new Label($"Phone: {customerPhone}")
                {
                    X = 1,
                    Y = 2,
                    Width = Dim.Fill()
                };
                var addressLabel = new Label($"Address: {customerAddress}")
                {
                    X = 1,
                    Y = 3,
                    Width = Dim.Fill()
                };
                var emailLabel = new Label($"Email: {customerEmail}")
                {
                    X = 1,
                    Y = 4,
                    Width = Dim.Fill()
                };
                var genderLabel = new Label($"Gender: {customerGender}")
                {
                    X = 1,
                    Y = 5,
                    Width = Dim.Fill()
                };
                var dobLabel = new Label($"DOB: {customerDateOfBirth:yyyy-MM-dd}")
                {
                    X = 1,
                    Y = 6,
                    Width = Dim.Fill()
                };
                var orderCountLabel = new Label($"Orders: {orderCount}")
                {
                    X = 1,
                    Y = 7,
                    Width = Dim.Fill()
                };
                var totalSpentLabel = new Label($"Spent: {totalSpent:C}")
                {
                    X = 1,
                    Y = 8,
                    Width = Dim.Fill()
                };

                var usernamelabel = new Label($"Username: {username}")
                {
                    X = 1,
                    Y = 9
                };
                var passlabel = new Label($"Password : {password}")
                {
                    X = 1, 
                    Y = 10
                };
                if (role == "admin")
                {
                    var editButton = new Button("Edit")
                    {
                        X = 1,
                        Y = 12
                    };
                    editButton.Clicked += () =>
                    {
                        top.Remove(displayWindow);
                        cus.EditCustomer(role, customerId, customerName, customerPhone, customerAddress, customerEmail, customerGender, customerDateOfBirth, username, password);
                    };

                    var deleteButton = new Button("Delete")
                    {
                        X = Pos.Right(editButton) + 2,
                        Y = 12
                    };
                    deleteButton.Clicked += () =>
                    {
                        bool confirmed = MessageBox.Query("Confirm", "Are you sure you want to disable this customer?", "Yes", "No") == 0;
                        if (confirmed)
                        {
                            try
                            {
                                cus.DeleteCustomer(customerId);
                                customerWindow.Dispose(); // Remove customer window from view
                            }
                            catch (Exception ex)
                            {
                                MessageBox.ErrorQuery("Error", ex.Message, "OK");
                            }
                        }
                    };

                    customerWindow.Add(editButton, deleteButton);
                }

                customerWindow.Add(nameLabel, phoneLabel, addressLabel, emailLabel, genderLabel, dobLabel, orderCountLabel, totalSpentLabel, usernamelabel, passlabel);
                customerContainer.Add(customerWindow);

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

        // Tạo nút Back
        var backButton = new Button("Back")
        {
            X = Pos.Center(),
            Y = Pos.Bottom(displayWindow) - 3
        };
        backButton.Clicked += () =>
        {
            top.Remove(displayWindow);
            switch (role)
            {
                case "superadmin":
                    superadmin.SuperAdminMenu(); // Gọi menu user
                    break;
                case "admin":
                    admin.AdminMenu(); // Gọi menu admin
                    break;
            }
        };

        displayWindow.Add(backButton);
    }

    public void FindCustomer(string role)
    {
        var top = Application.Top;
        var findCustomerWin = new Window("Find Customer")
        {
            X = 0,
            Y = 0,
            Width = Dim.Fill(),
            Height = Dim.Fill()
        };
        top.Add(findCustomerWin);

        // Label and field for entering the customer name
        var customerNameLabel = new Label("Customer name:")
        {
            X = 2,
            Y = 2
        };
        var customerNameField = new TextField("")
        {
            X = Pos.Right(customerNameLabel) + 1,
            Y = 2,
            Width = 100
        };

        // Button to find the customer
        var findButton = new Button("Find")
        {
            X = Pos.Center(),
            Y = 4
        };

        // Event handler for the Find button
        findButton.Clicked += () =>
        {
            try
            {
                top.Remove(findCustomerWin);
                string customerName = customerNameField.Text.ToString();
        
                // Query to retrieve customer information based on customer name
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    string query = @"
                        SELECT 
                            cus.customer_name, 
                            cus.customer_phone_number, 
                            cus.customer_address, 
                            cus.customer_email, 
                            cus.customer_gender, 
                            cus.customer_dateofbirth,
                            cus.customer_count,
                            cus.customer_totalspent,
                            usr.username,
                            usr.password_hash
                        FROM customers cus
                        INNER JOIN users usr ON cus.customer_id = usr.user_customer_id
                        WHERE cus.customer_name LIKE @SearchTerm AND usr.role = 'user' AND cus.active = TRUE";
                    
                    MySqlCommand command = new MySqlCommand(query, connection);
                    command.Parameters.AddWithValue("@SearchTerm", "%" + customerName + "%");
                    connection.Open();
                    MySqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {

                        cus.CustomerName = reader["customer_name"].ToString();
                        cus.CustomerPhone = reader["customer_phone_number"].ToString();
                        cus.CustomerAddress = reader["customer_address"].ToString();
                        cus.CustomerEmail = reader["customer_email"].ToString();
                        cus.CustomerGender = reader["customer_gender"].ToString();
                        cus.CustomerDateOfBirth = DateTime.Parse(reader["customer_dateofbirth"].ToString());
                        cus.CustomerCount = int.Parse(reader["customer_count"].ToString());
                        cus.CustomerTotalSpent = decimal.Parse(reader["customer_totalspent"].ToString());
                        user.Username = reader["username"].ToString();
                        user.PasswordHash = reader["password_hash"].ToString();

                        ListCustomers.Add(cus);
                        ListUsers.Add(user);
                    }

                    reader.Close();
                }
                int row = 0;
                int col = 0;
                int colCount = 2; // Số cột trong ScrollView
                int colWidth = 40; // Chiều rộng của mỗi cửa sổ khách hàng
                int rowHeight = 12; // Chiều cao của mỗi cửa sổ khách hàng
                int margin = 2; // Lề giữa các khách hàng

                if (ListCustomers.Count == 0 && ListUsers.Count == 0)
                {
                    // Display error if no customers found
                    MessageBox.ErrorQuery("Error", "Customer not found!", "OK");
                    cus.FindCustomer("admin");
                }
                else
                {
                    // Create a new window to display the customers
                    var customersWindow = new Window("Customers")
                    {
                        X = 0,
                        Y = 0,
                        Width = Dim.Fill(),
                        Height = Dim.Fill()
                    };

                    // Create a ScrollView to hold the customers
                    var scrollView = new ScrollView()
                    {
                        X = 0,
                        Y = 0,
                        Width = Dim.Fill(),
                        Height = Dim.Fill(),
                        ContentSize = new Size(0, 0) // Kích thước nội dung sẽ được thiết lập sau
                    };

                    customersWindow.Add(scrollView);
                    top.Add(customersWindow);

                    var customerContainer = new View()
                    {
                        X = 0,
                        Y = 0,
                        Width = Dim.Fill(),
                        Height = Dim.Fill()
                    };

                    scrollView.Add(customerContainer);

                    foreach (var customer in ListCustomers)
                    {
                        var customerWindow = new Window($"Customer {row * colCount + col + 1}")
                        {
                            X = col * (colWidth + margin),
                            Y = row * (rowHeight + margin),
                            Width = colWidth,
                            Height = rowHeight
                        };

                        // Create labels for displaying customer details
                        var customerNameLabel = new Label($"Name: {customer.CustomerName}")
                        {
                            X = 1,
                            Y = 1,
                            Width = Dim.Fill(),
                            TextAlignment = TextAlignment.Left // Left text alignment
                        };
                        var customerPhoneLabel = new Label($"Phone: {customer.CustomerPhone}")
                        {
                            X = 1,
                            Y = Pos.Bottom(customerNameLabel) + 1,
                            Width = Dim.Fill(),
                            TextAlignment = TextAlignment.Left // Left text alignment
                        };
                        var customerAddressLabel = new Label($"Address: {customer.CustomerAddress}")
                        {
                            X = 1,
                            Y = Pos.Bottom(customerPhoneLabel) + 1,
                            Width = Dim.Fill(),
                            TextAlignment = TextAlignment.Left // Left text alignment
                        };
                        var customerEmailLabel = new Label($"Email: {customer.CustomerEmail}")
                        {
                            X = 1,
                            Y = Pos.Bottom(customerAddressLabel) + 1,
                            Width = Dim.Fill(),
                            TextAlignment = TextAlignment.Left // Left text alignment
                        };
                        var customerGenderLabel = new Label($"Gender: {customer.CustomerGender}")
                        {
                            X = 1,
                            Y = Pos.Bottom(customerEmailLabel) + 1,
                            Width = Dim.Fill(),
                            TextAlignment = TextAlignment.Left // Left text alignment
                        };
                        var customerDOBLabel = new Label($"DOB: {customer.CustomerDateOfBirth.ToShortDateString()}")
                        {
                            X = 1,
                            Y = Pos.Bottom(customerGenderLabel) + 1,
                            Width = Dim.Fill(),
                            TextAlignment = TextAlignment.Left // Left text alignment
                        };
                        var customerCountLabel = new Label($"Count: {customer.CustomerCount}")
                        {
                            X = 1,
                            Y = Pos.Bottom(customerDOBLabel) + 1,
                            Width = Dim.Fill(),
                            TextAlignment = TextAlignment.Left // Left text alignment
                        };
                        var customerTotalSpentLabel = new Label($"Total Spent: {customer.CustomerTotalSpent:C}")
                        {
                            X = 1,
                            Y = Pos.Bottom(customerCountLabel) + 1,
                            Width = Dim.Fill(),
                            TextAlignment = TextAlignment.Left // Left text alignment
                        };
                        foreach(var user in ListUsers)
                        {
                            var usernameLabel = new Label($"Username: {user.Username}")
                            {
                                X = 1,
                                Y = Pos.Bottom(customerTotalSpentLabel) + 1,
                                Width = Dim.Fill(),
                                TextAlignment = TextAlignment.Left // Left text alignment
                            };
                            var passwordLabel = new Label($"Password: {user.PasswordHash}")
                            {
                                X = 1,
                                Y = Pos.Bottom(usernameLabel) + 1,
                                Width = Dim.Fill(),
                                TextAlignment = TextAlignment.Left // Left text alignment
                            };
                            // Add labels to the customer window
                            customerWindow.Add(customerNameLabel, customerPhoneLabel, customerAddressLabel, customerEmailLabel, customerGenderLabel, customerDOBLabel, customerCountLabel, customerTotalSpentLabel, usernameLabel, passwordLabel);
                            customerContainer.Add(customerWindow);
                        }
                        

                        col++;
                        if (col >= colCount)
                        {
                            col = 0;
                            row++;
                        }
                    }
                // Cập nhật kích thước nội dung của ScrollView
                scrollView.ContentSize = new Size((colWidth + margin) * colCount, (row + 1) * (rowHeight + margin));
                // Create a button labeled "Close" to close the customers window
                var btnClose = new Button("Close")
                {
                    X = Pos.Center(),
                    Y = 1
                };

                // Define the action to be taken when the close button is clicked
                btnClose.Clicked += () =>
                {
                    ListCustomers.Clear();
                    ListUsers.Clear();
                    // Remove the customers window
                    top.Remove(customersWindow);
                    switch (role)
                    {
                        case "admin":
                            cus.FindCustomer("admin");
                            break;
                        case "superadmin":
                            cus.FindCustomer("superadmin");
                            break;
                    }
                };

                // Add the close button to the customers window
                customersWindow.Add(btnClose);
                }

            }
            catch (Exception ex)
            {
                // Display error message for any exception
                MessageBox.ErrorQuery("Error", ex.Message, "OK");
            }
        };

        // Button to close the find customer window
        var closeButton = new Button("Close")
        {
            X = Pos.Center(),
            Y = Pos.Percent(100) - 1
        };

        // Event handler for the Close button
        closeButton.Clicked += () =>
        {
            ListCustomers.Clear();
            ListUsers.Clear();
            top.Remove(findCustomerWin);
            // Return to respective menu based on user role
            switch (role)
            {
                case "admin":
                    admin.AdminMenu();
                    break;
                case "superadmin":
                    superadmin.SuperAdminMenu();
                    break;
            }
        };

        // Add components to the window
        findCustomerWin.Add(customerNameLabel, customerNameField, findButton, closeButton);
    }
    public void UserMenu()
    {
        // Initialize the application
        Application.Init();

        // Get the top-level application instance
        var top = Application.Top;

        // Create the main window for user menu
        var userMenu = new Window()
        {
            Title = "Electronic Shop",
            X = 0,
            Y = 0,
            Width = Dim.Fill(),
            Height = Dim.Fill()
        };
        top.Add(userMenu);

        // Left frame for displaying menu functions
        var leftFrame = new FrameView("Function")
        {
            X = 0,
            Y = 0,
            Width = Dim.Percent(30),
            Height = Dim.Fill()
        };
        userMenu.Add(leftFrame);

        // Right top frame for displaying welcome message
        var rightTopFrame = new FrameView("Welcome to")
        {
            
            X = Pos.Percent(30),
            Y = 0,
            Width = Dim.Fill(),
            Height = Dim.Percent(50)
        };
        var sign = new Label(@"███████╗██╗     ███████╗ ██████╗████████╗██████╗  ██████╗ ███╗   ██╗██╗ ██████╗    ███████╗██╗  ██╗ ██████╗ ██████╗ 
██╔════╝██║     ██╔════╝██╔════╝╚══██╔══╝██╔══██╗██╔═══██╗████╗  ██║██║██╔════╝    ██╔════╝██║  ██║██╔═══██╗██╔══██╗
█████╗  ██║     █████╗  ██║        ██║   ██████╔╝██║   ██║██╔██╗ ██║██║██║         ███████╗███████║██║   ██║██████╔╝
██╔══╝  ██║     ██╔══╝  ██║        ██║   ██╔══██╗██║   ██║██║╚██╗██║██║██║         ╚════██║██╔══██║██║   ██║██╔═══╝ 
███████╗███████╗███████╗╚██████╗   ██║   ██║  ██║╚██████╔╝██║ ╚████║██║╚██████╗    ███████║██║  ██║╚██████╔╝██║     
╚══════╝╚══════╝╚══════╝ ╚═════╝   ╚═╝   ╚═╝  ╚═╝ ╚═════╝ ╚═╝  ╚═══╝╚═╝ ╚═════╝    ╚══════╝╚═╝  ╚═╝ ╚═════╝ ╚═╝     "
        )
        {
            X = Pos.Center(),
            Y = Pos.Center(),
        };
        rightTopFrame.Add(sign);
        userMenu.Add(rightTopFrame);

        // Right bottom frame for displaying top products by order count
        var rightBottomFrame = new FrameView("HELLO")
        {
            X = Pos.Percent(30),
            Y = Pos.Percent(50),
            Width = Dim.Fill(),
            Height = Dim.Fill()
        };
        userMenu.Add(rightBottomFrame);

        var btnDisplayProduct = new Button("Display Product")
        {
            X = 2,
            Y = 2
        };
        btnDisplayProduct.Clicked += () =>
        {
            try
            {
                top.Remove(userMenu);
                pd.DisplayProduct("user");
            }
            catch
            {
                MessageBox.ErrorQuery("Error","" ,"OK");
            }
        };
        var btnFindProduct = new Button("Find Product")
        {
            X = 2,
            Y = 3
        };
        btnFindProduct.Clicked += () =>
        {
            try
            {
                top.Remove(userMenu);
                pd.FindProduct("user");
            }
            catch
            {
                MessageBox.ErrorQuery("Error", "" ,"OK");
            }
        };
        var btnViewOrder = new Button("View My Order")
        {
            X = 2,
            Y = 4
        }; 
        btnViewOrder.Clicked += () =>
        {
            try
            {
                top.Remove(userMenu);
                order.DisplayMyOrder();
            }
            catch
            {
                MessageBox.ErrorQuery ("Error","" ,"OK");
            }
        };
        var btnViewCart = new Button("View My Cart")
        {
            X = 2,
            Y = 5,
        };
        btnViewCart.Clicked += () =>
        {
            try
            {
                top.Remove(userMenu);
                userCart.DisplayCart();
            }
            catch 
            {
                MessageBox.ErrorQuery("Error","","OK");
            }
        };
        var btnDisplayCategory = new Button("Display Category")
        {
            X = 2,
            Y = 6
        };
        btnDisplayCategory.Clicked += () =>
        {
            try
            {
                top.Remove(userMenu);
                cate.DisplayCategories("user");
            }
            catch
            {
                MessageBox.ErrorQuery("Error","","OK");
            }
        };

        var btnLogout = new Button("Logout")
        {
            X = 2,
            Y = 19,
        };
        btnLogout.Clicked += () =>
        {
            top.Remove(userMenu);
            program.Login();
        };

        // Add buttons to the left frame
        leftFrame.Add(btnDisplayProduct,btnFindProduct, btnViewOrder, btnViewCart, btnDisplayCategory, btnLogout);
        
        string customerName = "";
        string customerCount = "";
        string customerTotalSpent = "";
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            string query = @"SELECT customer_name,
                                    customer_count,
                                    customer_totalspent
                                    FROM customers WHERE customer_id = @CustomerID";
            MySqlCommand command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@CustomerID", SessionData.Instance.CurrentCustomerID);
            connection.Open();
            MySqlDataReader reader = command.ExecuteReader();
            if (reader.Read())
            {
                customerName = reader["customer_name"].ToString();
                customerCount = reader["customer_count"].ToString();
                customerTotalSpent = reader["customer_totalspent"].ToString();
            }
        }
        var nameLabel = new Label()
        {
            Text = string.Format(
                "Name: {0}\n" +
                "Number of times ordered: {1}\n" +
                "Total amount spent: {2:C}",
                customerName,
                customerCount,
                customerTotalSpent),
            X = Pos.Center(),
            Y = Pos.Center(),
        };


        rightBottomFrame.Add(nameLabel);        
    }
}

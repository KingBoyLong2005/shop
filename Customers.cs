using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;                      //Import namesapce to use function
using System.Threading.Tasks;
using MySql.Data.MySqlClient;  
using Spectre.Console;
using Terminal.Gui;

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

    // Retrieve the current customer ID from the session data
    public static int currentCustomerID = SessionData.Instance.CurrentCustomerID;

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
            string query = "SELECT * FROM customers"; 
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
            Application.Shutdown();
        };

        // Add all controls to the registration window
        registerWin.Add(usernameLabel, usernameField, passwordLabel, passwordField,
                        CustomerNameLabel, CustomerNameField, CustomerPhoneNumberLabel,
                        CustomerPhoneNumberField, CustomerAddressLabel, CustomerAddressField,
                        CustomerEmailLabel, CustomerEmailField, CustomerGenderLabel, CustomerGenderField,
                        CustomerDateOfBirthLabel, CustomerDateOfBirthField, registerButton, closeButton);

    
    }
    public void EditCustomer()
    {
        // Create a new Customers object and a list of customers loaded from the database
        Customers customer = new Customers();
        List<Customers> customersList = LoadCustomers(connectionString);

        // Get the top-level application window and create a new window for editing customer details
        var top = Application.Top;
        var editCustomerWin = new Window("Edit Customer")
        {
            X = 0,
            Y = 0,
            Width = Dim.Fill() - 4,
            Height = Dim.Fill() - 4
        };
        top.Add(editCustomerWin);

        // Define labels and text fields for searching and editing customer details
        var findCustomerIDLabel = new Label("Find Customer ID:")
        {
            X = 1,
            Y = 1
        };
        var findCustomerIDField = new TextField("")
        {
            X = 31,
            Y = 1,
            Width = 100
        };
        var editCustomerNameLabel = new Label("Customer Name:")
        {
            X = 1,
            Y = 5,
            Visible = false
        };
        var editCustomerNameField = new TextField("")
        {
            X = 31,
            Y = 5,
            Width = 100,
            Visible = false
        };
        var editCustomerPhoneLabel = new Label("Customer Phone:")
        {
            X = 1,
            Y = 7,
            Visible = false
        };
        var editCustomerPhoneField = new TextField("")
        {
            X = 31,
            Y = 7,
            Width = 100,
            Visible = false
        };
        var editCustomerAddressLabel = new Label("Customer Address:")
        {
            X = 1,
            Y = 9,
            Visible = false
        };
        var editCustomerAddressField = new TextField("")
        {
            X = 31,
            Y = 9,
            Width = 100,
            Visible = false
        };
        var editCustomerEmailLabel = new Label("Customer Email:")
        {
            X = 1,
            Y = 11,
            Visible = false
        };
        var editCustomerEmailField = new TextField("")
        {
            X = 31,
            Y = 11,
            Width = 100,
            Visible = false
        };
        var editCustomerGenderLabel = new Label("Customer Gender:")
        {
            X = 1,
            Y = 13,
            Visible = false
        };
        var editCustomerGenderField = new TextField("")
        {
            X = 31,
            Y = 13,
            Width = 100,
            Visible = false
        };

        var editCustomerDateOfBirthLabel = new Label("Customer Date of Birth:")
        {
            X = 1,
            Y = 15,
            Visible = false
        };
        var editCustomerDateOfBirthField = new TextField("")
        {
            X = 31,
            Y = 15,
            Width = 100,
            Visible = false
        };
        
        // Define a save button for saving the edited customer details to the database
        var saveButton = new Button("Save")
        {
            X = Pos.Center(),
            Y = 17,
            Visible = false
        };
        saveButton.Clicked += () =>
        {
            try
            {
                // Update the customer object with data from the text fields
                customer.CustomerID = int.Parse(findCustomerIDField.Text.ToString());
                customer.CustomerName = editCustomerNameField.Text.ToString();
                customer.CustomerPhone = editCustomerPhoneField.Text.ToString();
                customer.CustomerAddress = editCustomerAddressField.Text.ToString();
                customer.CustomerEmail = editCustomerEmailField.Text.ToString();
                customer.CustomerGender = editCustomerGenderField.Text.ToString();
                customer.CustomerDateOfBirth = DateTime.Parse(editCustomerDateOfBirthField.Text.ToString());

                // Open a connection to the database and execute the update query
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "UPDATE customers SET customer_name = @CustomerName, customer_phone_number = @CustomerPhone, customer_address = @CustomerAddress, customer_email = @CustomerEmail, customer_gender = @CustomerGender, customer_dateofbirth = @CustomerDateOfBirth WHERE customer_id = @CustomerID";
                    MySqlCommand command = new MySqlCommand(query, connection);
                    command.Parameters.AddWithValue("@CustomerID", customer.CustomerID);
                    command.Parameters.AddWithValue("@CustomerName", customer.CustomerName);
                    command.Parameters.AddWithValue("@CustomerPhone", customer.CustomerPhone);
                    command.Parameters.AddWithValue("@CustomerAddress", customer.CustomerAddress);
                    command.Parameters.AddWithValue("@CustomerEmail", customer.CustomerEmail);
                    command.Parameters.AddWithValue("@CustomerGender", customer.CustomerGender);
                    command.Parameters.AddWithValue("@CustomerDateOfBirth", customer.CustomerDateOfBirth);

                    command.ExecuteNonQuery();
                }
                // Display a success message and return to the admin menu
                MessageBox.Query("Success", "Customer information has been updated!", "OK");
                top.Remove(editCustomerWin);
                admin.AdminMenu();
            }
            catch (Exception ex)
            {
                // Display an error message if the update fails
                MessageBox.ErrorQuery("Error", ex.Message, "OK");
            }
        };

        // Define a find button for searching for a customer by ID
        var findButton = new Button("Find")
        {
            X = Pos.Center(),
            Y = 3
        };
        findButton.Clicked += () =>
        {
            try
            {
                int customerID = int.Parse(findCustomerIDField.Text.ToString());
                var foundCustomer = customersList.FirstOrDefault(c => c.CustomerID == customerID);

                if (foundCustomer != null)
                {
                    // Populate fields with customer information and show the fields
                    editCustomerNameField.Text = foundCustomer.CustomerName;
                    editCustomerPhoneField.Text = foundCustomer.CustomerPhone;
                    editCustomerAddressField.Text = foundCustomer.CustomerAddress;
                    editCustomerEmailField.Text = foundCustomer.CustomerEmail;
                    editCustomerGenderField.Text = foundCustomer.CustomerGender;
                    editCustomerDateOfBirthField.Text = foundCustomer.CustomerDateOfBirth.ToString();

                    findCustomerIDLabel.Visible = false;
                    findCustomerIDField.Visible = false;
                    findButton.Visible = false;

                    editCustomerNameLabel.Visible = true;
                    editCustomerNameField.Visible = true;
                    editCustomerPhoneLabel.Visible = true;
                    editCustomerPhoneField.Visible = true;
                    editCustomerAddressLabel.Visible = true;
                    editCustomerAddressField.Visible = true;
                    editCustomerEmailLabel.Visible = true;
                    editCustomerEmailField.Visible = true;
                    editCustomerGenderLabel.Visible = true;
                    editCustomerGenderField.Visible = true;
                    editCustomerDateOfBirthLabel.Visible = true;
                    editCustomerDateOfBirthField.Visible = true;

                    saveButton.Visible = true;
                }
                else
                {
                    // Display an error message if the customer is not found
                    MessageBox.ErrorQuery("Error", "Customer not found!", "OK");
                }
            }
            catch (Exception ex)
            {
                // Display an error message if an exception occurs
                MessageBox.ErrorQuery("Error", ex.Message, "OK");
            }
        };
        // Define a close button for closing the edit customer window
        var closeButton = new Button("Close")
        {
            X = Pos.Center(),
            Y = 19
        };
        closeButton.Clicked += () =>
        {
            // Ask for confirmation before closing the window
            bool confirmed = MessageBox.Query("Confirm", "Are you sure you want to close?", "Yes", "No") == 0;
            if (confirmed)
            {
            top.Remove(editCustomerWin);
            admin.AdminMenu();
            }
        };

        // Add all controls to the edit customer window
        editCustomerWin.Add(findCustomerIDLabel, findCustomerIDField, findButton,
                            editCustomerNameLabel, editCustomerNameField,
                            editCustomerPhoneLabel, editCustomerPhoneField,
                            editCustomerAddressLabel, editCustomerAddressField,
                            editCustomerEmailLabel, editCustomerEmailField,
                            editCustomerGenderLabel, editCustomerGenderField,
                            editCustomerDateOfBirthLabel, editCustomerDateOfBirthField,
                            saveButton, closeButton);
    }
    public void DeleteCustomer()
    {
        // Load the list of customers from the database using the connection string.
        ListCustomers = LoadCustomers(connectionString);

        // Get the top-level window of the application.
        var top = Application.Top;

        // Create a new window titled "Delete Customer" with specific dimensions.
        var deleteCustomerWin = new Window("Delete Customer")
        {
            X = 0,
            Y = 0,
            Width = Dim.Fill() - 4,
            Height = Dim.Fill() - 4
        };

        // Add the delete customer window to the top-level window.
        top.Add(deleteCustomerWin);

        // Create a label for customer ID input.
        var customerIDLabel = new Label("Customer ID:")
        {
            X = 1,
            Y = 1
        };

        // Create a text field for entering the customer ID.
        var customerIDField = new TextField("")
        {
            X = Pos.Right(customerIDLabel) + 1,
            Y = 1,
            Width = 100
        };

        // Create a button labeled "Delete" for deleting the customer.
        var deleteButton = new Button("Delete")
        {
            X = Pos.Center(),
            Y = Pos.Bottom(customerIDField) + 1
        };

        // Define the action to be taken when the delete button is clicked.
        deleteButton.Clicked += () =>
        {
            try
            {
                // Check if the input is a valid integer representing customer ID.
                if (int.TryParse(customerIDField.Text.ToString(), out int customerID))
                {
                    // Find the customer in the list with the matching ID.
                    Customers kh = ListCustomers.Find(kh => kh.CustomerID == customerID);

                    // If the customer is found in the list.
                    if (kh != null)
                    {
                        // Connect to the database and execute the deletion queries.
                        using (MySqlConnection connection = new MySqlConnection(connectionString))
                        {
                            connection.Open();

                            // First delete from the 'users' table.
                            string userQuery = "DELETE FROM users WHERE user_customer_id = @customerid";
                            MySqlCommand userCommand = new MySqlCommand(userQuery, connection);
                            userCommand.Parameters.AddWithValue("@customerid", customerID);
                            userCommand.ExecuteNonQuery();

                            // Then delete from the 'customers' table.
                            string customerQuery = "DELETE FROM customers WHERE customer_id = @customerid";
                            MySqlCommand customerCommand = new MySqlCommand(customerQuery, connection);
                            customerCommand.Parameters.AddWithValue("@customerid", customerID);
                            customerCommand.ExecuteNonQuery();
                        }

                        // Remove the customer from the local list.
                        ListCustomers.Remove(kh);

                        // Show a success message box.
                        MessageBox.Query("Success", "Successfully deleted customer!", "OK");

                        // Remove the delete customer window and go back to the admin menu.
                        top.Remove(deleteCustomerWin);
                        admin.AdminMenu();
                    }
                    else
                    {
                        // Show an error message if the customer is not found.
                        MessageBox.ErrorQuery("Error", "Customer not found!", "OK");
                    }
                }
                else
                {
                    // Show an error message if the input is not a valid customer ID.
                    MessageBox.ErrorQuery("Error", "Invalid customer ID!", "OK");
                }
            }
            catch (Exception ex)
            {
                // Show an error message if an exception occurs during the process.
                MessageBox.ErrorQuery("Error", ex.Message, "OK");
            }
        };

        // Create a button labeled "Close" to close the delete customer window.
        var closeButton = new Button("Close")
        {
            X = Pos.Center(),
            Y = Pos.Bottom(deleteButton) + 1
        };

        // Define the action to be taken when the close button is clicked.
        closeButton.Clicked += () =>
        {
            top.Remove(deleteCustomerWin);
            admin.AdminMenu();
        };

        // Add all the created UI elements to the delete customer window.
        deleteCustomerWin.Add(customerIDLabel, customerIDField, deleteButton, closeButton);
    }

    public void DisplayCustomers()
    {
        // Load the list of customers from the database using the connection string.
        List<Customers> customersList = LoadCustomers(connectionString);
        
        // Get the top-level window of the application.
        var top = Application.Top;

        // Create a new window titled "Display Customers" with specific dimensions.
        var displayCustomerWindow = new Window("Display Customers")
        {
            X = 0,
            Y = 0,
            Width = Dim.Fill(),
            Height = Dim.Fill()
        };

        // Add the display customer window to the top-level window.
        top.Add(displayCustomerWindow);

        // Move focus to the next UI element in the window.
        displayCustomerWindow.FocusNext();

        // Establish a connection to the database.
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            // Define the SQL query to retrieve customer data.
            string query = @"SELECT 
                                cus.customer_id, 
                                cus.customer_name, 
                                cus.customer_phone_number, 
                                cus.customer_address, 
                                cus.customer_email, 
                                cus.customer_gender, 
                                cus.customer_dateofbirth,
                                cus.customer_count,
                                cus.customer_totalspent
                            FROM customers cus";

            // Create a command object to execute the query.
            MySqlCommand command = new MySqlCommand(query, connection);
            
            // Open the connection to the database.
            connection.Open();
            
            // Execute the query and obtain a data reader to read the results.
            MySqlDataReader reader = command.ExecuteReader();

            // Define the column headers to be displayed in the window.
            var columnDisplayListCustomer = new string[]
            {
                "Customer ID", "Name", "Phone Number", "Address", "Email", "Gender", "Date of Birth", "Order Count", "Total Spent"
            };

            // Add column headers to the window.
            for (int i = 0; i < columnDisplayListCustomer.Length; i++)
            {
                displayCustomerWindow.Add(new Label(columnDisplayListCustomer[i])
                {
                    X = i * 20,
                    Y = 0,
                    Width = 20,
                    Height = 1
                });
            }

            // Initialize the row offset for displaying data rows.
            int rowOffset = 1;

            // Read the data row by row from the data reader.
            while (reader.Read())
            {
                // Retrieve customer details from the current row.
                int customerId = Convert.ToInt32(reader["customer_id"]);
                string customerName = reader["customer_name"].ToString();
                string customerPhone = reader["customer_phone_number"].ToString();
                string customerAddress = reader["customer_address"].ToString();
                string customerEmail = reader["customer_email"].ToString();
                string customerGender = reader["customer_gender"].ToString();
                DateTime customerDateOfBirth = Convert.ToDateTime(reader["customer_dateofbirth"]);
                int orderCount = Convert.ToInt32(reader["customer_count"]);
                decimal totalSpent = Convert.ToDecimal(reader["customer_totalspent"]);

                // Add customer details to the window as labels.
                displayCustomerWindow.Add(new Label(customerId.ToString())
                {
                    X = 0,
                    Y = rowOffset,
                    Width = 20,
                    Height = 1
                });
                displayCustomerWindow.Add(new Label(customerName)
                {
                    X = 1 * 20,
                    Y = rowOffset,
                    Width = 20,
                    Height = 1
                });
                displayCustomerWindow.Add(new Label(customerPhone)
                {
                    X = 2 * 20,
                    Y = rowOffset,
                    Width = 20,
                    Height = 1
                });
                displayCustomerWindow.Add(new Label(customerAddress)
                {
                    X = 3 * 20,
                    Y = rowOffset,
                    Width = 20,
                    Height = 1
                });
                displayCustomerWindow.Add(new Label(customerEmail)
                {
                    X = 4 * 20,
                    Y = rowOffset,
                    Width = 20,
                    Height = 1
                });
                displayCustomerWindow.Add(new Label(customerGender)
                {
                    X = 5 * 20,
                    Y = rowOffset,
                    Width = 20,
                    Height = 1
                });
                displayCustomerWindow.Add(new Label(customerDateOfBirth.ToString("yyyy-MM-dd"))
                {
                    X = 6 * 20,
                    Y = rowOffset,
                    Width = 20,
                    Height = 1
                });
                displayCustomerWindow.Add(new Label(orderCount.ToString())
                {
                    X = 7 * 20,
                    Y = rowOffset,
                    Width = 20,
                    Height = 1
                });
                displayCustomerWindow.Add(new Label(totalSpent.ToString("0.00"))
                {
                    X = 8 * 20,
                    Y = rowOffset,
                    Width = 20,
                    Height = 1
                });

                // Increment the row offset for the next data row.
                rowOffset++;
            }

            // Close the data reader.
            reader.Close();
        }

        // Create a button labeled "Close" to close the display customer window.
        var btnClose = new Button("Close")
        {
            X = Pos.Center(),
            Y = Pos.Percent(100) - 1
        };

        // Define the action to be taken when the close button is clicked.
        btnClose.Clicked += () =>
        {
            // Remove the display customer window and go back to the admin menu.
            top.Remove(displayCustomerWindow);
            admin.AdminMenu();
        };

        // Add the close button to the display customer window.
        displayCustomerWindow.Add(btnClose);
    }


    public void FindCustomer()
    {
        // Create a list to hold customer data.
        List<string[]> customers = new List<string[]>();
        
        // Get the top-level window of the application.
        var top = Application.Top;

        // Create a new window titled "Find Customer" with specific dimensions.
        var findCustomerWindow = new Window("Find Customer")
        {
            X = 0,
            Y = 0,
            Width = Dim.Fill(),
            Height = Dim.Fill()
        };

        // Add the find customer window to the top-level window.
        top.Add(findCustomerWindow);

        // Move focus to the next UI element in the window.
        findCustomerWindow.FocusNext();

        // Create and position a label for the search field.
        var searchLabel = new Label("Enter customer name:")
        {
            X = 1,
            Y = 1
        };

        // Create and position a text field for entering the customer name to search.
        var searchField = new TextField("")
        {
            X = Pos.Right(searchLabel) + 1,
            Y = 1,
            Width = 40
        };

        // Create and position a button to initiate the search.
        var searchButton = new Button("Search")
        {
            X = Pos.Right(searchField) + 1,
            Y = 1
        };

        // Add the search label, field, and button to the find customer window.
        findCustomerWindow.Add(searchLabel, searchField, searchButton);

        // Create and position a label for displaying the search results.
        var resultLabel = new Label("Results:")
        {
            X = 1,
            Y = 3
        };
        findCustomerWindow.Add(resultLabel);

        // Define the column headers to be displayed in the window.
        var columnDisplayListCustomer = new string[]
        {
            "Customer's name", "Phone number", "Address", "Email", "Gender", "Date of birth", "Order count", "Total spent"
        };

        // Define the width of each column.
        int columnWidth = 20; // Increase column width

        // Add column headers to the window.
        for (int i = 0; i < columnDisplayListCustomer.Length; i++)
        {
            findCustomerWindow.Add(new Label(columnDisplayListCustomer[i])
            {
                X = i * columnWidth,
                Y = 4,
                Width = columnWidth,
                Height = 1
            });
        }

        // Define the action to be taken when the search button is clicked.
        searchButton.Clicked += () =>
        {
            // Clear the previous search results.
            customers.Clear();

            // Get the search term from the text field.
            string searchTerm = searchField.Text.ToString();

            // Establish a connection to the database.
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                // Define the SQL query to search for customers by name.
                string query = @"SELECT 
                                    cus.customer_name, 
                                    cus.customer_phone_number, 
                                    cus.customer_address, 
                                    cus.customer_email, 
                                    cus.customer_gender, 
                                    cus.customer_dateofbirth,
                                    cus.customer_count,
                                    cus.customer_totalspent
                                FROM customers cus
                                WHERE cus.customer_name LIKE @SearchTerm";

                // Create a command object to execute the query.
                MySqlCommand command = new MySqlCommand(query, connection);

                // Add a parameter to the query for the search term.
                command.Parameters.AddWithValue("@SearchTerm", "%" + searchTerm + "%");

                // Open the connection to the database.
                connection.Open();

                // Execute the query and obtain a data reader to read the results.
                MySqlDataReader reader = command.ExecuteReader();

                // Read the data row by row from the data reader.
                while (reader.Read())
                {
                    // Add the customer details from the current row to the customers list.
                    customers.Add(new string[]{
                        reader["customer_name"].ToString(),
                        reader["customer_phone_number"].ToString(),
                        reader["customer_address"].ToString(),
                        reader["customer_email"].ToString(),
                        reader["customer_gender"].ToString(),
                        reader["customer_dateofbirth"].ToString(),
                        reader["customer_count"].ToString(),
                        reader["customer_totalspent"].ToString()
                    });
                }

                // Display the search results in the window.
                for (int i = 0; i < customers.Count; i++)
                {
                    for (int j = 0; j < customers[i].Length; j++)
                    {
                        findCustomerWindow.Add(new Label(customers[i][j])
                        {
                            X = j * columnWidth,
                            Y = i + 5,
                            Width = columnWidth,
                            Height = 1
                        });
                    }
                }
            }
        };

        // Create a button labeled "Close" to close the find customer window.
        var btnClose = new Button("Close")
        {
            X = Pos.Center(),
            Y = Pos.Percent(100) - 1
        };

        // Define the action to be taken when the close button is clicked.
        btnClose.Clicked += () =>
        {
            // Remove the find customer window and go back to the admin menu.
            top.Remove(findCustomerWindow);
            admin.AdminMenu();
        };

        // Add the close button to the find customer window.
        findCustomerWindow.Add(btnClose);
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
            Title = "Menu",
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
        var rightTopFrame = new FrameView("Welcome")
        {
            X = Pos.Percent(30),
            Y = 0,
            Width = Dim.Fill(),
            Height = Dim.Percent(50)
        };
        userMenu.Add(rightTopFrame);

        // Right bottom frame for displaying top products by order count
        var rightBottomFrame = new FrameView("Top Products in month")
        {
            X = Pos.Percent(30),
            Y = Pos.Percent(50),
            Width = Dim.Fill(),
            Height = Dim.Fill()
        };
        userMenu.Add(rightBottomFrame);

        // Buttons for various menu options
        var btnDisplayProducts = new Button("Display Products")
        {
            X = 2,
            Y = 2,
        };
        // Click event handler for displaying products
        btnDisplayProducts.Clicked += () =>
        {
            try
            {
                top.Remove(userMenu);
                pd.DisplayProduct("user");
            }
            catch (Exception ex)
            {
                MessageBox.ErrorQuery("Error", ex.Message, "OK");
            }
        };

        // Similar setup for other buttons like View Cart, Order, View My Orders, Find Product, and Logout
        

        var btnViewCart = new Button("View Cart")
        {
            X = 2,
            Y = 3,
        };
        btnViewCart.Clicked += () =>
        {
            try
            {
                top.Remove(userMenu); 
                userCart.DisplayCart("user");
            }
            catch (Exception ex)
            {
                MessageBox.ErrorQuery("Error", ex.Message, "OK");
            }
        };

        var btnOrder = new Button("Order")
        {
            X = 2,
            Y = 4,
        };
        btnOrder.Clicked += () =>
        {
            try
            {
                top.Remove(userMenu);
                order.DisplayProductToOrder();
            }
            catch (Exception ex)
            {
                MessageBox.ErrorQuery("Error", ex.Message, "OK");
            }
        };
        var btnViewMyOrders = new Button("View My Orders")
        {
            X = 2,
            Y = 5,
        };
        btnViewMyOrders.Clicked += () =>
        {
            top.Remove(userMenu);
            order.DisplayMyOrder();
        };
        var btnFindProduct = new Button("Find Product")
        {
            X = 2,
            Y = 6
        };
        btnFindProduct.Clicked += () =>
        {
            top.Remove(userMenu);
            pd.FindProduct("user");
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
        leftFrame.Add(btnDisplayProducts, btnViewCart, btnOrder, btnViewMyOrders, btnFindProduct, btnLogout);

        // Retrieve customer's name from the database
        string customerName = "";
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            string query = "SELECT customer_name FROM customers WHERE customer_id = @CustomerID";
            MySqlCommand command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@CustomerID", currentCustomerID);
            connection.Open();
            MySqlDataReader reader = command.ExecuteReader();
            if (reader.Read())
            {
                customerName = reader["customer_name"].ToString();
            }
        }

        // Display customer's name in the right top frame
        var rightTopLabel = new Label(customerName)
        {
            X = 1,
            Y = 1
        };
        rightTopFrame.Add(rightTopLabel);

        // Retrieve top products by order count from the database
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            string query = @"SELECT 
                                p.product_name, 
                                COUNT(o.order_product_id) AS product_count
                            FROM 
                                orders o
                            JOIN 
                                products p ON o.order_product_id = p.product_id
                            GROUP BY 
                                p.product_name
                            ORDER BY 
                                product_count DESC
                            LIMIT 3";
            MySqlCommand command = new MySqlCommand(query, connection);
            connection.Open();
            MySqlDataReader reader = command.ExecuteReader();

            int maxProductCount = 0;
            List<(string ProductName, int ProductCount)> products = new List<(string, int)>();

            // Retrieve and process each product's order count
            while (reader.Read())
            {
                string productName = reader["product_name"].ToString();
                int productCount = Convert.ToInt32(reader["product_count"]);
                products.Add((productName, productCount));
                if (productCount > maxProductCount)
                {
                    maxProductCount = productCount;
                }
            }

            // Display products and their order counts using labels
            int row = 0;
            int yPosition = 1;
            foreach (var product in products)
            {
                string productName = product.ProductName;
                int productCount = product.ProductCount;

                // Calculate bar length based on order count for visual representation
                int barLength = (int)((productCount / (double)maxProductCount) * 30);
                string bar = new string('=', barLength);

                // Create label for each product with formatted text
                var productLabel = new Label($"{productName.PadRight(15)} | {bar} {productCount} orders")
                {
                    X = 1,
                    Y = yPosition
                };
                rightBottomFrame.Add(productLabel);
                yPosition += 2; // Adjust Y position for the next label
                row++;
            }
        }
    }
}

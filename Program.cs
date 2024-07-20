﻿using System;
using System.Text;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;                            //Import namesapce to use function
using System.Threading.Tasks;
using System.Security.Cryptography.X509Certificates;
using Terminal.Gui;
using Microsoft.VisualBasic;

public class Program
{
    // Declare static variables
    public static string connectionString;

    // Initialize lists for various entities
    public static List<Products> ListProducts = new List<Products>();
    public static List<Categories> ListCategories = new List<Categories>();
    public static List<Users> ListUsers = new List<Users>();
    public static List<Customers> ListCustomers = new List<Customers>();
    public static List<Cart> ListCarts = new List<Cart>();

    // Create instances of various entities
    public static Cart userCart = new Cart();
    public static Products pd = new Products();
    public static Orders order = new Orders();
    public static Customers customer = new Customers();
    public static Program program = new Program();
    public static Users user = new Users();
    public static Admin admin = new Admin();
    public static SuperAdmin superadmin = new SuperAdmin();

    // Static constructor to initialize the connection string
    static Program()
    {
        // Prompt user for database password
        Console.Write("Enter the database password: ");
        string password = Console.ReadLine();

        // Base connection string with a placeholder for the password
        string baseConnectionString = "Server=localhost;Database=shop;Uid=root;Pwd=@pass";

        // Replace the placeholder with the actual password
        connectionString = baseConnectionString.Replace("@pass", password);
        Configuration.ConnectionString = connectionString;
    }
    // Main method
    static void Main()
    {
        // Initialize the application and set colors
        Application.Init();

        // Set base colors
        Colors.Base.Normal = Application.Driver.MakeAttribute(Color.White, Color.Black);
        Colors.Base.Focus = Application.Driver.MakeAttribute(Color.White, Color.DarkGray);

        // Set colors for dialogs
        Colors.Dialog.Normal = Application.Driver.MakeAttribute(Color.White, Color.Black);
        Colors.Dialog.Focus = Application.Driver.MakeAttribute(Color.White, Color.DarkGray);
        Colors.Dialog.HotNormal = Application.Driver.MakeAttribute(Color.White, Color.Black);
        Colors.Dialog.HotFocus = Application.Driver.MakeAttribute(Color.White, Color.DarkGray);

        // Set colors for menus
        Colors.Menu.Normal = Application.Driver.MakeAttribute(Color.White, Color.Black);
        Colors.Menu.Focus = Application.Driver.MakeAttribute(Color.White, Color.DarkGray);
        Colors.Menu.HotNormal = Application.Driver.MakeAttribute(Color.White, Color.Black);
        Colors.Menu.HotFocus = Application.Driver.MakeAttribute(Color.White, Color.DarkGray);

        // Set colors for errors
        Colors.Error.Normal = Application.Driver.MakeAttribute(Color.White, Color.Red);
        Colors.Error.Focus = Application.Driver.MakeAttribute(Color.White, Color.DarkGray);

        // Set colors for top level
        Colors.TopLevel.Normal = Application.Driver.MakeAttribute(Color.White, Color.Black);
        Colors.TopLevel.Focus = Application.Driver.MakeAttribute(Color.White, Color.DarkGray);

        // Initialize the application and run the login method
        Application.Init();
        program.Login();
        Application.Run();  
        
    }

    // Login method
    public void Login()
    {
        SessionData.Instance.ResetCustomerID();
        // Create and configure the login window
        var top = Application.Top;
        Application.Init();
        var loginWin = new Window()
        {
            Title = "Login",
            X = 0,
            Y = 0,
            Width = Dim.Fill(),
            Height = Dim.Fill()
        };
        top.Add(loginWin);

        // Create and configure the ASCII art label
        var asciiArt = new Label(@"███████╗██╗     ███████╗ ██████╗████████╗██████╗  ██████╗ ███╗   ██╗██╗ ██████╗    ███████╗██╗  ██╗ ██████╗ ██████╗ 
██╔════╝██║     ██╔════╝██╔════╝╚══██╔══╝██╔══██╗██╔═══██╗████╗  ██║██║██╔════╝    ██╔════╝██║  ██║██╔═══██╗██╔══██╗
█████╗  ██║     █████╗  ██║        ██║   ██████╔╝██║   ██║██╔██╗ ██║██║██║         ███████╗███████║██║   ██║██████╔╝
██╔══╝  ██║     ██╔══╝  ██║        ██║   ██╔══██╗██║   ██║██║╚██╗██║██║██║         ╚════██║██╔══██║██║   ██║██╔═══╝ 
███████╗███████╗███████╗╚██████╗   ██║   ██║  ██║╚██████╔╝██║ ╚████║██║╚██████╗    ███████║██║  ██║╚██████╔╝██║     
╚══════╝╚══════╝╚══════╝ ╚═════╝   ╚═╝   ╚═╝  ╚═╝ ╚═════╝ ╚═╝  ╚═══╝╚═╝ ╚═════╝    ╚══════╝╚═╝  ╚═╝ ╚═════╝ ╚═╝     
                                                                                                                    ")
        {
            X = Pos.Center(),
            Y = 0,
        };

        // Create and configure the username label and field
        var usernameLabel = new Label("Username:")
        {
            X = 2,
            Y = Pos.Bottom(asciiArt) + 1
        };
        var usernameField = new TextField("")
        {
            X = Pos.Right(usernameLabel) + 1,
            Y = Pos.Bottom(asciiArt) + 1,
            Width = Dim.Fill() - 4
        };

        // Create and configure the password label and field
        var passwordLabel = new Label("Password:")
        {
            X = 2,
            Y = Pos.Bottom(usernameLabel) + 1
        };
        var passwordField = new TextField("")
        {
            Secret = true,
            X = Pos.Right(passwordLabel) + 1,
            Y = Pos.Bottom(usernameLabel) + 1,
            Width = Dim.Fill() - 4
        };

        // Create and configure the login button
        var loginButton = new Button("Login")
        {
            X = Pos.Center(),
            Y = Pos.Bottom(passwordLabel) + 1
        };
        loginButton.Clicked += () =>
        {
            try
            {
                string username = usernameField.Text.ToString();
                string password = passwordField.Text.ToString();
                bool isAuthenticated = false;
                string role = "";

                // Authenticate the user
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT user_customer_id, password_hash, role FROM users WHERE username = @Username AND active = TRUE";
                    MySqlCommand command = new MySqlCommand(query, connection);
                    command.Parameters.AddWithValue("@Username", username);

                    MySqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        string storedHash = reader.GetString("password_hash");
                        if (password == storedHash)
                        {
                            isAuthenticated = true;
                            role = reader.GetString("role");
                            int currentCustomerID = reader.GetInt32("user_customer_id");
                            SessionData.Instance.CurrentCustomerID = currentCustomerID;
                        }
                    }
                }

                // Handle authentication result
                if (isAuthenticated)
                {
                    if (role == "user")
                    {
                        MessageBox.Query("Success", "Welcome to our Electronic Shop!", "OK");
                    }
                    else if (role == "admin")
                    {
                        MessageBox.Query("Success", "Welcome staff!", "OK");
                    }
                    else if (role == "Manager")
                    {
                        MessageBox.Query("Success", "Welcome Manager!", "OK");
                    }
                    top.Remove(loginWin);
                    switch (role)
                    {
                        case "user":
                            customer.UserMenu();
                            break;
                        case "admin":
                            admin.AdminMenu();
                            break;
                        case "superadmin":
                            superadmin.SuperAdminMenu();
                            break;
                    }
                }
                else
                {
                    MessageBox.ErrorQuery("Error", "Invalid username or password!", "OK");
                }
            }
            catch (Exception ex)
            {
                MessageBox.ErrorQuery("Error", ex.Message, "OK");
            }
        };

        // Create and configure the register button
        var btnRegister = new Button("Register")
        {
            X = Pos.Center(),
            Y = Pos.Bottom(loginButton) + 1
        };
        btnRegister.Clicked += () =>
        {
            top.Remove(loginWin);
            program.Register();
        };

        // Create and configure the close button
        var closeButton = new Button("Close")
        {
            X = Pos.Center(),
            Y = Pos.Bottom(btnRegister) + 1
        };
        closeButton.Clicked += () =>
        {
            top.Remove(loginWin);
            Application.Shutdown();
        };

        // Add controls to the login window
        loginWin.Add(asciiArt, usernameLabel, usernameField, passwordLabel, passwordField, loginButton, btnRegister, closeButton);
    }


    // Register method
    public void Register()
    {
    // Initialize user and customer objects
    Users us = new Users();
    Customers cus = new Customers();

    // Create the main window for the registration form
    var top = Application.Top;
    var registerWin = new Window()
    {
        Title = $"Register",
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
                    program.Login();
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
            program.Login();
        };

        // Add all controls to the registration window
        registerWin.Add(usernameLabel, usernameField, passwordLabel, passwordField,
                        CustomerNameLabel, CustomerNameField, CustomerPhoneNumberLabel,
                        CustomerPhoneNumberField, CustomerAddressLabel, CustomerAddressField,
                        CustomerEmailLabel, CustomerEmailField, CustomerGenderLabel, CustomerGenderField,
                        CustomerDateOfBirthLabel, CustomerDateOfBirthField, registerButton, closeButton);

    
    }
}

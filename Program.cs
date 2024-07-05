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
    public static int currentCustomerID = SessionData.Instance.CurrentCustomerID;

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

    // Method to load categories from the database
    static List<Categories> LoadCategory(string connectionString)
    {
        List<Categories> ListCategory = new List<Categories>();

        // Establish connection to the database
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {   
            string query = "SELECT * FROM categories"; 
            MySqlCommand command = new MySqlCommand(query, connection);
            connection.Open();
            MySqlDataReader read = command.ExecuteReader();
            while (read.Read())
            {
                Categories c = new Categories();
                // Load category properties from the database
                c.CategoryID = read.GetInt32("category_id");
                c.CategoryName = read.GetString("category_name");
                c.CategoryDescription = read.GetString("category_description");

                ListCategory.Add(c);
            }
        }
        return ListCategory;
    }

    // Main method
    static void Main()
    {
        // Initialize the application and set colors
        Application.Init();
        Colors.Base.Normal = Application.Driver.MakeAttribute(Color.Blue, Color.Black); // blue
        Colors.Base.Focus = Application.Driver.MakeAttribute(Color.White, Color.DarkGray);

        // Set colors for dialogs
        Colors.Dialog.Normal = Application.Driver.MakeAttribute(Color.BrightCyan, Color.Black); // Bright cyan 
        Colors.Dialog.Focus = Application.Driver.MakeAttribute(Color.White, Color.DarkGray);
        Colors.Dialog.HotNormal = Application.Driver.MakeAttribute(Color.Red, Color.Black); // Default colors
        Colors.Dialog.HotFocus = Application.Driver.MakeAttribute(Color.Red, Color.DarkGray); // Default colors

        // Set colors for menus
        Colors.Menu.Normal = Application.Driver.MakeAttribute(Color.White, Color.Blue); // Dark blue 
        Colors.Menu.Focus = Application.Driver.MakeAttribute(Color.Black, Color.Gray);
        Colors.Menu.HotNormal = Application.Driver.MakeAttribute(Color.BrightYellow, Color.Blue); // Yellow 
        Colors.Menu.HotFocus = Application.Driver.MakeAttribute(Color.BrightYellow, Color.Gray);

        // Set colors for errors
        Colors.Error.Normal = Application.Driver.MakeAttribute(Color.Red, Color.White); // Default colors
        Colors.Error.Focus = Application.Driver.MakeAttribute(Color.White, Color.Red); // Default colors

        // Set colors for top level
        Colors.TopLevel.Normal = Application.Driver.MakeAttribute(Color.Magenta, Color.Black); // Magenta 
        Colors.TopLevel.Focus = Application.Driver.MakeAttribute(Color.White, Color.DarkGray);

        // Initialize the application and run the login method
        Application.Init();
        program.Login();
        Application.Run();
    }

    // Login method
    public void Login()
    {
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

        // Create and configure the username label and field
        var usernameLabel = new Label("Username:")
        {
            X = 2,
            Y = 2
        };
        var usernameField = new TextField("")
        {
            X = Pos.Right(usernameLabel) + 1,
            Y = 2,
            Width = Dim.Fill() - 4
        };

        // Create and configure the password label and field
        var passwordLabel = new Label("Password:")
        {
            X = 2,
            Y = 4
        };
        var passwordField = new TextField("")
        {
            Secret = true,
            X = Pos.Right(passwordLabel) + 1,
            Y = 4,
            Width = Dim.Fill() - 4
        };

        // Create and configure the login button
        var loginButton = new Button("Login")
        {
            X = Pos.Center(),
            Y = 6
        };
        loginButton.Clicked += () =>
        {
            string username = usernameField.Text.ToString();
            string password = passwordField.Text.ToString();
            bool isAuthenticated = false;
            string role = "";

            // Authenticate the user
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT user_customer_id, password_hash, role FROM users WHERE username = @Username";
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
                        currentCustomerID = reader.GetInt32("user_customer_id");
                        SessionData.Instance.CurrentCustomerID = currentCustomerID;
                    }
                }
            }

            // Handle authentication result
            if (isAuthenticated)
            {
                MessageBox.Query("Success", $"Welcome {role}!", "OK");
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
        };

        // Create and configure the register button
        var btnRegister = new Button("Register")
        {
            X = Pos.Center(),
            Y = 8
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
            Y = 10
        };
        closeButton.Clicked += () =>
        {
            top.Remove(loginWin);
            Application.Shutdown();
        };

        // Add controls to the login window
        loginWin.Add(usernameLabel, usernameField, passwordLabel, passwordField, loginButton, btnRegister, closeButton);
    }

    // Register method
    public void Register()
    {
        Users us = new Users();
        Customers cus = new Customers();
        var top = Application.Top;
        Application.Init();
        var registerWin = new Window()
        {
            Title = $"Register user",
            X = 0,
            Y = 0,
            Width = Dim.Fill(),
            Height = Dim.Fill()
        };
        top.Add(registerWin);

        // Create and configure registration fields
        var usernameLabel = new Label("Username:")
        {
            X = 2,
            Y = 2
        };
        var usernameField = new TextField("")
        {
            X = Pos.Right(usernameLabel) + 1,
            Y = 2,
            Width = Dim.Fill() - 4
        };

        var passwordLabel = new Label("Password:")
        {
            X = 2,
            Y = 4
        };
        var passwordField = new TextField("")
        {
            Secret = true,
            X = Pos.Right(passwordLabel) + 1,
            Y = 4,
            Width = Dim.Fill() - 4
        };

        var CustomerNameLabel = new Label("Name: ")
        {
            X = 2,
            Y = 6
        };
        var CustomerNameField = new TextField("")
        {
            X = Pos.Right(CustomerNameLabel) + 1,
            Y  = 6,
            Width = Dim.Fill() - 4
        };

        var CustomerPhoneNumberLabel = new Label("Phone number: ")
        {
            X = 2,
            Y = 8
        };
        var CustomerPhoneNumberField = new TextField("")
        {
            X = Pos.Right(CustomerPhoneNumberLabel) + 1,
            Y  = 8,
            Width = Dim.Fill() - 4
        };

        var CustomerAddressLabel = new Label("Address: ")
        {
            X = 2,
            Y = 10
        };
        var CustomerAddressField = new TextField("")
        {
            X = Pos.Right(CustomerAddressLabel) + 1,
            Y  = 10,
            Width = Dim.Fill() - 4
        };

        var roleLabel = new Label("Role:")
        {
            X = 2,
            Y = 12
        };
        var roleField = new TextField("")
        {
            X = Pos.Right(roleLabel) + 1,
            Y = 12,
            Width = Dim.Fill() - 4
        };

        // Create and configure register button
        var registerButton = new Button("Register")
        {
            X = Pos.Center(),
            Y = 14
        };
        registerButton.Clicked += () =>
        {
            string username = usernameField.Text.ToString();
            string password = passwordField.Text.ToString();
            string role = roleField.Text.ToString();
            string name = CustomerNameField.Text.ToString();
            string phonenumber = CustomerPhoneNumberField.Text.ToString();
            string address = CustomerAddressField.Text.ToString();

            // Insert new user and customer into the database
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                using (MySqlCommand command = new MySqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = "INSERT INTO customers (customer_name, customer_phonenumber, customer_address) VALUES (@Name, @PhoneNumber, @Address); SELECT LAST_INSERT_ID();";
                    command.Parameters.AddWithValue("@Name", name);
                    command.Parameters.AddWithValue("@PhoneNumber", phonenumber);
                    command.Parameters.AddWithValue("@Address", address);
                    int customerId = Convert.ToInt32(command.ExecuteScalar());

                    command.CommandText = "INSERT INTO users (username, password_hash, role, user_customer_id) VALUES (@Username, @Password, @Role, @CustomerId)";
                    command.Parameters.AddWithValue("@Username", username);
                    command.Parameters.AddWithValue("@Password", password);
                    command.Parameters.AddWithValue("@Role", role);
                    command.Parameters.AddWithValue("@CustomerId", customerId);
                    command.ExecuteNonQuery();
                }
            }

            // Show success message and return to login screen
            MessageBox.Query("Success", $"Registration successful!", "OK");
            top.Remove(registerWin);
            program.Login();
        };

        // Create and configure close button
        var closeButton = new Button("Close")
        {
            X = Pos.Center(),
            Y = 16
        };
        closeButton.Clicked += () =>
        {
            top.Remove(registerWin);
            Application.Shutdown();
        };

        // Add controls to the registration window
        registerWin.Add(usernameLabel, usernameField, passwordLabel, passwordField, CustomerNameLabel, CustomerNameField, CustomerPhoneNumberLabel, CustomerPhoneNumberField, CustomerAddressLabel, CustomerAddressField, roleLabel, roleField, registerButton, closeButton);
    }
}

using System;
using System.Text;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Security.Cryptography.X509Certificates;
using Terminal.Gui;
public class Users
{
    public int UserId { get; set; }
    public string Username { get; set; }
    public string PasswordHash { get; set; }
    public string Roles { get; set; }   
    public int CustomerID  {get; set;}   

    public static List<Products> ListProducts = new List<Products>();
    public static List<Categories> ListCategories = new List<Categories>();
    public static List<Users> ListUsers = new List<Users>();
    public static List<Customers> ListCustomers = new List<Customers>();
    public static List<Cart> ListCarts = new List<Cart>();

    public static string connectionString;

    public static Cart userCart = new Cart();
    public static Products pd = new Products();
    public static Orders order = new Orders();
    public static Customers customer = new Customers();
    public static Program program = new Program();
    public static Users user = new Users();
    public static Admin admin = new Admin();
    public static SuperAdmin superadmin = new SuperAdmin();

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

                        // Use a method to compare the provided password with the stored hash
                        if (VerifyPassword(password, storedHash))
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
                    string welcomeMessage = role switch
                    {
                        "user" => "Welcome to our Electronic Shop!",
                        "admin" => "Welcome staff!",
                        "Manager" => "Welcome Manager!",
                        _ => "Welcome!"
                    };

                    MessageBox.Query("Success", welcomeMessage, "OK");
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
            catch
            {
                MessageBox.ErrorQuery("Error", "An error occurred during the login process. Please try again.", "OK");
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
            user.Register();
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

    // Method to verify the password (example using a simple hash comparison)
    private bool VerifyPassword(string inputPassword, string storedHash)
    {
        // Implement password hash verification logic here
        // This is just an example; you should use a secure hashing algorithm and comparison
        return inputPassword == storedHash;
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
            Title = "Register",
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

        var customerNameLabel = new Label("Name:")
        {
            X = 2,
            Y = 6
        };
        var customerNameField = new TextField("")
        {
            X = 31,
            Y = 6,
            Width = 100
        };

        var customerPhoneNumberLabel = new Label("Phone number:")
        {
            X = 2,
            Y = 8
        };
        var customerPhoneNumberField = new TextField("")
        {
            X = 31,
            Y = 8,
            Width = 100
        };

        var customerAddressLabel = new Label("Address:")
        {
            X = 2,
            Y = 10
        };
        var customerAddressField = new TextField("")
        {
            X = 31,
            Y = 10,
            Width = 100
        };

        var customerEmailLabel = new Label("Email:")
        {
            X = 2,
            Y = 12
        };
        var customerEmailField = new TextField("")
        {
            X = 31,
            Y = 12,
            Width = 100
        };

        var customerGenderLabel = new Label("Gender:")
        {
            X = 2,
            Y = 14
        };
        var customerGenderField = new TextField("")
        {
            X = 31,
            Y = 14,
            Width = 100
        };

        var customerDateOfBirthLabel = new Label("Date of birth (DD-MM-YYYY):")
        {
            X = 2,
            Y = 16
        };
        var customerDateOfBirthField = new TextField("")
        {
            X = 31,
            Y = 16,
            Width = 100
        };

        var registerButton = new Button("Register")
        {
            X = Pos.Center(),
            Y = Pos.Bottom(customerDateOfBirthField) + 1
        };

        // Define the click event for the "Register" button
        registerButton.Clicked += () =>
        {
            // Validate input values
            if (string.IsNullOrWhiteSpace(usernameField.Text.ToString()) ||
                string.IsNullOrWhiteSpace(passwordField.Text.ToString()) ||
                string.IsNullOrWhiteSpace(customerNameField.Text.ToString()) ||
                string.IsNullOrWhiteSpace(customerPhoneNumberField.Text.ToString()) ||
                string.IsNullOrWhiteSpace(customerAddressField.Text.ToString()) ||
                string.IsNullOrWhiteSpace(customerEmailField.Text.ToString()) ||
                string.IsNullOrWhiteSpace(customerGenderField.Text.ToString()) ||
                string.IsNullOrWhiteSpace(customerDateOfBirthField.Text.ToString()))
            {
                MessageBox.ErrorQuery("Error", "All fields must be filled.", "OK");
                return;
            }

            if (!DateTime.TryParseExact(customerDateOfBirthField.Text.ToString(), "dd-MM-yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime customerDateOfBirth))
            {
                MessageBox.ErrorQuery("Error", "Invalid date format. Please use DD-MM-YYYY.", "OK");
                return;
            }

            if (!IsValidEmail(customerEmailField.Text.ToString()))
            {
                MessageBox.ErrorQuery("Error", "Invalid email format.", "OK");
                return;
            }

            // Assign input values to user and customer objects
            us.Username = usernameField.Text.ToString();
            us.PasswordHash = passwordField.Text.ToString(); // Consider hashing password before storing
            cus.CustomerName = customerNameField.Text.ToString();
            cus.CustomerPhone = customerPhoneNumberField.Text.ToString();
            cus.CustomerAddress = customerAddressField.Text.ToString();
            cus.CustomerEmail = customerEmailField.Text.ToString();
            cus.CustomerGender = customerGenderField.Text.ToString();
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
                                            "VALUES (@CustomerName, @CustomerPhoneNumber, @CustomerAddress, @CustomerEmail, @CustomerGender, @CustomerDateOfBirth, 0, 0)";
                    MySqlCommand customerCommand = new MySqlCommand(customerQuery, connection, transaction);
                    customerCommand.Parameters.AddWithValue("@CustomerName", cus.CustomerName);
                    customerCommand.Parameters.AddWithValue("@CustomerPhoneNumber", cus.CustomerPhone);
                    customerCommand.Parameters.AddWithValue("@CustomerAddress", cus.CustomerAddress);
                    customerCommand.Parameters.AddWithValue("@CustomerEmail", cus.CustomerEmail);
                    customerCommand.Parameters.AddWithValue("@CustomerGender", cus.CustomerGender);
                    customerCommand.Parameters.AddWithValue("@CustomerDateOfBirth", cus.CustomerDateOfBirth);
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
                    
                    // Close the registration window and go back to login
                    top.Remove(registerWin);
                    user.Login();
                }
                catch
                {
                    transaction.Rollback();
                    MessageBox.ErrorQuery("Error", "An error occurred during registration. Please try again.", "OK");
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
            user.Login();
        };

        // Add all controls to the registration window
        registerWin.Add(usernameLabel, usernameField, passwordLabel, passwordField,
                        customerNameLabel, customerNameField, customerPhoneNumberLabel, customerPhoneNumberField,
                        customerAddressLabel, customerAddressField, customerEmailLabel, customerEmailField,
                        customerGenderLabel, customerGenderField, customerDateOfBirthLabel, customerDateOfBirthField,
                        registerButton, closeButton);
    }
    // Helper method to validate email format
    private bool IsValidEmail(string email)
    {
        try
        {
            var mailAddress = new System.Net.Mail.MailAddress(email);
            return mailAddress.Address == email;
        }
        catch
        {
            return false;
        }
    }
}

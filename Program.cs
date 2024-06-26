using System;
using System.Text;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Security.Cryptography.X509Certificates;
using Terminal.Gui;
using Microsoft.VisualBasic;


public class Program
{
    static Window mainMenu;
    public static string connectionString;
    public static int currentCustomerID = SessionData.Instance.CurrentCustomerID;
    public static List<Products> ListProducts = new List<Products>();
    public static List<Categories> ListCategories = new List<Categories>();
    public static List<Users> ListUsers = new List<Users>();
    public static List<Customers> ListCustomers = new List<Customers>();
    public static List<Cart> ListCarts = new List<Cart>();
    public static Cart userCart = new Cart();
    public static Products pd = new Products();
    public static Orders order = new Orders();
    public static Customers customer = new Customers();
    public static Program program = new Program();
    public static Users user = new Users();
    public static Admin admin = new Admin();
    
    public static SuperAdmin superadmin = new SuperAdmin();
    static Program()
    {
        // Hỏi mật khẩu từ người dùng
        Console.Write("Enter the database password: ");
       string password = Console.ReadLine();

        // Gốc chuỗi kết nối với placeholder @pass
        string baseConnectionString = "Server=localhost;Database=shop;Uid=root;Pwd=@pass";

        // Thay thế @pass bằng mật khẩu thực
        connectionString = baseConnectionString.Replace("@pass", password);
        Configuration.ConnectionString = connectionString;
    }


    static List<Categories> LoadCategory(string connectionString)
    {
        List<Categories> ListCategory = new List<Categories>();

        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {   
            string query = "SELECT * FROM categories"; 
            MySqlCommand command = new MySqlCommand(query, connection);
            connection.Open();
            MySqlDataReader read = command.ExecuteReader();
            while (read.Read())
            {
                Categories c = new Categories();
                // Nạp các thuộc tính 
                c.CategoryID = read.GetInt32("category_id");
                c.CategoryName = read.GetString("category_name");
                c.CategoryDescription = read.GetString("category_description");

                ListCategory.Add(c);
            }
        }
        return ListCategory;
    }


    static void Main()
    {
        Application.Init();
        Colors.Base.Normal = Application.Driver.MakeAttribute(Color.BrightGreen, Color.Black);
        Colors.Base.Focus = Application.Driver.MakeAttribute(Color.White, Color.DarkGray);

        // Thiết lập màu sắc cho Dialog
        Colors.Dialog.Normal = Application.Driver.MakeAttribute(Color.Cyan, Color.Black);
        Colors.Dialog.Focus = Application.Driver.MakeAttribute(Color.White, Color.DarkGray);
        Colors.Dialog.HotNormal = Application.Driver.MakeAttribute(Color.Red, Color.Black);
        Colors.Dialog.HotFocus = Application.Driver.MakeAttribute(Color.Red, Color.DarkGray);

        // Thiết lập màu sắc cho Menu
        Colors.Menu.Normal = Application.Driver.MakeAttribute(Color.White, Color.Blue);
        Colors.Menu.Focus = Application.Driver.MakeAttribute(Color.Black, Color.Gray);
        Colors.Menu.HotNormal = Application.Driver.MakeAttribute(Color.BrightYellow, Color.Blue);
        Colors.Menu.HotFocus = Application.Driver.MakeAttribute(Color.BrightYellow, Color.Gray);

        // Thiết lập màu sắc cho Error
        Colors.Error.Normal = Application.Driver.MakeAttribute(Color.Red, Color.White);
        Colors.Error.Focus = Application.Driver.MakeAttribute(Color.White, Color.Red);

        // Thiết lập màu sắc cho TopLevel
        Colors.TopLevel.Normal = Application.Driver.MakeAttribute(Color.BrightMagenta, Color.Black);
        Colors.TopLevel.Focus = Application.Driver.MakeAttribute(Color.White, Color.DarkGray);

        
        Application.Init();
        program.Login();
        Application.Run();
    }
    public void Login()
    {
        var top = Application.Top;
        var loginWin = new Window()
        {
            Title = "Login",
            X = 0,
            Y = 0,
            Width = Dim.Fill(),
            Height = Dim.Fill()
        };
        top.Add(loginWin);

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

        var closeButton = new Button("Close")
        {
            X = Pos.Center(),
            Y = Pos.Bottom(loginButton) + 1
        };
        closeButton.Clicked += () =>
        {
            top.Remove(loginWin);
            Application.Shutdown();
        };

        loginWin.Add(usernameLabel, usernameField, passwordLabel, passwordField, loginButton, closeButton);
    }

    static void RegisterUser()
    {
        Register("user");
    }

    static void RegisterAdmin()
    {
        Register("admin");
    }

    static void RegisterSuperAdmin()
    {
        Register("superadmin");
    }

    static void Register(string role)
    {
        Users  us = new Users();
        Customers cus = new Customers();
        var top = Application.Top;
        var registerWin = new Window()
        {
            Title = $"Register {role}",
            X = 0,
            Y = 0,
            Width = Dim.Fill(),
            Height = Dim.Fill()
        };
        top.Add(registerWin);

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
        var CustomerEmailLabel = new Label("Email: ")
        {
            X = 2,
            Y = 12
        };
        var CustomerEmailField = new TextField("")
        {
            X = Pos.Right(CustomerEmailLabel) + 1,
            Y  = 12,
            Width = Dim.Fill() - 4
        };
        var CustomerGenderLabel = new Label("Gender: ")
        {
            X = 2,
            Y = 14
        };
        var CustomerGenderField = new TextField("")
        {
            X = Pos.Right(CustomerGenderLabel) + 1,
            Y  = 14,
            Width = Dim.Fill() - 4
        };
        var CustomerDateOfBirthLabel = new Label("Date of birth (YYYY-MM-DD): ")
        {
            X = 2,
            Y = 16
        };
        var CustomerDateOfBirthField = new TextField("")
        {
            X = Pos.Right(CustomerDateOfBirthLabel) + 1,
            Y  = 16,
            Width = Dim.Fill() - 4
        };

        var registerButton = new Button("Register")
        {
            X = Pos.Center(),
            Y = Pos.Bottom(CustomerDateOfBirthField),
        };
        registerButton.Clicked += () =>
        {
            us.Username = usernameField.Text.ToString();
            us.PasswordHash = passwordField.Text.ToString();
            cus.CustomerName = CustomerNameField.Text.ToString();
            cus.CustomerPhone = CustomerPhoneNumberField.Text.ToString();
            cus.CustomerAddress = CustomerAddressField.Text.ToString();
            cus.CustomerEmail = CustomerEmailField.Text.ToString();
            cus.CustomerGender = CustomerGenderField.Text.ToString();
            
            DateTime customerDateOfBirth;
            if (!DateTime.TryParse(CustomerDateOfBirthField.Text.ToString(), out customerDateOfBirth))
            {
                MessageBox.ErrorQuery("Error", "Invalid date format. Please use YYYY-MM-DD.", "OK");
                return;
            }
            cus.CustomerDateOfBirth = customerDateOfBirth;

            using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            connection.Open();
            MySqlTransaction transaction = connection.BeginTransaction();
            try
            {
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

                long customerId = customerCommand.LastInsertedId;
                string userQuery = "INSERT INTO users (username, password_hash, role, user_customer_id) VALUES (@Username, @PasswordHash, @Role, @CustomerId)";
                MySqlCommand userCommand = new MySqlCommand(userQuery, connection, transaction);
                userCommand.Parameters.AddWithValue("@Username", us.Username);
                userCommand.Parameters.AddWithValue("@PasswordHash", us.PasswordHash);
                userCommand.Parameters.AddWithValue("@Role", role);
                userCommand.Parameters.AddWithValue("@CustomerId", customerId);
                userCommand.ExecuteNonQuery();

                transaction.Commit();

                ListUsers.Add(us);
                ListCustomers.Add(cus);
                MessageBox.Query("Success", "Registration successful!", "OK");
                Application.RequestStop();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                MessageBox.ErrorQuery("Error", ex.Message, "OK");
            }
        }
    };


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

        registerWin.Add(usernameLabel, usernameField, passwordLabel, passwordField,
                        CustomerNameLabel, CustomerNameField, CustomerPhoneNumberLabel,
                        CustomerPhoneNumberField, CustomerAddressLabel, CustomerAddressField,
                        CustomerEmailLabel, CustomerEmailField, CustomerGenderLabel, CustomerGenderField,
                        CustomerDateOfBirthLabel, CustomerDateOfBirthField, registerButton, closeButton);

    }
  }
using System;
using System.Text;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Security.Cryptography.X509Certificates;
using Terminal.Gui;


public class Program
{
    static Window mainMenu;
    static Window productMenu;
    static Window customerMenu;
    static Window displayProductWindow;
    static Window addProductWin;
    static Window editProductWin;
    static Window deleteProductWin;
    static Window findProductWin;
    static Window displayCustomerWindow;
    static Window addCustomerWin;
    static Window DisplayProductToOrderWin;
    public static string connectionString;
    public static List<Products> ListProducts = new List<Products>();
    public static List<Categories> ListCategories = new List<Categories>();
    public static List<Users> ListUsers = new List<Users>();
    public static List<Customers> ListCustomers = new List<Customers>();
    public static List<Cart> ListCarts = new List<Cart>();
    public static Cart userCart = new Cart();
    public static Products pd = new Products();
    public static Orders order = new Orders();
    public static Program program = new Program();
    
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

public void MainMenu()
    {
        var top = Application.Top;

        mainMenu = new Window("Main Menu")
        {
            X = 0,
            Y = 0,
            Width = Dim.Fill(),
            Height = Dim.Fill()
        };
        top.Add(mainMenu);

        var btnCustomer = new Button("Customers")
        {
            X = 2,
            Y = 2,
        };
        btnCustomer.Clicked += () =>
        {
            try
            {
                // Xử lý sự kiện cho nút Customers
            }
            catch (Exception ex)
            {
                MessageBox.ErrorQuery("Error", ex.Message, "OK");
            }
        };

        var btnProduct = new Button("Products")
        {
            X = 2,
            Y = 3
        };
        btnProduct.Clicked += () =>
        {
            try
            {
                top.Remove(mainMenu);
                pd.ProductMenu();
            }
            catch (Exception ex)
            {
                MessageBox.ErrorQuery("Error", ex.Message, "OK");
            }
        };

        var btnCategories = new Button("Categories")
        {
            X = 2,
            Y = 4
        };
        btnCategories.Clicked += () =>
        {
            try
            {
                // Xử lý sự kiện cho nút Categories
            }
            catch (Exception ex)
            {
                MessageBox.ErrorQuery("Error", ex.Message, "OK");
            }
        };

        var btnOrder = new Button("Orders")
        {
            X = 2,
            Y = 5,
        };
        btnOrder.Clicked += () =>
        {
            try
            {
                // Xử lý sự kiện cho nút Orders
            }
            catch (Exception ex)
            {
                MessageBox.ErrorQuery("Error", ex.Message, "OK");
            }
        };

        var btnClose = new Button("Close")
        {
            X = Pos.Center(),
            Y = Pos.Percent(100) - 1
        };
        btnClose.Clicked += () =>
        {
            Application.RequestStop();
        };

        mainMenu.Add(btnCustomer, btnProduct, btnCategories, btnOrder, btnClose);
    }

//Products
  

  //Customer
    
    // Cart

//Order
    static void Main()
    {
        Application.Init();
        Colors.Base.Normal = Application.Driver.MakeAttribute(Color.BrightGreen, Color.Black);
        Colors.Base.Focus = Application.Driver.MakeAttribute(Color.White, Color.DarkGray);

        
        Application.Init();
        UserMenu();
        Application.Run();
    }
    static void Login()
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
                string query = "SELECT customer_id, password_hash, role FROM users WHERE username = @Username";
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
                        int currentCustomerID = reader.GetInt32("customer_id");
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
                        UserMenu();
                        break;
                    case "admin":
                        AdminMenu();
                        break;
                    case "superadmin":
                        SuperAdminMenu();
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
            program.MainMenu();
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
                string customerquery ="INSERT INTO customers (customer_name, customer_phone_number, customer_address, customer_email, customer_gender, customer_dateofbirth, customer_count, customer_totalspent)"+
                                    "VALUES (@customername, @customerphonenumber, @customeraddress, @customeremail, @customergender, @customerdateofbirth, 0, 0)";
                MySqlCommand customercommand = new MySqlCommand(customerquery, connection);
                customercommand.Parameters.AddWithValue("@customername", cus.CustomerName);
                customercommand.Parameters.AddWithValue("@customerphonenumber", cus.CustomerPhone);
                customercommand.Parameters.AddWithValue("@customeraddress",cus.CustomerAddress);
                customercommand.Parameters.AddWithValue("@customeremail", cus.CustomerEmail);
                customercommand.Parameters.AddWithValue("@customergender", cus.CustomerGender);
                customercommand.Parameters.AddWithValue("@customerdateofbirth", cus.CustomerDateOfBirth);
                customercommand.ExecuteNonQuery();

                long customerId = customercommand.LastInsertedId;
                string userquery = "INSERT INTO users (username, password_hash, role, customer_id) VALUES (@Username, @PasswordHash, @Role, @CustomerId)";
                MySqlCommand usercommand = new MySqlCommand(userquery, connection);
                usercommand.Parameters.AddWithValue("@Username", us.Username);
                usercommand.Parameters.AddWithValue("@PasswordHash", us.PasswordHash);
                usercommand.Parameters.AddWithValue("@Role", role);
                usercommand.Parameters.AddWithValue("@CustomerId", customerId);

                try
                {
                    usercommand.ExecuteNonQuery();
                    ListUsers.Add(us);
                    ListCustomers.Add(cus);
                    MessageBox.Query("Success", "Registration successful!", "OK");
                    Application.RequestStop();
                }
                catch (Exception ex)
                {
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
            program.MainMenu();
        };

        registerWin.Add(usernameLabel, usernameField, passwordLabel, passwordField,
                        CustomerNameLabel, CustomerNameField, CustomerPhoneNumberLabel,
                        CustomerPhoneNumberField, CustomerAddressLabel, CustomerAddressField,
                        CustomerEmailLabel, CustomerEmailField, CustomerGenderLabel, CustomerGenderField,
                        CustomerDateOfBirthLabel, CustomerDateOfBirthField, registerButton, closeButton);

    }

   static void UserMenu()
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
        // Tạo cửa sổ chính
        var top = Application.Top;

        var userMenu = new Window()
        {
            Title = "User Menu",
            X = 0,
            Y = 0,
            Width = Dim.Fill(),
            Height = Dim.Fill()
        };
        top.Add(userMenu);

        // Tạo FrameView bên trái chứa các nút
        var leftFrame = new FrameView("Left Frame")
        {
            X = 0,
            Y = 0,
            Width = Dim.Percent(30), // Chiếm 30% chiều rộng cửa sổ
            Height = Dim.Fill() // Chiếm toàn bộ chiều cao
        };
        userMenu.Add(leftFrame);

        // Tạo FrameView bên phải trên
        var rightTopFrame = new FrameView("Right Top Frame")
        {
            X = Pos.Percent(30), // Bắt đầu từ vị trí chiếm 30% chiều rộng cửa sổ
            Y = 0,
            Width = Dim.Fill(), // Chiếm toàn bộ phần còn lại của chiều rộng
            Height = Dim.Percent(50) // Chiếm 50% chiều cao
        };
        userMenu.Add(rightTopFrame);

        // Tạo FrameView bên phải dưới
        var rightBottomFrame = new FrameView("Right Bottom Frame")
        {
            X = Pos.Percent(30), // Bắt đầu từ vị trí chiếm 30% chiều rộng cửa sổ
            Y = Pos.Percent(50), // Bắt đầu từ giữa chiều cao
            Width = Dim.Fill(), // Chiếm toàn bộ phần còn lại của chiều rộng
            Height = Dim.Fill() // Chiếm phần còn lại của chiều cao
        };
        userMenu.Add(rightBottomFrame);

        // Thêm các nút vào Left FrameView
        var btnViewProducts = new Button("View Products")
        {
            X = 2,
            Y = 2,
        };
        btnViewProducts.Clicked += () =>
        {
            try
            {
                top.Remove(userMenu);
                pd.ProductMenu();
            }
            catch (Exception ex)
            {
                MessageBox.ErrorQuery("Error", ex.Message, "OK");
            }
        };

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
                userCart.DisplayCart();
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

        var btnLogout = new Button("Logout")
        {
            X = 2,
            Y = 5,
        };
        btnLogout.Clicked += () =>
        {
            top.Remove(userMenu);
            Login();
        };

        leftFrame.Add(btnViewProducts, btnViewCart, btnOrder, btnLogout);

        // Thêm một nhãn vào Right Top FrameView
        var rightTopLabel = new Label("Hello, Right Top Frame!")
        {
            X = 1,
            Y = 1
        };
        rightTopFrame.Add(rightTopLabel);

        // Thêm một nhãn vào Right Bottom FrameView
        var rightBottomLabel = new Label("Hello, Right Bottom Frame!")
        {
            X = 1,
            Y = 1
        };
        rightBottomFrame.Add(rightBottomLabel);
    }
    static void AdminMenu()
    {
        var top = Application.Top;
        var adminMenu = new Window()
        {
            Title = "Admin Menu",
            X = 0,
            Y = 0,
            Width = Dim.Fill(),
            Height = Dim.Fill()
        };
        top.Add(adminMenu);

        var closeButton = new Button("Close")
        {
            X = Pos.Center(),
            Y = Pos.Percent(100) - 3
        };
        closeButton.Clicked += () => 
        {
            top.Remove(adminMenu);
            program.MainMenu();
        };
        adminMenu.Add(closeButton);
    }

    static void SuperAdminMenu()
    {
        var top = Application.Top;
        var superAdminMenu = new Window()
        {
            Title = "Super Admin Menu",
            X = 0,
            Y = 0,
            Width = Dim.Fill(),
            Height = Dim.Fill()
        };
        top.Add(superAdminMenu);

        var closeButton = new Button("Close")
        {
            X = Pos.Center(),
            Y = Pos.Percent(100) - 3
        };
        closeButton.Clicked += () => 
        {
            top.Remove(superAdminMenu);
            program.MainMenu();
        };
        superAdminMenu.Add(closeButton);

    }
  }
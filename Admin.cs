using System;
using System.Text;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Security.Cryptography.X509Certificates;
using Terminal.Gui;

public class Admin
{
    public int AdminID { get; set; }
    public string AdminName { get; set; }
    public string AdminPhone{ get; set; }
    public string AdminEmail { get; set; }
    public string AdminGender { get; set; }

    public static Cart userCart = new Cart();
    public static Products pd = new Products();
    public static Orders order = new Orders();
    public static Customers customer = new Customers();
    public static Users user = new Users();
    public static SuperAdmin superadmin = new SuperAdmin();
    public static Program program = new Program();
    public static string connectionString = Configuration.ConnectionString;
    public static int currentCustomerID = SessionData.Instance.CurrentCustomerID;
    
    public static List<Users> ListUsers = new List<Users>();
    public static List<Admin> ListAdmin = new List<Admin>();
    public void AdminMenu()
    {
        Application.Init();
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

        var leftFrame = new FrameView("Function")
        {
            X = 0,
            Y = 0,
            Width = Dim.Percent(30), // Chiếm 30% chiều rộng cửa sổ
            Height = Dim.Fill() // Chiếm toàn bộ chiều cao
        };
        adminMenu.Add(leftFrame);

        var rightTopFrame = new FrameView("Welcome")
        {
            X = Pos.Percent(30), // Bắt đầu từ vị trí chiếm 30% chiều rộng cửa sổ
            Y = 0,
            Width = Dim.Fill(), // Chiếm toàn bộ phần còn lại của chiều rộng
            Height = Dim.Percent(50) // Chiếm 50% chiều cao
        };
        adminMenu.Add(rightTopFrame);

        var rightBottomFrame = new FrameView("Number of products sold")
        {
            X = Pos.Percent(30), // Bắt đầu từ vị trí chiếm 30% chiều rộng cửa sổ
            Y = Pos.Percent(50), // Bắt đầu từ giữa chiều cao
            Width = Dim.Fill(), // Chiếm toàn bộ phần còn lại của chiều rộng
            Height = Dim.Fill() // Chiếm phần còn lại của chiều cao
        };
        adminMenu.Add(rightBottomFrame);
        var btnDisplayProduct = new Button("Order for customer")
        {
            X = 2,
            Y = 2
        };
        btnDisplayProduct.Clicked += () =>
        {
            top.Remove(adminMenu);
            order.DisplayProductToOrderForCustomer();
        };
        var btnOrderForCustomer = new Button("Display Products")
        {
            X = 2,
            Y = 4
        };
        btnOrderForCustomer.Clicked += () =>
        {
            top.Remove(adminMenu);
            pd.DisplayProduct("admin");
        };
        var btnDisplayCustomer = new Button("Display Customer")
        {
            X = 2,
            Y = 6
        }; 
        btnDisplayCustomer.Clicked += () =>
        {
            top.Remove(adminMenu);
            customer.DisplayCustomers();
        };
        var btnFindCustomer = new Button("Find Customer")
        {
            X = 2,
            Y = 8
        };
        btnFindCustomer.Clicked += () =>
        {
            top.Remove(adminMenu);
            customer.FindCustomer();
        };
        var btnEditCustomer = new Button("Edit Customer")
        {
            X = 2,
            Y = 10
        };
        btnEditCustomer.Clicked += () =>
        {
            top.Remove(adminMenu);
            customer.EditCustomer();
        };
        var btnAddCustomer = new Button("Add New Customer")
        {
            X = 2,
            Y = 12
        };
        btnAddCustomer.Clicked += () =>
        {
            top.Remove(adminMenu);
            customer.AddCustomer();
        };
        var btnDeleteCustomer = new Button("Delete Customer")
        {
            X = 2,
            Y = 14
        };
        btnDeleteCustomer.Clicked += () =>
        {
            top.Remove(adminMenu);
            customer.DeleteCustomer();
        };

        var LogoutButton = new Button("Logout")
        {
            X = Pos.Center(),
            Y = Pos.Percent(100) - 3
        };
        LogoutButton.Clicked += () => 
        {
            top.Remove(adminMenu);
            program.Login();
        };
        leftFrame.Add(btnDisplayProduct, btnOrderForCustomer, btnDisplayCustomer, btnFindCustomer, btnEditCustomer, btnAddCustomer, btnDeleteCustomer, LogoutButton);

        string adminName = "";
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            string query = "SELECT admin_name FROM admins WHERE admin_id = @AdminID";
            MySqlCommand command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@AdminID", currentCustomerID);
            connection.Open();
            MySqlDataReader reader = command.ExecuteReader();
            if (reader.Read())
            {
                AdminName = reader["admin_name"].ToString();
            }
        }

        var rightTopLabel = new Label(AdminName)
        {
            X = 1,
            Y = 1
        };
        rightTopFrame.Add(rightTopLabel);
        using(MySqlConnection connection = new MySqlConnection(connectionString))
        {
            connection.Open();
            string query = "SELECT COUNT(*) FROM orders";
            MySqlCommand command = new MySqlCommand(query, connection);
            object result = command.ExecuteScalar(); // Thực thi truy vấn và lấy kết quả
            int count = Convert.ToInt32(result);
            var countOrder = new Label($"Total orders: {count}")
            {
                X = 1 ,
                Y = 1
            };
        rightBottomFrame.Add(countOrder);
        }
    }
    public void AddStaff()
    {
        Users  us = new Users();
        Admin ad = new Admin();
        var top = Application.Top;
        var registerWin = new Window()
        {
            Title = $"Register for staff",
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
        var AdminNameLabel = new Label("Name: ")
        {
            X = 2,
            Y = 6
        };
        var AdminNameField = new TextField("")
        {
            X = Pos.Right(AdminNameLabel) + 1,
            Y  = 6,
            Width = Dim.Fill() - 4
        };
        var AdminPhoneNumberLabel = new Label("Phone number: ")
        {
            X = 2,
            Y = 8
        };
        var AdminPhoneNumberField = new TextField("")
        {
            X = Pos.Right(AdminPhoneNumberLabel) + 1,
            Y  = 8,
            Width = Dim.Fill() - 4
        };
        
        var AdminEmailLabel = new Label("Email: ")
        {
            X = 2,
            Y = 12
        };
        var AdminEmailField = new TextField("")
        {
            X = Pos.Right(AdminEmailLabel) + 1,
            Y  = 12,
            Width = Dim.Fill() - 4
        };
        var AdminGenderLabel = new Label("Gender: ")
        {
            X = 2,
            Y = 14
        };
        var AdminGenderField = new TextField("")
        {
            X = Pos.Right(AdminGenderLabel) + 1,
            Y  = 14,
            Width = Dim.Fill() - 4
        };
        

        var registerButton = new Button("Register")
        {
            X = Pos.Center(),
            Y = Pos.Bottom(AdminGenderField),
        };
        registerButton.Clicked += () =>
        {
            us.Username = usernameField.Text.ToString();
            us.PasswordHash = passwordField.Text.ToString();
            ad.AdminName = AdminNameField.Text.ToString();
            ad.AdminPhone = AdminPhoneNumberField.Text.ToString();
            ad.AdminEmail = AdminEmailField.Text.ToString();
            ad.AdminGender = AdminGenderField.Text.ToString();
            
            

            using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            connection.Open();
            MySqlTransaction transaction = connection.BeginTransaction();
            try
            {
                string adminQuery = "INSERT INTO admins (admin_name, admin_phone, admin_email, admin_gender)" +
                                       "VALUES (@adminname, @adminphonenumber, @adminemail, @admingender)";
                MySqlCommand adminCommand = new MySqlCommand(adminQuery, connection, transaction);
                adminCommand.Parameters.AddWithValue("@adminname", ad.AdminName);
                adminCommand.Parameters.AddWithValue("@adminphonenumber", ad.AdminPhone);
                adminCommand.Parameters.AddWithValue("@adminemail", ad.AdminEmail);
                adminCommand.Parameters.AddWithValue("@admingender", ad.AdminGender);
                adminCommand.ExecuteNonQuery();

                long adminId = adminCommand.LastInsertedId;
                string userQuery = "INSERT INTO users (username, password_hash, role, user_customer_id) VALUES (@Username, @PasswordHash, admin, @CustomerId)";
                MySqlCommand userCommand = new MySqlCommand(userQuery, connection, transaction);
                userCommand.Parameters.AddWithValue("@Username", us.Username);
                userCommand.Parameters.AddWithValue("@PasswordHash", us.PasswordHash);
                userCommand.Parameters.AddWithValue("@CustomerId", adminId);
                userCommand.ExecuteNonQuery();

                transaction.Commit();

                ListUsers.Add(us);
                ListAdmin.Add(ad);
                MessageBox.Query("Success", "Registration successful!", "OK");
                
                top.Remove(registerWin);
                superadmin.SuperAdminMenu();
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
                        AdminNameLabel, AdminNameField, AdminPhoneNumberLabel,
                        AdminPhoneNumberField,
                        AdminEmailLabel, AdminEmailField, AdminGenderLabel, AdminGenderField,
                        registerButton, closeButton);

    
}
    public void RegisterAdmin()
    {
        program.Register("admin");
    }
}

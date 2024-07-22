using System;
using System.Text;
using System.Data;
using System.Collections.Generic;     
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Security.Cryptography.X509Certificates;            //Import namesapce to use function
using Terminal.Gui;

public class Admin
{
    // Properties of the Admin class
    public int AdminID { get; set; }
    public string AdminName { get; set; }
    public string AdminPhone { get; set; }
    public string AdminEmail { get; set; }
    public string AdminGender { get; set; }

    // Static variables
    public static Cart userCart = new Cart();             // Represents a cart associated with the admin
    public static Products pd = new Products();           // Represents a product associated with the admin
    public static Orders order = new Orders();            // Represents an order associated with the admin
    public static Customers customer = new Customers();   // Represents a customer associated with the admin
    public static Users user = new Users();     
    public static Admin admin = new Admin();          // Represents a user associated with the admin
    public static SuperAdmin superadmin = new SuperAdmin();// Represents a superadmin associated with the admin
    public static Program program = new Program();        // Represents a program instance
    public static string connectionString = Configuration.ConnectionString; // Connection string for database access

    // Lists to store users and admins
    public static List<Users> ListUsers = new List<Users>();
    public static List<Admin> ListAdmin = new List<Admin>();
    static List<Admin> LoadAdmin(string connectionString)
    {
        List<Admin> ListAdmin = new List<Admin>();

        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            string query = "SELECT * FROM admins WHERE active = TRUE";
            MySqlCommand command = new MySqlCommand(query, connection);
            
            try
            {
                connection.Open();
                MySqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Admin ad = new Admin();
                    ad.AdminID = reader.GetInt32("admin_id");
                    ad.AdminName = reader.GetString("admin_name");
                    ad.AdminPhone = reader.GetString("admin_phone");
                    ad.AdminEmail = reader.GetString("admin_email"); // Corrected from "cusmin_email"

                    ListAdmin.Add(ad);
                }

                reader.Close(); // Close the reader when done
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error loading admins: " + ex.Message);
                // Consider logging or handling the exception appropriately
            }
        }

        return ListAdmin;
    }
    static List<Users> LoadUsers(string connectionString)
    {
        List<Admin> ListAdmin = new List<Admin>();

        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            string query = "SELECT * FROM users WHERE active = TRUE";
            MySqlCommand command = new MySqlCommand(query, connection);
            
            try
            {
                connection.Open();
                MySqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Users us = new Users();
                    us.UserId = reader.GetInt32("user_id");
                    us.Username = reader.GetString("username");
                    us.PasswordHash = reader.GetString("password_hash");
                    us.CustomerID = reader.GetInt32("user_customer_admin_id");
                    us.Roles = reader.GetString("role");

                    ListUsers.Add(us);
                }

                reader.Close(); // Close the reader when done
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error loading admins: " + ex.Message);
                // Consider logging or handling the exception appropriately
            }
        }

        return ListUsers;
    }

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
            Width = Dim.Percent(30),
            Height = Dim.Fill()
        };
        adminMenu.Add(leftFrame);

        var rightTopFrame = new FrameView("Welcome back")
        {
            X = Pos.Percent(30),
            Y = 0,
            Width = Dim.Fill(),
            Height = Dim.Percent(50)
        };
        adminMenu.Add(rightTopFrame);

        var rightBottomFrame = new FrameView("Statistical")
        {
            X = Pos.Percent(30),
            Y = Pos.Percent(50),
            Width = Dim.Fill(),
            Height = Dim.Fill()
        };
        adminMenu.Add(rightBottomFrame);

        var btnDisplayCustomer = new Button("Management Customer")
        {
            X = 2,
            Y = 4
        };
        btnDisplayCustomer.Clicked += () =>
        {
            try
            {
                top.Remove(adminMenu);
                customer.DisplayCustomers("admin");
            }
            catch
            {
                MessageBox.ErrorQuery("Error", "An error occurred while displaying customers. Please try again.", "OK");
            }
        };

        var btnFindCustomer = new Button("Find Customer")
        {
            X = 2,
            Y = 5
        };
        btnFindCustomer.Clicked += () =>
        {
            try
            {
                top.Remove(adminMenu);
                customer.FindCustomer("admin");
            }
            catch
            {
                MessageBox.ErrorQuery("Error", "An error occurred while finding a customer. Please try again.", "OK");
            }
        };

        var btnAddCustomer = new Button("Add New Customer")
        {
            X = 2,
            Y = 6
        };
        btnAddCustomer.Clicked += () =>
        {
            try
            {
                top.Remove(adminMenu);
                customer.AddCustomer();
            }
            catch
            {
                MessageBox.ErrorQuery("Error", "An error occurred while adding a new customer. Please try again.", "OK");
            }
        };

        var btnDisplayProduct = new Button("Management Products")
        {
            X = 2,
            Y = 7
        };
        btnDisplayProduct.Clicked += () =>
        {
            try
            {
                top.Remove(adminMenu);
                pd.DisplayProduct("admin");
            }
            catch
            {
                MessageBox.ErrorQuery("Error", "An error occurred while displaying products. Please try again.", "OK");
            }
        };

        var btnAddProduct = new Button("Add Product")
        {
            X = 2,
            Y = 8
        };
        btnAddProduct.Clicked += () =>
        {
            try
            {
                top.Remove(adminMenu);
                pd.AddProduct();
            }
            catch
            {
                MessageBox.ErrorQuery("Error", "An error occurred while adding a new product. Please try again.", "OK");
            }
        };

        var btnFindProduct = new Button("Find Product")
        {
            X = 2,
            Y = 9
        };
        btnFindProduct.Clicked += () =>
        {
            try
            {
                top.Remove(adminMenu);
                pd.FindProduct("admin");
            }
            catch
            {
                MessageBox.ErrorQuery("Error", "An error occurred while finding a product. Please try again.", "OK");
            }
        };

        var btnUpdateStatus = new Button("Update Status")
        {
            X = 2,
            Y = 10
        };
        btnUpdateStatus.Clicked += () =>
        {
            try
            {
                top.Remove(adminMenu);
                order.UpdateStatus();
            }
            catch
            {
                MessageBox.ErrorQuery("Error", "An error occurred while updating the status. Please try again.", "OK");
            }
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

        leftFrame.Add(btnDisplayCustomer, btnFindCustomer,
                    btnAddCustomer, btnDisplayProduct,
                    btnAddProduct, btnFindProduct, btnUpdateStatus, LogoutButton);

        var rightTopLabel = new Label(@"███████╗██╗     ███████╗ ██████╗████████╗██████╗  ██████╗ ███╗   ██╗██╗ ██████╗    ███████╗██╗  ██╗ ██████╗ ██████╗ 
██╔════╝██║     ██╔════╝██╔════╝╚══██╔══╝██╔══██╗██╔═══██╗████╗  ██║██║██╔════╝    ██╔════╝██║  ██║██╔═══██╗██╔══██╗
█████╗  ██║     █████╗  ██║        ██║   ██████╔╝██║   ██║██╔██╗ ██║██║██║         ███████╗███████║██║   ██║██████╔╝
██╔══╝  ██║     ██╔══╝  ██║        ██║   ██╔══██╗██║   ██║██║╚██╗██║██║██║         ╚════██║██╔══██║██║   ██║██╔═══╝ 
███████╗███████╗███████╗╚██████╗   ██║   ██║  ██║╚██████╔╝██║ ╚████║██║╚██████╗    ███████║██║  ██║╚██████╔╝██║     
╚══════╝╚══════╝╚══════╝ ╚═════╝   ╚═╝   ╚═╝  ╚═╝ ╚═════╝ ╚═╝  ╚═══╝╚═╝ ╚═════╝    ╚══════╝╚═╝  ╚═╝ ╚═════╝ ╚═╝     ")
        {
            X = Pos.Center(),
            Y = Pos.Center()
        };
        rightTopFrame.Add(rightTopLabel);

         // Connect to the database to get the total number of orders
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
        connection.Open();
        string query = @"SELECT COUNT(*) AS TotalOrders, 
                                SUM(order_total_price) AS TotalRevenue, 
                                SUM(order_quantity) AS TotalQuantity 
                        FROM shop.orders";
        MySqlCommand command = new MySqlCommand(query, connection);
        
            using (MySqlDataReader reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    int totalOrders = reader.GetInt32("TotalOrders");
                    decimal totalRevenue = reader.GetDecimal("TotalRevenue");
                    int totalQuantity = reader.GetInt32("TotalQuantity");

                    // Add the total order count label to the bottom right frame
                    var countOrder = new Label($"Total orders: {totalOrders}")
                    {
                        X = Pos.Center(),
                        Y = Pos.Center() - 2 // Adjust position as needed
                    };
                    rightBottomFrame.Add(countOrder);

                    // Add the total revenue label to the bottom right frame
                    var totalRevenueLabel = new Label($"Total revenue: {totalRevenue:C}")
                    {
                        X = Pos.Center(),
                        Y = Pos.Center() // Adjust position as needed
                    };
                    rightBottomFrame.Add(totalRevenueLabel);

                    // Add the total quantity label to the bottom right frame
                    var totalQuantityLabel = new Label($"Total sold: {totalQuantity}")
                    {
                        X = Pos.Center(),
                        Y = Pos.Center() + 2 // Adjust position as needed
                    };
                    rightBottomFrame.Add(totalQuantityLabel);
                }
            }
        }
    }
    public void AddStaff()
    {
        Users us = new Users();
        Admin ad = new Admin();
        var top = Application.Top;
        var registerWin = new Window()
        {
            Title = "Register for Staff",
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
            X = 16,
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
            X = 16,
            Y = 4,
            Width = Dim.Fill() - 4
        };

        var adminNameLabel = new Label("Name:")
        {
            X = 2,
            Y = 6
        };
        var adminNameField = new TextField("")
        {
            X = 16,
            Y = 6,
            Width = Dim.Fill() - 4
        };

        var adminPhoneNumberLabel = new Label("Phone number:")
        {
            X = 2,
            Y = 8
        };
        var adminPhoneNumberField = new TextField("")
        {
            X = 16,
            Y = 8,
            Width = Dim.Fill() - 4
        };

        var adminEmailLabel = new Label("Email:")
        {
            X = 2,
            Y = 10
        };
        var adminEmailField = new TextField("")
        {
            X = 16,
            Y = 10,
            Width = Dim.Fill() - 4
        };

        var adminGenderLabel = new Label("Gender:")
        {
            X = 2,
            Y = 12
        };
        var adminGenderField = new TextField("")
        {
            X = 16,
            Y = 12,
            Width = Dim.Fill() - 4
        };

        var registerButton = new Button("Register")
        {
            X = Pos.Center(),
            Y = Pos.Bottom(adminGenderField) + 2,
        };
        registerButton.Clicked += () =>
        {
            // Validate input values
            if (string.IsNullOrWhiteSpace(usernameField.Text.ToString()) ||
                string.IsNullOrWhiteSpace(passwordField.Text.ToString()) ||
                string.IsNullOrWhiteSpace(adminNameField.Text.ToString()) ||
                string.IsNullOrWhiteSpace(adminPhoneNumberField.Text.ToString()) ||
                string.IsNullOrWhiteSpace(adminEmailField.Text.ToString()) ||
                string.IsNullOrWhiteSpace(adminGenderField.Text.ToString()))
            {
                MessageBox.ErrorQuery("Error", "All fields must be filled.", "OK");
                return;
            }

            if (!IsValidEmail(adminEmailField.Text.ToString()))
            {
                MessageBox.ErrorQuery("Error", "Invalid email format.", "OK");
                return;
            }

            us.Username = usernameField.Text.ToString();
            us.PasswordHash = HashPassword(passwordField.Text.ToString());
            ad.AdminName = adminNameField.Text.ToString();
            ad.AdminPhone = adminPhoneNumberField.Text.ToString();
            ad.AdminEmail = adminEmailField.Text.ToString();
            ad.AdminGender = adminGenderField.Text.ToString();

            MySqlConnection connection = null;
            try
            {
                connection = new MySqlConnection(connectionString);
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

                    // Insert admin data
                    string adminQuery = "INSERT INTO admins (admin_name, admin_phone, admin_email, admin_gender)" +
                                        " VALUES (@adminname, @adminphonenumber, @adminemail, @admingender)";
                    MySqlCommand adminCommand = new MySqlCommand(adminQuery, connection, transaction);
                    adminCommand.Parameters.AddWithValue("@adminname", ad.AdminName);
                    adminCommand.Parameters.AddWithValue("@adminphonenumber", ad.AdminPhone);
                    adminCommand.Parameters.AddWithValue("@adminemail", ad.AdminEmail);
                    adminCommand.Parameters.AddWithValue("@admingender", ad.AdminGender);
                    adminCommand.ExecuteNonQuery();

                    // Retrieve the last inserted admin ID
                    long adminId = adminCommand.LastInsertedId;

                    // Insert user data linked to the admin
                    string userQuery = "INSERT INTO users (username, password_hash, role, user_customer_admin_id) " +
                                    "VALUES (@Username, @PasswordHash, 'admin', @AdminId)";
                    MySqlCommand userCommand = new MySqlCommand(userQuery, connection, transaction);
                    userCommand.Parameters.AddWithValue("@Username", us.Username);
                    userCommand.Parameters.AddWithValue("@PasswordHash", us.PasswordHash);
                    userCommand.Parameters.AddWithValue("@AdminId", adminId);
                    userCommand.ExecuteNonQuery();

                    transaction.Commit();

                    // Add new user and admin to lists
                    ListUsers.Add(us);
                    ListAdmin.Add(ad);

                    MessageBox.Query("Success", "Registration successful!", "OK");

                    top.Remove(registerWin);
                    superadmin.SuperAdminMenu();
                }
                catch
                {
                    transaction.Rollback();
                    MessageBox.ErrorQuery("Error", "An error occurred during registration. Please try again.", "OK");
                }
            }
            catch
            {
                MessageBox.ErrorQuery("Error", "Could not connect to the database. Please try again later.", "OK");
            }
            finally
            {
                connection?.Close();
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
            superadmin.SuperAdminMenu();
        };

        registerWin.Add(usernameLabel, usernameField, passwordLabel, passwordField,
                        adminNameLabel, adminNameField, adminPhoneNumberLabel, adminPhoneNumberField,
                        adminEmailLabel, adminEmailField, adminGenderLabel, adminGenderField,
                        registerButton, closeButton);
    }

    // Helper method for email validation
    private bool IsValidEmail(string email)
    {
        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }

    // Helper method for hashing password
    private string HashPassword(string password)
    {
        using (var sha256 = System.Security.Cryptography.SHA256.Create())
        {
            var bytes = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(bytes);
        }
    }

    public void FindStaff()
    {
        var top = Application.Top;
        var findStaffWin = new Window("Find Staff")
        {
            X = 0,
            Y = 0,
            Width = Dim.Fill(),
            Height = Dim.Fill()
        };
        top.Add(findStaffWin);

        // Label and field for entering the staff name
        var staffNameLabel = new Label("Staff name:")
        {
            X = 2,
            Y = 2
        };
        var staffNameField = new TextField("")
        {
            X = Pos.Right(staffNameLabel) + 1,
            Y = 2,
            Width = 40
        };

        // Button to find the staff
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
                string staffName = staffNameField.Text.ToString();

                if (string.IsNullOrWhiteSpace(staffName))
                {
                    MessageBox.ErrorQuery("Error", "You must enter the staff name you want to find.", "OK");
                    return;
                }
                top.Remove(findStaffWin);
                // Query to retrieve staff information based on staff name
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    string query = @"
                        SELECT 
                            adm.admin_name,
                            adm.admin_phone AS admin_phone_number,
                            adm.admin_email,
                            adm.admin_gender,
                            usr.username,
                            usr.password_hash
                        FROM admins adm
                        INNER JOIN users usr ON adm.admin_id = usr.user_customer_admin_id
                        WHERE adm.admin_name LIKE @SearchTerm AND usr.role = 'admin' AND adm.active = TRUE";
                    MySqlCommand command = new MySqlCommand(query, connection);
                    command.Parameters.AddWithValue("@SearchTerm", "%" + staffName + "%");
                    connection.Open();
                    MySqlDataReader reader = command.ExecuteReader();

                    ListAdmin.Clear();
                    ListUsers.Clear();

                    while (reader.Read())
                    {
                        var admin = new Admin
                        {
                            AdminName = reader["admin_name"].ToString(),
                            AdminPhone = reader["admin_phone_number"].ToString(),
                            AdminEmail = reader["admin_email"].ToString(),
                            AdminGender = reader["admin_gender"].ToString()
                        };
                        var user = new Users
                        {
                            Username = reader["username"].ToString(),
                            PasswordHash = reader["password_hash"].ToString()
                        };

                        ListAdmin.Add(admin);
                        ListUsers.Add(user);
                    }

                    reader.Close();
                }

                if (ListAdmin.Count == 0)
                {
                    // Display error if no staff found
                    MessageBox.ErrorQuery("Error", "Staff not found!", "OK");
                    return;
                }

                top.Remove(findStaffWin);

                // Create a new window to display the staff
                var staffWindow = new Window("Staff")
                {
                    X = 0,
                    Y = 0,
                    Width = Dim.Fill(),
                    Height = Dim.Fill()
                };
                top.Add(staffWindow);

                var scrollView = new ScrollView()
                {
                    X = 0,
                    Y = 0,
                    Width = Dim.Fill(),
                    Height = Dim.Fill(),
                    ContentSize = new Size(0, 0) // Content size will be set later
                };
                staffWindow.Add(scrollView);

                var staffContainer = new View()
                {
                    X = 0,
                    Y = 0,
                    Width = Dim.Fill(),
                    Height = Dim.Fill()
                };
                scrollView.Add(staffContainer);

                int row = 0;
                int col = 0;
                int colCount = 2; // Number of columns in the ScrollView
                int colWidth = 40; // Width of each staff window
                int rowHeight = 10; // Height of each staff window
                int margin = 2; // Margin between staff windows

                foreach (var admin in ListAdmin)
                {
                    var AdminWindow = new Window($"Staff {row * colCount + col + 1}")
                    {
                        X = col * (colWidth + margin),
                        Y = row * (rowHeight + margin),
                        Width = colWidth,
                        Height = rowHeight
                    };

                    var staffNameLabel = new Label($"Name: {admin.AdminName}")
                    {
                        X = 1,
                        Y = 1,
                        Width = Dim.Fill(),
                        TextAlignment = TextAlignment.Left
                    };
                    var staffPhoneLabel = new Label($"Phone: {admin.AdminPhone}")
                    {
                        X = 1,
                        Y = Pos.Bottom(staffNameLabel) + 1,
                        Width = Dim.Fill(),
                        TextAlignment = TextAlignment.Left
                    };
                    var staffEmailLabel = new Label($"Email: {admin.AdminEmail}")
                    {
                        X = 1,
                        Y = Pos.Bottom(staffPhoneLabel) + 1,
                        Width = Dim.Fill(),
                        TextAlignment = TextAlignment.Left
                    };
                    var staffGenderLabel = new Label($"Gender: {admin.AdminGender}")
                    {
                        X = 1,
                        Y = Pos.Bottom(staffEmailLabel) + 1,
                        Width = Dim.Fill(),
                        TextAlignment = TextAlignment.Left
                    };

                    var user = ListUsers[row * colCount + col];
                    var usernameLabel = new Label($"Username: {user.Username}")
                    {
                        X = 1,
                        Y = Pos.Bottom(staffGenderLabel) + 1,
                        Width = Dim.Fill(),
                        TextAlignment = TextAlignment.Left
                    };
                    var passwordHashLabel = new Label($"Password Hash: {user.PasswordHash}")
                    {
                        X = 1,
                        Y = Pos.Bottom(usernameLabel) + 1,
                        Width = Dim.Fill(),
                        TextAlignment = TextAlignment.Left
                    };

                    AdminWindow.Add(staffNameLabel, staffPhoneLabel, staffEmailLabel, staffGenderLabel, usernameLabel, passwordHashLabel);
                    staffContainer.Add(AdminWindow);

                    col++;
                    if (col >= colCount)
                    {
                        col = 0;
                        row++;
                    }
                }

                scrollView.ContentSize = new Size((colWidth + margin) * colCount, (row + 1) * (rowHeight + margin));

                var btnClose = new Button("Close")
                {
                    X = Pos.Center(),
                    Y = 1
                };
                btnClose.Clicked += () =>
                {
                    ListAdmin.Clear();
                    ListUsers.Clear();
                    top.Remove(staffWindow);
                    FindStaff();
                };
                staffWindow.Add(btnClose);
            }
            catch 
            {
                MessageBox.ErrorQuery("Error", "An error occurred while searching for staff.", "OK");
            }
        };

        var closeButton = new Button("Close")
        {
            X = Pos.Center(),
            Y = Pos.Percent(100) - 1
        };
        closeButton.Clicked += () =>
        {
            ListAdmin.Clear();
            ListUsers.Clear();
            top.Remove(findStaffWin);
            superadmin.SuperAdminMenu();
        };

        findStaffWin.Add(staffNameLabel, staffNameField, findButton, closeButton);
    }
    public void DeleteStaff(int adminID)
    {
        try
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                using (MySqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        string query = @"
                            UPDATE admins
                            SET active = FALSE 
                            WHERE admin_id = @AdminID;
                            
                            UPDATE users
                            SET active = FALSE
                            WHERE user_customer_admin_id = @AdminID";
                        
                        MySqlCommand command = new MySqlCommand(query, connection, transaction);
                        command.Parameters.AddWithValue("@AdminID", adminID);
                        int rowsAffected = command.ExecuteNonQuery();
                        
                        if (rowsAffected > 0)
                        {
                            // Commit the transaction if the update was successful
                            transaction.Commit();
                            MessageBox.Query("Success", "Staff successfully marked as inactive!", "OK");
                        }
                        else
                        {
                            // Rollback the transaction if no rows were affected
                            transaction.Rollback();
                            MessageBox.ErrorQuery("Error", "No staff found with the provided ID.", "OK");
                        }
                    }
                    catch 
                    {
                        // Rollback the transaction in case of an error
                        transaction.Rollback();
                        MessageBox.ErrorQuery("Error", "An unexpected error occurred while updating the staff. Please try again later.", "OK");
                    }
                }
            }
        }
        catch 
        {
            // Handle any issues with opening the connection or starting the transaction
            MessageBox.ErrorQuery("Error", "An unexpected error occurred while connecting to the database. Please try again later.", "OK");
        }
    }

    public void DisplayStaff()
    {
        var top = Application.Top;

        // Tạo cửa sổ chính
        var displayWindow = new Window("Staff List")
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

        // Tạo một View để chứa tất cả các cửa sổ nhân viên
        var staffContainer = new View()
        {
            X = 0,
            Y = 0,
            Width = Dim.Fill(),
            Height = Dim.Fill()
        };

        scrollView.Add(staffContainer);

        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            string query = @"
                SELECT 
                    adm.admin_id,
                    adm.admin_name,
                    adm.admin_phone AS admin_phone_number,
                    adm.admin_email,
                    adm.admin_gender,
                    usr.username,
                    usr.password_hash
                FROM admins adm
                INNER JOIN users usr ON adm.admin_id = usr.user_customer_admin_id
                WHERE usr.role = 'admin'";

            MySqlCommand command = new MySqlCommand(query, connection);
            connection.Open();
            MySqlDataReader reader = command.ExecuteReader();

            int colCount = 3; // Number of columns per row
            int colWidth = 40; // Width of each staff window
            int rowHeight = 15; // Height of each staff window
            int margin = 2; // Margin between staff windows

            int col = 0;
            int row = 0;

            while (reader.Read())
            {
                int adminId = int.Parse(reader["admin_id"].ToString());
                string adminName = reader["admin_name"].ToString();
                string adminPhone = reader["admin_phone_number"].ToString();
                string adminEmail = reader["admin_email"].ToString();
                string adminGender = reader["admin_gender"].ToString();
                string username = reader["username"].ToString();
                string password = reader["password_hash"].ToString();

                var staffWindow = new Window($"{adminName}")
                {
                    X = col * (colWidth + margin),
                    Y = row * (rowHeight + margin),
                    Width = colWidth,
                    Height = rowHeight
                };

                var nameLabel = new Label($"Name: {adminName}")
                {
                    X = 1,
                    Y = 1,
                    Width = Dim.Fill()
                };
                var phoneLabel = new Label($"Phone: {adminPhone}")
                {
                    X = 1,
                    Y = 2,
                    Width = Dim.Fill()
                };
                var emailLabel = new Label($"Email: {adminEmail}")
                {
                    X = 1,
                    Y = 3,
                    Width = Dim.Fill()
                };
                var genderLabel = new Label($"Gender: {adminGender}")
                {
                    X = 1,
                    Y = 4,
                    Width = Dim.Fill()
                };
                var usernameLabel = new Label($"Username: {username}")
                {
                    X = 1,
                    Y = 5,
                    Width = Dim.Fill()
                };
                var passwordLabel = new Label($"Password: {password}")
                {
                    X = 1,
                    Y = 6,
                    Width = Dim.Fill()
                };

                var editButton = new Button("Edit")
                {
                    X = 1,
                    Y = 8
                };
                editButton.Clicked += () =>
                {
                    top.Remove(displayWindow);
                    admin.EditStaff(adminId, adminName, adminPhone, adminEmail, adminGender, username, password);
                };

                var deleteButton = new Button("Delete")
                {
                    X = Pos.Right(editButton) + 2,
                    Y = 8
                };
                deleteButton.Clicked += () =>
                {
                    bool confirmed = MessageBox.Query("Confirm", "Are you sure you want to disable this staff?", "Yes", "No") == 0;
                    if (confirmed)
                    {
                        try
                        {
                            admin.DeleteStaff(adminId);
                        }
                        catch
                        {
                            MessageBox.ErrorQuery("Error", "An unexpected error occurred while trying to disable the staff. Please try again later.", "OK");
                        }
                    }
                };

                staffWindow.Add(editButton, deleteButton);

                staffWindow.Add(nameLabel, phoneLabel, emailLabel, genderLabel, usernameLabel, passwordLabel);
                staffContainer.Add(staffWindow);

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
            Y = Pos.Top(scrollView)
        };
        backButton.Clicked += () =>
        {
            top.Remove(displayWindow);
            superadmin.SuperAdminMenu(); // Gọi menu superadmin

        };

        displayWindow.Add(backButton);
    }
    public void EditStaff(int adminID, string adminName, string adminPhone, string adminEmail, string adminGender, string username, string password)
    {
        var top = Application.Top;

        // Create a new window for editing staff details
        var editStaffWin = new Window("Edit Staff")
        {
            X = 0,
            Y = 0,
            Width = Dim.Fill(),
            Height = Dim.Fill()
        };
        top.Add(editStaffWin);

        // Labels and fields for editing staff information
        var editAdminNameLabel = new Label("Staff Name:")
        {
            X = 2,
            Y = 2
        };
        var editAdminNameField = new TextField(adminName)
        {
            X = 18,
            Y = 2,
            Width = 100
        };

        var editAdminPhoneLabel = new Label("Staff Phone:")
        {
            X = 2,
            Y = 4
        };
        var editAdminPhoneField = new TextField(adminPhone)
        {
            X = 18,
            Y = 4,
            Width = 100
        };

        var editAdminEmailLabel = new Label("Staff Email:")
        {
            X = 2,
            Y = 6
        };
        var editAdminEmailField = new TextField(adminEmail)
        {
            X = 18,
            Y = 6,
            Width = 100
        };

        var editAdminGenderLabel = new Label("Staff Gender:")
        {
            X = 2,
            Y = 8
        };
        var editAdminGenderField = new TextField(adminGender)
        {
            X = 18,
            Y = 8,
            Width = 100
        };

        var editAdminUserNameLabel = new Label("Username:")
        {
            X = 2,
            Y = 10
        };
        var editAdminUserNameField = new TextField(username)
        {
            X = 18,
            Y = 10,
            Width = 100
        };

        var editAdminPasswordLabel = new Label("Password:")
        {
            X = 2,
            Y = 12
        };
        var editAdminPasswordField = new TextField(password)
        {
            X = 18,
            Y = 12,
            Width = 100,
            Secret = true
        };

        // Button to save edited staff information
        var saveButton = new Button("Save")
        {
            X = 20,
            Y = 14
        };
        saveButton.Clicked += () =>
        {
            try
            {
                // Update the admin object with data from the text fields
                var updatedAdminName = editAdminNameField.Text.ToString();
                var updatedAdminPhone = editAdminPhoneField.Text.ToString();
                var updatedAdminEmail = editAdminEmailField.Text.ToString();
                var updatedAdminGender = editAdminGenderField.Text.ToString();
                var updateusername = editAdminUserNameField.Text.ToString();
                var updatepass = editAdminPasswordField.Text.ToString();

                // Open a connection to the database and execute the update query
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    string query = @"UPDATE admins 
                                    SET admin_name = @AdminName, 
                                        admin_phone = @AdminPhone, 
                                        admin_email = @AdminEmail, 
                                        admin_gender = @AdminGender 
                                    WHERE admin_id = @AdminID;
                                    
                                    UPDATE users 
                                    SET username = @UserName, 
                                        password_hash = @Pass 
                                    WHERE user_customer_admin_id = @AdminID";
                    MySqlCommand command = new MySqlCommand(query, connection);
                    command.Parameters.AddWithValue("@AdminID", adminID);
                    command.Parameters.AddWithValue("@AdminName", updatedAdminName);
                    command.Parameters.AddWithValue("@AdminPhone", updatedAdminPhone);
                    command.Parameters.AddWithValue("@AdminEmail", updatedAdminEmail);
                    command.Parameters.AddWithValue("@AdminGender", updatedAdminGender);
                    command.Parameters.AddWithValue("@UserName", updateusername);
                    command.Parameters.AddWithValue("@Pass", updatepass);

                    command.ExecuteNonQuery();
                }

                // Display a success message and close the window
                MessageBox.Query("Success", "Staff information has been updated!", "OK");
                top.Remove(editStaffWin);
                // Refresh the staff list or do something else after saving
                // Example: superadmin.SuperAdminMenu();
            }
            catch 
            {
                // Display an error message if the update fails
                MessageBox.ErrorQuery("Error", "An unexpected error occurred while updating the staff information. Please try again later.", "OK");
            }
        };

        // Button to close the window
        var closeButton = new Button("Close")
        {
            X = 20,
            Y = Pos.Bottom(saveButton) + 1
        };
        closeButton.Clicked += () =>
        {
            // Confirm before closing the window
            bool confirmed = MessageBox.Query("Confirm", "Are you sure you want to close?", "Yes", "No") == 0;
            if (confirmed)
            {
                top.Remove(editStaffWin);
                // Return to the admin menu or do something else after closing
                // Example: superadmin.SuperAdminMenu();
            }
        };

        // Add all controls to the edit staff window
        editStaffWin.Add(editAdminNameLabel, editAdminNameField,
                            editAdminPhoneLabel, editAdminPhoneField,
                            editAdminEmailLabel, editAdminEmailField,
                            editAdminGenderLabel, editAdminGenderField,
                            editAdminUserNameLabel, editAdminUserNameField,
                            editAdminPasswordLabel, editAdminPasswordField,
                            saveButton, closeButton);
    }
}

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
    public static Users user = new Users();               // Represents a user associated with the admin
    public static SuperAdmin superadmin = new SuperAdmin();// Represents a superadmin associated with the admin
    public static Program program = new Program();        // Represents a program instance
    public static string connectionString = Configuration.ConnectionString; // Connection string for database access
    public static int currentCustomerID = SessionData.Instance.CurrentCustomerID; // Current customer ID

    // Lists to store users and admins
    public static List<Users> ListUsers = new List<Users>();
    public static List<Admin> ListAdmin = new List<Admin>();
    static List<Admin> LoadAdmin(string connectionString)
    {
        List<Admin> ListAdmin = new List<Admin>();

        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            string query = "SELECT * FROM admins";
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
            string query = "SELECT * FROM users";
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
                    us.CustomerID = reader.GetInt32("user_customer_id");
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

        var rightTopFrame = new FrameView("Welcome")
        {
            X = Pos.Percent(30),
            Y = 0,
            Width = Dim.Fill(),
            Height = Dim.Percent(50)
        };
        adminMenu.Add(rightTopFrame);

        var rightBottomFrame = new FrameView("Number of products sold")
        {
            X = Pos.Percent(30),
            Y = Pos.Percent(50),
            Width = Dim.Fill(),
            Height = Dim.Fill()
        };
        adminMenu.Add(rightBottomFrame);

        var btnOrderForCustomer = new Button("Order for customer")
        {
            X = 2,
            Y = 2
        };
        btnOrderForCustomer.Clicked += () =>
        {
            top.Remove(adminMenu);
            order.DisplayProductToOrderForCustomer();
        };

        var btnDisplayCustomer = new Button("Display Customer")
        {
            X = 2,
            Y = 4
        };
        btnDisplayCustomer.Clicked += () =>
        {
            top.Remove(adminMenu);
            customer.DisplayCustomers();
        };

        var btnFindCustomer = new Button("Find Customer")
        {
            X = 2,
            Y = 6
        };
        btnFindCustomer.Clicked += () =>
        {
            top.Remove(adminMenu);
            customer.FindCustomer();
        };

        var btnEditCustomer = new Button("Edit Customer")
        {
            X = 2,
            Y = 8
        };
        btnEditCustomer.Clicked += () =>
        {
            top.Remove(adminMenu);
            customer.EditCustomer();
        };

        var btnAddCustomer = new Button("Add New Customer")
        {
            X = 2,
            Y = 10
        };
        btnAddCustomer.Clicked += () =>
        {
            top.Remove(adminMenu);
            customer.AddCustomer();
        };

        var btnDeleteCustomer = new Button("Delete Customer")
        {
            X = 2,
            Y = 12
        };
        btnDeleteCustomer.Clicked += () =>
        {
            top.Remove(adminMenu);
            customer.DeleteCustomer();
        };

        var btnDisplayProduct = new Button("Display Products")
        {
            X = 2,
            Y = 14
        };
        btnDisplayProduct.Clicked += () =>
        {
            top.Remove(adminMenu);
            pd.DisplayProduct("admin");
        };

        var btnEditProduct = new Button("Edit Product")
        {
            X = 2,
            Y = 16
        };
        btnEditProduct.Clicked += () =>
        {
            top.Remove(adminMenu);
            pd.EditProductInformations();
        };

        var btnAddProduct = new Button("Add Product")
        {
            X = 2,
            Y = 18
        };
        btnAddProduct.Clicked += () =>
        {
            top.Remove(adminMenu);
            pd.AddProduct();
        };

        var btnFindProduct = new Button("Find Product")
        {
            X = 2,
            Y = 20
        };
        btnFindProduct.Clicked += () =>
        {
            top.Remove(adminMenu);
            pd.FindProduct("admin");
        };

        var btnDeleteProduct = new Button("Delete Product")
        {
            X = 2,
            Y = 22
        };
        btnDeleteProduct.Clicked += () =>
        {
            top.Remove(adminMenu);
            pd.DeleteProduct();
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

        leftFrame.Add(btnOrderForCustomer, btnDisplayCustomer, btnFindCustomer, btnEditCustomer,
                    btnAddCustomer, btnDeleteCustomer, btnDisplayProduct, btnEditProduct,
                    btnAddProduct, btnFindProduct, btnDeleteProduct, LogoutButton);

        // Fetch admin name for display
        string adminName = "";
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            string query = "SELECT admin_name FROM admins WHERE admin_id = @AdminID";
            MySqlCommand command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@AdminID", AdminID); // Use AdminID property
            connection.Open();
            MySqlDataReader reader = command.ExecuteReader();
            if (reader.Read())
            {
                adminName = reader["admin_name"].ToString();
            }
        }

        var rightTopLabel = new Label($"Welcome, {adminName}")
        {
            X = 1,
            Y = 1
        };
        rightTopFrame.Add(rightTopLabel);

        // Fetch and display total number of orders
        try
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT COUNT(*) FROM orders";
                MySqlCommand command = new MySqlCommand(query, connection);
                object result = command.ExecuteScalar();
                int count = Convert.ToInt32(result);
                var countOrder = new Label($"Total orders: {count}")
                {
                    X = 1,
                    Y = 1
                };
                rightBottomFrame.Add(countOrder);
            }
        }
        catch (Exception ex)
        {
            MessageBox.ErrorQuery("Error", ex.Message, "OK");
        }

        Application.Run();
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
            us.Username = usernameField.Text.ToString();
            us.PasswordHash = passwordField.Text.ToString();
            ad.AdminName = adminNameField.Text.ToString();
            ad.AdminPhone = adminPhoneNumberField.Text.ToString();
            ad.AdminEmail = adminEmailField.Text.ToString();
            ad.AdminGender = adminGenderField.Text.ToString();

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                MySqlTransaction transaction = connection.BeginTransaction();
                try
                {
                    // Insert into admins table
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

                    // Insert into users table
                    string userQuery = "INSERT INTO users (username, password_hash, role, user_customer_id) " +
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
        };

        registerWin.Add(usernameLabel, usernameField, passwordLabel, passwordField,
                        adminNameLabel, adminNameField, adminPhoneNumberLabel, adminPhoneNumberField,
                        adminEmailLabel, adminEmailField, adminGenderLabel, adminGenderField,
                        registerButton, closeButton);
    }
   
    public void FindStaff()
    {
        List<string[]> admin = new List<string[]>();
        var top = Application.Top;

        var findAdminWindow = new Window("Find Admin")
        {
            X = 0,
            Y = 0,
            Width = Dim.Fill(),
            Height = Dim.Fill()
        };
        top.Add(findAdminWindow);
        findAdminWindow.FocusNext();

        var searchLabel = new Label("Enter staff name:")
        {
            X = 1,
            Y = 1
        };
        var searchField = new TextField("")
        {
            X = Pos.Right(searchLabel) + 1,
            Y = 1,
            Width = 40
        };
        var searchButton = new Button("Search")
        {
            X = Pos.Right(searchField) + 1,
            Y = 1
        };

        findAdminWindow.Add(searchLabel, searchField, searchButton);

        var resultLabel = new Label("Results:")
        {
            X = 1,
            Y = 3
        };
        findAdminWindow.Add(resultLabel);

        var columnDisplayListAdmin = new string[]
        {
            "Admin's name", "Phone number", "Email", "Gender", "Username", "Password"
        };

        int columnWidth = 20; // Width of each column

        // Add column headers
        for (int i = 0; i < columnDisplayListAdmin.Length; i++)
        {
            findAdminWindow.Add(new Label(columnDisplayListAdmin[i])
            {
                X = i * columnWidth,
                Y = 4,
                Width = columnWidth,
                Height = 1
            });
        }

        // Handle search button click event
        searchButton.Clicked += () =>
        {
            admin.Clear(); // Clear previous search results

            string searchTerm = searchField.Text.ToString();

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
                    INNER JOIN users usr ON adm.admin_id = usr.user_customer_id
                    WHERE adm.admin_name LIKE @SearchTerm AND usr.role = 'admin' ";

                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@SearchTerm", "%" + searchTerm + "%");

                connection.Open();
                MySqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    admin.Add(new string[]
                    {
                        reader["admin_name"].ToString(),
                        reader["admin_phone_number"].ToString(),
                        reader["admin_email"].ToString(),
                        reader["admin_gender"].ToString(),
                        reader["username"].ToString(),
                        reader["password_hash"].ToString()
                    });
                }

                // Display search results
                for (int i = 0; i < admin.Count; i++)
                {
                    for (int j = 0; j < admin[i].Length; j++)
                    {
                        findAdminWindow.Add(new Label(admin[i][j])
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

        var btnClose = new Button("Close")
        {
            X = Pos.Center(),
            Y = Pos.Percent(100) - 1
        };
        btnClose.Clicked += () =>
        {
            top.Remove(findAdminWindow);
            superadmin.SuperAdminMenu();
        };

        findAdminWindow.Add(btnClose);
    }

    public void DeleteStaff()
    {
        ListAdmin = LoadAdmin(connectionString); // Load admin list from database
        var top = Application.Top;

        var deleteAdminWindow = new Window("Delete Admin")
        {
            X = 0,
            Y = 0,
            Width = Dim.Fill() - 4,
            Height = Dim.Fill() - 4
        };
        top.Add(deleteAdminWindow);

        var adminIDLabel = new Label("Admin ID:")
        {
            X = 1,
            Y = 1
        };

        var adminIDField = new TextField("")
        {
            X = Pos.Right(adminIDLabel) + 1,
            Y = 1,
            Width = Dim.Fill() - 4
        };

        var deleteButton = new Button("Delete")
        {
            X = Pos.Center(),
            Y = Pos.Bottom(adminIDField) + 1
        };

        deleteButton.Clicked += () =>
        {
            try
            {
                if (int.TryParse(adminIDField.Text.ToString(), out int adminID))
                {
                    Admin ad = ListAdmin.Find(k => k.AdminID == adminID);

                    if (ad != null)
                    {
                        using (MySqlConnection connection = new MySqlConnection(connectionString))
                        {
                            connection.Open();
                            string deleteQuery = "DELETE FROM admins WHERE admin_id = @adminid; " +
                                                "DELETE FROM users WHERE user_customer_id = @adminid;";
                            MySqlCommand deleteCommand = new MySqlCommand(deleteQuery, connection);
                            deleteCommand.Parameters.AddWithValue("@adminid", ad.AdminID);
                            deleteCommand.ExecuteNonQuery();
                        }

                        ListAdmin.Remove(ad); // Remove from local list
                        MessageBox.Query("Success", "Successfully deleted staff!", "OK");
                        top.Remove(deleteAdminWindow); // Remove window from UI
                        superadmin.SuperAdminMenu(); // Return to super admin menu
                    }
                    else
                    {
                        MessageBox.ErrorQuery("Error", "Staff not found!", "OK");
                    }
                }
                else
                {
                    MessageBox.ErrorQuery("Error", "Invalid Admin ID!", "OK");
                }
            }
            catch (Exception ex)
            {
                MessageBox.ErrorQuery("Error", ex.Message, "OK");
            }
        };

        var closeButton = new Button("Close")
        {
            X = Pos.Center(),
            Y = Pos.Bottom(deleteButton) + 1
        };
        closeButton.Clicked += () =>
        {
            top.Remove(deleteAdminWindow);
            superadmin.SuperAdminMenu();
        };

        deleteAdminWindow.Add(adminIDLabel, adminIDField, deleteButton, closeButton);
    }

    public void DisplayStaff()
    {
        List<Admin> adminList = LoadAdmin(connectionString); // Load admin list from database
        var top = Application.Top;

        var displayStaffWindow = new Window("Display Staff")
        {
            X = 0,
            Y = 0,
            Width = Dim.Fill(),
            Height = Dim.Fill()
        };
        top.Add(displayStaffWindow);

        displayStaffWindow.FocusNext();

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
                INNER JOIN users usr ON adm.admin_id = usr.user_customer_id
                WHERE usr.role = 'admin'";
            MySqlCommand command = new MySqlCommand(query, connection);
            connection.Open();
            MySqlDataReader reader = command.ExecuteReader();

            var columnDisplayListAdmin = new string[]
            {
                "Staff ID", "Name", "Phone Number", "Email", "Gender", "Username", "Password"
            };

            // Add column headers
            for (int i = 0; i < columnDisplayListAdmin.Length; i++)
            {
                displayStaffWindow.Add(new Label(columnDisplayListAdmin[i])
                {
                    X = i * 20,
                    Y = 0,
                    Width = 20,
                    Height = 1
                });
            }

            int rowOffset = 1;
            while (reader.Read())
            {
                int adminId = Convert.ToInt32(reader["admin_id"]);
                string adminName = reader["admin_name"].ToString();
                string adminPhone = reader["admin_phone_number"].ToString();
                string adminEmail = reader["admin_email"].ToString();
                string adminGender = reader["admin_gender"].ToString();
                string username = reader["username"].ToString();
                string password = reader["password_hash"].ToString();

                // Add staff information
                displayStaffWindow.Add(new Label(adminId.ToString())
                {
                    X = 0,
                    Y = rowOffset,
                    Width = 20,
                    Height = 1
                });
                displayStaffWindow.Add(new Label(adminName)
                {
                    X = 1 * 20,
                    Y = rowOffset,
                    Width = 20,
                    Height = 1
                });
                displayStaffWindow.Add(new Label(adminPhone)
                {
                    X = 2 * 20,
                    Y = rowOffset,
                    Width = 20,
                    Height = 1
                });
                displayStaffWindow.Add(new Label(adminEmail)
                {
                    X = 3 * 20,
                    Y = rowOffset,
                    Width = 20,
                    Height = 1
                });
                displayStaffWindow.Add(new Label(adminGender)
                {
                    X = 4 * 20,
                    Y = rowOffset,
                    Width = 20,
                    Height = 1
                });
                displayStaffWindow.Add(new Label(username)
                {
                    X = 5 * 20,
                    Y = rowOffset,
                    Width = 20,
                    Height = 1
                });
                displayStaffWindow.Add(new Label(password)
                {
                    X = 6 * 20,
                    Y = rowOffset,
                    Width = 20,
                    Height = 1
                });

                rowOffset++;
            }

            reader.Close();
        }

        var btnClose = new Button("Close")
        {
            X = Pos.Center(),
            Y = Pos.Percent(100) - 1
        };
        btnClose.Clicked += () =>
        {
            top.Remove(displayStaffWindow);
            superadmin.SuperAdminMenu();
        };

        displayStaffWindow.Add(btnClose);
    }

    public void EditStaff()
    {
        Admin admin = new Admin();
        ListAdmin = LoadAdmin(connectionString); // Load admin list from database
        ListUsers = LoadUsers(connectionString); // Load user list from database

        var top = Application.Top;
        var editStaffWin = new Window("Edit Staff")
        {
            X = 0,
            Y = 0,
            Width = Dim.Fill() - 4,
            Height = Dim.Fill() - 4
        };
        top.Add(editStaffWin);

        var findAdminIDLabel = new Label("Find Staff ID:")
        {
            X = 1,
            Y = 1
        };
        var findAdminIDField = new TextField("")
        {
            X = Pos.Right(findAdminIDLabel) + 1,
            Y = 1,
            Width = Dim.Fill() - 4
        };

        var editAdminNameLabel = new Label("Staff Name:")
        {
            X = 1,
            Y = 5,
            Visible = false
        };

        var editAdminNameField = new TextField("")
        {
            X = Pos.Right(editAdminNameLabel) + 1,
            Y = 5,
            Width = Dim.Fill() - 4,
            Visible = false
        };

        var editAdminPhoneLabel = new Label("Staff Phone:")
        {
            X = 1,
            Y = 7,
            Visible = false
        };
        var editAdminPhoneField = new TextField("")
        {
            X = Pos.Right(editAdminPhoneLabel) + 1,
            Y = 7,
            Width = Dim.Fill() - 4,
            Visible = false
        };
        var editAdminEmailLabel = new Label("Staff Email:")
        {
            X = 1,
            Y = 9,
            Visible = false
        };

        var editAdminEmailField = new TextField("")
        {
            X = Pos.Right(editAdminEmailLabel) + 1,
            Y = 9,
            Width = Dim.Fill() - 4,
            Visible = false
        };

        var editAdminGenderLabel = new Label("Staff Gender:")
        {
            X = 1,
            Y = 11,
            Visible = false
        };
        var editAdminGenderField = new TextField("")
        {
            X = Pos.Right(editAdminGenderLabel) + 1,
            Y = 11,
            Width = Dim.Fill() - 4,
            Visible = false
        };

        var editAdminUserNameLabel = new Label("Username:")
        {
            X = 1,
            Y = 13,
            Visible = false
        };
        var editAdminUserNameField = new TextField("")
        {
            X = Pos.Right(editAdminUserNameLabel) + 1,
            Y = 13,
            Width = Dim.Fill() - 4,
            Visible = false
        };

        var editAdminPasswordLabel = new Label("Password:")
        {
            X = 1,
            Y = 15,
            Visible = false
        };
        var editAdminPasswordField = new TextField("")
        {
            X = Pos.Right(editAdminPasswordLabel) + 1,
            Y = 15,
            Width = Dim.Fill() - 4,
            Visible = false,
            Secret = true
        };

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
                admin.AdminID = int.Parse(findAdminIDField.Text.ToString());
                admin.AdminName = editAdminNameField.Text.ToString();
                admin.AdminPhone = editAdminPhoneField.Text.ToString();
                admin.AdminEmail = editAdminEmailField.Text.ToString();
                admin.AdminGender = editAdminGenderField.Text.ToString();
                string username = editAdminUserNameField.Text.ToString();
                string password = editAdminPasswordField.Text.ToString();

                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    string query = @"UPDATE admins 
                                    SET admin_name = @adminName, 
                                        admin_phone = @adminPhone, 
                                        admin_email = @adminEmail, 
                                        admin_gender = @adminGender 
                                    WHERE admin_id = @adminID;
                                    
                                    UPDATE users 
                                    SET username = @adminUserName, 
                                        password_hash = @adminPassword 
                                    WHERE user_customer_id = @adminID";
                    MySqlCommand command = new MySqlCommand(query, connection);
                    command.Parameters.AddWithValue("@adminID", admin.AdminID);
                    command.Parameters.AddWithValue("@adminName", admin.AdminName);
                    command.Parameters.AddWithValue("@adminPhone", admin.AdminPhone);
                    command.Parameters.AddWithValue("@adminEmail", admin.AdminEmail);
                    command.Parameters.AddWithValue("@adminGender", admin.AdminGender);
                    command.Parameters.AddWithValue("@adminUserName", username);
                    command.Parameters.AddWithValue("@adminPassword", password);

                    command.ExecuteNonQuery();
                }
                MessageBox.Query("Success", "Staff information has been updated!", "OK");
                top.Remove(editStaffWin);
                superadmin.SuperAdminMenu();
            }
            catch (Exception ex)
            {
                MessageBox.ErrorQuery("Error", ex.Message, "OK");
            }
        };

        var findButton = new Button("Find")
        {
            X = Pos.Center(),
            Y = 3
        };
        findButton.Clicked += () =>
        {
            try
            {
                int adminID = int.Parse(findAdminIDField.Text.ToString());
                var foundAdmin = ListAdmin.FirstOrDefault(a => a.AdminID == adminID);
                var foundUser = ListUsers.FirstOrDefault(a => a.CustomerID == adminID);

                if (foundAdmin != null)
                {
                    // Populate fields with staff information
                    editAdminNameField.Text = foundAdmin.AdminName;
                    editAdminPhoneField.Text = foundAdmin.AdminPhone;
                    editAdminEmailField.Text = foundAdmin.AdminEmail;
                    editAdminGenderField.Text = foundAdmin.AdminGender;
                    editAdminUserNameField.Text = foundUser.Username; // Assuming Username is stored in the Admin class
                    editAdminPasswordField.Text = ""; // Clear password field for security reasons

                    // Show edit fields and hide find controls
                    findAdminIDLabel.Visible = false;
                    findAdminIDField.Visible = false;
                    findButton.Visible = false;

                    editAdminNameLabel.Visible = true;
                    editAdminNameField.Visible = true;
                    editAdminPhoneLabel.Visible = true;
                    editAdminPhoneField.Visible = true;
                    editAdminEmailLabel.Visible = true;
                    editAdminEmailField.Visible = true;
                    editAdminGenderLabel.Visible = true;
                    editAdminGenderField.Visible = true;
                    editAdminUserNameLabel.Visible = true;
                    editAdminUserNameField.Visible = true;
                    editAdminPasswordLabel.Visible = true;
                    editAdminPasswordField.Visible = true;

                    saveButton.Visible = true;
                }
                else
                {
                    MessageBox.ErrorQuery("Error", "Staff not found!", "OK");
                }
            }
            catch (Exception ex)
            {
                MessageBox.ErrorQuery("Error", ex.Message, "OK");
            }
        };

        var closeButton = new Button("Close")
        {
            X = Pos.Center(),
            Y = 19
        };
        closeButton.Clicked += () =>
        {
            bool confirmed = MessageBox.Query("Confirm", "Are you sure you want to close?", "Yes", "No") == 0;
            if (confirmed)
            {
                top.Remove(editStaffWin);
                superadmin.SuperAdminMenu();
            }
        };

        editStaffWin.Add(findAdminIDLabel, findAdminIDField, findButton,
                            editAdminNameLabel, editAdminNameField,
                            editAdminPhoneLabel, editAdminPhoneField,
                            editAdminEmailLabel, editAdminEmailField,
                            editAdminGenderLabel, editAdminGenderField,
                            editAdminUserNameLabel, editAdminUserNameField,
                            editAdminPasswordLabel, editAdminPasswordField,
                            saveButton, closeButton);
    }

}

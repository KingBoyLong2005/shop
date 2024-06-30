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
    static List<Admin> LoadAdmin(string connectionString)
    {
        List<Admin> ListAdmin = new List<Admin>();

        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {   
            string query = "SELECT * FROM admins"; 
            MySqlCommand command = new MySqlCommand(query, connection);
            connection.Open();
            MySqlDataReader read = command.ExecuteReader();
            while (read.Read())
            {
                Admin ad = new Admin(); // các thuộc tính 
                ad.AdminID = read.GetInt32("admin_id");
                ad.AdminName = read.GetString("admin_name");
                ad.AdminPhone = read.GetString("admin_phone");
                ad.AdminEmail = read.GetString("cusmin_email");

                ListAdmin.Add(ad);
            }
        }
        return ListAdmin;
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
            "Admin's name", "Phone number", "Email", "Gender", "Username", "Pass"
        };

        int columnWidth = 20; // Tăng độ rộng cột

        // Thêm các Label cho tiêu đề cột
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

        // Xử lý sự kiện Clicked của nút tìm kiếm
        searchButton.Clicked += () =>
        {
            admin.Clear(); // Xóa dữ liệu khách hàng cũ

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

                // Hiển thị kết quả tìm kiếm
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
        ListAdmin = LoadAdmin(connectionString);
        var top = Application.Top;
        var deleteCustomerWin = new Window("Delete Admin")
        {
            X = 0,
            Y = 0,
            Width = Dim.Fill() - 4,
            Height = Dim.Fill() - 4
        };
        top.Add(deleteCustomerWin);

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

                if(int.TryParse(adminIDField.Text.ToString(), out int adminID))
                {
                    Admin ad = ListAdmin.Find(k => k.AdminID == adminID);

                    if (ad!= null)
                    {
                        using (MySqlConnection connection = new MySqlConnection(connectionString))
                        {
                            connection.Open();
                            string customerquery = "DELETE FROM admins WHERE admin_id = @adminid"+
                                                    "DELETE FROM users WHERE user_customer_id = @adminid";
                            MySqlCommand customercommand = new MySqlCommand(customerquery, connection);
                            customercommand.Parameters.AddWithValue("@adminid", ad.AdminID);
                            customercommand.ExecuteNonQuery();
                        }
                        ListAdmin.Remove(ad);
                        MessageBox.Query("Success", "Successfully deleted Staff!", "OK");
                        top.Remove(deleteCustomerWin);
                        superadmin.SuperAdminMenu();
                    }
                    else
                    {
                        MessageBox.ErrorQuery("Error", "Staff not found!", "OK");
                    }
                }
                else
                {
                    MessageBox.ErrorQuery("Error", "Invalid Staff ID!", "OK");
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
        top.Remove(deleteCustomerWin);
        superadmin.SuperAdminMenu();
    };

    deleteCustomerWin.Add(adminIDLabel, adminIDField, deleteButton, closeButton);
    }
    public void DisplayStaff()
{
    List<Admin> adminList = LoadAdmin(connectionString);
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
        string query = @"SELECT 
                            adm.admin_name,
                            adm.admin_phone AS admin_phone_number,
                            adm.admin_email,
                            adm.admin_gender,
                            usr.username,
                            usr.password_hash
                        FROM admins adm
                        INNER JOIN users usr ON adm.admin_id = usr.user_customer_id
                        WHERE  usr.role = 'admin'";
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
            string adminPhone = reader["admin_phone"].ToString();
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
        ListAdmin = LoadAdmin(connectionString);

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
        var editAdminUserName = new Label("Username's Staff")
        {
            X =1,
            Y = 13,
        };
        var editAdminUserNameField = new TextField("")
        {
            X = Pos.Right(editAdminUserName) + 1,
            Y = 13,
            Width = Dim.Fill() - 4,
            Visible = false
        };

        var editAdminPassword = new Label("Password's Staff")
        {
            X =1,
            Y = 13,
        };
        var editAdminPasswordField = new TextField("")
        {
            X = Pos.Right(editAdminPassword) + 1,
            Y = 13,
            Width = Dim.Fill() - 4,
            Visible = false
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
                    string query = "UPDATE admins SET admin_name = @adminName, admin_phone = @adminPhone, admin_email = @adminEmail, admin_gender = @adminGender WHERE admin_id = @adminID"+
                                    "UPATE users SET username = @adminUserName, password_hash = @adminPassword WHERE user_customer_id = @adminID";
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

                if (foundAdmin != null)
                {
                    // Populate fields with staff information
                    editAdminNameField.Text = foundAdmin.AdminName;
                    editAdminPhoneField.Text = foundAdmin.AdminPhone;
                    editAdminEmailField.Text = foundAdmin.AdminEmail;
                    editAdminGenderField.Text = foundAdmin.AdminGender;

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

                    saveButton.Visible = true;
                }
                else
                {
                    MessageBox.ErrorQuery("Error", "Admin not found!", "OK");
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
            admin.AdminMenu();
            }
        };

        editStaffWin.Add(findAdminIDLabel, findAdminIDField, findButton,
                            editAdminNameLabel, editAdminNameField,
                            editAdminPhoneLabel, editAdminPhoneField,
                            editAdminEmailLabel, editAdminEmailField,
                            editAdminGenderLabel, editAdminGenderField,
                            editAdminUserName, editAdminUserNameField,
                            editAdminPassword, editAdminPasswordField,
                            saveButton, closeButton);
    }
    public void RegisterAdmin()
    {
        program.Register("admin");
    }
}

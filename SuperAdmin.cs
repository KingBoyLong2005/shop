using System;
using System.Text;
using System.Data;
using System.Collections.Generic;   //Import namesapce to use function
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using Terminal.Gui;

public class SuperAdmin
{

    public static Cart userCart = new Cart();
    public static Customers cus = new Customers();
    public static Products pd = new Products();
    public static Orders order = new Orders();
    public static Admin admin= new Admin();
    public static Program program = new Program();
    public static Categories cate = new Categories();
    public static Users user = new Users();

    public static string connectionString = Configuration.ConnectionString;

    // Display the Super Admin Menu
    public void SuperAdminMenu()
    {
        // Initialize the application
        var top = Application.Top;
        Application.Init();
        
        // Create the main window for the Super Admin Menu
        var SuperAdminMenu = new Window()
        {
            Title = "Manager Menu",
            X = 0,
            Y = 0,
            Width = Dim.Fill(),
            Height = Dim.Fill()
        };
        top.Add(SuperAdminMenu);

        // Create the left frame for function buttons
        var leftFrame = new FrameView("Function")
        {
            X = 0,
            Y = 0,
            Width = Dim.Percent(30), // Occupy 30% of the window width
            Height = Dim.Fill() // Occupy the entire height
        };
        SuperAdminMenu.Add(leftFrame);

        // Create the top right frame for the welcome message
        var rightTopFrame = new FrameView()
        {
            X = Pos.Percent(30), // Start from 30% of the window width
            Y = 0,
            Width = Dim.Fill(), // Occupy the remaining width
            Height = Dim.Percent(50) // Occupy 50% of the height
        };
        SuperAdminMenu.Add(rightTopFrame);

        // Create the bottom right frame for the number of products sold
        var rightBottomFrame = new FrameView("Statistical")
        {
            X = Pos.Percent(30), // Start from 30% of the window width
            Y = Pos.Percent(50), // Start from the middle of the height
            Width = Dim.Fill(), // Occupy the remaining width
            Height = Dim.Fill() // Occupy the remaining height
        };
        SuperAdminMenu.Add(rightBottomFrame);
        var btnDisplayProduct = new Button("Display Product")
        {
            X = 2,
            Y = 2
        };
        btnDisplayProduct.Clicked += () =>
        {
            try
            {
                top.Remove(SuperAdminMenu);
                pd.DisplayProduct("superadmin");
            }
            catch
            {
                MessageBox.ErrorQuery("Error", "An error occurred while displaying the product. Please try again.", "OK");
            }
        };

        var btnFindProduct = new Button("FindProduct")
        {
            X = 2,
            Y = 3
        };
        btnFindProduct.Clicked += () =>
        {
            try
            {
                top.Remove(SuperAdminMenu);
                pd.FindProduct("superadmin");
            }
            catch
            {
                MessageBox.ErrorQuery("Error", "An error occurred while finding the product. Please try again.", "OK");
            }
        };

        var btnAddStaff = new Button("Add Staff")
        {
            X = 2,
            Y = 4
        };
        btnAddStaff.Clicked += () =>
        {
            try
            {
                top.Remove(SuperAdminMenu);
                admin.AddStaff();
            }
            catch
            {
                MessageBox.ErrorQuery("Error", "An error occurred while adding staff. Please try again.", "OK");
            }
        };

        var btnDisplayStaff = new Button("Display Staff")
        {
            X = 2,
            Y = 5
        };
        btnDisplayStaff.Clicked += () =>
        {
            try
            {
                top.Remove(SuperAdminMenu);
                admin.DisplayStaff();
            }
            catch
            {
                MessageBox.ErrorQuery("Error", "An error occurred while displaying staff. Please try again.", "OK");
            }
        };

        var btnAddCategory = new Button("Add Category")
        {
            X = 2,
            Y = 6
        };
        btnAddCategory.Clicked += () =>
        {
            try
            {
                top.Remove(SuperAdminMenu);
                cate.AddCategory();
            }
            catch
            {
                MessageBox.ErrorQuery("Error", "An error occurred while adding the category. Please try again.", "OK");
            }
        };

        var btnDisplayCategory = new Button("Display Category")
        {
            X = 2,
            Y = 7
        };
        btnDisplayCategory.Clicked += () =>
        {
            try
            {
                top.Remove(SuperAdminMenu);
                cate.DisplayCategories("superadmin");
            }
            catch
            {
                MessageBox.ErrorQuery("Error", "An error occurred while displaying categories. Please try again.", "OK");
            }
        };

        var btnAddCustomer = new Button("Add Customer")
        {
            X = 2,
            Y = 8
        };
        btnAddCustomer.Clicked += () =>
        {
            try
            {
                top.Remove(SuperAdminMenu);
                cus.AddCustomer();
            }
            catch
            {
                MessageBox.ErrorQuery("Error", "An error occurred while adding the customer. Please try again.", "OK");
            }
        };

        var btnDisplayCustomer = new Button("Display Customer")
        {
            X = 2,
            Y = 9
        };
        btnDisplayCustomer.Clicked += () =>
        {
            try
            {
                top.Remove(SuperAdminMenu);
                cus.DisplayCustomers("superadmin");
            }
            catch
            {
                MessageBox.ErrorQuery("Error", "An error occurred while displaying customers. Please try again.", "OK");
            }
        };

        // Add logout button
        var btnLogout = new Button("Logout")
        {
            X = Pos.Center(),
            Y = Pos.Percent(100) - 1,
        };
        btnLogout.Clicked += () =>
        {
            top.Remove(SuperAdminMenu);
            user.Login();
        };

        // Add all buttons to the left frame
        leftFrame.Add(btnDisplayProduct, btnFindProduct, btnAddStaff, btnDisplayStaff, btnAddCategory, btnDisplayCategory, btnAddCustomer, btnDisplayCustomer, btnLogout);

        // Add a welcome label to the top right frame
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
}

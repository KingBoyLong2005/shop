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
    public static Products pd = new Products();
    public static Orders order = new Orders();
    public static Admin admin= new Admin();
    public static Program program = new Program();

    public static string connectionString = Configuration.ConnectionString;
    public static int currentCustomerID = SessionData.Instance.CurrentCustomerID;

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
        var rightTopFrame = new FrameView("Welcome")
        {
            X = Pos.Percent(30), // Start from 30% of the window width
            Y = 0,
            Width = Dim.Fill(), // Occupy the remaining width
            Height = Dim.Percent(50) // Occupy 50% of the height
        };
        SuperAdminMenu.Add(rightTopFrame);

        // Create the bottom right frame for the number of products sold
        var rightBottomFrame = new FrameView("Number of products sold")
        {
            X = Pos.Percent(30), // Start from 30% of the window width
            Y = Pos.Percent(50), // Start from the middle of the height
            Width = Dim.Fill(), // Occupy the remaining width
            Height = Dim.Fill() // Occupy the remaining height
        };
        SuperAdminMenu.Add(rightBottomFrame);
        
        // Add button to add staff
        var btnAddStaff = new Button("Add Staff")
        {
            X = 2,
            Y = 2
        };
        btnAddStaff.Clicked += () =>
        {
            top.Remove(SuperAdminMenu);
            admin.AddStaff();
        };

        // Add button to find staff
        var btnFindStaff = new Button("Find Staff")
        {
            X = 2,
            Y = 4,
        };
        btnFindStaff.Clicked += () =>
        {
            top.Remove(SuperAdminMenu);
            admin.FindStaff();
        };

        // Add button to edit staff
        var btnEditStaff = new Button("Edit Staff")
        {
            X = 2,
            Y = 6
        };
        btnEditStaff.Clicked += () =>
        {
            top.Remove(SuperAdminMenu);
            admin.EditStaff();
        };

        // Add button to delete staff
        var btnDeleteStaff = new Button("Delete Staff")
        {
            X = 2,
            Y = 8
        };
        btnDeleteStaff.Clicked += () =>
        {
            top.Remove(SuperAdminMenu);
            admin.DeleteStaff();
        };

        // Add button to display staff
        var btnDisplayStaff = new Button("Display Staff")
        {
            X = 2,
            Y = 10
        };
        btnDisplayStaff.Clicked += () =>
        {
            top.Remove(SuperAdminMenu);
            admin.DisplayStaff();
        };

        // Add button to display products
        var btnDisplayProduct = new Button("Display Product")
        {
            X = 2,
            Y = 12
        };
        btnDisplayProduct.Clicked += () =>
        {
            top.Remove(SuperAdminMenu);
            pd.DisplayProduct("superadmin");
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
            program.Login();
        };

        // Add all buttons to the left frame
        leftFrame.Add(btnAddStaff, btnFindStaff, btnEditStaff, btnDeleteStaff, btnDisplayStaff, btnDisplayProduct, btnLogout);

        // Add a welcome label to the top right frame
        var rightTopLabel = new Label("Manager")
        {
            X = Pos.Center(),
            Y = Pos.Center()
        };
        rightTopFrame.Add(rightTopLabel);

        // Connect to the database to get the total number of orders
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            connection.Open();
            string query = "SELECT COUNT(*) FROM orders";
            MySqlCommand command = new MySqlCommand(query, connection);
            object result = command.ExecuteScalar(); // Execute the query and get the result
            int count = Convert.ToInt32(result);

            // Add the total order count label to the bottom right frame
            var countOrder = new Label($"Total orders: {count}")
            {
                X = Pos.Center(),
                Y = Pos.Center()
            };
            rightBottomFrame.Add(countOrder);
        }
    }

}

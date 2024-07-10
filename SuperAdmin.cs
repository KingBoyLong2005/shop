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
    public static Categories cate = new Categories();

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
            try
            {
                top.Remove(SuperAdminMenu);
                admin.AddStaff();
            }
            catch (Exception ex)
            {
                MessageBox.ErrorQuery("Error", ex.Message, "OK");
            }
        };

        // Add button to find staff
        var btnFindStaff = new Button("Find Staff")
        {
            X = 2,
            Y = 4,
        };
        btnFindStaff.Clicked += () =>
        {
            try
            {
                top.Remove(SuperAdminMenu);
                admin.FindStaff();
            }
            catch (Exception ex)
            {
                MessageBox.ErrorQuery("Error", ex.Message, "OK");
            }
        };

        // Add button to edit staff
        var btnEditStaff = new Button("Edit Staff")
        {
            X = 2,
            Y = 6
        };
        btnEditStaff.Clicked += () =>
        {
            try
            {
            top.Remove(SuperAdminMenu);
            admin.EditStaff();
            }
            catch (Exception ex)
            {
                MessageBox.ErrorQuery("Error", ex.Message, "OK");
            }
        };

        // Add button to delete staff
        var btnDeleteStaff = new Button("Delete Staff")
        {
            X = 2,
            Y = 8
        };
        btnDeleteStaff.Clicked += () =>
        {
            try
            {
            top.Remove(SuperAdminMenu);
            admin.DeleteStaff();
            }
            catch (Exception ex)
            {
                MessageBox.ErrorQuery("Error", ex.Message, "OK");
            }
        };

        // Add button to display staff
        var btnDisplayStaff = new Button("Display Staff")
        {
            X = 2,
            Y = 10
        };
        btnDisplayStaff.Clicked += () =>
        {
            try
            {
            top.Remove(SuperAdminMenu);
            admin.DisplayStaff();
            }
            catch (Exception ex)
            {
                MessageBox.ErrorQuery("Error", ex.Message, "OK");
            }
        };

        // Add button to display products
        var btnAddCategory = new Button("Add Category")
        {
            X = 2,
            Y = 12
        };

        btnAddCategory.Clicked += () =>
        {
            try
            {
            top.Remove(SuperAdminMenu);
            cate.AddCategory();
            }
            catch (Exception ex)
            {
                MessageBox.ErrorQuery("Error", ex.Message, "OK");
            }
        };
        var btnDeleteCategory = new Button("Delete Category")
        {
            X = 2,
            Y = 14
        };
        btnDeleteCategory.Clicked += () =>
        {
            try
            {
                top.Remove(SuperAdminMenu);
                cate.DeleteCategory();
            }
            catch (Exception ex)
            {
                MessageBox.ErrorQuery("Error", ex.Message, "OK");
            }
        };
        var btnbDisplayCategory = new Button("Display Category")
        {
            X = 2,
            Y = 16
        };
        btnbDisplayCategory.Clicked += () =>
        {
            try
            {
                top.Remove(SuperAdminMenu);
                cate.Displaycategorys();
            }
            catch (Exception ex)
            {
                MessageBox.ErrorQuery ("Error", ex.Message,"OK");
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
            program.Login();
        };

        // Add all buttons to the left frame
        leftFrame.Add(btnAddStaff, btnFindStaff, btnEditStaff, btnDeleteStaff, btnDisplayStaff, btnAddCategory, btnDeleteCategory, btnbDisplayCategory, btnLogout);

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

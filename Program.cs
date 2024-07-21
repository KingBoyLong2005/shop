﻿using System;
using System.Text;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;                            //Import namesapce to use function
using System.Threading.Tasks;
using System.Security.Cryptography.X509Certificates;
using Terminal.Gui;
using Microsoft.VisualBasic;

public class Program
{
    // Declare static variables
    public static string connectionString;

    // Initialize lists for various entities
    public static List<Products> ListProducts = new List<Products>();
    public static List<Categories> ListCategories = new List<Categories>();
    public static List<Users> ListUsers = new List<Users>();
    public static List<Customers> ListCustomers = new List<Customers>();
    public static List<Cart> ListCarts = new List<Cart>();

    // Create instances of various entities
    public static Cart userCart = new Cart();
    public static Products pd = new Products();
    public static Orders order = new Orders();
    public static Customers customer = new Customers();
    public static Program program = new Program();
    public static Users user = new Users();
    public static Admin admin = new Admin();
    public static SuperAdmin superadmin = new SuperAdmin();

    // Static constructor to initialize the connection string
    static Program()
    {
        // Prompt user for database password
        Console.Write("Enter the database password: ");
        string password = Console.ReadLine();

        // Base connection string with a placeholder for the password
        string baseConnectionString = "Server=localhost;Database=shop;Uid=root;Pwd=@pass";

        // Replace the placeholder with the actual password
        connectionString = baseConnectionString.Replace("@pass", password);
        Configuration.ConnectionString = connectionString;
    }
    // Main method
    static void Main()
    {
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            connection.Open();
            string query = @"SET foreign_key_checks = 0;
                            SET SQL_SAFE_UPDATES = 0;";
            MySqlCommand command = new MySqlCommand(query, connection);
            command.ExecuteNonQuery();
        }
        // Initialize the application and set colors
        Application.Init();

        // Set base colors
        Colors.Base.Normal = Application.Driver.MakeAttribute(Color.White, Color.Black);
        Colors.Base.Focus = Application.Driver.MakeAttribute(Color.White, Color.DarkGray);

        // Set colors for dialogs
        Colors.Dialog.Normal = Application.Driver.MakeAttribute(Color.White, Color.Black);
        Colors.Dialog.Focus = Application.Driver.MakeAttribute(Color.White, Color.DarkGray);
        Colors.Dialog.HotNormal = Application.Driver.MakeAttribute(Color.White, Color.Black);
        Colors.Dialog.HotFocus = Application.Driver.MakeAttribute(Color.White, Color.DarkGray);

        // Set colors for menus
        Colors.Menu.Normal = Application.Driver.MakeAttribute(Color.White, Color.Black);
        Colors.Menu.Focus = Application.Driver.MakeAttribute(Color.White, Color.DarkGray);
        Colors.Menu.HotNormal = Application.Driver.MakeAttribute(Color.White, Color.Black);
        Colors.Menu.HotFocus = Application.Driver.MakeAttribute(Color.White, Color.DarkGray);

        // Set colors for errors
        Colors.Error.Normal = Application.Driver.MakeAttribute(Color.White, Color.Red);
        Colors.Error.Focus = Application.Driver.MakeAttribute(Color.White, Color.DarkGray);

        // Set colors for top level
        Colors.TopLevel.Normal = Application.Driver.MakeAttribute(Color.White, Color.Black);
        Colors.TopLevel.Focus = Application.Driver.MakeAttribute(Color.White, Color.DarkGray);

        // Initialize the application and run the login method
        Application.Init();
        user.Login();
        Application.Run();  
        
    }
}

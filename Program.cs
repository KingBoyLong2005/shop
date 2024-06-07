using System;
using System.Text;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using Spectre.Console;
using System.Security.Cryptography.X509Certificates;

class Program
{
    public static string connectionString;
    public static List<Products> ListProducts = new List<Products>();
    public static List<Categories> ListCategories = new List<Categories>();
    static Program()
    {
        // Hỏi mật khẩu từ người dùng
        Console.Write("Enter the database password: ");
       string password = Console.ReadLine();

        // Gốc chuỗi kết nối với placeholder @pass
        string baseConnectionString = "Server=localhost;Database=shop;Uid=root;Pwd=@pass";

        // Thay thế @pass bằng mật khẩu thực
        connectionString = baseConnectionString.Replace("@pass", password);
    }

    static List<Products> LoadProducts(string connectionString)
    {
        List<Products> ListProduct = new List<Products>();

        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {   
            string query = "SELECT * FROM products"; 
            MySqlCommand command = new MySqlCommand(query, connection);
            connection.Open();
            MySqlDataReader read = command.ExecuteReader();
            while (read.Read())
            {
                Products sp = new Products();
                // Nạp các thuộc tính 
                sp.ProductID = read.GetInt32("product_id");
                sp.ProductName = read.GetString("product_name");
                sp.ProductDescription = read.GetString("product_description");
                sp.ProductPrice = read.GetDecimal("product_price");
                sp.ProductStockQuantity = read.GetInt32("product_stock_quantity");
                sp.ProductBrand = read.GetString("product_brand");
                sp.ProductCategoryID = read.GetInt32("product_category_id");
                sp.ProductImage = read.GetString("product_image");

                ListProduct.Add(sp);
            }
        }
        return ListProduct;
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
        MainMenu();
    }

    static void MainMenu()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("==== MAIN MENU ====");
            Console.WriteLine("1. Customers");
            Console.WriteLine("2. Products");
            Console.WriteLine("3. Categories");
            Console.WriteLine("4. Orders");
            Console.WriteLine("0. Quit");
            Console.Write("Please pick a function: ");

            string choice = Console.ReadLine();

            switch (choice)
            {
                case "2":
                    ProductMenu();
                    break;
                case "0":
                    return;
                default:
                    Console.WriteLine("Invalid option, please choose again.");
                    Console.ReadKey();
                    break;
            }
        }
    }

    static void ProductMenu()
    {
        Console.Clear();
        Console.OutputEncoding = Encoding.UTF8;
        while (true)
        {
            var table = new Table();
            table.Border(TableBorder.Rounded);
            table.AddColumn(new TableColumn("Menu"));

            // Create the menu prompt
            var menuPrompt = new SelectionPrompt<string>()
                .Title("=== PRODUCT MENU ===")
                .PageSize(20)
                .MoreChoicesText("[grey](Move up and down to reveal more choice)[/]")
                .AddChoices(new[] {
                    "Product List", "Add a new product", "Find product", 
                    "Delete product", "Edit product informations", "Return to the Main Menu",
                });

            // Prompt the user for a choice and store it
            var choice = AnsiConsole.Prompt(menuPrompt);

            // Create a panel for the selected choice
            var panel = new Panel($"You selected: [bold yellow]{choice}[/]")
                .Header("[bold yellow]Product Menu[/]")
                .Expand()
                .Border(BoxBorder.Rounded)
                .BorderStyle(Style.Parse("green"));

            // Add the menu and the choice result to the table
            table.AddRow(panel);

            // Display the updated table
            AnsiConsole.Clear();
            AnsiConsole.Write(table);
            if (choice == "Product List")
            {
                DisplayProduct(ListProducts);
            }
            else if (choice == "Add a new product")
            {
               AddProduct(ListProducts);
            }
            else if (choice == "Find product")
            {

            }
            else if (choice == "Delete product")
            {

            }
            else if (choice == "Edit product informations")
            {

            }
            else if (choice == "Return to the Main Menu")
            {
                return;
            }
            }
        }
    

    static void DisplayProduct(List<Products> ListProducts)
    {
        ListProducts = LoadProducts(connectionString);
        ListCategories = LoadCategory(connectionString);
        var table = new Table();
        Console.OutputEncoding = Encoding.UTF8;
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {   
            string query = @"SELECT 
                        p.product_name, 
                        p.product_stock_quantity, 
                        p.product_description, 
                        p.product_price, 
                        c.category_name, 
                        p.product_brand,
                        p.product_image
                    FROM products p
                    INNER JOIN categories c ON p.product_category_id = c.category_id;";
            MySqlCommand command = new MySqlCommand(query, connection);
            connection.Open();
            MySqlDataReader read = command.ExecuteReader();
            Console.WriteLine("=== List Product ===");
            table.AddColumns(new TableColumn("Product's name").Centered());
            table.AddColumns(new TableColumn("Stock quantity").Centered());
            table.AddColumns(new TableColumn("Description").Centered());
            table.AddColumns(new TableColumn("Price").Centered());
            table.AddColumns(new TableColumn("Category's name").Centered());
            table.AddColumns(new TableColumn("Brand").Centered());
            table.AddColumns(new TableColumn("Image").Centered());

            while (read.Read())
            {
                table.AddRow(new string[] {
                    read["product_name"].ToString(),
                    read["product_stock_quantity"].ToString(),
                    read["product_description"].ToString(),
                    read["product_price"].ToString(),
                    read["category_name"].ToString(),
                    read["product_brand"].ToString(),
                    read["product_image"].ToString()
                });
            }
            read.Close();
            AnsiConsole.Write(table);
            Console.WriteLine("Press any to continue");
            Console.ReadKey();
        }
    }

 
static void AddProduct(List<Products> ListProducts)
    {
        Console.WriteLine("New product information: ");
        Products sp = new Products();
        Console.Write("Product ID: ");
        sp.ProductID = int.Parse(Console.ReadLine());
        Console.Write("Product name: ");
        sp.ProductName = Console.ReadLine();
        Console.Write("Product category ID: ");
        sp.ProductCategoryID = int.Parse(Console.ReadLine());
        Console.Write("Product price: ");
        sp.ProductPrice = decimal.Parse(Console.ReadLine());
        Console.Write("Product description: ");
        sp.ProductDescription = Console.ReadLine();
        Console.Write("Product brand: ");
        sp.ProductBrand = Console.ReadLine();

        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            connection.Open();
            string query = "INSERT INTO products (product_id, product_name, product_category_id, product_price, product_description, product_brand) VALUES (@ProductID, @ProductName, @ProductCategoryID, @ProductPrice, @ProductDescription, @ProductBrand)";
            MySqlCommand command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@ProductID", sp.ProductID);
            command.Parameters.AddWithValue("@ProductName", sp.ProductName);
            command.Parameters.AddWithValue("@ProductCategoryID", sp.ProductCategoryID);
            command.Parameters.AddWithValue("@ProductPrice", sp.ProductPrice);
            command.Parameters.AddWithValue("@ProductDescription", sp.ProductDescription);
            command.Parameters.AddWithValue("@ProductBrand", sp.ProductBrand);
            command.ExecuteNonQuery();
        }

        ListProducts.Add(sp);
        Console.WriteLine("Successfully added new product!");
    }
}
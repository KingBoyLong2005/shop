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

    static void Main()
    {
        DisplayProduct(ListProducts);
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
            table.AddColumn(new TableColumn("Product's name").Centered());
            table.AddColumn(new TableColumn("Stock quantity").Centered());
            table.AddColumn(new TableColumn("Description").Centered());
            table.AddColumn(new TableColumn("Price").Centered());
            table.AddColumn(new TableColumn("Category's name").Centered());
            table.AddColumn(new TableColumn("Brand").Centered());
            table.AddColumn(new TableColumn("Image").Centered());

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
            Console.WriteLine("Ấn bất kì để tiếp tục");
            Console.ReadKey();
        }
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
    
}

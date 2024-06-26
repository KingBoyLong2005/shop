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
    public string AdminPassword { get; set; }
    public string AdminEmail { get; set; }
    public DateOnly AdminCreatedDate { get; set; }

    public static Cart userCart = new Cart();
    public static Products pd = new Products();
    public static Orders order = new Orders();
    public static Program program = new Program();
    public static string connectionString = Configuration.ConnectionString;
    public static int currentCustomerID = SessionData.Instance.CurrentCustomerID;
    public void AdminMenu()
    {
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
            pd.DisplayProduct("admin");
        };
        var btnOrderForCustomer = new Button("Display Products")
        {
            X = 2,
            Y = 4
        };
        btnOrderForCustomer.Clicked += () =>
        {
            top.Remove(adminMenu);
            order.DisplayProductToOrderForCustomer();
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
        leftFrame.Add(btnDisplayProduct, btnOrderForCustomer, LogoutButton);

        string adminName = "";
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            string query = "SELECT admin_name FROM admins WHERE cadmin_id = @AdminID";
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
            string query = "SELECT COUNT(*) FROM orders";
            MySqlCommand command = new MySqlCommand(query, connection);
            object result = command.ExecuteScalar(); // Thực thi truy vấn và lấy kết quả
            int count = Convert.ToInt32(result);
            var countOrder = new Label($"count")
            {
                X = 1 ,
                Y = 1
            };
        rightBottomFrame.Add(countOrder);
        }
    }
}

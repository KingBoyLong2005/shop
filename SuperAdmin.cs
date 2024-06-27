using System;
using System.Text;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Security.Cryptography.X509Certificates;
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

    public void SuperAdminMenu()
    {
        var top = Application.Top;

        var adminMenu = new Window()
        {
            Title = "Manager Menu",
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
        
        var btnAddStaff = new Button("Add Staff")
        {
            X = 2,
            Y = 2
        };
        btnAddStaff.Clicked += () =>
        {
            top.Remove(adminMenu);
            admin.RegisterAdmin();
        };
        
        leftFrame.Add(btnAddStaff);

        var rightTopLabel = new Label("Manager")
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

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
    public int SuperAdminID { get; set; }
    public string SuperAdminName { get; set; }
    public string SuperAdminPassword { get; set; }
    public string SuperAdminEmail { get; set; }
    public DateOnly SuperAdminCreatedDate { get; set; }
    public static Cart userCart = new Cart();
    public static Products pd = new Products();
    public static Orders order = new Orders();
    public static Program program = new Program();
    public static string connectionString = Configuration.ConnectionString;
    public static int currentCustomerID = SessionData.Instance.CurrentCustomerID;

    public void SuperAdminMenu()
    {
        var top = Application.Top;
        var superAdminMenu = new Window()
        {
            Title = "Super Admin Menu",
            X = 0,
            Y = 0,
            Width = Dim.Fill(),
            Height = Dim.Fill()
        };
        top.Add(superAdminMenu);

        var closeButton = new Button("Close")
        {
            X = Pos.Center(),
            Y = Pos.Percent(100) - 3
        };
        closeButton.Clicked += () => 
        {
            top.Remove(superAdminMenu);
            program.Login();
        };
        superAdminMenu.Add(closeButton);

    }
}

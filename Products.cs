using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using Spectre.Console;


public class Products
{
    public int ProductID { get; set; }
    public string ProductName { get; set;}
    public int ProductStockQuantity { get; set; }
    public string ProductDescription { get; set; }
    public decimal ProductPrice { get; set; }
    public int ProductCategoryID { get; set; }
    public string ProductBrand { get; set; }
    public string ProductImage { get; set; }

   
}

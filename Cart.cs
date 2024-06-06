using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class Cart
{
    public int CartID { get; set; }
    public int CartCustomerID { get; set; }
    public int CartProductID { get; set; }
    public int CartQuantity { get; set; }
    public int CartOrderID { get; set; }
    public decimal CartProductPrice { get; set; }
    public decimal CartOrderPrice { get; set; }
    public int CartTotalProduct { get; set; }
}

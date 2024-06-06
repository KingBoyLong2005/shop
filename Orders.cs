using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class Orders
{
    public int OrderID { get; set; }
    public int OrderCustomerID { get; set; }
    public Decimal OrderTotalPrice { get; set; }
    public DateTime OrderDate { get; set; }
    public DateTime OrderDeliveryDate { get; set; }
    public string OrderPaymentMethod { get; set; }
    public DateTime OrderPaymentDate { get; set; }
    public string OrderStatus { get; set; }
    public string OrderAddress { get; set; }

    }

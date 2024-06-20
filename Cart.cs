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
    

    public static List<CartItem> CartItems { get; set; } = new List<CartItem>();

    public void AddItem(Products product, int quantity)
    {
        var cartItem = CartItems.FirstOrDefault(c => c.Product.ProductID == product.ProductID);
        if (cartItem != null)
        {
            cartItem.Quantity += quantity;
        }
        else
        {
            CartItems.Add(new CartItem { Product = product, Quantity = quantity });
        }
    }

    public void RemoveItem(int productID)
    {
        var cartItem = CartItems.FirstOrDefault(c => c.Product.ProductID == productID);
        if (cartItem != null)
        {
            CartItems.Remove(cartItem);
        }
    }
    
}


public class CartItem
{
    public Products Product { get; set; }
    public int Quantity { get; set; }
    
}

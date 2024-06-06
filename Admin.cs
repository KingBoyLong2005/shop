using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class Admin
{
    public int AdminID { get; set; }
    public string AdminName { get; set; }
    public string AdminPassword { get; set; }
    public string AdminEmail { get; set; }
    public DateOnly AdminCreatedDate { get; set; }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class Users
{
    public int UserId { get; set; }
    public string Username { get; set; }
    public string PasswordHash { get; set; }
    public UserRole Role { get; set; }        
}
public enum UserRole
{
    User,
    Admin,
    SuperAdmin
}

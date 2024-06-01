using System;
using System.Collections.Generic;

namespace AuthApp
{
    class Program
    {
        static Dictionary<string, (string Password, string Role)> users = new Dictionary<string, (string Password, string Role)>
        {
            { "admin", ("admin123", "admin") },
            { "user1", ("user123", "customer") },
        };

        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to the AuthApp");
            Console.Write("Enter username: ");
            string username = Console.ReadLine();
            Console.Write("Enter password: ");
            string password = Console.ReadLine();

            if (Authenticate(username, password, out string role))
            {
                Console.WriteLine($"Login successful! Role: {role}");
                if (role == "admin")
                {
                    AdminMenu();
                }
                else if (role == "customer")
                {
                    CustomerMenu();
                }
            }
            else
            {
                Console.WriteLine("Invalid username or password.");
            }
        }

        static bool Authenticate(string username, string password, out string role)
        {
            role = null;
            if (users.ContainsKey(username) && users[username].Password == password)
            {
                role = users[username].Role;
                return true;
            }
            return false;
        }

        static void AdminMenu()
        {
            Console.WriteLine("Welcome to the Admin Menu!");
            Console.WriteLine("1. View all users");
            Console.WriteLine("2. Add a user");
            Console.WriteLine("3. Exit");
            string choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    ViewAllUsers();
                    break;
                case "2":
                    AddUser();
                    break;
                case "3":
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("Invalid choice. Exiting.");
                    Environment.Exit(0);
                    break;
            }
        }

        static void CustomerMenu()
        {
            Console.WriteLine("Welcome to the Customer Menu!");
            Console.WriteLine("1. View profile");
            Console.WriteLine("2. Exit");
            string choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    ViewProfile();
                    break;
                case "2":
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("Invalid choice. Exiting.");
                    Environment.Exit(0);
                    break;
            }
        }

        static void ViewAllUsers()
        {
Console.WriteLine("List of all users:");
            foreach (var user in users)
            {
                Console.WriteLine($"Username: {user.Key}, Role: {user.Value.Role}");
            }
        }

        static void AddUser()
        {
            Console.Write("Enter new username: ");
            string newUsername = Console.ReadLine();
            Console.Write("Enter password for new user: ");
            string newPassword = Console.ReadLine();
            Console.Write("Enter role for new user (admin/customer): ");
            string newRole = Console.ReadLine();

            if (newRole == "admin" || newRole == "customer")
            {
                users[newUsername] = (newPassword, newRole);
                Console.WriteLine("New user added successfully!");
            }
            else
            {
                Console.WriteLine("Invalid role. User not added.");
            }
        }

        static void ViewProfile()
        {
            Console.WriteLine("This is your profile.");
            // Implement profile viewing logic here
        }
    }
}
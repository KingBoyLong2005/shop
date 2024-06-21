using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using Spectre.Console;
using Terminal.Gui;

public class Customers
{
    public int CustomerID {get; set;}
    public string CustomerName {get; set;}
    public string CustomerPhone {get; set;}
    public string CustomerAddress {get; set;}
    public string CustomerEmail {get; set;}
    public string CustomerGender {get; set;}
    public DateTime CustomerDateOfBirth {get; set;}
    public int CustomerCount {get; set;}
    public decimal CustomerTotalSpent {get; set;}
    public static string connectionString = Configuration.ConnectionString;
    public static Customers cus = new Customers();
    public static List<Customers> ListCustomers = new List<Customers>();
    static List<Customers> LoadCustomers(string connectionString)
    {
        List<Customers> ListCustomers = new List<Customers>();

        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {   
            string query = "SELECT * FROM customers"; 
            MySqlCommand command = new MySqlCommand(query, connection);
            connection.Open();
            MySqlDataReader read = command.ExecuteReader();
            while (read.Read())
            {
                Customers cus = new Customers();
                // Nạp các thuộc tính 
                cus.CustomerID = read.GetInt32("customer_id");
                cus.CustomerName = read.GetString("customer_name");
                cus.CustomerPhone = read.GetString("customer_phone_number");
                cus.CustomerAddress = read.GetString("customer_address");
                cus.CustomerEmail = read.GetString("customer_email");
                cus.CustomerGender = read.GetString("customer_gender");
                cus.CustomerDateOfBirth = read.GetDateTime("customer_dateofbirth");
                cus.CustomerCount = read.GetInt32("customer_count");
                cus.CustomerTotalSpent = read.GetDecimal("customer_totalspent");

                ListCustomers.Add(cus);
            }
        }
        return ListCustomers;
    }
    public void CustomerMenu()
    {
        var top = Application.Top;
        var customerMenu = new Window()
        {
            Title = "Customer Menu",
            X = 0,
            Y = 0,
            Width = Dim.Fill(),
            Height = Dim.Fill()
        };
        top.Add(customerMenu);
        customerMenu.FocusNext();
        var btnDisplayCustomer = new Button("Display Customer")
        {
            X =2,
            Y = 2
        };
        btnDisplayCustomer.Clicked += () =>
        {
            try
            {
                top.Remove(customerMenu);
            }
            catch (Exception ex)
            {
                MessageBox.ErrorQuery("Error", ex.Message, "OK");
            }
        };

        var btnAddCustomer = new Button("Add Customer")
        {
            X =2,
            Y = 4
        };
        btnAddCustomer.Clicked += () =>
        {
            try
            {
                top.Remove(customerMenu);
            }
            catch (Exception ex)
            {
                MessageBox.ErrorQuery("Error", ex.Message, "OK");
            }
        };

        var btnEditCustomer = new Button("Edit Customer:")
        {
            X =2,
            Y = 6
        };
        btnEditCustomer.Clicked += () =>
        {
            try
            {
                top.Remove(customerMenu);
                EditCustomer();
            }
            catch (Exception ex)
            {
                MessageBox.ErrorQuery("Error", ex.Message, "OK");
            }
        };

        var btnDeleteCustomer = new Button("Delete Customer:")
        {
            X =2,
            Y = 8
        };
        btnDeleteCustomer.Clicked += () =>
        {
            try
            {
                top.Remove(customerMenu);
                DeleteCustomer();
            }
            catch (Exception ex)
            {
                MessageBox.ErrorQuery("Error", ex.Message, "OK");
            }
        };

        var btnClose = new Button("Close")
        {
            X = Pos.Center(),
            Y = Pos.Percent(100) - 3
        };

        customerMenu.Add(btnDisplayCustomer, btnAddCustomer, btnEditCustomer, btnDeleteCustomer, btnClose);

    }
 
   static void EditCustomer()
{
    Customers customer = new Customers();
    List<Customers> customersList = LoadCustomers(connectionString);

    var top = Application.Top;
    var editCustomerWin = new Window("Edit Customer")
    {
        X = 0,
        Y = 0,
        Width = Dim.Fill() - 4,
        Height = Dim.Fill() - 4
    };
    top.Add(editCustomerWin);

    var findCustomerIDLabel = new Label("Find Customer ID:")
    {
        X = 1,
        Y = 1
    };
    var findCustomerIDField = new TextField("")
    {
        X = Pos.Right(findCustomerIDLabel) + 1,
        Y = 1,
        Width = Dim.Fill() - 4
    };

    var editCustomerNameLabel = new Label("Customer Name:")
    {
        X = 1,
        Y = 5,
        Visible = false
    };

    var editCustomerNameField = new TextField("")
    {
        X = Pos.Right(editCustomerNameLabel) + 1,
        Y = 5,
        Width = Dim.Fill() - 4,
        Visible = false
    };

    var editCustomerPhoneLabel = new Label("Customer Phone:")
    {
        X = 1,
        Y = 7,
        Visible = false
    };
    var editCustomerPhoneField = new TextField("")
    {
        X = Pos.Right(editCustomerPhoneLabel) + 1,
        Y = 7,
        Width = Dim.Fill() - 4,
        Visible = false
    };

    var editCustomerAddressLabel = new Label("Customer Address:")
    {
        X = 1,
        Y = 9,
        Visible = false
    };

    var editCustomerAddressField = new TextField("")
    {
        X = Pos.Right(editCustomerAddressLabel) + 1,
        Y = 9,
        Width = Dim.Fill() - 4,
        Visible = false
    };

    var editCustomerEmailLabel = new Label("Customer Email:")
    {
        X = 1,
        Y = 11,
        Visible = false
    };

    var editCustomerEmailField = new TextField("")
    {
        X = Pos.Right(editCustomerEmailLabel) + 1,
        Y = 11,
        Width = Dim.Fill() - 4,
        Visible = false
    };

    var editCustomerGenderLabel = new Label("Customer Gender:")
    {
        X = 1,
        Y = 13,
        Visible = false
    };
    var editCustomerGenderField = new TextField("")
    {
        X = Pos.Right(editCustomerGenderLabel) + 1,
        Y = 13,
        Width = Dim.Fill() - 4,
        Visible = false
    };

    var editCustomerDateOfBirthLabel = new Label("Customer Date of Birth:")
    {
        X = 1,
        Y = 15,
        Visible = false
    };
    var editCustomerDateOfBirthField = new TextField("")
    {
        X = Pos.Right(editCustomerDateOfBirthLabel) + 1,
        Y = 15,
        Width = Dim.Fill() - 4,
        Visible = false
    };
    

    var saveButton = new Button("Save")
    {
        X = Pos.Center(),
        Y = 17,
        Visible = false
    };
    saveButton.Clicked += () =>
    {
        try
        {
            customer.CustomerID = int.Parse(findCustomerIDField.Text.ToString());
            customer.CustomerName = editCustomerNameField.Text.ToString();
            customer.CustomerPhone = editCustomerPhoneField.Text.ToString();
            customer.CustomerAddress = editCustomerAddressField.Text.ToString();
            customer.CustomerEmail = editCustomerEmailField.Text.ToString();
            customer.CustomerGender = editCustomerGenderField.Text.ToString();
            customer.CustomerDateOfBirth = DateTime.Parse(editCustomerDateOfBirthField.Text.ToString());

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = "UPDATE customers SET customer_name = @CustomerName, customer_phone_number = @CustomerPhone, customer_address = @CustomerAddress, customer_email = @CustomerEmail, customer_gender = @CustomerGender, customer_dateofbirth = @CustomerDateOfBirth WHERE customer_id = @CustomerID";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@CustomerID", customer.CustomerID);
                command.Parameters.AddWithValue("@CustomerName", customer.CustomerName);
                command.Parameters.AddWithValue("@CustomerPhone", customer.CustomerPhone);
                command.Parameters.AddWithValue("@CustomerAddress", customer.CustomerAddress);
                command.Parameters.AddWithValue("@CustomerEmail", customer.CustomerEmail);
                command.Parameters.AddWithValue("@CustomerGender", customer.CustomerGender);
                command.Parameters.AddWithValue("@CustomerDateOfBirth", customer.CustomerDateOfBirth);

                command.ExecuteNonQuery();
            }
            MessageBox.Query("Success", "Customer information has been updated!", "OK");
            top.Remove(editCustomerWin);
            cus.CustomerMenu();
        }
        catch (Exception ex)
        {
            MessageBox.ErrorQuery("Error", ex.Message, "OK");
        }
    };

    var findButton = new Button("Find")
    {
        X = Pos.Center(),
        Y = 3
    };
    findButton.Clicked += () =>
    {
        try
        {
            int customerID = int.Parse(findCustomerIDField.Text.ToString());
            var foundCustomer = customersList.FirstOrDefault(c => c.CustomerID == customerID);

            if (foundCustomer != null)
            {
                // Populate fields with customer information
                editCustomerNameField.Text = foundCustomer.CustomerName;
                editCustomerPhoneField.Text = foundCustomer.CustomerPhone;
                editCustomerAddressField.Text = foundCustomer.CustomerAddress;
                editCustomerEmailField.Text = foundCustomer.CustomerEmail;
                editCustomerGenderField.Text = foundCustomer.CustomerGender;
                editCustomerDateOfBirthField.Text = foundCustomer.CustomerDateOfBirth.ToString();

                // Show edit fields and hide find controls
                findCustomerIDLabel.Visible = false;
                findCustomerIDField.Visible = false;
                findButton.Visible = false;

                editCustomerNameLabel.Visible = true;
                editCustomerNameField.Visible = true;
                editCustomerPhoneLabel.Visible = true;
                editCustomerPhoneField.Visible = true;
                editCustomerAddressLabel.Visible = true;
                editCustomerAddressField.Visible = true;
                editCustomerEmailLabel.Visible = true;
                editCustomerEmailField.Visible = true;
                editCustomerGenderLabel.Visible = true;
                editCustomerGenderField.Visible = true;
                editCustomerDateOfBirthLabel.Visible = true;
                editCustomerDateOfBirthField.Visible = true;

                saveButton.Visible = true;
            }
            else
            {
                MessageBox.ErrorQuery("Error", "Customer not found!", "OK");
            }
        }
        catch (Exception ex)
        {
            MessageBox.ErrorQuery("Error", ex.Message, "OK");
        }
    };

    var closeButton = new Button("Close")
    {
        X = Pos.Center(),
        Y = 19
    };
    closeButton.Clicked += () =>
    {
        top.Remove(editCustomerWin);
        cus.CustomerMenu();
    };

    editCustomerWin.Add(findCustomerIDLabel, findCustomerIDField, findButton,
                        editCustomerNameLabel, editCustomerNameField,
                        editCustomerPhoneLabel, editCustomerPhoneField,
                        editCustomerAddressLabel, editCustomerAddressField,
                        editCustomerEmailLabel, editCustomerEmailField,
                        editCustomerGenderLabel, editCustomerGenderField,
                        editCustomerDateOfBirthLabel, editCustomerDateOfBirthField,
                        saveButton, closeButton);
}
   static void DeleteCustomer()
    {
        ListCustomers = LoadCustomers(connectionString);
        var top = Application.Top;
        var deleteCustomerWin = new Window("Delete Customer")
        {
            X = 0,
            Y = 0,
            Width = Dim.Fill() - 4,
            Height = Dim.Fill() - 4
        };
        top.Add(deleteCustomerWin);

        var customerIDLabel = new Label("Customer ID:")
        {
            X = 1,
            Y = 1
        };

        var customerIDField = new TextField("")
        {
            X = Pos.Right(customerIDLabel) + 1,
            Y = 1,
            Width = Dim.Fill() - 4
        };

        var deleteButton = new Button("Delete")
        {
            X = Pos.Center(),
            Y = Pos.Bottom(customerIDField) + 1
        };

        deleteButton.Clicked += () =>
        {
            try
            {

                if(int.TryParse(customerIDField.Text.ToString(), out int customerID))
                {
                    Customers kh = ListCustomers.Find(k => k.CustomerID == customerID);

                    if (kh!= null)
                    {
                        using (MySqlConnection connection = new MySqlConnection(connectionString))
                        {
                            connection.Open();
                            string customerquery = "DELETE FROM customers WHERE customer_id = @customerid";
                            MySqlCommand customercommand = new MySqlCommand(customerquery, connection);
                            customercommand.Parameters.AddWithValue("@customerid", kh.CustomerID);
                            customercommand.ExecuteNonQuery();
                        }
                        ListCustomers.Remove(kh);
                        MessageBox.Query("Success", "Successfully deleted customer!", "OK");
                        top.Remove(deleteCustomerWin);
                        cus.CustomerMenu();
                    }
                    else
                    {
                        MessageBox.ErrorQuery("Error", "Customer not found!", "OK");
                    }
                }
                else
                {
                    MessageBox.ErrorQuery("Error", "Invalid customer ID!", "OK");
                }
            }
            catch (Exception ex)
            {
                MessageBox.ErrorQuery("Error", ex.Message, "OK");
            }

    };

    var closeButton = new Button("Close")
    {
        X = Pos.Center(),
        Y = Pos.Bottom(deleteButton) + 1
    };
    closeButton.Clicked += () =>
    {
        top.Remove(deleteCustomerWin);
        cus.CustomerMenu();
    };

    deleteCustomerWin.Add(customerIDLabel, customerIDField, deleteButton, closeButton);
    }
   static void DisplayCustomers()
{
    List<Customers> customersList = LoadCustomers(connectionString);
    var top = Application.Top;

    var displayCustomerWindow = new Window("Display Customers")
    {
        X = 0,
        Y = 0,
        Width = Dim.Fill(),
        Height = Dim.Fill()
    };
    top.Add(displayCustomerWindow);

    displayCustomerWindow.FocusNext();

    using (MySqlConnection connection = new MySqlConnection(connectionString))
    {
        string query = @"SELECT 
                            cus.customer_id, 
                            cus.customer_name, 
                            cus.customer_phone_number, 
                            cus.customer_address, 
                            cus.customer_email, 
                            cus.customer_gender, 
                            cus.customer_dateofbirth,
                            cus.customer_count,
                            cus.customer_totalspent
                        FROM customers cus";
        MySqlCommand command = new MySqlCommand(query, connection);
        connection.Open();
        MySqlDataReader reader = command.ExecuteReader();

        var columnDisplayListCustomer = new string[]
        {
            "Customer ID", "Name", "Phone Number", "Address", "Email", "Gender", "Date of Birth", "Order Count", "Total Spent"
        };

        // Add column headers
        for (int i = 0; i < columnDisplayListCustomer.Length; i++)
        {
            displayCustomerWindow.Add(new Label(columnDisplayListCustomer[i])
            {
                X = i * 20,
                Y = 0,
                Width = 20,
                Height = 1
            });
        }

        int rowOffset = 1;
        while (reader.Read())
        {
            int customerId = Convert.ToInt32(reader["customer_id"]);
            string customerName = reader["customer_name"].ToString();
            string customerPhone = reader["customer_phone_number"].ToString();
            string customerAddress = reader["customer_address"].ToString();
            string customerEmail = reader["customer_email"].ToString();
            string customerGender = reader["customer_gender"].ToString();
            DateTime customerDateOfBirth = Convert.ToDateTime(reader["customer_dateofbirth"]);
            int orderCount = Convert.ToInt32(reader["customer_count"]);
            decimal totalSpent = Convert.ToDecimal(reader["customer_totalspent"]);

            // Add customer information
            displayCustomerWindow.Add(new Label(customerId.ToString())
            {
                X = 0,
                Y = rowOffset,
                Width = 20,
                Height = 1
            });
            displayCustomerWindow.Add(new Label(customerName)
            {
                X = 1 * 20,
                Y = rowOffset,
                Width = 20,
                Height = 1
            });
            displayCustomerWindow.Add(new Label(customerPhone)
            {
                X = 2 * 20,
                Y = rowOffset,
                Width = 20,
                Height = 1
            });
            displayCustomerWindow.Add(new Label(customerAddress)
            {
                X = 3 * 20,
                Y = rowOffset,
                Width = 20,
                Height = 1
            });
            displayCustomerWindow.Add(new Label(customerEmail)
            {
                X = 4 * 20,
                Y = rowOffset,
                Width = 20,
                Height = 1
            });
            displayCustomerWindow.Add(new Label(customerGender)
            {
                X = 5 * 20,
                Y = rowOffset,
                Width = 20,
                Height = 1
            });
            displayCustomerWindow.Add(new Label(customerDateOfBirth.ToString("yyyy-MM-dd"))
            {
                X = 6 * 20,
                Y = rowOffset,
                Width = 20,
                Height = 1
            });
            displayCustomerWindow.Add(new Label(orderCount.ToString())
            {
                X = 7 * 20,
                Y = rowOffset,
                Width = 20,
                Height = 1
            });
            displayCustomerWindow.Add(new Label(totalSpent.ToString("0.00"))
            {
                X = 8 * 20,
                Y = rowOffset,
                Width = 20,
                Height = 1
            });

            rowOffset++;
        }

        reader.Close();
    }

    var btnClose = new Button("Close")
    {
        X = Pos.Center(),
        Y = Pos.Percent(100) - 1
    };
    btnClose.Clicked += () =>
    {
        top.Remove(displayCustomerWindow);
        cus.CustomerMenu();
    };

    displayCustomerWindow.Add(btnClose);
}
  static void AddCustomer()
{
    Customers cus = new Customers();
    var top = Application.Top;
    var addCustomerWin = new Window("Add Customer")
    {
        X = 0,
        Y = 0,
        Width = Dim.Fill(),
        Height = Dim.Fill()
    };
    top.Add(addCustomerWin);
    addCustomerWin.FocusNext();

    var customerNameLabel = new Label("Customer Name: ")
    {
        X = 2,
        Y = 2
    };

    var customerNameField = new TextField("")
    {
        X = Pos.Right(customerNameLabel) + 1,
        Y = 2,
        Width = Dim.Fill() - 4
    };

    var customerAddressLabel = new Label("Customer Address: ")
    {
        X = 2,
        Y = 4
    };

    var customerAddressField = new TextField("")
    {
        X = Pos.Right(customerAddressLabel) + 1,
        Y = 4,
        Width = Dim.Fill() - 4
    };

    var customerPhoneNumberLabel = new Label("Customer Phone: ")
    {
        X = 2,
        Y = 6
    };

    var customerPhoneNumberField = new TextField("")
    {
        X = Pos.Right(customerPhoneNumberLabel) + 1,
        Y = 6,
        Width = Dim.Fill() - 4
    };

    var customerEmailLabel = new Label("Customer Email: ")
    {
        X = 2,
        Y = 8
    };

    var customerEmailField = new TextField("")
    {
        X = Pos.Right(customerEmailLabel) + 1,
        Y = 8,
        Width = Dim.Fill() - 4
    };

    var customerGenderLabel = new Label("Customer Gender: ")
    {
        X = 2,
        Y = 10
    };

    var customerGenderField = new TextField("")
    {
        X = Pos.Right(customerGenderLabel) + 1,
        Y = 10,
        Width = Dim.Fill() - 4
    };

    var customerDateOfBirthLabel = new Label("Customer Date Of Birth: ")
    {
        X = 2,
        Y = 12
    };

    var customerDateOfBirthField = new TextField("")
    {
        X = Pos.Right(customerDateOfBirthLabel) + 1,
        Y = 12,
        Width = Dim.Fill() - 4
    };

    var saveButton = new Button("Save")
    {
        X = Pos.Center(),
        Y = 14
    };
    saveButton.Clicked += () =>
    {
        try
        {
            cus.CustomerName = customerNameField.Text.ToString();
            cus.CustomerAddress = customerAddressField.Text.ToString();
            cus.CustomerPhone = customerPhoneNumberField.Text.ToString();
            cus.CustomerEmail = customerEmailField.Text.ToString();
            cus.CustomerGender = customerGenderField.Text.ToString();
            DateTime customerDateOfBirth;
            if (!DateTime.TryParse(customerDateOfBirthField.Text.ToString(), out customerDateOfBirth))
            {
                MessageBox.ErrorQuery("Error", "Invalid date format. Please use YYYY-MM-DD.", "OK");
                return;
            }
            cus.CustomerDateOfBirth = customerDateOfBirth;

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = "INSERT INTO customers (customer_name, customer_address, customer_phone_number, customer_email, customer_gender, customer_dateofbirth) VALUES (@CustomerName, @CustomerAddress, @CustomerPhone, @CustomerEmail, @CustomerGender, @CustomerDateOfBirth)";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@CustomerName", cus.CustomerName);
                command.Parameters.AddWithValue("@CustomerAddress", cus.CustomerAddress);
                command.Parameters.AddWithValue("@CustomerPhone", cus.CustomerPhone);
                command.Parameters.AddWithValue("@CustomerEmail", cus.CustomerEmail);
                command.Parameters.AddWithValue("@CustomerGender", cus.CustomerGender);
                command.Parameters.AddWithValue("@CustomerDateOfBirth", cus.CustomerDateOfBirth);
                command.ExecuteNonQuery();
            }

            ListCustomers.Add(cus);
            MessageBox.Query("Success", "Successfully added new customer information!", "OK");
            top.Remove(addCustomerWin);
            cus.CustomerMenu();
        }
        catch (Exception ex)
        {
            MessageBox.ErrorQuery("Error", ex.Message, "OK");
        }
    };

    var closeButton = new Button("Close")
    {
        X = Pos.Center(),
        Y = Pos.Bottom(saveButton) + 1
    };
    closeButton.Clicked += () =>
    {
        top.Remove(addCustomerWin);
        cus.CustomerMenu();
    };

    addCustomerWin.Add(
        customerNameLabel, customerNameField,
        customerAddressLabel, customerAddressField,
        customerPhoneNumberLabel, customerPhoneNumberField,
        customerEmailLabel, customerEmailField,
        customerGenderLabel, customerGenderField,
        customerDateOfBirthLabel, customerDateOfBirthField,
        saveButton, closeButton
    );
}
   
 static void FindCustomer()
    {
        List<string[]> customers = new List<string[]>();
        var top = Application.Top;

        var findCustomerWindow = new Window("Find Customer")
        {
            X = 0,
            Y = 0,
            Width = Dim.Fill(),
            Height = Dim.Fill()
        };
        top.Add(findCustomerWindow);
        findCustomerWindow.FocusNext();

        var searchLabel = new Label("Enter customer name:")
        {
            X = 1,
            Y = 1
        };
        var searchField = new TextField("")
        {
            X = Pos.Right(searchLabel) + 1,
            Y = 1,
            Width = 40
        };
        var searchButton = new Button("Search")
        {
            X = Pos.Right(searchField) + 1,
            Y = 1
        };

        findCustomerWindow.Add(searchLabel, searchField, searchButton);

        var resultLabel = new Label("Results:")
        {
            X = 1,
            Y = 3
        };
        findCustomerWindow.Add(resultLabel);

        var columnDisplayListCustomer = new string[]
        {
            "Customer's name", "Phone number", "Address", "Email", "Gender", "Date of birth", "Order count", "Total spent"
        };

        int columnWidth = 20; // Increase column width

        for (int i = 0; i < columnDisplayListCustomer.Length; i++)
        {
            findCustomerWindow.Add(new Label(columnDisplayListCustomer[i])
            {
                X = i * columnWidth,
                Y = 4,
                Width = columnWidth,
                Height = 1
            });
        }

        searchButton.Clicked += () =>
        {
            customers.Clear();
            string searchTerm = searchField.Text.ToString();

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string query = @"SELECT 
                                    cus.customer_name, 
                                    cus.customer_phone_number, 
                                    cus.customer_address, 
                                    cus.customer_email, 
                                    cus.customer_gender, 
                                    cus.customer_dateofbirth,
                                    cus.customer_count,
                                    cus.customer_totalspent
                                FROM customers cus
                                WHERE cus.customer_name LIKE @SearchTerm";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@SearchTerm", "%" + searchTerm + "%");
                connection.Open();
                MySqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    customers.Add(new string[]{
                        reader["customer_name"].ToString(),
                        reader["customer_phone_number"].ToString(),
                        reader["customer_address"].ToString(),
                        reader["customer_email"].ToString(),
                        reader["customer_gender"].ToString(),
                        reader["customer_dateofbirth"].ToString(),
                        reader["customer_count"].ToString(),
                        reader["customer_totalspent"].ToString()
                    });
                }

                for (int i = 0; i < customers.Count; i++)
                {
                    for (int j = 0; j < customers[i].Length; j++)
                    {
                        findCustomerWindow.Add(new Label(customers[i][j])
                        {
                            X = j * columnWidth,
                            Y = i + 5,
                            Width = columnWidth,
                            Height = 1
                        });
                    }
                }
            }
        };

        var btnClose = new Button("Close")
        {
            X = Pos.Center(),
            Y = Pos.Percent(100) - 1
        };
        btnClose.Clicked += () =>
        {
            top.Remove(findCustomerWindow);
            cus.CustomerMenu();
        };

        findCustomerWindow.Add(btnClose);
    }
}

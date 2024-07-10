using System;
using System.Text;
using System.Data;
using System.Collections.Generic;   //Import namesapce to use function
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using Terminal.Gui;

public class Categories
{
    public int CategoryID { get; set; }
    public string CategoryName { get; set; } 
    public string CategoryDescription { get; set; }

    public static List<Categories> ListCategories = new List<Categories>();

    public static SuperAdmin superadmin = new SuperAdmin();
    public static string connectionString = Configuration.ConnectionString;

    // Method to load categories from the database
    static List<Categories> LoadCategory(string connectionString)
    {
        List<Categories> ListCategory = new List<Categories>();

        // Establish connection to the database
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {   
            string query = "SELECT * FROM categories"; 
            MySqlCommand command = new MySqlCommand(query, connection);
            connection.Open();
            MySqlDataReader read = command.ExecuteReader();
            while (read.Read())
            {
                Categories c = new Categories();
                // Load category properties from the database
                c.CategoryID = read.GetInt32("category_id");
                c.CategoryName = read.GetString("category_name");
                c.CategoryDescription = read.GetString("category_description");

                ListCategory.Add(c);
            }
        }
        return ListCategory;
    }
    
    public void AddCategory()
    {
        Categories cg = new Categories();
        var top = Application.Top;
        var addCategorytWin = new Window("Add Category")
        {
            X = 0,
            Y = 0,
            Width = Dim.Fill(),
            Height = Dim.Fill()
        };
        top.Add(addCategorytWin);
        addCategorytWin.FocusNext();

        // Labels and text fields for inputting cateogory details
        var CategoryNameLabel = new Label("Category Name:")
        {
            X = 2,
            Y = 2
        };
        var CategoryNameField = new TextField("")
        {
            X = 18,
            Y = 2,
            Width = 100
        };

        var CategoryDescriptionLabel = new Label("Description:")
        {
            X = 2,
            Y = 4
        };
        var CategoryDescriptionField = new TextField("")
        {
            X = 18,
            Y = 4,
            Width = 100
        };


        // Button to save the new category
        var saveButton = new Button("Save")
        {
            X = Pos.Center(),
            Y = 14
        };
        saveButton.Clicked += () =>
        {
            try
            {
                // Assign input values to the category object
                cg.CategoryName = CategoryNameField.Text.ToString();
                cg.CategoryDescription = CategoryDescriptionField.Text.ToString();


                // Insert the new category into the database
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "INSERT INTO categories (category_name, category_description) VALUES (@CategoryName, @CategoryDescription)";
                    MySqlCommand command = new MySqlCommand(query, connection);
                    command.Parameters.AddWithValue("@CategoryName", cg.CategoryName);
                    command.Parameters.AddWithValue("@CategoryDescription", cg.CategoryDescription);

                    command.ExecuteNonQuery();
                }

                // Add the new category to the static list
                ListCategories.Add(cg);

                // Show success message and return to superadmin menu
                MessageBox.Query("Success", "Successfully added new category!", "OK");
                top.Remove(addCategorytWin);
                superadmin.SuperAdminMenu();
            }
            catch (Exception ex)
            {
                // Display error message if an exception occurs
                MessageBox.ErrorQuery("Error", ex.Message, "OK");
            }
        };

        // Button to close the window
        var closeButton = new Button("Close")
        {
            X = Pos.Center(),
            Y = Pos.Bottom(saveButton) + 1
        };
        closeButton.Clicked += () =>
        {
            // Prompt confirmation before closing the window
            bool confirmed = MessageBox.Query("Confirm", "Are you sure you want to close?", "Yes", "No") == 0;
            if (confirmed)
            {
                top.Remove(addCategorytWin);
                superadmin.SuperAdminMenu(); // Return to superadmin menu
            }
        };

        // Add labels, text fields, and buttons to the window
        addCategorytWin.Add(CategoryNameLabel, CategoryNameField, CategoryDescriptionLabel, CategoryDescriptionField,
                        saveButton, closeButton);
    }
    public void DeleteCategory()
    {
        ListCategories = LoadCategory(connectionString);
        var top = Application.Top;
        var deletecategoryWin = new Window("Delete category")
        {
            X = 0,
            Y = 0,
            Width = Dim.Fill(),
            Height = Dim.Fill()
        };
        top.Add(deletecategoryWin);

        // Label and field for entering the category ID
        var categoryIDLabel = new Label("Category ID:")
        {
            X = 2,
            Y = 2
        };
        var categoryIDField = new TextField("")
        {
            X = Pos.Right(categoryIDLabel) + 1,
            Y = 2,
            Width = 100
        };

        // Button to delete the category
        var deleteButton = new Button("Delete")
        {
            X = Pos.Center(),
            Y = 4
        };
        deleteButton.Clicked += () =>
        {
            try
            {   
                
                // Validate and parse the category ID from the text field
                    if (int.TryParse(categoryIDField.Text.ToString(), out int categoryID))
                    {
                        
                        // Find the category in the list based on the ID
                        Categories cg = ListCategories.Find(s => s.CategoryID == categoryID);

                        if (cg != null)
                        {
                            // Delete the category from the database
                            using (MySqlConnection connection = new MySqlConnection(connectionString))
                            {
                                connection.Open();
                                string query = "DELETE FROM categories WHERE category_id = @categoryID";
                                MySqlCommand command = new MySqlCommand(query, connection);
                                command.Parameters.AddWithValue("@categoryID", categoryID);
                                command.ExecuteNonQuery();
                            }

                            // Remove the category from the application list
                            ListCategories.Remove(cg);

                            // Display success message and return to admin menu
                            MessageBox.Query("Success", "Successfully deleted category!", "OK");
                            top.Remove(deletecategoryWin);
                            superadmin.SuperAdminMenu();
                        }
                        else
                        {
                            // Display error if category not found
                            MessageBox.ErrorQuery("Error", "Category not found!", "OK");
                        }
                    }
                    else
                    {
                        // Display error for invalid category ID format
                        MessageBox.ErrorQuery("Error", "Invalid Category ID!", "OK");
                    }
                
            }
            catch (Exception ex)
            {
                // Display error for any exception during deletion process
                MessageBox.ErrorQuery("Error", ex.Message, "OK");
            }
        };

        // Button to close the window
        var closeButton = new Button("Close")
        {
            X = Pos.Center(),
            Y = Pos.Bottom(deleteButton) + 1
        };
        closeButton.Clicked += () =>
        {
            // Prompt confirmation before closing the window
            bool confirmed = MessageBox.Query("Confirm", "Are you sure you want to close?", "Yes", "No") == 0;
            if (confirmed)
            {
                top.Remove(deletecategoryWin);
                superadmin.SuperAdminMenu(); // Return to admin menu
            }
        };

        // Add all components to the window
        deletecategoryWin.Add(categoryIDLabel, categoryIDField, deleteButton, closeButton);
    }
    public void Displaycategorys()
    {
        // Load the list of categorys from the database using the connection string.
        List<Categories> categorysList = LoadCategory(connectionString);
        
        // Get the top-level window of the application.
        var top = Application.Top;

        // Create a new window titled "Display categorys" with specific dimensions.
        var displaycategoryWindow = new Window("Display categories")
        {
            X = 0,
            Y = 0,
            Width = Dim.Fill(),
            Height = Dim.Fill()
        };

        // Add the display category window to the top-level window.
        top.Add(displaycategoryWindow);

        // Move focus to the next UI element in the window.
        displaycategoryWindow.FocusNext();

        // Establish a connection to the database.
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            // Define the SQL query to retrieve category data.
            string query = @"SELECT 
                                cate.category_id, 
                                cate.category_name, 
                                cate.category_description
                            FROM categories cate";

            // Create a command object to execute the query.
            MySqlCommand command = new MySqlCommand(query, connection);
            
            // Open the connection to the database.
            connection.Open();
            
            // Execute the query and obtain a data reader to read the results.
            MySqlDataReader reader = command.ExecuteReader();

            // Define the column headers to be displayed in the window.
            var columnDisplayListcategory = new string[]
            {
                "category ID", "Name", "Description"
            };

            // Add column headers to the window.
            for (int i = 0; i < columnDisplayListcategory.Length; i++)
            {
                displaycategoryWindow.Add(new Label(columnDisplayListcategory[i])
                {
                    X = i * 20,
                    Y = 0,
                    Width = 20,
                    Height = 1
                });
            }

            // Initialize the row offset for displaying data rows.
            int rowOffset = 1;

            // Read the data row by row from the data reader.
            while (reader.Read())
            {
                // Retrieve category details from the current row.
                int categoryId = Convert.ToInt32(reader["category_id"]);
                string categoryName = reader["category_name"].ToString();
                string categorydescription = reader["category_decription"].ToString();

                // Add category details to the window as labels.
                displaycategoryWindow.Add(new Label(categoryId.ToString())
                {
                    X = 0,
                    Y = rowOffset,
                    Width = 20,
                    Height = 1
                });
                displaycategoryWindow.Add(new Label(categoryName)
                {
                    X = 1 * 20,
                    Y = rowOffset,
                    Width = 20,
                    Height = 1
                });
                displaycategoryWindow.Add(new Label(categorydescription)
                {
                    X = 2 * 20,
                    Y = rowOffset,
                    Width = 20,
                    Height = 1
                });

                // Increment the row offset for the next data row.
                rowOffset++;
            }

            // Close the data reader.
            reader.Close();
        }

        // Create a button labeled "Close" to close the display category window.
        var btnClose = new Button("Close")
        {
            X = Pos.Center(),
            Y = Pos.Percent(100) - 1
        };

        // Define the action to be taken when the close button is clicked.
        btnClose.Clicked += () =>
        {
            // Remove the display category window and go back to the admin menu.
            top.Remove(displaycategoryWindow);
            superadmin.SuperAdminMenu();
        };

        // Add the close button to the display category window.
        displaycategoryWindow.Add(btnClose);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class Configuration
{
    private static string connectionString;

    public static string ConnectionString
    {
        get { return connectionString; }
        set { connectionString = value; }
    } 
    
}

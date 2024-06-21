using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


public class SessionData
{
    private static SessionData instance;
    private int currentCustomerID;

    private SessionData()
    {
        // Khởi tạo các giá trị mặc định khi cần thiết
    }

    public static SessionData Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new SessionData();
            }
            return instance;
        }
    }

    public int CurrentCustomerID
    {
        get { return currentCustomerID; }
        set { currentCustomerID = value; }
    }
}

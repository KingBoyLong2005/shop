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
        // Khởi tạo giá trị mặc định
        currentCustomerID = -1; // Giá trị mặc định để chỉ ra rằng không có customer ID nào được thiết lập
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
        set 
        { 
            currentCustomerID = value; 
            NotifyCustomerIDChange(); 
        }
    }

    public void ResetCustomerID()
    {
        CurrentCustomerID = -1;
    }

    // Định nghĩa sự kiện
    public event Action<int> CustomerIDChanged;

    private void NotifyCustomerIDChange()
    {
        CustomerIDChanged?.Invoke(currentCustomerID);
    }
}


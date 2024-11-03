using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gyminize.Models
{
    public class Customer
    {
        public int customer_id
        {
            get; set;
        }
        public string customer_name
        {
            get; set;
        }
        public int auth_type
        {
            get; set;
        }
        public string username
        {
            get; set;
        }
        public string customer_password
        {
            get; set;
        }

        // Tạo một instance duy nhất của Customer
        

 
        public Customer()
        {
            this.customer_name = "";
            this.auth_type = 0;
            this.username = "";
            this.customer_password = "";
        }

        public Customer(string customername,int auth_type, string username, string password)
        {

            this.customer_name = customername;
            this.auth_type = auth_type;
            this.username = username;
            this.customer_password = password;
        }
        // Phương thức static để lấy instance duy nhất của Customer
        
    }

}

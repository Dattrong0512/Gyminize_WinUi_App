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
        public int role_user
        {
            get; set;
        }

        public Customer()
        {
            this.customer_name = "";
            this.auth_type = 1;
            this.username = "";
            this.customer_password = "";
            this.role_user = 1;
        }

        public Customer(string customername,int auth_type, string username, string password, int role_user)
        {

            this.customer_name = customername;
            this.auth_type = auth_type;
            this.username = username;
            this.customer_password = password;
            this.role_user = role_user;
        }
        // Phương thức static để lấy instance duy nhất của Customer
        
    }

}

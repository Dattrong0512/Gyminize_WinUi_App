using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gyminize.Models
{
    class Customer
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
        public Customer( string customer_name, int auth_type, string username, string customer_password)
        {
            this.customer_name = customer_name;
            this.auth_type = auth_type;
            this.username = username;
            this.customer_password = customer_password;
        }
        public Customer()
        {
            this.customer_name = "";
            this.auth_type = 0;
            this.username = "";
            this.customer_password = "";
        }
    }
}

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Gyminize_API.Data.Model;
namespace Gyminize_API.Data.Models
{

    [Table("customer")]
    public class Customer
    {
        [Key]
        public int customer_id
        {
            get; set;
        }
        public string? customer_name
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
        public string email
        {
            get; set;
        }
        public virtual Customer_health? Customer_health
        {
            get; set;
        }

        public virtual ICollection<Dailydiary>? Dailydiaries
        {
            get; set;
        }
        public virtual ICollection<Plandetail>? Plandetails
        {
            get; set;
        }
        public virtual ICollection<Orders>? Orders
        {
            get; set;
        }
    }

}

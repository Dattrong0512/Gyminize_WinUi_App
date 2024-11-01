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
        public required string? customer_name
        {
            get; set;
        }
        public required int auth_type
        {
            get; set;
        }
        public required string username
        {
            get; set;
        }
        public required string customer_password
        {
            get; set;
        }
        public virtual Customer_health Customer_health
        {
            get; set;
        }
    }

}

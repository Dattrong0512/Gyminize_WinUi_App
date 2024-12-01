using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Gyminize_API.Data.Model;
using Gyminize_API.Data.Models;
namespace Gyminize_API.Data.Model
{
    [Table("orders")]
    public class Orders
    {
        [Key]
        public int orders_id
        {
            get; set;
        }
        [ForeignKey("Customer")]
        public int customer_id
        {
            get; set;
        }
        public DateTime order_date
        {
            get; set;
        }
        public decimal total_price
        {
            get; set;
        }
        public ICollection<Orderdetail>? Orderdetail
        {
            get; set;
        }
        [JsonIgnore]
        public Customer? Customer
        {
            get; set;
        }
        [JsonIgnore]
        public Payment? Payment
        {
            get; set;
        }
    }
}



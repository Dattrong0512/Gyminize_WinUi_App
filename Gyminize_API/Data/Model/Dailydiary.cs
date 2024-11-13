using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Gyminize_API.Data.Models;
namespace Gyminize_API.Data.Model
{
    [Table("dailydiary")]

    public class Dailydiary
    {
        [Key]
        public int dailydiary_id
        {
            get; set;
        }
        [ForeignKey("Customer")]
        public int customer_id
        {
            get; set;
        }
        public DateTime diary_date
        {
            get; set;
        }
        public int calories_remain
        {
            get; set;
        }
        public int daily_weight
        {
            get; set;
        }
        public decimal total_calories
        {
            get; set;
        }   
        public string notes
        {
            get; set;
        }
        public Customer? Customer
        {
            get; set;
        }
        public ICollection<Fooddetail>? Fooddetails
        {
            get; set;
        }
    }

}


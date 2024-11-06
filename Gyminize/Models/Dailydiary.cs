using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gyminize.Models
{
    public class Dailydiary
    {
        public int dailydiary_id
        {
            get; set;
        }
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
        public Customer Customer
        {
            get; set;
        }

        // Danh sách FoodDetail
        public ICollection<FoodDetail> Fooddetails
        {
            get; set;
        }
    }
}

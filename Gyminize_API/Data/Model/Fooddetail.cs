﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Gyminize_API.Data.Model
{
    [Table("fooddetail")]
    public class Fooddetail
    {
        [Key]
        public int fooddetail_id
        {
            get; set;
        }
        [ForeignKey("Dailydiary")]
        public int dailydiary_id
        {
            get; set;
        }
        [ForeignKey("Food")]
        public int food_id
        {
            get; set;
        }
        public int meal_type
        {
            get; set;
        }
        public int food_amount
        {
            get; set;
        }
        [JsonIgnore]
        public Dailydiary? Dailydiary
        {
            get; set;
        }
        public Food? Food
        {
            get; set;
        }
    }
}



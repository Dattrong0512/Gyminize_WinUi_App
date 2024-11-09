using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Gyminize.Models
{
    public class FoodDetail
    {
        public int fooddetail_id
        {
            get; set;
        }
        public int dailydiary_id
        {
            get; set;
        }
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
        public Food? Food
        {
            get; set;
        }
        public int TotalCalories => Food != null ? Food.calories * food_amount : 0;
    }
}

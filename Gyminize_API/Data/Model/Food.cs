using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gyminize_API.Data.Model
{
    [Table("food")]
    public class Food
    {
        [Key]
        public int food_id
        {
            get; set;
        }
        public string food_name
        {
            get; set;
        }
        public decimal calories
        {
            get; set;
        }
        public double protein
        {
            get; set;
        }
        public double carbs
        {
            get; set;
        }
        public double fats
        {
            get; set;
        }
        public ICollection<Fooddetail> Fooddetails
        {
            get; set;
        }
    }
}



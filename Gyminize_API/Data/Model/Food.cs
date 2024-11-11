using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

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
        public int calories
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
        public string serving_unit
        {
            get; set;
        }
        [JsonIgnore]
        public ICollection<Fooddetail>? Fooddetails
        {
            get; set;
        }

    }
}



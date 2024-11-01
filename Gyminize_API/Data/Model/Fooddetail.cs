using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        public int food_amount
        {
            get; set;
        }
        public Dailydiary Dailydiary
        {
            get; set;
        }
        public Food Food
        {
            get; set;
        }
    }
}



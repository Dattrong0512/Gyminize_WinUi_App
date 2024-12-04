using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
namespace Gyminize_API.Data.Model
{
    [Table("orderdetail")]
    public class Orderdetail
    {
        [Key]
        public int orderdetail_id
        {
            get; set;
        }
        [ForeignKey("Orders")]
        public int orders_id
        {
            get; set;
        }
        [ForeignKey("Product")]
        public int product_id
        {
            get; set;
        }
        public int product_amount
        {
            get; set;
        }
        [JsonIgnore]
        public Orders? Orders
        {
            get; set;
        }
        public Product? Product
        {
            get; set;
        }


    }

}


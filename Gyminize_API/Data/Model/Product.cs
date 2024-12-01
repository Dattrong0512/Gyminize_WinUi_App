using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
namespace Gyminize_API.Data.Model
{
    [Table("product")]
    public class Product
    {
        [Key]
        public int product_id
        {
            get; set;
        }
        [ForeignKey("Category")]
        public int category_id
        {
            get; set;
        }
        public string product_name
        {
            get; set;
        }
        public decimal product_price
        {
            get; set;
        }
        public string product_provider
        {
            get; set;
        }
        public string description
        {
            get; set;
        }
        
        public Category? Category
        {
            get; set;
        }
        [JsonIgnore]
        public ICollection<Orderdetail>? Orderdetail
        {
            get; set;
        }
    }

}


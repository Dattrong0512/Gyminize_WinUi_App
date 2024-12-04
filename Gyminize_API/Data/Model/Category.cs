using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
namespace Gyminize_API.Data.Model
{
    [Table("category")]
    public class Category
    {
        [Key]
        public int category_id
        {
            get; set;
        }
        public string category_name
        {
            get; set;
        }
        public string description
        {
            get; set;
        }

        public ICollection<Product>? Product
        {
            get; set;
        }
    }

}


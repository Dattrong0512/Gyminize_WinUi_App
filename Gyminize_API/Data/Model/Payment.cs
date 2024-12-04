using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
namespace Gyminize_API.Data.Model
{
    [Table("payment")]
    public class Payment
    {
        [Key]
        public int payment_id
        {
            get; set;
        }
        [ForeignKey("Orders")]
        public int orders_id
        {
            get; set;
        }
        public DateTime payment_date
        {
            get; set;
        }
        public decimal payment_amount
        {
            get; set;
        }
        public string transaction_id
        {
            get; set;
        }
        public string payment_method
        {
            get; set;
        }
        public string payment_status
        {
            get; set;
        }

        public virtual Orders? Orders
        {
            get; set;
        }
    }

}


using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Gyminize_API.Data.Model;
using Microsoft.EntityFrameworkCore;
namespace Gyminize_API.Data.Model
{
    [Table("plandetail")]
    public class Plandetail
    {
        [Key]
        public int plandetail_id
        {
            get; set;
        }
        [ForeignKey("Plan")]
        public int plan_id
        {
            get; set;
        }
        [ForeignKey("Customer")]
        public int customer_id
        {
            get; set;
        }
        public DateTime created_at
        {
            get; set;
        }
        public DateTime start_date
        {
            get; set;
        }
        public DateTime end_date
        {
            get; set;
        }
    }
}











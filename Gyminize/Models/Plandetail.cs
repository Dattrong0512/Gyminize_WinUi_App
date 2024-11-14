using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gyminize.Models
{
    public class Plandetail
    {
        public int plandetail_id
        {
            get; set;
        }
        public int plan_id
        {
            get; set;
        }
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
        public Plan? Plan
        {
            get; set;
        }
        public ICollection<Workoutdetail>? Workoutdetails
        {
            get; set;
        }
    }
}
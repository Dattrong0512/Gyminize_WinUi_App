using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gyminize.Models
{
    public class Plan
    {
        public int plan_id
        {
            get; set;
        }
        public string plan_name
        {
            get; set;
        }
        public string description
        {
            get; set;
        }
        public int duration_week
        {
            get; set;
        }
        public ICollection<Workoutdetail>? Workoutdetails
        {
            get; set;
        }

    }
}


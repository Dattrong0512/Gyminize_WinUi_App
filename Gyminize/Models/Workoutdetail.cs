using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gyminize.Models
{
    public class Workoutdetail
    {
        public int workoutdetail_id
        {
            get; set;
        }
        public int typeworkout_id
        {
            get; set;
        }
        public int plan_id
        {
            get; set;
        }
        public DateTime date_workout
        {
            get; set;
        }
        public string description
        {
            get; set;
        }
        public Typeworkout? Typeworkout
        {
            get; set;
        }
    }
}

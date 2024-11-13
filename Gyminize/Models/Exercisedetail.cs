using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gyminize.Models
{
    public class Exercisedetail
    {
        public int exercisedetail_id
        {
            get; set;
        }
        public int typeworkout_id
        {
            get; set;
        }
        public int exercise_id
        {
            get; set;
        }
        public int workout_sets
        {
            get; set;
        }
        public Exercise? Exercise
        {
            get; set;
        }

    }
}

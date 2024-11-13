using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gyminize.Models
{

    public class Exercise
    {
        public int exercise_id
        {
            get; set;
        }
        public string exercise_name
        {
            get; set;
        }
        public string description
        {
            get; set;
        }
        public string linkvideo
        {
            get; set;
        }
        public int reps
        {
            get; set;
        }
      

    }
}


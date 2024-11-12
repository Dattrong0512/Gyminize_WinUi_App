using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Gyminize_API.Data.Model;
using Microsoft.EntityFrameworkCore;
namespace Gyminize_API.Data.Model
{
    [Table("exercise")]
    public class Exercise
    {
        [Key]
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











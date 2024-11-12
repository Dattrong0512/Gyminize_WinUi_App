using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Gyminize_API.Data.Model;
using Microsoft.EntityFrameworkCore;
namespace Gyminize_API.Data.Model
{
    [Table("exercisedetail")]
    public class Exercisedetail
    {
        [Key]
        public int excercisedetail_id
        {
            get; set;
        }
        [ForeignKey("Typeworkout")]
        public int typeworkout_id
        {
            get; set;
        }
        [ForeignKey("Exercise")]
        public int exercise_id
        {
            get; set;
        }
        public int workout_sets
        {
            get; set;
        }


    }
}











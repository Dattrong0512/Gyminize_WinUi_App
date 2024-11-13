using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Gyminize_API.Data.Model;
using Microsoft.EntityFrameworkCore;
namespace Gyminize_API.Data.Model
{
    [Table("typeworkout")]
    public class Typeworkout
    {
        [Key]
        public int typeworkout_id
        {
            get; set;
        }
        public string workoutday_type
        {
            get; set;
        }
        public string description
        {
            get; set;
        }
      
        public ICollection<Exercisedetail>? Exercisedetails
        {
            get; set;
        }
        [JsonIgnore]
        public ICollection<Workoutdetail>? Workoutdetails
        {
            get; set;
        }

    }
}











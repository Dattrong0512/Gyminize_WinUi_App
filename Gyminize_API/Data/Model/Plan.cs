using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Gyminize_API.Data.Model;
using Microsoft.EntityFrameworkCore;
namespace Gyminize_API.Data.Model
{
    [Table("plan")]
    public class Plan
    {
        [Key]
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
        [JsonIgnore]
        public ICollection<Plandetail> ? Plandetail
        {
            get; set;
        }
    }
}











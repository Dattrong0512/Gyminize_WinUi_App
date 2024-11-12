using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Gyminize_API.Data.Model;
using Microsoft.EntityFrameworkCore;
namespace Gyminize_API.Data.Model
{
    [Table("plan")]
    public class Plan
    {
        [Key]
        int plan_id
        {
            get; set;
        }
        string plan_name
        {
            get; set;
        }
        string description
        {
            get; set;
        }
        int duration
        {
            get; set;
        }
    }
}











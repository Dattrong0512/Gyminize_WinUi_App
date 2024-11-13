﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Gyminize_API.Data.Model;
using Microsoft.EntityFrameworkCore;
namespace Gyminize_API.Data.Model
{
    [Table("workoutdetail")]
    public class Workoutdetail
    {

        [Key]
        public int workoutdetail_id
        {
            get; set;
        }
        [ForeignKey("Typeworkout")]
        public int typeworkout_id
        {
            get; set;
        }
        [ForeignKey("Plan")]
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
        public Typeworkout ? Typeworkout
        {
            get; set;
        }
        [JsonIgnore]
        public Plan? Plan
        {
            get; set;
        }

    }
}











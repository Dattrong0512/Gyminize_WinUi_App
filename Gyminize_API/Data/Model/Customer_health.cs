
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Gyminize_API.Data.Models;
using Microsoft.Identity.Client;
namespace Gyminize_API.Data.Model;

[Table("customer_health")]
public class Customer_health
{
    [Key,ForeignKey("Customer")]
    public int customer_id
    {
        get; set;
    }
    public int gender
    {
        get; set;
    }
    public int height
    {
        get;
        set;
    }
    public int weight
    {
        get;
        set;
    }
    public int age
    {
        get;
        set;
    }
    public int activity_level
    {
        get;
        set;
    }
    public decimal body_fat
    {
        get; set;
    }
    public decimal tdee
    {
        get; set;
    }

    public required Customer Customer
    {
        get; set;
    }
}


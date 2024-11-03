using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Gyminize.Models;
public class CustomerHealth
{
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
    public CustomerHealth()
    {
        customer_id = 0;
        gender = 0;
        height = 0;
        weight = 0;
        age = 0;
        activity_level = 0;
        body_fat = 0;
        tdee = 0;
    }

}

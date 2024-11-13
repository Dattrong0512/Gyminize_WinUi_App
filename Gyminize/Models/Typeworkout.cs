using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gyminize.Models;
public class Typeworkout
{
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

}

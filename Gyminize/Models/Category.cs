using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gyminize.Models;
public class Category
{
    public int category_id
    {
        get; set;
    }
    public string category_name
    {
        get; set;
    }
    public string description
    {
        get; set;
    }
}

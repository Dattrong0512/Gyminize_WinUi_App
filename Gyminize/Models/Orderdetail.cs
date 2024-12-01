using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gyminize.Models;
public class Orderdetail
{
    public int orderdetail_id
    {
        get; set;
    }
    public int orders_id
    {
        get; set;
    }
    public int product_id
    {
        get; set;
    }
    public int product_amount
    {
        get; set;
    }
    public Product? Product
    {
        get; set;
    }

}

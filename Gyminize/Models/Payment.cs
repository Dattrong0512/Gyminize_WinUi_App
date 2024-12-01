using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gyminize.Models;
public class Payment
{
    public int payment_id
    {
        get; set;
    }
    public int orders_id
    {
        get; set;
    }
    public DateTime payment_date
    {
        get; set;
    }
    public decimal payment_amount
    {
        get; set;
    }
    public string transaction_id
    {
        get; set;
    }
    public string payment_method
    {
        get; set;
    }
    public string payment_status
    {
        get; set;
    }
}

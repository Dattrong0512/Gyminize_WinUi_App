﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gyminize.Models;
public class Orders
{
    public int orders_id
    {
        get; set;
    }
    public int customer_id
    {
        get; set;
    }
    public DateTime order_date
    {
        get; set;
    }
    public decimal total_price
    {
        get; set;
    }
    public string address
    {
        get; set;
    }
    public string phone_number
    {
        get; set;
    }
    public string status
    {
        get; set;
    }
    public ICollection<Orderdetail>? Orderdetail
    {
        get; set;
    }
}

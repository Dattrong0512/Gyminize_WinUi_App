﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gyminize.Models;
public class CustomerInfo
{
    public string? username { get; set; }
    public int sex { get; set; }
    public int Age { get; set; }
    public int Weight { get; set; }
    public int Height { get; set; }
    public int ActivityLevel { get; set; }
    public double Tdee
    {
        get; set;
    }
    public double BodyFat { get; set; }
}

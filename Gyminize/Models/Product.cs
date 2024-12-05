using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gyminize.Models;
public class Product
{
    public int product_id
    {
        get; set;
    }
    public int category_id
    {
        get; set;
    }
    public string product_name
    {
        get; set;
    }
    public decimal product_price
    {
        get; set;
    }
    public string product_provider
    {
        get; set;
    }
    public string description
    {
        get; set;
    }

    [NotMapped]
    public string product_source => $"ms-appx:///Assets/Product_Img/{product_id}.png";

    public Category? Category
    {
        get; set;
    }
}
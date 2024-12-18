using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gyminize.Core.Models;
public class Influencer
{
    public int influencer_id
    {
        get; set;
    }
    public string influencer_name
    {
        get; set;
    }

    // Lượng calo.
    public string influencer_background
    {
        get; set;
    }

    public string influencer_platform
    {
        get; set;
    }

    public string influencer_country
    {
        get; set;
    }

    public string influencer_link
    {
        get; set;
    }
    public string influencer_avatar => $"ms-appx:///Assets/Influencer_Avatar/{influencer_id}.jpg";
    public string influencer_img => $"ms-appx:///Assets/Influencer_Img/{influencer_id}.jpg";
}


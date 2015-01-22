using System;

namespace SGI.Core.SteamApi
{
    //"price_overview": {
    //            "currency": "RUB",
    //            "initial": 24900,
    //            "final": 24900,
    //            "discount_percent": 0
    //        }
    public class PriceOverview
    {
        public string Currency { get; set; }
        public string Initial { get; set; }
        public string Final { get; set; }
        public string Discount_Percent { get; set; }

        public Exception Exception { get; set; }
    }
}
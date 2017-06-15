using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVCWebUIComponent.Models
{
    [Serializable]
    public class Booking
    {

        public string SeatCode { get; set; }
        public string Building { get; set; }
        public int Floor { get; set; }
        public string Coordinate { get; set; }
        public string Begin { get; set; }
        public string End { get; set; }
        public string Employee { get; set; }


    }
}
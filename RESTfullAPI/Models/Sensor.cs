using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RESTfullAPI.Models
{
    public class Sensor
    {
        public int  Id { get; set; }
        public bool IsPersonal { get; set; }
        public int UserID { get; set; }
        public int Floor { get; set; }
    }
}
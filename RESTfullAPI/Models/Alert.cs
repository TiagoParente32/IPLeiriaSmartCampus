using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RESTfullAPI.Models
{
    public class Alert
    {
        public string Type { get; set; }
        public int Value { get; set; }
        public string Operator { get; set; }
        public int Between_value { get; set; }
        public string Message { get; set; }
        public Boolean Enabled { get; set; }
        public int SensorID { get; set; }
        public DateTime Timestamp { get; set; }
        //for new data just add new prop

    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RoosterApp.Models
{
    public class StatusImg
    {
        public int ID { get; set; }
        public DateTime Timestamp { get; set; }
        public bool Running { get; set; }

        public string Url { get; set; }
    }

    
}
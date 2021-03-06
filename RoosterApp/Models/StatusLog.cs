﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RoosterApp.Models
{
    public class StatusLog
    {
        public int TaskId { get; set; }
        public bool Completed { get; set; }
        public int Klas { get; set; }
        public int Week { get; set; }
        public string Exception { get; set; }
        public Log.DataAction Action { get; set; }
        public int Duration { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
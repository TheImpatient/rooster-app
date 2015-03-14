using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RoosterApp.Models
{
    public class Log
    {
        public enum DataAction
        {
            SyncDone,
            NoSync,
            Exception
        }
    }
}
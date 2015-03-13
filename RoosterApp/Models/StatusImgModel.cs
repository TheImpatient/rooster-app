using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RoosterApp.Models
{
    public class StatusImgModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
    }

    public class Repository
    {
        public static List<StatusImgModel> GetData()
        {
            List<StatusImgModel> list = new List<StatusImgModel>
            {
                new StatusImgModel
                {
                    ID = 1,
                    Name = "default",
                    ImageUrl = "../../Images/heartbeat2.png"
                },
                new StatusImgModel
                {
                    ID = 2,
                    Name = "Online",
                    ImageUrl = "../../Images/heartbeat_green2.png"
                },
                new StatusImgModel
                {
                    ID = 3,
                    Name = "Offline",
                    ImageUrl = "../../Images/heartbeat_red2.png"
                }
            };

            return list;
        }
    }
}
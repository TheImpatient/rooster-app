using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using RoosterApp.Models;
using System.Web.Helpers;

namespace RoosterApp.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Status()
        {
            StatusImg img = Repository.GetStatusImgData();
            List<StatusLog> logs = Repository.GetStatusLogData();

            ViewBag.RoostersPerUur = logs.Count(x => x.Timestamp > DateTime.Now.AddHours(-1));
            ViewBag.ErrorGauge = logs.Count(x => x.Timestamp > DateTime.Now.AddDays(-1) && x.Completed == false);
            ViewBag.CrawlTijdGauge = (int)logs.Average(x => x.Duration);
            ViewBag.StatusImg = img;
            ViewBag.StatusLog = logs;

            return View();
        }

        public PartialViewResult StatusImgPartialView()
        {
            StatusImg img = Repository.GetStatusImgData();

            if (img != null)
            {
                if (img.Timestamp.CompareTo(DateTime.Now.AddSeconds(-10)) == 1)
                {
                    img.Url = "../../images/heartbeat_green2.png";
                }
                else
                {
                    img.Url = "../../images/heartbeat_red2.png";
                }
            }
            else
            {
                img.Url = "../../images/heartbeat2.png";
            }

            return PartialView("_StatusImgPartial", img);
        }

        public PartialViewResult StatusHistoryPartialView()
        {
            List<StatusLog> list = Repository.GetStatusLogData();

            return PartialView("_StatusHistoryPartial", list.Skip((list.Count-10)).ToList());
        }

        public PartialViewResult StatusErrorsPartialView()
        {
            List<StatusLog> list = Repository.GetStatusLogData();

            return PartialView("_StatusErrorsPartial", list.Where(x => x.Completed == false).ToList());
        }

        public int ErrorGaugeResult()
        {
            List<StatusLog> list = Repository.GetStatusLogData();

            return list.Count(x => x.Timestamp > DateTime.Now.AddDays(-1) && x.Completed == false);
        }

        public int RoosterGaugeResult()
        {
            List<StatusLog> list = Repository.GetStatusLogData();

            return list.Count(x => x.Timestamp > DateTime.Now.AddHours(-1));
        }

        public int CrawlTijdGaugeResult()
        {
            List<StatusLog> list = Repository.GetStatusLogData();
            return (int) list.Average(x=>x.Duration);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using RoosterApp.Models;

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
            IEnumerable<StatusImg> imgs = Repository.GetStatusImgData();
            List<StatusLog> logs = Repository.GetStatusLogData();

            ViewBag.RoostersPerUur = logs.Count(x => x.Timestamp > DateTime.Now.AddHours(-1));
            ViewBag.ErrorGauge = logs.Count(x => x.Timestamp > DateTime.Now.AddDays(-1) && x.Completed == false);
            ViewBag.StatusImg = imgs;
            ViewBag.StatusLog = logs;

            return View();
        }

        public PartialViewResult StatusImgPartialView()
        {
            IEnumerable<StatusImg> list = Repository.GetStatusImgData();

            int min = list.Min(m => m.ID);
            int max = list.Max(m => m.ID);

            int randomId = new Random().Next(min, (max + 1));

            StatusImg model = list.FirstOrDefault(m => m.ID == randomId);
            return PartialView("_StatusImgPartial", model);
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

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
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
            IEnumerable<StatusImg> list = Repository.GetStatusImgData();
            return View(list);
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

            return PartialView("_StatusHistoryPartial", list);
        }

        public PartialViewResult StatusErrorsPartialView()
        {
            
            return PartialView("_StatusErrorsPartial");
        }

    }
}

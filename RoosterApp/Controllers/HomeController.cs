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
            IEnumerable<StatusImgModel> list = Repository.GetData();
            return View(list);
        }

        public PartialViewResult StatusImgPartialView()
        {
            IEnumerable<StatusImgModel> list = Repository.GetData();

            int min = list.Min(m => m.ID);
            int max = list.Max(m => m.ID);

            int randomId = new Random().Next(min, (max + 1));

            StatusImgModel model = list.FirstOrDefault(m => m.ID == randomId);
            return PartialView("_StatusImgPartial", model);
        }

    }
}

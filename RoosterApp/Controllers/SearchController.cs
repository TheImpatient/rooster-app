using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AgendaLibrary;
using PagedList;
using RoosterApp.Models;
using Les = RoosterApp.Models.Les;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;

namespace RoosterApp.Controllers
{
    public class SearchController : Controller
    {
        //
        // GET: /Search/

        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.VakSortParm = String.IsNullOrEmpty(sortOrder) ? "vak_desc" : "";
            ViewBag.KlasSortParm = sortOrder == "Klas" ? "klas_desc" : "Klas";
            ViewBag.DocentSortParm = sortOrder == "Docent" ? "docent_desc" : "Docent";
            ViewBag.LokaalSortParm = sortOrder == "Lokaal" ? "lokaal_desc" : "Lokaal";
            ViewBag.AanvangSortParm = sortOrder == "Aanvang" ? "aanvang_desc" : "Aanvang";
            ViewBag.DuurSortParm = sortOrder == "Duur" ? "duur_desc" : "Duur";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;


            //todo get rooster data once, look for solution in datamining project
            IEnumerable<Les> rooster = Repository.GetRoosterData(String.IsNullOrEmpty(searchString)?String.Empty:searchString);

            switch (sortOrder)
            {
                case "vak_desc":
                    rooster = rooster.OrderByDescending(x => x.Vak);
                    break;
                case "Klas":
                    rooster = rooster.OrderBy(s => s.Klas);
                    break;
                case "klas_desc":
                    rooster = rooster.OrderByDescending(s => s.Klas);
                    break;
                case "Docent":
                    rooster = rooster.OrderBy(s => s.Docent);
                    break;
                case "docent_desc":
                    rooster = rooster.OrderByDescending(s => s.Docent);
                    break;
                case "Lokaal":
                    rooster = rooster.OrderBy(s => s.Lokaal);
                    break;
                case "lokaal_desc":
                    rooster = rooster.OrderByDescending(s => s.Lokaal);
                    break;
                case "Aanvang":
                    rooster = rooster.OrderBy(s => s.StartTijd);
                    break;
                case "aanvang_desc":
                    rooster = rooster.OrderByDescending(s => s.StartTijd);
                    break;
                case "Duur":
                    rooster = rooster.OrderBy(s => s.Lengte);
                    break;
                case "duur_desc":
                    rooster = rooster.OrderByDescending(s => s.Lengte);
                    break;
                default:
                    rooster = rooster.OrderBy(s => s.Vak);
                    break;
            }
            int pageSize = 30;
            int pageNumber = (page ?? 1);

            var b = rooster.ToList();

            return View(b.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult MoreInfo(string id,bool add = false)
        {
            AgendaLibrary.Agenda a = new Agenda();
            List<Les> rooster = Repository.GetRooster();
            Les les = rooster.FirstOrDefault(x => x.GetHashCode().Equals(int.Parse(id)));

            if (add)
            {
                AgendaLibrary.Les l = new AgendaLibrary.Les()
                {
                    CalendarGuid = les.CalendarGuid,
                    Docent = les.Docent,
                    Klas = les.Klas,
                    Lengte = les.Lengte,
                    LesGuid = les.LesGuid,
                    Lokaal = les.Lokaal,
                    StartTijd = les.StartTijd,
                    Vak = les.Vak,
                    VakCode = les.VakCode,
                    VakId = les.VakId
                };
                a.UpdateEvent(l);
            }

            var userAgenda = a.GetUserAgenda();

            les.Attending = userAgenda.Items.Any(x => x.Id.Equals(les.CalendarGuid));

            var recurring = rooster.Where(x => x.VakCode == les.VakCode && x.Klas == les.Klas).ToList();
            foreach (var lese in recurring)
            {
                lese.Attending = userAgenda.Items.Any(x => x.Id.Equals(lese.CalendarGuid));
            }
            ViewBag.Recurring = recurring;

            return View(les);
        }
    }
}

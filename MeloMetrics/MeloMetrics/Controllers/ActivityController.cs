using MeloMetrics.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using MongoDB.Driver;


namespace MeloMetrics.Controllers
{
    public class ActivityController : Controller
    {

        // GET: Activity
        //[ValidateAntiForgeryToken]
        public ViewResult Index(string sortOrder, string searchString, string currentFilter, int? page)
        {

            ViewBag.CurrentSort = sortOrder;
            ViewBag.DateSortParm = String.IsNullOrEmpty(sortOrder) ? "date_desc" : "";
            ViewBag.NameSortParm =  sortOrder == "name" ? "name_desc" : "name";


            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            

            ViewBag.CurrentFilter = searchString;

            String aux = "";
            if (searchString != null)
            {
                aux = searchString;
            }
           
         
            int id_user = 0;
            MongoCursor<Activity> r;

            switch (sortOrder)
            {
                case "name":
                    r = MeloMetricsDB.getMeloMetricsDB().getMyActivityCollectionByNameAsc(id_user, aux);
                    break;
                case "name_desc":
                    r = MeloMetricsDB.getMeloMetricsDB().getMyActivityCollectionByNameDesc(id_user, aux);
                    break;
                case "date":
                    r = MeloMetricsDB.getMeloMetricsDB().getMyActivityCollectionByDateAsc(id_user, aux);
                    break;
                case "date_desc":
                    r = MeloMetricsDB.getMeloMetricsDB().getMyActivityCollectionByDateDesc(id_user, aux);
                    break;
                default:
                    r = MeloMetricsDB.getMeloMetricsDB().getMyActivityCollectionByDateAsc(id_user, aux);
                   break;
            }

            int pageSize = 3;
            int pageNumber = (page ?? 1);

            try
            {
                return View(r.ToPagedList(pageNumber, pageSize));
            }
            catch (MongoException ex)
            {
                throw new MongoException("Error Consultada la Base de Datos");
            }

        }


        // GET: Activity/Delete/5
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Activity/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            Exception ex = filterContext.Exception;
            filterContext.ExceptionHandled = true;

            var model = new HandleErrorInfo(filterContext.Exception, "Activity", "Index");

            filterContext.Result = new ViewResult()
            {
                ViewName = "Error",
                ViewData = new ViewDataDictionary(model)
            };

        }
    }
}

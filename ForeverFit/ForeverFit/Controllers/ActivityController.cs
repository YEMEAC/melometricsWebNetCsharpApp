using ForeverFit.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using MongoDB.Driver;
using Microsoft.AspNet.Identity;

namespace ForeverFit.Controllers
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


            string id_user = User.Identity.GetUserName();
            MongoCursor<Activity> r;

            switch (sortOrder)
            {
                case "name":
                    r = ForeverFitDB.getForeverFitDB().getMyActivityCollectionByNameAsc(id_user, aux);
                    break;
                case "name_desc":
                    r = ForeverFitDB.getForeverFitDB().getMyActivityCollectionByNameDesc(id_user, aux);
                    break;
                case "date":
                    r = ForeverFitDB.getForeverFitDB().getMyActivityCollectionByDateAsc(id_user, aux);
                    break;
                case "date_desc":
                    r = ForeverFitDB.getForeverFitDB().getMyActivityCollectionByDateDesc(id_user, aux);
                    break;
                default:
                    r = ForeverFitDB.getForeverFitDB().getMyActivityCollectionByDateAsc(id_user, aux);
                   break;
            }

            int pageSize = 13;
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
        //[ValidateAntiForgeryToken]
        public ActionResult Delete(string id)
        {
            try
            {
                ForeverFitDB.getForeverFitDB().deleteMyActivityCollectionAndRecors(id);
            }
            catch
            {
                throw new MongoException("Error borrando Activity");
            }

            return RedirectToAction("Index", "Activity");
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

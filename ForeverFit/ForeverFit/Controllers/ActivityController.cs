using ForeverFit.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using MongoDB.Driver;
using Microsoft.AspNet.Identity;
using System.Web.Security;

namespace ForeverFit.Controllers
{
    public class ActivityController : Controller
    {



       

        //autorized hace que pase solo si ha quedado como logueado anteriormente
        [Authorize]
        public ActionResult Index(string sortOrder, string searchString, string currentFilter, int? page)
        {

            //si se ha iniciado sesion y no se ha cerrado
            if (Request.Cookies.AllKeys.Contains("user") && Request.Cookies.AllKeys.Contains("pwd"))
            {
                //si el user se ha borrado -> al parar el servidor o cerrar la web para,
                //lo recupero porque no se cerre la sesion
                if (System.Web.HttpContext.Current.Session["user"]==null)
                {
                    if (!UserController.LoginAux(Request.Cookies["user"].Value, Request.Cookies["pwd"].Value))
                    {
                        //este caso solo pasaria si se borrara el usuario de bd directamente
                        //cuando no ha hecho logout me aseguro de borrar todo y cerrar la sesion
                        return RedirectToAction("Logout", "User");  
                    }
                }


                ViewBag.CurrentSort = sortOrder;
                ViewBag.DateSortParm = String.IsNullOrEmpty(sortOrder) ? "date_desc" : "";
                ViewBag.NameSortParm = sortOrder == "name" ? "name_desc" : "name";


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

                var u = ((User)System.Web.HttpContext.Current.Session["user"]);
                var id_user = u.Id.ToString();

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
            else
            {
                //usuario ha quedado logueado pero se ha borrado la coocki
                //me aseguro de borrar todo y cerrar la sesion

                return RedirectToAction("Logout", "User");

            }
        }

        // GET: Activity/Delete/5
        //[ValidateAntiForgeryToken]
        [Authorize]
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

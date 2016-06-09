using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ForeverFit.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
             //si se ha iniciado sesion y no se ha cerrado
            if (Request.Cookies.AllKeys.Contains("user") && Request.Cookies.AllKeys.Contains("pwd"))
            {
                //si el user se ha borrado -> al parar el servidor o cerrar la web para,
                //lo recupero porque no se cerre la sesion
                if (System.Web.HttpContext.Current.Session["user"] == null)
                {
                    if (!UserController.LoginAux(Request.Cookies["user"].Value, Request.Cookies["pwd"].Value))
                    {
                        //este caso solo pasaria si se borrara el usuario de bd directamente
                        //cuando no ha hecho logout me aseguro de borrar todo y cerrar la sesion
                        RedirectToAction("Logout", "User");
                    }
                }
                //todo ok
            }
            else
            {
                //usuario ha quedado logueado pero se ha borrado la coocki
                //me aseguro de borrar todo y cerrar la sesion
                RedirectToAction("Logout", "User");
            }
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            Exception ex = filterContext.Exception;
            filterContext.ExceptionHandled = true;

            var model = new HandleErrorInfo(filterContext.Exception, "HomeController", "Index");

            filterContext.Result = new ViewResult()
            {
                ViewName = "Error",
                ViewData = new ViewDataDictionary(model)
            };

        }
    }
}
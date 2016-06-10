using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Data.Entity;
using ForeverFit.Models;


namespace ForeverFit.Controllers
{
    public class UserController : Controller
    {

        
        public ActionResult Index()
        {
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(Models.User user)
        {
            if (ModelState.IsValid)
            {
                var u = user.IsValid(user.UserName, user.Password);
                if (u!=null)
                {

                    Response.Cookies["user"].Value = user.UserName;
                    Response.Cookies["pwd"].Value = user.Password;

                    FormsAuthentication.SetAuthCookie(u.UserName,true);

                    //guardar la infor del usuario logueado en la sesion - se borra al parar el server o cerrar la web
                    System.Web.HttpContext.Current.Session.Add("user", u);

                    FormsAuthentication.RedirectFromLoginPage(u.UserName,true);
                    //return RedirectToAction("Index", "Home");  
                }
                else
                {
                    ModelState.AddModelError("", "Identificacion Incorrecta!");
                }
            }
            return View(user);
        }

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(Models.User user)
        {
            if (ModelState.IsValid)
            {
                if (user.Persist())
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Error registrando usuario");
                }
            }
            return View(user);
        }

        internal static bool LoginAux(string user, string pwd)
        {
            User u = new User();
            u = u.IsValid(user, pwd);

            if (u != null)
            {
                FormsAuthentication.SetAuthCookie(u.UserName, true);
                System.Web.HttpContext.Current.Session.Add("user", u);
                return true;
            }
            return false;
        }


        [Authorize]
        public ActionResult Logout()
        {
            //borrar cookies
            Response.Cookies["user"].Expires = DateTime.Now.AddDays(-1);   
            Response.Cookies["pwd"].Expires = DateTime.Now.AddDays(-1);
            Session.Abandon();
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            Exception ex = filterContext.Exception;
            filterContext.ExceptionHandled = true;

            var model = new HandleErrorInfo(filterContext.Exception, "UserController", "Index");

            filterContext.Result = new ViewResult()
            {
                ViewName = "Error",
                ViewData = new ViewDataDictionary(model)
            };

        }
    }
}
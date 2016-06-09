using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Data.Entity;


namespace ForeverFit.Controllers
{
    public class UserController : Controller
    {


        [HttpPost]
        public ActionResult Login(Models.User user)
        {
            if (ModelState.IsValid)
            {
                var u = user.IsValid(user.UserName, user.Password);
                if (u!=null)
                {
                    FormsAuthentication.SetAuthCookie(u.UserName,true);
                    System.Web.HttpContext.Current.Session.Add("user", u);

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Identificacion Incorrecta!");
                }
            }
            return View(user);
        }


        [Authorize]
        public ActionResult Logout()
        {
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
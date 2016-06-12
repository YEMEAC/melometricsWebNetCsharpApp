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
            if (Request.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
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
            if (Request.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.GenreList = getGenreList();
            return View();
        }

        [HttpPost]
        public ActionResult Register(Models.User user)
        {
            if (ModelState.IsValid)
            {
                user.Genero = Int32.Parse(Request.Form["GenreList"]);
                var r= user.Persist();
                if (r==0)
                {
                    Login(user);
                    return RedirectToAction("Index", "Home");
                }
                else if(r==2)
                {
                    ModelState.AddModelError("", "Nombre de usuario en uso");
                }else{
                    
                    ModelState.AddModelError("", "Error registrando usuario");
                }
            }
            ViewBag.GenreList = getGenreList();
            return View(user);
        }


        [HttpGet]
        [Authorize]
        public ActionResult Edit()
        {
            if (!Request.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            var user = (User)System.Web.HttpContext.Current.Session["user"];
            ViewBag.GenreList = getGenreListEdit(user.Genero);
            return View(user);
        }

        [HttpPost]
        [Authorize]
        public ActionResult Edit(Models.User user)
        {
            ModelState.Remove("Password");
            ModelState.Remove("Username");
            if (ModelState.IsValid)
            {
                user.Genero = Int32.Parse(Request.Form["GenreList"]);
                var r = user.PersistEdit();
                if (r != 0)
                {
                    ModelState.AddModelError("", "Error modificando el usuario");
                }
                else
                {
                    System.Web.HttpContext.Current.Session.Remove("user");
                    System.Web.HttpContext.Current.Session.Add("user", user);
                }
            }

            ViewBag.GenreList = getGenreListEdit(user.Genero);
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

        private List<SelectListItem> getGenreList()
        {
            List<SelectListItem> items = new List<SelectListItem>();

            items.Add(new SelectListItem { Text = "Mujer", Value = "0" });

            items.Add(new SelectListItem { Text = "Hombre", Value = "1", Selected = true });

            return items;
        }

        private List<SelectListItem> getGenreListEdit(int genre)
        {
            List<SelectListItem> items = new List<SelectListItem>();

            if (genre == 1)
            {
                return getGenreList();
            }
            items.Add(new SelectListItem { Text = "Mujer", Value = "0", Selected = true});
            items.Add(new SelectListItem { Text = "Hombre", Value = "1" });

            return items;
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
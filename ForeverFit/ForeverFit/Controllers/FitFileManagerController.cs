using ForeverFit.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;

//limpiar obran metodos
namespace ForeverFit.Controllers
{
    public class FitFileManagerController : Controller
    {

        private readonly FitFileManager fitFileManager = new FitFileManager();

        // This action handles the form POST and the upload
        [HttpPost]
        public ActionResult Index(HttpPostedFileBase fileNameIdController)
        {
            // vericar que se ha seleccionado unarchivo
            if (fileNameIdController != null && fileNameIdController.ContentLength > 0)
            {

                //subir archivo al server
                string id_user = ((User) System.Web.HttpContext.Current.Session["user"]).Id.ToString();
                var fileName = Path.GetFileName(fileNameIdController.FileName);
                string path = Path.Combine(Server.MapPath("~/uploads"), fileName);
                fileNameIdController.SaveAs(path);

                //leer archivo
                List<String> records = fitFileManager.readFile(path); 
                //guardar en BD
                ForeverFitDB.getForeverFitDB().insertActivityAndRecords(records, id_user, fileName, records[1]);
               
            }
            return RedirectToAction("Index", "Activity");
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            Exception ex = filterContext.Exception;
            filterContext.ExceptionHandled = true;

            var model = new HandleErrorInfo(filterContext.Exception, "FitFileManageController", "Index");

            filterContext.Result = new ViewResult()
            {
                ViewName = "Error",
                ViewData = new ViewDataDictionary(model)
            };

        }
    }


}

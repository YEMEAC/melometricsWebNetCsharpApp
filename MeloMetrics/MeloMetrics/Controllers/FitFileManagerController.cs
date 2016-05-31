using MeloMetrics.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

//limpiar obran metodos
namespace MeloMetrics.Controllers
{
    public class FitFileManagerController : Controller
    {

        private readonly FitFileManager fitFileManager = new FitFileManager();

        // GET: FitFileManager
        public ActionResult Index()
        {
            return View();
        }

        // This action handles the form POST and the upload
        [HttpPost]
        public ActionResult Index(HttpPostedFileBase fileNameIdController)
        {
            // Verify that the user selected a file
            if (fileNameIdController != null && fileNameIdController.ContentLength > 0)
            {

                string id_user = "0";
                var fileName = Path.GetFileName(fileNameIdController.FileName);
                //string path = Path.Combine(Server.MapPath("~/App_Data/uploads"), fileName);
                string path = Path.Combine(Server.MapPath("~/uploads"), fileName);
                fileNameIdController.SaveAs(path);

               List<String> records = fitFileManager.readFile(path);
               MeloMetricsDB.getMeloMetricsDB().insertActivityAndRecords(records, id_user, fileName, records[1]);
               
            }
            // redirect back to the index action to show the form once again
            return RedirectToAction("Index");
        }

        // GET: FitFileManager/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: FitFileManager/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: FitFileManager/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: FitFileManager/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: FitFileManager/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: FitFileManager/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: FitFileManager/Delete/5
        [HttpPost]
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

            var model = new HandleErrorInfo(filterContext.Exception, "FitFileManageController", "Index");

            filterContext.Result = new ViewResult()
            {
                ViewName = "Error",
                ViewData = new ViewDataDictionary(model)
            };

        }
    }


}

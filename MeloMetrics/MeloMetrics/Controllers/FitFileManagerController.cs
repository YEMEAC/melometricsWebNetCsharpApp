using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace MeloMetrics.Controllers
{
    public class FitFileManagerController : Controller
    {
        // GET: FitFileManager
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// to Save DropzoneJs Uploaded Files
        /// </summary>
        public ActionResult SaveDropzoneJsUploadedFiles()
        {
            foreach (string fileName in Request.Files)
            {
                HttpPostedFileBase file = Request.Files[fileName];
                //You can Save the file content here
            }

            return Json(new { Message = string.Empty });
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
    }
}

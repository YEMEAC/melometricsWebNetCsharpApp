using MeloMetrics.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MeloMetrics.Controllers
{
    public class ActivityController : Controller
    {
        private readonly MeloMetricsDB meloMetricsDB = new MeloMetricsDB();
        private readonly FitFileManager fitFileManager = new FitFileManager();

        // GET: Activity
        //[ValidateAntiForgeryToken]
        public ActionResult Index()
        {

            string id_user = "0";

            //List<String> records = fitFileManager.readFile();
            //meloMetricsDB.insertActivityAndRecords(records, id_user, "test1", records[1]);


            var r=meloMetricsDB.getMyActivityCollection(id_user);
            return View(r);
        }

        public ActionResult DetalleActiviy(string id_activity)
        {
            var r = meloMetricsDB.getMyActivitysRecordsCollection(id_activity);
            return View(r);
        }


        // GET: Activity/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Activity/Create
         //[ValidateAntiForgeryToken]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Activity/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
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

        // GET: Activity/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Activity/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
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

        // GET: Activity/Delete/5
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
    }
}

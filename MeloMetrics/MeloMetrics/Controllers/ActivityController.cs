﻿using MeloMetrics.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using MongoDB.Driver;

namespace MeloMetrics.Controllers
{
    public class ActivityController : Controller
    {

        // GET: Activity
        //[ValidateAntiForgeryToken]
        public ActionResult Index(string sortOrder)
        {

            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";

            string id_user = "0";
            MongoCursor<Activity> r;

            switch (sortOrder)
            {
                case "name":
                     r = MeloMetricsDB.getMeloMetricsDB().getMyActivityCollection(id_user);
                    break;
                case "name_desc":
                    r = MeloMetricsDB.getMeloMetricsDB().getMyActivityCollection(id_user);
                    break;
                case "Date":
                    r = MeloMetricsDB.getMeloMetricsDB().getMyActivityCollection(id_user);
                    break;
                case "date_desc":
                    r =MeloMetricsDB.getMeloMetricsDB().getMyActivityCollection(id_user);
                    break;
                case "identificador"
                     r =MeloMetricsDB.getMeloMetricsDB().getMyActivityCollection(id_user);
                    break;
                case "identificador_desc"
                     r =MeloMetricsDB.getMeloMetricsDB().getMyActivityCollection(id_user);
                    break;
                default:
                    r = MeloMetricsDB.getMeloMetricsDB().getMyActivityCollection(id_user);
                    break;
            }

            
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

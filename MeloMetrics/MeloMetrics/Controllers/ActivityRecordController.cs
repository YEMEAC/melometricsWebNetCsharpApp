﻿using System.Web.Mvc;
using MeloMetrics.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver.Builders;
using Microsoft.Scripting.Hosting;
using IronPython.Hosting;
using Microsoft.Scripting;
using MongoDB.Driver;
using PagedList;

namespace MeloMetrics.Controllers
{
    public class ActivityRecordController : Controller
    {
       
     
        // GET: ActivityRecord
        //[ValidateAntiForgeryToken]
        public ActionResult Index(string id_activity, int? page)
        {

            if (id_activity != null)
            {
                ViewBag.IdActivity = id_activity;
            }


            MongoCursor<ActivityRecord> r = MeloMetricsDB.getMeloMetricsDB().getMyActivitysRecordsCollection(id_activity);

            int pageSize = 3;
            int pageNumber = (page ?? 1);
            return View(r.ToPagedList(pageNumber, pageSize));
        }


        // GET: ActivityRecord
        /*
        public ActionResult Index()
        {
            long id_user = 0;
            //List<String>  aux = fitFileManager.readFile();
          
            //meloMetricsDB.insertDocumentsOneMileWalkTes(aux, id_user);

            //var oneMileWalktest = meloMetricsDB.getMyOneMileWalkTestCollection(id_user);
            //var oneMileWalktest = meloMetricsDB.getMyActiVYrECOR(id_user, "14627228151429631557");
            return View(oneMileWalktest);

        }

        // GET: ActivityRecord/Details/5
        public ActionResult Details(string id)
        {
            var test = meloMetricsDB.OneMileWalkTestCollection.FindOneById(new ObjectId(id));
            return View(test);
        }


        // GET: ActivityRecord/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ActivityRecord/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ActivityRecordController m)
        {
            if (ModelState.IsValid)
            {
                meloMetricsDB.OneMileWalkTestCollection.Insert(m);
                return RedirectToAction("Index");
            }
            return View();
        }

        // GET: ActivityRecord/Edit/5
        public ActionResult Edit(string id)
        {
            var test = meloMetricsDB.OneMileWalkTestCollection.FindOneById(new ObjectId(id));
            return View(test);
        }

        // POST: ActivityRecord/Edit/5
        [HttpPost]
        public ActionResult Edit(ActivityRecordController t)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    meloMetricsDB.OneMileWalkTestCollection.Save(t);
                    return RedirectToAction("Index");
                }

                return View();
            }
            catch
            {
                return View();
            }
        }

        // GET: ActivityRecord/Delete/5
        public ActionResult Delete(string Id)
        {
            //var rental = meloMetricsDB.OneMileWalkTestCollection.FindOneById(new ObjectId("572b96fcf23adc16440d2daa"));
            var rental = meloMetricsDB.OneMileWalkTestCollection.FindOneById(new ObjectId(Id));
            return View(rental);
        }

        // POST: ActivityRecord/Delete/5
        [HttpPost]
        public ActionResult Delete(string id, FormCollection collection)
        {
            try
            {
                var rental = meloMetricsDB.OneMileWalkTestCollection.Remove(Query.EQ("_id", new ObjectId(id)));
                return RedirectToAction("Index");

            }
            catch
            {
                return View();
            }
        }*/
    }
}

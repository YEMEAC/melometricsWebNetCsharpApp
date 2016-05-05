using System.Web.Mvc;
using MeloMetrics.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;

namespace MeloMetrics.Controllers
{
    public class OneMileWalkTestController : Controller
    {
        private readonly MeloMetricsDB Context = new MeloMetricsDB();
        
        // GET: OneMileWalkTest
        public ActionResult Index()
        {
            var filter = new BsonDocument();
            var sort = Builders<BsonDocument>.Sort.Ascending("borough").Ascending("address.zipcode");

            var test = await Context.OneMileWalkTestCollection.Find(filter).Sort(sort).ToListAsync();
            return View();
        }

        // GET: OneMileWalkTest/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: OneMileWalkTest/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: OneMileWalkTest/Create
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

        // GET: OneMileWalkTest/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: OneMileWalkTest/Edit/5
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

        // GET: OneMileWalkTest/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: OneMileWalkTest/Delete/5
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

using System.Web.Mvc;
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

namespace MeloMetrics.Controllers
{
    public class OneMileWalkTestController : Controller
    {
        private readonly MeloMetricsDB Context = new MeloMetricsDB();
        private ScriptEngine m_engine = Python.CreateEngine();
        private ScriptScope m_scope = null;
        
        // GET: OneMileWalkTest
        public ActionResult Index()
        {



            string pathFile = "C:/Users/Jeison/Source/Repos/tfgweb/MeloMetrics/MeloMetrics/python-fitparse-master/scripts/sample_program.py";
            string pathFitFile = "C:/Users/Jeison/Source/Repos/tfgweb/MeloMetrics/MeloMetrics/python-fitparse-master/tests/data/mio.fit";
            

            ScriptEngine engine = Python.CreateEngine();
            //urls donde ir a buscar includes y clases para compilar el script
            engine.SetSearchPaths(new string[] { "C:/Users/Jeison/Source/Repos/tfgweb/MeloMetrics/MeloMetrics/python-fitparse-master/fitparse", "D:/Program Files (x86)/IronPython 2.7/Lib" });
            ScriptSource source = engine.CreateScriptSourceFromFile(pathFile);
            ScriptScope scope = engine.CreateScope();
            scope.SetVariable("pathFitFile", pathFitFile);
            ObjectOperations op = engine.Operations;
            var result=source.Execute(scope);

           //dynamic Calculator = scope.GetVariable("txt");
           
            

            var oneMileWalktest = Context.OneMileWalkTestCollection.FindAll().SetSortOrder(SortBy<OneMileWalkTest>.Ascending(r => r.Id));
            return View(oneMileWalktest);

        }

        // GET: OneMileWalkTest/Details/5
        public ActionResult Details(string id)
        {
            var test = Context.OneMileWalkTestCollection.FindOneById(new ObjectId(id));
            return View(test);
        }


        // GET: OneMileWalkTest/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: OneMileWalkTest/Create
        [HttpPost]
        public ActionResult Create(OneMileWalkTest m)
        {
            if (ModelState.IsValid)
            {
                Context.OneMileWalkTestCollection.Insert(m);
                return RedirectToAction("Index");
            }
            return View();
        }

        // GET: OneMileWalkTest/Edit/5
        public ActionResult Edit(string id)
        {
            var test = Context.OneMileWalkTestCollection.FindOneById(new ObjectId(id));
            return View(test);
        }

        // POST: OneMileWalkTest/Edit/5
        [HttpPost]
        public ActionResult Edit(OneMileWalkTest t)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Context.OneMileWalkTestCollection.Save(t);
                    return RedirectToAction("Index");
                }

                return View();
            }
            catch
            {
                return View();
            }
        }

        // GET: OneMileWalkTest/Delete/5
        public ActionResult Delete(string Id)
        {
            //var rental = Context.OneMileWalkTestCollection.FindOneById(new ObjectId("572b96fcf23adc16440d2daa"));
            var rental = Context.OneMileWalkTestCollection.FindOneById(new ObjectId(Id));
            return View(rental);
        }

        // POST: OneMileWalkTest/Delete/5
        [HttpPost]
        public ActionResult Delete(string id, FormCollection collection)
        {
            try
            {
                var rental = Context.OneMileWalkTestCollection.Remove(Query.EQ("_id", new ObjectId(id)));
                return RedirectToAction("Index");

            }
            catch
            {
                return View();
            }
        }
    }
}

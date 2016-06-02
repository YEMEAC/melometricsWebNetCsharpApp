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
using MongoDB.Driver;
using PagedList;

namespace MeloMetrics.Controllers
{
    public class ActivityRecordController : Controller
    {
       
     
        // GET: ActivityRecord
        //[ValidateAntiForgeryToken]
        public ActionResult Index(string id, int? page)
        {

            if (id != null)
            {
                ViewBag.IdActivity = id;
            }


            MongoCursor<ActivityRecord> r = MeloMetricsDB.getMeloMetricsDB().getMyActivitysRecordsCollection(id);
            if (r.Size() == 0){throw new Exception("No results");}
            calculaMetricas(r);

            int pageSize = 3;
            int pageNumber = (page ?? 1);

            return View(r.ToPagedList(pageNumber, pageSize));
        }

        private void calculaMetricas(MongoCursor<ActivityRecord> r)
        {
            List<ActivityRecord> aux = r.ToList<ActivityRecord>();
            ViewBag.date = aux[0].timestamp;
            ViewBag.count = aux.Count;
            ViewBag.distance = aux[aux.Count - 1].distance;
            ViewBag.duration = (aux[aux.Count - 1].timestamp - aux[0].timestamp).Minutes;

            vo2maxSpeedTest(aux);
            oneHalfMileRunTest(aux);
            OneMileWalkTest(aux);
           
        }

        private void vo2maxSpeedTest(List<ActivityRecord> registros)
        {
            var duration =  (registros[registros.Count - 1].timestamp- registros[0].timestamp).Minutes;
            
            //la duracion minima es de 12 minutos de actividad para el test
            if (duration >= 12)
            {

                int registroInicioTest = -1;

                //buscar a partir de donde han pasado 12 minutos los anteriores se no tienen en cuenta
                for (int i = 0; i < registros.Count  && registroInicioTest==-1; ++i)
                {
                   var diferencia = (registros[i].timestamp - registros[0].timestamp).Minutes;
                   if (diferencia >= 12) { registroInicioTest = i; }
                }

                var contadorVo2maxSpeedMuestras = registros.Count;
                var acumuladorVo2maxSpeed = 0.0d;
                var maxHeartRate = 186.0d;
                var restingHeartRate = 56.0d;
                var media = 0.0d;
                
                //registros del test
                for (int i = registroInicioTest; i < registros.Count; ++i)
                {
                    var heartRateReserve = maxHeartRate - restingHeartRate;
                    //aux=current runnig heart rate as a percentage of hr reserve
                    var aux = (registros[i].heart_rate - restingHeartRate) / heartRateReserve;

                    var velocidad = registros[i].speed * 2.23694; // m/s to mph

                    var estimacionVo2maxSpeed =  velocidad/aux;

                    acumuladorVo2maxSpeed += estimacionVo2maxSpeed;   
                }
                media = acumuladorVo2maxSpeed / (contadorVo2maxSpeedMuestras - registroInicioTest -1);

                 ViewBag.vo2maxspeed = media;
            }
        }


         private void oneHalfMileRunTest(List<ActivityRecord> registros)
         {

             var registroInicioTest = -1;
             for (int i = 0; i < registros.Count && registroInicioTest == -1; ++i)
             {
                 var distanciaAux = registros[i].distance * 2.23694;  // m/s tp mph
                 if (distanciaAux >= 1.5) { registroInicioTest = i; }   //distancia recorrida necesaria
             }

             if (registroInicioTest != -1)
             {

             }
            
        }

         private void OneMileWalkTest(List<ActivityRecord> registros)
        {

                var registroInicioTest = -1;
                for (int i = 0; i < registros.Count && registroInicioTest == -1; ++i)
                {
                    var distanciaAux = registros[i].distance * 2.23694;  // m/s tp mph
                    if (distanciaAux >= 1) { registroInicioTest = i; }   //distancia recorrida necesaria
                }

                if (registroInicioTest != -1)
                {

                }
        }


        protected override void OnException(ExceptionContext filterContext)
        {
            Exception ex = filterContext.Exception;
            filterContext.ExceptionHandled = true;

            var model = new HandleErrorInfo(filterContext.Exception, "ActivityRecordController", "Index");

            filterContext.Result = new ViewResult()
            {
                ViewName = "Error",
                ViewData = new ViewDataDictionary(model)
            };

        }

        // GET: ActivityRecord
        /*
        public ActionResult Index()
        {
            long id_user = 0;
            //List<String>  registros = fitFileManager.readFile();
          
            //meloMetricsDB.insertDocumentsOneMileWalkTes(registros, id_user);

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

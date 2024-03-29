﻿using System.Web.Mvc;
using ForeverFit.Models;
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
using System.Web.Security;

namespace ForeverFit.Controllers
{
    public class ActivityRecordController : Controller
    {


    

        // GET: ActivityRecord
        //[ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Index(string id, int? page)
        {

              //si se ha iniciado sesion y no se ha cerrado
            if (Request.Cookies.AllKeys.Contains("user") && Request.Cookies.AllKeys.Contains("pwd"))
            {
                //si el user se ha borrado -> al parar el servidor o cerrar la web para,
                //lo recupero porque no se cerre la sesion
                if (System.Web.HttpContext.Current.Session["user"] == null)
                {
                    if (!UserController.LoginAux(Request.Cookies["user"].Value, Request.Cookies["pwd"].Value))
                    {
                        //este caso solo pasaria si se borrara el usuario de bd directamente
                        //cuando no ha hecho logout me aseguro de borrar todo y cerrar la sesion
                        return RedirectToAction("Logout", "User");
                    }
                }

                if (id == null || string.IsNullOrEmpty(id))
                {
                    return RedirectToAction("Index", "Home");
                }

                if (id != null)
                {
                    ViewBag.IdActivity = id;
                }


                MongoCursor<ActivityRecord> r = ForeverFitDB.getForeverFitDB().getMyActivitysRecordsCollection(id);
                if (r.Size() == 0)
                {
                    throw new Exception("Sin resultados para el activity " + id);
                }

                calculaMetricas(r);

                int pageSize = 13;
                int pageNumber = (page ?? 1);

                try
                {
                    return View(r.ToPagedList(pageNumber, pageSize));
                }
                catch (MongoException ex)
                {
                    throw new MongoException("Error Consultada la Base de Datos");
                }
            }
            else
            {
                //usuario ha quedado logueado pero se ha borrado la coocki
                //me aseguro de borrar todo y cerrar la sesion
                return RedirectToAction("Logout", "User");
            }
          
        }


        private void calculaMetricas(MongoCursor<ActivityRecord> r)
        {
            try
            {
                List<ActivityRecord> aux = r.ToList<ActivityRecord>();
                ViewBag.date = aux[0].timestamp;
                ViewBag.count = aux.Count;
                ViewBag.distance = aux[aux.Count - 1].distance;
                ViewBag.duration = (aux[aux.Count - 1].timestamp - aux[0].timestamp).Minutes;

                OneMileWalkTest(aux);
               
                oneHalfMileRunTest(aux);
                vo2maxSpeedTest(aux);
                
            }
            catch (Exception e)
            {
                throw new Exception("Error calculandometricas: " + e.ToString());
            }
           
        }


        private void OneMileWalkTest(List<ActivityRecord> registros)
        {

            //1 milla = 1,60934 km = 1609,34 m
            //1,5 milla = 2,41402 km = 2414,02 m
            var distancia = registros[registros.Count() - 1].distance / 1000;  // m to km

            if (distancia >= 1.61d)
            {
                var registroInicioTest = -1;
                for (int i = 0; i < registros.Count && registroInicioTest == -1; ++i)
                {
                    var distanciaAux = registros[i].distance / 1000;   // metres to km
                    if (distanciaAux >= 1.61d)
                    {
                        registroInicioTest = i;
                    }   //distancia recorrida necesaria
                }

                if (registroInicioTest != -1)
                {
                    //tiempo en completar la distancia
                    var minutos = (registros[registroInicioTest].timestamp - registros[0].timestamp).TotalMinutes;
                    // 1 male female 0
                    var genero = ((User)System.Web.HttpContext.Current.Session["user"]).Genero;
                    var peso = ((User)System.Web.HttpContext.Current.Session["user"]).Weight*1000 *0.00220462;    //pounds  kg to g, 1g =0.00220462 pounds
                    var edad = GetAge(((User)System.Web.HttpContext.Current.Session["user"]).BirthDate);
                        



                    var aux = 132.853 - 0.0769 * peso - 0.3877 * edad + 6.315 * genero - 3.2649 * minutos - 0.1565 * registros[registroInicioTest].heart_rate;
                    ViewBag.OneMileWalkTest = string.Format("{0:0.00}", aux); 
                }
                else
                {
                    ViewBag.OneMileWalkTest = "problema encontrado el punto de corte";
                }
            }
            else
            {
                ViewBag.OneMileWalkTest = "distancia insuficiente";
            }
        }

        private int GetAge(DateTime birthday)
        {
            DateTime now = DateTime.Today;
            int age = now.Year - birthday.Year;
            if (now < birthday.AddYears(age)) age--;

            return age;
        }


         private void oneHalfMileRunTest(List<ActivityRecord> registros)
         {

             //1 milla = 1,60934 km = 1609,34 m
             //1,5 milla = 2,41402 km = 2414,02 m
             var distancia = registros[registros.Count()-1].distance / 1000;  // m to km

             if (distancia >= 2.42d) //distancia recorrida necesaria
             {
                 var registroInicioTest = -1;
                 for (int i = 0; i < registros.Count && registroInicioTest == -1; ++i)
                 {
                     var distanciaAux = registros[i].distance / 1000;   // metres to km
                     if (distanciaAux >= 2.42d) { 
                         registroInicioTest = i; 
                     }   
                 }

                 if (registroInicioTest != -1)
                 {
                     var a = 483.0d;
                     var b = 3.5d;
                     //tiempo en completar la distancia
                     var minutos = (registros[registroInicioTest].timestamp - registros[0].timestamp).TotalMinutes;
                     var c = (a / minutos) + b;
                     ViewBag.oneHalfMileRunTest = string.Format("{0:0.00}", c);
                 }
                 else
                 {
                     ViewBag.oneHalfMileRunTest = "problema encontrado el punto de corte";
                 }
             }
             else
             {
                 ViewBag.oneHalfMileRunTest  = "distancia insuficiente";
             }
             
        }

         private void vo2maxSpeedTest(List<ActivityRecord> registros)
         {
             var duration = (registros[registros.Count - 1].timestamp - registros[0].timestamp).Minutes;

             //la duracion minima es de 12 minutos de actividad para el test
             if (duration >= 12)
             {

                 int registroInicioTest = -1;

                 //buscar a partir de donde han pasado 12 minutos los anteriores se no tienen en cuenta
                 for (int i = 0; i < registros.Count && registroInicioTest == -1; ++i)
                 {
                     var diferencia = (registros[i].timestamp - registros[0].timestamp).Minutes;
                     if (diferencia >= 12) { registroInicioTest = i; }
                 }

          
                 var acumuladorVo2maxSpeed = 0.0d;
                 var maxHeartRate = ((User)System.Web.HttpContext.Current.Session["user"]).MaxHeartRate;
                 var restingHeartRate = ((User)System.Web.HttpContext.Current.Session["user"]).RestingHeartRate;
                
                 //registros del test
                 for (int i = registroInicioTest; i < registros.Count; ++i)
                 {
                     var heartRateReserve = maxHeartRate - restingHeartRate;
                     //aux=current runnig heart rate as a percentage of hr reserve
                     var aux = (registros[i].heart_rate - restingHeartRate) / heartRateReserve;

                     var velocidad = registros[i].speed * 2.23694; // m/s to mph

                     var estimacionVo2maxSpeed = velocidad / aux;

                     acumuladorVo2maxSpeed += estimacionVo2maxSpeed;

                     if (i == registroInicioTest)
                     {
                         ViewBag.vo2maxspeedDoceMinutos = string.Format("{0:0.00}", acumuladorVo2maxSpeed);
                         //estimacion al llegar a 12minutos, la otra sera de todo el activity por lo tanto una media
                     }

                 }
                 //la estimacion de todo equivale a la continua y es la media de las estimaciones, el numero de estiamciones
                 //es el total menos las iniciales antes de llegar a los 12 minutos que se descartan
                 //el -1 es para que el registro justo del momento 12 si se tenga en cuenta
                 ViewBag.vo2maxspeedActivityCompleto = string.Format("{0:0.00}", acumuladorVo2maxSpeed / (registros.Count - (registroInicioTest)));


             }
             else
             {
                  ViewBag.vo2maxspeedDoceMinutos = "N/A";
                  ViewBag.vo2maxspeedActivityCompleto = "N/A";
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
    }
}

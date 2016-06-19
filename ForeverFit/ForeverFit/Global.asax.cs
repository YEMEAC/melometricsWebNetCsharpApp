using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;

namespace ForeverFit
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            Application_BeginRequest();
        }

        protected void Application_BeginRequest()
        {
                //definir idioma para los mensajes de error automaticas ..etc.
                Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture("es-ES");
         
        }
    }
}

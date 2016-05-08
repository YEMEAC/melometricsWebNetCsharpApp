using IronPython.Hosting;
using Microsoft.Scripting.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MeloMetrics.Models
{
    public class FitFileManager
    {

        ScriptEngine engine;
        ScriptSource source;
        ScriptScope scope;

        public FitFileManager()
        {
            engine = Python.CreateEngine();
            //urls donde ir a buscar includes y clases para compilar el script
            engine.SetSearchPaths(new string[] { "C:/Users/Jeison/Source/Repos/tfgweb/MeloMetrics/MeloMetrics/python-fitparse-master/fitparse", "D:/Program Files (x86)/IronPython 2.7/Lib" });
            
        }

        public List<String> readFile()
        {
            string pathFile = "C:/Users/Jeison/Source/Repos/tfgweb/MeloMetrics/MeloMetrics/python-fitparse-master/scripts/meloMetricsFitReader.py";
            string pathFitFile = "C:/Users/Jeison/Source/Repos/tfgweb/MeloMetrics/MeloMetrics/python-fitparse-master/tests/data/sample-activity.fit";
   
            source = engine.CreateScriptSourceFromFile(pathFile);
            scope = engine.CreateScope();
     
            scope.SetVariable("pathFitFile", pathFitFile);
            source.Execute(scope);

            string datos = scope.GetVariable("result_string");
            string[] tokens = datos.Split('|');
            List<string> aux = tokens.ToList(); //confirimado que da lo mismo que en el out del archivo al hacer el split-1 , token size=12*lineas-1, lineas = numero de documentso en bd
            aux.RemoveAt(aux.Count - 1);
            return aux;
        }
    }
}
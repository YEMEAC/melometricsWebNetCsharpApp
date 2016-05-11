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

        private ScriptEngine engine;
        private ScriptSource source;
        private ScriptScope scope;
        private string pathPythonScript = "C:/Users/ymeloa/Source/Repos/tfgweb/MeloMetrics/MeloMetrics/python-fitparse-master/scripts/meloMetricsFitReader.py";

        public FitFileManager()
        {
            engine = Python.CreateEngine();
            //urls donde ir a buscar includes y clases para compilar el script
            engine.SetSearchPaths(new string[] { "C:/Users/ymeloa/Source/Repos/tfgweb/MeloMetrics/MeloMetrics/python-fitparse-master/fitparse", "D:/Program Files (x86)/IronPython 2.7/Lib" });
               
        }

        public List<String> readFile(string pathFitFile)
        {
            
            source = engine.CreateScriptSourceFromFile(pathPythonScript);
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
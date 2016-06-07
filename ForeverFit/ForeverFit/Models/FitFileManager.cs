using IronPython.Hosting;
using Microsoft.Scripting.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ForeverFit.Models
{
    public class FitFileManager
    {

        private ScriptEngine engine;
        private ScriptSource source;
        private ScriptScope scope;
        //programa python para leer los archivos .fit
        private  readonly string pathPythonScript = "C:/Users/jeison/Source/Repos/tfgweb/ForeverFit/MeloMetrics/python-fitparse-master/scripts/ForeverFitFitReader.py";
        //urls donde ir a buscar includes y clases para compilar el script
        private readonly string[] searchPaths = { "C:/Users/jeison/Source/Repos/tfgweb/ForeverFit/MeloMetrics/python-fitparse-master/fitparse", "D:/Program Files (x86)/IronPython 2.7/Lib" };

       
    public FitFileManager()
        {
            engine = Python.CreateEngine();
            engine.SetSearchPaths(searchPaths);
            source = engine.CreateScriptSourceFromFile(pathPythonScript);
            scope = engine.CreateScope();
               
        }

        public List<String> readFile(string pathFitFile)
        {
            
            //path del archivo .fit que leera el script
            scope.SetVariable("pathFitFile", pathFitFile);
            dynamic a = source.Execute(scope);

            //result_string = Variabledonde esta el resultado en el archivo python
            string datos = scope.GetVariable("result_string");
            string[] tokens = datos.Split('|');
            
            //confirimado que da lo mismo que en el out del archivo al hacer el split-1 , token size=12*lineas-1, lineas = numero de documentso en bd
            // -1 porque añade un registro vacio alfinal siempre
            List<string> aux = tokens.ToList(); 
            aux.RemoveAt(aux.Count - 1);
            return aux;
        }
    }


}
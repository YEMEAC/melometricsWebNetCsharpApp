using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


using System.Threading.Tasks;



namespace MeloMetrics.Models
{
    public class MeloMetricsDB
    {

        public MongoDatabase Database;

        public MeloMetricsDB()
	    {
            var client = new MongoClient(System.Configuration.ConfigurationManager.ConnectionStrings["MongoConnectionString"].ConnectionString);     
            var server = client.GetServer();

            Database = server.GetDatabase(System.Configuration.ConfigurationManager.ConnectionStrings["MongoConnectionDB"].ConnectionString);
	    }

        public MongoCollection<OneMileWalkTest> OneMileWalkTestCollection
        {
            get
            {
                var oneMileWalkTest = Database.GetCollection<OneMileWalkTest>("OneMileWalkTest");

                return oneMileWalkTest;
            }
        }

        public void insertDocuments(List<string> datos){

            for (int i=0; i< datos.Count; i+=12){
                var document = new BsonDocument
                {
                    {"user_id", 0}, 
                    {datos[i], datos[i+1]}, 
                    {datos[i+2], datos[i+3]}, 
                    {datos[i+4], datos[i+5]}, 
                    {datos[i+6], datos[i+7]}, 
                    {datos[i+8], datos[i+9]}, 
                    {datos[i+10], datos[i+11]}
                };
                 OneMileWalkTestCollection.Insert(document);
            } 
        }
    }
}
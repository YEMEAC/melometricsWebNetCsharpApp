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
    }
}
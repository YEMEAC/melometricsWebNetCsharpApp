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
        //For Best practice, Please put this in the web.config. This is only for demo purpose.
        //====================================================
        public String connectionString = "mongodb://localhost";
        public String DataBaseName = "MeloMetric";
        //====================================================

        public MongoDatabase Database;

        public MeloMetricsDB()
	    {
            var client = new MongoClient(connectionString);
            var server = client.GetServer();

            Database = server.GetDatabase(DataBaseName);
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
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
        public String connectionString = "mongodb://localhost:27017";
        public String DataBaseName = "MeloMetrics";
        //====================================================

        public IMongoDatabase Database;

        public MeloMetricsDB()
	    {
            IMongoClient client = new MongoClient(connectionString);

            Database = client.GetDatabase(DataBaseName);
          
	    }

        public IMongoCollection<BsonDocument> OneMileWalkTestCollection
        {
            get
            {
                var oneMileWalkTest = Database.GetCollection<BsonDocument>("OneMileWalkTest");

                return oneMileWalkTest;
            }
        }
    }
}
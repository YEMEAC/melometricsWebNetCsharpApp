using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using MongoDB.Driver.Builders;



namespace MeloMetrics.Models{
    public class MeloMetricsDB {

        public MongoDatabase Database;

        public MeloMetricsDB(){
            var client = new MongoClient(System.Configuration.ConfigurationManager.ConnectionStrings["MongoConnectionString"].ConnectionString);     
            var server = client.GetServer();

            Database = server.GetDatabase(System.Configuration.ConfigurationManager.ConnectionStrings["MongoConnectionDB"].ConnectionString);
	    }

        //collection completa
        public MongoCollection<OneMileWalkTest> VO2MaxSpeedTestCollection
        {
            get
            {
                var VO2MaxSpeedTest = Database.GetCollection<OneMileWalkTest>("VO2MaxSpeedTest");
                return VO2MaxSpeedTest;
            }
        }

        public MongoCollection<OneMileWalkTest> OneMileWalkTestCollection
        {
            get
            {
                var oneMileWalkTest = Database.GetCollection<OneMileWalkTest>("OneMileWalkTest");
                return oneMileWalkTest;
            }
        }

        public MongoCollection<OneMileWalkTest> OneHalfMileWalkTestCollection
        {
            get
            {
                var OneHalfMileWalkTest = Database.GetCollection<OneMileWalkTest>("OneHalfMileWalkTest");
                return OneHalfMileWalkTest;
            }
        }

      
        public MongoCursor<OneMileWalkTest> getMyOneMileWalkTestCollection(long id_user)
        {
            MongoCursor<OneMileWalkTest> OneMileWalkTest = Database.GetCollection<OneMileWalkTest>("OneMileWalkTest").Find(Query.EQ("id_user", id_user));
            return OneMileWalkTest;
        }

        public void insertDocumentsVO2MaxSpeedTest(List<string> datos, long id_user)
        {

            for (int i=0; i< datos.Count; i+=12){
                var document = createDocument(datos,i, id_user);
                VO2MaxSpeedTestCollection.Insert(document);
            } 
        }

        public void insertOneMileWalkTes(List<string> datos, long id_user)
        {

            for (int i = 0; i < datos.Count; i += 12) {
                var document = createDocument(datos, i, id_user);
                OneMileWalkTestCollection.Insert(document);
            }
        }

        public void insertDocumentsOneHalfMileWalkTest(List<string> datos, long id_user) {

            for (int i = 0; i < datos.Count; i += 12){
                var document = createDocument(datos, i, id_user);
                OneHalfMileWalkTestCollection.Insert(document);
            }
        }

        private BsonDocument createDocument(List<string> datos, int i, long id_user){
            var document = new BsonDocument{
                    {"id_user", id_user}, 
                    {datos[i], datos[i+1]}, 
                    {datos[i+2], datos[i+3]}, 
                    {datos[i+4], datos[i+5]}, 
                    {datos[i+6], datos[i+7]}, 
                    {datos[i+8], datos[i+9]}, 
                    {datos[i+10], datos[i+11]}
                };
            return document;
        }
    }
}
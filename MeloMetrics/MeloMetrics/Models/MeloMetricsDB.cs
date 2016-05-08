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


        public MongoCollection<Activity> ActivityCollection
        {
            get
            {
                var t = Database.GetCollection<Activity>("Activity");
                return t;
            }
        }

        public MongoCollection<Activity> ActivityRecordCollection
        {
            get
            {
                var t = Database.GetCollection<Activity>("ActivityRecord");
                return t;
            }
        }

        public MongoCursor<Activity> getMyActivityCollection(string id_user)
        {
            var t = Database.GetCollection<Activity>("Activity").Find(Query.EQ("id_user", id_user));
            return t;
        }

        public MongoCursor<ActivityRecord> getMyActivitysRecordsCollection(string id_activity)
        {
            var t = Database.GetCollection<ActivityRecord>("ActivityRecord").Find(Query.EQ("id_activity", id_activity));
            return t;
        }

        public void insertActivityAndRecords(List<string> datos, string id_user, string nombre, string timestamp)
        {
            string id_activity = getActivityId(datos[1]);
            var activity = new BsonDocument{
                    {"id_user", id_user},
                    {"id_activity", id_activity},
                    {"nombre", nombre},
                    {"timestamp", timestamp}
            };
            ActivityCollection.Insert(activity);

            for (int i = 0; i < datos.Count; i += 12)
            {
                var record = createDocumentRecord(datos, i, id_activity);
                ActivityRecordCollection.Insert(record);
            }
        }


        /*collection completa
        public MongoCollection<ActivityRecord> VO2MaxSpeedTestCollection
        {
            get
            {
                var VO2MaxSpeedTest = Database.GetCollection<ActivityRecord>("VO2MaxSpeedTest");
                return VO2MaxSpeedTest;
            }
        }

        public MongoCollection<ActivityRecord> OneMileWalkTestCollection
        {
            get
            {
                var oneMileWalkTest = Database.GetCollection<ActivityRecord>("ActivityRecord");
                return oneMileWalkTest;
            }
        }

        public MongoCollection<ActivityRecord> OneHalfMileWalkTestCollection
        {
            get
            {
                var OneHalfMileWalkTest = Database.GetCollection<ActivityRecord>("OneHalfMileWalkTest");
                return OneHalfMileWalkTest;
            }
        }*/

      
        /*collecion completa del usuario
        public MongoCursor<ActivityRecord> getMyOneMileWalkTestCollection(long id_user)
        {
            MongoCursor<ActivityRecord> OneMileWalkTest = Database.GetCollection<ActivityRecord>("ActivityRecord").Find(Query.EQ("id_user", id_user));
            return OneMileWalkTest;
        }

        //activity completa del usuario
        public MongoCursor<ActivityRecord> getMyOneMileWalkTestActivity(long id_user, string id_activity)
        {
            var query = Query.And(
            Query.EQ("id_user", id_user),
            Query.EQ("id_activity", id_activity)
            );

            MongoCursor<ActivityRecord> OneMileWalkTest = Database.GetCollection<ActivityRecord>("ActivityRecord").Find(query);
            return OneMileWalkTest;
        }*/

        /*insert documents en colleciones
        public void insertDocumentsVO2MaxSpeedTest(List<string> datos, long id_user)
        {
            string id_activity = getActivityId(datos[1]);
            for (int i=0; i< datos.Count; i+=12){
                var document = createDocument(datos,i, id_user, id_activity);
                VO2MaxSpeedTestCollection.Insert(document);
            } 
        }

        public void insertDocumentsOneMileWalkTes(List<string> datos, long id_user)
        {
            string id_activity = getActivityId(datos[1]);
            for (int i = 0; i < datos.Count; i += 12) {
                var document = createDocument(datos, i, id_user, id_activity);
                OneMileWalkTestCollection.Insert(document);
            }
        }

        public void insertDocumentsOneHalfMileWalkTest(List<string> datos, long id_user)
        {
            string id_activity = getActivityId(datos[1]);
            for (int i = 0; i < datos.Count; i += 12){
                var document = createDocument(datos, i, id_user, id_activity);
                OneHalfMileWalkTestCollection.Insert(document);
            }
        }*/


        //privates
        private BsonDocument createDocumentRecord(List<string> datos, int i, string id_activity)
        {
            var document = new BsonDocument{
                        {"id_activity", id_activity},
                        {datos[i], datos[i+1]}, 
                        {datos[i+2], datos[i+3]}, 
                        {datos[i+4], datos[i+5]}, 
                        {datos[i+6], datos[i+7]}, 
                        {datos[i+8], datos[i+9]}, 
                        {datos[i+10], datos[i+11]}
                };
            return document;
        }

        private string getActivityId(string timestamp){
            //convert to epoch time uni time
            string t1 = (Math.Ceiling(((DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds))).ToString();
            string t2 = (Math.Ceiling(((DateTime.Parse(timestamp)- new DateTime(1970, 1, 1)).TotalSeconds))).ToString();
            string id_activity = string.Concat(t1,t2);
            return id_activity;
        }

       
    }
}
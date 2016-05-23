using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using MongoDB.Driver.Builders;


//singleton
namespace MeloMetrics.Models{
    public class MeloMetricsDB {

        private MongoDatabase Database { get; set; }
        private static MeloMetricsDB meloMetricsDB;

        private MeloMetricsDB(){
            var client = new MongoClient(System.Configuration.ConfigurationManager.ConnectionStrings["MongoConnectionString"].ConnectionString);     
            var server = client.GetServer();

            Database = server.GetDatabase(System.Configuration.ConfigurationManager.ConnectionStrings["MongoConnectionDB"].ConnectionString);
	    }

        public static MeloMetricsDB getMeloMetricsDB()
        {
            if (meloMetricsDB == null)
            {
                meloMetricsDB = new MeloMetricsDB();
            }
            
            return meloMetricsDB;
        }



        public MongoCursor<Activity> getMyActivityCollectionByNameAsc(string id_user, string searchString)
        {
            
            var t = Database.GetCollection<Activity>("Activity").Find(
                Query.And(
                    Query.EQ("id_user", id_user),
                    Query.Matches("nombre", searchString) 
                  )
                ).SetSortOrder(SortBy.Ascending("nombre"));;
            return t;
        }

        public MongoCursor<Activity> getMyActivityCollectionByNameDesc(string id_user, string searchString)
        {
            var t = Database.GetCollection<Activity>("Activity").Find(
                Query.And(
                    Query.EQ("id_user", id_user),
                    Query.Matches("nombre", searchString)
                  )
                ).SetSortOrder(SortBy.Descending("nombre")); ;
            return t;
        }

        public MongoCursor<Activity> getMyActivityCollectionByDateAsc(string id_user, string searchString)
        {
            var t = Database.GetCollection<Activity>("Activity").Find(
                Query.And(
                    Query.EQ("id_user", id_user),
                    Query.Matches("nombre", searchString)
                   )
                  ).SetSortOrder(SortBy.Ascending("timestamp")); ;
            return t;
        }

        public MongoCursor<Activity> getMyActivityCollectionByDateDesc(string id_user, string searchString)
        {
            var t = Database.GetCollection<Activity>("Activity").Find(
                Query.And(
                    Query.EQ("id_user", id_user),
                    Query.Matches("nombre", searchString)
                  )
                ).SetSortOrder(SortBy.Descending("timestamp")); ;
            return t;
        }
       

       
        //INSERT
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
                var record = createActivityRecordDocument(datos, i, id_activity);
                ActivityRecordCollection.Insert(record);
            }
        }


       
        //PRIVATES
        //revisar xk estos dos deverian ser solo metodos no propieaddes
        //hacer que solo se le pasa el record o el activity y lo meta directamente
        private MongoCollection<Activity> ActivityCollection
        {
            get
            {
                var t = Database.GetCollection<Activity>("Activity");
                return t;
            }
        }

        private MongoCollection<Activity> ActivityRecordCollection
        {
            get
            {
                var t = Database.GetCollection<Activity>("ActivityRecord");
                return t;
            }
        }


        private BsonDocument createActivityRecordDocument(List<string> datos, int i, string id_activity)
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


        //OTHERS
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

    }
}
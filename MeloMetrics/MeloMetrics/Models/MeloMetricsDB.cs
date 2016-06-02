using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using MongoDB.Driver.Builders;
using System.Globalization;


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



        public MongoCursor<Activity> getMyActivityCollectionByNameAsc(int id_user, string searchString)
        {
            
            var t = Database.GetCollection<Activity>("Activity").Find(
                Query.And(
                    Query.EQ("id_user", id_user),
                    Query.Matches("nombre", searchString) 
                  )
                ).SetSortOrder(SortBy.Ascending("nombre"));;
            return t;
        }

        public MongoCursor<Activity> getMyActivityCollectionByNameDesc(int id_user, string searchString)
        {
            var t = Database.GetCollection<Activity>("Activity").Find(
                Query.And(
                    Query.EQ("id_user", id_user),
                    Query.Matches("nombre", searchString)
                  )
                ).SetSortOrder(SortBy.Descending("nombre")); ;
            return t;
        }

        public MongoCursor<Activity> getMyActivityCollectionByDateAsc(int id_user, string searchString)
        {
            var t = Database.GetCollection<Activity>("Activity").Find(
                Query.And(
                    Query.EQ("id_user", id_user),
                    Query.Matches("nombre", searchString)
                   )
                  ).SetSortOrder(SortBy.Ascending("timestamp")); ;
            return t;
        }

        public MongoCursor<Activity> getMyActivityCollectionByDateDesc(int id_user, string searchString)
        {
            var t = Database.GetCollection<Activity>("Activity").Find(
                Query.And(
                    Query.EQ("id_user", id_user),
                    Query.Matches("nombre", searchString)
                  )
                ).SetSortOrder(SortBy.Descending("timestamp")); ;
            return t;
        }


        public MongoCursor<ActivityRecord> getMyActivitysRecordsCollection(string id_activity)
        {
            var t = Database.GetCollection<ActivityRecord>("ActivityRecord").Find(Query.EQ("id_activity", id_activity));
            return t;
        }

       
        //INSERT
        public void insertActivityAndRecords(List<string> datos, string id_user, string nombre, string timestamp)
        {
            
            var activityDoc = new BsonDocument{
                    {"id_user", Int32.Parse(id_user)},
                    {"nombre", nombre},
                    {"timestamp", DateTime.Parse(timestamp,CultureInfo.InvariantCulture.NumberFormat)}
            };

            Database.GetCollection<Activity>("Activity").Insert(activityDoc);

            for (int i = 0; i < datos.Count; i += 12)
            {
                
                var record = createActivityRecordDocument(datos, i, activityDoc.GetElement(0).Value);
                Database.GetCollection<Activity>("ActivityRecord").Insert(record);
            }
        }


       
        private object createActivityRecordDocument(List<string> datos, int i, BsonValue id_activity)
        {
          
            var document = new BsonDocument{
                        {"id_activity", id_activity.ToString()},
                        {datos[i], DateTime.Parse(datos[i+1],CultureInfo.InvariantCulture.NumberFormat)}, //timestamp
                        {datos[i+2], Int32.Parse(datos[i+3])}, //latitud
                        {datos[i+4], Int32.Parse(datos[i+5])}, //longitud
                        {datos[i+6], float.Parse(datos[i + 7], CultureInfo.InvariantCulture.NumberFormat)}, //distancia
                        {datos[i+8], float.Parse(datos[i + 9], CultureInfo.InvariantCulture.NumberFormat)},  //velocidad
                        {datos[i+10], Int32.Parse(datos[i+11])} //hearrate
                };
            return document;
        }

        //OTHERS

      /*  private string getActivityId(string timestamp)
        {
            //convert to epoch time uni time ////no meto aki un array de records por eficiencia y no tener que cargarlo ver diario que explico bien porque diria
            string t1 = (Math.Ceiling(((DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds))).ToString();
            string t2 = (Math.Ceiling(((DateTime.Parse(timestamp) - new DateTime(1970, 1, 1)).TotalSeconds))).ToString();
            string id_activity = string.Concat(t1, t2);
            return id_activity;
        }
        public MongoCursor<Activity> getMyActivityCollection(string id_user)
        {
            var t = Database.GetCollection<Activity>("Activity").Find(Query.EQ("id_user", id_user));
            return t;
        }

       */

    }
}
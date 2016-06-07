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
namespace ForeverFit.Models{
    public class ForeverFitDB {

        private MongoDatabase Database { get; set; }
        private static ForeverFitDB foreverFitDB;

        private ForeverFitDB(){
            var client = new MongoClient(System.Configuration.ConfigurationManager.ConnectionStrings["MongoConnectionString"].ConnectionString);     
            var server = client.GetServer();

            Database = server.GetDatabase(System.Configuration.ConfigurationManager.ConnectionStrings["MongoConnectionDB"].ConnectionString);
	    }

        public static ForeverFitDB getForeverFitDB()
        {
            if (foreverFitDB == null)
            {
                foreverFitDB = new ForeverFitDB();
            }
            
            return foreverFitDB;
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


        public MongoCursor<ActivityRecord> getMyActivitysRecordsCollection(string id_activity)
        {
            var t = Database.GetCollection<ActivityRecord>("ActivityRecord").Find(Query.EQ("id_activity", id_activity));
            return t;
        }

       
        //INSERT
        public void insertActivityAndRecords(List<string> datos, string id_user, string nombre, string timestamp)
        {
            
            var activityDoc = new BsonDocument{
                    {"id_user", id_user},
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

        internal void deleteMyActivityCollectionAndRecors(string id)
        {
            Database.GetCollection("Activity").Remove(Query.EQ("_id", new ObjectId(id)));
            Database.GetCollection("ActivityRecord").Remove(Query.EQ("id_activity", id));
        }
    }
}
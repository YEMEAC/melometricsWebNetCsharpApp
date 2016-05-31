using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MeloMetrics.Models
{
    public class Activity
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public int id_user { get; set; }
        public DateTime timestamp { get; set; }
        public string nombre { get; set; }
        //convinacion timestamp formato epoch momento guardado+ timestamp primer registro
        //no meto aki un array de records por eficiencia y no tener que cargarlo ver diario que explico bien porque diria
    }
}
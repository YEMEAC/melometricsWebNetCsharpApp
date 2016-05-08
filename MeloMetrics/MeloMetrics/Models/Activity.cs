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
        public string id_activity { get; set; } //convinacion timestamp formato epoch momento guardado+ timestamp primer registro

        public string id_user { get; set; }
        public string timestamp { get; set; }
        public string nombre { get; set; }
        List<ActivityRecord> records { get; set; }

    }
}
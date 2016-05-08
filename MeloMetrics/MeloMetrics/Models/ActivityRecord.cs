using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MeloMetrics.Models
{
    public class ActivityRecord
    {
      // [BsonId(IdGenerator = typeof(StringObjectIdGenerator))]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        //public string nombre { get; set; }
        public string timestamp { get; set; }
        public string position_lat { get; set; }
        public string position_long { get; set; }
        public string distance { get; set; }
        public string speed { get; set; }
        public string heart_rate { get; set; }
       
       
    }
}
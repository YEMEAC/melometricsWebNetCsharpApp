using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ForeverFit.Models
{
    public class ActivityRecord
    {
      // [BsonId(IdGenerator = typeof(StringObjectIdGenerator))]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        //public string nombre { get; set; }
        public string id_activity { get; set; }
        public DateTime timestamp { get; set; }
        public int position_lat { get; set; }
        public int position_long { get; set; }
        public float distance { get; set; }
        public float speed { get; set; }
        public int heart_rate { get; set; }
       
       
    }
}
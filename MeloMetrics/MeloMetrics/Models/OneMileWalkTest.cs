using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MeloMetrics.Models
{
    public class OneMileWalkTest
    {
      // [BsonId(IdGenerator = typeof(StringObjectIdGenerator))]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        
   
        public long id_user { get; set; }
        public string id_activity { get; set; } //convinacion timestamp formato epoch momento guardado+ timestamp primer registro
        public string timestamp { get; set; }
        public string position_lat { get; set; }
        public string position_long { get; set; }
        public string distance { get; set; }
        public string speed { get; set; }
        public string heart_rate { get; set; }
       
       
    }
}
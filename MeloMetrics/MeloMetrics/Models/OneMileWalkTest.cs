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

        
   
        public long propietario { get; set; }
        public decimal ritmocardiaco { get; set; }
        public float distancia { get; set; }
        public float velocidad { get; set; }
        public string tiempo { get; set; }
        public string fecha { get; set; }
       
    }
}
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MeloMetrics.Models
{
    public class OneMileWalkTest
    {
        public ObjectId Id { get; set; }

        public int ritmocardiaco { get; set; }
        public decimal distancia { get; set; }
        public int velocidad { get; set; }
        public string tiempo { get; set; }
        public string fecha { get; set; }
       
    }
}
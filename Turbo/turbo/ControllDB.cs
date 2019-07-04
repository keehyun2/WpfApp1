using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Turbo.turbo
{
    public static class ControllDB
    {
        public static MongoClient client = new MongoClient("mongodb://product1:product1@ds147723.mlab.com:47723/product");

        public static IMongoDatabase db = client.GetDatabase("product");

    }
}

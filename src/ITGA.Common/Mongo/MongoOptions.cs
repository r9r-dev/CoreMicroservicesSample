using System;
using System.Collections.Generic;
using System.Security.Authentication;
using System.Text;

namespace ITGA.Common.Mongo
{
    public class MongoOptions
    {
        public string ConnectionString { get;set; }
        public string Database { get; set; }
        public bool Seed { get;set; }
        public string Ssl { get; set; }
    }
}

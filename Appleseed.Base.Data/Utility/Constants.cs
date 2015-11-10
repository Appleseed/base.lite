using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Appleseed.Base.Data.Utility
{
    public static class Constants
    {
        public static readonly string MySql = string.Format(@"MySql");
        public static readonly string SqlServer = string.Format(@"SqlServer");
        public static readonly string MongoDb = string.Format(@"MongoDb");
        public static readonly string Lucene = string.Format(@"Lucene");
        public static readonly string InMemory = string.Format(@"InMemory");
    }
}

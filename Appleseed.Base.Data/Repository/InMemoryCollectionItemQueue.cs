using System.Collections.Generic;

namespace Appleseed.Base.Data.Repository
{
    public static class InMemoryCollectionItemQueue
    {
        public class CollectionItemQueue
        {
            public int ItemId { get; set; }
            public string ItemTitle { get; set; }
            public string ItemUrl { get; set; }
            public string ItemDescription { get; set; }
            public string ItemTags { get; set; }
            public bool ItemProcessed { get; set; }
        }

        public static List<CollectionItemQueue> InMemoryCollectionItems;

        static InMemoryCollectionItemQueue()
        {
            InMemoryCollectionItems = new List<CollectionItemQueue>();
            InMemoryCollectionItems.Add(new CollectionItemQueue
            {
                ItemProcessed = false,
                ItemDescription = "A really cool nosql db which updates its clients in realtime.",
                ItemId = 1,
                ItemTags = "database,nosql, open source",
                ItemUrl = "http://rethinkdb.com/",
                ItemTitle = "RethinkDB"
            });
            InMemoryCollectionItems.Add(new CollectionItemQueue
            {
                ItemProcessed = false,
                ItemDescription = "Another nosql database that stores data as JSON",
                ItemId = 2,
                ItemTags = "database, nosql, open source",
                ItemUrl = "http://www.mongodb.com",
                ItemTitle = "MongoDB"
            });
            InMemoryCollectionItems.Add(new CollectionItemQueue
            {
                ItemProcessed = false,
                ItemDescription = "Comprehensive tutorial on SQL Data Read",
                ItemId = 3,
                ItemTags = ".net,sql,ado",
                ItemUrl = "https://msdn.microsoft.com/en-us/library/system.data.sqlclient.sqldatareader.read%28v=vs.110%29.aspx",
                ItemTitle = "SQL Datareader Read"
            });
        }
    }
}

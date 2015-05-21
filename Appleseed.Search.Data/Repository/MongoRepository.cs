using GA.Data.Clients;
using NLog;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace GA.Data.Repository
{
    public class MongoRepository : IMongoRepository
    {
        public IMongoClient MongoClient { get; set; }
        public MongoDatabase Db { get; set; }

        public Logger Log { get; set; }

        public MongoRepository(Logger logger)
        {
			Log = logger;
        }

        public MongoRepository(IMongoClient mongoClient, string databaseName)
            : this()
        {
            MongoClient = mongoClient;
            Db = MongoClient.GetDatabase(databaseName);
        }

        public IEnumerable<TItem> SelectAll<TItem>() where TItem : class, Model.IHasStringId
        {
            return Db.GetCollection<TItem>(typeof(TItem).Name)
                .AsQueryable<TItem>()
                .AsEnumerable<TItem>();
        }

        public TItem SelectById<TItem>(string id) where TItem : class, Model.IHasStringId
        {
            return Db.GetCollection<TItem>(typeof(TItem).Name).FindOneById(id);
        }

        public TItem Insert<TItem>(TItem item) where TItem : class, Model.IHasStringId
        {
            Db.GetCollection<TItem>(typeof(TItem).Name).Insert(item);
            return SelectById<TItem>(item.Id);
        }

        public TItem Update<TItem>(TItem item) where TItem : class, Model.IHasStringId
        {
            return Insert<TItem>(item);
        }

        public void Delete<TItem>(string id) where TItem : class, Model.IHasStringId
        {
            Db.GetCollection<TItem>(typeof(TItem).Name).Remove(Query<TItem>.EQ(x => x.Id, id));
        }

        public bool Exists<TItem>(string id) where TItem : class, Model.IHasStringId
        {
            return Db.GetCollection<TItem>(typeof(TItem).Name).FindOneById(id) != null;
        }

        public bool IsServerAvailable()
        {
            try
            {
                Db.Server.Connect();
                Db.Server.Disconnect();
                return true;
            }
            catch (Exception exception)
            {
                Log.Error(exception);
                return false;
            }
        }
    }
}

using System;
using System.Collections.Generic;

using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

using GA.Data.Model;
using NLog;

namespace GA.Data.Repository
{
    public class DatabaseRepository : IDatabaseRepository
    {
        public Logger Log { get; set; }
        public DbContext Db;

		public DatabaseRepository(Logger logger)
		{
			Log = logger;
		}

        public DatabaseRepository(DbContext db)
        {
            Db = db;
        }

        public IEnumerable<TItem> SelectAll<TItem>() where TItem : class, IHasLongId
        {
            return Db.Set<TItem>().ToList<TItem>();
        }

        public TItem SelectById<TItem>(long id) where TItem : class, IHasLongId
        {
            return Db.Set<TItem>().Find(id);

        }

        public TItem Insert<TItem>(TItem item) where TItem : class, IHasLongId
        {
            return Db.Set<TItem>().Add(item);
        }

        public TItem Update<TItem>(TItem item) where TItem : class, IHasLongId
        {
            Db.Set<TItem>().Attach(item);
            Db.Entry(item).State = EntityState.Modified;
            return Db.Entry(item).Entity;
        }

        public void Delete<TItem>(long id) where TItem : class, IHasLongId
        {
            TItem existing = Db.Set<TItem>().Find(id);
            Db.Set<TItem>().Remove(existing);
        }

        public void Save()
        {
            Db.SaveChanges();
        }

        public bool Exists<TItem>(long id) where TItem : class, IHasLongId
        {
            return Db.Set<TItem>().Find(id) != null;
        }

        public void SetState<TItem>(TItem item, EntityState entityState) where TItem : class
        {
            Db.Entry(item).State = entityState;
        }

        public void Dispose()
        {
            Db.Dispose();
        }

        public bool IsServerAvailable()
        {
            try 
            {
                Db.Database.Connection.Open();
                return true;
            }
            catch(Exception exception) 
            {
                Log.Error(exception);
                return false;
            }
        }
    }
}

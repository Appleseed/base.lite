using System.Collections.Generic;
using GA.Data.Model;

namespace GA.Data.Repository
{
    public interface IRepository
    {
        IEnumerable<TItem> SelectAll<BaseCollectionItem>();
        TItem SelectById<TItem>(TId id)
        TItem Insert<TItem>(TItem item)
        TItem Update<TItem>(TItem item)
        void Delete<TItem>(TId id)
        bool Exists<TItem>(TId id)
        bool IsServerAvailable();
    }
}

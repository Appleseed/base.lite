using GA.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace GA.Data.Repository
{
    public interface IElasticsearchRepository : IStringIdRepository
    {
        IEnumerable<TItem> SelectAll<TItem, TKey>(Expression<Func<TItem, TKey>> orderBy, bool ascending = true, int pageIndex = 0, int rowCount = 10) where TItem : class, IHasStringId;
        IEnumerable<TItem> Search<TItem, TKey>(string query, Expression<Func<TItem, TKey>> orderBy, bool ascending = true, int pageIndex = 0, int rowCount = 10) where TItem : class, IHasStringId;
    }
}

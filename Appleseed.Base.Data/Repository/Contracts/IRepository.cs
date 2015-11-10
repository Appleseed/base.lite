using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Appleseed.Base.Data.Model;

namespace Appleseed.Base.Data.Repository.Contracts
{
    public interface IRepository
    {
        void Insert(BaseCollectionItem baseCollectionItem);

        void Update(BaseCollectionItem baseCollectionItem);

        void DeleteProcessedItems();

        List<BaseCollectionItem> GetAllFromQueue();

        BaseCollectionItem GetById(Guid id);

        IQueryable<BaseCollectionItem> GetUnProcessedBaseCollectionItems();
    }
}

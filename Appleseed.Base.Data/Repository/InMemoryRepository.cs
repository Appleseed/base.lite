using System;
using System.Collections.Generic;
using System.Linq;
using Appleseed.Base.Data.Model;
using Appleseed.Base.Data.Repository.Contracts;

namespace Appleseed.Base.Data.Repository
{
    public class InMemoryRepository : IInMemoryRepository
    {
        public static List<BaseCollectionItem> InMemoryBaseCollectionItems;

        static InMemoryRepository()
        {
            InMemoryBaseCollectionItems = new List<BaseCollectionItem>();
        }

        public InMemoryRepository()
        {
            InMemoryBaseCollectionItems = new List<BaseCollectionItem>();
        }
        public void Insert(BaseCollectionItem baseCollectionItem)
        {
            baseCollectionItem.Id = Guid.NewGuid();
            baseCollectionItem.CreatedDate = DateTime.Now;
            InMemoryBaseCollectionItems.Add(baseCollectionItem);
        }

        public void Update(BaseCollectionItem baseCollectionItem)
        {
            var itemsToUpdate = InMemoryBaseCollectionItems.Where(x => x.Id == baseCollectionItem.Id).ToList();
            foreach (var item in itemsToUpdate)
            {
                item.ItemProcessed = baseCollectionItem.ItemProcessed;
            }
        }

        public void DeleteProcessedItems()
        {
            InMemoryBaseCollectionItems.RemoveAll(x=>x.ItemProcessed);
        }

        public List<BaseCollectionItem> GetAllFromQueue()
        {
            var itemCollection = new List<BaseCollectionItem>();
            foreach (var baseCollectionItem in InMemoryCollectionItemQueue.InMemoryCollectionItems)
            {
                var baseCollectionItemData = new BaseCollectionItemData
                {
                    ItemID = baseCollectionItem.ItemId,
                    ItemTitle = baseCollectionItem.ItemTitle,
                    ItemUrl = baseCollectionItem.ItemUrl,
                    ItemContent_Image = "",
                    ItemContent_Raw = "",
                    ItemDescription = baseCollectionItem.ItemDescription,
                    ItemTags = baseCollectionItem.ItemTags,
                    ItemProcessedDate = DateTime.Today
                };

                itemCollection.Add(new BaseCollectionItem
                {
                    Id = Guid.NewGuid(),
                    Data = baseCollectionItemData
                });
            }

            return itemCollection;
        }

        public BaseCollectionItem GetById(Guid id)
        {
            throw new NotImplementedException();
        }

        public IQueryable<BaseCollectionItem> GetUnProcessedBaseCollectionItems()
        {
            var items = InMemoryBaseCollectionItems.Where(x => x.ItemProcessed == false);
            return items.AsQueryable();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Appleseed.Base.Data.Model;
using Appleseed.Base.Data.Repository.Contracts;
using Appleseed.Base.Data.Service;
using Appleseed.Base.Data.Utility;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search;
using Version = Lucene.Net.Util.Version;

namespace Appleseed.Base.Data.Repository
{
    public class LuceneRepository:ILuceneRepository
    {
        public void Insert(BaseCollectionItem baseCollectionItem)
        {
            baseCollectionItem.Id = Guid.NewGuid();
            baseCollectionItem.CreatedDate = DateTime.Now;
            var analyzer = new StandardAnalyzer(Version.LUCENE_30);
            using (var writer = new IndexWriter(LuceneService.Directory, analyzer, IndexWriter.MaxFieldLength.UNLIMITED))
            {
                // add data to lucene search index (replaces older entry if any)

                AddToLuceneIndex(baseCollectionItem, writer);
                // close handles
                analyzer.Close();
                writer.Dispose();
            }
        }

        public void Update(BaseCollectionItem baseCollectionItem)
        {
            var query = baseCollectionItem.Id.ToString();
            var items = SearchItems(query, "Id");
            foreach (var item in items)
            {
                ClearLuceneIndexRecord(item.Id);
                Insert(baseCollectionItem);
            }
        }

        public void DeleteProcessedItems()
        {
            ////TODO :  find all the processed items and delete them one by one or all at once
            
            var query = "true";
            var items = SearchItems(query, "ItemProcessed");

            foreach (var item in items)
            {
                ClearLuceneIndexRecord(item.Id);
            }
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
            var query = "False";
            var items = SearchItems(query, "ItemProcessed");
            var allItems = GetAllIndexRecords();

            return items.AsQueryable();
        }

        private static void AddToLuceneIndex(BaseCollectionItem baseCollectionItem, IndexWriter writer)
        {
            // remove older index entry
            var searchQuery = new TermQuery(new Term("Id", baseCollectionItem.Id.ToString()));
            writer.DeleteDocuments(searchQuery);

            // add new index entry
            var doc = new Document();

            // add lucene fields mapped to db fields
            doc.Add(new Field("Id", baseCollectionItem.Id.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
            doc.Add(new Field("TableId", baseCollectionItem.TableId.ToString(), Field.Store.YES, Field.Index.ANALYZED));
            doc.Add(new Field("Data", baseCollectionItem.Data.XmlSerializeToString(), Field.Store.YES, Field.Index.ANALYZED));
            doc.Add(new Field("ItemProcessed", baseCollectionItem.ItemProcessed.ToString(), Field.Store.YES, Field.Index.ANALYZED));
            doc.Add(new Field("CreatedDate", baseCollectionItem.CreatedDate.ToString(CultureInfo.InvariantCulture), Field.Store.YES, Field.Index.ANALYZED));

            // add entry to index
            writer.AddDocument(doc);
        }

        private static void ClearLuceneIndexRecord(Guid recordId)
        {
            // init lucene
            var analyzer = new StandardAnalyzer(Version.LUCENE_30);
            using (var writer = new IndexWriter(LuceneService.Directory, analyzer, IndexWriter.MaxFieldLength.UNLIMITED))
            {
                // remove older index entry
                var searchQuery = new TermQuery(new Term("Id", recordId.ToString()));
                writer.DeleteDocuments(searchQuery);

                // close handles
                analyzer.Close();
                writer.Dispose();
            }
        }

        private static IEnumerable<BaseCollectionItem> SearchLucene(string searchQuery, string searchField = "")
        {
            // validation
            if (string.IsNullOrEmpty(searchQuery.Replace("*", "").Replace("?", ""))) return new List<BaseCollectionItem>();

            // set up lucene searcher
            using (var searcher = new IndexSearcher(LuceneService.Directory, false))
            {
                var hits_limit = 1000;
                var analyzer = new StandardAnalyzer(Version.LUCENE_30);

                // search by single field
                if (!string.IsNullOrEmpty(searchField))
                {
                    var parser = new QueryParser(Version.LUCENE_30, searchField, analyzer);
                    var query = ParseQuery(searchQuery, parser);
                    var hits = searcher.Search(query, hits_limit).ScoreDocs;
                    var results = MapLuceneToDataList(hits, searcher);
                    analyzer.Close();
                    searcher.Dispose();
                    return results;
                }
                // search by multiple fields (ordered by RELEVANCE)
                else
                {
                    var parser = new MultiFieldQueryParser
                        (Version.LUCENE_30, new[] { "Id", "TableId", "Data","ItemProcessed", "CreatedDate" }, analyzer);
                    var query = ParseQuery(searchQuery, parser);
                    var hits = searcher.Search(query, null, hits_limit, Sort.RELEVANCE).ScoreDocs;
                    var results = MapLuceneToDataList(hits, searcher);
                    analyzer.Close();
                    searcher.Dispose();
                    return results;
                }
            }
        }
        private static Query ParseQuery(string searchQuery, QueryParser parser)
        {
            Query query;
            try
            {
                query = parser.Parse(searchQuery.Trim());
            }
            catch (ParseException)
            {
                query = parser.Parse(QueryParser.Escape(searchQuery.Trim()));
            }
            return query;
        }

        private static BaseCollectionItem MapLuceneDocumentToData(Document doc)
        {
            var docData = doc.Get("Data");

            return new BaseCollectionItem
            {
                Id = Guid.Parse(doc.Get("Id")),
                TableId = Convert.ToInt32(doc.Get("TableId")),
                Data = doc.Get("Data").XmlDeserializeFromString<BaseCollectionItemData>(),
                ItemProcessed = Convert.ToBoolean(doc.Get("ItemProcessed")),
                CreatedDate = Convert.ToDateTime(doc.Get("CreatedDate"))
            };
        }

        private static IEnumerable<BaseCollectionItem> MapLuceneToDataList(IEnumerable<Document> hits)
        {
            return hits.Select(MapLuceneDocumentToData).ToList();
        }
        private static IEnumerable<BaseCollectionItem> MapLuceneToDataList(IEnumerable<ScoreDoc> hits,
            IndexSearcher searcher)
        {
            return hits.Select(hit => MapLuceneDocumentToData(searcher.Doc(hit.Doc))).ToList();
        }


        private static IEnumerable<BaseCollectionItem> GetAllIndexRecords()
        {
            // validate search index
            if (!System.IO.Directory.EnumerateFiles(LuceneService.LuceneDir).Any()) return new List<BaseCollectionItem>();

            // set up lucene searcher
            var searcher = new IndexSearcher(LuceneService.Directory, false);
            var reader = IndexReader.Open(LuceneService.Directory, false);
            var docs = new List<Document>();
            var term = reader.TermDocs();
            while (term.Next()) docs.Add(searcher.Doc(term.Doc));
            reader.Dispose();
            searcher.Dispose();
            return MapLuceneToDataList(docs);
        }

        public static IEnumerable<BaseCollectionItem> SearchItems(string input, string fieldName = "")
        {
            if (string.IsNullOrEmpty(input)) return new List<BaseCollectionItem>();

            var terms = input.Trim().Replace("-", " ").Split(' ')
                .Where(x => !string.IsNullOrEmpty(x)).Select(x => x.Trim() + "*");
            input = string.Join(" ", terms);

            return SearchLucene(input, fieldName);
        }
    }
}

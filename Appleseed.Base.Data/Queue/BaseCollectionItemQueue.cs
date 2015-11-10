using System;
using System.Collections.Generic;
using System.Linq;
using Appleseed.Base.Data.Model;
using Appleseed.Base.Data.Repository.Contracts;
using MySql.Data.MySqlClient;
using NLog;

namespace Appleseed.Base.Data.Queue
{
    /// <summary>
    /// Collection item queue. Gets / Removes items from the Collection Item queue in MySQL 
    /// </summary>
    [Serializable]
    public class BaseCollectionItemQueue : IBaseMessageQueue<BaseCollectionItem>
    {
        /// <summary>
        /// Gets the log.
        /// </summary>
        /// <value>The log.</value>
        public Logger Log
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the connection.
        /// </summary>
        /// <value>The connection.</value>
        public string ConnectionString
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets or sets the client.
        /// </summary>
        /// <value>The client.</value>
        protected MySqlConnection Client
        {
            get;
            set;
        }

        private IRepository repository;

        /// <summary>
        /// Gets the name of the database.
        /// </summary>
        /// <value>The name of the database.</value>
        public string DatabaseName
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the name of the table.
        /// </summary>
        /// <value>The name of the table.</value>
        public string TableName
        {
            get;
            private set;
        }


        /// <summary>
        /// Gets the name of the queue.
        /// </summary>
        /// <value>The name of the queue.</value>
        public string QueueName
        {
            get;
            private set;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseCollectionItemQueue"/> class.
        /// </summary>
        /// <param name="repository">Incoming connection string.</param>
        /// <param name="logger">Logger.</param>
        /// <param name="incomingTableName">Incoming table name.</param>
        /// <param name="incomingQueueName">Incoming queue name.</param>
        public BaseCollectionItemQueue(IRepository repository, Logger logger, string incomingTableName = null, string incomingQueueName = null)
        {
            this.Log = logger;

            this.repository = repository;

            ////this.TableName = !String.IsNullOrWhiteSpace(incomingTableName) ? incomingTableName : "BaseCollectionItem"; //TODO: need to make this so that it can be used in any table
            ////this.QueueName = !String.IsNullOrWhiteSpace(incomingQueueName) ? incomingQueueName : "Aggregate";//TODO need to be able to define which queue 
        }

        /// <summary>
        /// Gets full item queue of unprocessed items 
        /// </summary>
        /// <returns>The item queue.</returns>
        public List<BaseCollectionItem> GetItemQueue()
        {
            Client.Open();
            Log.Info("Connected to the database");

            List<BaseCollectionItem> itemCollection = new List<BaseCollectionItem>();
            
            Log.Info("Retreiving Items from Queue");

            Client.Close();
            return itemCollection;
        }

        //DONE: implement method to insert individual queue items 
        public void Enqueue(BaseCollectionItem item)
        {
            item.Data.ItemQueue = this.QueueName;
            item.Data.ItemCreatedDate = DateTime.Now;
            item.Data.ItemProcessed = false;

            item.ItemProcessed = false;

            this.repository.Insert(item);
        }

        //DONE: implement method to insert batch queue items 
        public void Enqueue(List<BaseCollectionItem> items)
        {
            ////var db = CollectionDatabase.Init(Client, 2);
            foreach (BaseCollectionItem item in items)
            {
                this.Enqueue(item);
            }
        }

        //TODO: implement method to take one of the queue 
        public BaseCollectionItem Dequeue(BaseCollectionItem item)
        {
            ///var db = CollectionDatabase.Init(Client, 2);
            ////BaseCollectionItem item = Peek(); //refactored to use Peek
            if (item != null)
            {
                item.ItemProcessed = true;
                this.repository.Update(item);
            }

            return item;
        }

        //TODO: implement method to peek at the next one in the queue
        public BaseCollectionItem Peek()
        {
            //// var db = CollectionDatabase.Init(Client, 2);
            var result = GetEnumerator();
            result.MoveNext();
            BaseCollectionItem item = result.Current;
            return item;
        }

        
        //TODO: implement method to clean out the queue
        public void Clear()
        {
            this.repository.DeleteProcessedItems();
        }

        public IEnumerator<BaseCollectionItem> GetEnumerator()
        {
            IEnumerable<BaseCollectionItem> result = this.repository.GetUnProcessedBaseCollectionItems();
            var copiedResult = new List<BaseCollectionItem>();
            copiedResult.AddRange(result);
            return copiedResult.GetEnumerator();
        }
    }
}


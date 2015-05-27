using System;
using NLog;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using MySql.Data;
using MySql.Data.MySqlClient;

using Dapper;
using Dapper.Contrib.Extensions;

using Appleseed.Base.Data;
using Appleseed.Base.Data.Model;

namespace Appleseed.Base.Data
{


	public class CollectionDatabase : Database<CollectionDatabase>
	{
		public Table<BaseCollectionItem> CollectionItems { get; set;  } 
		public CollectionDatabase() {
			//TODO: need to make this so that the collectionItems can be stored in any table with a different name.
			this.CollectionItems = new Table<BaseCollectionItem> (this, "BaseItemCollection");
		}
	}

	/// <summary>
	/// Collection item queue. Gets / Removes items from the Collection Item queue in MySQL 
	/// </summary>
	public class BaseCollectionItemQueue : IBaseMessageQueue<BaseCollectionItem>
	{
		/// <summary>
		/// Gets the log.
		/// </summary>
		/// <value>The log.</value>
		public Logger Log {
			get;
			private set;
		}

		/// <summary>
		/// Gets the connection.
		/// </summary>
		/// <value>The connection.</value>
		public string ConnectionString {
			get;
			private set;
		}

		/// <summary>
		/// Gets or sets the client.
		/// </summary>
		/// <value>The client.</value>
		protected MySqlConnection Client {
			get;
			set;
		}

		/// <summary>
		/// Gets the name of the database.
		/// </summary>
		/// <value>The name of the database.</value>
		public string DatabaseName {
			get;
			private set;
		}

		/// <summary>
		/// Gets the name of the table.
		/// </summary>
		/// <value>The name of the table.</value>
		public string TableName {
			get;
			private set;
		}


		/// <summary>
		/// Gets the name of the queue.
		/// </summary>
		/// <value>The name of the queue.</value>
		public string QueueName {
			get;
			private set;
		}
			

		public Database<CollectionDatabase> Data {
			get { 
				return CollectionDatabase.Init (Client, 2);
			}
			private set {

			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Appleseed.Base.Data.BaseCollectionItemQueue"/> class.
		/// </summary>
		/// <param name="incomingConnectionString">Incoming connection string.</param>
		/// <param name="logger">Logger.</param>
		/// <param name="incomingTableName">Incoming table name.</param>
		/// <param name="incomingQueueName">Incoming queue name.</param>
		public BaseCollectionItemQueue (string incomingConnectionString, Logger logger, string incomingTableName = null, string incomingQueueName = null )
		{ 
			this.Log = logger;
			this.ConnectionString = incomingConnectionString;
			this.Client = new MySqlConnection (incomingConnectionString);
			this.Data = CollectionDatabase.Init(Client, 2);
			this.TableName = !String.IsNullOrWhiteSpace(incomingTableName)? incomingTableName :"BaseCollectionItem"; //TODO: need to make this so that it can be used in any table
			this.QueueName = !String.IsNullOrWhiteSpace(incomingQueueName)? incomingQueueName :"Aggregate";//TODO need to be able to define which queue 
		}

		/// <summary>
		/// Gets full item queue of unprocessed items 
		/// </summary>
		/// <returns>The item queue.</returns>
		public List<BaseCollectionItem> GetItemQueue (){
			
			Client.Open ();
			var db = CollectionDatabase.Init (Client, 2);
			Log.Info ("Connected to the database");

			//var result = db.BaseCollectionItems.All(); //Would work if the table name was "BaseCollectionItem"
			List<BaseCollectionItem> itemCollection = 
				db.Query<BaseCollectionItem>(@"SELECT * FROM "+this.TableName).AsList();
			Log.Info ("Retreiving Items from Queue");	

			Client.Close ();
			return itemCollection;

		}

		//DONE: implement method to insert individual queue items 
		public void Enqueue(BaseCollectionItem item){
			var db = CollectionDatabase.Init (Client, 2);
			//item.ItemQueue = !String.IsNullOrWhiteSpace(item.ItemQueue) ? item.ItemQueue : this.QueueName;
			item.ItemQueue = this.QueueName;
			item.ItemCreatedDate = DateTime.Now;
			item.ItemProcessed = false;
			long? newItemID = db.CollectionItems.Insert(item);
		}

		//DONE: implement method to insert batch queue items 
		public void Enqueue(List<BaseCollectionItem> items){	
			var db = CollectionDatabase.Init (Client, 2);
			foreach (BaseCollectionItem item in items) {
				this.Enqueue (item);
			}
		}

		//TODO: implement method to take one of the queue 
		public BaseCollectionItem Dequeue(){
			var db = CollectionDatabase.Init (Client, 2);
			BaseCollectionItem item = Peek (); //refactored to use Peek
			item.ItemProcessed = true;
			db.CollectionItems.InsertOrUpdate (item);
			return item;
		}

		//TODO: implement method to peek at the next one in the queue
		public BaseCollectionItem Peek(){
			var db = CollectionDatabase.Init (Client, 2);
			var result = GetEnumerator();
			result.MoveNext ();
			BaseCollectionItem item = result.Current;
			return item;
		}

		/// <summary>
		/// Gets the select builder.
		/// </summary>
		/// <returns>The select builder.</returns>
		private SqlBuilder.Template GetSelectBuilder ()
		{
			var builder = new SqlBuilder ();
			// /**select**/  -- has to be low case
			var selectQueueItems = builder.AddTemplate ("SELECT /**select**/ FROM " + this.TableName + " /**where**/ ");
			builder.Select ("*");
			builder.Where ("ItemProcessed = @Processed", new {
				Processed = false
			});
			builder.Where ("ItemQueue = @Queue", new {
				Queue = this.QueueName
			});
			return selectQueueItems;
		}

		//TODO: implement method to clean out the queue
		public void Clear(){
			var db = CollectionDatabase.Init (Client, 2);

			var builder = new SqlBuilder ();
			// /**select**/  -- has to be low case
			var deleteQueueItems = builder.AddTemplate ("DELETE FROM " + this.TableName + " /**where**/ ");
			builder.Where ("ItemProcessed = @Processed", new {
				Processed = true
			});
			builder.Where ("ItemQueue = @Queue", new {
				Queue = this.QueueName
			});

			db.Execute (deleteQueueItems.RawSql, deleteQueueItems.Parameters);
		}

		public IEnumerator<BaseCollectionItem> GetEnumerator(){
			var db = CollectionDatabase.Init (Client, 2);
			IEnumerable<BaseCollectionItem> result;

			// Old method using straight SQL now replaced with SQL Builder 
			//result= db.Query<BaseCollectionItem> ("@SELECT * FROM "+this.TableName+" WHERE ItemProcessed=false").AsList ();

			var selectQueueItems = GetSelectBuilder (); //refactored to a method 
			result = db.Query<BaseCollectionItem>(selectQueueItems.RawSql, selectQueueItems.Parameters);

			return result.GetEnumerator();
		}



	}
}


using System;
using NLog;
using System.Collections.Generic;
using MongoDB.Driver;
using System.Linq;

using System.Threading.Tasks;

namespace GA.Data
{
	/// <summary>
	/// Collection item repository. Gets / adds information to a MongoDB Collection
	/// </summary>
	public class CollectionItemRepository
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
		private MongoClient Client {
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
		/// Initializes a new instance of the <see cref="GA.Data.CollectionItemRepository"/> class.
		/// </summary>
		/// <param name="incomingConnectionString">Incoming connection string.</param>
		/// <param name="logger">Logger.</param>
		public CollectionItemRepository (string incomingConnectionString, Logger logger)
		{ 
			this.Log = logger;
			this.ConnectionString = incomingConnectionString;
			this.DatabaseName = "test"; //TODO: change from default database name to configurable 
			this.Client = new MongoClient (); // TODO: need to be incomingConnectionString later

		}

		/// <summary>
		/// Inserts the many.
		/// </summary>
		/// <param name="itemQueueCollection">Item queue collection.</param>
		public void UpsertMany (List<CollectionItem> itemQueueCollection)
		{
			Log.Info ("Inserting collection items from the Queue");
			var Database = Client.GetDatabase (this.DatabaseName);
			var Collection = Database.GetCollection<CollectionItem> ("CollectionItems");

			Log.Info ("Connected to MongoDB ");

			Log.Info ("Starting Async Context");
			// This is needed to run asynchronous methods in command line 
				
				//TODO: update items in the repository 
				foreach(CollectionItem item in itemQueueCollection){
					Task.Run (async () =>  {
						// Find document if it exists in the DB 
						var page = await Collection.Find(x => x.ItemUrl == item.ItemUrl).FirstOrDefaultAsync();
						// If it doesn't exists ,add it 
						if(page == null ){
							await Collection.InsertOneAsync(item);
						} else {
								
							// If it exists,  update / replace the page and update / merge the tags
							page.ItemContentCache = item.ItemContentCache;
							page.ItemContentImage = item.ItemContentImage;
							page.ItemTitle = item.ItemTitle;
							page.ItemDescription = item.ItemDescription;
							page.ItemProcessedDate = item.ItemProcessedDate;
							page.ItemTags = page.ItemTags.Concat(item.ItemTags).Distinct().ToList();
							var result = await Collection.ReplaceOneAsync(x => x.ItemUrl == page.ItemUrl, page);
						}
					}).Wait ();
				}
					

				//DONE: add all of the new ones into the DB at once -- but this adds them regardless of whether they are there
				//await Collection.InsertManyAsync (itemQueueCollection, null);
		
				Log.Info ("Added/updated Multiple pages into Mongo");
			
		}

		/// <summary>
		/// Gets the items.
		/// </summary>
		/// <returns>The items.</returns>
		public List<CollectionItem> GetItems(){
			//TODO: implement method to get batch repository items 
			// gets all the repository items in the repo 

			Log.Info ("Retreiving Collection Items .. ");
			var Database = Client.GetDatabase (this.DatabaseName);
			var Collection = Database.GetCollection<CollectionItem> ("CollectionItems");

			List<CollectionItem> returnedItems = new List<CollectionItem>{};

			//var items = null;
			Task.Run (async () =>  {
				var items = await Collection.Find (x => x.ItemTitle != "").ToListAsync ();
			

				foreach (CollectionItem item in items) {
					returnedItems.Add (item);
				}

			}).Wait ();


			return returnedItems;
		}

		//TODO: implement method to insert batch repository items 
			// inserts a bunch of items at the same time 

		//TODO: implement method to get individual repository items
			// not needed right now - will be able to get individual view 

		//TODO: implement method to insert individual repository items 
			// not needed right now - will be able to add items incrementally 

	}
}


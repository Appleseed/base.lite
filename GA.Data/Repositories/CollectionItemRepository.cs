using System;
using NLog;
using System.Collections.Generic;
using MongoDB.Driver;

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
			this.DatabaseName = "test"; //default database name 
			this.Client = new MongoClient (incomingConnectionString);

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


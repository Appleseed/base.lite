using System;
using NLog;
using System.Collections.Generic;
using MySql.Data;
using MySql.Data.MySqlClient;
using GA.Data;

namespace GA.Data
{
	/// <summary>
	/// Collection item queue. Gets / Removes items from the Collection Item queue in MySQL
	/// </summary>
	public class CollectionItemQueue
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
		/// Initializes a new instance of the <see cref="GA.Data.CollectionItemQueue"/> class.
		/// </summary>
		/// <param name="incomingConnectionString">Incoming connection string.</param>
		/// <param name="logger">Logger.</param>
		public CollectionItemQueue (string incomingConnectionString, Logger logger)
		{ 
			this.Log = logger;
			this.ConnectionString = incomingConnectionString;
			this.Client = new MySqlConnection (incomingConnectionString);
		}

		/// <summary>
		/// Gets the item queue.
		/// </summary>
		/// <returns>The item queue.</returns>
		public List<CollectionItem> GetItemQueue (){
			//DONE: implement method to get batch repository items 
			// gets unprocessed items 

			Client.Open ();
			Log.Info ("Connected to the database");

			//DONE: Select data from table in MYSQL Successfully and get it back in .NET
			//TODO: filter if processed or not?
			MySqlCommand myCommand = new MySqlCommand ("SELECT * FROM collectionitemqueue", this.Client);
			MySqlDataReader myReader = myCommand.ExecuteReader ();

			Log.Info ("Retreiving Items from Queue");
			//DONE: enumerate through data , create collection to return ;

			List<CollectionItem> itemCollection = new List<CollectionItem>();

			if (myReader.HasRows) {
				Log.Info ("This Collection has Items");
				while (myReader.Read ()) {
					Log.Info (myReader.GetString ("ItemTitle") + ":" + myReader.GetString ("ItemUrl"));
					//DONE: convert list of tags separated by commas into a List of strings
					List<string> itemTags = new List<string>(myReader.GetString ("ItemTags").Split(','));

					itemCollection.Add (new CollectionItem {
						ItemID = myReader.GetInt32("ItemID"),
						ItemTitle = myReader.GetString ("ItemTitle"),
						ItemUrl = myReader.GetString ("ItemUrl"),
						ItemContentImage = "",
						ItemContentCache = "",
						ItemDescription = myReader.GetString ("ItemDescription"),
						ItemTags = itemTags,
						ItemProcessedDate	= DateTime.Today,
					}
					);
				}
			}
			// close out stuff 
			myReader.Close ();
			Client.Close ();
			return itemCollection;
		}

		//TODO: implement method to insert individual repository items 
			// adds one record - will probably do this in gridview 

		//TODO: implement method to insert batch repository items 
			// not needed - data will be added manually 

		//TODO: implement method to get individual repository items
			// not needed - we'll be batch processing


	}
}


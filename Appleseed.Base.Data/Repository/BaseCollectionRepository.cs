using System;
using System.Collections.Generic;
using MySql.Data;
using MySql.Data.MySqlClient;

using Appleseed.Base.Data.Repository;
using NLog;


using Appleseed.Base.Data.Model; 

namespace Appleseed.Base.Data.Repository
{
	/// <summary>
	/// Base collection repository. This is a generic repository of collection Items - the main contract are
	/// lists of "BaseCollectionItem" 
	/// </summary>
	public class BaseCollectionItemRepository 
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
		/// Gets the name of the collection. 
		/// </summary>
		/// <value>The name of the collection.</value>
		public string CollectionName {
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
		/// Initializes a new instance of the <see cref="Appleseed.Base.Data.Repository.BaseCollectionItemRepository"/> class.
		/// </summary>
		/// <param name="incomingConnectionString">Repo connection string.</param>
		/// <param name="logger">Logger.</param>
		public BaseCollectionItemRepository (string incomingConnectionString, Logger logger)
		{
			this.Log = logger;
			this.ConnectionString = incomingConnectionString;
			this.Client = new MySqlConnection (incomingConnectionString);
	
			// TODO: Client , Repository need to be injected like logger 
				//this.Client = new ??Client ();
		}

		//TEMPORARY - for testing only 


		/// <summary>
		/// Gets the item queue.
		/// </summary>
		/// <returns>The item queue.</returns>
		public List<BaseCollectionItem> GetItemQueue (){
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

			List<BaseCollectionItem> itemCollection = new List<BaseCollectionItem>();

			if (myReader.HasRows) {
				Log.Info ("This Collection has Items");
				while (myReader.Read ()) {
					Log.Info (myReader.GetString ("ItemTitle") + ":" + myReader.GetString ("ItemUrl"));
					//DONE: convert list of tags separated by commas into a List of strings
					List<string> itemTags = new List<string>(myReader.GetString ("ItemTags").Split(','));

					itemCollection.Add (new BaseCollectionItem {
						ItemID = myReader.GetInt32("ItemID"),
						ItemTitle = myReader.GetString ("ItemTitle"),
						ItemUrl = myReader.GetString ("ItemUrl"),
						ItemContent_Image = "",
						ItemContent_Raw = "",
						ItemDescription = myReader.GetString ("ItemDescription"),
						ItemTags = myReader.GetString ("ItemTags"),
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

	}
}


using System;
using MySql.Data;
using MySql.Data.MySqlClient;
using MongoDB.Driver;
using MongoDB.Bson;
using System.Threading.Tasks;
using NReadability;
using NLog;
using Appleseed.Base.Data;
using Appleseed.Base.Data.Model;

using System.Collections.Generic;

namespace Appleseed.Base.Engine
{
	/// <summary>
	/// Playground. Old methods. 
	/// </summary>
	public class Playground
	{
		public Playground ()
		{
		}


		static void CallMongoData ()
		{
			//TODO: connect to mongodb
			//string connectionString = ConfigurationManager.ConnectionStrings[CONNECTION_STRING_KEY].ConnectionString;
			Logger mylog = LogManager.GetCurrentClassLogger ();
			MongoClient client = new MongoClient ();
			var database = client.GetDatabase ("test");
			var collection = database.GetCollection<BaseCollectionItem> ("CollectionItems");
			mylog.Info ("Connected to MongoDB ");
			//TODO: prepare seed data into mongodb
			List<BaseCollectionItemObject> listOfItems = new List<BaseCollectionItemObject> {
				new BaseCollectionItemObject {
					ItemID = 1,
					ItemTitle = "Anant",
					ItemUrl = "http://www.anant.us",
					ItemContent_Image = "",
					ItemContent_Raw = "",
					ItemDescription = "",
					ItemTags = new List<String> {
						"Service",
						"Support",
						"Advisory",
						"Managed",
						"Maintenance",
						"CIO",
						"CTO"
					},
					ItemProcessedDate = DateTime.Today,
				},
				new BaseCollectionItemObject {
					ItemID = 2,
					ItemTitle = "Appleseed",
					ItemUrl = "http://www.appleseedapp.com",
					ItemContent_Image = "",
					ItemContent_Raw = "",
					ItemDescription = "",
					ItemTags = new List<String> {
						"Product",
						"Portal",
						"Search",
						"Integration",
						"Open Source"
					},
					ItemProcessedDate = DateTime.Today,
				},
				new BaseCollectionItemObject {
					ItemID = 3,
					ItemTitle = "KonoTree",
					ItemUrl = "http://www.konotree.com",
					ItemContent_Image = "",
					ItemContent_Raw = "",
					ItemDescription = "",
					ItemTags = new List<String> {
						"Software as a Service",
						"Content",
						"Knowledge",
						"Relationships",
						"Internet Software"
					},
					ItemProcessedDate = DateTime.Today,
				},
			};
			mylog.Info ("Starting Async Context");
			// This is needed to run asynchronous methods in command line 
			Task.Run (async () =>  {
				//TODO: prepare seed data into mongodb
				//await collection.InsertOneAsync (listOfItems [0]);
				//mylog.Info ("Add One Object into Mongo");
				await collection.InsertManyAsync (listOfItems, null);
				mylog.Info ("Add Multiple Objects into Mongo");
			}).Wait ();


			Task.Run (async () =>  {
				//TODO: retrieve data from mongodb
				var items = await collection.Find (x => x.ItemTitle != "").ToListAsync ();
				mylog.Info ("Retreive Information from Database");
				foreach (BaseCollectionItem item in items) {
					//System.Console.WriteLine (item.ItemID + ":" + item._id);
					System.Console.WriteLine (item.ItemTitle + ":" + item.ItemUrl);
				}
			}).Wait ();
			mylog.Info ("Leaving Async Context");
			//TODO: close mongodb connection - not needed
		}

		static void CallMySQLData ()
		{
			Logger mylog = LogManager.GetCurrentClassLogger ();
			//TODO: Connect to MYSQL Successfully with out error
			MySqlConnection myConnection = new MySqlConnection ("Server=localhost;Database=ga;Uid=root;Pwd=;");
			myConnection.Open ();
			mylog.Info ("Connected to the database");
			//TODO: Select data from table in MYSQL Successfully and get it back in .NET
			MySqlCommand myCommand = new MySqlCommand ("SELECT * FROM collectionitemqueue", myConnection);
			MySqlDataReader myReader = myCommand.ExecuteReader ();
			mylog.Info ("Executed the command in the database");
			//TODO: enumerate through data and display on console
			if (myReader.HasRows) {
				mylog.Info ("This reader has rows");
				while (myReader.Read ()) {
					System.Console.WriteLine (myReader.GetString ("ItemTitle") + ":" + myReader.GetString ("ItemUrl"));
				}
			}
			myReader.Close ();
			//TODO: insert a new record into the database 
			myCommand.CommandText = @"INSERT 
									INTO collectionitemqueue 
									(ItemTitle, ItemUrl, ItemDescription, ItemTags, ItemProcessed ) 
									VALUES 
									('SQL Datareader Read',
									'https://msdn.microsoft.com/en-us/library/system.data.sqlclient.sqldatareader.read%28v=vs.110%29.aspx',
									'Comprehensive tutorial on SQL Data Read',
									'.net,sql,ado', 
									false);";
			int returned = myCommand.ExecuteNonQuery ();
			mylog.Info ("Database Returned: " + returned);
			myConnection.Close ();
			mylog.Info ("Closed the database connection");
			//TODO: refactor into method that returns a dataset / enumerable of items
		}

		static void CallRestService ()
		{
			// Log entry -> connecting to rest service
			Logger mylog = LogManager.GetCurrentClassLogger ();
			mylog.Info ("Connecting to rest service");
			Rest newRestService = new Rest ();
			List<String> dataFromService = newRestService.getEventNames ();
			foreach (String item in dataFromService) {
				System.Console.WriteLine ("Info:" + item);
				// Log entry for each time this works
				mylog.Info ("Info:" + item);
			}
			System.Console.WriteLine ("Hello World!");
			System.Console.ReadKey ();
			// Log entry for when the app is done 
			mylog.Info ("I'm done");
		}
	}
}


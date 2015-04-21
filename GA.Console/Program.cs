using System;
using GA.Data;
using System.Collections.Generic;
using NLog;
using MySql.Data;
using MySql.Data.MySqlClient;
using MongoDB.Driver;
using MongoDB.Bson;

namespace GA.Console
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			//CallRestService ();
			//CallMySQLData ();

			//TODO: connect to mongodb
			//string connectionString = ConfigurationManager.ConnectionStrings[CONNECTION_STRING_KEY].ConnectionString;

			MongoClient client = new MongoClient();
			var database = client.GetDatabase("test");
			var collection = database.GetCollection<CollectionItem>("CollectionItems");

			//TODO: insert seed data into mongodb

			List<CollectionItem> listOfItems = new List<CollectionItem>{ 
					new CollectionItem{
						ItemID 				= 1,
						ItemTitle 			= "Anant",
						ItemUrl 			= "http://www.anant.us",
						ItemContentImage	= "",
						ItemContentCache	= "",
						ItemDescription		= "",
						ItemTags			= new List<String>{ "Service", "Support", "Advisory", "Managed", "Maintenance", "CIO", "CTO"},
						ItemProcessedDate 	= DateTime.Today,
					}, 
					new CollectionItem{
						ItemID 				= 2,
						ItemTitle 			= "Appleseed",
						ItemUrl 			= "http://www.appleseedpapp.com",
						ItemContentImage	= "",
						ItemContentCache	= "",
						ItemDescription		= "",
						ItemTags			= new List<String>{ "Product", "Portal", "Search", "Integration", "Open Source"},
						ItemProcessedDate 	= DateTime.Today,
					},
					new CollectionItem{
						ItemID 				= 3,
						ItemTitle 			= "KonoTree",
						ItemUrl 			= "http://www.konotree.com",
						ItemContentImage	= "",
						ItemContentCache	= "",
						ItemDescription		= "",
						ItemTags			= new List<String>{ "Software as a Service", "Content", "Knowledge", "Relationships", "Internet Software"},
						ItemProcessedDate 	= DateTime.Today,
					},
			};

			collection.InsertOneAsync (listOfItems [0]);

			//collection.InsertManyAsync (listOfItems, null);

			//TODO: retrieve data from mongodb

			//IMongoCollection<BsonDocument> collection = db.GetCollection<BsonDocument>("startup_log");


			//TODO: insert data into mongodb

			//TODO: close mongodb connection


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

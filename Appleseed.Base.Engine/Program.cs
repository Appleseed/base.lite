using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Configuration;

using Owin;
using Microsoft.Owin;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin.Hosting;
using Microsoft.CSharp;

using NLog;
using NLog.SignalR;
using Microsoft.Owin.Cors;

using Appleseed.Base.Data;
using Appleseed.Base.Data.Model;
using Appleseed.Base.Data.Repository;

[assembly: OwinStartup(typeof(Appleseed.Base.Engine.Startup))]
namespace Appleseed.Base.Engine
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			// DONE: define the logger 
			Logger log = LogManager.GetCurrentClassLogger ();

			string url = "http://localhost:9000";
			using (WebApp.Start(url))
			{
				Console.WriteLine("Leaves Processing Service - Running on {0}", url);

				// DONE: implement a continuously running application that monitors MySQL and inserts to MongoDB if new data comes in
				Console.WriteLine("Leaves Processing Service - Press ESC to stop Server");

				/*
				do {
					while (! Console.KeyAvailable) {*/


						// DEFINE THE AGGREGATION QUEUE
						BaseCollectionItemQueue itemAggregateQueue = new BaseCollectionItemQueue (ConfigurationManager.AppSettings ["Database_CollectionItemQueue"].ToString (), log, "BaseCollectionItem", "Aggregate");
						
						// GET FROM COLLECTIONITEMQUEUE - SEED DATA 
						List<BaseCollectionItem> itemAggregateCollection = GetAggregateItemsFromRepository (log);

						//ENQUEUE ITEMS FROM COLLECTION AND ONE OFFS
						//DONE: test batch insert of items into queue
						itemAggregateQueue.Enqueue(itemAggregateCollection);

						//DONE: test single insert of items into queue
						itemAggregateQueue.Enqueue(new BaseCollectionItem{
							ItemTitle = "KonoTree",
							ItemUrl = "http://www.konotree.com",
							ItemContent_Image = "",
							ItemContent_Raw = "",
							ItemDescription = ""
						});

						//DONE: test single insert of items into queue
						itemAggregateQueue.Enqueue(new BaseCollectionItem{
							ItemTitle = "Anant",
							ItemUrl = "http://www.anant.us/Home.aspx",
							ItemContent_Image = "",
							ItemContent_Raw = "",
							ItemDescription = ""
						});

						//DONE: test single insert of items into queue
						itemAggregateQueue.Enqueue(new BaseCollectionItem{
							ItemTitle = "Appleseed",
							ItemUrl = "http://www.appleseedapp.com",
							ItemContent_Image = "",
							ItemContent_Raw = "",
							ItemDescription = ""
						});

						//TODO: INSERT: insert object into mongo repository  / queue?
						//BaseCollectionRepository repo = new BaseCollectionRepository("",log);
						//repo.AddMany(itemCollection);

						//DONE: wait 10 seconds before next poll 

						List<BaseCollectionItem> itemExtractedCollection = new List<BaseCollectionItem>();

						//TODO: DEQUEUE AND ENQUEUE IN BATCH 
						foreach (BaseCollectionItem item in itemAggregateQueue) {
							item.ItemID = 0; //RESET the ID to 0 so it doesn't show up as duplicate
							itemExtractedCollection.Add(ExtractContentFromWeb(item, log));
							itemAggregateQueue.Dequeue();
						}

						BaseCollectionItemQueue itemExtractQueue = new BaseCollectionItemQueue (ConfigurationManager.AppSettings ["Database_CollectionItemQueue"].ToString (), log, "BaseCollectionItem", "Extracted");
						itemExtractQueue.Enqueue(itemExtractedCollection);
						itemAggregateQueue.Clear ();

						// REQUEUE ItemAggregateCollection 
						itemAggregateQueue.Enqueue(itemAggregateCollection);

						//TODO: DEQUEUE AND ENQUEUE ONE BY ONE  
						foreach (BaseCollectionItem item in itemAggregateQueue) {
							item.ItemID = 0; //RESET the ID to 0 so it doesn't show up as duplicate
							//itemExtractedCollection.Add(ExtractContentFromWeb(item, log));
							itemExtractQueue.Enqueue (ExtractContentFromWeb(item, log));
							itemAggregateQueue.Dequeue();
						}

						itemAggregateQueue.Clear ();

						//Thread.Sleep (10000);
						log.Info ("Leaves Processing Service - Next Cycle Starting Now - {0}", DateTime.Now);

				/*

					}
				} while (Console.ReadKey(true).Key != ConsoleKey.Escape); 
*/
			}
				
		}

		static  List<BaseCollectionItem> GetAggregateItemsFromRepository (Logger log)
		{
			//GET THE Data from REPO
			//DONE: Connect to MYSQL Successfully via the repository 
			BaseCollectionItemRepository itemRepository = new BaseCollectionItemRepository (ConfigurationManager.AppSettings ["Database_CollectionItemQueue"].ToString (), log);
			List<BaseCollectionItem> itemRepositoryCollection = itemRepository.GetItemQueue ();
			//DONE: test clearing the queue 
			//itemQueue.Clear();
			// PREPARE THE DATE FOR QUEUE
			List<BaseCollectionItem> itemAggregateCollection = new List<BaseCollectionItem> ();
			//TODO: first aggregate into queue from repository 
			foreach (BaseCollectionItem item in itemRepositoryCollection) {
				item.ItemID = 0;
				//RESET the ID to 0 so it doesn't show up as duplicate
				itemAggregateCollection.Add (item);
			}
			return itemAggregateCollection;
		}

		static BaseCollectionItem ExtractContentFromWeb (BaseCollectionItem item, Logger log)
		{
			//TODO: extract content from web using repository
			WebPageDataService webpage = new WebPageDataService (item.ItemUrl, log);
			item.ItemTitle = webpage.ExtractedTitle;
			item.ItemContent_Raw = webpage.ExtractedContent;
			item.ItemContent_Image = webpage.ExtractedImage;
			Console.WriteLine (item.ItemUrl + ":" + item.ItemTitle);
			return item;
		}

		static void MySqlToMongoDb ()
		{
			/*
			//DONE: Connect to MYSQL Successfully via the repository 
			CollectionItemQueue itemQueue = new CollectionItemQueue (ConfigurationManager.AppSettings ["Database_CollectionItemQueue"].ToString (), log);
			List<CollectionItem> itemQueueCollection = itemQueue.GetItemQueue ();
			foreach (CollectionItem item in itemQueueCollection) {
				//TODO: extract content from web using repository
				WebPageRepository webpage = new WebPageRepository (item.ItemUrl, log);
				item.ItemTitle = webpage.ExtractedTitle;
				item.ItemContentCache = webpage.ExtractedContent;
				item.ItemContentImage = webpage.ExtractedImage;
				Console.WriteLine (item.ItemUrl + ":" + item.ItemTitle);
			}
			//DONE: insert object into MongoDB 
			CollectionItemRepository itemRepository = new CollectionItemRepository ("", log);
			itemRepository.UpsertMany (itemQueueCollection);
			//DONE: wait 10 seconds before next poll 
			Thread.Sleep (10000);
			log.Info ("Leaves Processing Service - Next Cycle Starting Now - {0}", DateTime.Now);
			*/
		}
	}

	// TODO: start owin and get signalr started
	class Startup
	{
		public void Configuration(IAppBuilder app)
		{
			app.UseCors(CorsOptions.AllowAll);
			app.MapSignalR();
		}
	}

	// TODO: implement and test NLog.SignalR that shows all log messages from console on admin page
	public class LoggingHub : Hub<ILoggingHub>
	{
		public void Log(LogEvent logEvent)
		{
			Clients.All.Log (logEvent);

		}

		public void Hello(){
			this.Clients.Caller.Log(new LogEvent{Level="Info", Message="SignalR Connected", TimeStamp = DateTime.UtcNow});
		}
	}

	// TODO: implement a self hosted signalr server on OWIN to send messages to Appleseed.Base.Web
	public class WebHub : Hub
	{
		public void Update(string title, string description, string url)
		{
			//Clients.All.updateListing(title, description, url);
		}
	}

}

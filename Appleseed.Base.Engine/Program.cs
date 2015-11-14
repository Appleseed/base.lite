using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Appleseed.Base.Data.Model;
using Appleseed.Base.Data.Queue;
using Appleseed.Base.Data.Repository;
using Appleseed.Base.Data.Repository.Contracts;
using Appleseed.Base.Data.Service;
using Appleseed.Base.Data.Utility;
using Appleseed.Base.Engine;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Hosting;
using NLog;
using NLog.SignalR;
using Owin;

[assembly: OwinStartup(typeof(Startup))]

namespace Appleseed.Base.Engine
{
    internal class MainClass
    {
        public static void Main(string[] args)
        {

            // DONE: define the logger 
            var log = LogManager.GetCurrentClassLogger();

            var url = "http://localhost:9000";
            using (WebApp.Start(url))
            {
                Console.WriteLine("Leaves Processing Service - Running on {0}", url);

                // DONE: implement a continuously running application that monitors MySQL and inserts to MongoDB if new data comes in
                Console.WriteLine("Leaves Processing Service - Press ESC to stop Server");
                /*
				do {
					while (! Console.KeyAvailable) {*/

                var queueSection = ConfigurationManager.GetSection("queue") as QueueSection;
                var singleQueueName = "MySqlQueue";
                
                var queue = queueSection.Queue.SingleOrDefault(x => String.Equals(x.QueueName, singleQueueName, StringComparison.CurrentCultureIgnoreCase));
                if (queue != null)
                {
                    var repository = RepositoryService.GetRepository(queue.ConnectionString, queue.QueueName);
                    ProcessQueueCycle(repository, log);
                }
                
                /*else
                {
                    var repositories = RepositoryService.GetAllRepositories(queueSection);

                    foreach (var repository in repositories)
                    {
                        ProcessQueueCycle(repository, log);
                    }
                }*/

                /*

					}
				} while (Console.ReadKey(true).Key != ConsoleKey.Escape); */
            }
        }

        private static void ProcessQueueCycle(IRepository repository, Logger log)
        {
            var itemAggregateQueue = new BaseCollectionItemQueue(repository, log, "BaseCollectionItem", "Aggregate");

            // GET FROM COLLECTIONITEMQUEUE - SEED DATA 
            var itemAggregateCollection = GetAggregateItemsFromRepository(repository, log);

            //ENQUEUE ITEMS FROM COLLECTION AND ONE OFFS
            //DONE: test batch insert of items into queue
            itemAggregateQueue.Enqueue(itemAggregateCollection);

            //DONE: test single insert of items into queue
            itemAggregateQueue.Enqueue(new BaseCollectionItem
            {
                Data = new BaseCollectionItemData
                {
                    ItemTitle = "KonoTree",
                    ItemUrl = "http://www.konotree.com",
                    ItemContent_Image = "",
                    ItemContent_Raw = "",
                    ItemDescription = ""
                }
            });

            //DONE: test single insert of items into queue
            itemAggregateQueue.Enqueue(new BaseCollectionItem
            {
                Data = new BaseCollectionItemData
                {
                    ItemTitle = "Anant",
                    ItemUrl = "http://www.anant.us/Home.aspx",
                    ItemContent_Image = "",
                    ItemContent_Raw = "",
                    ItemDescription = ""
                }
            });

            //DONE: test single insert of items into queue
            itemAggregateQueue.Enqueue(new BaseCollectionItem
            {
                Data = new BaseCollectionItemData
                {
                    ItemTitle = "Appleseed",
                    ItemUrl = "http://www.appleseedapp.com",
                    ItemContent_Image = "",
                    ItemContent_Raw = "",
                    ItemDescription = ""
                }
            });

            //TODO: INSERT: insert object into mongo repository  / queue?
            //BaseCollectionRepository repo = new BaseCollectionRepository("",log);
            //repo.AddMany(itemCollection);

            //DONE: wait 10 seconds before next poll 

            var itemExtractedCollection = new List<BaseCollectionItem>();

            //TODO: DEQUEUE AND ENQUEUE IN BATCH 
            foreach (var item in itemAggregateQueue)
            {
                item.Data.ItemID = 0; //RESET the ID to 0 so it doesn't show up as duplicate
                itemExtractedCollection.Add(ExtractContentFromWeb(item, log));
                itemAggregateQueue.Dequeue(item);
            }

            var itemExtractQueue = new BaseCollectionItemQueue(repository, log, "BaseCollectionItem", "Extracted");
            itemExtractQueue.Enqueue(itemExtractedCollection);
            itemAggregateQueue.Clear();

            // REQUEUE ItemAggregateCollection 
            itemAggregateQueue.Enqueue(itemAggregateCollection);


            //TODO: DEQUEUE AND ENQUEUE ONE BY ONE  
            foreach (var item in itemAggregateQueue)
            {
                item.Data.ItemID = 0; //RESET the ID to 0 so it doesn't show up as duplicate
                //itemExtractedCollection.Add(ExtractContentFromWeb(item, log));
                itemExtractQueue.Enqueue(ExtractContentFromWeb(item, log));
                itemAggregateQueue.Dequeue(item);
            }

            itemAggregateQueue.Clear();

            //Thread.Sleep (10000);
            log.Info("Leaves Processing Service - Next Cycle Starting Now - {0}", DateTime.Now);
        }

        private static List<BaseCollectionItem> GetAggregateItemsFromRepository(IRepository itemRepository, Logger log)
        {
            //GET THE Data from REPO
            //DONE: Connect to MYSQL Successfully via the repository 
            ////SqlRepository itemRepository = new MySqlRepository(ConfigurationManager.AppSettings["Database_CollectionItemQueue"], log);
            var itemRepositoryCollection = itemRepository.GetAllFromQueue();
            //DONE: test clearing the queue 
            //itemQueue.Clear();
            // PREPARE THE DATE FOR QUEUE
            var itemAggregateCollection = new List<BaseCollectionItem>();
            //TODO: first aggregate into queue from repository 
            foreach (var item in itemRepositoryCollection)
            {
                item.TableId = 0;
                //RESET the ID to 0 so it doesn't show up as duplicate
                itemAggregateCollection.Add(item);
            }
            return itemAggregateCollection;
        }

        private static BaseCollectionItem ExtractContentFromWeb(BaseCollectionItem item, Logger log)
        {
            //TODO: extract content from web using repository
            var webpage = new WebPageDataService(item.Data.ItemUrl, log);
            item.Data.ItemTitle = webpage.ExtractedTitle;
            item.Data.ItemContent_Raw = webpage.ExtractedContent;
            item.Data.ItemContent_Image = webpage.ExtractedImage;
            Console.WriteLine(item.Data.ItemUrl + ":" + item.Data.ItemTitle);
            return item;
        }

        private static void MySqlToMongoDb()
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
    internal class Startup
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
            Clients.All.Log(logEvent);
        }

        public void Hello()
        {
            Clients.Caller.Log(new LogEvent { Level = "Info", Message = "SignalR Connected", TimeStamp = DateTime.UtcNow });
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
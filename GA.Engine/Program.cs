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

using GA.Data;

[assembly: OwinStartup(typeof(GA.Engine.Startup))]
namespace GA.Engine
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			// DONE: define the logger 
			Logger log = LogManager.GetCurrentClassLogger ();

			string url = "http://localhost:8081";
			using (WebApp.Start(url))
			{
				Console.WriteLine("Leaves Processing Service - Running on {0}", url);

				// DONE: implement a continuously running application that monitors MySQL and inserts to MongoDB if new data comes in
				Console.WriteLine("Leaves Processing Service - Press ESC to stop Server");

				do {
					while (! Console.KeyAvailable) {
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
						itemRepository.UpsertMany(itemQueueCollection);

						//DONE: wait 10 seconds before next poll 
						Thread.Sleep(10000);
						log.Info("Leaves Processing Service - Next Cycle Starting Now - {0}", DateTime.Now);
					}
				} while (Console.ReadKey(true).Key != ConsoleKey.Escape); 

			}
				
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

	// TODO: implement a self hosted signalr server on OWIN to send messages to ga.web
	public class WebHub : Hub
	{
		public void Update(string title, string description, string url)
		{
			//Clients.All.updateListing(title, description, url);
		}
	}

}

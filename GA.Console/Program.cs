using System;
using GA.Data;
using System.Collections.Generic;
using NLog;
using System.Configuration;

namespace GA.Console
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			// DONE: define the logger 
			Logger log = LogManager.GetCurrentClassLogger ();
			//TODO: Connect to MYSQL Successfully via the repository 
			CollectionItemQueue itemQueue = new CollectionItemQueue (ConfigurationManager.AppSettings ["Database_CollectionItemQueue"].ToString (), log);
			List<CollectionItem> itemQueueCollection = itemQueue.GetItemQueue ();

			foreach (CollectionItem item in itemQueueCollection) {
				//TODO: extract content from web using repository
				WebPageRepository webpage = new WebPageRepository (item.ItemUrl, log);
				item.ItemTitle = webpage.ExtractedTitle;
				item.ItemContentCache = webpage.ExtractedContent;
				item.ItemContentImage = webpage.ExtractedImage;
				System.Console.WriteLine (item.ItemUrl + ":" + item.ItemTitle);
			}

			//TODO: insert object into MongoDB 
			CollectionItemRepository itemRepository = new CollectionItemRepository ("", log);
			itemRepository.UpsertMany(itemQueueCollection);

			//TODO: notify page to rebind via signalr ?
		}

	}
}

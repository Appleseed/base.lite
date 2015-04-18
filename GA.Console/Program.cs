using System;
using GA.Data;
using System.Collections.Generic;
using NLog;

namespace GA.Console
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			// Log entry -> connecting to rest service
			Logger mylog = LogManager.GetCurrentClassLogger();
			mylog.Info ("Connecting to rest service");

			Rest newRestService = new Rest ();
			List<String> dataFromService = newRestService.getEventNames ();

			foreach( String item in dataFromService){
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

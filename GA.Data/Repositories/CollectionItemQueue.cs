using System;
using NLog;

namespace GA.Data
{
	/// <summary>
	/// Collection item queue. Gets / Removes items from the Collection Item queue in MySQL
	/// </summary>
	public class CollectionItemQueue
	{
		public Logger Log {
			get;
			private set;
		}

		public string Connection {
			get;
			private set;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="GA.Data.CollectionItemQueue"/> class.
		/// </summary>
		/// <param name="ConnectionString">Connection string.</param>
		/// <param name="logger">Logger.</param>
		public CollectionItemQueue (string ConnectionString, Logger logger)
		{ 
			this.Log = logger;
			this.Connection = ConnectionString;
		}

		//TODO: implement method to get batch repository items 
			// gets unprocessed items 

		//TODO: implement method to insert individual repository items 
			// adds one record - will probably do this in gridview 

		//TODO: implement method to insert batch repository items 
			// not needed - data will be added manually 

		//TODO: implement method to get individual repository items
			// not needed - we'll be batch processing


	}
}


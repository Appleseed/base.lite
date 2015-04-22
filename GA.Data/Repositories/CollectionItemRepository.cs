using System;
using NLog;

namespace GA.Data
{
	/// <summary>
	/// Collection item repository. Gets / adds information to a MongoDB Collection
	/// </summary>
	public class CollectionItemRepository
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
		/// Initializes a new instance of the <see cref="GA.Data.CollectionItemRepository"/> class.
		/// </summary>
		/// <param name="ConnectionString">Connection string.</param>
		/// <param name="logger">Logger.</param>
		public CollectionItemRepository (string ConnectionString, Logger logger)
		{ 
			this.Log = logger;
			this.Connection = ConnectionString;
		}

		//TODO: implement method to get batch repository items 
			// gets all the repository items in the repo 

		//TODO: implement method to insert batch repository items 
			// inserts a bunch of items at the same time 

		//TODO: implement method to get individual repository items
			// not needed right now - will be able to get individual view 

		//TODO: implement method to insert individual repository items 
			// not needed right now - will be able to add items incrementally 

	}
}


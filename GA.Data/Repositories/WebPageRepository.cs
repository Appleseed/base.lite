using System;
using System.Web;
using System.Net;
using NLog;

namespace GA.Data
{
	public class WebPageRepository
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
		public string ConnectionURL {
			get;
			private set;
		}

		/// <summary>
		/// Gets or sets the client.
		/// </summary>
		/// <value>The client.</value>
		private WebClient Client {
			get;
			set;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="GA.Data.WebPageRepository"/> class.
		/// </summary>
		/// <param name="incomingConnectionURL">Incoming connection UR.</param>
		/// <param name="logger">Logger.</param>
		public WebPageRepository (string incomingConnectionURL, Logger logger)
		{
			this.Log = logger;
			this.ConnectionURL = incomingConnectionURL;

		}

		//TODO: implement a page scraper via NReadability or an API 
		// maybe use a REST service to get image/ etc and NReadability to the article itself
		// https://www.mashape.com/pbkwee/html2text + http://scraper.io/
	}
}


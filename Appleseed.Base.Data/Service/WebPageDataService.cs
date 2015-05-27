using System;
using System.Web;
using System.Net;
using NLog;
using NReadability;

namespace Appleseed.Base.Data.Repository
{
	public class WebPageDataService
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
		/// Gets the content of the extracted.
		/// </summary>
		/// <value>The content of the extracted.</value>
		public string ExtractedContent {
			get;
			private set;
		}

		/// <summary>
		/// Gets the extracted title.
		/// </summary>
		/// <value>The extracted title.</value>
		public string ExtractedTitle {
			get;
			private set;
		}

		/// <summary>
		/// Gets the extracted image.
		/// </summary>
		/// <value>The extracted image.</value>
		public string ExtractedImage {
			get;
			private set;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Appleseed.Base.Data.WebPageRepository"/> class.
		/// </summary>
		/// <param name="incomingConnectionURL">Incoming connection UR.</param>
		/// <param name="logger">Logger.</param>
		public WebPageDataService (string incomingConnectionURL, Logger logger)
		{
			this.Log = logger;
			this.ConnectionURL = incomingConnectionURL;

			//DONE: implement a page scraper via NReadability or an API 
			// maybe use a REST service to get image/ etc and NReadability to the article itself
			// https://www.mashape.com/pbkwee/html2text + http://scraper.io/

			try {
				NReadabilityWebTranscoder wt = new NReadabilityWebTranscoder ();
				WebTranscodingResult wtr = wt.Transcode (new WebTranscodingInput (this.ConnectionURL));

				this.ExtractedContent = wtr.ExtractedContent;
				this.ExtractedTitle = wtr.ExtractedTitle;
				this.ExtractedImage = "";
			} catch (Exception ex) {
				Log.ErrorException ("Error", ex);
			} 
		}


	}
}


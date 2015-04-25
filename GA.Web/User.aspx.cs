using System;
using System.Web;
using System.Web.UI;
using GA.Data;
using NLog;
using System.Configuration;

namespace GA.Web
{
	
	public partial class User : System.Web.UI.Page
	{

		public Logger mylog = LogManager.GetCurrentClassLogger();

		protected void Page_Load(Object sender, EventArgs e){
			//TODO: bind count to the number of items in the DB 


			var connString = ConfigurationManager.AppSettings ["Database_CollectionItemRepository"].ToString ();

			CollectionItemRepository repo = new CollectionItemRepository ( 
				connString
				, mylog);
			
			//TODO: bind list view to the items in the DB

			//TODO: when item is selected, bind data to panel and then make panel visible 

		}
		
	}
}


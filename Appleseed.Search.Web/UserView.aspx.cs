using System;
using System.Web;
using System.Web.UI;
using GA.Data;
using GA.Data.Model;
using NLog;
using System.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Threading.Tasks;
using System.Web.UI.WebControls;

namespace GA.Web
{
	
	public partial class UserView : System.Web.UI.Page
	{
		/// <summary>
		/// The mylog.
		/// </summary>
		public Logger mylog = LogManager.GetCurrentClassLogger();

		/// <summary>
		/// Pages the load.
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="e">E.</param>
		protected void Page_Load(Object sender, EventArgs e){

			CollectionList_DataBind ();

		}	

		/// <summary>
		/// Collections the list data bind.
		/// </summary>
		void CollectionList_DataBind(){
			//TODO: bind list view to the items in the DB

			MongoClient client = new MongoClient ();
			var database = client.GetDatabase ("test");
			var collection = database.GetCollection<BaseCollectionItem> ("CollectionItems");
			mylog.Info ("Connected to MongoDB ");

			Task.Run (async () =>  {
				//TODO: retrieve data from mongodb
				var items = await collection.Find (x => x.ItemTitle != "").ToListAsync ();
				mylog.Info ("Retreive Information from Database");
				lvCollectionItems.DataSource = items;
				lvCollectionItems.DataBind();
			}).Wait ();

			//TODO: bind count to the number of items in the DB 

			lblCount.Text = lvCollectionItems.Items.Count.ToString();
		}
		/// <summary>
		/// Lbs the view item on command.
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="args">Arguments.</param>
		protected void lbViewItem_OnCommand(Object sender, CommandEventArgs args){
			//TODO: when item is selected, bind data to panel and then make panel visible 

			if (args.CommandName == "View") {
				//TODO go to mongo, get data, bind to form view 
				MongoClient client = new MongoClient ();
				var database = client.GetDatabase ("test");
				var collection = database.GetCollection<BaseCollectionItem> ("CollectionItems");
				mylog.Info ("Connected to MongoDB ");

				Task.Run (async () =>  {
					//TODO: retrieve data from mongodb
					//var items = await collection.Find (x => x._id == ObjectId.Parse ((string)args.CommandArgument) ).ToListAsync ();
					mylog.Info ("Retreive Information from Database");
					//fvCollectionItem.DataSource = items;
					//fvCollectionItem.DataBind();

					//ifContentCache.InnerHtml = "Things will go here";
				}).Wait ();

				fvCollectionItem.Visible = true;
			}
		}
	}

}
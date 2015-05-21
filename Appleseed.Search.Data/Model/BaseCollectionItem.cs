using System;
using System.Collections.Generic;

namespace GA.Data.Model
{
	/// <summary>
	/// Base collection item. The base item in the collection of leaves or elements in the Appleseed.Base system of things. 
	/// </summary>
	public class BaseCollectionItem 
	{
		public long 		ItemID { get; set; }
		public string 		ItemPath { set; get; }
		public string 		ItemUrl { set; get; }
		public string 		ItemName { set; get; }
		public string 		ItemTitle { set; get; }
		public string 		ItemDescription { set; get; }
		public string 		ItemSummary { set; get; }
		public string 		ItemContent_Raw { set; get; }
		public string 		ItemContent_Rich { set; get; }
		public string 		ItemContent_Text { set; get; }
		public string 		ItemContent_Image { set; get; }
		public string 		ItemContent_Image_Url { set; get; }
		public string 		ItemTags { set; get; }
		public string 		ItemKeywords { set; get; }
		public string	 	ItemCategories { set; get; }
		public DateTime 	ItemCreatedDate { set; get;}
		public DateTime 	ItemProcessedDate { set; get;}
		public bool			ItemProcessed { set; get; }
		public string 		ItemQueue { set; get;}
	}
}

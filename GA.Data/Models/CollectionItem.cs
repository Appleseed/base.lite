using System;
using MongoDB.Bson;
using System.Collections.Generic;

namespace GA.Data
{
	public class CollectionItem
	{
		public ObjectId _id { get; set; }
		public int ItemID { get; set; }
		public string ItemTitle { set; get; }
		public string ItemUrl { set; get; }
		public string ItemDescription { set; get; }
		public string ItemContentCache { set; get; }
		public string ItemContentImage { set; get; }
		public List<string> ItemTags { set; get; }
		public DateTime ItemProcessedDate { set; get;}

	}
}


using System;
using System.Collections.Generic;

namespace GA.Data.Model
{
	/// <summary>
	/// Collection item. Specific overides related to what we currently call "CollectionItem" which is used in Appleseed.Search 
	/// as the base model that is used in processing, indexing, and retrieval of information. 
	/// </summary>
	public class BaseCollectionItemObject : BaseCollectionItem
	{
		public new List<string> ItemTags { set; get; }
		public new List<string> ItemKeywords { set; get; }
		public new List<string>	ItemCategories { set; get; }
	}
}


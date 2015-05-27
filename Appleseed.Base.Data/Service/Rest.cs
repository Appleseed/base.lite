using System;
using System.Collections.Generic;

namespace Appleseed.Base.Data
{
	public class Rest
	{
		public Rest ()
		{
		}

		public List<String> getEventNames(){
			List<String> listOfEvents = new List<String>{ "Interface", 
										"Software", 
										"Database", 
										"Systems"};
			return listOfEvents;
		}
	}
}


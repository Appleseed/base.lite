using System;
using System.Web;
using System.Web.UI;
using Appleseed.Base.Data;
using System.Collections.Generic;
using NLog;
using System.Web.UI.WebControls;
using System.Threading;


namespace Appleseed.Base.Web
{
	
	public partial class Default : System.Web.UI.Page
	{
		public Logger mylog = LogManager.GetCurrentClassLogger();


		protected void btnLongRunningProcess_Click(Object sender, EventArgs e)
		{
			Thread.Sleep(5000);
		}

	}

}


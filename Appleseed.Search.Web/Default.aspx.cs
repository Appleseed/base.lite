using System;
using System.Web;
using System.Web.UI;
using GA.Data;
using System.Collections.Generic;
using NLog;
using System.Web.UI.WebControls;
using System.Threading;


namespace GA.Web
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


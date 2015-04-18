using System;
using System.Web;
using System.Web.UI;
using GA.Data;
using System.Collections.Generic;
using NLog;

namespace GA.Web
{
	
	public partial class Default : System.Web.UI.Page
	{
		public Logger mylog = LogManager.GetCurrentClassLogger();


		public void button1Clicked (object sender, EventArgs args)
		{

			//log when button is clicked 
			mylog.Info ("Button is clicked");

			ddlEventNames.DataSource = getMyEvents ();
			ddlEventNames.DataBind ();
			ddlEventNames.Visible = true;

			button1.Text = "Retreived data!";
		}

		List<String> getMyEvents ()
		{
			//log when this service is called 
			mylog.Info ("Service is called");
			Rest newRestService = new Rest ();
			List<String> dataFromService = newRestService.getEventNames ();
			return dataFromService;
		}

		protected void ddlEventNames_SelectedIndexChanged(object sender, EventArgs e)
		{
			//log when 
			button1.Text = ddlEventNames.SelectedValue;
			mylog.Info ("Selection box was changed");

		}

	}
}


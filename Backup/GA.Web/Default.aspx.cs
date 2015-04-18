using System;
using System.Web;
using System.Web.UI;
using GA.Data;
using System.Collections.Generic;

namespace GA.Web
{
	
	public partial class Default : System.Web.UI.Page
	{
		public void button1Clicked (object sender, EventArgs args)
		{

			ddlEventNames.DataSource = getMyEvents ();
			ddlEventNames.DataBind ();
			ddlEventNames.Visible = true;

			button1.Text = "Retreived data!";
		}

		List<String> getMyEvents ()
		{
			Rest newRestService = new Rest ();
			List<String> dataFromService = newRestService.getEventNames ();
			return dataFromService;
		}

		protected void ddlEventNames_SelectedIndexChanged(object sender, EventArgs e)
		{
			button1.Text = ddlEventNames.SelectedValue;


		}

	}
}


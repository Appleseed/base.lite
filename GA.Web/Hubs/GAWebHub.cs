using System;
using System.Threading;
using Microsoft.AspNet.SignalR;
using NLog.SignalR;

namespace GA.Web
{
	public class GAWebHub : Hub<ILoggingHub>
	{
		public GAWebHub ()
		{
			
		}

		public void Log(LogEvent logEvent)
		{
			Clients.Others.Log(logEvent);
		}

	}
}


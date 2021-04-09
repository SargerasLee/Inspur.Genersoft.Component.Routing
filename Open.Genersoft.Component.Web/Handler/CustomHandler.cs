using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Open.Genersoft.Component.Web.Handler
{
	public class CustomHandler : IHttpHandler
	{
		public bool IsReusable
		{
			get
			{
				return true;
			}
		}

		public void ProcessRequest(HttpContext context)
		{
			
		}
	}
}

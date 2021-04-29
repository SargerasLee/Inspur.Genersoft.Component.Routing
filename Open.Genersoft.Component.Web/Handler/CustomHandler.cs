using Open.Genersoft.Component.Routing.Entity;
using Open.Genersoft.Component.Routing.Public.Spi;
using System.IO;
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
			HttpRequest request = context.Request;
			HttpResponse response = context.Response;
			string str = string.Empty;
			CustomComponentResult result;
			if (request.HttpMethod == "POST")
			{
					StreamReader sr = new StreamReader(request.InputStream);
					str = sr.ReadToEnd(); 
			}
			result = Router.Routing(request.RawUrl, str);
			response.ContentType = result.MediaType;
			response.Write(result.Data.ToString());
			response.End();
		}
	}
}

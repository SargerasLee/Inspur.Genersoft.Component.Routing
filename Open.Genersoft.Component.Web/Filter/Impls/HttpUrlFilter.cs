using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Open.Genersoft.Component.Web.Filter.Impls
{
	public class HttpUrlFilter : IHttpFilter
	{
		public void DoFilter(HttpRequest request, HttpResponse response, FilterChain chain)
		{
			//request.RequestContext.HttpContext.ApplicationInstance.Context.RemapHandler
			chain.DoFilter(request, response);
		}
	}
}

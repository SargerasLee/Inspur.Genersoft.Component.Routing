using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
namespace Open.Genersoft.Component.Web.Filter
{
	public class FilterChain
	{
		private readonly IHttpFilter Current;
		public FilterChain Next { get; set; }

		public FilterChain(IHttpFilter filter)
		{
			Current = filter;
		}

		public void DoFilter(HttpRequest request, HttpResponse response)
		{
			if (Current == null) return;
			if (Next == null)
				Next = new FilterChain(null);
			Current.DoFilter(request, response, Next); 
		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
namespace Open.Genersoft.Component.Web.Filter
{
	public interface IHttpFilter
	{
		void DoFilter(HttpRequest request, HttpResponse response, FilterChain chain);
	}
}

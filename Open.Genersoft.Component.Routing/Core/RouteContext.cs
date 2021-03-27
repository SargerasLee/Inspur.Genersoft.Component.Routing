using System.Collections.Generic;

namespace Open.Genersoft.Component.Routing.Core
{
	public class RouteContext
	{
		public Dictionary<string, string> UrlParams { get; }

		public Dictionary<string, string> RouteParams { get; }

		public object[] Objects { get; }

		public RouteContext(Dictionary<string, string> urlParams, Dictionary<string, string> routeParams, object[] objs)
		{
			UrlParams = urlParams;
			RouteParams = routeParams;
			Objects = objs;
		}
	}
}

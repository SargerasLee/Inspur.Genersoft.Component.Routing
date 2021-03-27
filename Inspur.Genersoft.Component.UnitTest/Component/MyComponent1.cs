using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Open.Genersoft.Component.Routing;
using Open.Genersoft.Component.Routing.Attributes;
using Open.Genersoft.Component.Routing.Core;

namespace Open.Genersoft.Component.UnitTest.Component
{
	[CustomComponent]
	[RouteMapping("/test/comp1")]
	public class MyComponent1
	{
		[RouteMapping("/{code}/{name}")]
		[Json]
		public Dictionary<string, string> Test1([RouteParam]string code, [RouteParam]string name)
		{
			return new Dictionary<string, string> { { "code", code }, { "name", name } };
		}

		[RouteMapping("/change")]
		public object Test2([UrlParam]int age)
		{
			return age;
		}

		[RouteMapping("/write")]
		public object Test3(RouteContext context)
		{
			return context.UrlParams;
		}
	}
}

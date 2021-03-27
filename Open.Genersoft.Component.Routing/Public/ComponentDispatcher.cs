using Open.Genersoft.Component.Routing.Core;
using Open.Genersoft.Component.Routing.Exceptions;
using System.Collections.Generic;
using System.Linq;

namespace Open.Genersoft.Component.Routing.Public
{
	internal class ComponentDispatcher
	{
		private readonly CustomComponentContainer container = CustomComponentContainer.Instance;

		public object Dispatch(string route, params object[] objs)
		{
			route = route.Trim();
			Dictionary<string, CustomComponentProxy> dict = container.ClassMapping;
			string targetKey = dict.Keys.Where(key => route.StartsWith(key)).FirstOrDefault();
			if (string.IsNullOrWhiteSpace(targetKey))
				throw new RouteNotMatchException("未匹配对应的类");
			object obj = dict[targetKey].Invoke(route.Substring(targetKey.Length), objs);
			return obj;
		}

		public static ComponentDispatcher Instance { get; } = new ComponentDispatcher();
	}
}

﻿using Open.Genersoft.Component.Routing.Core;
using Open.Genersoft.Component.Routing.Entity;
using Open.Genersoft.Component.Routing.Exceptions;
using Open.Genersoft.Component.Routing.Public.Utils;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
namespace Open.Genersoft.Component.Routing.Public
{
	internal class ComponentDispatcher
	{
		private readonly CustomComponentContainer container = CustomComponentContainer.Instance;

		public CustomComponentResult Dispatch(string route, object objs)
		{
			route = route.Trim();
			if (!Regex.IsMatch(route, RegexPattern.NormalUrlPattern))
				throw new RouteNotMatchException("非标准url路径");

			Dictionary<string, CustomComponentProxy> dict = container.ClassMapping;
			string url = HttpUrlUtil.GetDecodeUrlNoParam(route);
			Dictionary<string, string> urlParams = HttpUrlUtil.GetUrlParams(route);

			string targetKey = dict.Keys.Where(key => url.StartsWith(key)).FirstOrDefault();
			if (string.IsNullOrWhiteSpace(targetKey))
				throw new RouteNotMatchException("未匹配对应的类");
			return dict[targetKey].Invoke(url.Substring(targetKey.Length), urlParams , objs);
		}

		public static ComponentDispatcher Instance { get; } = new ComponentDispatcher();
	}
}

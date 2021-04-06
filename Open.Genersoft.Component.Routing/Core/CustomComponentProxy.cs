using Open.Genersoft.Component.Routing.Attributes;
using Open.Genersoft.Component.Routing.Exceptions;
using Open.Genersoft.Component.Routing.Public.Utils;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Open.Genersoft.Component.Routing.Core
{
	internal class CustomComponentProxy
	{
		public string Id { get; private set; }

		private readonly object realCustomComponent;
		private readonly Dictionary<string, MethodProxy> MethodDict = new Dictionary<string, MethodProxy>();

		private readonly List<string> constUrls = new List<string>();
		private readonly List<string> routeUrls = new List<string>();


		public CustomComponentProxy(object comp, string id)
		{
			realCustomComponent = comp;
			Id = id;
			MethodInfo[] methodInfos = realCustomComponent.GetType().GetMethods(BindingFlags.Public | BindingFlags.DeclaredOnly | BindingFlags.Instance);
			RouteMappingAttribute routeMapping;
			string value;
			foreach (MethodInfo info in methodInfos)
			{
				routeMapping = info.GetCustomAttribute<RouteMappingAttribute>(false);
				if (routeMapping != null)
				{
					value = routeMapping.Value;
					MethodDict.Add(value, new MethodProxy(realCustomComponent, info));
					if (Regex.IsMatch(value, RegexPattern.ConstUrlPattern))
						constUrls.Add(value);
					if (Regex.IsMatch(value, RegexPattern.RouteUrlPattern))
						routeUrls.Add(value);
				}
			}
		}

		public object Invoke(string url, Dictionary<string, string> urlParams, object obj)
		{
			url = url.Trim();
			string target = FindPattern(url);

			if (string.IsNullOrWhiteSpace(target))
				throw new RouteNotMatchException("未匹配方法");

			Dictionary<string, string> routeParams = ExtractRouteParams(url, target);

			object o = MethodDict[target].Invoke(urlParams, routeParams, obj);
			return o;
		}

		private Dictionary<string, string> ExtractRouteParams(string url, string target)
		{
			if (!Regex.IsMatch(target, RegexPattern.RouteParamPattern))
				return null;

			url = url.IndexOf('/', url.Length - 1) < 0 ? url + "/" : url;//最后一位补一个/
			target = target.IndexOf('/', target.Length - 1) < 0 ? target + "/" : url;//最后一位补一个/

			MatchCollection mc = Regex.Matches(target, RegexPattern.RouteParamPattern);
			Dictionary<string, string> routeParamDict = new Dictionary<string, string>();

			string key, value;
			int priorPosition = target.IndexOf('{');//存放上一个"/"之后的位置
			int length;
			foreach (Match match in mc)
			{
				if (match.Success)//迭代
				{
					key = match.Value.Substring(1, match.Value.Length - 2);
					length = url.IndexOf('/', priorPosition) - priorPosition;
					value = url.Substring(priorPosition, length);//每个匹配的"{"位置，找到其后第一个"/"位置，截取中间的部分
					routeParamDict.Add(key, value);
					priorPosition = url.IndexOf('/', priorPosition) + 1;
				}
			}
			return routeParamDict;
		}

		private string FindPattern(string url)
		{
			string target = null;
			string path = constUrls.Where(item => item == url).FirstOrDefault();
			if (string.IsNullOrWhiteSpace(path))
			{
				string[] urlSegment = url.Split('/');
				foreach (string item in routeUrls)
				{
					string[] pathSegment = item.Split('/');
					if (urlSegment.Length != pathSegment.Length)
						continue;
					int index = item.IndexOf('{');
					if (url.Substring(0, index) == item.Substring(0, index))
					{
						target = item;
						break;
					}
				}
			}
			else
			{
				target = path;
			}
			return target;
		}
	}
}

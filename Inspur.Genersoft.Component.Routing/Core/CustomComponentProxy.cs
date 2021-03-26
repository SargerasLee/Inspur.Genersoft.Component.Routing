using Inspur.Genersoft.Component.Routing.Attributes;
using Inspur.Genersoft.Component.Routing.Exceptions;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Inspur.Genersoft.Component.Routing.Core
{
	internal class CustomComponentProxy
	{
		public string Id { get; private set; }

		private readonly object realCustomComponent;
		private readonly Dictionary<string, MethodProxy> MethodDict = new Dictionary<string, MethodProxy>();

		private readonly List<string> constUrls = new List<string>();
		private readonly List<string> routeUrls = new List<string>();

		private const string constUrlPattern = "(/\\w+)+";
		private const string routeUrlPattern = "(/\\w+)*(/{\\w+})+";
		private const string routeParamPattern = "{\\w+}";

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
					if (Regex.IsMatch(value, constUrlPattern))
						constUrls.Add(value);
					if (Regex.IsMatch(value, routeUrlPattern))
						routeUrls.Add(value);
				}
			}
		}

		public object Invoke(string route, params object[] objs)
		{
			route = route.Trim();
			string url = route, kv = string.Empty;

			SplitUrl(route, ref url, ref kv);

			string target = FindPattern(url);
			if (string.IsNullOrWhiteSpace(target))
				throw new RouteNotMatchException("未匹配方法");

			Dictionary<string, string> urlParams = ExtractUrlParams(kv);
			Dictionary<string, string> routeParams = ExtractRouteParams(url, target);

			object o = MethodDict[target].Invoke(urlParams, routeParams, objs);
			return o;
		}

		private Dictionary<string, string> ExtractRouteParams(string url, string target)
		{
			if (!Regex.IsMatch(target, routeParamPattern))
				return null;

			url = url.IndexOf('/', url.Length - 1) < 0 ? url + "/" : url;//最后一位补一个/
			target = target.IndexOf('/', target.Length - 1) < 0 ? target + "/" : url;//最后一位补一个/

			MatchCollection mc = Regex.Matches(target, routeParamPattern);
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

		private static void SplitUrl(string route, ref string url, ref string kv)
		{
			int mark = route.IndexOf('?');
			if (mark > 0)
			{
				url = route.Substring(0, mark);
				if (mark < route.Length - 1)
					kv = route.Substring(mark + 1);
			}
		}

		private static Dictionary<string, string> ExtractUrlParams(string kvStr)
		{
			kvStr = kvStr.Trim();
			if (string.IsNullOrWhiteSpace(kvStr))
				return null;
			string[] keyValues = kvStr.Split('&');
			Dictionary<string, string> urlParams = new Dictionary<string, string>();
			string[] s;
			foreach (string kv in keyValues)
			{
				if (string.IsNullOrWhiteSpace(kv.Trim()))
					continue;
				s = kv.Split('=');
				if (string.IsNullOrWhiteSpace(s[0].Trim()))
					continue;
				urlParams.Add(s[0], s[1]);
			}
			return urlParams;
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

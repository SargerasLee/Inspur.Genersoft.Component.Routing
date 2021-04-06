using Open.Genersoft.Component.Routing.Core;
using Open.Genersoft.Component.Routing.Exceptions;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace Open.Genersoft.Component.Routing.Public.Utils
{
	public class HttpUrlUtil
	{
		public static string Decode(string url)
		{
			if (string.IsNullOrWhiteSpace(url))
				return string.Empty;
			string next = HttpUtility.UrlDecode(url, Encoding.UTF8);
			if (next == url)
				return next;
			else
				return Decode(next);
		}

		public static Dictionary<string, string> GetUrlParams(string rawUrl)
		{
			if (!Regex.IsMatch(rawUrl, RegexPattern.NormalUrlPattern))
				throw new InvalidUrlException("不合法的路径");
			string[] fragments = rawUrl.Split('?');
			Dictionary<string, string> paramDict = new Dictionary<string, string>();
			if (fragments.Length > 1)
			{
				string param = fragments[1];
				param = param.Trim();
				if (string.IsNullOrWhiteSpace(param))
					return paramDict;
				string[] keyValues = param.Split('&');
				string[] s;
				foreach (string kv in keyValues)
				{
					if (string.IsNullOrWhiteSpace(kv.Trim()))
						continue;
					s = kv.Split('=');
					if (string.IsNullOrWhiteSpace(s[0].Trim()))
						continue;
					paramDict.Add(s[0], Decode(s[1]));
				}
			}
			return paramDict;
		}

		public static string GetDecodeUrlNoParam(string rawUrl)
		{
			if (!Regex.IsMatch(rawUrl, RegexPattern.NormalUrlPattern))
				throw new InvalidUrlException("不合法的路径");
			int mark = rawUrl.IndexOf('?');
			if (mark > 0)
			{
				return Decode(rawUrl.Substring(0, mark));
			}
			return Decode(rawUrl);
		}
	}
}

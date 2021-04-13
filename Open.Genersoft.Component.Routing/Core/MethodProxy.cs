using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Open.Genersoft.Component.Routing.Attributes;
using Open.Genersoft.Component.Routing.Entity;
using Open.Genersoft.Component.Routing.Exceptions;
using Open.Genersoft.Component.Routing.Public.Utils;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Open.Genersoft.Component.Routing.Core
{
	internal class MethodProxy
	{
		private readonly object targetObj;
		private readonly MethodInfo methodInfo;
		private readonly Dictionary<int, ParameterInfo> parametersDict = new Dictionary<int, ParameterInfo>();
		private readonly int parametersCount;
		public MethodProxy(object obj, MethodInfo method)
		{
			targetObj = obj;
			methodInfo = method;
			ParameterInfo[] parameters = methodInfo.GetParameters();
			parametersCount = parameters == null || parameters.Length <= 0 ? 0 : parameters.Length;
			foreach (ParameterInfo info in parameters)
			{
				parametersDict[info.Position] = info;
			}
		}

		public object Invoke(Dictionary<string, string> urlParams, Dictionary<string, string> routeParams, object objs)
		{
			object[] paramObjects = null;
			JObject jObject = null;
			if (objs!=null && !string.IsNullOrWhiteSpace(objs.ToString()))
				jObject = JObject.Parse(objs.ToString());
			if (parametersCount > 0)
			{
				paramObjects = new object[parametersCount];
			}
			Dictionary<Type, Dictionary<string, string>> paramDict = new Dictionary<Type, Dictionary<string, string>>
			{
				{ typeof(UrlParamAttribute), urlParams },
				{ typeof(RouteParamAttribute), routeParams }
			};

			bool hasParam = urlParams != null && urlParams.Count > 0 || routeParams != null && routeParams.Count > 0;
			for (int i = 0; i < parametersCount; i++)
			{
				if (parametersDict[i].ParameterType == typeof(RouteContext))
				{
					paramObjects[i] = new RouteContext(urlParams, routeParams, objs);
					continue;
				}

				if (hasParam)
				{
					AssembleParam<UrlParamAttribute>(paramDict, paramObjects, i);
					AssembleParam<RouteParamAttribute>(paramDict, paramObjects, i); 
				}

				if(jObject!=null && jObject.GetValue(parametersDict[i].Name)!=null)
				{
					Type t = parametersDict[i].ParameterType;
					paramObjects[i] = JsonConvert.DeserializeObject(jObject[parametersDict[i].Name].ToString(), t);
				}				
			}

			CustomComponentResult result = new CustomComponentResult();
			try
			{
				result.Data = methodInfo.Invoke(targetObj, paramObjects);
				result.MediaType = "text/plain";
			}
			catch (TargetInvocationException tex)//反调调用的方法 抛出的异常被封装在TargetInvocationException 里
			{
				string msg = $"类名：{methodInfo.DeclaringType.FullName}，方法名：{methodInfo.Name}";
				if (tex.InnerException is BusinessLogicException)
					throw tex.InnerException;

				throw new TargetInvocationException(msg, tex.InnerException);
			}
			JsonAttribute jsonAttribute = methodInfo.GetCustomAttribute<JsonAttribute>(false);
			if (jsonAttribute != null)
			{
				result.Data = JsonConvert.SerializeObject(result.Data, Formatting.Indented);
				result.MediaType = "application/json";
			}
			return result;
		}

		private void AssembleParam<T>(Dictionary<Type, Dictionary<string, string>> paramDict, object[] paramObjects, int i) where T : ParameterAttribute
		{
			ParameterAttribute paramAttr = parametersDict[i].GetCustomAttribute<T>(false);
			if (paramAttr != null)
			{
				string paramName = paramAttr.Name;
				if (!string.IsNullOrWhiteSpace(paramName))
				{
					paramObjects[i] = TypeConvertUtil.ConvertToBasicType(parametersDict[i].ParameterType, paramDict[paramAttr.GetType()][paramName]);
				}
				else
				{
					paramObjects[i] = TypeConvertUtil.ConvertToBasicType(parametersDict[i].ParameterType, paramDict[paramAttr.GetType()][parametersDict[i].Name]);
				}
			}
		}
	}
}

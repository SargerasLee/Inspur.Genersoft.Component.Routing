using Open.Genersoft.Component.Routing.Attributes;
using Open.Genersoft.Component.Routing.Exceptions;
using Newtonsoft.Json;
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

		public object Invoke(Dictionary<string, string> urlParams, Dictionary<string, string> routeParams, params object[] objs)
		{
			object[] paramObjects = null;
			RouteContext context = new RouteContext(urlParams, routeParams, objs);
			if (parametersCount > 0)
			{
				paramObjects = new object[parametersCount];
			}
			Dictionary<Type, Dictionary<string, string>> paramDict = new Dictionary<Type, Dictionary<string, string>>();
			paramDict.Add(typeof(UrlParamAttribute), urlParams);
			paramDict.Add(typeof(RouteParamAttribute), routeParams);
			if (urlParams != null && urlParams.Count > 0 || routeParams != null && routeParams.Count > 0)
			{
				for (int i = 0; i < parametersCount; i++)
				{
					if (parametersDict[i].ParameterType == typeof(RouteContext))
					{
						paramObjects[i] = context;
						continue;
					}

					AssembleParam<UrlParamAttribute>(paramDict, paramObjects, i);
					AssembleParam<RouteParamAttribute>(paramDict, paramObjects, i);
				}
			}

			object obj;
			try
			{
				obj = methodInfo.Invoke(targetObj, paramObjects);
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
				obj = JsonConvert.SerializeObject(obj, Formatting.Indented);
			return obj;
		}

		private void AssembleParam<T>(Dictionary<Type, Dictionary<string, string>> paramDict, object[] paramObjects, int i) where T : ParameterAttribute
		{
			ParameterAttribute paramAttr = parametersDict[i].GetCustomAttribute<T>(false);
			if (paramAttr != null)
			{
				string paramName = paramAttr.Name;
				if (!string.IsNullOrWhiteSpace(paramName))
				{
					paramObjects[i] = ConvertParamToBasicType(parametersDict[i].ParameterType, paramDict[paramAttr.GetType()][paramName]);
				}
				else
				{
					paramObjects[i] = ConvertParamToBasicType(parametersDict[i].ParameterType, paramDict[paramAttr.GetType()][parametersDict[i].Name]);
				}
			}
		}

		private object ConvertParamToBasicType(Type t, string value)
		{

			if (t == typeof(int)) return Convert.ToInt32(value);
			else if (t == typeof(long)) return Convert.ToInt64(value);
			else if (t == typeof(short)) return Convert.ToInt16(value);
			else if (t == typeof(decimal)) return Convert.ToDecimal(value);
			else if (t == typeof(bool)) return Convert.ToBoolean(value);
			else if (t == typeof(DateTime)) return Convert.ToDateTime(value);
			else if (t == typeof(char)) return Convert.ToChar(value);
			else if (t == typeof(double)) return Convert.ToDouble(value);
			else if (t == typeof(float)) return Convert.ToSingle(value);
			else if (t == typeof(byte)) return Convert.ToByte(value);
			else if (t == typeof(sbyte)) return Convert.ToSByte(value);
			else return value;
		}
	}
}

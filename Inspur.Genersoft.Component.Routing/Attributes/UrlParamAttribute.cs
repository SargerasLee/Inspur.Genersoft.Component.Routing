using System;

namespace Inspur.Genersoft.Component.Routing.Attributes
{
	[AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false, Inherited = false)]
	public class UrlParamAttribute : ParameterAttribute
	{
		public UrlParamAttribute(string name = "")
		{
			Name = name;
		}
	}
}

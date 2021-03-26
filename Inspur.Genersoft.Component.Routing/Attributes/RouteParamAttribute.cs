using System;

namespace Inspur.Genersoft.Component.Routing.Attributes
{
	[AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false, Inherited = false)]
	public class RouteParamAttribute : ParameterAttribute
	{
		public RouteParamAttribute(string name = "")
		{
			Name = name;
		}
	}
}

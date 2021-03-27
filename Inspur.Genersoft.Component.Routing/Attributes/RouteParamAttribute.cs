using System;

namespace Open.Genersoft.Component.Routing.Attributes
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

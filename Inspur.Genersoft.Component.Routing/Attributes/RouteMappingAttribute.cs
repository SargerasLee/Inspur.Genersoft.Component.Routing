using System;

namespace Inspur.Genersoft.Component.Routing.Attributes
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
	public class RouteMappingAttribute : Attribute
	{
		public string Value { get; }

		public RouteMappingAttribute(string value)
		{
			Value = value;
		}
	}
}

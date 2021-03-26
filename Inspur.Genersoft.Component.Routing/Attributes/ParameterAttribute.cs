using System;

namespace Inspur.Genersoft.Component.Routing.Attributes
{
	[AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false, Inherited = false)]
	public class ParameterAttribute : Attribute
	{
		public string Name { get; protected set; }

		public ParameterAttribute(string name = "")
		{
			Name = name;
		}
	}
}

using System;

namespace Inspur.Genersoft.Component.Routing.Attributes
{
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
	public class CustomComponentAttribute : Attribute
	{
		public string Id { get; set; }

		public CustomComponentAttribute(string id = "")
		{
			if (id.Trim() == "")
				Id = Guid.NewGuid().ToString();
			else
				Id = id;
		}
	}
}

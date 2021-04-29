using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Open.Genersoft.Component.Web.Attributes
{
	[AttributeUsage(AttributeTargets.Class,AllowMultiple =false,Inherited =false)]
	public class HttpFilterAttribute : Attribute
	{
		public int Number{ get; private set; }
		public string Name{ get; set; }
		public string Description{ get; set; }
		public HttpFilterAttribute(int number)
		{
			Number = number;
		}
	}
}

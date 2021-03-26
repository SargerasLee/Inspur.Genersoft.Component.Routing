using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inspur.Genersoft.Component.Config.Exceptions
{
	public class ConfigNotFoundException : Exception
	{
		public ConfigNotFoundException(string message) : base(message) { }

		public ConfigNotFoundException(string message, Exception innerException) : base(message, innerException) { }

		public ConfigNotFoundException() : base() { }
	}
}

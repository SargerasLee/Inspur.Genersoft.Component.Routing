using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Open.Genersoft.Component.Routing.Exceptions
{
	public class InvalidUrlException : Exception
	{
		public InvalidUrlException(string message) : base(message) { }

		public InvalidUrlException(string message, Exception innerException) : base(message, innerException) { }

		public InvalidUrlException() : base() { }
	}
}

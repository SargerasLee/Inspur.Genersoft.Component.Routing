using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Open.Genersoft.Component.Routing.Exceptions
{
	public class RouteNotMatchException : Exception
	{
		public RouteNotMatchException(string message) : base(message) { }

		public RouteNotMatchException(string message, Exception innerException) : base(message, innerException) { }

		public RouteNotMatchException() : base() { }
	}
}

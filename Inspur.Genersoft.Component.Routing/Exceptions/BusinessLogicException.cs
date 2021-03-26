using System;

namespace Inspur.Genersoft.Component.Routing.Exceptions
{
	public class BusinessLogicException : Exception
	{
		public BusinessLogicException(string message) : base(message) { }

		public BusinessLogicException(string message, Exception innerException) : base(message, innerException) { }

		public BusinessLogicException() : base() { }
	}
}

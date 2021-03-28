using Open.Genersoft.Component.Logging.Facade;
using System;
namespace Open.Genersoft.Component.Logging.Default
{
	[Obsolete("使用原生log即可",true)]
	public class LoggerDecorator
	{
		private GeneralLogger logger;

		public string UserName { get; set; }
		public string FunctionName{ get; set; }

		public LoggerDecorator(GeneralLogger logger)
		{
			this.logger = logger;
		}


	}
}

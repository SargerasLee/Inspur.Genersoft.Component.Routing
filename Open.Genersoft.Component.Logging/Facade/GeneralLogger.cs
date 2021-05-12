using Open.Genersoft.Component.Logging.Default;
using System;
using System.Collections.Generic;

namespace Open.Genersoft.Component.Logging.Facade
{
	public abstract class GeneralLogger : ILogger
	{
		public string Code{ get; set; }
		public string Name{ get; set; }
		public string DatePattern { get; protected set; }
		public string TimePattern { get; set; }
		public string Path { set; get; }
		public LogLevel Level { set; get; }
		public double Slice{ set; get; }

		protected static readonly Dictionary<LogLevel, string> levelDict = new Dictionary<LogLevel, string>
		{
			{LogLevel.OFF,"OFF" },
			{LogLevel.FATAL,"FATAL" },
			{LogLevel.ERROR,"ERROR" },
			{LogLevel.WARN,"WARN" },
			{LogLevel.INFO,"INFO" },
			{LogLevel.DEBUG,"DEBUG" },
			{LogLevel.TRACE,"TRACE" },
			{LogLevel.ALL,"ALL" }
		};

		public abstract void PrintXml(string desc, string xmlStr);
		public abstract void PrintObject(object obj);
		public abstract void Debug(params string[] text);
		public abstract void Info(params string[] text);
		public abstract void Error(string text, Exception ex = null);
		public abstract void Warn(params string[] text);
		public abstract void Fatal(string text, Exception ex = null);
		public abstract void Trace(params string[] text);
	}
}

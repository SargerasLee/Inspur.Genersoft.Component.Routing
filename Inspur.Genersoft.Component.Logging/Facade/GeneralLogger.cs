using Inspur.Genersoft.Component.Logging.Default;
using System;
using System.Collections.Generic;

namespace Inspur.Genersoft.Component.Logging.Facade
{
	public abstract class GeneralLogger
	{
		public string DatePattern { get; set; }
		public string TimePattern { get; set; }
		public string Directory { set; get; }
		public LogLevel Level { set; get; }

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

		public abstract void LogXml(string desc, string xmlStr);
		public abstract void LogObject(object obj);
		public abstract void Debug(params string[] text);
		public abstract void Info(params string[] text);
		public abstract void Error(string text, Exception ex);
		public abstract void Warn(params string[] text);
	}
}

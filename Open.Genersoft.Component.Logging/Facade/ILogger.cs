using System;

namespace Open.Genersoft.Component.Logging.Facade
{
	public interface ILogger
	{
		void Debug(params string[] text);
		void Info(params string[] text);
		void Error(string text, Exception ex);
		void Warn(params string[] text);
		void Fatal(string text, Exception ex);
		void Trace(params string[] text);
	}
}

using System;

namespace Open.Genersoft.Component.Config.Exceptions
{
	/// <summary>
	/// 此异常用来表示 日志配置未在配置文件找到
	/// </summary>
	public class ConfigNotFoundException : Exception
	{
		public ConfigNotFoundException(string message) : base(message) { }

		public ConfigNotFoundException(string message, Exception innerException) : base(message, innerException) { }

		public ConfigNotFoundException() : base() { }
	}
}

namespace Open.Genersoft.Component.Config.Entity
{
	/// <summary>
	/// 日志配置实体类
	/// </summary>
	public class LogConfig
	{
		/// <summary>
		/// 日志编号，用来在代码里获取日志类
		/// </summary>
		public string Code { get; }
		/// <summary>
		/// 日志名称，放在日志文件开头的部分
		/// </summary>
		public string Name { get; }
		/// <summary>
		/// 日志的路径，不带文件名
		/// </summary>
		public string Path { get; }
		/// <summary>
		/// 日志实现的程序集，不填则启用系统预制
		/// </summary>
		public string Assembly { get; }
		/// <summary>
		/// 日志实现类，不填则启用系统预制
		/// </summary>
		public string Class { get; }
		/// <summary>
		/// 日志级别
		/// </summary>
		public string Level { get; }
		/// <summary>
		/// 日志时间戳 格式 时分秒.毫秒
		/// </summary>
		public string TimePattern{ get; }

		public LogConfig(string code, string name, string path, string assembly,string clazz, string level, string timePattern)
		{
			Code = code;
			Name = name;
			Path = path;
			Assembly = assembly;
			Class = clazz;
			Level = level;
			TimePattern = timePattern;
		}
	}
}

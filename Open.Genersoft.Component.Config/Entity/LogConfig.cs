namespace Open.Genersoft.Component.Config.Entity
{
	public class LogConfig
	{
		public string Code { get; }
		public string Name { get; }
		public string Path { get; }
		public string Assembly { get; }
		public string Class { get; }
		public string Level { get; }
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

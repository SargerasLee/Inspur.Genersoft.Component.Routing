using System;

namespace Open.Genersoft.Component.Config.Events
{
	/// <summary>
	/// 配置文件改变事件
	/// </summary>
	public class FileChangedEvent : EventArgs
	{
		/// <summary>
		/// 配置文件路径
		/// </summary>
		public string ConfigFilePath { get; set; }
		/// <summary>
		/// 配置文件名称
		/// </summary>
		public string ConfigFileName { get; set; }
		/// <summary>
		/// 上一次更新的时间
		/// </summary>
		public DateTime BeforeTime { get; set; }
		/// <summary>
		/// 当前更新的时间
		/// </summary>
		public DateTime UpdateTime { get; set; }
	}
}

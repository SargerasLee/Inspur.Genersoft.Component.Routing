using Open.Genersoft.Component.Config.Events;

namespace Open.Genersoft.Component.Config.Delegates
{
	/// <summary>
	///  配置文件改变事件处理方法的委托
	/// </summary>
	/// <param name="sender">事件发出者</param>
	/// <param name="changedEvent">文件改变事件</param>
	public delegate void ConfigFileChangedEventHandler(object sender, FileChangedEvent changedEvent);
}

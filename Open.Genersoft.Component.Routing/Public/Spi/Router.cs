namespace Open.Genersoft.Component.Routing.Public.Spi
{
	/// <summary>
	/// 路由映射器
	/// </summary>
	public class Router
	{
		/// <summary>
		/// 映射方法
		/// </summary>
		/// <param name="route">url</param>
		/// <param name="objects">序列化后的json参数，键为方法参数名，值为方法参数实体</param>
		/// <returns></returns>
		public static object Routing(string route, object objects)
		{
			return ComponentDispatcher.Instance.Dispatch(route, objects);
		}
	}
}
namespace Open.Genersoft.Component.Routing.Public.Spi
{
	public class Router
	{
		public static object Routing(string route, params object[] objects)
		{
			return ComponentDispatcher.Instance.Dispatch(route, objects);
		}
	}
}

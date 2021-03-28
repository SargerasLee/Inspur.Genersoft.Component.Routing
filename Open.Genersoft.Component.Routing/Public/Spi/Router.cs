namespace Open.Genersoft.Component.Routing.Public.Spi
{
	public class Router
	{
		public static object Routing(string route, object objects)
		{
			return ComponentDispatcher.Instance.Dispatch(route, objects);
		}
	}
}

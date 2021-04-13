using Open.Genersoft.Component.Config.Global;
using Open.Genersoft.Component.Routing.Public.Spi;
using Open.Genersoft.Component.Web.Handler;
using System;
using System.IO;
using System.Text;
using System.Web;
using System.Web.UI;

namespace Open.Genersoft.Component.Web
{
	public class RequestInterceptor : IHttpModule
	{
		private static readonly CustomHandler handler = new CustomHandler();
		/// <summary>
		/// 您将需要在网站的 Web.config 文件中配置此模块
		/// 并向 IIS 注册它，然后才能使用它。有关详细信息，
		/// 请参阅以下链接: https://go.microsoft.com/?linkid=8101007
		/// </summary>
		#region IHttpModule 成员

		public void Dispose()
		{
			//此处放置清除代码。
		}

		public void Init(HttpApplication context)
		{
			// 下面是如何处理 LogRequest 事件并为其 
			// 提供自定义日志记录实现的示例
			context.PostResolveRequestCache += RequestHandler;
		}

		private void RequestHandler(object sender, EventArgs e)
		{
			HttpApplication httpApplication = (HttpApplication)sender;
			if(httpApplication.Request.RawUrl.IndexOf("/") > 0)
			httpApplication.Context.RemapHandler(handler);
			
		}

		#endregion

	}
}

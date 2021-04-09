using Open.Genersoft.Component.Routing.Public.Spi;
using System;
using System.Text;
using System.Web;
using System.Web.UI;

namespace Open.Genersoft.Component.Web
{
	public class RequestInterceptor : IHttpModule
	{
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
			context.MapRequestHandler += RequestHandler;
		}

		private void RequestHandler(object sender, EventArgs e)
		{
			HttpApplication httpApplication = (HttpApplication)sender;
			//httpApplication.Context.RemapHandler()
			HttpRequest request = httpApplication.Context.Request;
			HttpResponse response = httpApplication.Context.Response;
			response.ContentType = "application/json";
			string str = string.Empty;
			object result;
			if(request.HttpMethod=="POST")
			{
				byte[] b = new byte[request.ContentLength];
				int length = request.InputStream.Read(b, 0, request.ContentLength);
				if (length != request.ContentLength)
					throw new HttpException("请求数据缺失");
				str = Encoding.UTF8.GetString(b);
			}
			result = Router.Routing(request.RawUrl, str);
			response.Write(result);
		}

		#endregion

	}
}

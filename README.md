# Open.Genersoft.Component.Routing
## 说明
包含**日志**,**配置**和**路由映射**功能
+ 只使用日志 需要 Config和Logging两个dll
+ 只使用路由映射 需要Config 和Routing两个dll
+ 单独使用Config，确保Config.dll被加载到内存中
	1. 如果是自己写的桌面程序，需要添加Config引用
	2. 如果在IIS ，需要放到应用程序bin下
## 使用范围
+ `Config`和Logging可以无条件使用
+ `Routing`只是一个组件，不是完整的框架，实际使用需要自己注册`IHttpModule`，然后拦截请求进入Router处理
## 全局配置文件
### 程序集 `Open.Genersoft.Component.Config`
### 核心类
#### ProjectConfigContainer
命名空间：`Open.Genersoft.Component.Config.Global`
程序集：`Open.Genersoft.Component.Config.dll`
该类为静态类，当第一次访问类中的方法时，会读取配置文件并完成初始化功能，根据当前文件修改时间与上次修改时间判断需不需要再次加载。
##### 方法
+ GetProperty(string code)
读取配置文件中的属性
 
+ GetAutoScanAssemblies()
读取配置文件中的程序集
 
+ GetLogConfig(string code)
读取日志配置
 
#### 配置文件模板
目前没有xsd约束，需要放在程序安装目录下+`/zzy/Global/`下
```xml
<?xml version="1.0" encoding="utf-8" ?>
<Configuration>
  <Modules>
    <Properties>
      <Property Code="zj" Value="interface" Name=""/>
      <Property Code="fy" Value="api" Name=""/>
    </Properties>
    <Component-Scan>
      <Assembly Name="Open.Genersoft.Component.UnitTest, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"/>
    </Component-Scan>
    <Log>
      <Type Code="ZJ" Name="资金日志" Path="c:\log\zjlog\" TimePattern="HH : mm : ss : ffffff" Assembly="" Class="" Level="ALL"/>
      <Type Code="YS" Name="应收日志" Path="c:\log\arlog\" TimePattern="" Assembly="" Class="" Level="DEBUG"/>
    </Log>
  </Modules>
</Configuration>

<!--Code 日志编号,不要用default，系统已预置，可直接使用-->
<!--Name 日志名称，给人看-->
<!--FullPath 日志文件路径，不带文件名-->
<!--Assembly 程序集强名称，带公钥-->
<!--Class 类全限定名-->
<!--Level 日志级别由低到高ALL,TRACE,DEBUG,INFO,WARN,ERROR,FATAL,OFF-->
```

 
## Routing
### 程序集 `Open.Genersoft.Component.Routing`
### 核心
#### Router调度器
命名空间：`Open.Genersoft.Component.Routing.Public.Spi`
定义
```c#
public class Router
{
	public static object Routing(string route, object objects)
	{
		return ComponentDispatcher.Instance.Dispatch(route, objects);
	}
}
```
使用：Routing方法
route 为路径
objects 为 序列化后的json对象，json格式
```json
{
	"方法参数1":"数组/字符串/数字/json/布尔/null",
	"方法参数2":"数组/字符串/数字/json/布尔/null"
}
```
参数I 的名称对应方法参数名称，会自动转换类型，见示例
 
### 异常
+ `BusinessLogicException`
+ `RouteNotMatchException`
 
### 可使用的特性
使用时 不需要加Attribute后缀 ，例如
```c#
[CustomComponent]
public class helper
{
}
```
+ `CustomComponentAttribute`
只能用在类上，标识 此类是自定义组件，会被扫描进组件容器
+ `JsonAttribute`
用在方法上，标识 方法返回值 要序列化为 json串
+ `RouteMappingAttribute`
用在类和方法上，标识 映射的路径，类上不可以使用路由参数，方法上可以使用路由参数
+ `RouteParamAttribute`
路由参数，用在方法参数上，标识此方法参数需要取路由中的值，会将url中的路由参数 赋给方法参数，可以 起别名，没有别名默认按参数名称匹配
+ `UrlParamAttribute`
url参数：url问号 后面的参数，用在方法上，和路由参数用法相似，也可以起别名
+ 上下文参数`RouteContext`
内含 url，route参数字典，和前台传进来的 json参数
 
### 配置
需要配置 自动扫描的程序集，见[全局配置文件](#配置文件模板)
### 示例
#### 前端
不提供前端代码，如要使用，使用`$.get()`或者`$.post()`
#### 后端
使用参考：建一个ashx文件， 调用此调度器。
```c#
namespace Test.Base.Component
{
	public class BusinessLogic : IHttpHandler
	{
		public void ProcessRequest(HttpContext context)
		{
			//验证。登录根据实际情况添加
			string rawUrl = context.Request.RawUrl;
			byte[] b = new byte[context.Request.ContentLength];
			int count = context.Request.InputStream.Read(b, 0, b.Length);
			string objs = Encoding.UTF8.GetString(b);
			object res = MethodMapping();
			string data = JsonConvert.SerializeObject(res);
			context.Response.Charset = "utf-8";
			context.Response.ContentEncoding = Encoding.UTF8;
			context.Response.ContentType = "application/json";
			context.Response.Write(data);
			
		}
		private object MethodMapping(string route, string objs)
		{
			try
			{
				return Router.Routing(route, objs);
			}
			catch (BusinessLogicException e)
			{
				Dictionary<string, string> res = new Dictionary<string, string>
				{
					{"flag","e" },
					{"msg",e.Message }
				};
				return res;
			}
		}
		
		public bool IsReusable
		{
			get
			{
				return true;
			}
		}
	}
}
```
```c#
namespace Component
{
	[CustomComponent]
	[RouteMapping("/robxdj")]
	public class PlfkdLogic
	{
		//private static List<Plfkd> plfkds = GetPLFKDList();
		private readonly Database db = DbFactory.getConnection();
		private readonly GeneralLogger logger = LoggerFactory.Instance.GetLogger("default");

		[RouteMapping("/query/{lx}/{nm}")]
		[Json]
		public DataTable SelectBill([RouteParam] string lx, [RouteParam] string nm)
		{
			string sql = "select * from saleorders where lx={0} and id={1}";
			logger.Trace(sql);
			return db.Select(sql, djlx, djnm).Tables[0];
		}

		[RouteMapping("/save")]
		[Json]
		public string SaveBill()
		{
			try
			{
				throw new BusinessLogicException("哎呀");
			}
			catch (Exception e)
			{
				logger.Error("异常", e);
				throw e;
			}
		}

		[RouteMapping("/all/{id}/{code}")]
		[Json]
		public RouteContext GetAll(RouteContext context)
		{
			return context;
		}

		[RouteMapping("/delete/{id}")]
		[Json]
		public int DeleteBill([RouteParam] string id)
		{
			string sql = $"delete from saleorders where nm='{id}'";
			logger.Debug(sql);
			return db.Delete(sql);
		}

		private static List<Plfkd> GetPLFKDList()
		{
			List<Plfkd> list = new List<Plfkd>();
			for (int i = 0; i < 10; i++)
			{
				Plfkd p = new Plfkd
				{
					ID = i.ToString(),
					DWBH = "1" + i,
					BMBH = "10" + i,
					BZR = "lch" + i,
					DJBH = "20210303" + i,
					JE = 10m + i
				};
				list.Add(p);
			}
			return list;
		}
	}

	public class Plfkd : IEquatable<Plfkd>
	{
		public string ID { get; set; }
		public string DJBH { get; set; }
		public string DWBH { get; set; }
		public string BMBH { get; set; }
		public decimal JE { get; set; }
		public string BZR { get; set; }


		public override bool Equals(object obj)
		{
			Plfkd t = obj as Plfkd;
			return ID == t.ID;
		}

		public bool Equals(Plfkd other)
		{
			return ID == other.ID;
		}

		public override int GetHashCode()
		{
			return ID.GetHashCode();
		}
	}
}
```
## Logging日志
### 程序集 `Open.Genersoft.Component.Logging`
### 核心
#### `LoggerFactory`
##### 命名空间：`Open.Genersoft.Component.Logging.Factory`
用法：对应配置文件 `GetLogger(string code)` 
code对应配置的Code,
其中Code Path Level 必填，其他选填
 
#### `GeneralLogger`
##### 命名空间：`Open.Genersoft.Component.Logging.Facade`
日志抽象类公开方法如下
```c#
public abstract void PrintXml(string desc, string xmlStr);

public abstract void PrintObject(object obj);

public abstract void Debug(params string[] text);

public abstract void Info(params string[] text);

public abstract void Error(string text, Exception ex = null);

public abstract void Warn(params string[] text);

public abstract void Fatal(string text, Exception ex = null);

public abstract void Trace(params string[] text);
```
##### 用法
`private readonly GeneralLogger logger = LoggerFactory.Instance.GetLogger("default");`
## Dll
### 依赖关系
+ Config.dll不依赖
+ Routing和Logging都依赖Config 和Newtonsoft （版本6.0.1）
    

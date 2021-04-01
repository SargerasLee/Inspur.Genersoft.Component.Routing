# Open.Genersoft.Component.Routing
## 说明
包含 日志 ，配置， 和 路由映射 功能
只使用日志 需要 Config和Logging两个dll
只使用路由映射 需要Config 和Routing两个dll

-单独使用Config，确保Config.dll被加载到内存中，使用gs7不用看此段
1如果是自己写的桌面程序，需要添加Config引用
2如果在IIS ，需要放到应用程序bin下
## 使用范围
Config和Logging全局可以使用
Routing用于前台调用后台，如果是自动任务建议用平台的 方法构件，只是建议
## 全局配置文件
### 程序集 `Open.Genersoft.Component.Config`
### 核心类
#### ProjectConfigContainer
命名空间：`Open.Genersoft.Component.Config.Global`
程序集：`Open.Genersoft.Component.Config.dll`
该类为静态类，当第一次访问类中的方法时，会读取配置文件并完成初始化功能，根据当前文件修改时间与上次修改时间判断需不需要再次加载。
##### 方法
##### GetProperty(string code)
读取配置文件中的属性
 
##### GetAutoScanAssemblies()
读取配置文件中的程序集
 
##### GetLogConfig(string code)
读取日志配置
 
#### 配置文件模板
目前没有xsd约束，需要放在程序安装目录下`bscw_local/zzy/Global/`下（gs7为`bscw_local/zzy/Global/`下）

 
## Routing
### 程序集 `Open.Genersoft.Component.Routing`
### 核心
#### Router调度器
命名空间：`Open.Genersoft.Component.Routing.Public.Spi`
定义
 
使用：Routing方法
route 为路径
objects 为 序列化后的json对象，json格式
```
{
	参数1:数组/字符串/数字/json对象/布尔/null
	参数2: 数组/字符串/数字/json对象/布尔/null
}
```
参数I 的名称对应方法参数名称，会自动转换类型，见示例
 
### 异常
#### `BusinessLogicException`
#### `RouteNotMatchException`
 
### 可使用的特性
使用时 不需要加Attribute后缀 ，例如
```
[CustomComponent]
public class helper
{

}
```
#### `CustomComponentAttribute`
只能用在类上，标识 此类是自定义组件，会被扫描进组件容器
#### `JsonAttribute`
用在方法上，标识 方法返回值 要序列化为 json串
#### `RouteMappingAttribute`
用在类和方法上，标识 映射的路径，类上不可以使用路由参数，方法上可以使用路由参数
#### `RouteParamAttribute`
路由参数，用在方法参数上，标识此方法参数需要取路由中的值，会将url中的路由参数 赋给方法参数，可以 起别名，没有别名默认按参数名称匹配
#### `UrlParamAttribute`
url参数：url问号 后面的参数，用在方法上，和路由参数用法相似，也可以起别名
#### 上下文参数`RouteContext`
内涵 url，route参数字典，和前台传进来的 json参数
 
### 配置
需要配置 自动扫描的程序集，见全局配置文件
### 示例
使用参考：整个项目可以建一个业务逻辑构件 调用此调度器。
自己建的自定义构件 业务逻辑错误可以抛出 `BusinessLogicException`异常。在最外层捕获并抛出 GSPException
#### 前端
`Fetch 见ZZY_FSSC_Common`脚本
#### 后端
## Logging日志
### 程序集 `Open.Genersoft.Component.Logging`
### 核心
#### `LoggerFactory`
命名空间：`Open.Genersoft.Component.Logging.Factory`
用法：
对应配置文件 `GetLogger(code)` code对应配置的Code
Code Path Level 必填，其他选填
 
#### `GeneralLogger`
命名空间：`Open.Genersoft.Component.Logging.Facade`
日志抽象类
公开方法如下
## Dll
依赖关系
Config.dll不依赖
Routing和Logging都依赖Config 和Newtonsoft （版本6.0.1）
    

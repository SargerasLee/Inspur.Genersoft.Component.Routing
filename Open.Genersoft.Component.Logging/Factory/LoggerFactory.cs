using Open.Genersoft.Component.Config.Entity;
using Open.Genersoft.Component.Config.Global;
using Open.Genersoft.Component.Logging.Default;
using Open.Genersoft.Component.Logging.Facade;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Open.Genersoft.Component.Logging.Factory
{
	public class LoggerFactory
	{
		private static readonly object o = new object();//公共对象锁
		private static readonly Dictionary<string, GeneralLogger> logDict = new Dictionary<string, GeneralLogger>();
		private static readonly Dictionary<string, LogLevel> levelDict = new Dictionary<string, LogLevel>
		{
			{"OFF",LogLevel.OFF },
			{"FATAL",LogLevel.FATAL },
			{"ERROR",LogLevel.ERROR },
			{"WARN",LogLevel.WARN },
			{"INFO",LogLevel.INFO },
			{"DEBUG",LogLevel.DEBUG },
			{"TRACE",LogLevel.TRACE },
			{"ALL",LogLevel.ALL }
		};
		private static readonly string DefaultClass = typeof(DefaultLogger).FullName;
		private static readonly string DefaultAssembly = typeof(DefaultLogger).Assembly.FullName;
		private const string DefaultTimePattern = "HH : mm : ss : fff";
		private const string DefaultLevel = "ALL";
		private const string DefaultCode = "default";
		private const string DefaultName = "default";
		private const string DefaultPath = "c:\\log\\default\\";

		private LoggerFactory() { }

		public static LoggerFactory Instance { get; } = new LoggerFactory();

		private GeneralLogger Bulid(string code)
		{
			Dictionary<string, string> module = GetModuleConfig(code);
			GeneralLogger logger = CreateLogger(module);
			PutCache(code, logger);
			return logDict[code];
		}

		/// <summary>
		/// 获取日志类
		/// </summary>
		/// <param name="code">日志编号</param>
		/// <returns></returns>
		public GeneralLogger GetLogger(string code)
		{
			if (logDict.ContainsKey(code))
			{
				return logDict[code];
			}
			else
			{
				return Bulid(code);
			}
		}
		private Dictionary<string, string> GetModuleConfig(string code)
		{
			Dictionary<string, string> module = new Dictionary<string, string>(7);
			LogConfig config = ProjectConfigContainer.GetLogConfig(code);
			module["code"] = string.IsNullOrWhiteSpace(config.Code) ? DefaultCode : config.Code;
			module["name"] = string.IsNullOrWhiteSpace(config.Name) ? DefaultName : config.Name;
			module["path"] = string.IsNullOrWhiteSpace(config.Path) ? DefaultPath : config.Path;
			module["className"] = string.IsNullOrWhiteSpace(config.Class) ? DefaultClass : config.Class;
			module["assembly"] = string.IsNullOrWhiteSpace(config.Assembly) ? DefaultAssembly : config.Assembly;
			module["logLevel"] = string.IsNullOrWhiteSpace(config.Level) ? DefaultLevel : config.Level;
			module["timePattern"] = string.IsNullOrWhiteSpace(config.TimePattern) ? DefaultTimePattern : config.TimePattern;

			return module;
		}

		private GeneralLogger CreateLogger(Dictionary<string, string> module)
		{
			Type t = Assembly.Load(module["assembly"].ToString()).GetType(module["className"].ToString());
			GeneralLogger logger = Activator.CreateInstance(t) as GeneralLogger;
			logger.Code = module["code"];
			logger.Name = module["name"];
			logger.Path = module["path"].EndsWith("\\") ? module["path"] : module["path"] + "\\";
			logger.Level = levelDict[module["logLevel"]];
			logger.TimePattern = module["timePattern"];
			return logger;
		}


		private void PutCache(string code, GeneralLogger logger)
		{
			if (!logDict.ContainsKey(code))
			{
				lock (o)
				{
					if (!logDict.ContainsKey(code))
					{
						logDict.Add(code, logger);
					}
				}
			}
		}
	}
}

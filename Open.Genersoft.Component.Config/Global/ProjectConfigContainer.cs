using Open.Genersoft.Component.Config.Delegates;
using Open.Genersoft.Component.Config.Entity;
using Open.Genersoft.Component.Config.Events;
using Open.Genersoft.Component.Config.Exceptions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace Open.Genersoft.Component.Config.Global
{
	/// <summary>
	/// 配置容器类
	/// </summary>
	public static class ProjectConfigContainer
	{
		public static event ConfigFileChangedEventHandler ConfigFileChanged;
		private static readonly object obj = new object();
		private static readonly string PATH = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "\\Global\\project-config.xml";
		private static bool flag = false;
		private static DateTime lastModifyTime = DateTime.MinValue;

		private static readonly Dictionary<string, string> Properties = new Dictionary<string, string>();
		private static readonly List<string> Assemblies = new List<string>();
		private static readonly Dictionary<string, LogConfig> LoggerConfig = new Dictionary<string, LogConfig>();


		private static void Load()
		{
			if (!flag)
			{
				XmlDocument xd = new XmlDocument();
				lastModifyTime = File.GetLastWriteTime(PATH);//GSPContext.Current.ServerInstallPath
				xd.Load(PATH);
				PutProperties(xd);
				PutAssemblies(xd);
				PutLogConfig(xd);
				flag = true;
			}
		}

		private static void PutLogConfig(XmlDocument xd)
		{
			XmlNodeList list = xd.SelectNodes("/Configuration/Modules/Log/Type");
			LoggerConfig.Clear();
			foreach (XmlNode item in list)
			{
				LoggerConfig.Add(item.Attributes["Code"].Value, new LogConfig
				(
					item.Attributes["Code"] == null ? "":item.Attributes["Code"].Value,
					item.Attributes["Name"] == null ? "" : item.Attributes["Name"].Value,
					item.Attributes["Path"] == null ? "" : item.Attributes["Path"].Value,
					item.Attributes["Assembly"] == null ? "" : item.Attributes["Assembly"].Value,
					item.Attributes["Class"] == null ? "" : item.Attributes["Class"].Value,
					item.Attributes["Level"] == null ? "" : item.Attributes["Level"].Value,
					item.Attributes["TimePattern"] == null ? "" : item.Attributes["TimePattern"].Value,
					item.Attributes["Slice"] == null ? 10.0 : Convert.ToDouble(item.Attributes["Slice"].Value)
				));
			}
			LoggerConfig.Add("default", new LogConfig("", "", "", "", "", "", "",10.0));
		}

		private static void PutAssemblies(XmlDocument xd)
		{
			XmlNodeList assembliesList = xd.SelectNodes("/Configuration/Modules/Component-Scan/Assembly");
			Assemblies.Clear();
			foreach (XmlNode item in assembliesList)
			{
				Assemblies.Add(item.Attributes["Name"].Value);
			}
		}

		private static void PutProperties(XmlDocument xd)
		{
			XmlNodeList list = xd.SelectNodes("/Configuration/Modules/Properties/Property");
			Properties.Clear();
			foreach (XmlNode item in list)
			{
				Properties.Add(item.Attributes["Code"].Value, item.Attributes["Value"].Value);
			}
		}

		/// <summary>
		/// 获取键值对
		/// </summary>
		/// <param name="code">键</param>
		/// <returns></returns>
		public static string GetProperty(string code)
		{
			ReloadIfFileChanged();
			if (Properties.ContainsKey(code))
				return Properties[code];
			else
				throw new ConfigNotFoundException("未找到相应的属性配置");
		}

		/// <summary>
		/// 获取配置的需要扫描的程序集
		/// </summary>
		/// <returns></returns>
		public static List<string> GetAutoScanAssemblies()
		{
			ReloadIfFileChanged();
			return Assemblies;
		}

		/// <summary>
		/// 获取日志配置类
		/// </summary>
		/// <param name="code">日志编号</param>
		/// <returns></returns>
		public static LogConfig GetLogConfig(string code)
		{
			ReloadIfFileChanged();
			if (LoggerConfig.ContainsKey(code))
				return LoggerConfig[code];
			else
				throw new ConfigNotFoundException("未找到相应的日志配置");
		}

		private static void ReloadIfFileChanged()
		{
			if (File.GetLastWriteTime(PATH) > lastModifyTime)
			{
				lock (obj)
				{
					if (File.GetLastWriteTime(PATH) > lastModifyTime)
					{
						DateTime before = lastModifyTime;
						flag = false;
						Load();
						ConfigFileChanged?.Invoke(typeof(ProjectConfigContainer), new FileChangedEvent
						{
							ConfigFileName = "ProjectGlobalConfig.xml",
							ConfigFilePath = PATH,
							BeforeTime = before,
							UpdateTime = lastModifyTime
						});
					}
				}
			}
		}
	}
}

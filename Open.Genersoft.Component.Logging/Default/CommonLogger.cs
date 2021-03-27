using Open.Genersoft.Component.Logging.Facade;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using System.Xml;

namespace Open.Genersoft.Component.Logging.Default
{
	public class CommonLogger : GeneralLogger
	{
		private readonly object lockObj = new object();
		private StreamWriter writer;
		private string processName;
		public CommonLogger()
		{
			DatePattern = "yyyy-MM-dd";
			TimePattern = "HH : mm : ss : fff";
			processName = Process.GetCurrentProcess().ProcessName;
		}

		public override void Debug(params string[] text)
		{
			if (Level > LogLevel.DEBUG) return;
			Log(LogLevel.DEBUG, text);
		}

		public override void Error(string text, Exception ex = null)
		{
			if (Level > LogLevel.ERROR) return;
			if (ex != null)
				Log(LogLevel.ERROR, text, ex.Message, ex.StackTrace);
			else
				Log(LogLevel.ERROR, text);
		}

		public override void Info(params string[] text)
		{
			if (Level > LogLevel.INFO) return;
			Log(LogLevel.INFO, text);
		}

		public override void Warn(params string[] text)
		{
			if (Level > LogLevel.WARN) return;
			Log(LogLevel.WARN, text);
		}


		private void Log(LogLevel level, params string[] text)
		{
			if (Level == LogLevel.OFF) return;
			StringBuilder sb = new StringBuilder(200);
			foreach (var s in text)
			{
				sb.Append(s);
				sb.Append(Environment.NewLine);
			}

			try
			{
				CreateFileIfNotExists();
				bool token = false;
				Monitor.TryEnter(lockObj, 500, ref token);
				if (token)
				{
					string date = DateTime.Now.ToString(DatePattern);
					string p = Directory + $"Log{date}.txt";
					using (writer = new StreamWriter(p, true, Encoding.Default))
					{
						string time = DateTime.Now.ToString(TimePattern);
						writer.AutoFlush = false;
						writer.WriteLine(time + $":【{ levelDict[level]}】");
						writer.WriteLine("【进程】：" + processName);
						writer.WriteLine("【线程ID】：" + Thread.CurrentThread.ManagedThreadId);
						writer.WriteLine("【信息】：" + sb);
						writer.Flush();
					}
					Monitor.Exit(lockObj);
				}
				else
				{
					throw new TimeoutException("获得锁超时");
				}
			}
			catch (Exception) { }
		}

		public override void LogObject(object obj)
		{
			Log(Level, JsonConvert.SerializeObject(obj, Newtonsoft.Json.Formatting.Indented));
		}

		/// <summary>
		///  xml打印
		/// </summary>
		/// <param name="desc"></param>
		/// <param name="doc"></param>
		public override void LogXml(string desc, string xmlStr)
		{
			XmlDocument doc = new XmlDocument();
			doc.LoadXml(xmlStr);
			StringBuilder sb = new StringBuilder();
			StringWriter sw = new StringWriter(sb);
			using (XmlTextWriter writer = new XmlTextWriter(sw))
			{
				writer.Indentation = 4;  // 缩进个数
				writer.IndentChar = ' ';  // 缩进字符
				writer.Formatting = System.Xml.Formatting.Indented;
				doc.WriteTo(writer);
			}
			Log(Level, desc, sb.ToString());
		}

		private void CreateFileIfNotExists()
		{
			if (!System.IO.Directory.Exists(Directory))
			{
				Console.WriteLine("没有这个路径");
				System.IO.Directory.CreateDirectory(Directory);
			}
		}
	}
}

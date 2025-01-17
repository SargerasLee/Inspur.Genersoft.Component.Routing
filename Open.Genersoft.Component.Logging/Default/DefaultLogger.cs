﻿
using Newtonsoft.Json;
using Open.Genersoft.Component.Logging.Facade;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using System.Xml;
using System.Reflection;

namespace Open.Genersoft.Component.Logging.Default
{
	public class DefaultLogger : GeneralLogger
	{
		private readonly object lockObj = new object();
		private StreamWriter writer;
		private readonly string processName;
		private string currentFile = "";
		private int num = 0;

		public DefaultLogger()
		{
			DatePattern = "yyyy-MM-dd";
			processName = Process.GetCurrentProcess().ProcessName;
		}

		public override void Debug(params string[] text)
		{
			if (Level > LogLevel.DEBUG) return;
			Print(LogLevel.DEBUG, text);
		}

		public override void Error(string text, Exception ex = null)
		{
			if (Level > LogLevel.ERROR) return;
			if (ex != null)
				Print(LogLevel.ERROR, text, ex.Message, ex.StackTrace);
			else
				Print(LogLevel.ERROR, text);
		}

		public override void Info(params string[] text)
		{
			if (Level > LogLevel.INFO) return;
			Print(LogLevel.INFO, text);
		}

		public override void Warn(params string[] text)
		{
			if (Level > LogLevel.WARN) return;
			Print(LogLevel.WARN, text);
		}


		private void Print(LogLevel level, params string[] text)
		{
			if (Level == LogLevel.OFF) return;
			StringBuilder sb = new StringBuilder(256);
			StackTrace st;
			StackFrame sf;
			MethodBase method = null;
			int line = 0;
			string className = "";
			if (level == LogLevel.TRACE)
			{
				st = new StackTrace(true);
				sf = st.GetFrame(2);
				method = sf.GetMethod();
				line = sf.GetFileLineNumber();
				className = method.DeclaringType.FullName;
			}

			foreach (var s in text)
			{
				sb.Append(s);
				sb.Append(Environment.NewLine);
			}

			try
			{
				CreatePathIfNotExists();
				bool token = false;
				while(!token)
					Monitor.TryEnter(lockObj, 100, ref token);
				if (token)
				{
					string date = DateTime.Now.ToString(DatePattern);
					if(!currentFile.Contains(date))
					{
						num = 0;
						currentFile = Path + $"{Name}-Log{date}-part{num}.txt";		
					}
					else
					{
						SliceFileIfSpill(date);
					}
					
					using (writer = new StreamWriter(currentFile, true, Encoding.Default))
					{
						string time = DateTime.Now.ToString(TimePattern);
						writer.AutoFlush = false;
						writer.WriteLine(time + $":【{ levelDict[level]}】");
						if(level == LogLevel.FATAL)
						{
							writer.WriteLine("【系统错误】：" + text[0]);
						}
						if(level == LogLevel.TRACE)
						{			
							writer.WriteLine("【进程】：" + processName);
							writer.WriteLine("【线程ID】：" + Thread.CurrentThread.ManagedThreadId);
							writer.WriteLine("【类名】：" + className);
							writer.WriteLine("【方法名】：" + method.Name);
							writer.WriteLine("【行号】：" + line);
						}
						writer.WriteLine("【信息】：" + sb);
						writer.Flush();
					}
					Monitor.Exit(lockObj);		
				}
			}
			catch (Exception) {}
		}

		public override void PrintObject(object obj)
		{
			Print(Level, JsonConvert.SerializeObject(obj, Newtonsoft.Json.Formatting.Indented));
		}

		/// <summary>
		///  xml打印
		/// </summary>
		/// <param name="desc"></param>
		/// <param name="doc"></param>
		public override void PrintXml(string desc, string xmlStr)
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
			Print(Level, desc, sb.ToString());
		}

		private void CreatePathIfNotExists()
		{
			if (!Directory.Exists(Path))
			{
				Directory.CreateDirectory(Path);
			}
		}

		private void SliceFileIfSpill(string date)
		{
			if (File.Exists(currentFile))
			{
				FileInfo info = new FileInfo(currentFile);
				if (info.Length > Slice * 1024 * 1024)
				{
					num++;
					currentFile = Path + $"{Name}-Log{date}-part{num}.txt";
				} 
			}
			
		}

		public override void Fatal(string text, Exception ex = null)
		{
			if (Level > LogLevel.FATAL) return;
			if (ex != null)
				Print(LogLevel.FATAL, text, ex.Message, ex.StackTrace);
			else
				Print(LogLevel.FATAL, text);
		}

		public override void Trace(params string[] text)
		{
			if (Level > LogLevel.TRACE) return;
			Print(LogLevel.TRACE, text);
		}
	}
}

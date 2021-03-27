using Open.Genersoft.Component.Config.Events;
using Open.Genersoft.Component.Config.Global;
using Open.Genersoft.Component.Routing.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Open.Genersoft.Component.Routing.Core
{
	internal class CustomComponentContainer
	{
		public static List<string> BaseScanAssemblies { get; private set; }

		public Dictionary<string, CustomComponentProxy> ClassMapping { get; private set; }

		private CustomComponentContainer()
		{
			AutoScan();
			AppDomain.CurrentDomain.AssemblyLoad += new AssemblyLoadEventHandler(ReloadCustomComponent);
			ProjectConfigContainer.ConfigFileChanged += ReScan;
		}

		private void ReScan(object sender, FileChangedEvent changedEvent)
		{
			if (changedEvent.BeforeTime < changedEvent.UpdateTime)
				AutoScan();
		}

		private void AutoScan()
		{
			BaseScanAssemblies = ProjectConfigContainer.GetAutoScanAssemblies();
			ClassMapping = new Dictionary<string, CustomComponentProxy>(50);
			foreach (string s in BaseScanAssemblies)
			{
				SingleScanAssembly(s);
			}
		}

		private void SingleScanAssembly(string assemblyFullName)
		{
			if (string.IsNullOrWhiteSpace(assemblyFullName)) return;
			//Assembly target = AppDomain.CurrentDomain.GetAssemblies().Where(assembly => assembly.FullName == assemblyFullName).FirstOrDefault();
			Type[] types = Assembly.Load(assemblyFullName).GetTypes();//如果内存中已经存在，则不会加载程序集
			foreach (Type type in types)
			{
				CustomComponentAttribute customComponentAttribute = type.GetCustomAttribute<CustomComponentAttribute>(false);
				if (customComponentAttribute != null)
				{
					RouteMappingAttribute routeMapping = type.GetCustomAttribute<RouteMappingAttribute>(false);
					ClassMapping[routeMapping.Value] = new CustomComponentProxy(Activator.CreateInstance(type), customComponentAttribute.Id);
				}
			}
		}

		private void ReloadCustomComponent(object sender, AssemblyLoadEventArgs args)
		{
			string name = args.LoadedAssembly.FullName;
			string target = BaseScanAssemblies.Where(s => s == name).FirstOrDefault();
			SingleScanAssembly(target);
		}

		public static CustomComponentContainer Instance { get; } = new CustomComponentContainer();
	}
}

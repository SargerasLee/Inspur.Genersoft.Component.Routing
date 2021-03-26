using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inspur.Genersoft.Component.Config.Events
{
	public class FileChangedEvent : EventArgs
	{
		public string ConfigFilePath { get; set; }
		public string ConfigFileName { get; set; }
		public DateTime BeforeTime { get; set; }
		public DateTime UpdateTime { get; set; }
	}
}

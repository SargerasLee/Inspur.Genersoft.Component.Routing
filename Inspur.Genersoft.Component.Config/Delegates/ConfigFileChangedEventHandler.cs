using Inspur.Genersoft.Component.Config.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inspur.Genersoft.Component.Config.Delegates
{
	public delegate void ConfigFileChangedEventHandler(object sender, FileChangedEvent changedEvent);
}

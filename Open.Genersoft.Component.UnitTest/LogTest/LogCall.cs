using Open.Genersoft.Component.Logging.Facade;
using Open.Genersoft.Component.Logging.Factory;
using System.Threading;
using System.Threading.Tasks;

namespace Open.Genersoft.Component.UnitTest.LogTest
{
	public class LogCall
	{
		public static void Call()
		{
			GeneralLogger logger = LoggerFactory.Instance.GetLogger("ZJ");
			Task[] task = new Task[60];
			for (int i=0;i<10;i++)
			{
				task[i*6+0] =Task.Run(() => logger.Debug($"debug:我尼玛我尼玛我尼玛我尼玛我尼玛我尼玛{i}"));
				task[i*6+1] = Task.Run(() => logger.Info($"info:我尼玛我尼玛我尼玛我尼玛我尼玛我尼玛{i}"));
				task[i * 6 + 2] =Task.Run(() => logger.Warn($"warn:我尼玛我尼玛我尼玛我尼玛我尼玛我尼玛{i}"));
				task[i * 6 + 3] =Task.Run(() => logger.Fatal($"Fatal:我尼玛我尼玛我尼玛我尼玛我尼玛我尼玛{i}"));
				task[i * 6 + 4] =Task.Run(() => logger.Error($"Error:我尼玛我尼玛我尼玛我尼玛我尼玛我尼玛{i}"));
				task[i * 6 + 5] = Task.Run(() => logger.Trace($"Trace:我尼玛我尼玛我尼玛我尼玛我尼玛我尼玛{i}"));
				Thread.Sleep(100);
			}
			Task.WaitAll(task);
			//logger.Debug("我尼玛");
			//Console.WriteLine(new StackTrace(true).GetFrame(0).GetFileName());
		}
	}
}

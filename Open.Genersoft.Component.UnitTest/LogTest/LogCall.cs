using Open.Genersoft.Component.Logging.Facade;
using Open.Genersoft.Component.Logging.Factory;
using System.Threading.Tasks;

namespace Open.Genersoft.Component.UnitTest.LogTest
{
	public class LogCall
	{
		public static void Call()
		{
			GeneralLogger logger = LoggerFactory.Instance.GetLogger("ZJ");
			for (int i=0;i<10;i++)
			{
				Task.Run(() => logger.Debug($"debug:{i}"));
				Task.Run(() => logger.Info($"info:{i}"));
				Task.Run(() => logger.Warn($"warn:{i}"));
				Task.Run(() => logger.Fatal($"warn:{i}"));
				Task.Run(() => logger.Error($"warn:{i}"));
				Task.Run(() => logger.Trace($"warn:{i}"));
			}
			
			logger.Debug("我尼玛");
			//Console.WriteLine(new StackTrace(true).GetFrame(0).GetFileName());
		}
	}
}

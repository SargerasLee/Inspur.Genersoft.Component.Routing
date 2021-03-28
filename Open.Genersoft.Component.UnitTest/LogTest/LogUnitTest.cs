using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text.RegularExpressions;
using Open.Genersoft.Component.Routing.Core;
using System;
namespace Open.Genersoft.Component.UnitTest.LogTest
{
	[TestClass]
	public class LogUnitTest
	{
		[TestMethod]
		public void TestMethod1()
		{
			LogCall.Call();
		}
	}
}

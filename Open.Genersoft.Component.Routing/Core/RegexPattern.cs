﻿namespace Open.Genersoft.Component.Routing.Core
{
	public static class RegexPattern
	{
		public const string ConstUrlPattern = "^(/[\\w\\-,@?^=%&:/~\\+#]+)+$";
		public const string RouteUrlPattern = "^(/[\\w\\-,@?^=%&:/~\\+#]+)*(/{\\w+})+$";
		public const string RouteParamPattern = "{\\w+}";
		public const string NormalUrlPattern = "^(/[\\w\\-,@?^=%&:/~\\+#]+)+\\??(\\w+=[\\w\\-,@?^=%&:/~\\+#]+&?)*$";
	}
}

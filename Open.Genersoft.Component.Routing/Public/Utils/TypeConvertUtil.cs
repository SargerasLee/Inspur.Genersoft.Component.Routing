using System;

namespace Open.Genersoft.Component.Routing.Public.Utils
{
	public class TypeConvertUtil
	{
		public static object ConvertToBasicType(Type t, string value)
		{
			if (t == typeof(int)) return Convert.ToInt32(value);
			else if (t == typeof(long)) return Convert.ToInt64(value);
			else if (t == typeof(short)) return Convert.ToInt16(value);
			else if (t == typeof(decimal)) return Convert.ToDecimal(value);
			else if (t == typeof(bool)) return Convert.ToBoolean(value);
			else if (t == typeof(DateTime)) return Convert.ToDateTime(value);
			else if (t == typeof(char)) return Convert.ToChar(value);
			else if (t == typeof(double)) return Convert.ToDouble(value);
			else if (t == typeof(float)) return Convert.ToSingle(value);
			else if (t == typeof(byte)) return Convert.ToByte(value);
			else if (t == typeof(sbyte)) return Convert.ToSByte(value);
			else return value;
		}
	}
}

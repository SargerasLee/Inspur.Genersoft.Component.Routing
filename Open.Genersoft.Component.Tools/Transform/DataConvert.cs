using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Open.Genersoft.Component.Tools.Transform
{
	public class DataConvert
	{
		/// <summary>
		/// 列表转datatable，泛型T为实体类，只写公共属性，例如   public string Name{set;get;}
		/// </summary>
		/// <param name="list"></param>
		/// <returns></returns>
		public static DataTable ListToDataTable<T>(List<T> list)
		{
			if (list == null) return null;
			Type t = typeof(T);
			DataTable table = new DataTable(t.Name);
			DataRowCollection collection = table.Rows;
			PropertyInfo[] infos = t.GetProperties(BindingFlags.Public | BindingFlags.Instance);
			foreach (var info in infos)
			{
				table.Columns.Add(info.Name, info.PropertyType);
			}
			foreach (var item in list)
			{
				DataRow row = table.NewRow();
				foreach (var info in infos)
				{
					row[info.Name] = info.GetValue(item);
				}
				collection.Add(row);
			}
			return table;
		}

		/// <summary>
		/// dataset转列表
		/// </summary>
		/// <param name="table"></param>
		/// <returns></returns>
		public static List<T> DataTableToList<T>(DataTable table)
		{
			if (table == null) return null;
			List<T> list = new List<T>();
			Type type = typeof(T);
			PropertyInfo[] pInfo = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
			List<PropertyInfo> info = pInfo.ToList();
			foreach (DataRow row in table.Rows)
			{
				T t = Activator.CreateInstance<T>();
				for (int i = 0; i < table.Columns.Count; i++)
				{
					PropertyInfo pi = info.Find(p => p.Name == table.Columns[i].ColumnName);
					if (pi.PropertyType == typeof(string))
						pi.SetValue(t, row[i] is DBNull | row[i] is null ? "" : Convert.ToString(row[i]));
					else if (pi.PropertyType == typeof(int))
						pi.SetValue(t, row[i] is DBNull | row[i] is null ? 0 : Convert.ToInt32(row[i]));
					else if (pi.PropertyType == typeof(decimal))
						pi.SetValue(t, row[i] is DBNull | row[i] is null ? 0m : Convert.ToDecimal(row[i]));
					else if (pi.PropertyType == typeof(bool))
						pi.SetValue(t, row[i] is DBNull | row[i] is null ? false : Convert.ToBoolean(row[i]));
					else if (pi.PropertyType == typeof(double))
						pi.SetValue(t, row[i] is DBNull | row[i] is null ? 0d : Convert.ToDouble(row[i]));
					else if (pi.PropertyType == typeof(byte))
						pi.SetValue(t, row[i] is DBNull | row[i] is null ? 0 : Convert.ToByte(row[i]));
					else if (pi.PropertyType == typeof(char))
						pi.SetValue(t, row[i] is DBNull | row[i] is null ? '\0' : Convert.ToChar(row[i]));
					else
						pi.SetValue(t, row[i] is DBNull ? null : row[i]);
				}
				list.Add(t);
			}
			return list;
		}
	}
}

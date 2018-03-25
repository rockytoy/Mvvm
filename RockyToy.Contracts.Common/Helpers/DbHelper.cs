using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace RockyToy.Contracts.Common.Helpers
{
	public static class DbHelper
	{
		public static object ValueOrNull<T>(T val)
		{
			return val == null ? DBNull.Value : (object)val;
		}

		public static object ValueOrNull<T>(T? val) where T : struct
		{
			return val == null ? DBNull.Value : (object) val.Value;
		}

		public static int StringMaxLength<T>(string propertyName)
		{
			var type = typeof(T);
			PropertyInfo pi;
			if (type.IsInterface)
			{
				pi = type.GetInterfaces().Union(new[] { type }).SelectMany(x => x.GetProperties())
					.FirstOrDefault(x => x.Name == propertyName);
			}
			else
			{
				pi = type.GetProperty(propertyName);
			}

			if (pi == null)
				throw new Exception($"{typeof(T).FullName} does not have property: {propertyName}");
			
			if (!(Attribute.GetCustomAttributes(pi, typeof(MaxLengthAttribute), true).FirstOrDefault() is MaxLengthAttribute attr))
				throw new Exception($"{typeof(T).FullName}.{propertyName} does not have {typeof(MaxLengthAttribute).Name}");
			return attr.Length;
		}
	}
}
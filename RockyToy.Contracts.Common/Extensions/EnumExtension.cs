using System;
using System.ComponentModel.DataAnnotations;

namespace RockyToy.Contracts.Common.Extensions
{
	public static class EnumExtension
	{
		public static string GetDisplayName(this Enum value)
		{
			var fi = value.GetType().GetField(value.ToString());
			var attributes = (DisplayAttribute[]) fi.GetCustomAttributes(typeof(DisplayAttribute), false);
			return attributes.Length > 0 ? attributes[0].Name : value.ToString();
		}

		public static string GetDisplayDescription(this Enum value)
		{
			var fi = value.GetType().GetField(value.ToString());
			var attributes = (DisplayAttribute[]) fi.GetCustomAttributes(typeof(DisplayAttribute), false);
			return attributes.Length > 0 ? attributes[0].Description : value.ToString();
		}
	}
}
﻿using System;
using System.Globalization;
using System.Windows.Data;

namespace RockyToy.Resources.Wpf.Converters
{
	public class DateToStringConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value == null)
				return string.Empty;
			// convert to date string
			if (value is DateTime dt) return dt.ToString("yyyy-MM-dd");

			var valString = value as string;
			if (string.IsNullOrWhiteSpace(valString))
				return string.Empty;
			return DateTime.TryParse(valString, out var dt_) ? dt_.ToString("yyyy-MM-dd") : string.Empty;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (targetType != typeof(DateTime) && targetType != typeof(DateTime?))
				return value;

			var valStr = value as string;
			if (string.IsNullOrWhiteSpace(valStr))
				return targetType == typeof(DateTime?) ? null : Binding.DoNothing;

			try
			{
				return DateTime.TryParse(valStr, out var dt_) ? dt_.Date : Binding.DoNothing;
			}
			catch
			{
				return Binding.DoNothing;
			}
		}
	}
}
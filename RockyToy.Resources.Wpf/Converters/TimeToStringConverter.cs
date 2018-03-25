using System;
using System.Globalization;
using System.Windows.Data;

namespace RockyToy.Resources.Wpf.Converters
{
	public class TimeToStringConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			switch (value)
			{
				case null:
					return string.Empty;
				case TimeSpan t:
					return t.ToString("c");
				case DateTime dt:
					return dt.TimeOfDay.ToString("c");
				case string valString:
					if (string.IsNullOrWhiteSpace(valString))
						return string.Empty;
					if (TimeSpan.TryParse(valString, out var ts_))
						return ts_.ToString("c");
					if (DateTime.TryParse(valString, out var dt_))
						return dt_.TimeOfDay.ToString("c");
					break;
			}

			return string.Empty;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var valStr = value as string;
			if (targetType == typeof(DateTime) || targetType == typeof(DateTime?))
			{
				if (string.IsNullOrWhiteSpace(valStr))
					return targetType == typeof(DateTime?) ? null : Binding.DoNothing;

				try
				{
					if (DateTime.TryParse(valStr, out var dt_))
						return dt_;
					if (TimeSpan.TryParse(valStr, out var ts_))
						return DateTime.Today.Add(ts_);
					return targetType == typeof(DateTime?) ? null : Binding.DoNothing;
				}
				catch
				{
					return targetType == typeof(DateTime?) ? null : Binding.DoNothing;
				}
			}

			if (targetType == typeof(TimeSpan) || targetType == typeof(TimeSpan?))
			{
				if (string.IsNullOrWhiteSpace(valStr))
					return targetType == typeof(TimeSpan?) ? null : Binding.DoNothing;

				try
				{
					if (TimeSpan.TryParse(valStr, out var ts_))
						return ts_;
					if (DateTime.TryParse(valStr, out var dt_))
						return dt_.TimeOfDay;
					return targetType == typeof(TimeSpan?) ? null : Binding.DoNothing;
				}
				catch
				{
					return targetType == typeof(TimeSpan?) ? null : Binding.DoNothing;
				}
			}

			return value;
		}
	}
}
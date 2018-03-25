using System;
using System.Globalization;
using System.Windows.Data;

namespace RockyToy.Resources.Wpf.Converters
{
	public class NumberToStringConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var formatString = parameter as string;
			const string defaultFloatFormatString = @"{0:n2}";

			switch (value)
			{
				case null:
					return string.Empty;
				case string _:
					return value;
				case int valInt:
					if (string.IsNullOrWhiteSpace(formatString))
						return valInt.ToString(culture);
					return string.Format(culture, formatString, valInt);
				case long valLong:
					if (string.IsNullOrWhiteSpace(formatString))
						return valLong.ToString(culture);
					return string.Format(culture, formatString, valLong);
				case float valFloat:
					if (string.IsNullOrWhiteSpace(formatString))
						return string.Format(culture, defaultFloatFormatString, valFloat);
					return string.Format(culture, formatString, valFloat);
				case double valDouble:
					if (string.IsNullOrWhiteSpace(formatString))
						return string.Format(culture, defaultFloatFormatString, valDouble);
					return string.Format(culture, formatString, valDouble);
				case decimal valDecimal:
					if (string.IsNullOrWhiteSpace(formatString))
						return string.Format(culture, defaultFloatFormatString, valDecimal);
					return string.Format(culture, formatString, valDecimal);
			}

			return string.Empty;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (targetType != typeof(int)
			    && targetType != typeof(int?)
			    && targetType != typeof(long)
			    && targetType != typeof(long?)
			    && targetType != typeof(float)
			    && targetType != typeof(float?)
			    && targetType != typeof(double)
			    && targetType != typeof(double?)
			    && targetType != typeof(decimal)
			    && targetType != typeof(decimal?))
				return value;

			var valStr = value as string;
			if (string.IsNullOrWhiteSpace(valStr))
			{
				if (targetType != typeof(int?)
				    && targetType != typeof(long?)
				    && targetType != typeof(float?)
				    && targetType != typeof(double?)
				    && targetType != typeof(decimal?))
					return 0;
				return null;
			}

			try
			{
				if (targetType == typeof(int)
				    || targetType == typeof(int?))
					return int.Parse(valStr, culture);
				if (targetType == typeof(long)
				    || targetType == typeof(long?))
					return long.Parse(valStr, culture);
				if (targetType == typeof(float)
				    || targetType == typeof(float?))
					return float.Parse(valStr, culture);
				if (targetType == typeof(double)
				    || targetType == typeof(double?))
					return double.Parse(valStr, culture);
				if (targetType == typeof(decimal)
				    || targetType == typeof(decimal?))
					return decimal.Parse(valStr, culture);
			}
			catch
			{
				return 0;
			}

			// should not reachable
			return value;
		}
	}
}
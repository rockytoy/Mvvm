using System;
using System.Globalization;
using System.Windows.Data;

namespace RockyToy.Resources.Wpf.Converters
{
	public class BaseNumberConverter<TIn, TOut> : IValueConverter
		where TIn : struct, IConvertible
		where TOut : struct, IConvertible
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value == null)
				return null;

			TIn? val;
			if (value is TIn valIn)
				val = valIn;
			else
				try
				{
					val = (TIn?) System.Convert.ChangeType(parameter, typeof(TIn));
				}
				catch
				{
					val = null;
				}

			if (targetType == typeof(TOut))
				return val == null ? default(TOut) : System.Convert.ChangeType(val.Value, typeof(TOut));

			if (targetType == typeof(TOut?))
				return val == null ? null : System.Convert.ChangeType(val.Value, typeof(TOut));

			return value;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value == null)
				return null;

			TOut? val;
			if (value is TOut valOut)
				val = valOut;
			else
				try
				{
					val = (TOut?) System.Convert.ChangeType(parameter, typeof(TOut));
				}
				catch
				{
					val = null;
				}

			if (targetType == typeof(TIn))
				return val == null ? default(TIn) : System.Convert.ChangeType(val.Value, typeof(TIn));

			if (targetType == typeof(TIn?))
				return val == null ? null : System.Convert.ChangeType(val.Value, typeof(TIn));

			return value;
		}
	}
}
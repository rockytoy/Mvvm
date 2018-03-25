using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Data;

namespace RockyToy.Resources.Wpf.Converters
{
	public class EqualToConverter : IValueConverter
	{
		public virtual object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value == null)
				return parameter == null;

			if (parameter == null)
				return false;

			if (!(parameter is string paramString) || value is string)
				return parameter.Equals(value);

			// if parameter is string and binding value is not, try to convert it first
			try
			{
				parameter = TypeDescriptor.GetConverter(value.GetType()).ConvertFromString(paramString);
			}
			catch
			{
				return false;
			}

			return parameter != null && parameter.Equals(value);
		}

		public virtual object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value == null)
				return Binding.DoNothing;

			if ((bool) value == false)
				return Binding.DoNothing;

			var paramString = parameter as string;
			// if parameter is not string and binding value is also not string
			if (paramString == null && targetType != typeof(string))
				return parameter;
			// if parameter is not string but binding value is string
			if (paramString == null && targetType == typeof(string)) return parameter?.ToString();

			// if parameter is string and binding value is not, try to convert it first
			try
			{
				parameter = TypeDescriptor.GetConverter(targetType).ConvertFromString(paramString);
			}
			catch
			{
				return Binding.DoNothing;
			}

			return parameter;
		}
	}
}
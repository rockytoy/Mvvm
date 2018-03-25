using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace RockyToy.Resources.Wpf.Converters
{
	public class NotEqualToVisibilityConverter : NotEqualToConverter
	{
		public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var convert = base.Convert(value, targetType, parameter, culture);
			if (convert != null && (bool) convert)
				return Visibility.Visible;
			return Visibility.Collapsed;
		}

		public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return value == null
				? Binding.DoNothing
				: base.ConvertBack((Visibility) value == Visibility.Visible, targetType, parameter, culture);
		}
	}
}
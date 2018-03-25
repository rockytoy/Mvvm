using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace RockyToy.Resources.Wpf.Converters
{
	public class GridLengthValueConverter : IValueConverter
	{
		private readonly GridLengthConverter _converter = new GridLengthConverter();

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return _converter.ConvertFrom(value);
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return value != null ? _converter.ConvertTo(value, targetType) : Binding.DoNothing;
		}
	}
}
using System;
using System.Globalization;
using System.Windows.Data;

namespace RockyToy.Resources.Wpf.Converters
{
	public class DoNothingConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return Binding.DoNothing;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return Binding.DoNothing;
		}
	}
}
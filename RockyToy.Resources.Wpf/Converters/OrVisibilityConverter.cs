using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;

namespace RockyToy.Resources.Wpf.Converters
{
	public class OrVisibilityConverter : IMultiValueConverter
	{
		public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
		{
			var items = (values?.OfType<IConvertible>() ?? Enumerable.Empty<IConvertible>()).ToArray();
			return items.Length != 0 && items.Any(System.Convert.ToBoolean) ? Visibility.Visible : Visibility.Collapsed;
		}

		public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace RockyToy.Resources.Wpf.Converters
{
	public class AndConverter : IMultiValueConverter
	{
		public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
		{
			var items = (values?.OfType<IConvertible>() ?? Enumerable.Empty<IConvertible>()).ToArray();
			return items.Length != 0 && items.All(System.Convert.ToBoolean);
		}

		public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
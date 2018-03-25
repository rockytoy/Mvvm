using System.Diagnostics.CodeAnalysis;
using System.Windows.Media;

namespace RockyToy.Resources.Wpf.Resources
{
	[SuppressMessage("ReSharper", "PossibleNullReferenceException")]
	public static class Brushes
	{
		static Brushes()
		{
			var defForeground = new SolidColorBrush(Colors.Black);
			var defBackground = new SolidColorBrush(Colors.White);

			DisabledForeground = defForeground;
			DisabledBackground = new SolidColorBrush(Colors.LightGray);
			Border = new SolidColorBrush((Color) ColorConverter.ConvertFromString("#CDCDCD"));
			ButtonBackground = new SolidColorBrush((Color) ColorConverter.ConvertFromString("#F5F5F5"));
			OptionBar = defBackground;
			TitleForeground = defForeground;
			TitleBackground = ButtonBackground;
			DataGridBackground = defBackground;
			DataGridHeaderForeground = defForeground;
			DataGridHeaderBackground = new SolidColorBrush((Color) ColorConverter.ConvertFromString("#CFECF0"));
			DataGridRowHeaderBackground = new SolidColorBrush((Color) ColorConverter.ConvertFromString("#656565"));
			DataGridBorder = new SolidColorBrush((Color) ColorConverter.ConvertFromString("#EFEFEF"));
			DataGridBorderHighlight = new SolidColorBrush((Color) ColorConverter.ConvertFromString("#EFEFEF"));
			DataGridBackgroundHighlight = new SolidColorBrush(Colors.LightBlue) {Opacity = 0.5};
			TabItemHeaderHighlightForeground = new SolidColorBrush((Color) ColorConverter.ConvertFromString("#4EC1D0"));
			TabItemHeaderHoverBackground = new SolidColorBrush(Colors.LightBlue);
			TabItemBackground = defBackground;
			TabItemBorder = ButtonBackground;
			SplitterBackground = defBackground;
		}

		public static Brush DisabledForeground { get; }
		public static Brush DisabledBackground { get; }
		public static Brush Border { get; }
		public static Brush ButtonBackground { get; }

		public static Brush OptionBar { get; }
		public static Brush TitleForeground { get; }
		public static Brush TitleBackground { get; }

		public static Brush DataGridBackground { get; }
		public static Brush DataGridHeaderForeground { get; }
		public static Brush DataGridHeaderBackground { get; }
		public static Brush DataGridRowHeaderBackground { get; }
		public static Brush DataGridBorder { get; }
		public static Brush DataGridBorderHighlight { get; }
		public static Brush DataGridBackgroundHighlight { get; }

		public static Brush TabItemHeaderHighlightForeground { get; }
		public static Brush TabItemHeaderHoverBackground { get; }
		public static Brush TabItemBackground { get; }
		public static Brush TabItemBorder { get; }

		public static Brush SplitterBackground { get; }
	}
}
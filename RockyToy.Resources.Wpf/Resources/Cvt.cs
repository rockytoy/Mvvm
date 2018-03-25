using System.Windows.Controls;
using RockyToy.Resources.Wpf.Converters;

namespace RockyToy.Resources.Wpf.Resources
{
	public static class Cvt
	{
		static Cvt()
		{
			And = new AndConverter();
			AndVis = new AndVisibilityConverter();
			Or = new OrConverter();
			OrVis = new OrVisibilityConverter();
			Eq = new EqualToConverter();
			EqVis = new EqualToVisibilityConverter();
			Neq = new NotEqualToConverter();
			NeqVis = new NotEqualToVisibilityConverter();
			DoNothing = new DoNothingConverter();
			GridLength = new GridLengthValueConverter();
			IsNull = new IsNullConverter();
			B2Vis = new BooleanToVisibilityConverter();
			T2S = new TimeToStringConverter();
			Dt2S = new DateTimeToStringConverter();
			D2S = new DateToStringConverter();
			I2D = new IntToDoubleConverter();
		}

		public static AndConverter And { get; }
		public static AndVisibilityConverter AndVis { get; }
		public static OrConverter Or { get; }
		public static OrVisibilityConverter OrVis { get; }

		public static EqualToConverter Eq { get; }
		public static EqualToVisibilityConverter EqVis { get; }
		public static NotEqualToConverter Neq { get; }
		public static NotEqualToVisibilityConverter NeqVis { get; }

		public static DoNothingConverter DoNothing { get; }
		public static GridLengthValueConverter GridLength { get; }
		public static IsNullConverter IsNull { get; }
		public static BooleanToVisibilityConverter B2Vis { get; }
		public static TimeToStringConverter T2S { get; }
		public static DateTimeToStringConverter Dt2S { get; }
		public static DateToStringConverter D2S { get; }
		public static IntToDoubleConverter I2D { get; }
	}
}
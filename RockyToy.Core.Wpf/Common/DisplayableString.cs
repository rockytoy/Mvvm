using System.Collections.Generic;

namespace RockyToy.Core.Wpf.Common
{
	public class DisplayableString : DisplayableData<string>
	{
		public DisplayableString(string desc) : base(desc, desc)
		{
		}

		public DisplayableString(string desc, string value) : base(desc, value)
		{
		}

		public static IEnumerable<DisplayableString> NoneItems(string noneDesc = null, string noneValue = null)
		{
			yield return new DisplayableString(noneDesc ?? NoneDesc(), noneValue ?? string.Empty);
		}
	}
}
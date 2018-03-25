using System.Collections.Generic;
using RockyToy.Contracts.Wpf.Common;

namespace RockyToy.Core.Wpf.Common
{
	public class DisplayableInt : DisplayableData<int>, IDisplayableInt
	{
		public DisplayableInt(string desc, int val) : base(desc, val)
		{
		}

		public int DisplayInt => DisplayData;

		public static IEnumerable<DisplayableInt> NoneItems(string noneDesc = null, int noneValue = 0)
		{
			yield return new DisplayableInt(noneDesc ?? NoneDesc(), noneValue);
		}
	}
}
using System.Collections.Generic;

namespace RockyToy.Contracts.Common.Extensions
{
	public static class EnumerableExtension
	{
		public static HashSet<T> ToHashSet<T>(this IEnumerable<T> items)
		{
			return new HashSet<T>(items);
		}
	}
}
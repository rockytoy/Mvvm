using System;
using System.Collections.Generic;

namespace RockyToy.Contracts.Common.Helpers
{
	public static class EnumHelper
	{
		/// <summary>
		///   https://stackoverflow.com/questions/972307/can-you-loop-through-all-enum-values
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public static IReadOnlyList<T> GetValues<T>()
		{
			return (T[]) Enum.GetValues(typeof(T));
		}
	}
}
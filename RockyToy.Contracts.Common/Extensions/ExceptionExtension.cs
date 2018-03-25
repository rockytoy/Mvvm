using System;
using System.Linq;

namespace RockyToy.Contracts.Common.Extensions
{
	public static class ExceptionExtension
	{
		public static bool IsCancellation(this Exception e)
		{
			if (e is AggregateException ae)
				return ae.Flatten().InnerExceptions.All(ie => ie is OperationCanceledException);

			return e is OperationCanceledException;
		}
	}
}
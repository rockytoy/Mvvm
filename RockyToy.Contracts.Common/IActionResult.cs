using System;

namespace RockyToy.Contracts.Common
{
	public interface IActionResult
	{
		ActionStatus Status { get; }
		string Action { get; }
		string ErrorMessage { get; }
		Exception Error { get; }

		void ThrowIfError();

		IActionResults AsResults();
	}

	public interface IActionResult<out TOutput> : IActionResult
	{
		TOutput Output { get; }
	}
}
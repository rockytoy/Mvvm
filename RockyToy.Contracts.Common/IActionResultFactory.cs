using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace RockyToy.Contracts.Common
{
	public interface IActionResultFactory
	{
		IActionResults MergeActions(IEnumerable<IActionResult> results, [CallerMemberName] string action = null);

		IActionResults CancelActions([CallerMemberName] string action = null);
		IActionResults CancelActions(Exception e, [CallerMemberName] string action = null);
		IActionResults SuccessActions([CallerMemberName] string action = null);
		IActionResults ErrorActions(Exception e, [CallerMemberName] string action = null);

		IActionResult CancelAction([CallerMemberName] string action = null);
		IActionResult CancelAction(Exception e, [CallerMemberName] string action = null);
		IActionResult SuccessAction([CallerMemberName] string action = null);
		IActionResult ErrorAction(Exception e, [CallerMemberName] string action = null);

		IActionResult<TResult> CancelAction<TResult>([CallerMemberName] string action = null);
		IActionResult<TResult> CancelAction<TResult>(Exception e, [CallerMemberName] string action = null);
		IActionResult<TResult> SuccessAction<TResult>(TResult res, [CallerMemberName] string action = null);
		IActionResult<TResult> ErrorAction<TResult>(Exception e, [CallerMemberName] string action = null);

	}
}
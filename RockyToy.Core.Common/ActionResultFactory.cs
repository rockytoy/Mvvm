using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using RockyToy.Contracts.Common;

namespace RockyToy.Core.Common
{
	public class ActionResultFactory : IActionResultFactory
	{
		private readonly OperationCanceledException _cancelledException = new OperationCanceledException();

		public IActionResults MergeActions(IEnumerable<IActionResult> results, [CallerMemberName] string action = null)
		{
			return new ActionResults(action, results);
		}

		public IActionResults CancelActions([CallerMemberName] string action = null)
		{
			return CancelActions(_cancelledException, action);
		}

		public IActionResults CancelActions(Exception e, [CallerMemberName] string action = null)
		{
			return ErrorActions(e is OperationCanceledException ? e : _cancelledException, action);
		}

		public IActionResults SuccessActions([CallerMemberName] string action = null)
		{
			return new ActionResults(action);
		}

		public IActionResults ErrorActions(Exception e, [CallerMemberName] string action = null)
		{
			return new ActionResults(action, e);
		}

		public IActionResult CancelAction([CallerMemberName] string action = null)
		{
			return CancelAction(_cancelledException, action);
		}

		public IActionResult CancelAction(Exception e, [CallerMemberName] string action = null)
		{
			return ErrorAction(e is OperationCanceledException ? e : _cancelledException, action);
		}

		public IActionResult SuccessAction([CallerMemberName] string action = null)
		{
			return new ActionResult(action);
		}

		public IActionResult ErrorAction(Exception e, [CallerMemberName] string action = null)
		{
			return new ActionResult(action, e);
		}

		public IActionResult<TResult> CancelAction<TResult>(string action = null)
		{
			return CancelAction<TResult>(_cancelledException, action);
		}

		public IActionResult<TResult> CancelAction<TResult>(Exception e, string action = null)
		{
			return ErrorAction<TResult>(e is OperationCanceledException ? e : _cancelledException, action);
		}

		public IActionResult<TResult> SuccessAction<TResult>(TResult res, string action = null)
		{
			return new ActionResult<TResult>(action, res);
		}

		public IActionResult<TResult> ErrorAction<TResult>(Exception e, string action = null)
		{
			return new ActionResult<TResult>(action, e, default(TResult));
		}
	}
}
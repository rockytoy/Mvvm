using System;
using JetBrains.Annotations;
using RockyToy.Contracts.Common;
using RockyToy.Contracts.Common.Extensions;

namespace RockyToy.Core.Common
{
	public class ActionResult : IActionResult
	{
		internal ActionResult([NotNull] string action) : this(action, null)
		{
		}

		internal ActionResult([NotNull] string action, Exception error)
		{
			if (error == null)
			{
				Status = ActionStatus.Success;
				Action = action;
				Error = null;
			}
			else
			{
				Status = error.IsCancellation() ? ActionStatus.Cancelled : ActionStatus.Failed;
				Action = action;
				Error = error;
			}
		}

		public virtual ActionStatus Status { get; }
		public virtual string Action { get; }

		public virtual string ErrorMessage =>
			Status == ActionStatus.Cancelled ? "Cancelled" : (Error?.Message ?? string.Empty);

		public virtual Exception Error { get; }

		public void ThrowIfError()
		{
			if (Error != null && !Error.IsCancellation())
				throw Error;
		}

		public virtual IActionResults AsResults()
		{
			return new ActionResults(this);
		}
	}

	public class ActionResult<TResult> : ActionResult, IActionResult<TResult>
	{
		internal ActionResult([NotNull] string action, TResult output) : base(action)
		{
			Output = output;
		}

		internal ActionResult([NotNull] string action, [NotNull] Exception error, TResult output) : base(action, error)
		{
			Output = output;
		}

		public TResult Output { get; }
	}
}
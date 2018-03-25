using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using RockyToy.Contracts.Common;

namespace RockyToy.Core.Common
{
	public class ActionResults : ActionResult, IActionResults, IActionResults<ActionResult>
	{
		private readonly List<IActionResult> _results = new List<IActionResult>();

		internal ActionResults([NotNull] string action) : base(action)
		{
		}

		internal ActionResults([NotNull] string action, [NotNull] Exception error) : base(action, error)
		{
		}

		internal ActionResults(IActionResult ar) : base(ar.Action, ar.Error)
		{
			_results.Add(ar);
		}

		internal ActionResults([NotNull] string action, IEnumerable<IActionResult> ars) : base(action, null)
		{
			_results.AddRange(ars);
		}

		public override ActionStatus Status
		{
			get
			{
				if (base.Status != ActionStatus.Success)
					return base.Status;

				var ret = ActionStatus.Success;
				foreach (var r in _results)
				{
					if (r.Status == ActionStatus.Failed)
						return ActionStatus.Failed;
					if (r.Status == ActionStatus.Cancelled)
						ret = ActionStatus.Cancelled;
				}

				return ret;
			}
		}

		public override string Action =>
			base.Action ?? _results.FirstOrDefault(x => !string.IsNullOrEmpty(x.Action))?.Action ?? string.Empty;

		public override string ErrorMessage => base.ErrorMessage ??
		                                       _results.FirstOrDefault(x => !string.IsNullOrEmpty(x.ErrorMessage))
			                                       ?.ErrorMessage ?? string.Empty;

		public override Exception Error => base.Error ?? _results.FirstOrDefault(x => x.Error != null)?.Error;

		public override IActionResults AsResults()
		{
			return this;
		}

		public void Add(IActionResult result)
		{
			_results.Add(result);
		}

		public void AddRange(IEnumerable<IActionResult> results)
		{
			_results.AddRange(results);
		}

		public void AddRange(IActionResults results)
		{
			_results.AddRange(results.Results);
		}

		IEnumerable<IActionResult> IActionResults.Results => _results;

		public void Add(ActionResult result)
		{
			_results.Add(result);
		}

		public void AddRange(IEnumerable<ActionResult> results)
		{
			_results.AddRange(results);
		}

		public void AddRange(IActionResults<ActionResult> results)
		{
			_results.AddRange(results.Results);
		}

		public IEnumerable<ActionResult> Results => _results.OfType<ActionResult>();
	}
}
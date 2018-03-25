using System.Collections.Generic;

namespace RockyToy.Contracts.Common
{
	public interface IActionResults : IActionResult
	{
		IEnumerable<IActionResult> Results { get; }
		void Add(IActionResult result);
		void AddRange(IEnumerable<IActionResult> results);
		void AddRange(IActionResults results);
	}

	public interface IActionResults<TActionResult> : IActionResult where TActionResult : IActionResult
	{
		IEnumerable<TActionResult> Results { get; }
		void Add(TActionResult result);
		void AddRange(IEnumerable<TActionResult> results);
		void AddRange(IActionResults<TActionResult> results);
	}
}
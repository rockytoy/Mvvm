using System;
using System.Threading;
using System.Threading.Tasks;
using RockyToy.Contracts.Common;

namespace RockyToy.Contracts.Wpf.MetroDialog
{
	public class BusyDialogOptionsWithResults : CustomMetroDialogSettings
	{
		public BusyDialogOptionsWithResults()
		{
			Title = "Processing";
			Message = "Please wait...";
		}

		public Func<IProgress<Progression>, CancellationToken, Task<IActionResults>> Task { get; set; }
		public ILogger Log { get; set; }
	}
}
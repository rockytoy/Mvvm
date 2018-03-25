using System;
using System.Threading;
using System.Threading.Tasks;
using RockyToy.Contracts.Common;

namespace RockyToy.Contracts.Wpf.MetroDialog
{
	public class BusyDialogOptions : CustomMetroDialogSettings
	{
		public BusyDialogOptions()
		{
			Title = "Processing";
			Message = "Please wait...";
		}

		public Func<IProgress<Progression>, CancellationToken, Task> Task { get; set; }
		public ILogger Log { get; set; }
	}
}
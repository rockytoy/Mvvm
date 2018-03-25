using System;
using System.Threading.Tasks;
using RockyToy.Contracts.Common;

namespace RockyToy.Contracts.Wpf
{
	public interface IShellViewModel : IClosableViewModel
	{
		IScreenViewModel ActiveScreen { get; }
		DateTimeOffset? LastActivedDateTime { get; set; }
		Task<IActionResults> ShowScreen(Func<IScreenViewModel> screen);
	}
}
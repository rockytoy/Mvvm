using System;
using System.Reactive;
using System.Threading.Tasks;
using RockyToy.Contracts.Common;

namespace RockyToy.Contracts.Wpf
{
	public interface IViewModel : IBaseReactive
	{
		VmStateEnum VmState { get; }
		IObservable<Unit> WhenViewModelInitialized { get; }
		IObservable<Unit> WhenViewModelActivated { get; }
		IObservable<Unit> WhenViewModelDeactivated { get; }
		IObservable<Unit> WhenViewModelClosed { get; }

		Task Activate();
		Task Deactivate(bool close);
	}
}
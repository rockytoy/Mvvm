using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows;
using Autofac;
using ReactiveUI;
using RockyToy.Contracts.Common;
using RockyToy.Contracts.Wpf;
using RockyToy.Core.Common;

namespace RockyToy.Core.Wpf
{
	public abstract class BaseViewModel : BaseReactive, IViewModel
	{
		private VmStateEnum _vmState;

		protected BaseViewModel()
		{
			WhenViewModelInitialized = this.WhenAnyValue(x => x.VmState).Where(x => x == VmStateEnum.Initialized)
				.Select(x => Unit.Default);
			WhenViewModelActivated = this.WhenAnyValue(x => x.VmState).Where(x => x == VmStateEnum.Activated)
				.Select(x => Unit.Default);
			WhenViewModelDeactivated = this.WhenAnyValue(x => x.VmState).Where(x => x == VmStateEnum.Deactivated)
				.Select(x => Unit.Default);
			WhenViewModelClosed =
				this.WhenAnyValue(x => x.VmState).Where(x => x == VmStateEnum.Closed).Select(x => Unit.Default);

			var log = ((IApp)Application.Current).Root.Resolve<Func<Type, ILogger>>()(typeof(BaseShellView));

			WhenViewModelInitialized.Subscribe(_ => log.Trace($"{this} inited"));
			WhenViewModelActivated.Subscribe(_ => log.Trace($"{this} activated"));
			WhenViewModelDeactivated.Subscribe(_ => log.Trace($"{this} deactivated"));
			WhenViewModelClosed.Subscribe(_ => log.Trace($"{this} closed"));

		}

		public virtual async Task Activate()
		{
			switch (VmState)
			{
				case VmStateEnum.Created:
					await OnInitializing();
					VmState = VmStateEnum.Initialized;
					goto case VmStateEnum.Initialized;
				case VmStateEnum.Initialized:
				case VmStateEnum.Deactivated:
					await OnActivating();
					VmState = VmStateEnum.Activated;
					break;
				case VmStateEnum.Activated:
				case VmStateEnum.Closed:
				default:
					return;
			}
		}

		public virtual async Task Deactivate(bool close)
		{
			switch (VmState)
			{
				case VmStateEnum.Created:
					await Activate();
					goto case VmStateEnum.Activated;
				case VmStateEnum.Initialized:
				case VmStateEnum.Activated:
					await OnDeactivating(close);
					VmState = VmStateEnum.Deactivated;
					if (close)
					{
						await OnClosing();
						VmState = VmStateEnum.Closed;
					}

					break;
				case VmStateEnum.Deactivated:
					if (close)
					{
						await OnClosing();
						VmState = VmStateEnum.Closed;
					}

					break;
				default:
				case VmStateEnum.Closed:
					return;
			}
		}

		public VmStateEnum VmState
		{
			get => _vmState;
			private set => this.SetProperty(ref _vmState, value);
		}

		public IObservable<Unit> WhenViewModelInitialized { get; }
		public IObservable<Unit> WhenViewModelActivated { get; }
		public IObservable<Unit> WhenViewModelDeactivated { get; }
		public IObservable<Unit> WhenViewModelClosed { get; }

		protected virtual Task OnActivating()
		{
			return Task.CompletedTask;
		}

		protected virtual Task OnDeactivating(bool close)
		{
			return Task.CompletedTask;
		}

		protected virtual Task OnClosing()
		{
			return Task.CompletedTask;
		}

		protected virtual Task OnInitializing()
		{
			return Task.CompletedTask;
		}
	}
}
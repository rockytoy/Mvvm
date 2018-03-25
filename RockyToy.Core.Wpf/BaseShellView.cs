using System;
using System.ComponentModel;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Windows;
using Autofac;
using MahApps.Metro.Controls;
using ReactiveUI;
using RockyToy.Contracts.Common;
using RockyToy.Contracts.Wpf;

namespace RockyToy.Core.Wpf
{
	public abstract class BaseShellView : MetroWindow, IShellView
	{
		protected BaseShellView()
		{
			WindowStartupLocation = WindowStartupLocation.CenterOwner;

			IDisposable[] observeActivatedOnce = {null};

			observeActivatedOnce[0] = this.WhenActivated(d =>
			{

				var log = ((IApp)Application.Current).Root.Resolve<Func<Type, ILogger>>()(typeof(BaseShellView));
				log.Trace($"{this} loaded");

				d(this.OneWayBind(ViewModel, vm => vm.ActiveScreen.Title, v => v.Title));

				d(WhenShellLoaded
					.Merge(WhenShellActivated)
					.Select(_ => ViewModel.Activate().ToObservable().Take(1))
					.Concat()
					.Subscribe()
				);
				d(WhenShellDeactivated
					.Select(_ => ViewModel.Deactivate(false).ToObservable().Take(1))
					.Concat()
					.Subscribe()
				);
				d(WhenShellClosing
					.Where(_ => !ForceClose)
					.Do(e => e.Cancel = true)
					.Select(e => ViewModel.TryClose().ToObservable().Take(1))
					.Concat()
					.Where(closed => closed.Status == ActionStatus.Success)
					.Do(_ => ForceClose = true)
					.Select(_ => Observable.Start(Close, RxApp.MainThreadScheduler).Take(1))
					.Concat()
					.Subscribe()
				);
				d(WhenShellClosed
					.Do(_ => IsClosed = true)
					.Select(_ => ViewModel.Deactivate(true).ToObservable().Take(1))
					.Concat()
					.Subscribe()
				);

				d(ViewModel.WhenViewModelClosed
					.Where(_ => !IsClosed)
					.Do(_ => ForceClose = true)
					.Select(_ => Observable.Start(Close, RxApp.MainThreadScheduler).Take(1))
					.Concat()
					.Subscribe()
				);

				d(this.WhenAnyValue(x => x.IsActive).Where(x => x)
					.Subscribe(_ => ViewModel.LastActivedDateTime = DateTimeOffset.Now));

				d(observeActivatedOnce[0]);

				d(WhenShellLoaded.Subscribe(_ => log.Trace($"{this} loaded")));
				d(WhenShellActivated.Subscribe(_ => log.Trace($"{this} activated")));
				d(WhenShellDeactivated.Subscribe(_ => log.Trace($"{this} deactivated")));
				d(WhenShellClosing.Subscribe(_ => log.Trace($"{this} closing")));
				d(WhenShellClosed.Subscribe(_ => log.Trace($"{this} closed")));

				ViewModel?.Activate().Wait();
			});
		}

		public IObservable<Unit> WhenShellLoaded => this.Events().Loaded.Select(x => Unit.Default);

		public IObservable<Unit> WhenShellActivated => this.Events().Activated.Select(x => Unit.Default);

		public IObservable<Unit> WhenShellDeactivated => this.Events().Deactivated.Select(x => Unit.Default);

		public IObservable<CancelEventArgs> WhenShellClosing => this.Events().Closing;

		public IObservable<Unit> WhenShellClosed => this.Events().Closed.Select(x => Unit.Default);

		private IShellViewModel ViewModel => DataContext as IShellViewModel;

		public bool ForceClose { get; private set; }

		public bool IsClosed { get; private set; }

		object IViewFor.ViewModel
		{
			get => DataContext;
			set => DataContext = (IShellViewModel) value;
		}
	}
}
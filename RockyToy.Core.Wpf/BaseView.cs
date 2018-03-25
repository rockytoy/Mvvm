using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Autofac;
using ReactiveUI;
using RockyToy.Contracts.Common;
using RockyToy.Contracts.Wpf;

namespace RockyToy.Core.Wpf
{
	public abstract class BaseView : UserControl, IView
	{
		protected BaseView()
		{
			IDisposable[] observeActivatedOnce = {null};

			observeActivatedOnce[0] = this.WhenActivated(d =>
			{
				var log = ((IApp)Application.Current).Root.Resolve<Func<Type, ILogger>>()(typeof(BaseShellView));
				log.Trace($"{this} loaded");

				d(WhenViewLoaded
					.Select(_ => ViewModel.Activate().ToObservable().Take(1))
					.Concat()
					.Subscribe());
				d(WhenViewUnloaded
					.Select(_ => ViewModel.Deactivate(false).ToObservable().Take(1))
					.Concat()
					.Subscribe());
				d(observeActivatedOnce[0]);


				WhenViewLoaded.Subscribe(_ => log.Trace($"{this} loaded"));
				WhenViewUnloaded.Subscribe(_ => log.Trace($"{this} unloaded"));

				ViewModel.Activate().Wait();
			});
		}

		public IObservable<Unit> WhenViewLoaded => this.Events().Loaded.Select(_ => Unit.Default);

		public IObservable<Unit> WhenViewUnloaded => this.Events().Unloaded.Select(_ => Unit.Default);

		private IViewModel ViewModel => DataContext as IViewModel;

		object IViewFor.ViewModel
		{
			get => DataContext;
			set => DataContext = value;
		}
	}
}
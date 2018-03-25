using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using Autofac.Features.Indexed;
using RockyToy.Contracts.Wpf;

namespace RockyToy.Core.Wpf
{
	public class ViewHelper : IViewHelper
	{
		private readonly IIndex<Type, Func<Type, IScreenView>> _screenViewLocator;
		private readonly IIndex<Type, Func<Type, IShellView>> _shellViewLocator;
		private readonly IIndex<Type, Func<Type, IView>> _viewLocator;

		public ViewHelper(IIndex<Type, Func<Type, IView>> viewLocator, IIndex<Type, Func<Type, IShellView>> shellViewLocator,
			IIndex<Type, Func<Type, IScreenView>> screenViewLocator)
		{
			_viewLocator = viewLocator;
			_shellViewLocator = shellViewLocator;
			_screenViewLocator = screenViewLocator;

		}

		public void ActivateShell(IShellView shell)
		{
			(shell as Window)?.Activate();
		}

		public IShellView GetActiveShellView()
		{
			return Application.Current.Windows.OfType<IShellView>().Where(x =>
			{
				if (x.ViewModel is IShellViewModel vm)
					return (vm.VmState == VmStateEnum.Deactivated || vm.VmState == VmStateEnum.Initialized ||
					        vm.VmState == VmStateEnum.Activated) && vm.LastActivedDateTime != null;
				return false;
			}).OrderByDescending(x => (x.ViewModel as IShellViewModel)?.LastActivedDateTime).FirstOrDefault();
		}

		public IShellViewModel GetActiveShellViewModel()
		{
			return (GetActiveShellView() as Window)?.DataContext as IShellViewModel;
		}
		
		public IShellView GetMainShellView()
		{
			return Application.Current.MainWindow as IShellView;
		}

		public IShellViewModel GetMainShellViewModel()
		{
			return (GetMainShellView() as Window)?.DataContext as IShellViewModel;
		}

		private IEnumerable<IView> FindVisualView(DependencyObject depObj)
		{
			if (depObj == null)
				yield break;

			for (var i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
			{
				var child = VisualTreeHelper.GetChild(depObj, i);
				if (child is IView)
					yield return child as IView;

				foreach (var childOfChild in FindVisualView(child))
					yield return childOfChild;
			}
		}

		public IShellView GetShellViewOf(IViewModel viewModel)
		{
			if (viewModel is IShellViewModel shellVm)
				return GetShellViewOf(shellVm);

			var candidateShells = Application.Current.Windows.OfType<IShellView>().Where(x =>
			{
				if (x.IsClosed)
					return false;
				if (x.ViewModel is IShellViewModel vm)
					return (vm.VmState == VmStateEnum.Deactivated || vm.VmState == VmStateEnum.Initialized ||
					        vm.VmState == VmStateEnum.Activated) && vm.LastActivedDateTime != null;
				return false;
			}).ToList();

			// only single shells?
			if (candidateShells.Count == 1)
				return candidateShells.First();

			foreach (var shell in candidateShells)
			{
				if (!(shell is Window win))
					continue;

				var views = FindVisualView(win);
				if (views.Any(v => ReferenceEquals(v.ViewModel, viewModel)))
					return shell;
			}

			// not found? return active shell
			return GetActiveShellView();
		}

		public IShellView GetShellViewOf(IShellViewModel viewModel)
		{
			return Application.Current.Windows.OfType<IShellView>().Where(x =>
			{
				if (x.IsClosed)
					return false;
				if (x.ViewModel is IShellViewModel vm && ReferenceEquals(vm, viewModel))
					return (vm.VmState == VmStateEnum.Deactivated || vm.VmState == VmStateEnum.Initialized ||
					        vm.VmState == VmStateEnum.Activated) && vm.LastActivedDateTime != null;
				return false;
			}).FirstOrDefault();
		}

		public IView CreateViewFor<TViewModel>(TViewModel viewModel) where TViewModel : IViewModel
		{
			if (viewModel is IScreenViewModel svm) return CreateViewForScreen(svm);

			return _viewLocator[viewModel.GetType()](viewModel.GetType());
		}

		public async Task<IShellView> CreateShellForScreen<TScreenViewModel>(
			IShellViewModel ownerShellVm, TScreenViewModel viewModel)
			where TScreenViewModel : IScreenViewModel
		{
			var simpleShellVm = new SimpleShellViewModel();
			var shellView = CreateShellFor(simpleShellVm);
			var ownerShellView = GetShellViewOf(ownerShellVm);
			if (ownerShellView is Window ownerWin && shellView is Window curWin) curWin.Owner = ownerWin;

			shellView.ViewModel = simpleShellVm;

			var showVm = await simpleShellVm.ShowScreen(() => viewModel);
			if (showVm.Error != null)
				throw showVm.Error;

			return shellView;
		}

		public IShellView CreateShellFor<TShellViewModel>(TShellViewModel shellViewModel)
			where TShellViewModel : IShellViewModel
		{
			return _shellViewLocator[shellViewModel.GetType()](shellViewModel.GetType());
		}

		public IScreenView CreateViewForScreen<TScreenViewModel>(TScreenViewModel viewModel)
			where TScreenViewModel : IScreenViewModel
		{
			return _screenViewLocator[viewModel.GetType()](viewModel.GetType());
		}
	}
}
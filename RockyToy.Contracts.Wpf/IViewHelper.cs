using System.Threading.Tasks;

namespace RockyToy.Contracts.Wpf
{
	public interface IViewHelper
	{
		void ActivateShell(IShellView shell);
		IShellView GetActiveShellView();
		IShellViewModel GetActiveShellViewModel();
		IShellView GetMainShellView();
		IShellViewModel GetMainShellViewModel();
		IShellView GetShellViewOf(IViewModel viewModel);
		IShellView GetShellViewOf(IShellViewModel viewModel);
		IView CreateViewFor<TViewModel>(TViewModel viewModel) where TViewModel : IViewModel;

		Task<IShellView> CreateShellForScreen<TScreenViewModel>(
			IShellViewModel ownerShellVm, TScreenViewModel viewModel)
			where TScreenViewModel : IScreenViewModel;

		IShellView CreateShellFor<TShellViewModel>(TShellViewModel shellViewModel) where TShellViewModel : IShellViewModel;
	}
}
using System.Reactive;
using System.Threading.Tasks;
using System.Windows;
using Autofac;
using ReactiveUI;
using RockyToy.Contracts.Common;
using RockyToy.Contracts.Wpf;

namespace RockyToy.Core.Wpf
{

	public abstract class BaseScreenViewModel : BaseViewModel, IScreenViewModel
	{
		private string _title = "No Title";

		protected BaseScreenViewModel()
		{
			TryCloseCommand = ReactiveCommand.CreateFromTask(TryClose);
		}

		public virtual ReactiveCommand<Unit, IActionResults> TryCloseCommand { get; }

		public virtual async Task<IActionResults> TryClose()
		{
			var arf = ((IApp) Application.Current).Root.Resolve<IActionResultFactory>();

			await Deactivate(true);
			if (VmState == VmStateEnum.Closed)
				return arf.SuccessActions();
			return arf.CancelActions();
		}

		public string Title
		{
			get => _title;
			set => this.SetProperty(ref _title, value);
		}
	}
}
using System;
using System.Threading.Tasks;
using System.Windows;
using Autofac;
using RockyToy.Contracts.Common;
using RockyToy.Contracts.Wpf;

namespace RockyToy.Core.Wpf
{
	public abstract class BaseShellViewModel : BaseViewModel, IShellViewModel
	{
		private IScreenViewModel _activeScreen;
		private DateTimeOffset? _lastActivedDateTime;
		private string _title;
		
		public string Title
		{
			get => _title;
			set => this.SetProperty(ref _title, value);
		}

		public virtual async Task<IActionResults> TryClose()
		{
			var arf = ((IApp) Application.Current).Root.Resolve<IActionResultFactory>();
			if (ActiveScreen != null)
			{
				var activeResult = await ActiveScreen.TryClose();
				if (activeResult.Status != ActionStatus.Success)
					return activeResult;
			}

			await Deactivate(true);
			if (VmState == VmStateEnum.Closed)
				return arf.SuccessActions();
			return arf.CancelActions();
		}

		public IScreenViewModel ActiveScreen
		{
			get => _activeScreen;
			private set => this.SetProperty(ref _activeScreen, value);
		}

		public DateTimeOffset? LastActivedDateTime
		{
			get => _lastActivedDateTime;
			set => this.SetProperty(ref _lastActivedDateTime, value);
		}

		public async Task<IActionResults> ShowScreen(Func<IScreenViewModel> screen)
		{
			var arf = ((IApp) Application.Current).Root.Resolve<IActionResultFactory>();

			var nextScreen = screen();
			if (ActiveScreen != null && !ReferenceEquals(ActiveScreen, nextScreen))
			{
				var closingOldScreen = await ActiveScreen.TryClose();
				if (closingOldScreen.Status != ActionStatus.Success)
					return closingOldScreen;
			}
			else if (ActiveScreen != null && ReferenceEquals(ActiveScreen, nextScreen))
			{
				if (VmState == VmStateEnum.Activated)
					await ActiveScreen.Activate();
				return arf.SuccessActions();
			}

			ActiveScreen = nextScreen;
			if (VmState == VmStateEnum.Activated)
				await ActiveScreen.Activate();
			return arf.SuccessActions();
		}
	}
}
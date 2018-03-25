using System.Threading.Tasks;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using RockyToy.Contracts.Wpf;

namespace RockyToy.Core.Wpf.MetroDialog
{
	public class MetroDialogCoordinator : IDialogCoordinator
	{
		private readonly IViewHelper _viewHelper;

		public MetroDialogCoordinator(IViewHelper viewHelper)
		{
			_viewHelper = viewHelper;
		}

		public MetroWindow GetMetroWindow(object context)
		{
			if (context == null)
				return _viewHelper.GetActiveShellView() as MetroWindow;
			if (context is IViewModel vm)
				return _viewHelper.GetShellViewOf(vm) as MetroWindow;
			return _viewHelper.GetActiveShellView() as MetroWindow;
		}

		public Task<string> ShowInputAsync(object context, string title, string message, MetroDialogSettings settings = null)
		{
			var metroWindow = GetMetroWindow(context);
			return metroWindow.Invoke(() => metroWindow.ShowInputAsync(title, message, settings));
		}

		public string ShowModalInputExternal(object context, string title, string message, MetroDialogSettings settings = null)
		{
			var metroWindow = GetMetroWindow(context);
			return metroWindow.ShowModalInputExternal(title, message, settings);
		}

		public Task<LoginDialogData> ShowLoginAsync(object context, string title, string message, LoginDialogSettings settings = null)
		{
			var metroWindow = GetMetroWindow(context);
			return metroWindow.Invoke(() => metroWindow.ShowLoginAsync(title, message, settings));
		}

		public LoginDialogData ShowModalLoginExternal(object context, string title, string message,
			LoginDialogSettings settings = null)
		{
			var metroWindow = GetMetroWindow(context);
			return metroWindow.ShowModalLoginExternal(title, message, settings);
		}

		public Task<MessageDialogResult> ShowMessageAsync(object context, string title, string message, MessageDialogStyle style = MessageDialogStyle.Affirmative,
			MetroDialogSettings settings = null)
		{
			var metroWindow = GetMetroWindow(context);
			return metroWindow.Invoke(() => metroWindow.ShowMessageAsync(title, message, style, settings));
		}

		public MessageDialogResult ShowModalMessageExternal(object context, string title, string message,
			MessageDialogStyle style = MessageDialogStyle.Affirmative, MetroDialogSettings settings = null)
		{
			var metroWindow = GetMetroWindow(context);
			return metroWindow.ShowModalMessageExternal(title, message, style, settings);
		}

		public Task<ProgressDialogController> ShowProgressAsync(object context, string title, string message, bool isCancelable = false,
			MetroDialogSettings settings = null)
		{
			var metroWindow = GetMetroWindow(context);
			return metroWindow.Invoke(() => metroWindow.ShowProgressAsync(title, message, isCancelable, settings));
		}

		public Task ShowMetroDialogAsync(object context, BaseMetroDialog dialog, MetroDialogSettings settings = null)
		{
			var metroWindow = GetMetroWindow(context);
			return metroWindow.Invoke(() => metroWindow.ShowMetroDialogAsync(dialog, settings));
		}

		public Task HideMetroDialogAsync(object context, BaseMetroDialog dialog, MetroDialogSettings settings = null)
		{
			var metroWindow = GetMetroWindow(context);
			return metroWindow.Invoke(() => metroWindow.HideMetroDialogAsync(dialog, settings));
		}

		public Task<TDialog> GetCurrentDialogAsync<TDialog>(object context) where TDialog : BaseMetroDialog
		{
			var metroWindow = GetMetroWindow(context);
			return metroWindow.Invoke(() => metroWindow.GetCurrentDialogAsync<TDialog>());
		}
	}
}
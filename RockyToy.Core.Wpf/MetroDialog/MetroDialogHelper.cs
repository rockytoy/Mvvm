using System;
using System.Threading;
using System.Threading.Tasks;
using MahApps.Metro.Controls.Dialogs;
using RockyToy.Contracts.Common;
using RockyToy.Contracts.Common.Extensions;
using RockyToy.Contracts.Wpf.MetroDialog;

namespace RockyToy.Core.Wpf.MetroDialog
{
	public class MetroDialogHelper : IMetroDialogHelper
	{
		private readonly IDialogCoordinator _dialog;
		private readonly IActionResultFactory _arf;

		public MetroDialogHelper(IDialogCoordinator dialog, IActionResultFactory arf)
		{
			_dialog = dialog;
			_arf = arf;
		}

		public async Task<ProgressDialogController> GetProgressDialog(MessageDialogOption options)
		{
			return await _dialog.ShowProgressAsync(options.Context,
				options.Title,
				options.Message,
				options.CancellationToken != default(CancellationToken),
				options);
		}

		public async Task<IActionResults> BusyTask(BusyDialogOptions options)
		{
			var controller = await _dialog.ShowProgressAsync(options.Context,
				options.Title,
				options.Message,
				options.CancellationToken != default(CancellationToken),
				options);

			var progression = new Progress<Progression>(p =>
			{
				if (p.Message != null)
					controller.SetMessage(p.Message);
				if (p.Value == null)
					controller.SetIndeterminate();
				else
					controller.SetProgress(p.Value.Value);
			});

			controller.SetIndeterminate();
			IActionResults ret;

			try
			{
				await options.Task(progression, options.CancellationToken);
				ret = _arf.SuccessActions(options.Title);
			}
			catch (Exception e) when (e.IsCancellation())
			{
				await _dialog.ShowMessageAsync(options.Context, options.Title, "Cancelled");
				ret = _arf.CancelActions(options.Title);
			}
			catch (Exception e)
			{
				if (!(e is IDoNotLog))
					options.Log?.ErrorException($"Error while processing {options.Title} in {options.Context.GetType().FullName}", e);
				await _dialog.ShowMessageAsync(options.Context, $"Error while {options.Title}", e.Message);
				ret = _arf.ErrorActions(e, options.Title);
			}

			await controller.CloseAsync();
			return ret;
		}

		public async Task<IActionResults> BusyTask(BusyDialogOptionsWithResults options)
		{
			var controller = await _dialog.ShowProgressAsync(options.Context,
				options.Title,
				options.Message,
				options.CancellationToken != default(CancellationToken),
				options);

			var progression = new Progress<Progression>(p =>
			{
				if (p.Message != null)
					controller.SetMessage(p.Message);
				if (p.Value == null)
					controller.SetIndeterminate();
				else
					controller.SetProgress(p.Value.Value);
			});

			controller.SetIndeterminate();
			IActionResults ret;

			try
			{
				ret = await options.Task(progression, options.CancellationToken);
				ret.ThrowIfError();
			}
			catch (Exception e) when (e.IsCancellation())
			{
				await _dialog.ShowMessageAsync(options.Context, options.Title, "Cancelled");
				ret = _arf.CancelActions(options.Title);
			}
			catch (Exception e)
			{
				if (!(e is IDoNotLog))
					options.Log?.ErrorException($"Error while processing {options.Title} in {options.Context.GetType().FullName}", e);
				await _dialog.ShowMessageAsync(options.Context, $"Error while {options.Title}", e.Message);
				ret = _arf.ErrorActions(e, options.Title);
			}

			await controller.CloseAsync();
			return ret;
		}

		public async Task<IActionResult<TResult>> BusyTask<TResult>(BusyDialogOptions<TResult> options)
		{
			var controller = await _dialog.ShowProgressAsync(options.Context,
				options.Title,
				options.Message,
				options.CancellationToken != default(CancellationToken),
				options);

			var progression = new Progress<Progression>(p =>
			{
				if (p.Message != null)
					controller.SetMessage(p.Message);
				if (p.Value == null)
					controller.SetIndeterminate();
				else
					controller.SetProgress(p.Value.Value);
			});

			controller.SetIndeterminate();
			IActionResult<TResult> ret;

			try
			{
				ret = _arf.SuccessAction(await options.Task(progression, options.CancellationToken), options.Title);
			}
			catch (Exception e) when (e.IsCancellation())
			{
				await _dialog.ShowMessageAsync(options.Context, options.Title, "Cancelled");
				ret = _arf.CancelAction<TResult>(options.Title);
			}
			catch (Exception e)
			{
				if (!(e is IDoNotLog))
					options.Log?.ErrorException($"Error while processing {options.Title} in {options.Context.GetType().FullName}", e);
				await _dialog.ShowMessageAsync(options.Context, $"Error while {options.Title}", e.Message);
				ret = _arf.ErrorAction<TResult>(e, options.Title);
			}

			await controller.CloseAsync();
			return ret;
		}

		public async Task ShowMessage(MessageDialogOption options)
		{
			await _dialog.ShowMessageAsync(options.Context, options.Title, options.Message, MessageDialogStyle.Affirmative,
				options);
		}

		public async Task<DialogButton> ShowChoice(ChoiceDialogOption options)
		{
			var style = MessageDialogStyle.Affirmative;
			options.AffirmativeButtonText = "Ok";
			options.NegativeButtonText = "Cancel";
			options.FirstAuxiliaryButtonText = "Cancel";
			options.DefaultButtonFocus = MessageDialogResult.Affirmative;
			switch (options.Buttons)
			{
				default:
				case DialogButtons.Ok:
					break;
				case DialogButtons.OkCancel:
					style = MessageDialogStyle.AffirmativeAndNegative;
					if (options.DefaultButton == DialogButton.No || options.DefaultButton == DialogButton.Cancel)
						options.DefaultButtonFocus = MessageDialogResult.Negative;
					break;
				case DialogButtons.YesNo:
					style = MessageDialogStyle.AffirmativeAndNegative;
					options.AffirmativeButtonText = "Yes";
					options.NegativeButtonText = "No";
					if (options.DefaultButton == DialogButton.No || options.DefaultButton == DialogButton.Cancel)
						options.DefaultButtonFocus = MessageDialogResult.Negative;
					break;
				case DialogButtons.YesNoCancel:
					style = MessageDialogStyle.AffirmativeAndNegativeAndSingleAuxiliary;
					options.AffirmativeButtonText = "Yes";
					options.NegativeButtonText = "No";
					if (options.DefaultButton == DialogButton.No)
						options.DefaultButtonFocus = MessageDialogResult.Negative;
					if (options.DefaultButton == DialogButton.Cancel)
						options.DefaultButtonFocus = MessageDialogResult.FirstAuxiliary;
					break;
			}

			var ret = await _dialog.ShowMessageAsync(options.Context, options.Title, options.Message, style, options);

			switch (ret)
			{
				case MessageDialogResult.Negative:
					if (options.Buttons == DialogButtons.YesNoCancel || options.Buttons == DialogButtons.YesNo)
						return DialogButton.No;
					return DialogButton.Cancel;
				case MessageDialogResult.Affirmative:
					if (options.Buttons == DialogButtons.YesNoCancel || options.Buttons == DialogButtons.YesNo)
						return DialogButton.Yes;
					return DialogButton.Ok;
				case MessageDialogResult.FirstAuxiliary:
					return DialogButton.Cancel;
			}

			return DialogButton.Cancel;
		}

		public async Task<string> ShowInputString(InputStringDialogOption options)
		{
			return await _dialog.ShowInputAsync(options.Context, options.Title, options.Message, options);
		}
	}
}
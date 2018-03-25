using System.Threading.Tasks;
using MahApps.Metro.Controls.Dialogs;
using RockyToy.Contracts.Common;

namespace RockyToy.Contracts.Wpf.MetroDialog
{
	public interface IMetroDialogHelper
	{
		Task<ProgressDialogController> GetProgressDialog(MessageDialogOption options);
		Task<IActionResults> BusyTask(BusyDialogOptions options);
		Task<IActionResults> BusyTask(BusyDialogOptionsWithResults options);
		Task<IActionResult<TResult>> BusyTask<TResult>(BusyDialogOptions<TResult> options);
		Task ShowMessage(MessageDialogOption options);
		Task<DialogButton> ShowChoice(ChoiceDialogOption options);
		Task<string> ShowInputString(InputStringDialogOption options);
	}
}
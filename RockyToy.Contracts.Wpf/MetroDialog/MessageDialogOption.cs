using MahApps.Metro.Controls.Dialogs;

namespace RockyToy.Contracts.Wpf.MetroDialog
{
	public class MessageDialogOption : CustomMetroDialogSettings
	{
		public MessageDialogOption()
		{
			DefaultButtonFocus = MessageDialogResult.Affirmative;
		}
	}
}
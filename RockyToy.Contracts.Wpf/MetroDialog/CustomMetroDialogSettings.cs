using MahApps.Metro.Controls.Dialogs;

namespace RockyToy.Contracts.Wpf.MetroDialog
{
	public class CustomMetroDialogSettings : MetroDialogSettings
	{
		public CustomMetroDialogSettings()
		{
			AnimateHide = false;
			AnimateShow = false;
			OwnerCanCloseWithDialog = false;
		}
		public IViewModel Context { get; set; }
		public string Title { get; set; }
		public string Message { get; set; }
	}
}
namespace RockyToy.Contracts.Wpf.MetroDialog
{
	public class ChoiceDialogOption : CustomMetroDialogSettings
	{
		public DialogButtons Buttons { get; set; }
		public DialogButton DefaultButton { get; set; }
	}
}
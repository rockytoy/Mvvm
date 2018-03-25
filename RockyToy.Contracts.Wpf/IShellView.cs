namespace RockyToy.Contracts.Wpf
{
	public interface IShellView : IView
	{
		bool ForceClose { get; }
		bool IsClosed { get; }
		void Show();
	}
}
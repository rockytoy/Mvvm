using ReactiveUI;

namespace RockyToy.Contracts.Common
{
	public interface IBaseReactive : IReactiveNotifyPropertyChanged<IReactiveObject>, IHandleObservableErrors,
		IReactiveObject
	{
	}
}
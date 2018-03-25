using System.Threading.Tasks;
using RockyToy.Contracts.Common;

namespace RockyToy.Contracts.Wpf
{
	public interface IClosableViewModel : IViewModel
	{
		Task<IActionResults> TryClose();
	}
}
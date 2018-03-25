using RockyToy.Contracts.Common;
using RockyToy.Contracts.Wpf.Common;

namespace RockyToy.Core.Wpf.Common
{
	public class CheckableData<T> : DisplayableData<T>, ICheckableData
	{
		private bool? _isChecked;

		public CheckableData(string desc, T val, bool? isChecked = false) : base(desc, val)
		{
			_isChecked = isChecked;
		}

		public bool? IsChecked
		{
			get => _isChecked;
			set => this.SetProperty(ref _isChecked, value);
		}
	}
}
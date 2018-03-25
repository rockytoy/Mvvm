using System;
using System.ComponentModel;

namespace RockyToy.Contracts.Wpf.Common
{
	public interface IDisplayableData : INotifyPropertyChanged, IEquatable<IDisplayableData>
	{
		string DisplayString { get; }
		object DisplayValue { get; }
	}
}
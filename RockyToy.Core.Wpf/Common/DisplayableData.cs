using System.Collections.Generic;
using RockyToy.Contracts.Wpf.Common;
using RockyToy.Core.Common;

namespace RockyToy.Core.Wpf.Common
{
	public class DisplayableData<T> : BaseReactive, IDisplayableData
	{
		public DisplayableData(string desc, T val)
		{
			DisplayString = desc;
			DisplayData = val;
		}

		public T DisplayData { get; }

		public string DisplayString { get; }
		public object DisplayValue => DisplayData;

		public bool Equals(IDisplayableData other)
		{
			return Equals(other as object);
		}

		public static string NoneDesc()
		{
			// TODO: use Eivf.Lang
			return "None";
			//return $"({LanguageManager.GetString("lang.ui.None", "None")})";
		}

		public static string AllDesc()
		{
			// TODO: use Eivf.Lang
			return "All";
			//return $"({LanguageManager.GetString("lang.ui.All", "All")})";
		}

		public bool Equals(DisplayableData<T> other)
		{
			return EqualityComparer<T>.Default.Equals(DisplayData, other.DisplayData);
		}

		public override bool Equals(object obj)
		{
			if (obj is null) return false;
			if (ReferenceEquals(this, obj)) return true;
			return obj.GetType() == GetType() && Equals((DisplayableData<T>) obj);
		}

		public override int GetHashCode()
		{
			return EqualityComparer<T>.Default.GetHashCode(DisplayData);
		}

		public static bool operator ==(DisplayableData<T> a, DisplayableData<T> b)
		{
			return Equals(a, b);
		}

		public static bool operator !=(DisplayableData<T> a, DisplayableData<T> b)
		{
			return !(a == b);
		}
	}
}
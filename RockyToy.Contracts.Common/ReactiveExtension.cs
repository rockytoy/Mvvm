using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using ReactiveUI;

namespace RockyToy.Contracts.Common
{
	public static class ReactiveExtension
	{
		public static void SetProperty<T>(this IBaseReactive This, ref T prop, T value,
			[CallerMemberName] string propName = null)
		{
			This.RaiseAndSetIfChanged(ref prop, value, propName);
		}

		public static void SetProperty<TOwner, TProp>(this TOwner This, Expression<Func<TOwner, TProp>> prop, TProp value)
			where TOwner : IBaseReactive
		{
			var curVal = prop.Compile().Invoke(This);
			if (EqualityComparer<TProp>.Default.Equals(curVal, value))
				return;
			MemberExpression me;
			switch (prop.Body.NodeType)
			{
				case ExpressionType.Convert:
				case ExpressionType.ConvertChecked:
					var ue = prop.Body as UnaryExpression;
					me = ue?.Operand as MemberExpression;
					break;
				default:
					me = prop.Body as MemberExpression;
					break;
			}

			if (me == null)
				throw new ArgumentException("not a member expression", nameof(prop));

			// gather nested reactive object
			var nestedReact = new List<KeyValuePair<IBaseReactive, string>>();
			object lastOwner = null;
			object curOwner = This;
			PropertyInfo pi = null;
			while (me != null)
			{
				if (curOwner is IBaseReactive curReactive)
					nestedReact.Add(new KeyValuePair<IBaseReactive, string>(curReactive, me.Member.Name));

				pi = me.Member as PropertyInfo;
				if (pi == null)
					throw new ArgumentException("no a property info", nameof(prop));

				lastOwner = curOwner;
				curOwner = pi.GetValue(curOwner);
				me = me.Expression as MemberExpression;
			}

			// call it in reverse order
			nestedReact.Reverse();

			foreach (var o in nestedReact)
				o.Key.RaisePropertyChanging(o.Value);

			pi.SetValue(lastOwner, value);

			foreach (var o in nestedReact)
				o.Key.RaisePropertyChanged(o.Value);
		}
	}
}
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Interactivity;
using System.Windows.Threading;

namespace RockyToy.Resources.Wpf.Behaviors
{
	/// <summary>
	///   make SelectedItem cancel-able
	///   ref: https://stackoverflow.com/questions/7800032/cancel-combobox-selection-in-wpf-with-mvvm
	/// </summary>
	public class SelectorBehavior : Behavior<Selector>
	{
		/// <summary>
		///   attach <see cref="Selector.SelectionChanged" /> event to <see cref="Behavior.AssociatedObject" />
		/// </summary>
		protected override void OnAttached()
		{
			base.OnAttached();
			AssociatedObject.SelectionChanged += OnSelectionChanged;
		}

		/// <summary>
		///   detach <see cref="Selector.SelectionChanged" /> event to <see cref="Behavior.AssociatedObject" />
		/// </summary>
		protected override void OnDetaching()
		{
			AssociatedObject.SelectionChanged -= OnSelectionChanged;
			base.OnDetaching();
		}

		#region Cancellable SelectedItem property

		/// <summary>
		///   bind to this property instead of normal SelectedItem to make it cancel-able
		/// </summary>
		public static readonly DependencyProperty SelectedItemProperty =
			DependencyProperty.Register("SelectedItem", typeof(object), typeof(SelectorBehavior),
				new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnSelectedItemChanged));

		/// <summary>
		///   bind to this property instead of normal SelectedItem to make it cancel-able
		/// </summary>
		public object SelectedItem
		{
			get => GetValue(SelectedItemProperty);
			set => SetValue(SelectedItemProperty, value);
		}

		/// <summary>
		///   Silently ignore setting SelectedItem to <code>null</code>.
		///   Only work on <see cref="ComboBox" />.
		///   Default is true.
		/// </summary>
		public static readonly DependencyProperty IgnoreNullSelectionProperty =
			DependencyProperty.Register("IgnoreNullSelection", typeof(bool), typeof(SelectorBehavior),
				new PropertyMetadata(true));

		/// <summary>
		///   Determines whether null selection (which usually occurs since the combobox is rebuilt or its list is refreshed)
		///   should be ignored.
		///   True by default.
		/// </summary>
		public bool IgnoreNullSelection
		{
			get => (bool) GetValue(IgnoreNullSelectionProperty);
			set => SetValue(IgnoreNullSelectionProperty, value);
		}

		/// <summary>
		///   Called when the SelectedItem dependency property is changed.
		///   Updates the associated selector's SelectedItem with the new value.
		/// </summary>
		private static void OnSelectedItemChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var behavior = (SelectorBehavior) d;

			if (behavior.AssociatedObject == null)
				System.Windows.Threading.Dispatcher.CurrentDispatcher.BeginInvoke(new Action(() =>
				{
					var selector = behavior.AssociatedObject;
					if (selector == null)
						return;
					selector.SelectedItem = e.NewValue;
				}));
			else
				behavior.AssociatedObject.SelectedItem = e.NewValue;
		}

		/// <summary>
		///   Called when the associated selector's selection is changed.
		///   Tries to assign it to the <see cref="SelectedItem" /> property.
		///   If it fails, updates the selector's with  <see cref="SelectedItem" /> property's current value.
		/// </summary>
		private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (IgnoreNullSelection && (e.AddedItems == null || e.AddedItems.Count == 0))
				return;

			SelectedItem = AssociatedObject.SelectedItem;
			if (SelectedItem != AssociatedObject.SelectedItem)
				Dispatcher.BeginInvoke(DispatcherPriority.Normal,
					new Action(() => { AssociatedObject.SelectedItem = SelectedItem; }));
		}

		#endregion
	}
}
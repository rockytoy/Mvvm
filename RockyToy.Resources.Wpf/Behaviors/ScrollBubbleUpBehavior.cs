using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interactivity;
using RockyToy.Resources.Wpf.Resources;

namespace RockyToy.Resources.Wpf.Behaviors
{
	/// <summary>
	///   bubble scrolling event ONLY if the child is at the top and scrolling up or the bottom and scrolling down
	///   ref: https://stackoverflow.com/questions/1585462/bubbling-scroll-events-from-a-listview-to-its-parent
	/// </summary>
	public class ScrollBubbleUpBehavior : Behavior<FrameworkElement>
	{
		protected override void OnAttached()
		{
			base.OnAttached();
			AssociatedObject.PreviewMouseWheel += PreviewMouseWheel;
		}

		protected override void OnDetaching()
		{
			AssociatedObject.PreviewMouseWheel -= PreviewMouseWheel;
			base.OnDetaching();
		}

		private void PreviewMouseWheel(object sender, MouseWheelEventArgs e)
		{
			var scrollViewer = ResHelper.FindVisualChildren<ScrollViewer>(AssociatedObject).FirstOrDefault();
			if (scrollViewer == null)
				return;

			const float tolerance = 1e-4f;

			var scrollPos = scrollViewer.ContentVerticalOffset;
			if (
				(!(Math.Abs(scrollPos - scrollViewer.ScrollableHeight) < tolerance) || e.Delta >= 0)
				&& (!(Math.Abs(scrollPos) < tolerance) || e.Delta <= 0)
			)
				return;

			if (!(((Control) sender).Parent is UIElement parent))
				return;

			e.Handled = true;
			var e2 = new MouseWheelEventArgs(e.MouseDevice, e.Timestamp, e.Delta)
			{
				RoutedEvent = UIElement.MouseWheelEvent,
				Source = sender
			};
			parent.RaiseEvent(e2);
		}
	}
}
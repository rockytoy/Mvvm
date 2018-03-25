using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media;

namespace RockyToy.Resources.Wpf.Resources
{
	public static class ResHelper
	{
		private static bool? _inDesignMode;

		/// <summary>
		///   Gets a value that indicates whether the process is running in design mode.
		/// </summary>
		public static bool InDesignMode
		{
			get
			{
				if (_inDesignMode == null)
				{
					var descriptor =
						DependencyPropertyDescriptor.FromProperty(DesignerProperties.IsInDesignModeProperty, typeof(FrameworkElement));
					_inDesignMode = (bool) descriptor.Metadata.DefaultValue;
				}

				return _inDesignMode ?? false;
			}
		}

		/// <summary>
		///   return the nearest ancestor with type <typeparamref name="T" /> for <paramref name="depObj" /> in logical tree.
		///   ref:
		///   http://www.infragistics.com/community/blogs/blagunas/archive/2013/05/29/find-the-parent-control-of-a-specific-type-in-wpf-and-silverlight.aspx
		/// </summary>
		/// <typeparam name="T">Target ancester type</typeparam>
		/// <param name="depObj">an object in logical tree</param>
		/// <returns>The nearest ancestor with type <typeparamref name="T" /> or null if not found</returns>
		public static T FindLogicalParent<T>(DependencyObject depObj) where T : DependencyObject
		{
			while (true)
			{
				if (depObj == null)
					return null;

				//get parent item
				var parentObject = LogicalTreeHelper.GetParent(depObj);

				//we've reached the end of the tree
				if (parentObject == null)
					return null;

				//check if the parent matches the type we're looking for
				if (parentObject is T parent)
					return parent;
				depObj = parentObject;
			}
		}

		/// <summary>
		///   return the nearest ancestor with type <typeparamref name="T" /> for <paramref name="depObj" /> in visual tree.
		///   ref:
		///   http://www.infragistics.com/community/blogs/blagunas/archive/2013/05/29/find-the-parent-control-of-a-specific-type-in-wpf-and-silverlight.aspx
		/// </summary>
		/// <typeparam name="T">Target ancester type</typeparam>
		/// <param name="depObj">an object in visual tree</param>
		/// <returns>The nearest ancestor with type <typeparamref name="T" /> or null if not found</returns>
		public static T FindVisualParent<T>(DependencyObject depObj) where T : DependencyObject
		{
			while (true)
			{
				if (depObj == null)
					return null;

				//get parent item
				var parentObject = VisualTreeHelper.GetParent(depObj);

				//we've reached the end of the tree
				if (parentObject == null)
					return null;

				//check if the parent matches the type we're looking for
				if (parentObject is T parent)
					return parent;
				depObj = parentObject;
			}
		}

		/// <summary>
		///   return all descendant with type <typeparamref name="T" /> for <paramref name="depObj" /> in visual tree.
		///   ref: http://stackoverflow.com/questions/974598/find-all-controls-in-wpf-window-by-type
		/// </summary>
		/// <typeparam name="T">Target descendant type</typeparam>
		/// <param name="depObj">an object in visual tree</param>
		/// <returns>All descendant with type <typeparamref name="T" /></returns>
		public static IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
		{
			if (depObj == null)
				yield break;

			for (var i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
			{
				var child = VisualTreeHelper.GetChild(depObj, i);
				if (child is T children)
					yield return children;

				foreach (var childOfChild in FindVisualChildren<T>(child))
					yield return childOfChild;
			}
		}
	}
}
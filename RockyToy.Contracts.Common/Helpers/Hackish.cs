using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;

namespace RockyToy.Contracts.Common.Helpers
{
	[SuppressMessage("ReSharper", "InconsistentNaming")]
	public static class Hackish
	{
		public static IntPtr GetWindowHandle(Window window)
		{
			return new WindowInteropHelper(window).Handle;
		}

		#region window native disable

		private const int GWL_STYLE = -16;
		private const int WS_DISABLED = 0x08000000;

		[DllImport("user32.dll")]
		private static extern int GetWindowLong(IntPtr hWnd, int nIndex);

		[DllImport("user32.dll")]
		private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

		/// <summary>
		///   ref : https://stackoverflow.com/questions/428494/is-it-possible-to-use-showdialog-without-blocking-all-forms
		/// </summary>
		/// <param name="window"></param>
		/// <param name="value"></param>
		public static void SetWindowNativeEnabled(Window window, bool value)
		{
			var hWnd = GetWindowHandle(window);
			SetWindowLong(hWnd, GWL_STYLE, (GetWindowLong(hWnd, GWL_STYLE) & ~WS_DISABLED) | (value ? 0 : WS_DISABLED));
		}

		#endregion
	}
}
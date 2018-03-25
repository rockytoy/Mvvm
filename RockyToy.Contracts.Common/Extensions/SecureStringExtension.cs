using System;
using System.Runtime.InteropServices;
using System.Security;

namespace RockyToy.Contracts.Common.Extensions
{
	public static class SecureStringExtension
	{
		public static SecureString ToSecureString(this string str)
		{
			var ss = new SecureString();
			foreach (var c in str) ss.AppendChar(c);

			return ss;
		}

		// ref http://stackoverflow.com/questions/4502676/c-sharp-compare-two-securestrings-for-equality
		public static bool IsEqualTo(this SecureString ss1, SecureString ss2)
		{
			var bstr1 = IntPtr.Zero;
			var bstr2 = IntPtr.Zero;
			try
			{
				bstr1 = Marshal.SecureStringToBSTR(ss1);
				bstr2 = Marshal.SecureStringToBSTR(ss2);
				var length1 = Marshal.ReadInt32(bstr1, -4);
				var length2 = Marshal.ReadInt32(bstr2, -4);
				if (length1 == length2)
					for (var x = 0; x < length1; ++x)
					{
						var b1 = Marshal.ReadByte(bstr1, x);
						var b2 = Marshal.ReadByte(bstr2, x);
						if (b1 != b2) return false;
					}
				else return false;

				return true;
			}
			finally
			{
				if (bstr2 != IntPtr.Zero) Marshal.ZeroFreeBSTR(bstr2);
				if (bstr1 != IntPtr.Zero) Marshal.ZeroFreeBSTR(bstr1);
			}
		}

		// ref https://blogs.msdn.microsoft.com/fpintos/2009/06/12/how-to-properly-convert-securestring-to-string/
		public static string ToUnsecureString(this SecureString ss)
		{
			if (ss == null)
				return string.Empty;

			var unmanagedString = IntPtr.Zero;
			try
			{
				unmanagedString = Marshal.SecureStringToGlobalAllocUnicode(ss);
				return Marshal.PtrToStringUni(unmanagedString);
			}
			finally
			{
				Marshal.ZeroFreeGlobalAllocUnicode(unmanagedString);
			}
		}

		public static bool IsBase64(this string str)
		{
			try
			{
				Convert.FromBase64String(str);
				return true;
			}
			catch
			{
				return false;
			}
		}

	}
}
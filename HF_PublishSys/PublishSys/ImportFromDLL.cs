using System;
using System.Runtime.InteropServices;

namespace PublishSys
{
	public class ImportFromDLL
	{
		public struct COPYDATASTRUCT
		{
			public int dwData;

			public int cbData;

			[MarshalAs(UnmanagedType.LPStr)]
			public string lpData;
		}

		public const int WM_COPYDATA = 74;

		[DllImport("User32.dll")]
		public static extern int SendMessage(IntPtr hWnd, int Msg, IntPtr wParam, ref COPYDATASTRUCT pcd);

		[DllImport("User32.dll")]
		public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

		[DllImport("Kernel32.dll")]
		public static extern IntPtr GetConsoleWindow();
	}
}

using System;
using System.Runtime.InteropServices;
namespace SysFrame.Gnnt.Common.Library
{
	public struct CopyDataStruct
	{
		public IntPtr dwData;
		public int cbData;
		[MarshalAs(UnmanagedType.LPStr)]
		public string str;
	}
}

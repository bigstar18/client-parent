using System;
using System.Runtime.InteropServices;
namespace YrdceClient.Yrdce.Common.Library
{
	public struct CopyDataStruct
	{
		public IntPtr dwData;
		public int cbData;
		[MarshalAs(UnmanagedType.LPStr)]
		public string str;
	}
}

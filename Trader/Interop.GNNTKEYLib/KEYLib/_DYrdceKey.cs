using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
namespace GNNTKEYLib
{
	[Guid("6A9DAF25-D035-4CC5-8113-A673FD915D21"), InterfaceType(2), TypeLibType(4112)]
	[ComImport]
	public interface _DGnntKey
	{
		[DispId(1)]
		[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall)]
		[return: MarshalAs(UnmanagedType.BStr)]
		string VerifyUser(short market, [MarshalAs(UnmanagedType.BStr)] string user);
		[DispId(2)]
		[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall)]
		[return: MarshalAs(UnmanagedType.BStr)]
		string ReadFile([MarshalAs(UnmanagedType.BStr)] string fileName);
		[DispId(3)]
		[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall)]
		bool WriteFile([MarshalAs(UnmanagedType.BStr)] string fileName, [MarshalAs(UnmanagedType.BStr)] string fileContent, [MarshalAs(UnmanagedType.BStr)] string writeType);
	}
}

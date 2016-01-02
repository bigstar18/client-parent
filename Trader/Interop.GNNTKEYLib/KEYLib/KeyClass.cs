using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
namespace GNNTKEYLib
{
    [ComImport, Guid("0023145A-18C6-40C7-9C99-1DB6C3288C3A"), TypeLibType((short)0x22), ComSourceInterfaces("GNNTKEYLib._DGnntKeyEvents\0"), ClassInterface((short)0)]
	public class GnntKeyClass : _DGnntKey, GnntKey, _DGnntKeyEvents_Event
    {
        [return: MarshalAs(UnmanagedType.BStr)]
        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(2)]
        public virtual extern string ReadFile([MarshalAs(UnmanagedType.BStr)] string fileName);
        [return: MarshalAs(UnmanagedType.BStr)]
        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(1)]
        public virtual extern string VerifyUser(short market, [MarshalAs(UnmanagedType.BStr)] string user);
        [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(3)]
        public virtual extern bool WriteFile([MarshalAs(UnmanagedType.BStr)] string fileName, [MarshalAs(UnmanagedType.BStr)] string fileContent, [MarshalAs(UnmanagedType.BStr)] string writeType);
    }



}

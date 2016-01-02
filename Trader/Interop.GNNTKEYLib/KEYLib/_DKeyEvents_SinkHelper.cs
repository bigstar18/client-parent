using System;
using System.Runtime.InteropServices;
namespace GNNTKEYLib
{
	[ClassInterface(ClassInterfaceType.None), TypeLibType(TypeLibTypeFlags.FHidden)]
	public sealed class _DGnntKeyEvents_SinkHelper : _DGnntKeyEvents
	{
		public int m_dwCookie;
		internal _DGnntKeyEvents_SinkHelper()
		{
			this.m_dwCookie = 0;
		}
	}
}

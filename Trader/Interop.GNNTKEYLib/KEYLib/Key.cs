using System;
using System.Runtime.InteropServices;
namespace GNNTKEYLib
{
    [ComImport, Guid("6A9DAF25-D035-4CC5-8113-A673FD915D21"), CoClass(typeof(GnntKeyClass))]
	public interface GnntKey : _DGnntKey, _DGnntKeyEvents_Event
	{
	}
}

using GNNTKEYLib;
using System;
using System.Runtime.InteropServices;
namespace AxKEYLib
{
	[ClassInterface(ClassInterfaceType.None)]
	public class AxKeyEventMulticaster : _DGnntKeyEvents
	{
		private AxKey parent;
		public AxKeyEventMulticaster(AxKey parent)
		{
			this.parent = parent;
		}
	}
}

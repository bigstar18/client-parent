using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Threading;

namespace GNNTKEYLib
{
	internal sealed class _DGnntKeyEvents_EventProvider : _DGnntKeyEvents_Event
	{
        private ArrayList m_aEventSinkHelpers;
        private IConnectionPoint m_ConnectionPoint;
        private IConnectionPointContainer m_ConnectionPointContainer;

        public _DGnntKeyEvents_EventProvider(object obj1)
        {
            this.m_ConnectionPointContainer = (IConnectionPointContainer)obj1;
        }

      
        



        private void Init()
        {
            IConnectionPoint ppCP = null;
            byte[] b = new byte[] { 0x18, 0xf1, 0x55, 2, 0x9d, 0xbd, 0x4e, 0x48, 0xbc, 0x92, 120, 0xd0, 0x5b, 0xd5, 0x3e, 0x11 };
            Guid riid = new Guid(b);
            this.m_ConnectionPointContainer.FindConnectionPoint(ref riid, out ppCP);
            this.m_ConnectionPoint = ppCP;
            this.m_aEventSinkHelpers = new ArrayList();
        }














    }
}

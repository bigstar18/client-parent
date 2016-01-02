using System;
using System.Threading;
namespace TradeClientApp.Gnnt.OTC.Library
{
	internal class ThreadPoolParameter
	{
		public AutoResetEvent Semaphores;
		public object obj;
	}
}

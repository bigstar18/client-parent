using System;
namespace Gnnt.MEBS.HQModel.DataVO
{
	public class Packet_HttpRequest
	{
		public const byte TYPE_DAYLINE = 0;
		public const byte TYPE_5MINLINE = 1;
		public const byte TYPE_1MINLINE = 2;
		public const byte TYPE_F10 = 2;
		public string marketID;
		public string sCode;
		public byte type;
	}
}

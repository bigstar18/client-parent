using System;
namespace Gnnt.MEBS.HQClient.gnnt.util
{
	public enum drawingMode
	{
		R2_BLACK = 1,
		R2_NOTMERGEPEN,
		R2_MASKNOTPEN,
		R2_NOTCOPYPEN,
		R2_MASKPENNOT,
		R2_NOT,
		R2_XORPEN,
		R2_NOTMASKPEN,
		R2_MASKPEN,
		R2_NOTXORPEN,
		R2_NOP,
		R2_MERGENOTPEN,
		R2_COPYPEN,
		R2_MERGEPENNOT,
		R2_MERGEPEN,
		R2_WHITE,
		R2_LAST = 16
	}
}

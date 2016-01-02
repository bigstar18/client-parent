// Decompiled with JetBrains decompiler
// Type: Gnnt.MEBS.HQModel.RHColor
// Assembly: HQModel, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: FD1CB918-942E-47F7-BED2-EBD1E7FF35B7
// Assembly location: D:\test\长三角商品交易所(模拟)\Trader\Modules\MEBSModule\HQModel.dll

using System.Drawing;

namespace Gnnt.MEBS.HQModel
{
  public class RHColor
  {
    public Color[] clIndicator = new Color[6];
    public const int COLORSTYLE_CLASSICAL = 0;
    public const int COLORSTYLE_MODERN = 1;
    public const int COLORSTYLE_ELEGANCE = 2;
    public const int COLORSTYLE_SOFTNESS = 3;
    public const int COLORSTYLE_DIGNITY = 4;
    public const int COLORSTYLE_TRADITION = 5;
    public Color clBackGround;
    public Color clPriceChange;
    public Color clIncrease;
    public Color clDecrease;
    public Color clEqual;
    public Color clGrid;
    public Color clMinLine;
    public Color clCursor;
    public Color clProductName;
    public Color clVolume;
    public Color clReserve;
    public Color clNumber;
    public Color clHighlight;
    public Color clItem;
    public Color clMultiQuote_TitleBack;
    public Color clKLineUp;
    public Color clKLineDown;
    public Color clKLineEqual;
    public Color clPolyLine;
    public Color clUSALine;

    public RHColor(int iStyle)
    {
      switch (iStyle)
      {
        case 1:
          this.clBackGround = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
          this.clIncrease = Color.FromArgb(240, 0, 0);
          this.clDecrease = Color.FromArgb(0, 128, 0);
          this.clEqual = Color.FromArgb(68, 68, 68);
          this.clGrid = Color.FromArgb(160, 160, 160);
          this.clMinLine = Color.FromArgb(0, 0, 160);
          this.clCursor = Color.FromArgb((int) byte.MaxValue, 128, 0);
          this.clHighlight = Color.FromArgb(112, 219, (int) byte.MaxValue);
          this.clItem = Color.FromArgb(0, 0, 128);
          this.clMultiQuote_TitleBack = Color.FromArgb(192, 192, 192);
          this.clProductName = Color.FromArgb(0, 0, 128);
          this.clVolume = Color.FromArgb(0, 0, 192);
          this.clReserve = Color.FromArgb(64, 128, 128);
          this.clNumber = Color.FromArgb(0, 0, 128);
          this.clKLineUp = Color.FromArgb((int) byte.MaxValue, 0, 0);
          this.clKLineDown = Color.FromArgb(0, 128, 0);
          this.clKLineEqual = Color.FromArgb(128, 128, 128);
          this.clPolyLine = Color.FromArgb(0, (int) byte.MaxValue, (int) byte.MaxValue);
          this.clUSALine = Color.FromArgb(0, (int) byte.MaxValue, (int) byte.MaxValue);
          this.clIndicator[0] = Color.FromArgb(0, 0, 64);
          this.clIndicator[1] = Color.FromArgb((int) byte.MaxValue, 0, 128);
          this.clIndicator[2] = Color.FromArgb((int) byte.MaxValue, 128, 0);
          this.clIndicator[3] = Color.FromArgb(128, 0, 0);
          this.clIndicator[4] = Color.FromArgb((int) byte.MaxValue, 0, (int) byte.MaxValue);
          this.clIndicator[5] = Color.FromArgb(128, 128, 16);
          this.clPriceChange = this.clHighlight;
          break;
        case 2:
          this.clBackGround = Color.FromArgb(0, 0, 128);
          this.clIncrease = Color.FromArgb((int) byte.MaxValue, 0, 0);
          this.clDecrease = Color.FromArgb(0, (int) byte.MaxValue, (int) byte.MaxValue);
          this.clEqual = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
          this.clGrid = Color.FromArgb(128, 128, 128);
          this.clMinLine = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
          this.clCursor = Color.FromArgb(192, 192, 192);
          this.clHighlight = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, 128);
          this.clItem = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
          this.clMultiQuote_TitleBack = Color.FromArgb(0, 0, 176);
          this.clProductName = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, 0);
          this.clVolume = Color.FromArgb(224, 224, 0);
          this.clReserve = Color.FromArgb(64, 128, 128);
          this.clNumber = Color.FromArgb(192, 192, 192);
          this.clKLineUp = Color.FromArgb((int) byte.MaxValue, 0, 0);
          this.clKLineDown = Color.FromArgb(128, (int) byte.MaxValue, (int) byte.MaxValue);
          this.clKLineEqual = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
          this.clPolyLine = Color.FromArgb(0, (int) byte.MaxValue, (int) byte.MaxValue);
          this.clUSALine = Color.FromArgb(0, (int) byte.MaxValue, (int) byte.MaxValue);
          this.clIndicator[0] = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, 0);
          this.clIndicator[1] = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
          this.clIndicator[2] = Color.FromArgb(0, 0, (int) byte.MaxValue);
          this.clIndicator[3] = Color.FromArgb((int) byte.MaxValue, 128, 64);
          this.clIndicator[4] = Color.FromArgb((int) byte.MaxValue, 0, (int) byte.MaxValue);
          this.clIndicator[5] = Color.FromArgb(128, 128, 16);
          this.clPriceChange = this.clHighlight;
          break;
        case 3:
          this.clBackGround = Color.FromArgb(248, 248, 240);
          this.clIncrease = Color.FromArgb((int) byte.MaxValue, 0, 0);
          this.clDecrease = Color.FromArgb(0, 160, 0);
          this.clEqual = Color.FromArgb(0, 0, 0);
          this.clGrid = Color.FromArgb(192, 192, 192);
          this.clMinLine = Color.FromArgb(0, 0, 0);
          this.clCursor = Color.FromArgb(64, 64, 64);
          this.clItem = Color.FromArgb(0, 0, 0);
          this.clMultiQuote_TitleBack = Color.FromArgb(232, 232, 224);
          this.clProductName = Color.FromArgb(0, 0, (int) byte.MaxValue);
          this.clHighlight = Color.FromArgb(112, 219, (int) byte.MaxValue);
          this.clReserve = Color.FromArgb(64, 128, 128);
          this.clVolume = Color.FromArgb(96, 96, 0);
          this.clNumber = Color.FromArgb(64, 64, 64);
          this.clKLineUp = Color.FromArgb((int) byte.MaxValue, 0, 0);
          this.clKLineDown = Color.FromArgb(0, 0, (int) byte.MaxValue);
          this.clKLineEqual = Color.FromArgb(128, 128, 128);
          this.clPolyLine = Color.FromArgb(0, (int) byte.MaxValue, (int) byte.MaxValue);
          this.clUSALine = Color.FromArgb(0, (int) byte.MaxValue, (int) byte.MaxValue);
          this.clIndicator[0] = Color.FromArgb(64, 64, 64);
          this.clIndicator[1] = Color.FromArgb(192, 0, 64);
          this.clIndicator[2] = Color.FromArgb(32, 128, 32);
          this.clIndicator[3] = Color.FromArgb(128, 0, 0);
          this.clIndicator[4] = Color.FromArgb((int) byte.MaxValue, 0, (int) byte.MaxValue);
          this.clIndicator[5] = Color.FromArgb(128, 128, 16);
          this.clPriceChange = this.clHighlight;
          break;
        case 4:
          this.clBackGround = Color.FromArgb(245, 252, 253);
          this.clIncrease = Color.FromArgb((int) byte.MaxValue, 128, 128);
          this.clDecrease = Color.FromArgb(0, (int) byte.MaxValue, 0);
          this.clEqual = Color.FromArgb(160, 160, 160);
          this.clGrid = Color.FromArgb(128, 128, 128);
          this.clMinLine = Color.FromArgb(160, 160, 160);
          this.clCursor = Color.FromArgb(32, 32, 32);
          this.clHighlight = Color.FromArgb(128, 128, (int) byte.MaxValue);
          this.clItem = Color.FromArgb(0, 0, (int) byte.MaxValue);
          this.clMultiQuote_TitleBack = Color.FromArgb(160, 240, 160);
          this.clProductName = Color.FromArgb(64, 64, (int) byte.MaxValue);
          this.clVolume = Color.FromArgb(128, 128, 0);
          this.clReserve = Color.FromArgb(64, 128, 128);
          this.clNumber = Color.FromArgb(32, 32, 32);
          this.clKLineUp = Color.FromArgb((int) byte.MaxValue, 0, 0);
          this.clKLineDown = Color.FromArgb(0, (int) byte.MaxValue, 0);
          this.clKLineEqual = Color.FromArgb(64, 64, 64);
          this.clPolyLine = Color.FromArgb(0, (int) byte.MaxValue, (int) byte.MaxValue);
          this.clUSALine = Color.FromArgb(0, (int) byte.MaxValue, (int) byte.MaxValue);
          this.clIndicator[0] = Color.FromArgb(160, 160, 0);
          this.clIndicator[1] = Color.FromArgb(175, 175, 175);
          this.clIndicator[2] = Color.FromArgb(0, 0, (int) byte.MaxValue);
          this.clIndicator[3] = Color.FromArgb((int) byte.MaxValue, 128, 64);
          this.clIndicator[4] = Color.FromArgb((int) byte.MaxValue, 0, (int) byte.MaxValue);
          this.clIndicator[5] = Color.FromArgb(128, 128, 16);
          this.clPriceChange = this.clHighlight;
          break;
        case 5:
          this.clBackGround = Color.FromArgb(10, 10, 10);
          this.clIncrease = Color.FromArgb((int) byte.MaxValue, 60, 60);
          this.clDecrease = Color.FromArgb(0, 230, 0);
          this.clEqual = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
          this.clGrid = Color.FromArgb(192, 0, 0);
          this.clMinLine = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
          this.clCursor = Color.FromArgb(192, 192, 192);
          this.clItem = Color.FromArgb(0, (int) byte.MaxValue, (int) byte.MaxValue);
          this.clMultiQuote_TitleBack = Color.FromArgb(128, 128, 128);
          this.clProductName = Color.FromArgb(230, 230, 230);
          this.clHighlight = Color.FromArgb(23, 38, 62);
          this.clVolume = Color.FromArgb(200, 200, 0);
          this.clReserve = Color.FromArgb(0, (int) byte.MaxValue, (int) byte.MaxValue);
          this.clNumber = Color.FromArgb(230, 230, 230);
          this.clKLineUp = Color.FromArgb((int) byte.MaxValue, 0, 0);
          this.clKLineDown = Color.FromArgb(0, (int) byte.MaxValue, (int) byte.MaxValue);
          this.clKLineEqual = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
          this.clPolyLine = Color.FromArgb(0, (int) byte.MaxValue, (int) byte.MaxValue);
          this.clUSALine = Color.FromArgb(0, (int) byte.MaxValue, (int) byte.MaxValue);
          this.clIndicator[0] = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
          this.clIndicator[1] = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, 0);
          this.clIndicator[2] = Color.FromArgb((int) byte.MaxValue, 0, (int) byte.MaxValue);
          this.clIndicator[3] = Color.FromArgb(0, (int) byte.MaxValue, 0);
          this.clIndicator[4] = Color.FromArgb((int) byte.MaxValue, 128, 0);
          this.clIndicator[5] = Color.FromArgb(128, 128, 16);
          this.clPriceChange = Color.FromArgb(0, 0, (int) byte.MaxValue);
          break;
        default:
          this.clBackGround = Color.FromArgb(0, 0, 0);
          this.clIncrease = Color.FromArgb((int) byte.MaxValue, 96, 96);
          this.clDecrease = Color.FromArgb(0, (int) byte.MaxValue, 96);
          this.clEqual = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
          this.clGrid = Color.FromArgb(160, 0, 0);
          this.clMinLine = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
          this.clCursor = Color.FromArgb(192, 192, 192);
          this.clItem = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
          this.clMultiQuote_TitleBack = Color.FromArgb(40, 40, 40);
          this.clProductName = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, 64);
          this.clHighlight = Color.FromArgb(40, 49, 98);
          this.clVolume = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, 64);
          this.clReserve = Color.FromArgb(0, (int) byte.MaxValue, 0);
          this.clNumber = Color.FromArgb(192, 192, 192);
          this.clKLineUp = Color.FromArgb((int) byte.MaxValue, 96, 96);
          this.clKLineDown = Color.FromArgb(0, (int) byte.MaxValue, (int) byte.MaxValue);
          this.clKLineEqual = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
          this.clPolyLine = Color.FromArgb(0, (int) byte.MaxValue, (int) byte.MaxValue);
          this.clUSALine = Color.FromArgb(0, (int) byte.MaxValue, (int) byte.MaxValue);
          this.clIndicator[0] = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
          this.clIndicator[1] = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, 0);
          this.clIndicator[2] = Color.FromArgb((int) byte.MaxValue, 0, (int) byte.MaxValue);
          this.clIndicator[3] = Color.FromArgb(0, (int) byte.MaxValue, 0);
          this.clIndicator[4] = Color.FromArgb((int) byte.MaxValue, 128, 0);
          this.clIndicator[5] = Color.FromArgb(128, 128, 16);
          this.clPriceChange = this.clHighlight;
          break;
      }
    }
  }
}

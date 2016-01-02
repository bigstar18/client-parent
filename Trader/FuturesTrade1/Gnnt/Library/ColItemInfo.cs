namespace FuturesTrade.Gnnt.Library
{
    using System;

    public class ColItemInfo
    {
        public string format;
        public string name;
        public int sortID;
        public int width;

        public ColItemInfo(string name, int width, string format, int sortID)
        {
            this.name = name;
            this.width = width;
            this.format = format;
            this.sortID = sortID;
        }
    }
}

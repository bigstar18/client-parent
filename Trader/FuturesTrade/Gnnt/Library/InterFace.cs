namespace FuturesTrade.Gnnt.Library
{
    using System;
    using System.Reflection;
    using System.Runtime.CompilerServices;

    public class InterFace
    {
        public static bool TopLevel = true;

        public static string AssemblyVersion
        {
            get
            {
                return Assembly.GetExecutingAssembly().GetName().Version.ToString();
            }
        }

        public class CommodityInfoEventArgs : EventArgs
        {
            private string commodityCode;
            private string eventInfo;

            public CommodityInfoEventArgs(string commodityCode, string eventInfo)
            {
                this.commodityCode = commodityCode;
                this.eventInfo = eventInfo;
            }

            public string CommodityCode
            {
                get
                {
                    return this.commodityCode;
                }
            }

            public string EventInfo
            {
                get
                {
                    return this.eventInfo;
                }
            }
        }

        public delegate void CommodityInfoEventHander(object sender, InterFace.CommodityInfoEventArgs e);
    }
}

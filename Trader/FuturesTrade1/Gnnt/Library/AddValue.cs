﻿namespace FuturesTrade.Gnnt.Library
{
    using System;

    public class AddValue
    {
        private string m_Display;
        private string m_Value;

        public AddValue(string Display, string Value)
        {
            this.m_Display = Display;
            this.m_Value = Value;
        }

        public string Display
        {
            get
            {
                return this.m_Display;
            }
        }

        public string Value
        {
            get
            {
                return this.m_Value;
            }
        }
    }
}

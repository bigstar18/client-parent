using System;
namespace Gnnt.MEBS.HQClient.gnnt.util
{
	internal class AddValue
	{
		private string m_Display;
		private string m_Value;
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
		public AddValue(string Display, string Value)
		{
			this.m_Display = Display;
			this.m_Value = Value;
		}
	}
}

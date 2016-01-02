using System;
namespace TradeInterface.Gnnt.OTC.DataVO
{
	public class CBListItem
	{
		private string _key = string.Empty;
		private string _value = string.Empty;
		public string Key
		{
			get
			{
				return this._key;
			}
			set
			{
				this._key = value;
			}
		}
		public string Value
		{
			get
			{
				return this._value;
			}
			set
			{
				this._value = value;
			}
		}
		public CBListItem(string pKey, string pValue)
		{
			this._key = pKey;
			this._value = pValue;
		}
		public override string ToString()
		{
			return this._value;
		}
	}
}

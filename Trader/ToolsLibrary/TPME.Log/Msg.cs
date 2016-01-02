using System;
namespace TPME.Log
{
	public class Msg
	{
		private DateTime datetime;
		private string text;
		private MsgType type;
		private string otherinfo;
		public string Info
		{
			get
			{
				return this.otherinfo;
			}
			set
			{
				this.otherinfo = value;
			}
		}
		public DateTime Datetime
		{
			get
			{
				return this.datetime;
			}
			set
			{
				this.datetime = value;
			}
		}
		public string Text
		{
			get
			{
				return this.text;
			}
			set
			{
				this.text = value;
			}
		}
		public MsgType Type
		{
			get
			{
				return this.type;
			}
			set
			{
				this.type = value;
			}
		}
		public Msg() : this("", MsgType.Unknown)
		{
		}
		public Msg(string t, MsgType p) : this(DateTime.Now, t, p)
		{
		}
		public Msg(DateTime dt, string t, MsgType p)
		{
			this.datetime = dt;
			this.type = p;
			this.text = t;
		}
		public string MsgTypetostring()
		{
			if (this.Type == MsgType.Error)
			{
				return "错误";
			}
			if (this.Type == MsgType.Information)
			{
				return "信息";
			}
			if (this.Type == MsgType.Warning)
			{
				return "警告";
			}
			if (this.Type == MsgType.Success)
			{
				return "成功";
			}
			return "未知";
		}
		public string tostring()
		{
			if (this.Type == MsgType.Error || this.Type == MsgType.Warning)
			{
				return string.Format("\r\n({0}) [{1}]:{2}  {3}", new object[]
				{
					this.datetime.ToString(),
					this.MsgTypetostring(),
					this.text,
					this.otherinfo
				});
			}
			return string.Empty;
		}
	}
}

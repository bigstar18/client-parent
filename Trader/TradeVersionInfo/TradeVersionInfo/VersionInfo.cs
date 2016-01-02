using System;
namespace TradeVersionInfo
{
	public class VersionInfo
	{
		private string versiontype;
		private string majorversion;
		private string minorversion;
		private string microversion;
		private string fullversion;
		public string VersionType
		{
			get
			{
				return this.versiontype;
			}
			set
			{
				this.versiontype = value;
			}
		}
		public string MajorVersion
		{
			get
			{
				return this.majorversion;
			}
			set
			{
				this.majorversion = value;
			}
		}
		public string MinorVersion
		{
			get
			{
				return this.minorversion;
			}
			set
			{
				this.minorversion = value;
			}
		}
		public string MicroVersion
		{
			get
			{
				return this.microversion;
			}
			set
			{
				this.microversion = value;
			}
		}
		public string FullVersion
		{
			get
			{
				return this.fullversion;
			}
			set
			{
				this.fullversion = value;
			}
		}
	}
}

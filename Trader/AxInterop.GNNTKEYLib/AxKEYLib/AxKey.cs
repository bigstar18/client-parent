using GNNTKEYLib;
using System;
using System.ComponentModel;
using System.Windows.Forms;
namespace AxKEYLib
{
	[DesignTimeVisible(true), AxHost.ClsidAttribute("{0023145a-18c6-40c7-9c99-1db6c3288c3a}")]
	public class AxKey : AxHost
	{
		private _DGnntKey ocx;
		private AxKeyEventMulticaster eventMulticaster;
		private AxHost.ConnectionPointCookie cookie;
		public AxKey() : base("0023145a-18c6-40c7-9c99-1db6c3288c3a")
		{
		}
		public virtual string VerifyUser(short market, string user)
		{
			if (this.ocx == null)
			{
				throw new AxHost.InvalidActiveXStateException("VerifyUser", AxHost.ActiveXInvokeKind.MethodInvoke);
			}
			return this.ocx.VerifyUser(market, user);
		}
		public virtual string ReadFile(string fileName)
		{
			if (this.ocx == null)
			{
				throw new AxHost.InvalidActiveXStateException("ReadFile", AxHost.ActiveXInvokeKind.MethodInvoke);
			}
			return this.ocx.ReadFile(fileName);
		}
		public virtual bool WriteFile(string fileName, string fileContent, string writeType)
		{
			if (this.ocx == null)
			{
				throw new AxHost.InvalidActiveXStateException("WriteFile", AxHost.ActiveXInvokeKind.MethodInvoke);
			}
			return this.ocx.WriteFile(fileName, fileContent, writeType);
		}
		protected override void CreateSink()
		{
			try
			{
				this.eventMulticaster = new AxKeyEventMulticaster(this);
				this.cookie = new AxHost.ConnectionPointCookie(this.ocx, this.eventMulticaster, typeof(_DGnntKeyEvents));
			}
			catch (Exception)
			{
			}
		}
		protected override void DetachSink()
		{
			try
			{
				this.cookie.Disconnect();
			}
			catch (Exception)
			{
			}
		}
		protected override void AttachInterfaces()
		{
			try
			{
				this.ocx = (_DGnntKey)base.GetOcx();
			}
			catch (Exception)
			{
			}
		}
	}
}

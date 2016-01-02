using Gnnt.MixAccountPlugin;
using YrdceClient.Yrdce.Common.Library;
using YrdceClient.Yrdce.Common.Operation.MainOperation;
using System;
using System.Collections.Generic;
using System.Drawing;
using ToolsLibrary.util;
using ToolStripRender;
namespace YrdceClient.Yrdce.Common.Operation.Manager
{
	public class OperationManager
	{
		public List<string> PluginNameList = new List<string>();
		public bool isLogin;
		public int FormStyle;
		public MixAccountPlugin mixAccountPlugin;
		private static volatile OperationManager operationManager;
		public ChangePasswordOperation changePassWordOperation;
		public DisplayFormsOperation displayFormsOperation;
		public LoginOperation loginOperation;
		public MixAccountOperation bindAccountOperation;
		public SpeedTestOperation speedTestOperation;
		public YrdceClientOperation YrdceClientOperation;
		public UpdateOperation updateOperation;
		public VerifyCodeOperation verifyCodeOparation;
		public StripButtonOperation stripButtonOperation;
		public static OperationManager GetInstance()
		{
			if (OperationManager.operationManager == null)
			{
				lock (typeof(OperationManager))
				{
					if (OperationManager.operationManager == null)
					{
						try
						{
							OperationManager.operationManager = new OperationManager();
						}
						catch (Exception ex)
						{
							throw ex;
						}
					}
				}
			}
			return OperationManager.operationManager;
		}
		public OperationManager()
		{
			this.changePassWordOperation = new ChangePasswordOperation();
			this.displayFormsOperation = new DisplayFormsOperation();
			this.loginOperation = new LoginOperation();
			this.speedTestOperation = new SpeedTestOperation();
			this.YrdceClientOperation = new YrdceClientOperation();
			this.updateOperation = new UpdateOperation();
			this.verifyCodeOparation = new VerifyCodeOperation();
			this.stripButtonOperation = new StripButtonOperation();
			this.bindAccountOperation = new MixAccountOperation();
		}
		public ToolStripColorTable CreateToolStrip()
		{
			ToolStripColorTable toolStripColorTable = new ToolStripColorTable();
			int num = Tools.StrToInt((string)Global.htConfig["ColorR"], 300);
			int num2 = Tools.StrToInt((string)Global.htConfig["ColorG"], 300);
			int num3 = Tools.StrToInt((string)Global.htConfig["ColorB"], 300);
			if (num3 < 256 && num < 256 && num2 < 256)
			{
				toolStripColorTable.Base = Color.FromArgb((int)((byte)num), (int)((byte)num2), (int)((byte)num3));
				toolStripColorTable.Fore = Color.White;
				toolStripColorTable.Border = Color.FromArgb((int)((byte)(num - 30)), (int)((byte)(num2 - 30)), (int)((byte)(num3 - 30)));
				toolStripColorTable.BackHover = Color.FromArgb((int)((byte)(num - 30)), (int)((byte)(num2 - 30)), (int)((byte)(num3 - 30)));
				toolStripColorTable.BackPressed = Color.FromArgb((int)((byte)(num - 30)), (int)((byte)(num2 - 30)), (int)((byte)(num3 - 30)));
			}
			return toolStripColorTable;
		}
	}
}

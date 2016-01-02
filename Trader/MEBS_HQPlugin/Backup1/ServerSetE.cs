// Decompiled with JetBrains decompiler
// Type: Gnnt.MEBS.HQClient.ServerSetE
// Assembly: HQClient, Version=4.0.2.0, Culture=neutral, PublicKeyToken=null
// MVID: 32B1E8FA-F247-4052-AB8C-B47D49C50CF5
// Assembly location: D:\test\长三角商品交易所(模拟)\Trader\Modules\MEBSModule\HQClient.dll

using DIYForm;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using System.Xml;

namespace Gnnt.MEBS.HQClient
{
  public class ServerSetE : MyForm
  {
    private IContainer components;
    private GroupBox groupBoxServer;
    private Button btConfirm;
    private Button btCancel;
    private Button btApply;
    private RadioButton radioNet;
    private RadioButton radioTel;
    private XmlElement xnConfigInfo;
    private XmlDocument xmlDoc;
    private Program.SET SET;
    private bool isUpdate;
    private int curServer;

    public ServerSetE()
    {
      base.\u002Ector();
      this.InitializeComponent();
    }

    protected virtual void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.groupBoxServer = new GroupBox();
      this.radioNet = new RadioButton();
      this.radioTel = new RadioButton();
      this.btConfirm = new Button();
      this.btCancel = new Button();
      this.btApply = new Button();
      this.groupBoxServer.SuspendLayout();
      ((Control) this).SuspendLayout();
      this.groupBoxServer.BackColor = Color.Transparent;
      this.groupBoxServer.Controls.Add((Control) this.radioNet);
      this.groupBoxServer.Controls.Add((Control) this.radioTel);
      this.groupBoxServer.FlatStyle = FlatStyle.Popup;
      this.groupBoxServer.Location = new Point(19, 37);
      this.groupBoxServer.Name = "groupBoxServer";
      this.groupBoxServer.Size = new Size(301, 116);
      this.groupBoxServer.TabIndex = 0;
      this.groupBoxServer.TabStop = false;
      this.groupBoxServer.Text = "服务器选择";
      this.radioNet.AutoSize = true;
      this.radioNet.Location = new Point(86, 88);
      this.radioNet.Name = "radioNet";
      this.radioNet.Size = new Size(83, 16);
      this.radioNet.TabIndex = 1;
      this.radioNet.TabStop = true;
      this.radioNet.Text = "联通服务器";
      this.radioNet.UseVisualStyleBackColor = true;
      this.radioNet.CheckedChanged += new EventHandler(this.radioTel_CheckedChanged);
      this.radioTel.AutoSize = true;
      this.radioTel.Location = new Point(86, 48);
      this.radioTel.Name = "radioTel";
      this.radioTel.Size = new Size(83, 16);
      this.radioTel.TabIndex = 0;
      this.radioTel.TabStop = true;
      this.radioTel.Text = "电信服务器";
      this.radioTel.UseVisualStyleBackColor = true;
      this.radioTel.CheckedChanged += new EventHandler(this.radioTel_CheckedChanged);
      this.btConfirm.Location = new Point(27, 173);
      this.btConfirm.Name = "btConfirm";
      this.btConfirm.Size = new Size(75, 23);
      this.btConfirm.TabIndex = 1;
      this.btConfirm.Text = "确定";
      this.btConfirm.UseVisualStyleBackColor = true;
      this.btConfirm.Click += new EventHandler(this.btConfirm_Click);
      this.btCancel.Location = new Point(120, 173);
      this.btCancel.Name = "btCancel";
      this.btCancel.Size = new Size(75, 23);
      this.btCancel.TabIndex = 2;
      this.btCancel.Text = "取消";
      this.btCancel.UseVisualStyleBackColor = true;
      this.btCancel.Click += new EventHandler(this.btCancel_Click);
      this.btApply.Location = new Point(211, 173);
      this.btApply.Name = "btApply";
      this.btApply.Size = new Size(75, 23);
      this.btApply.TabIndex = 3;
      this.btApply.Text = "应用";
      this.btApply.UseVisualStyleBackColor = true;
      this.btApply.Click += new EventHandler(this.btApply_Click);
      ((ContainerControl) this).AutoScaleDimensions = new SizeF(6f, 12f);
      ((ContainerControl) this).AutoScaleMode = AutoScaleMode.Font;
      ((Form) this).ClientSize = new Size(347, 208);
      ((Control) this).Controls.Add((Control) this.btApply);
      ((Control) this).Controls.Add((Control) this.btCancel);
      ((Control) this).Controls.Add((Control) this.btConfirm);
      ((Control) this).Controls.Add((Control) this.groupBoxServer);
      ((Form) this).MaximizeBox = false;
      ((Form) this).MinimizeBox = false;
      ((Control) this).Name = "ServerSetE";
      ((Form) this).StartPosition = FormStartPosition.CenterScreen;
      ((Control) this).Text = "网络设置";
      ((Form) this).FormClosed += new FormClosedEventHandler(this.ServerSetE_FormClosed);
      ((Form) this).Load += new EventHandler(this.ServerSetE_Load);
      ((Control) this).Controls.SetChildIndex((Control) this.groupBoxServer, 0);
      ((Control) this).Controls.SetChildIndex((Control) this.btConfirm, 0);
      ((Control) this).Controls.SetChildIndex((Control) this.btCancel, 0);
      ((Control) this).Controls.SetChildIndex((Control) this.btApply, 0);
      this.groupBoxServer.ResumeLayout(false);
      this.groupBoxServer.PerformLayout();
      ((Control) this).ResumeLayout(false);
      ((Control) this).PerformLayout();
    }

    private void ServerSetE_Load(object sender, EventArgs e)
    {
      this.xmlDoc = new XmlDocument();
      this.xmlDoc.Load(this.SET.myConfigFileName);
      this.xnConfigInfo = (XmlElement) this.xmlDoc.SelectSingleNode("ConfigInfo");
      this.LoadNetUserInfo();
      this.btApply.Enabled = false;
      this.isUpdate = false;
    }

    private void LoadNetUserInfo()
    {
      if (this.xnConfigInfo == null)
        return;
      try
      {
        this.curServer = int.Parse(this.xnConfigInfo.SelectSingleNode("ServerType").InnerText, NumberStyles.None);
      }
      catch
      {
      }
      if (this.curServer == 0)
        this.radioTel.Checked = true;
      else
        this.radioNet.Checked = true;
    }

    private void radioTel_CheckedChanged(object sender, EventArgs e)
    {
      this.btApply.Enabled = true;
      this.isUpdate = true;
    }

    private void btCancel_Click(object sender, EventArgs e)
    {
      ((Form) this).DialogResult = DialogResult.Cancel;
      ((Form) this).Close();
    }

    private void ServerSetE_FormClosed(object sender, FormClosedEventArgs e)
    {
      // ISSUE: explicit non-virtual call
      __nonvirtual (((Component) this).Dispose());
    }

    private void btApply_Click(object sender, EventArgs e)
    {
      try
      {
        if (this.isUpdate)
          this.updateNetUserInfo();
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(ex.Message);
      }
      this.btApply.Enabled = false;
    }

    private void btConfirm_Click(object sender, EventArgs e)
    {
      if (this.btApply.Enabled)
      {
        try
        {
          this.btApply_Click((object) null, (EventArgs) null);
          ((Form) this).Close();
        }
        catch (Exception ex)
        {
          int num = (int) MessageBox.Show(ex.Message);
        }
      }
      else
        ((Form) this).Close();
      if (!this.isUpdate)
        return;
      ((Form) this).DialogResult = DialogResult.OK;
    }

    private void updateNetUserInfo()
    {
      this.curServer = !this.radioNet.Checked ? 0 : 1;
      this.xnConfigInfo.SelectSingleNode("ServerType").InnerText = this.curServer.ToString();
      this.xmlDoc.Save(this.SET.myConfigFileName);
    }
  }
}

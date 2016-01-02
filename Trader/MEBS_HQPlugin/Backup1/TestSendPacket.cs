// Decompiled with JetBrains decompiler
// Type: Gnnt.MEBS.HQClient.TestSendPacket
// Assembly: HQClient, Version=4.0.2.0, Culture=neutral, PublicKeyToken=null
// MVID: 32B1E8FA-F247-4052-AB8C-B47D49C50CF5
// Assembly location: D:\test\长三角商品交易所(模拟)\Trader\Modules\MEBSModule\HQClient.dll

using Gnnt.MEBS.HQClient.gnnt.ClientForms;
using Gnnt.MEBS.HQClient.gnnt.MEBS.HQClient;
using Gnnt.MEBS.HQModel.DataVO;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Gnnt.MEBS.HQClient
{
  public class TestSendPacket : Form
  {
    private HQClientMain hqClient;
    private IContainer components;
    private Button button1;
    private Button button2;
    private Button button3;
    private Button button4;
    private Button button5;
    private Button button6;
    private Button button7;
    private Button button8;
    private Button button9;
    private Button button10;
    private Button button11;

    public TestSendPacket()
    {
      this.InitializeComponent();
    }

    private void TestSendPacket_Load(object sender, EventArgs e)
    {
      this.hqClient = new HQClientMain((HQForm) null);
      this.hqClient.init();
    }

    private void button1_Click(object sender, EventArgs e)
    {
      this.hqClient.sendThread.AskForData((CMDVO) new CMDProductInfoVO()
      {
        date = this.hqClient.m_iCodeDate,
        time = this.hqClient.m_iCodeTime
      });
    }

    private void button2_Click(object sender, EventArgs e)
    {
      byte num1 = (byte) 0;
      int num2 = 1;
      int num3 = 30;
      byte num4 = (byte) 0;
      CMDSortVO cmdSortVo = new CMDSortVO();
      cmdSortVo.isDescend = num1;
      cmdSortVo.sortBy = num4;
      if (num2 == num3)
        num3 = num2 + 1;
      cmdSortVo.start = num2;
      cmdSortVo.end = num3;
      this.hqClient.sendThread.AskForData((CMDVO) cmdSortVo);
    }

    private void button4_Click(object sender, EventArgs e)
    {
      this.hqClient.sendThread.AskForData((CMDVO) new CMDQuoteListVO());
    }

    private void button3_Click(object sender, EventArgs e)
    {
      CMDQuoteVO cmdQuoteVo = new CMDQuoteVO();
      cmdQuoteVo.codeList = new string[1, 2];
      cmdQuoteVo.codeList[0, 0] = "B80625";
      cmdQuoteVo.isAll = (byte) 1;
      cmdQuoteVo.codeList[0, 1] = "0";
      this.hqClient.sendThread.AskForData((CMDVO) cmdQuoteVo);
    }

    private void button5_Click(object sender, EventArgs e)
    {
      this.hqClient.sendThread.AskForData((CMDVO) new CMDMarketSortVO()
      {
        num = 10
      });
    }

    private void button6_Click(object sender, EventArgs e)
    {
    }

    private void button7_Click(object sender, EventArgs e)
    {
    }

    private void button8_Click(object sender, EventArgs e)
    {
    }

    private void button9_Click(object sender, EventArgs e)
    {
    }

    private void button10_Click(object sender, EventArgs e)
    {
    }

    private void button11_Click(object sender, EventArgs e)
    {
    }

    private void TestSendPacket_FormClosing(object sender, FormClosingEventArgs e)
    {
      this.hqClient.Dispose();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.button1 = new Button();
      this.button2 = new Button();
      this.button3 = new Button();
      this.button4 = new Button();
      this.button5 = new Button();
      this.button6 = new Button();
      this.button7 = new Button();
      this.button8 = new Button();
      this.button9 = new Button();
      this.button10 = new Button();
      this.button11 = new Button();
      this.SuspendLayout();
      this.button1.Location = new Point(74, 12);
      this.button1.Name = "button1";
      this.button1.Size = new Size(357, 50);
      this.button1.TabIndex = 0;
      this.button1.Text = "商品码表查询";
      this.button1.UseVisualStyleBackColor = true;
      this.button1.Click += new EventHandler(this.button1_Click);
      this.button2.Location = new Point(74, 68);
      this.button2.Name = "button2";
      this.button2.Size = new Size(357, 50);
      this.button2.TabIndex = 0;
      this.button2.Text = "报价排名查询";
      this.button2.UseVisualStyleBackColor = true;
      this.button2.Click += new EventHandler(this.button2_Click);
      this.button3.Location = new Point(74, 180);
      this.button3.Name = "button3";
      this.button3.Size = new Size(357, 50);
      this.button3.TabIndex = 0;
      this.button3.Text = "个股行情查询";
      this.button3.UseVisualStyleBackColor = true;
      this.button3.Click += new EventHandler(this.button3_Click);
      this.button4.Location = new Point(74, 124);
      this.button4.Name = "button4";
      this.button4.Size = new Size(357, 50);
      this.button4.TabIndex = 0;
      this.button4.Text = "实时行情查询";
      this.button4.UseVisualStyleBackColor = true;
      this.button4.Click += new EventHandler(this.button4_Click);
      this.button5.Location = new Point(74, 236);
      this.button5.Name = "button5";
      this.button5.Size = new Size(357, 50);
      this.button5.TabIndex = 0;
      this.button5.Text = "综合排名数据查询";
      this.button5.UseVisualStyleBackColor = true;
      this.button5.Click += new EventHandler(this.button5_Click);
      this.button6.Location = new Point(74, 292);
      this.button6.Name = "button6";
      this.button6.Size = new Size(357, 50);
      this.button6.TabIndex = 0;
      this.button6.Text = "分时数据查询";
      this.button6.UseVisualStyleBackColor = true;
      this.button6.Click += new EventHandler(this.button6_Click);
      this.button7.Location = new Point(74, 348);
      this.button7.Name = "button7";
      this.button7.Size = new Size(357, 50);
      this.button7.TabIndex = 0;
      this.button7.Text = "指定时间之后的分笔数据";
      this.button7.UseVisualStyleBackColor = true;
      this.button7.Click += new EventHandler(this.button7_Click);
      this.button8.Location = new Point(74, 404);
      this.button8.Name = "button8";
      this.button8.Size = new Size(184, 50);
      this.button8.TabIndex = 1;
      this.button8.Text = "所有分笔数据查询";
      this.button8.UseVisualStyleBackColor = true;
      this.button8.Click += new EventHandler(this.button8_Click);
      this.button9.Location = new Point(264, 407);
      this.button9.Name = "button9";
      this.button9.Size = new Size(167, 45);
      this.button9.TabIndex = 2;
      this.button9.Text = "最后数笔分笔";
      this.button9.UseVisualStyleBackColor = true;
      this.button9.Click += new EventHandler(this.button9_Click);
      this.button10.Location = new Point(484, 112);
      this.button10.Name = "button10";
      this.button10.Size = new Size(170, 35);
      this.button10.TabIndex = 3;
      this.button10.Text = "HTTP取日线数据";
      this.button10.UseVisualStyleBackColor = true;
      this.button10.Click += new EventHandler(this.button10_Click);
      this.button11.Location = new Point(484, 153);
      this.button11.Name = "button11";
      this.button11.Size = new Size(170, 37);
      this.button11.TabIndex = 4;
      this.button11.Text = "HTTP取5分钟线数据";
      this.button11.UseVisualStyleBackColor = true;
      this.button11.Click += new EventHandler(this.button11_Click);
      this.AutoScaleDimensions = new SizeF(6f, 12f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(663, 460);
      this.Controls.Add((Control) this.button11);
      this.Controls.Add((Control) this.button10);
      this.Controls.Add((Control) this.button9);
      this.Controls.Add((Control) this.button8);
      this.Controls.Add((Control) this.button7);
      this.Controls.Add((Control) this.button6);
      this.Controls.Add((Control) this.button5);
      this.Controls.Add((Control) this.button4);
      this.Controls.Add((Control) this.button3);
      this.Controls.Add((Control) this.button2);
      this.Controls.Add((Control) this.button1);
      this.Name = "TestSendPacket";
      this.Text = "通讯协议测试";
      this.Load += new EventHandler(this.TestSendPacket_Load);
      this.FormClosing += new FormClosingEventHandler(this.TestSendPacket_FormClosing);
      this.ResumeLayout(false);
    }
  }
}

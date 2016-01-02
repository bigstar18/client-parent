// Decompiled with JetBrains decompiler
// Type: Gnnt.MEBS.HQClient.InputWindow
// Assembly: HQClient, Version=4.0.2.0, Culture=neutral, PublicKeyToken=null
// MVID: 32B1E8FA-F247-4052-AB8C-B47D49C50CF5
// Assembly location: D:\test\长三角商品交易所(模拟)\Trader\Modules\MEBSModule\HQClient.dll

using DIYForm;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Gnnt.MEBS.HQClient
{
  public class InputWindow : MyForm
  {
    private IContainer components;
    private Label InputLabel;
    private TextBox InputBox;
    private Button ConfirmBtn;
    private Button CancelBtn;
    private int _KValue;
    private int _KType;

    public int KValue
    {
      get
      {
        if (this.InputBox.Text != "")
          return Convert.ToInt32(this.InputBox.Text);
        return 0;
      }
      set
      {
        this._KValue = value;
      }
    }

    public int KType
    {
      get
      {
        return this._KType;
      }
      set
      {
        this._KType = value;
      }
    }

    public InputWindow(int type)
    {
      this.\u002Ector();
      this.InitializeComponent();
      this.KType = type;
      if (this.KType == 1)
      {
        ((Control) this).Text = "任意天技术分析";
        this.InputLabel.Text = "请输入天数：";
      }
      else
      {
        ((Control) this).Text = "任意分钟技术分析";
        this.InputLabel.Text = "请输入分钟数：";
      }
      this.InputBox.Focus();
    }

    protected virtual void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.InputLabel = new Label();
      this.InputBox = new TextBox();
      this.ConfirmBtn = new Button();
      this.CancelBtn = new Button();
      ((Control) this).SuspendLayout();
      this.InputLabel.AutoSize = true;
      this.InputLabel.BackColor = Color.Transparent;
      this.InputLabel.Location = new Point(39, 36);
      this.InputLabel.Name = "InputLabel";
      this.InputLabel.Size = new Size(71, 12);
      this.InputLabel.TabIndex = 6;
      this.InputLabel.Text = "请输入天数:";
      this.InputBox.Location = new Point(39, 51);
      this.InputBox.Name = "InputBox";
      this.InputBox.Size = new Size(170, 21);
      this.InputBox.TabIndex = 7;
      this.InputBox.KeyPress += new KeyPressEventHandler(this.InputBox_KeyPress);
      this.ConfirmBtn.BackColor = Color.White;
      this.ConfirmBtn.FlatStyle = FlatStyle.Popup;
      this.ConfirmBtn.Location = new Point(50, 75);
      this.ConfirmBtn.Name = "ConfirmBtn";
      this.ConfirmBtn.Size = new Size(47, 23);
      this.ConfirmBtn.TabIndex = 8;
      this.ConfirmBtn.Text = "确定";
      this.ConfirmBtn.UseVisualStyleBackColor = false;
      this.ConfirmBtn.Click += new EventHandler(this.ConfirmBtn_Click);
      this.CancelBtn.BackColor = Color.White;
      this.CancelBtn.DialogResult = DialogResult.Cancel;
      this.CancelBtn.FlatStyle = FlatStyle.Popup;
      this.CancelBtn.Location = new Point(145, 75);
      this.CancelBtn.Name = "CancelBtn";
      this.CancelBtn.Size = new Size(47, 23);
      this.CancelBtn.TabIndex = 9;
      this.CancelBtn.Text = "取消";
      this.CancelBtn.UseVisualStyleBackColor = false;
      this.CancelBtn.Click += new EventHandler(this.CancelBtn_Click);
      ((Form) this).AcceptButton = (IButtonControl) this.ConfirmBtn;
      ((ContainerControl) this).AutoScaleDimensions = new SizeF(6f, 12f);
      ((ContainerControl) this).AutoScaleMode = AutoScaleMode.Font;
      ((Form) this).CancelButton = (IButtonControl) this.CancelBtn;
      ((Form) this).ClientSize = new Size(247, 110);
      ((Control) this).Controls.Add((Control) this.CancelBtn);
      ((Control) this).Controls.Add((Control) this.ConfirmBtn);
      ((Control) this).Controls.Add((Control) this.InputBox);
      ((Control) this).Controls.Add((Control) this.InputLabel);
      this.set_DIYMaximizeBox(false);
      this.set_DIYMinimizeBox(false);
      ((Control) this).Name = "InputWindow";
      ((Form) this).StartPosition = FormStartPosition.CenterParent;
      ((Control) this).Text = "InputWindow";
      ((Control) this).Controls.SetChildIndex((Control) this.InputLabel, 0);
      ((Control) this).Controls.SetChildIndex((Control) this.InputBox, 0);
      ((Control) this).Controls.SetChildIndex((Control) this.ConfirmBtn, 0);
      ((Control) this).Controls.SetChildIndex((Control) this.CancelBtn, 0);
      ((Control) this).ResumeLayout(false);
      ((Control) this).PerformLayout();
    }

    private void ConfirmBtn_Click(object sender, EventArgs e)
    {
      ((Form) this).DialogResult = DialogResult.Yes;
      ((Form) this).Close();
    }

    private void CancelBtn_Click(object sender, EventArgs e)
    {
      ((Form) this).DialogResult = DialogResult.No;
      ((Form) this).Close();
    }

    public static bool isNumberic(string _string)
    {
      if (string.IsNullOrEmpty(_string))
        return false;
      foreach (char c in _string)
      {
        if (!char.IsDigit(c))
          return false;
      }
      return true;
    }

    private void Window_KeyUp(object sender, KeyEventArgs e)
    {
      if (e.KeyData == Keys.Escape)
      {
        ((Form) this).DialogResult = DialogResult.No;
        ((Form) this).Close();
      }
      if (e.KeyData != Keys.Return)
        return;
      ((Form) this).DialogResult = DialogResult.Yes;
      ((Form) this).Close();
    }

    private void InputBox_KeyPress(object sender, KeyPressEventArgs e)
    {
      e.Handled = (int) e.KeyChar < 48 || (int) e.KeyChar > 57;
      if ((int) e.KeyChar == 8)
      {
        e.Handled = false;
      }
      else
      {
        if (this.InputBox.Text.Length <= 5)
          return;
        e.Handled = true;
      }
    }
  }
}

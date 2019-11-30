// Decompiled with JetBrains decompiler
// Type: Kiosk.DLG_Monitors
// Assembly: Kiosk, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: C3E32FFD-072D-4F9D-AAE4-A7F2B29E989A
// Assembly location: E:\kiosk\Kiosk.exe

using Kiosk.Properties;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace Kiosk
{
  public class DLG_Monitors : Form
  {
    private IContainer components = (IContainer) null;
    public bool OK;
    public Configuracion opciones;
    public MainWindow MWin;
    private SplashMonitor Mon1;
    private SplashMonitor Mon2;
    private Button bCancel;
    private Button bOk;
    private Label lPubli;
    private TextBox ePubli;
    private Button bCheck;
    private Button bClone;
    private Button bMon12;
    private Button bMon21;
    private CheckBox cMon;
    private Label lMon1;
    private Label lMon2;

    public DLG_Monitors(ref Configuracion _opc)
    {
      this.OK = false;
      this.opciones = _opc;
      this.MWin = (MainWindow) null;
      this.InitializeComponent();
      this.cMon.Checked = this.opciones.Monitors > 1;
      this.ePubli.Text = this.opciones.Publi;
      this.Localize();
    }

    private void Localize()
    {
      this.SuspendLayout();
      this.ResumeLayout();
    }

    public void Update_Info()
    {
      Screen[] allScreens = Screen.AllScreens;
      this.lMon1.Text = "Monitor 1: " + Screen.PrimaryScreen.Bounds.ToString();
      if (allScreens.Length > 1)
      {
        if (allScreens[1].Bounds.X == Screen.PrimaryScreen.Bounds.X)
          this.lMon2.Text = "Monitor 2: " + allScreens[0].Bounds.ToString();
        else
          this.lMon2.Text = "Monitor 2: " + allScreens[1].Bounds.ToString();
      }
      else
        this.lMon2.Text = "Monitor 2: -";
    }

    public void ID_Monitors_Clear()
    {
      if (this.Mon1 != null)
      {
        this.Mon1.Hide();
        this.Mon1 = (SplashMonitor) null;
      }
      if (this.Mon2 == null)
        return;
      this.Mon2.Hide();
      this.Mon2 = (SplashMonitor) null;
    }

    public void ID_Monitors()
    {
      this.ID_Monitors_Clear();
      Application.DoEvents();
      Thread.Sleep(500);
      Screen[] allScreens1 = Screen.AllScreens;
      Rectangle bounds1 = allScreens1[0].Bounds;
      this.Mon1 = new SplashMonitor(bounds1.X > 0 ? "2" : "1");
      this.Mon1.SetBounds(bounds1.X + bounds1.Width, bounds1.Height, 200, 200);
      this.Mon1.Show();
      if (allScreens1.Length > 1)
      {
        Rectangle bounds2 = allScreens1[1].Bounds;
        this.Mon2 = new SplashMonitor(bounds2.X > 0 ? "2" : "1");
        this.Mon2.SetBounds(bounds2.X + bounds2.Width, bounds2.Height, 200, 200);
        this.Mon2.Show();
        Screen[] allScreens2 = Screen.AllScreens;
        bounds1 = Screen.PrimaryScreen.Bounds;
        bounds2 = allScreens2[1].Bounds;
        if (bounds2.X <= 0)
          bounds2 = allScreens2[0].Bounds;
        this.MWin.SetBounds(bounds1.X, bounds1.Y, bounds1.Width, bounds1.Height);
        this.MWin.Update();
        if (this.opciones.Monitors > 1 && this.MWin.publi != null)
        {
          this.MWin.publi.SetBounds(bounds2.X, bounds2.Y, bounds2.Width, bounds2.Height);
          this.MWin.publi.Update();
        }
      }
      else
      {
        bounds1 = Screen.PrimaryScreen.Bounds;
        this.Mon1 = new SplashMonitor(bounds1.X > 0 ? "2" : "1");
        this.Mon1.SetBounds(bounds1.X + bounds1.Width, bounds1.Height, 200, 200);
        this.Mon1.Show();
        if (this.MWin.publi != null)
          this.MWin.publi.Hide();
      }
      this.Update_Info();
    }

    private void bCheck_Click(object sender, EventArgs e)
    {
      this.ID_Monitors();
    }

    private void bClone_Click(object sender, EventArgs e)
    {
      this.ID_Monitors_Clear();
      Process process = new Process();
      try
      {
        process.StartInfo.WorkingDirectory = "c:\\windows\\system32";
        process.StartInfo.FileName = "DisplaySwitch.exe";
        process.StartInfo.Arguments = "/clone";
        process.StartInfo.CreateNoWindow = true;
        process.StartInfo.WindowStyle = ProcessWindowStyle.Minimized;
        process.Start();
      }
      catch
      {
      }
      Application.DoEvents();
      Thread.Sleep(1000);
      this.ID_Monitors();
    }

    private void bMon21_Click(object sender, EventArgs e)
    {
      this.ID_Monitors_Clear();
      Process process = new Process();
      try
      {
        process.StartInfo.WorkingDirectory = "c:\\windows\\system32";
        process.StartInfo.FileName = "DisplaySwitch.exe";
        process.StartInfo.Arguments = "/extend";
        process.StartInfo.CreateNoWindow = true;
        process.StartInfo.WindowStyle = ProcessWindowStyle.Minimized;
        process.Start();
      }
      catch
      {
      }
      Application.DoEvents();
      Thread.Sleep(500);
      ControlDisplay.SwapMonitor(1, 0);
      Application.DoEvents();
      Thread.Sleep(1000);
      this.ID_Monitors();
    }

    private void bMon12_Click(object sender, EventArgs e)
    {
      this.ID_Monitors_Clear();
      Process process = new Process();
      try
      {
        process.StartInfo.WorkingDirectory = "c:\\windows\\system32";
        process.StartInfo.FileName = "DisplaySwitch.exe";
        process.StartInfo.Arguments = "/extend";
        process.StartInfo.CreateNoWindow = true;
        process.StartInfo.WindowStyle = ProcessWindowStyle.Minimized;
        process.Start();
      }
      catch
      {
      }
      Application.DoEvents();
      Thread.Sleep(500);
      ControlDisplay.SwapMonitor(0, 1);
      ControlDisplay.SwapMonitor(0, 1);
      Application.DoEvents();
      Thread.Sleep(1000);
      this.ID_Monitors();
    }

    private void bOk_Click(object sender, EventArgs e)
    {
      this.ID_Monitors_Clear();
      this.opciones.Publi = this.ePubli.Text;
      this.opciones.Monitors = this.cMon.Checked ? 2 : 1;
      this.Close();
    }

    private void bCancel_Click(object sender, EventArgs e)
    {
      this.ID_Monitors_Clear();
      this.Close();
    }

    private void DLG_Monitors_Load(object sender, EventArgs e)
    {
      this.ID_Monitors();
    }

    private void DLG_Monitors_FormClosed(object sender, FormClosedEventArgs e)
    {
      this.ID_Monitors_Clear();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.lPubli = new Label();
      this.ePubli = new TextBox();
      this.bCheck = new Button();
      this.bClone = new Button();
      this.bMon12 = new Button();
      this.bMon21 = new Button();
      this.bCancel = new Button();
      this.bOk = new Button();
      this.cMon = new CheckBox();
      this.lMon1 = new Label();
      this.lMon2 = new Label();
      this.SuspendLayout();
      this.lPubli.AutoSize = true;
      this.lPubli.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.lPubli.Location = new Point(12, 44);
      this.lPubli.Name = "lPubli";
      this.lPubli.Size = new Size(124, 13);
      this.lPubli.TabIndex = 39;
      this.lPubli.Text = "Slide show alternative url";
      this.ePubli.Location = new Point(12, 60);
      this.ePubli.Name = "ePubli";
      this.ePubli.Size = new Size(460, 20);
      this.ePubli.TabIndex = 40;
      this.bCheck.Location = new Point(12, 196);
      this.bCheck.Name = "bCheck";
      this.bCheck.Size = new Size(145, 40);
      this.bCheck.TabIndex = 43;
      this.bCheck.Text = "Check";
      this.bCheck.UseVisualStyleBackColor = true;
      this.bCheck.Click += new EventHandler(this.bCheck_Click);
      this.bClone.Location = new Point(12, 150);
      this.bClone.Name = "bClone";
      this.bClone.Size = new Size(145, 40);
      this.bClone.TabIndex = 44;
      this.bClone.Text = "Set Clone monitor";
      this.bClone.UseVisualStyleBackColor = true;
      this.bClone.Click += new EventHandler(this.bClone_Click);
      this.bMon12.Location = new Point(171, 150);
      this.bMon12.Name = "bMon12";
      this.bMon12.Size = new Size(145, 40);
      this.bMon12.TabIndex = 45;
      this.bMon12.Text = "Set Monitor 1/2";
      this.bMon12.UseVisualStyleBackColor = true;
      this.bMon12.Click += new EventHandler(this.bMon12_Click);
      this.bMon21.Location = new Point(330, 150);
      this.bMon21.Name = "bMon21";
      this.bMon21.Size = new Size(145, 40);
      this.bMon21.TabIndex = 46;
      this.bMon21.Text = "Set Monitor 2/1";
      this.bMon21.UseVisualStyleBackColor = true;
      this.bMon21.Click += new EventHandler(this.bMon21_Click);
      this.bCancel.BackgroundImage = (Image) Resources.ico_del;
      this.bCancel.BackgroundImageLayout = ImageLayout.Center;
      this.bCancel.Location = new Point(341, 233);
      this.bCancel.Name = "bCancel";
      this.bCancel.Size = new Size(64, 48);
      this.bCancel.TabIndex = 15;
      this.bCancel.UseVisualStyleBackColor = true;
      this.bCancel.Click += new EventHandler(this.bCancel_Click);
      this.bOk.BackgroundImage = (Image) Resources.ico_ok;
      this.bOk.BackgroundImageLayout = ImageLayout.Center;
      this.bOk.Location = new Point(411, 233);
      this.bOk.Name = "bOk";
      this.bOk.Size = new Size(64, 48);
      this.bOk.TabIndex = 14;
      this.bOk.UseVisualStyleBackColor = true;
      this.bOk.Click += new EventHandler(this.bOk_Click);
      this.cMon.AutoSize = true;
      this.cMon.Font = new Font("Microsoft Sans Serif", 20f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.cMon.Location = new Point(12, 6);
      this.cMon.Name = "cMon";
      this.cMon.Size = new Size(168, 35);
      this.cMon.TabIndex = 47;
      this.cMon.Text = "Slide Show";
      this.cMon.UseVisualStyleBackColor = true;
      this.lMon1.AutoSize = true;
      this.lMon1.Font = new Font("Microsoft Sans Serif", 10f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.lMon1.Location = new Point(15, 87);
      this.lMon1.Name = "lMon1";
      this.lMon1.Size = new Size(71, 17);
      this.lMon1.TabIndex = 48;
      this.lMon1.Text = "Monitor 1:";
      this.lMon2.AutoSize = true;
      this.lMon2.Font = new Font("Microsoft Sans Serif", 10f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.lMon2.Location = new Point(15, 112);
      this.lMon2.Name = "lMon2";
      this.lMon2.Size = new Size(71, 17);
      this.lMon2.TabIndex = 49;
      this.lMon2.Text = "Monitor 2:";
      this.AutoScaleMode = AutoScaleMode.None;
      this.ClientSize = new Size(484, 293);
      this.Controls.Add((Control) this.lMon2);
      this.Controls.Add((Control) this.lMon1);
      this.Controls.Add((Control) this.cMon);
      this.Controls.Add((Control) this.bMon21);
      this.Controls.Add((Control) this.bMon12);
      this.Controls.Add((Control) this.bClone);
      this.Controls.Add((Control) this.bCheck);
      this.Controls.Add((Control) this.ePubli);
      this.Controls.Add((Control) this.lPubli);
      this.Controls.Add((Control) this.bCancel);
      this.Controls.Add((Control) this.bOk);
      this.FormBorderStyle = FormBorderStyle.FixedToolWindow;
      this.Name = nameof (DLG_Monitors);
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Monitor Slide Show";
      this.FormClosed += new FormClosedEventHandler(this.DLG_Monitors_FormClosed);
      this.Load += new EventHandler(this.DLG_Monitors_Load);
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}

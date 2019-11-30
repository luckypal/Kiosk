// Decompiled with JetBrains decompiler
// Type: Kiosk.DLG_Calibrar
// Assembly: Kiosk, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: C3E32FFD-072D-4F9D-AAE4-A7F2B29E989A
// Assembly location: E:\kiosk\Kiosk.exe

using Kiosk.Properties;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace Kiosk
{
  public class DLG_Calibrar : Form
  {
    private IContainer components = (IContainer) null;
    public bool OK;
    public Configuracion opciones;
    private Button bOK;
    private Button bGALAX;
    private Button b3M;
    private Button eELO;
    private Button bETWO;
    private Button bGEN;
    private Button bTALENT;
    private Button bASUS;
    private Button bRESET;
    private Button bOFF;
    private Label lINFO;
    private Button bClock;

    public DLG_Calibrar(ref Configuracion _opc, int _modo)
    {
      this.OK = false;
      this.opciones = _opc;
      this.InitializeComponent();
      this.Localize();
      if (!File.Exists("c:\\Windows\\TouchUSM\\TouchCali.exe"))
        this.bTALENT.Enabled = false;
      if (!File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) + "\\Elo TouchSystems\\EloVa.exe"))
        this.eELO.Enabled = false;
      if (!File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) + "\\ETSerTouch\\etsercalib.exe"))
        this.bETWO.Enabled = false;
      if (!File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) + "\\MicroTouch\\MT 7\\TwCalib.exe"))
        this.b3M.Enabled = false;
      if (!File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) + "\\eGalaxTouch\\xAuto4PtsCal.exe"))
        this.bGALAX.Enabled = false;
      if (!File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) + "\\GeneralTouch\\APP\\x86\\GenCalib.exe"))
        this.bGEN.Enabled = false;
      if (!File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) + "\\Touch Package\\Touchpack.exe"))
        this.bASUS.Enabled = false;
      if (_modo == 0)
      {
        this.bRESET.Visible = false;
      }
      else
      {
        this.WindowState = FormWindowState.Maximized;
        this.FormBorderStyle = FormBorderStyle.None;
      }
      this.lINFO.Text = "Version " + this.opciones.VersionPRG + "\n[" + this.opciones.Srv_Ip + " - " + this.opciones.Srv_port + " - " + this.opciones.Srv_User + " - " + this.opciones.IDMAQUINA + "]\n";
    }

    private void Localize()
    {
      this.SuspendLayout();
      this.Text = this.opciones.Localize.Text("Touch Screen Calibration");
      this.ResumeLayout();
    }

    private void eELO_Click(object sender, EventArgs e)
    {
      string str = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) + "\\Elo TouchSystems\\EloVa.exe";
      if (File.Exists(str))
      {
        Process.Start(str);
      }
      else
      {
        int num = (int) MessageBox.Show("Missing: [" + str + "]");
      }
    }

    private void bOK_Click(object sender, EventArgs e)
    {
      this.Close();
    }

    private void bETWO_Click(object sender, EventArgs e)
    {
      string str = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) + "\\ETSerTouch\\etsercalib.exe";
      if (File.Exists(str))
      {
        Process.Start(str);
      }
      else
      {
        int num = (int) MessageBox.Show("Missing: [" + str + "]");
      }
    }

    private void b3M_Click(object sender, EventArgs e)
    {
      string str = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) + "\\MicroTouch\\MT 7\\TwCalib.exe";
      if (File.Exists(str))
      {
        Process.Start(str);
      }
      else
      {
        int num = (int) MessageBox.Show("Missing: [" + str + "]");
      }
    }

    private void bGALAX_Click(object sender, EventArgs e)
    {
      string str = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) + "\\eGalaxTouch\\xAuto4PtsCal.exe";
      if (File.Exists(str))
      {
        Process.Start(str);
      }
      else
      {
        int num = (int) MessageBox.Show("Missing: [" + str + "]");
      }
    }

    private void bGEN_Click(object sender, EventArgs e)
    {
      string str = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) + "\\GeneralTouch\\APP\\x86\\GenCalib.exe";
      if (File.Exists(str))
      {
        Process.Start(str);
      }
      else
      {
        int num = (int) MessageBox.Show("Missing: [" + str + "]");
      }
    }

    private void bTALENT_Click(object sender, EventArgs e)
    {
      string str = "c:\\Windows\\TouchUSM\\TouchCali.exe";
      if (File.Exists(str))
      {
        Process.Start(str);
      }
      else
      {
        int num = (int) MessageBox.Show("Missing: [" + str + "]");
      }
    }

    private void bASUS_Click(object sender, EventArgs e)
    {
      string str = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) + "\\Touch Package\\Touchpack.exe";
      if (File.Exists(str))
      {
        Process.Start(str);
      }
      else
      {
        int num = (int) MessageBox.Show("Missing: [" + str + "]");
      }
    }

    private void bRESET_Click(object sender, EventArgs e)
    {
      if (MessageBox.Show("Reset", "Stop", MessageBoxButtons.YesNo) != DialogResult.Yes)
        return;
      if (File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) + "\\uvnc bvba\\UltraVNC\\winvnc.exe") && Configuracion.VNC_Running())
        Configuracion.VNC_Build_Timestamp();
      Process.Start("shutdown.exe", "/r /t 2");
    }

    private void DLG_Calibrar_KeyPress(object sender, KeyPressEventArgs e)
    {
      switch (e.KeyChar)
      {
        case '0':
          this.bRESET_Click(sender, (EventArgs) null);
          break;
        case '1':
          this.eELO_Click(sender, (EventArgs) null);
          break;
        case '2':
          this.b3M_Click(sender, (EventArgs) null);
          break;
        case '3':
          this.bETWO_Click(sender, (EventArgs) null);
          break;
        case '4':
          this.bGALAX_Click(sender, (EventArgs) null);
          break;
        case '5':
          this.bGEN_Click(sender, (EventArgs) null);
          break;
        case '6':
          this.bTALENT_Click(sender, (EventArgs) null);
          break;
        case '7':
          this.bASUS_Click(sender, (EventArgs) null);
          break;
      }
    }

    private void DLG_Calibrar_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode != Keys.F10)
        return;
      this.Close();
    }

    private void eELO_KeyPress(object sender, KeyPressEventArgs e)
    {
      switch (e.KeyChar)
      {
        case '\r':
        case '1':
          this.eELO_Click(sender, (EventArgs) null);
          break;
        case '0':
          this.bRESET_Click(sender, (EventArgs) null);
          break;
        case '2':
          this.b3M_Click(sender, (EventArgs) null);
          break;
        case '3':
          this.bETWO_Click(sender, (EventArgs) null);
          break;
        case '4':
          this.bGALAX_Click(sender, (EventArgs) null);
          break;
        case '5':
          this.bGEN_Click(sender, (EventArgs) null);
          break;
        case '6':
          this.bTALENT_Click(sender, (EventArgs) null);
          break;
        case '7':
          this.bASUS_Click(sender, (EventArgs) null);
          break;
      }
    }

    private void bOFF_Click(object sender, EventArgs e)
    {
      if (File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) + "\\uvnc bvba\\UltraVNC\\winvnc.exe") && Configuracion.VNC_Running())
        Configuracion.VNC_Build_Timestamp();
      Process.Start("shutdown.exe", "/s /t 2");
    }

    private void b3M_KeyPress(object sender, KeyPressEventArgs e)
    {
      switch (e.KeyChar)
      {
        case '\r':
        case '2':
          this.b3M_Click(sender, (EventArgs) null);
          break;
        case '1':
          this.eELO_Click(sender, (EventArgs) null);
          break;
        case '3':
          this.bETWO_Click(sender, (EventArgs) null);
          break;
        case '4':
          this.bGALAX_Click(sender, (EventArgs) null);
          break;
        case '5':
          this.bGEN_Click(sender, (EventArgs) null);
          break;
        case '6':
          this.bTALENT_Click(sender, (EventArgs) null);
          break;
        case '7':
          this.bASUS_Click(sender, (EventArgs) null);
          break;
        case 'T':
        case 't':
          this.bClock_Click(sender, (EventArgs) null);
          break;
        case 'X':
        case 'x':
          this.bRESET_Click(sender, (EventArgs) null);
          break;
      }
    }

    private void bETWO_KeyPress(object sender, KeyPressEventArgs e)
    {
      switch (e.KeyChar)
      {
        case '\r':
        case '3':
          this.bETWO_Click(sender, (EventArgs) null);
          break;
        case '1':
          this.eELO_Click(sender, (EventArgs) null);
          break;
        case '2':
          this.b3M_Click(sender, (EventArgs) null);
          break;
        case '4':
          this.bGALAX_Click(sender, (EventArgs) null);
          break;
        case '5':
          this.bGEN_Click(sender, (EventArgs) null);
          break;
        case '6':
          this.bTALENT_Click(sender, (EventArgs) null);
          break;
        case '7':
          this.bASUS_Click(sender, (EventArgs) null);
          break;
        case 'T':
        case 't':
          this.bClock_Click(sender, (EventArgs) null);
          break;
        case 'X':
        case 'x':
          this.bRESET_Click(sender, (EventArgs) null);
          break;
      }
    }

    private void bGALAX_KeyPress(object sender, KeyPressEventArgs e)
    {
      switch (e.KeyChar)
      {
        case '\r':
        case '4':
          this.bGALAX_Click(sender, (EventArgs) null);
          break;
        case '1':
          this.eELO_Click(sender, (EventArgs) null);
          break;
        case '2':
          this.b3M_Click(sender, (EventArgs) null);
          break;
        case '3':
          this.bETWO_Click(sender, (EventArgs) null);
          break;
        case '5':
          this.bGEN_Click(sender, (EventArgs) null);
          break;
        case '6':
          this.bTALENT_Click(sender, (EventArgs) null);
          break;
        case '7':
          this.bASUS_Click(sender, (EventArgs) null);
          break;
        case 'T':
        case 't':
          this.bClock_Click(sender, (EventArgs) null);
          break;
        case 'X':
        case 'x':
          this.bRESET_Click(sender, (EventArgs) null);
          break;
      }
    }

    private void bGEN_KeyPress(object sender, KeyPressEventArgs e)
    {
      switch (e.KeyChar)
      {
        case '\r':
        case '5':
          this.bGEN_Click(sender, (EventArgs) null);
          break;
        case '1':
          this.eELO_Click(sender, (EventArgs) null);
          break;
        case '2':
          this.b3M_Click(sender, (EventArgs) null);
          break;
        case '3':
          this.bETWO_Click(sender, (EventArgs) null);
          break;
        case '4':
          this.bGALAX_Click(sender, (EventArgs) null);
          break;
        case '6':
          this.bTALENT_Click(sender, (EventArgs) null);
          break;
        case '7':
          this.bASUS_Click(sender, (EventArgs) null);
          break;
        case 'T':
        case 't':
          this.bClock_Click(sender, (EventArgs) null);
          break;
        case 'X':
        case 'x':
          this.bRESET_Click(sender, (EventArgs) null);
          break;
      }
    }

    private void bTALENT_KeyPress(object sender, KeyPressEventArgs e)
    {
      switch (e.KeyChar)
      {
        case '\r':
        case '6':
          this.bTALENT_Click(sender, (EventArgs) null);
          break;
        case '1':
          this.eELO_Click(sender, (EventArgs) null);
          break;
        case '2':
          this.b3M_Click(sender, (EventArgs) null);
          break;
        case '3':
          this.bETWO_Click(sender, (EventArgs) null);
          break;
        case '4':
          this.bGALAX_Click(sender, (EventArgs) null);
          break;
        case '5':
          this.bGEN_Click(sender, (EventArgs) null);
          break;
        case '7':
          this.bASUS_Click(sender, (EventArgs) null);
          break;
        case 'T':
        case 't':
          this.bClock_Click(sender, (EventArgs) null);
          break;
        case 'X':
        case 'x':
          this.bRESET_Click(sender, (EventArgs) null);
          break;
      }
    }

    private void bASUS_KeyPress(object sender, KeyPressEventArgs e)
    {
      switch (e.KeyChar)
      {
        case '\r':
        case '7':
          this.bASUS_Click(sender, (EventArgs) null);
          break;
        case '1':
          this.eELO_Click(sender, (EventArgs) null);
          break;
        case '2':
          this.b3M_Click(sender, (EventArgs) null);
          break;
        case '3':
          this.bETWO_Click(sender, (EventArgs) null);
          break;
        case '4':
          this.bGALAX_Click(sender, (EventArgs) null);
          break;
        case '5':
          this.bGEN_Click(sender, (EventArgs) null);
          break;
        case '6':
          this.bTALENT_Click(sender, (EventArgs) null);
          break;
        case 'T':
        case 't':
          this.bClock_Click(sender, (EventArgs) null);
          break;
        case 'X':
        case 'x':
          this.bRESET_Click(sender, (EventArgs) null);
          break;
      }
    }

    private void bOFF_KeyPress(object sender, KeyPressEventArgs e)
    {
      switch (e.KeyChar)
      {
        case '\r':
          this.bOFF_Click(sender, (EventArgs) null);
          break;
        case '1':
          this.eELO_Click(sender, (EventArgs) null);
          break;
        case '2':
          this.b3M_Click(sender, (EventArgs) null);
          break;
        case '3':
          this.bETWO_Click(sender, (EventArgs) null);
          break;
        case '4':
          this.bGALAX_Click(sender, (EventArgs) null);
          break;
        case '5':
          this.bGEN_Click(sender, (EventArgs) null);
          break;
        case '6':
          this.bTALENT_Click(sender, (EventArgs) null);
          break;
        case '7':
          this.bASUS_Click(sender, (EventArgs) null);
          break;
        case 'T':
        case 't':
          this.bClock_Click(sender, (EventArgs) null);
          break;
        case 'X':
        case 'x':
          this.bRESET_Click(sender, (EventArgs) null);
          break;
      }
    }

    private void bRESET_KeyPress(object sender, KeyPressEventArgs e)
    {
      switch (e.KeyChar)
      {
        case '\r':
        case 'X':
        case 'x':
          this.bRESET_Click(sender, (EventArgs) null);
          break;
        case '1':
          this.eELO_Click(sender, (EventArgs) null);
          break;
        case '2':
          this.b3M_Click(sender, (EventArgs) null);
          break;
        case '3':
          this.bETWO_Click(sender, (EventArgs) null);
          break;
        case '4':
          this.bGALAX_Click(sender, (EventArgs) null);
          break;
        case '5':
          this.bGEN_Click(sender, (EventArgs) null);
          break;
        case '6':
          this.bTALENT_Click(sender, (EventArgs) null);
          break;
        case '7':
          this.bASUS_Click(sender, (EventArgs) null);
          break;
        case 'T':
        case 't':
          this.bClock_Click(sender, (EventArgs) null);
          break;
      }
    }

    private void bOK_KeyPress(object sender, KeyPressEventArgs e)
    {
      switch (e.KeyChar)
      {
        case '\r':
          this.bOK_Click(sender, (EventArgs) null);
          break;
        case '1':
          this.eELO_Click(sender, (EventArgs) null);
          break;
        case '2':
          this.b3M_Click(sender, (EventArgs) null);
          break;
        case '3':
          this.bETWO_Click(sender, (EventArgs) null);
          break;
        case '4':
          this.bGALAX_Click(sender, (EventArgs) null);
          break;
        case '5':
          this.bGEN_Click(sender, (EventArgs) null);
          break;
        case '6':
          this.bTALENT_Click(sender, (EventArgs) null);
          break;
        case '7':
          this.bASUS_Click(sender, (EventArgs) null);
          break;
        case 'T':
        case 't':
          this.bClock_Click(sender, (EventArgs) null);
          break;
        case 'X':
        case 'x':
          this.bRESET_Click(sender, (EventArgs) null);
          break;
      }
    }

    private void bClock_Click(object sender, EventArgs e)
    {
      MSGBOX_Timer msgboxTimer = new MSGBOX_Timer("Updating Date/Time from Internet server. Wait 30 seconds", "", 30, true);
      msgboxTimer.Show();
      Program.Update_DateTime();
      Thread.Sleep(30000);
      msgboxTimer.Close();
      int num = (int) new MSGBOX_Timer("Current Date/Time " + DateTime.Now.ToLongDateString(), "", 5, true).ShowDialog();
    }

    private void bClock_KeyPress(object sender, KeyPressEventArgs e)
    {
      switch (e.KeyChar)
      {
        case '\r':
        case 'T':
        case 't':
          this.bClock_Click(sender, (EventArgs) null);
          break;
        case '1':
          this.eELO_Click(sender, (EventArgs) null);
          break;
        case '2':
          this.b3M_Click(sender, (EventArgs) null);
          break;
        case '3':
          this.bETWO_Click(sender, (EventArgs) null);
          break;
        case '4':
          this.bGALAX_Click(sender, (EventArgs) null);
          break;
        case '5':
          this.bGEN_Click(sender, (EventArgs) null);
          break;
        case '6':
          this.bTALENT_Click(sender, (EventArgs) null);
          break;
        case '7':
          this.bASUS_Click(sender, (EventArgs) null);
          break;
        case 'X':
        case 'x':
          this.bRESET_Click(sender, (EventArgs) null);
          break;
      }
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.lINFO = new Label();
      this.bClock = new Button();
      this.bOFF = new Button();
      this.bRESET = new Button();
      this.bASUS = new Button();
      this.bTALENT = new Button();
      this.bGEN = new Button();
      this.bETWO = new Button();
      this.eELO = new Button();
      this.b3M = new Button();
      this.bGALAX = new Button();
      this.bOK = new Button();
      this.SuspendLayout();
      this.lINFO.AutoSize = true;
      this.lINFO.Font = new Font("Microsoft Sans Serif", 16f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.lINFO.Location = new Point(13, 13);
      this.lINFO.Name = "lINFO";
      this.lINFO.Size = new Size(19, 26);
      this.lINFO.TabIndex = 10;
      this.lINFO.Text = "-";
      this.bClock.Font = new Font("Microsoft Sans Serif", 10f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.bClock.Image = (Image) Resources.Clock;
      this.bClock.Location = new Point(141, 170);
      this.bClock.Name = "bClock";
      this.bClock.Size = new Size(110, 96);
      this.bClock.TabIndex = 11;
      this.bClock.Text = "Set Time (T)";
      this.bClock.TextAlign = ContentAlignment.BottomCenter;
      this.bClock.UseVisualStyleBackColor = true;
      this.bClock.Click += new EventHandler(this.bClock_Click);
      this.bClock.KeyPress += new KeyPressEventHandler(this.bClock_KeyPress);
      this.bOFF.BackColor = Color.Orange;
      this.bOFF.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.bOFF.Image = (Image) Resources.ico_poff;
      this.bOFF.Location = new Point(257, 170);
      this.bOFF.Name = "bOFF";
      this.bOFF.Size = new Size(110, 96);
      this.bOFF.TabIndex = 9;
      this.bOFF.Text = "Power OFF";
      this.bOFF.TextAlign = ContentAlignment.BottomCenter;
      this.bOFF.UseVisualStyleBackColor = false;
      this.bOFF.Click += new EventHandler(this.bOFF_Click);
      this.bOFF.KeyPress += new KeyPressEventHandler(this.bOFF_KeyPress);
      this.bRESET.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.bRESET.Image = (Image) Resources.ico_poff;
      this.bRESET.Location = new Point(367, 170);
      this.bRESET.Name = "bRESET";
      this.bRESET.Size = new Size(110, 96);
      this.bRESET.TabIndex = 7;
      this.bRESET.Text = "Reboot (X)";
      this.bRESET.TextAlign = ContentAlignment.BottomCenter;
      this.bRESET.UseVisualStyleBackColor = true;
      this.bRESET.Click += new EventHandler(this.bRESET_Click);
      this.bRESET.KeyPress += new KeyPressEventHandler(this.bRESET_KeyPress);
      this.bASUS.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.bASUS.Image = (Image) Resources.Asus;
      this.bASUS.Location = new Point(12, 170);
      this.bASUS.Name = "bASUS";
      this.bASUS.Size = new Size(96, 96);
      this.bASUS.TabIndex = 6;
      this.bASUS.Text = "7";
      this.bASUS.TextAlign = ContentAlignment.BottomCenter;
      this.bASUS.UseVisualStyleBackColor = true;
      this.bASUS.Click += new EventHandler(this.bASUS_Click);
      this.bASUS.KeyPress += new KeyPressEventHandler(this.bASUS_KeyPress);
      this.bTALENT.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.bTALENT.Image = (Image) Resources.ico_ttouch;
      this.bTALENT.Location = new Point(492, 68);
      this.bTALENT.Name = "bTALENT";
      this.bTALENT.Size = new Size(96, 96);
      this.bTALENT.TabIndex = 5;
      this.bTALENT.Text = "6";
      this.bTALENT.TextAlign = ContentAlignment.BottomCenter;
      this.bTALENT.UseVisualStyleBackColor = true;
      this.bTALENT.Click += new EventHandler(this.bTALENT_Click);
      this.bTALENT.KeyPress += new KeyPressEventHandler(this.bTALENT_KeyPress);
      this.bGEN.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.bGEN.Image = (Image) Resources.ico_general;
      this.bGEN.Location = new Point(396, 68);
      this.bGEN.Name = "bGEN";
      this.bGEN.Size = new Size(96, 96);
      this.bGEN.TabIndex = 4;
      this.bGEN.Text = "5";
      this.bGEN.TextAlign = ContentAlignment.BottomCenter;
      this.bGEN.UseVisualStyleBackColor = true;
      this.bGEN.Click += new EventHandler(this.bGEN_Click);
      this.bGEN.KeyPress += new KeyPressEventHandler(this.bGEN_KeyPress);
      this.bETWO.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.bETWO.Image = (Image) Resources.ico_etwo;
      this.bETWO.Location = new Point(204, 68);
      this.bETWO.Name = "bETWO";
      this.bETWO.Size = new Size(96, 96);
      this.bETWO.TabIndex = 2;
      this.bETWO.Text = "3";
      this.bETWO.TextAlign = ContentAlignment.BottomCenter;
      this.bETWO.UseVisualStyleBackColor = true;
      this.bETWO.Click += new EventHandler(this.bETWO_Click);
      this.bETWO.KeyPress += new KeyPressEventHandler(this.bETWO_KeyPress);
      this.eELO.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.eELO.Image = (Image) Resources.ico_elo;
      this.eELO.Location = new Point(12, 68);
      this.eELO.Name = "eELO";
      this.eELO.Size = new Size(96, 96);
      this.eELO.TabIndex = 0;
      this.eELO.Text = "1";
      this.eELO.TextAlign = ContentAlignment.BottomCenter;
      this.eELO.UseVisualStyleBackColor = true;
      this.eELO.Click += new EventHandler(this.eELO_Click);
      this.eELO.KeyPress += new KeyPressEventHandler(this.eELO_KeyPress);
      this.b3M.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.b3M.Image = (Image) Resources.ico_3m;
      this.b3M.Location = new Point(108, 68);
      this.b3M.Name = "b3M";
      this.b3M.Size = new Size(96, 96);
      this.b3M.TabIndex = 1;
      this.b3M.Text = "2";
      this.b3M.TextAlign = ContentAlignment.BottomCenter;
      this.b3M.UseVisualStyleBackColor = true;
      this.b3M.Click += new EventHandler(this.b3M_Click);
      this.b3M.KeyPress += new KeyPressEventHandler(this.b3M_KeyPress);
      this.bGALAX.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.bGALAX.Image = (Image) Resources.ico_galax;
      this.bGALAX.Location = new Point(300, 68);
      this.bGALAX.Name = "bGALAX";
      this.bGALAX.Size = new Size(96, 96);
      this.bGALAX.TabIndex = 3;
      this.bGALAX.Text = "4";
      this.bGALAX.TextAlign = ContentAlignment.BottomCenter;
      this.bGALAX.UseVisualStyleBackColor = true;
      this.bGALAX.Click += new EventHandler(this.bGALAX_Click);
      this.bGALAX.KeyPress += new KeyPressEventHandler(this.bGALAX_KeyPress);
      this.bOK.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.bOK.Image = (Image) Resources.ico_ok;
      this.bOK.Location = new Point(477, 170);
      this.bOK.Name = "bOK";
      this.bOK.Size = new Size(110, 96);
      this.bOK.TabIndex = 8;
      this.bOK.Text = "Exit F10";
      this.bOK.TextAlign = ContentAlignment.BottomCenter;
      this.bOK.UseVisualStyleBackColor = true;
      this.bOK.Click += new EventHandler(this.bOK_Click);
      this.bOK.KeyPress += new KeyPressEventHandler(this.bOK_KeyPress);
      this.AutoScaleMode = AutoScaleMode.None;
      this.ClientSize = new Size(598, 341);
      this.Controls.Add((Control) this.bClock);
      this.Controls.Add((Control) this.lINFO);
      this.Controls.Add((Control) this.bOFF);
      this.Controls.Add((Control) this.bRESET);
      this.Controls.Add((Control) this.bASUS);
      this.Controls.Add((Control) this.bTALENT);
      this.Controls.Add((Control) this.bGEN);
      this.Controls.Add((Control) this.bETWO);
      this.Controls.Add((Control) this.eELO);
      this.Controls.Add((Control) this.b3M);
      this.Controls.Add((Control) this.bGALAX);
      this.Controls.Add((Control) this.bOK);
      this.FormBorderStyle = FormBorderStyle.FixedToolWindow;
      this.Name = nameof (DLG_Calibrar);
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Calibration";
      this.KeyDown += new KeyEventHandler(this.DLG_Calibrar_KeyDown);
      this.KeyPress += new KeyPressEventHandler(this.DLG_Calibrar_KeyPress);
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}

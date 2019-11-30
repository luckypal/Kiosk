// Decompiled with JetBrains decompiler
// Type: Kiosk.DLG_Config
// Assembly: Kiosk, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: C3E32FFD-072D-4F9D-AAE4-A7F2B29E989A
// Assembly location: E:\kiosk\Kiosk.exe

using Kiosk.Properties;
using Microsoft.Win32;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace Kiosk
{
  public class DLG_Config : Form
  {
    private int mw = 0;
    private int reboot = 0;
    private IContainer components = (IContainer) null;
    public MainWindow MWin;
    public bool OK;
    public Configuracion opciones;
    private Button bCancel;
    private Button bOK;
    private Button bSponsor;
    private Button bValidators;
    private Button bPassword;
    private Button bOptions;
    private Button bPanel;
    private Button bExplorer;
    private Button bMin;
    private Button bKey;
    private Button bDepositaire;
    private Button button1;
    private Button bRESET;
    private Button bTouch;
    private Button bMonitors;
    private Button button2;
    private Button btn_mail;
    private Label lInfo;
    private Button bVNC;
    private Button bVNC2;
    private Button bFreeze;
    private Button bWifi;
    private Button btnTICKET;

    public DLG_Config(ref Configuracion _opc)
    {
      this.OK = false;
      this.opciones = _opc;
      this.InitializeComponent();
      this.Localize();
      this.Text = "Kiosk v" + _opc.VersionPRG + " ID:" + _opc.IDMAQUINA;
      this.opciones.Log_Debug("Enter config mode");
      this.MWin = (MainWindow) null;
      this.reboot = 0;
      this.mw = 0;
    }

    private void Localize()
    {
      this.SuspendLayout();
      this.bOptions.Text = "1. " + this.opciones.Localize.Text("Preferences");
      this.bSponsor.Text = "2. " + this.opciones.Localize.Text("Server");
      this.bValidators.Text = "3. " + this.opciones.Localize.Text("Validators");
      this.bPassword.Text = "4. " + this.opciones.Localize.Text("Password");
      this.bDepositaire.Text = "5. " + this.opciones.Localize.Text("Ticket");
      this.bMonitors.Text = "6. " + this.opciones.Localize.Text("Slide Show");
      this.ResumeLayout();
    }

    private void bSponsor_Click(object sender, EventArgs e)
    {
      DLG_Sponsors dlgSponsors = new DLG_Sponsors(ref this.opciones);
      dlgSponsors.Focus();
      int num = (int) dlgSponsors.ShowDialog();
    }

    private void bValidators_Click(object sender, EventArgs e)
    {
      Devices_Wizard devicesWizard = new Devices_Wizard(ref this.opciones);
      devicesWizard.Focus();
      int num = (int) devicesWizard.ShowDialog();
      this.lInfo.Text = "Coins: " + this.opciones.Dev_Coin.ToUpper() + " (" + this.opciones.Dev_Coin_P.ToUpper() + ") / Notes: " + this.opciones.Dev_BNV.ToUpper() + " (" + this.opciones.Dev_BNV_P.ToUpper() + ")";
    }

    private void bOK_Click(object sender, EventArgs e)
    {
      Configuracion.Freeze_On();
      this.OK = false;
      this.opciones.Save_Net();
      this.opciones.Load_Net();
      this.opciones.SEND_Mail("SAVE", this.opciones.Update_Info());
      this.Close();
    }

    private void bCancel_Click(object sender, EventArgs e)
    {
      Configuracion.Freeze_On();
      this.Close();
    }

    private void bPassword_Click(object sender, EventArgs e)
    {
      int num = (int) new DLG_Password_Change(ref this.opciones, 1).ShowDialog();
    }

    private void bOptions_Click(object sender, EventArgs e)
    {
      int num = (int) new DLG_Preferencias(ref this.opciones, ref this.MWin).ShowDialog();
      this.Localize();
    }

    private void bNet_Click(object sender, EventArgs e)
    {
    }

    private void bExplorer_Click(object sender, EventArgs e)
    {
      Process.Start("explorer.exe");
    }

    private void bPanel_Click(object sender, EventArgs e)
    {
      this.mw = 1;
      this.MWin.UnLock_Windows();
      Configuracion.Freeze_Off();
      Process.Start("control.exe");
    }

    private void DLG_Config_Load(object sender, EventArgs e)
    {
      if (Configuracion.Freeze_Check() == 1)
        this.bFreeze.BackColor = System.Drawing.Color.Red;
      string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
      if (File.Exists(folderPath + "\\uvnc bvba\\UltraVNC\\winvnc.exe"))
      {
        this.bVNC.Visible = true;
        this.bVNC2.Visible = true;
      }
      if (!File.Exists(folderPath + "\\Toolwiz Time Freeze 2014\\ToolwizTimeFreeze.exe"))
        this.bFreeze.Visible = false;
      this.lInfo.Text = "Coins: " + this.opciones.Dev_Coin.ToUpper() + " (" + this.opciones.Dev_Coin_P.ToUpper() + ") / Notes: " + this.opciones.Dev_BNV.ToUpper() + " (" + this.opciones.Dev_BNV_P.ToUpper() + ")";
      this.opciones.SEND_Mail("CONFIG", "ENTER TO CONFIG");
    }

    private void bMin_Click(object sender, EventArgs e)
    {
      if (this.MWin == null)
        return;
      if (this.MWin.Visible)
        this.MWin.Hide();
      else
        this.MWin.Show();
    }

    private void DLG_Config_FormClosed(object sender, FormClosedEventArgs e)
    {
      if (this.mw == 1 && this.reboot == 0)
        this.MWin.Lock_Windows();
      if (this.MWin == null)
        return;
      this.MWin.Show();
    }

    private void bKey_Click(object sender, EventArgs e)
    {
      if (Process.GetProcessesByName("KVKeyboard").Length >= 1)
        return;
      Process.Start("KVKeyboard.exe", "es");
    }

    private void button1_Click(object sender, EventArgs e)
    {
      int num = (int) new DLG_Info(ref this.opciones).ShowDialog();
    }

    private void bDepositaire_Click(object sender, EventArgs e)
    {
      int num = (int) new DLG_Depositaire(ref this.opciones)
      {
        MWin = this.MWin
      }.ShowDialog();
    }

    private void bRESET_Click(object sender, EventArgs e)
    {
      Configuracion.Freeze_On();
      if (MessageBox.Show("Reset", "Stop", MessageBoxButtons.YesNo) != DialogResult.Yes)
        return;
      if (File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) + "\\uvnc bvba\\UltraVNC\\winvnc.exe") && Configuracion.VNC_Running())
        Configuracion.VNC_Build_Timestamp();
      Process.Start("shutdown.exe", "/r /t 2");
    }

    private void bTouch_Click(object sender, EventArgs e)
    {
      DLG_Calibrar dlgCalibrar = new DLG_Calibrar(ref this.opciones, 0);
      dlgCalibrar.Focus();
      int num = (int) dlgCalibrar.ShowDialog();
    }

    private void bMonitors_Click(object sender, EventArgs e)
    {
      DLG_Monitors dlgMonitors = new DLG_Monitors(ref this.opciones);
      dlgMonitors.Focus();
      dlgMonitors.MWin = this.MWin;
      int num = (int) dlgMonitors.ShowDialog();
    }

    private void button2_Click(object sender, EventArgs e)
    {
      if (MessageBox.Show("Turn to Windows Mode", "Stop", MessageBoxButtons.YesNo) != DialogResult.Yes)
        return;
      DLG_Login dlgLogin = new DLG_Login(ref this.opciones, 1);
      Application.DoEvents();
      dlgLogin.Focus();
      int num = (int) dlgLogin.ShowDialog();
      if (dlgLogin.Logeado == 1)
      {
        this.reboot = 1;
        if (File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) + "\\Toolwiz Time Freeze 2014\\ToolwizTimeFreeze.exe") && Configuracion.Freeze_Check() == 1)
        {
          Configuracion.Freeze_Build_Timestamp();
          Configuracion.Freeze_Build_TimestampBoot();
          Configuracion.Freeze_Off();
          Configuracion.WinReset();
        }
        else
        {
          this.MWin.UnLock_Windows();
          try
          {
            RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\WinLogon", true);
            registryKey.SetValue("Shell", (object) "explorer.exe", RegistryValueKind.String);
            registryKey.Close();
          }
          catch
          {
          }
          try
          {
            RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\WinLogon", true);
            registryKey.SetValue("Shell", (object) "explorer.exe", RegistryValueKind.String);
            registryKey.Close();
          }
          catch
          {
          }
          Configuracion.Access_Log("Turn Windows Mode");
          Configuracion.WinReset();
        }
      }
    }

    private void btn_mail_Click(object sender, EventArgs e)
    {
      this.opciones.SEND_Mail_Pub("STATUS", this.opciones.Update_Info());
    }

    private void bVNC_Click(object sender, EventArgs e)
    {
      string str = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) + "\\uvnc bvba\\UltraVNC\\winvnc.exe";
      if (File.Exists(str))
      {
        if (Configuracion.VNC_Running())
        {
          Process.Start(str, "-kill");
          Thread.Sleep(5000);
        }
        Process.Start(str, "-connect " + this.opciones.Server_VNC + ":5500 -run");
        int num = (int) MessageBox.Show("Waiting connection");
      }
      else
      {
        int num1 = (int) MessageBox.Show("No VNC Installed");
      }
    }

    private void button3_Click(object sender, EventArgs e)
    {
      string str = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) + "\\uvnc bvba\\UltraVNC\\winvnc.exe";
      if (File.Exists(str))
      {
        if (!Configuracion.VNC_Running())
          return;
        Process.Start(str, "-kill");
      }
      else
      {
        int num = (int) MessageBox.Show("No VNC Installed");
      }
    }

    private void bFreeze_Click(object sender, EventArgs e)
    {
      if (Configuracion.Freeze_Check() == 1)
      {
        if (MessageBox.Show("Turn to UNFREEZED MODE", "Stop", MessageBoxButtons.YesNo) != DialogResult.Yes)
          return;
        DLG_Login dlgLogin = new DLG_Login(ref this.opciones, 1);
        Application.DoEvents();
        dlgLogin.Focus();
        int num = (int) dlgLogin.ShowDialog();
        if (dlgLogin.Logeado == 1)
        {
          Configuracion.Freeze_Build_Timestamp();
          Configuracion.Freeze_Off();
          Configuracion.WinReset();
        }
      }
      else
      {
        int num1 = (int) MessageBox.Show("System is UNFREEZED");
      }
    }

    private void bWifi_Click(object sender, EventArgs e)
    {
      int num = (int) new DLG_Wifi(ref this.opciones).ShowDialog();
    }

    private void bOptions_KeyPress(object sender, KeyPressEventArgs e)
    {
      switch (e.KeyChar)
      {
        case '\r':
        case '1':
          this.bOptions_Click(sender, (EventArgs) null);
          break;
        case '2':
          this.bSponsor_Click(sender, (EventArgs) null);
          break;
        case '3':
          this.bValidators_Click(sender, (EventArgs) null);
          break;
        case '4':
          this.bPassword_Click(sender, (EventArgs) null);
          break;
        case '5':
          this.bDepositaire_Click(sender, (EventArgs) null);
          break;
        case '6':
          this.bMonitors_Click(sender, (EventArgs) null);
          break;
        case 'R':
        case 'r':
          this.bVNC_Click(sender, (EventArgs) null);
          break;
        case 'S':
        case 's':
          this.bOK_Click(sender, (EventArgs) null);
          break;
        case 'T':
        case 't':
          this.bTouch_Click(sender, (EventArgs) null);
          break;
        case 'W':
        case 'w':
          this.button2_Click(sender, (EventArgs) null);
          break;
        case 'X':
        case 'x':
          this.bCancel_Click(sender, (EventArgs) null);
          break;
      }
    }

    private void bSponsor_KeyPress(object sender, KeyPressEventArgs e)
    {
      switch (e.KeyChar)
      {
        case '\r':
        case '2':
          this.bSponsor_Click(sender, (EventArgs) null);
          break;
        case '1':
          this.bOptions_Click(sender, (EventArgs) null);
          break;
        case '3':
          this.bValidators_Click(sender, (EventArgs) null);
          break;
        case '4':
          this.bPassword_Click(sender, (EventArgs) null);
          break;
        case '5':
          this.bDepositaire_Click(sender, (EventArgs) null);
          break;
        case '6':
          this.bMonitors_Click(sender, (EventArgs) null);
          break;
        case 'R':
        case 'r':
          this.bVNC_Click(sender, (EventArgs) null);
          break;
        case 'S':
        case 's':
          this.bOK_Click(sender, (EventArgs) null);
          break;
        case 'T':
        case 't':
          this.bTouch_Click(sender, (EventArgs) null);
          break;
        case 'W':
        case 'w':
          this.button2_Click(sender, (EventArgs) null);
          break;
        case 'X':
        case 'x':
          this.bCancel_Click(sender, (EventArgs) null);
          break;
      }
    }

    private void bValidators_KeyPress(object sender, KeyPressEventArgs e)
    {
      switch (e.KeyChar)
      {
        case '\r':
        case '3':
          this.bValidators_Click(sender, (EventArgs) null);
          break;
        case '1':
          this.bOptions_Click(sender, (EventArgs) null);
          break;
        case '2':
          this.bSponsor_Click(sender, (EventArgs) null);
          break;
        case '4':
          this.bPassword_Click(sender, (EventArgs) null);
          break;
        case '5':
          this.bDepositaire_Click(sender, (EventArgs) null);
          break;
        case '6':
          this.bMonitors_Click(sender, (EventArgs) null);
          break;
        case 'R':
        case 'r':
          this.bVNC_Click(sender, (EventArgs) null);
          break;
        case 'S':
        case 's':
          this.bOK_Click(sender, (EventArgs) null);
          break;
        case 'T':
        case 't':
          this.bTouch_Click(sender, (EventArgs) null);
          break;
        case 'W':
        case 'w':
          this.button2_Click(sender, (EventArgs) null);
          break;
        case 'X':
        case 'x':
          this.bCancel_Click(sender, (EventArgs) null);
          break;
      }
    }

    private void bPassword_KeyPress(object sender, KeyPressEventArgs e)
    {
      switch (e.KeyChar)
      {
        case '\r':
        case '4':
          this.bPassword_Click(sender, (EventArgs) null);
          break;
        case '1':
          this.bOptions_Click(sender, (EventArgs) null);
          break;
        case '2':
          this.bSponsor_Click(sender, (EventArgs) null);
          break;
        case '3':
          this.bValidators_Click(sender, (EventArgs) null);
          break;
        case '5':
          this.bDepositaire_Click(sender, (EventArgs) null);
          break;
        case '6':
          this.bMonitors_Click(sender, (EventArgs) null);
          break;
        case 'R':
        case 'r':
          this.bVNC_Click(sender, (EventArgs) null);
          break;
        case 'S':
        case 's':
          this.bOK_Click(sender, (EventArgs) null);
          break;
        case 'T':
        case 't':
          this.bTouch_Click(sender, (EventArgs) null);
          break;
        case 'W':
        case 'w':
          this.button2_Click(sender, (EventArgs) null);
          break;
        case 'X':
        case 'x':
          this.bCancel_Click(sender, (EventArgs) null);
          break;
      }
    }

    private void bDepositaire_KeyPress(object sender, KeyPressEventArgs e)
    {
      switch (e.KeyChar)
      {
        case '\r':
        case '5':
          this.bDepositaire_Click(sender, (EventArgs) null);
          break;
        case '1':
          this.bOptions_Click(sender, (EventArgs) null);
          break;
        case '2':
          this.bSponsor_Click(sender, (EventArgs) null);
          break;
        case '3':
          this.bValidators_Click(sender, (EventArgs) null);
          break;
        case '4':
          this.bPassword_Click(sender, (EventArgs) null);
          break;
        case '6':
          this.bMonitors_Click(sender, (EventArgs) null);
          break;
        case 'R':
        case 'r':
          this.bVNC_Click(sender, (EventArgs) null);
          break;
        case 'S':
        case 's':
          this.bOK_Click(sender, (EventArgs) null);
          break;
        case 'T':
        case 't':
          this.bTouch_Click(sender, (EventArgs) null);
          break;
        case 'W':
        case 'w':
          this.button2_Click(sender, (EventArgs) null);
          break;
        case 'X':
        case 'x':
          this.bCancel_Click(sender, (EventArgs) null);
          break;
      }
    }

    private void bMonitors_KeyPress(object sender, KeyPressEventArgs e)
    {
      switch (e.KeyChar)
      {
        case '\r':
        case '6':
          this.bMonitors_Click(sender, (EventArgs) null);
          break;
        case '1':
          this.bOptions_Click(sender, (EventArgs) null);
          break;
        case '2':
          this.bSponsor_Click(sender, (EventArgs) null);
          break;
        case '3':
          this.bValidators_Click(sender, (EventArgs) null);
          break;
        case '4':
          this.bPassword_Click(sender, (EventArgs) null);
          break;
        case '5':
          this.bDepositaire_Click(sender, (EventArgs) null);
          break;
        case 'R':
        case 'r':
          this.bVNC_Click(sender, (EventArgs) null);
          break;
        case 'S':
        case 's':
          this.bOK_Click(sender, (EventArgs) null);
          break;
        case 'T':
        case 't':
          this.bTouch_Click(sender, (EventArgs) null);
          break;
        case 'W':
        case 'w':
          this.button2_Click(sender, (EventArgs) null);
          break;
        case 'X':
        case 'x':
          this.bCancel_Click(sender, (EventArgs) null);
          break;
      }
    }

    private void DLG_All_KeyPress(object sender, KeyPressEventArgs e)
    {
      switch (e.KeyChar)
      {
        case '1':
          this.bOptions_Click(sender, (EventArgs) null);
          break;
        case '2':
          this.bSponsor_Click(sender, (EventArgs) null);
          break;
        case '3':
          this.bValidators_Click(sender, (EventArgs) null);
          break;
        case '4':
          this.bPassword_Click(sender, (EventArgs) null);
          break;
        case '5':
          this.bDepositaire_Click(sender, (EventArgs) null);
          break;
        case '6':
          this.bMonitors_Click(sender, (EventArgs) null);
          break;
        case 'R':
        case 'r':
          this.bVNC_Click(sender, (EventArgs) null);
          break;
        case 'S':
        case 's':
          this.bOK_Click(sender, (EventArgs) null);
          break;
        case 'T':
        case 't':
          this.bTouch_Click(sender, (EventArgs) null);
          break;
        case 'W':
        case 'w':
          this.button2_Click(sender, (EventArgs) null);
          break;
        case 'X':
        case 'x':
          this.bCancel_Click(sender, (EventArgs) null);
          break;
      }
    }

    private void btnTICKET_Click(object sender, EventArgs e)
    {
      int num = (int) new DLG_Dispenser(ref this.opciones).ShowDialog();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.lInfo = new Label();
      this.btnTICKET = new Button();
      this.bWifi = new Button();
      this.bFreeze = new Button();
      this.bVNC2 = new Button();
      this.bVNC = new Button();
      this.btn_mail = new Button();
      this.button2 = new Button();
      this.bMonitors = new Button();
      this.bTouch = new Button();
      this.bRESET = new Button();
      this.button1 = new Button();
      this.bDepositaire = new Button();
      this.bKey = new Button();
      this.bMin = new Button();
      this.bExplorer = new Button();
      this.bPanel = new Button();
      this.bOptions = new Button();
      this.bPassword = new Button();
      this.bValidators = new Button();
      this.bSponsor = new Button();
      this.bCancel = new Button();
      this.bOK = new Button();
      this.SuspendLayout();
      this.lInfo.AutoSize = true;
      this.lInfo.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.lInfo.Location = new Point(9, 309);
      this.lInfo.Name = "lInfo";
      this.lInfo.Size = new Size(14, 20);
      this.lInfo.TabIndex = 17;
      this.lInfo.Text = "-";
      this.btnTICKET.Image = (Image) Resources.Ticket;
      this.btnTICKET.Location = new Point(207, 208);
      this.btnTICKET.Name = "btnTICKET";
      this.btnTICKET.Size = new Size(189, 49);
      this.btnTICKET.TabIndex = 21;
      this.btnTICKET.UseVisualStyleBackColor = true;
      this.btnTICKET.Click += new EventHandler(this.btnTICKET_Click);
      this.bWifi.Image = (Image) Resources.ico_wifi_connect;
      this.bWifi.Location = new Point(293, 334);
      this.bWifi.Name = "bWifi";
      this.bWifi.Size = new Size(48, 48);
      this.bWifi.TabIndex = 12;
      this.bWifi.UseVisualStyleBackColor = true;
      this.bWifi.Click += new EventHandler(this.bWifi_Click);
      this.bWifi.KeyPress += new KeyPressEventHandler(this.DLG_All_KeyPress);
      this.bFreeze.Image = (Image) Resources.ico_lock_open;
      this.bFreeze.Location = new Point(348, 280);
      this.bFreeze.Name = "bFreeze";
      this.bFreeze.Size = new Size(48, 48);
      this.bFreeze.TabIndex = 20;
      this.bFreeze.TabStop = false;
      this.bFreeze.UseVisualStyleBackColor = true;
      this.bFreeze.Click += new EventHandler(this.bFreeze_Click);
      this.bVNC2.Image = (Image) Resources.ico_monitors_del;
      this.bVNC2.Location = new Point(229, 394);
      this.bVNC2.Name = "bVNC2";
      this.bVNC2.Size = new Size(48, 48);
      this.bVNC2.TabIndex = 18;
      this.bVNC2.UseVisualStyleBackColor = true;
      this.bVNC2.Visible = false;
      this.bVNC2.Click += new EventHandler(this.button3_Click);
      this.bVNC2.KeyPress += new KeyPressEventHandler(this.DLG_All_KeyPress);
      this.bVNC.Image = (Image) Resources.ico_monitors;
      this.bVNC.Location = new Point(175, 394);
      this.bVNC.Name = "bVNC";
      this.bVNC.Size = new Size(48, 48);
      this.bVNC.TabIndex = 17;
      this.bVNC.Text = "R";
      this.bVNC.TextAlign = ContentAlignment.BottomRight;
      this.bVNC.UseVisualStyleBackColor = true;
      this.bVNC.Visible = false;
      this.bVNC.Click += new EventHandler(this.bVNC_Click);
      this.bVNC.KeyPress += new KeyPressEventHandler(this.DLG_All_KeyPress);
      this.btn_mail.Image = (Image) Resources.ico_at;
      this.btn_mail.Location = new Point(121, 394);
      this.btn_mail.Name = "btn_mail";
      this.btn_mail.Size = new Size(48, 48);
      this.btn_mail.TabIndex = 16;
      this.btn_mail.UseVisualStyleBackColor = true;
      this.btn_mail.Click += new EventHandler(this.btn_mail_Click);
      this.btn_mail.KeyPress += new KeyPressEventHandler(this.DLG_All_KeyPress);
      this.button2.Image = (Image) Resources.ico_modewin;
      this.button2.Location = new Point(67, 394);
      this.button2.Name = "button2";
      this.button2.Size = new Size(48, 48);
      this.button2.TabIndex = 15;
      this.button2.Text = "W";
      this.button2.TextAlign = ContentAlignment.BottomRight;
      this.button2.UseVisualStyleBackColor = true;
      this.button2.Click += new EventHandler(this.button2_Click);
      this.button2.KeyPress += new KeyPressEventHandler(this.DLG_All_KeyPress);
      this.bMonitors.Image = (Image) Resources.ico_monitors;
      this.bMonitors.ImageAlign = ContentAlignment.MiddleLeft;
      this.bMonitors.Location = new Point(13, 257);
      this.bMonitors.Name = "bMonitors";
      this.bMonitors.Size = new Size(383, 49);
      this.bMonitors.TabIndex = 6;
      this.bMonitors.UseVisualStyleBackColor = true;
      this.bMonitors.Click += new EventHandler(this.bMonitors_Click);
      this.bMonitors.KeyPress += new KeyPressEventHandler(this.bMonitors_KeyPress);
      this.bTouch.Image = (Image) Resources.ico_touch;
      this.bTouch.Location = new Point(347, 334);
      this.bTouch.Name = "bTouch";
      this.bTouch.Size = new Size(48, 48);
      this.bTouch.TabIndex = 13;
      this.bTouch.Text = "T";
      this.bTouch.TextAlign = ContentAlignment.BottomRight;
      this.bTouch.UseVisualStyleBackColor = true;
      this.bTouch.Click += new EventHandler(this.bTouch_Click);
      this.bTouch.KeyPress += new KeyPressEventHandler(this.DLG_All_KeyPress);
      this.bRESET.Image = (Image) Resources.ico_poff;
      this.bRESET.Location = new Point(13, 394);
      this.bRESET.Name = "bRESET";
      this.bRESET.Size = new Size(48, 48);
      this.bRESET.TabIndex = 14;
      this.bRESET.UseVisualStyleBackColor = true;
      this.bRESET.Click += new EventHandler(this.bRESET_Click);
      this.bRESET.KeyPress += new KeyPressEventHandler(this.DLG_All_KeyPress);
      this.button1.Image = (Image) Resources.ico_net;
      this.button1.Location = new Point(229, 334);
      this.button1.Name = "button1";
      this.button1.Size = new Size(48, 48);
      this.button1.TabIndex = 11;
      this.button1.UseVisualStyleBackColor = true;
      this.button1.Click += new EventHandler(this.button1_Click);
      this.button1.KeyPress += new KeyPressEventHandler(this.DLG_All_KeyPress);
      this.bDepositaire.Image = (Image) Resources.ico_note;
      this.bDepositaire.ImageAlign = ContentAlignment.MiddleLeft;
      this.bDepositaire.Location = new Point(13, 208);
      this.bDepositaire.Name = "bDepositaire";
      this.bDepositaire.Size = new Size(195, 49);
      this.bDepositaire.TabIndex = 5;
      this.bDepositaire.UseVisualStyleBackColor = true;
      this.bDepositaire.Click += new EventHandler(this.bDepositaire_Click);
      this.bDepositaire.KeyPress += new KeyPressEventHandler(this.bDepositaire_KeyPress);
      this.bKey.Image = (Image) Resources.ico_keyboard;
      this.bKey.Location = new Point(175, 334);
      this.bKey.Name = "bKey";
      this.bKey.Size = new Size(48, 48);
      this.bKey.TabIndex = 10;
      this.bKey.UseVisualStyleBackColor = true;
      this.bKey.Click += new EventHandler(this.bKey_Click);
      this.bKey.KeyPress += new KeyPressEventHandler(this.DLG_All_KeyPress);
      this.bMin.Image = (Image) Resources.ico_windows;
      this.bMin.Location = new Point(121, 334);
      this.bMin.Name = "bMin";
      this.bMin.Size = new Size(48, 48);
      this.bMin.TabIndex = 9;
      this.bMin.UseVisualStyleBackColor = true;
      this.bMin.Click += new EventHandler(this.bMin_Click);
      this.bMin.KeyPress += new KeyPressEventHandler(this.DLG_All_KeyPress);
      this.bExplorer.Image = (Image) Resources.ico_fexp;
      this.bExplorer.Location = new Point(67, 334);
      this.bExplorer.Name = "bExplorer";
      this.bExplorer.Size = new Size(48, 48);
      this.bExplorer.TabIndex = 8;
      this.bExplorer.UseVisualStyleBackColor = true;
      this.bExplorer.Click += new EventHandler(this.bExplorer_Click);
      this.bExplorer.KeyPress += new KeyPressEventHandler(this.DLG_All_KeyPress);
      this.bPanel.Image = (Image) Resources.ico_gear;
      this.bPanel.Location = new Point(13, 334);
      this.bPanel.Name = "bPanel";
      this.bPanel.Size = new Size(48, 48);
      this.bPanel.TabIndex = 7;
      this.bPanel.UseVisualStyleBackColor = true;
      this.bPanel.Click += new EventHandler(this.bPanel_Click);
      this.bPanel.KeyPress += new KeyPressEventHandler(this.DLG_All_KeyPress);
      this.bOptions.Image = (Image) Resources.ico_preferences;
      this.bOptions.ImageAlign = ContentAlignment.MiddleLeft;
      this.bOptions.Location = new Point(13, 12);
      this.bOptions.Name = "bOptions";
      this.bOptions.Size = new Size(383, 49);
      this.bOptions.TabIndex = 1;
      this.bOptions.UseVisualStyleBackColor = true;
      this.bOptions.Click += new EventHandler(this.bOptions_Click);
      this.bOptions.KeyPress += new KeyPressEventHandler(this.bOptions_KeyPress);
      this.bPassword.Image = (Image) Resources.ico_key;
      this.bPassword.ImageAlign = ContentAlignment.MiddleLeft;
      this.bPassword.Location = new Point(13, 159);
      this.bPassword.Name = "bPassword";
      this.bPassword.Size = new Size(383, 49);
      this.bPassword.TabIndex = 4;
      this.bPassword.UseVisualStyleBackColor = true;
      this.bPassword.Click += new EventHandler(this.bPassword_Click);
      this.bPassword.KeyPress += new KeyPressEventHandler(this.bPassword_KeyPress);
      this.bValidators.Image = (Image) Resources.ico_money;
      this.bValidators.ImageAlign = ContentAlignment.MiddleLeft;
      this.bValidators.Location = new Point(13, 110);
      this.bValidators.Name = "bValidators";
      this.bValidators.Size = new Size(383, 49);
      this.bValidators.TabIndex = 3;
      this.bValidators.UseVisualStyleBackColor = true;
      this.bValidators.Click += new EventHandler(this.bValidators_Click);
      this.bValidators.KeyPress += new KeyPressEventHandler(this.bValidators_KeyPress);
      this.bSponsor.Image = (Image) Resources.ico_at;
      this.bSponsor.ImageAlign = ContentAlignment.MiddleLeft;
      this.bSponsor.Location = new Point(13, 61);
      this.bSponsor.Name = "bSponsor";
      this.bSponsor.Size = new Size(383, 49);
      this.bSponsor.TabIndex = 2;
      this.bSponsor.UseVisualStyleBackColor = true;
      this.bSponsor.Click += new EventHandler(this.bSponsor_Click);
      this.bSponsor.KeyPress += new KeyPressEventHandler(this.bSponsor_KeyPress);
      this.bCancel.Image = (Image) Resources.ico_del;
      this.bCancel.Location = new Point(294, 394);
      this.bCancel.Name = "bCancel";
      this.bCancel.Size = new Size(48, 48);
      this.bCancel.TabIndex = 19;
      this.bCancel.Text = "X";
      this.bCancel.TextAlign = ContentAlignment.BottomRight;
      this.bCancel.UseVisualStyleBackColor = true;
      this.bCancel.Click += new EventHandler(this.bCancel_Click);
      this.bCancel.KeyPress += new KeyPressEventHandler(this.DLG_All_KeyPress);
      this.bOK.Image = (Image) Resources.ico_ok;
      this.bOK.Location = new Point(348, 394);
      this.bOK.Name = "bOK";
      this.bOK.Size = new Size(48, 48);
      this.bOK.TabIndex = 0;
      this.bOK.Text = "S";
      this.bOK.TextAlign = ContentAlignment.BottomRight;
      this.bOK.UseVisualStyleBackColor = true;
      this.bOK.Click += new EventHandler(this.bOK_Click);
      this.bOK.KeyPress += new KeyPressEventHandler(this.DLG_All_KeyPress);
      this.AutoScaleMode = AutoScaleMode.None;
      this.ClientSize = new Size(406, 457);
      this.ControlBox = false;
      this.Controls.Add((Control) this.btnTICKET);
      this.Controls.Add((Control) this.bWifi);
      this.Controls.Add((Control) this.bFreeze);
      this.Controls.Add((Control) this.bVNC2);
      this.Controls.Add((Control) this.bVNC);
      this.Controls.Add((Control) this.lInfo);
      this.Controls.Add((Control) this.btn_mail);
      this.Controls.Add((Control) this.button2);
      this.Controls.Add((Control) this.bMonitors);
      this.Controls.Add((Control) this.bTouch);
      this.Controls.Add((Control) this.bRESET);
      this.Controls.Add((Control) this.button1);
      this.Controls.Add((Control) this.bDepositaire);
      this.Controls.Add((Control) this.bKey);
      this.Controls.Add((Control) this.bMin);
      this.Controls.Add((Control) this.bExplorer);
      this.Controls.Add((Control) this.bPanel);
      this.Controls.Add((Control) this.bOptions);
      this.Controls.Add((Control) this.bPassword);
      this.Controls.Add((Control) this.bValidators);
      this.Controls.Add((Control) this.bSponsor);
      this.Controls.Add((Control) this.bCancel);
      this.Controls.Add((Control) this.bOK);
      this.FormBorderStyle = FormBorderStyle.FixedToolWindow;
      this.Name = nameof (DLG_Config);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "0";
      this.FormClosed += new FormClosedEventHandler(this.DLG_Config_FormClosed);
      this.Load += new EventHandler(this.DLG_Config_Load);
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}

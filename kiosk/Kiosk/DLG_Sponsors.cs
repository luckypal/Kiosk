// Decompiled with JetBrains decompiler
// Type: Kiosk.DLG_Sponsors
// Assembly: Kiosk, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: C3E32FFD-072D-4F9D-AAE4-A7F2B29E989A
// Assembly location: E:\kiosk\Kiosk.exe

using Kiosk.Properties;
using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Windows.Forms;

namespace Kiosk
{
  public class DLG_Sponsors : Form
  {
    private IContainer components = (IContainer) null;
    public bool OK;
    public Configuracion opciones;
    private Button bOk;
    private Button bCancel;
    private OpenFileDialog openDisk;
    private TextBox tLogin;
    private Label lLogin;
    private TextBox tPassword;
    private Label lPassword;
    private TextBox tWeb;
    private Label lWeb;
    private Button bDEF;

    public DLG_Sponsors(ref Configuracion _opc)
    {
      this.OK = false;
      this.opciones = _opc;
      this.InitializeComponent();
      this.tWeb.Text = _opc.Srv_Ip;
      this.tLogin.Text = _opc.Srv_User;
      this.tPassword.Text = _opc.Srv_User_P;
      this.Localize();
    }

    private void Localize()
    {
      this.SuspendLayout();
      this.Text = this.opciones.Localize.Text("Server");
      this.lWeb.Text = this.opciones.Localize.Text("Web server");
      this.lLogin.Text = this.opciones.Localize.Text("User login");
      this.lPassword.Text = this.opciones.Localize.Text("Password");
      this.ResumeLayout();
    }

    private void bOk_Click(object sender, EventArgs e)
    {
      Configuracion.Access_Log("User Kiosk changed");
      this.opciones.Srv_Ip = this.tWeb.Text;
      this.opciones.Srv_User = this.tLogin.Text;
      this.opciones.Srv_User_P = this.tPassword.Text;
      this.opciones.Save_Net();
      this.OK = true;
      this.Close();
    }

    private void bCancel_Click(object sender, EventArgs e)
    {
      this.Close();
    }

    private void bHDisk_Click(object sender, EventArgs e)
    {
      if (this.openDisk.ShowDialog() != DialogResult.OK)
        return;
      this.Config_Import_Disk(this.openDisk.FileName);
    }

    public bool Zip_Expand(string _file)
    {
      ZipStorer zipStorer;
      try
      {
        zipStorer = ZipStorer.Open(_file, FileAccess.Read);
      }
      catch (Exception ex)
      {
        return false;
      }
      foreach (ZipStorer.ZipFileEntry _zfe in zipStorer.ReadCentralDir())
      {
        string _filename = Path.Combine(Environment.CurrentDirectory + "\\data\\local\\", Path.GetFileName(_zfe.FilenameInZip));
        try
        {
          zipStorer.ExtractFile(_zfe, _filename);
        }
        catch (Exception ex)
        {
          return false;
        }
      }
      zipStorer.Close();
      return true;
    }

    public bool Config_Import_Disk(string _file)
    {
      string str = Environment.CurrentDirectory + "\\data\\local\\" + Path.GetFileName(_file);
      try
      {
        System.IO.File.Copy(_file, str, true);
      }
      catch (Exception ex)
      {
        return false;
      }
      return this.Zip_Expand(str);
    }

    public bool Config_Import_Web(string _web)
    {
      string str = Environment.CurrentDirectory + "\\data\\local\\" + Path.GetFileName(_web);
      string empty1 = string.Empty;
      string empty2 = string.Empty;
      try
      {
        new WebClient().DownloadFile(_web, str);
      }
      catch (Exception ex)
      {
        return false;
      }
      return this.Zip_Expand(str);
    }

    private void bHWeb_Click(object sender, EventArgs e)
    {
    }

    private void bDEF_Click(object sender, EventArgs e)
    {
      this.opciones.Servidor_Lux();
      this.tWeb.Text = this.opciones.Srv_Ip;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.openDisk = new OpenFileDialog();
      this.tLogin = new TextBox();
      this.lLogin = new Label();
      this.tPassword = new TextBox();
      this.lPassword = new Label();
      this.tWeb = new TextBox();
      this.lWeb = new Label();
      this.bCancel = new Button();
      this.bOk = new Button();
      this.bDEF = new Button();
      this.SuspendLayout();
      this.openDisk.FileName = "Source directory";
      this.openDisk.Filter = "Zip file|*.zip|All files|*.*";
      this.openDisk.Title = "Select ZIP file";
      this.tLogin.Location = new Point(15, 79);
      this.tLogin.Name = "tLogin";
      this.tLogin.Size = new Size(396, 20);
      this.tLogin.TabIndex = 1;
      this.lLogin.AutoSize = true;
      this.lLogin.Location = new Point(12, 58);
      this.lLogin.Name = "lLogin";
      this.lLogin.Size = new Size(33, 13);
      this.lLogin.TabIndex = 14;
      this.lLogin.Text = "Login";
      this.tPassword.Location = new Point(15, 128);
      this.tPassword.Name = "tPassword";
      this.tPassword.Size = new Size(396, 20);
      this.tPassword.TabIndex = 2;
      this.tPassword.UseSystemPasswordChar = true;
      this.lPassword.AutoSize = true;
      this.lPassword.Location = new Point(12, 107);
      this.lPassword.Name = "lPassword";
      this.lPassword.Size = new Size(53, 13);
      this.lPassword.TabIndex = 16;
      this.lPassword.Text = "Password";
      this.tWeb.Location = new Point(15, 30);
      this.tWeb.Name = "tWeb";
      this.tWeb.Size = new Size(326, 20);
      this.tWeb.TabIndex = 0;
      this.lWeb.AutoSize = true;
      this.lWeb.Location = new Point(12, 9);
      this.lWeb.Name = "lWeb";
      this.lWeb.Size = new Size(30, 13);
      this.lWeb.TabIndex = 18;
      this.lWeb.Text = "Web";
      this.bCancel.BackgroundImage = (Image) Resources.ico_del;
      this.bCancel.BackgroundImageLayout = ImageLayout.Center;
      this.bCancel.Location = new Point(277, 168);
      this.bCancel.Name = "bCancel";
      this.bCancel.Size = new Size(64, 48);
      this.bCancel.TabIndex = 4;
      this.bCancel.UseVisualStyleBackColor = true;
      this.bCancel.Click += new EventHandler(this.bCancel_Click);
      this.bOk.BackgroundImage = (Image) Resources.ico_ok;
      this.bOk.BackgroundImageLayout = ImageLayout.Center;
      this.bOk.Location = new Point(347, 168);
      this.bOk.Name = "bOk";
      this.bOk.Size = new Size(64, 48);
      this.bOk.TabIndex = 3;
      this.bOk.UseVisualStyleBackColor = true;
      this.bOk.Click += new EventHandler(this.bOk_Click);
      this.bDEF.BackgroundImage = (Image) Resources.ico_net;
      this.bDEF.BackgroundImageLayout = ImageLayout.Center;
      this.bDEF.Location = new Point(347, 15);
      this.bDEF.Name = "bDEF";
      this.bDEF.Size = new Size(64, 48);
      this.bDEF.TabIndex = 19;
      this.bDEF.UseVisualStyleBackColor = true;
      this.bDEF.Click += new EventHandler(this.bDEF_Click);
      this.AutoScaleMode = AutoScaleMode.None;
      this.ClientSize = new Size(427, 228);
      this.ControlBox = false;
      this.Controls.Add((Control) this.bDEF);
      this.Controls.Add((Control) this.lWeb);
      this.Controls.Add((Control) this.tWeb);
      this.Controls.Add((Control) this.lLogin);
      this.Controls.Add((Control) this.bCancel);
      this.Controls.Add((Control) this.tLogin);
      this.Controls.Add((Control) this.bOk);
      this.Controls.Add((Control) this.tPassword);
      this.Controls.Add((Control) this.lPassword);
      this.FormBorderStyle = FormBorderStyle.FixedToolWindow;
      this.Name = nameof (DLG_Sponsors);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Sponsors";
      this.TopMost = true;
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}

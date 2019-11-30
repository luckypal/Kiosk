// Decompiled with JetBrains decompiler
// Type: Kiosk.DLG_Wifi
// Assembly: Kiosk, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: C3E32FFD-072D-4F9D-AAE4-A7F2B29E989A
// Assembly location: E:\kiosk\Kiosk.exe

using Kiosk.Properties;
using SimpleWifi;
using SimpleWifi.Win32.Interop;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Windows.Forms;

namespace Kiosk
{
  public class DLG_Wifi : Form
  {
    private int lastindex = -1;
    private IContainer components = (IContainer) null;
    private bool RefreshNet;
    public bool OK;
    public bool IsClosed;
    public Configuracion opciones;
    private Wifi wifi;
    private Panel pBOTTOM;
    private Button bCancel;
    private Button bOk;
    private Button bFind;
    private ListBox listWifi;
    private Button bDisconnect;
    private Button bConnect;
    private Timer timerNet;

    public DLG_Wifi(ref Configuracion _opc)
    {
      this.IsClosed = false;
      this.OK = false;
      this.opciones = _opc;
      this.InitializeComponent();
      this.Localize();
      this.wifi = new Wifi();
      this.wifi.ConnectionStatusChanged += new EventHandler<WifiStatusEventArgs>(this.wifi_ConnectionStatusChanged);
      NetworkChange.NetworkAddressChanged += new NetworkAddressChangedEventHandler(this.AddressChangedCallback);
      this.RefreshNet = true;
    }

    public void AddressChangedCallback(object sender, EventArgs e)
    {
      this.RefreshNet = true;
    }

    private void Localize()
    {
      this.SuspendLayout();
      this.ResumeLayout();
    }

    private void wifi_ConnectionStatusChanged(object sender, WifiStatusEventArgs e)
    {
      this.RefreshNet = true;
    }

    private void Disconnect()
    {
      this.wifi.Disconnect();
    }

    private bool Status()
    {
      return this.wifi.ConnectionStatus == WifiStatus.Connected;
    }

    private bool Connect_Forced(string _info)
    {
      return false;
    }

    private bool Connect(string _info, bool _auto = true)
    {
      return this.Connect(_info.Split(',')[0], false, (string) null, (string) null, (string) null, (string) null, (string) null, _auto);
    }

    private bool Connect(string _info, string _sec = null, bool _auto = true)
    {
      return this.Connect(_info.Split(',')[0], false, (string) null, (string) null, (string) null, (string) null, _sec, _auto);
    }

    private bool Connect(string _info, bool _over, string _sec = null, bool _auto = true)
    {
      return this.Connect(_info.Split(',')[0], _over, (string) null, (string) null, (string) null, (string) null, _sec, _auto);
    }

    private bool Connect(string _info, string _password, string _sec = null, bool _auto = true)
    {
      return this.Connect(_info.Split(',')[0], true, (string) null, _password, (string) null, (string) null, _sec, _auto);
    }

    private bool Connect(string _info, string _password, string _rename, string _sec, bool _auto = true)
    {
      return this.Connect(_info.Split(',')[0], true, (string) null, _password, (string) null, _rename, _sec, _auto);
    }

    private bool Connect(
      string _name,
      bool _over,
      string _username,
      string _password,
      string _domain,
      string _rename,
      string _sec = null,
      bool _auto = true)
    {
      IEnumerable<AccessPoint> source = this.List();
      AccessPoint ap = (AccessPoint) null;
      int num = 1;
      for (int index = 0; index < source.ToArray<AccessPoint>().Length; ++index)
      {
        string str = "";
        if (!string.IsNullOrEmpty(source.ToList<AccessPoint>()[index].Name))
          str = source.ToList<AccessPoint>()[index].Name;
        else if (source.ToList<AccessPoint>()[index]._network.dot11BssType == Dot11BssType.Infrastructure && source.ToList<AccessPoint>()[index]._network.networkConnectable)
        {
          switch (source.ToList<AccessPoint>()[index]._network.dot11DefaultAuthAlgorithm)
          {
            case Dot11AuthAlgorithm.IEEE80211_Open:
              str = "<HIDEN OPEN> " + (object) num;
              ++num;
              break;
            case Dot11AuthAlgorithm.IEEE80211_SharedKey:
              str = "<HIDEN WEP> " + (object) num;
              ++num;
              break;
            case Dot11AuthAlgorithm.WPA:
              str = "<HIDEN WPA> " + (object) num;
              ++num;
              break;
            case Dot11AuthAlgorithm.WPA_PSK:
              str = "<HIDEN WPA PSK> " + (object) num;
              ++num;
              break;
            case Dot11AuthAlgorithm.RSNA:
              str = "<HIDEN RSNA> " + (object) num;
              ++num;
              break;
            case Dot11AuthAlgorithm.RSNA_PSK:
              str = "<HIDEN RSNA PSK> " + (object) num;
              ++num;
              break;
          }
        }
        if (str.ToLower() == _name.ToLower())
        {
          ap = source.ToList<AccessPoint>()[index];
          break;
        }
      }
      if (ap == null)
        return false;
      if (!string.IsNullOrEmpty(_rename))
      {
        ap._network.dot11Ssid.SSIDLength = (uint) _rename.Length;
        byte[] numArray = new byte[32];
        for (int index = 0; index < 32; ++index)
          numArray[index] = (byte) 0;
        byte[] bytes = Encoding.ASCII.GetBytes(_rename);
        for (int index = 0; index < bytes.Length; ++index)
          numArray[index] = bytes[index];
        ap._network.dot11Ssid.SSID = numArray;
        ap._network.profileName = _rename;
      }
      switch (_sec)
      {
        case "IEEE80211_Open":
          ap._network.dot11DefaultAuthAlgorithm = Dot11AuthAlgorithm.IEEE80211_Open;
          break;
        case "IEEE80211_SharedKey":
          ap._network.dot11DefaultAuthAlgorithm = Dot11AuthAlgorithm.IEEE80211_SharedKey;
          break;
        case "WPA":
          ap._network.dot11DefaultAuthAlgorithm = Dot11AuthAlgorithm.WPA;
          break;
        case "WPA_PSK":
          ap._network.dot11DefaultAuthAlgorithm = Dot11AuthAlgorithm.WPA_PSK;
          break;
        case "RSNA":
          ap._network.dot11DefaultAuthAlgorithm = Dot11AuthAlgorithm.RSNA;
          break;
        case "RSNA_PSK":
          ap._network.dot11DefaultAuthAlgorithm = Dot11AuthAlgorithm.RSNA_PSK;
          break;
      }
      AuthRequest request = new AuthRequest(ap);
      bool overwriteProfile = _over;
      if (request.IsPasswordRequired && overwriteProfile)
      {
        if (request.IsUsernameRequired && !string.IsNullOrEmpty(_username))
          request.Username = _username;
        request.Password = _password;
        if (request.IsDomainSupported && !string.IsNullOrEmpty(_domain))
          request.Domain = _domain;
      }
      request.AutoConnect = _auto;
      ap.ConnectAsync(request, overwriteProfile, new Action<bool>(this.OnConnectedComplete));
      return true;
    }

    private void OnConnectedComplete(bool success)
    {
      this.RefreshNet = true;
    }

    public IEnumerable<AccessPoint> List()
    {
      IEnumerable<AccessPoint> accessPoints = (IEnumerable<AccessPoint>) this.wifi.GetAccessPoints().OrderByDescending<AccessPoint, uint>((Func<AccessPoint, uint>) (ap => ap.SignalStrength));
      int num = 1;
      foreach (AccessPoint accessPoint in accessPoints)
      {
        if (!string.IsNullOrEmpty(accessPoint.Name))
          this.listWifi.Items.Add((object) (accessPoint.Name + "," + (object) accessPoint.SignalStrength + "," + (object) accessPoint.IsConnected + "," + accessPoint._network.dot11DefaultAuthAlgorithm.ToString()));
        else if (accessPoint._network.dot11BssType == Dot11BssType.Infrastructure && accessPoint._network.networkConnectable)
        {
          switch (accessPoint._network.dot11DefaultAuthAlgorithm)
          {
            case Dot11AuthAlgorithm.IEEE80211_Open:
              this.listWifi.Items.Add((object) ("<HIDEN OPEN> " + (object) num + "," + (object) accessPoint.SignalStrength + ",False," + accessPoint._network.dot11DefaultAuthAlgorithm.ToString()));
              ++num;
              break;
            case Dot11AuthAlgorithm.IEEE80211_SharedKey:
              this.listWifi.Items.Add((object) ("<HIDEN WEP> " + (object) num + "," + (object) accessPoint.SignalStrength + ",False," + accessPoint._network.dot11DefaultAuthAlgorithm.ToString()));
              ++num;
              break;
            case Dot11AuthAlgorithm.WPA:
              this.listWifi.Items.Add((object) ("<HIDEN WPA> " + (object) num + "," + (object) accessPoint.SignalStrength + ",False," + accessPoint._network.dot11DefaultAuthAlgorithm.ToString()));
              ++num;
              break;
            case Dot11AuthAlgorithm.WPA_PSK:
              this.listWifi.Items.Add((object) ("<HIDEN WPA PSK> " + (object) num + "," + (object) accessPoint.SignalStrength + ",False," + accessPoint._network.dot11DefaultAuthAlgorithm.ToString()));
              ++num;
              break;
            case Dot11AuthAlgorithm.RSNA:
              this.listWifi.Items.Add((object) ("<HIDEN RSNA> " + (object) num + "," + (object) accessPoint.SignalStrength + ",False," + accessPoint._network.dot11DefaultAuthAlgorithm.ToString()));
              ++num;
              break;
            case Dot11AuthAlgorithm.RSNA_PSK:
              this.listWifi.Items.Add((object) ("<HIDEN RSNA PSK> " + (object) num + "," + (object) accessPoint.SignalStrength + ",False," + accessPoint._network.dot11DefaultAuthAlgorithm.ToString()));
              ++num;
              break;
          }
        }
      }
      this.listWifi.Invalidate();
      return accessPoints;
    }

    private bool DeleteProfile(string _name)
    {
      IEnumerable<AccessPoint> source = this.List();
      for (int index = 0; index < source.ToArray<AccessPoint>().Length; ++index)
      {
        if (source.ToList<AccessPoint>()[index].Name.ToLower() == _name.ToLower())
        {
          source.ToList<AccessPoint>()[index].DeleteProfile();
          return true;
        }
      }
      return false;
    }

    private AccessPoint Info(string _name)
    {
      IEnumerable<AccessPoint> source = this.List();
      for (int index = 0; index < source.ToArray<AccessPoint>().Length; ++index)
      {
        if (source.ToList<AccessPoint>()[index].Name.ToLower() == _name.ToLower())
          return source.ToList<AccessPoint>()[index];
      }
      return (AccessPoint) null;
    }

    private void bFind_Click(object sender, EventArgs e)
    {
      this.RefreshNet = true;
    }

    private void bOk_Click(object sender, EventArgs e)
    {
      this.OK = true;
      this.Close();
    }

    private void bCancel_Click(object sender, EventArgs e)
    {
      this.OK = false;
      this.Close();
    }

    private void DLG_Wifi_FormClosed(object sender, FormClosedEventArgs e)
    {
      this.IsClosed = true;
    }

    private void listWifi_DrawItem(object sender, DrawItemEventArgs e)
    {
      e.Graphics.FillRectangle(Brushes.Black, e.Bounds);
      Rectangle bounds = e.Bounds;
      ++bounds.X;
      ++bounds.Y;
      bounds.Width -= 2;
      bounds.Height -= 2;
      Brush brush1 = Brushes.White;
      string[] strArray;
      if (e.Index >= 0)
        strArray = this.listWifi.Items[e.Index].ToString().Split(',');
      else
        strArray = new string[3]{ "NO WIFI", "0", "false" };
      if (strArray[2].ToLower() == "true".ToLower())
        brush1 = Brushes.Green;
      e.Graphics.FillRectangle(brush1, bounds);
      if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
        e.Graphics.FillRectangle(Brushes.Blue, bounds);
      Rectangle rect1 = bounds;
      rect1.Y = bounds.Y + bounds.Height - 24;
      rect1.Height = 24;
      e.Graphics.FillRectangle(Brushes.Red, rect1);
      int num1 = 0;
      try
      {
        num1 = int.Parse(strArray[1]);
      }
      catch
      {
      }
      Rectangle rect2 = bounds;
      rect2.Y = bounds.Y + bounds.Height - 24;
      rect2.Height = 24;
      int num2 = rect2.Width * num1 / 100;
      rect2.Width = num2;
      Brush brush2 = Brushes.OrangeRed;
      if (num1 > 50)
        brush2 = Brushes.Yellow;
      if (num1 > 75)
        brush2 = Brushes.YellowGreen;
      e.Graphics.FillRectangle(brush2, rect2);
      e.Graphics.MeasureString(strArray[1] + "%", e.Font);
      rect2.Width = bounds.Width;
      rect2.X += 32;
      e.Graphics.DrawString(strArray[1] + "%", e.Font, Brushes.Black, (RectangleF) rect2, StringFormat.GenericDefault);
      rect2 = bounds;
      rect2.Y += 8;
      rect2.X += 32;
      string s = strArray[0];
      if (strArray[2].ToLower() == "true".ToLower())
        s += " -> CONNECT";
      e.Graphics.DrawString(s, e.Font, Brushes.Black, (RectangleF) rect2, StringFormat.GenericDefault);
    }

    private void DLG_Wifi_Load(object sender, EventArgs e)
    {
      this.timerNet.Enabled = true;
      this.RefreshNet = true;
    }

    private void bConnect_Click(object sender, EventArgs e)
    {
      if (this.listWifi.SelectedIndex == -1)
      {
        MSG_Ok msgOk = new MSG_Ok(ref this.opciones, "Select any Wifi");
        int num = (int) msgOk.ShowDialog();
        msgOk.Dispose();
        this.RefreshNet = true;
      }
      else
      {
        string[] strArray = this.listWifi.Items[this.listWifi.SelectedIndex].ToString().Split(',');
        if (strArray.Length < 1)
        {
          this.RefreshNet = true;
        }
        else
        {
          DLG_Wifi_Connect dlgWifiConnect = new DLG_Wifi_Connect(ref this.opciones, strArray[0], "?", strArray[3]);
          int num = (int) dlgWifiConnect.ShowDialog();
          bool newProfile = dlgWifiConnect.NewProfile;
          bool alwaysConnect = dlgWifiConnect.AlwaysConnect;
          if (!dlgWifiConnect.OK)
          {
            this.RefreshNet = true;
          }
          else
          {
            this.Disconnect();
            if (string.IsNullOrEmpty(dlgWifiConnect.Password) || dlgWifiConnect.Password == "?")
              this.Connect(dlgWifiConnect.SID, alwaysConnect);
            else if (dlgWifiConnect.SID != strArray[0])
              this.Connect(strArray[0] + ",100,False", dlgWifiConnect.Password, dlgWifiConnect.SID, dlgWifiConnect.SEC, alwaysConnect);
            else
              this.Connect(strArray[0] + ",100,False", dlgWifiConnect.Password, alwaysConnect);
            this.RefreshNet = true;
          }
        }
      }
    }

    private void listWifi_DoubleClick(object sender, EventArgs e)
    {
      this.Disconnect();
      this.Connect(this.listWifi.Items[this.listWifi.SelectedIndex].ToString(), true);
      this.RefreshNet = true;
    }

    private void bDisconnect_Click(object sender, EventArgs e)
    {
      this.Disconnect();
      this.RefreshNet = true;
    }

    private void timerNet_Tick(object sender, EventArgs e)
    {
      if (!this.RefreshNet)
        return;
      this.listWifi.Items.Clear();
      this.List();
      this.RefreshNet = false;
    }

    private void DLG_Wifi_FormClosing(object sender, FormClosingEventArgs e)
    {
      this.RefreshNet = false;
      this.timerNet.Enabled = false;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.components = (IContainer) new Container();
      this.pBOTTOM = new Panel();
      this.bDisconnect = new Button();
      this.bConnect = new Button();
      this.bFind = new Button();
      this.bCancel = new Button();
      this.bOk = new Button();
      this.listWifi = new ListBox();
      this.timerNet = new Timer(this.components);
      this.pBOTTOM.SuspendLayout();
      this.SuspendLayout();
      this.pBOTTOM.Controls.Add((Control) this.bDisconnect);
      this.pBOTTOM.Controls.Add((Control) this.bConnect);
      this.pBOTTOM.Controls.Add((Control) this.bFind);
      this.pBOTTOM.Controls.Add((Control) this.bCancel);
      this.pBOTTOM.Controls.Add((Control) this.bOk);
      this.pBOTTOM.Dock = DockStyle.Bottom;
      this.pBOTTOM.Location = new Point(0, 379);
      this.pBOTTOM.Name = "pBOTTOM";
      this.pBOTTOM.Size = new Size(807, 48);
      this.pBOTTOM.TabIndex = 5;
      this.bDisconnect.Dock = DockStyle.Left;
      this.bDisconnect.Image = (Image) Resources.ico_wifi_disconnect;
      this.bDisconnect.Location = new Point(96, 0);
      this.bDisconnect.Name = "bDisconnect";
      this.bDisconnect.Size = new Size(48, 48);
      this.bDisconnect.TabIndex = 4;
      this.bDisconnect.UseVisualStyleBackColor = true;
      this.bDisconnect.Click += new EventHandler(this.bDisconnect_Click);
      this.bConnect.Dock = DockStyle.Left;
      this.bConnect.Image = (Image) Resources.ico_wifi_connect;
      this.bConnect.Location = new Point(48, 0);
      this.bConnect.Name = "bConnect";
      this.bConnect.Size = new Size(48, 48);
      this.bConnect.TabIndex = 3;
      this.bConnect.UseVisualStyleBackColor = true;
      this.bConnect.Click += new EventHandler(this.bConnect_Click);
      this.bFind.Dock = DockStyle.Left;
      this.bFind.Image = (Image) Resources.ico_wifi_find;
      this.bFind.Location = new Point(0, 0);
      this.bFind.Name = "bFind";
      this.bFind.Size = new Size(48, 48);
      this.bFind.TabIndex = 2;
      this.bFind.UseVisualStyleBackColor = true;
      this.bFind.Click += new EventHandler(this.bFind_Click);
      this.bCancel.Dock = DockStyle.Right;
      this.bCancel.Image = (Image) Resources.ico_del;
      this.bCancel.Location = new Point(711, 0);
      this.bCancel.Name = "bCancel";
      this.bCancel.Size = new Size(48, 48);
      this.bCancel.TabIndex = 1;
      this.bCancel.UseVisualStyleBackColor = true;
      this.bCancel.Visible = false;
      this.bCancel.Click += new EventHandler(this.bCancel_Click);
      this.bOk.Dock = DockStyle.Right;
      this.bOk.Image = (Image) Resources.ico_ok;
      this.bOk.Location = new Point(759, 0);
      this.bOk.Name = "bOk";
      this.bOk.Size = new Size(48, 48);
      this.bOk.TabIndex = 0;
      this.bOk.UseVisualStyleBackColor = true;
      this.bOk.Click += new EventHandler(this.bOk_Click);
      this.listWifi.Dock = DockStyle.Fill;
      this.listWifi.DrawMode = DrawMode.OwnerDrawFixed;
      this.listWifi.Font = new Font("Microsoft Sans Serif", 16f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.listWifi.FormattingEnabled = true;
      this.listWifi.ItemHeight = 64;
      this.listWifi.Location = new Point(0, 0);
      this.listWifi.Name = "listWifi";
      this.listWifi.Size = new Size(807, 379);
      this.listWifi.TabIndex = 6;
      this.listWifi.DrawItem += new DrawItemEventHandler(this.listWifi_DrawItem);
      this.listWifi.DoubleClick += new EventHandler(this.listWifi_DoubleClick);
      this.timerNet.Tick += new EventHandler(this.timerNet_Tick);
      this.AutoScaleMode = AutoScaleMode.None;
      this.ClientSize = new Size(807, 427);
      this.ControlBox = false;
      this.Controls.Add((Control) this.listWifi);
      this.Controls.Add((Control) this.pBOTTOM);
      this.FormBorderStyle = FormBorderStyle.FixedToolWindow;
      this.Name = nameof (DLG_Wifi);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = " ";
      this.FormClosing += new FormClosingEventHandler(this.DLG_Wifi_FormClosing);
      this.FormClosed += new FormClosedEventHandler(this.DLG_Wifi_FormClosed);
      this.Load += new EventHandler(this.DLG_Wifi_Load);
      this.pBOTTOM.ResumeLayout(false);
      this.ResumeLayout(false);
    }
  }
}

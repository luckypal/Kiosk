// Decompiled with JetBrains decompiler
// Type: Kiosk.Configuracion
// Assembly: Kiosk, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: C3E32FFD-072D-4F9D-AAE4-A7F2B29E989A
// Assembly location: E:\kiosk\Kiosk.exe

using GLib;
using GLib.Config;
using Kiosk.SerQuiosc2;
using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.Drawing.Printing;
using System.Globalization;
using System.IO;
using System.Management;
using System.Net;
using System.Net.NetworkInformation;
using System.Security;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;

namespace Kiosk
{
  public class Configuracion : XmlConfig
  {
    public bool GettonWait = false;
    public int modo_XP = 0;
    public int parche = 0;
    public int NoCreditsInGame = 0;
    public string Srv_Rtm = "Service";
    public string Srv_port = "1938";
    public string Srv_User = "-";
    public string Srv_User_P = "-";
    public int __ModoTablet = 0;
    private int Canales = 16;
    public int Reset = 0;
    public string SSP_Com = "?";
    public string SSP3_Com = "?";
    public string SIO_Com = "?";
    public string Tri_Com = "?";
    public string F40_Com = "?";
    public string RM5_Com = "?";
    public bool ForceSpy = false;
    public string VersionPRG;
    public string debugtxt;
    public int Verificar_Ticket;
    public bool ForceGoConfig;
    public Configuracion.Ticket_Resposta Ticket_Pago;
    public Configuracion.Ticket_Resposta Ticket_Verificar;
    public int News;
    public int Add_Getton;
    public bool GettonOff;
    public DateTime GettonOffLast;
    public int ForceRefresh;
    public int Ticket_Carburante;
    public string IDMAQUINA;
    public ulong dIDMAQUINA;
    public string Publi;
    public int Monitors;
    public string CPUID;
    public bool Emu_Mouse;
    public bool Emu_Mouse_RClick;
    public int JoinTicket;
    public int CfgVer;
    public string LastVersion;
    public int MissatgeCreditsGratuits;
    public string MissatgePrinter;
    public string Srv_Web_Ip;
    public string Srv_Web_Page;
    public string Server_VNC;
    public bool ForceReset;
    public bool ForceManteniment;
    public string ForceResetMask;
    public string ForceMantenimentMask;
    public int TicketHidePay;
    public int BillDisableServer;
    public int AutoTicketTime;
    public int Error_Billetero;
    public int Spy;
    public string UserSpy;
    public int Dev_Bank;
    public int ForceAllKey;
    public int ticketCleanTemps;
    public int EnManteniment;
    public int FreeGames;
    public int ModoKiosk;
    public int CursorOn;
    public string PasswordADM;
    public string Dev_Coin;
    public string Dev_Coin_P;
    public string Dev_BNV;
    public string Dev_BNV_P;
    public string[] Web_Zone;
    public bool TimeNavigate;
    public int ForceLogin;
    public int ResetTemps;
    public int ValorTemps;
    public string Home;
    public int EmuKeyboard;
    public string Srv_Ip;
    public string Srv_ID_Lin1;
    public string Srv_ID_Lin2;
    public string Srv_ID_Lin3;
    public string Srv_ID_Lin4;
    public string Srv_ID_Lin5;
    public string Srv_ID_LinBottom;
    public string Srv_ID_Tlf;
    public string Impresora_Tck;
    public string Barcode;
    public int ModoTickets;
    public int ModoPS2;
    public bool StopCredits;
    public int TimeoutCredits;
    public int BrowserBarOn;
    public string RemoteCmd;
    public string RemoteParam;
    public Decimal CreditsGratuits;
    public int ModoPlayCreditsGratuits;
    public int Pagar_Ticket_Val;
    public int Pagar_Ticket_Busy;
    public int Pagar_Ticket_ID;
    public int CancelTempsOn;
    public int Ticket_N_FEED;
    public int Ticket_N_HEAD;
    public int Ticket_Cut;
    public int Ticket_Model;
    public int Ticket_60mm;
    public long dMAC;
    public bool PrintTicket;
    public bool RunConfig;
    public DateTime LastMouseMove;
    public int ComprarTemps;
    public bool ModeAchat;
    public bool Running;
    public int FullScreen;
    public Decimal SaldoCredits;
    public Decimal Credits;
    public Decimal Sub_Credits;
    public Decimal Add_Credits;
    public Decimal Send_Add_Credits;
    public Decimal Send_Sub_Credits;
    public bool Logged;
    public bool InGame;
    public bool U_InGame;
    public int Temps;
    public int CancelTemps;
    public int TempsDeTicket;
    public int uCredits;
    public int uTemps;
    public bool Show_Browser;
    public int Enable_Lectors;
    public int LockCredits;
    public string Last_Device;
    public int Test_Barcode;
    public int TicketTemps;
    public int IdTicketTemps;
    public string MAC;
    public string MACLAN;
    public string MACWIFI;
    public string Srv_Room;
    public LogFile _logLogin;
    public LogFile _logCredits;
    public LogFile _logServer;
    public int BotoTicket;
    public int[] SSP_Value;
    public int[] SSP_Inhibit;
    public int[] SSP_Enabled;
    public int[] SSP3_Value;
    public int[] SSP3_Inhibit;
    public int[] SSP3_Enabled;
    public int[] SIO_Value;
    public int[] SIO_Inhibit;
    public int[] SIO_Enabled;
    public int[] Tri_Value;
    public int[] Tri_Inhibit;
    public int[] Tri_Enabled;
    public int[] F40_Value;
    public int[] F40_Inhibit;
    public int[] F40_Enabled;
    public int[] RM5_Value;
    public int[] RM5_Inhibit;
    public int[] RM5_Enabled;
    public int Disp_Enable;
    public int Disp_Val;
    public int Disp_Min;
    public int Disp_Max;
    public int Disp_Out;
    public int Disp_Ticket;
    public string Disp_Port;
    public string Disp_Model;
    public int Disp_Recovery;
    public string Disp_Recovery_Pass;
    public int Disp_Pay_Ticket;
    public int Disp_Pay_Ticket_Credits;
    public int Disp_Pay_Ticket_Out;
    public int Disp_Pay_Ticket_Fail;
    public int Disp_Pay_Ticket_Cnt_Fail;
    public int Disp_Pay_Ticket_Out_Flag;
    public int Disp_Pay_Running;
    public new string error;

    public Configuracion()
    {
      this.__ModoTablet = 0;
      this.Add_Getton = 0;
      this.CfgFile = this.AppName + ".options";
      this.Ticket_Pago = new Configuracion.Ticket_Resposta();
      this.Ticket_Verificar = new Configuracion.Ticket_Resposta();
      this.VersionPRG = "2.261";
      this.News = 0;
      this.MAC = this.GetMACAddress();
      string s1 = "0" + this.GetMACAddress();
      this.dMAC = 0L;
      bool flag;
      try
      {
        this.dMAC = long.Parse(s1, NumberStyles.AllowHexSpecifier);
      }
      catch (Exception ex)
      {
        flag = true;
        string message = ex.Message;
      }
      this.MACLAN = string.Format("0{0}", (object) this.GetMACAddress_LAN());
      this.MACWIFI = string.Format("0{0}", (object) this.GetMACAddress_WIFI());
      if (this.IDMAQUINA == "00")
        this.IDMAQUINA = "0";
      if (this.MAC == "00")
        this.MAC = "0";
      if (this.MACLAN == "00")
        this.MACLAN = "0";
      if (string.IsNullOrEmpty(this.MACLAN))
        this.MACLAN = "0";
      if (this.MACWIFI == "00")
        this.MACWIFI = "0";
      if (string.IsNullOrEmpty(this.MACWIFI))
        this.MACWIFI = "0";
      OperatingSystem osVersion = Environment.OSVersion;
      this.modo_XP = 0;
      if (osVersion.Version.Major > 5)
      {
        this.modo_XP = 0;
        this.CPUID = this.GetMACAddress();
      }
      else
      {
        this.modo_XP = 1;
        this.CPUID = "0";
      }
      this.IDMAQUINA = this.CPUID;
      if (this.MACLAN != "0")
        this.IDMAQUINA = this.MACLAN;
      string s2 = "0" + this.IDMAQUINA;
      this.dIDMAQUINA = 0UL;
      Exception exception;
      try
      {
        this.dIDMAQUINA = ulong.Parse(s2, NumberStyles.AllowHexSpecifier);
      }
      catch (Exception ex)
      {
        exception = ex;
      }
      this.MissatgePrinter = "";
      this.Spy = 0;
      this.GettonOff = true;
      this.UserSpy = "?";
      this.Emu_Mouse = false;
      this.Emu_Mouse_RClick = false;
      this.Dev_Bank = 0;
      this.debugtxt = "";
      this.FreeGames = 1;
      this.EnManteniment = 0;
      this.ModoKiosk = 1;
      this.loc = "frLU";
      this.unique = true;
      this.ModoPS2 = 0;
      this.ticketCleanTemps = 0;
      this.SaldoCredits = new Decimal(0);
      this.RunConfig = false;
      this.Running = false;
      this.FullScreen = 0;
      this.ModoPlayCreditsGratuits = 0;
      this.CursorOn = 1;
      this.ForceAllKey = 0;
      this.ValorTemps = 25;
      this.BotoTicket = 1;
      this.MissatgeCreditsGratuits = 0;
      this.ForceRefresh = 0;
      this.Disp_Enable = 0;
      this.Disp_Min = -1;
      this.Disp_Max = -10;
      this.Disp_Out = 0;
      this.Disp_Val = 500;
      this.Disp_Ticket = 1;
      this.Disp_Port = "?";
      this.Disp_Model = "ICT";
      this.Disp_Recovery = 1;
      this.Disp_Recovery_Pass = "5432";
      this.ForceGoConfig = false;
      this.CfgVer = 2;
      flag = true;
      this.AutoTicketTime = 0;
      this.ComprarTemps = 0;
      this.BrowserBarOn = 0;
      this.Verificar_Ticket = 0;
      this.TicketHidePay = 0;
      this.BillDisableServer = 0;
      this.Impresora_Tck = "";
      this.Server_VNC = "control.game-host.org";
      this.Web_Zone = new string[256];
      flag = false;
      this.Web_Zone[0] = "a.b.c";
      this.Web_Zone[1] = "tel.tbtlp.com";
      this.Web_Zone[2] = "www.jogarvip.com";
      this.Web_Zone[3] = "www.nalvip.com";
      this.Web_Zone[4] = "hw.html5slot.com";
      this.Web_Zone[5] = "tel.html5slot.com";
      this.Web_Zone[6] = "menu.html5slot.com";
      this.Web_Zone[7] = "hw.netkgame.com";
      this.Web_Zone[8] = "tel.netkgame.com";
      this.Web_Zone[9] = "menu.netkgame.com";
      this.Disp_Enable = 0;
      this.Disp_Val = 500;
      this.Disp_Min = -1;
      this.Disp_Max = -10;
      this.Disp_Out = 0;
      this.Disp_Ticket = 1;
      this.Disp_Port = "?";
      this.Disp_Model = "ICT";
      this.Disp_Recovery = 1;
      this.Disp_Recovery_Pass = "5432";
      this.Publi = "";
      this.ForceReset = false;
      this.ForceManteniment = false;
      this.ForceResetMask = "";
      this.ForceMantenimentMask = "";
      this.Monitors = 1;
      this.LastVersion = "0";
      this.Srv_Rtm = "Service";
      flag = false;
      this.VersionPRG += "t";
      this.Srv_Ip = "tel.tbtlp.com";
      this.Srv_Web_Ip = "tel.tbtlp.com";
      flag = true;
      flag = true;
      flag = true;
      this.Srv_Web_Page = "tickquiosc.aspx";
      flag = true;
      this.ModoTickets = 1;
      flag = true;
      this.LockCredits = 0;
      this.Srv_ID_Lin1 = " ";
      this.Srv_ID_Lin2 = " ";
      this.Srv_ID_Lin3 = " ";
      this.Srv_ID_Lin4 = " ";
      this.Srv_ID_Lin5 = " ";
      this.Srv_ID_LinBottom = "Changer pour un cadeau";
      this.Srv_ID_Tlf = " ";
      this.Home = "http://www.google.com";
      this.Srv_User = "-";
      this.Srv_User_P = "-";
      this.Srv_Room = "";
      this.PasswordADM = XmlConfig.X2Y("admin1234");
      this.Show_Browser = false;
      this.Enable_Lectors = 0;
      this.Error_Billetero = 0;
      this.Add_Credits = new Decimal(0);
      this.Sub_Credits = new Decimal(0);
      this.Send_Add_Credits = new Decimal(0);
      this.CreditsGratuits = new Decimal(0);
      this.ForceLogin = 0;
      this.Test_Barcode = 0;
      this.RemoteCmd = "";
      this.RemoteParam = "";
      this.Barcode = "NO_DEVICE";
      this.Logged = false;
      this.TimeoutCredits = 120;
      this.ResetTemps = 600;
      this.TicketTemps = 0;
      this.IdTicketTemps = 0;
      this.U_InGame = false;
      this.InGame = false;
      this.LastMouseMove = DateTime.Now;
      this.Ticket_Carburante = 0;
      this.Pagar_Ticket_Val = 0;
      this.Pagar_Ticket_Busy = 0;
      this.Pagar_Ticket_ID = 0;
      this.CancelTempsOn = 0;
      this.JoinTicket = 1;
      this.NoCreditsInGame = 0;
      this.Ticket_N_FEED = 3;
      this.Ticket_N_HEAD = 0;
      this.Ticket_Cut = 1;
      this.Ticket_Model = 0;
      this.Dev_Coin = "?";
      this.Dev_BNV = "?";
      this.SSP_Com = "?";
      this.SSP_Value = new int[this.Canales];
      this.SSP_Inhibit = new int[this.Canales];
      this.SSP_Enabled = new int[this.Canales];
      this.SSP3_Com = "?";
      this.SSP3_Value = new int[this.Canales];
      this.SSP3_Inhibit = new int[this.Canales];
      this.SSP3_Enabled = new int[this.Canales];
      this.SIO_Com = "?";
      this.SIO_Value = new int[this.Canales];
      this.SIO_Inhibit = new int[this.Canales];
      this.SIO_Enabled = new int[this.Canales];
      this.Tri_Com = "?";
      this.Tri_Value = new int[this.Canales];
      this.Tri_Inhibit = new int[this.Canales];
      this.Tri_Enabled = new int[this.Canales];
      this.F40_Com = "?";
      this.F40_Value = new int[this.Canales];
      this.F40_Inhibit = new int[this.Canales];
      this.F40_Enabled = new int[this.Canales];
      this.RM5_Com = "?";
      this.RM5_Value = new int[this.Canales];
      this.RM5_Inhibit = new int[this.Canales];
      this.RM5_Enabled = new int[this.Canales];
      this.DataPath = "data\\";
      this.AppName = "kiosk";
      this.CfgFile = this.AppName + ".options";
      this.CfgFileFull = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\" + this.CfgFile;
      this.Ticket_60mm = 0;
      this.StopCredits = false;
      if (this.modo_XP == 0)
      {
        try
        {
          PrintDocument printDocument = new PrintDocument();
          foreach (string installedPrinter in PrinterSettings.InstalledPrinters)
          {
            if (installedPrinter.ToLower() == "NII ExD NP-3511".ToLower())
            {
              this.Impresora_Tck = installedPrinter;
              this.Ticket_N_FEED = 3;
              this.Ticket_N_HEAD = 0;
              this.Ticket_Cut = 1;
              this.Ticket_Model = 0;
              break;
            }
            if (installedPrinter.ToLower() == "NII ExD NP-3411".ToLower())
            {
              this.Impresora_Tck = installedPrinter;
              this.Ticket_N_FEED = 3;
              this.Ticket_N_HEAD = 0;
              this.Ticket_Cut = 1;
              this.Ticket_Model = 0;
              break;
            }
            if (installedPrinter.ToLower() == "NII ExD NP-211".ToLower())
            {
              this.Impresora_Tck = installedPrinter;
              this.Ticket_N_FEED = 3;
              this.Ticket_N_HEAD = 0;
              this.Ticket_Cut = 1;
              this.Ticket_Model = 0;
              break;
            }
            if (installedPrinter.ToLower() == "CUSTOM TG2480-H".ToLower())
            {
              this.Impresora_Tck = installedPrinter;
              this.Ticket_N_FEED = 3;
              this.Ticket_N_HEAD = 3;
              this.Ticket_Cut = 1;
              this.Ticket_Model = 1;
              break;
            }
            if (installedPrinter.ToLower() == "CUSTOM Engineering TG2460".ToLower())
            {
              this.Impresora_Tck = installedPrinter;
              this.Ticket_Cut = 1;
              this.Ticket_Model = 1;
              this.Ticket_60mm = 1;
              this.Ticket_N_FEED = 12;
              this.Ticket_N_HEAD = 3;
              break;
            }
            if (installedPrinter.ToLower() == "Star TSP100 Cutter (TSP143)".ToLower())
            {
              this.Impresora_Tck = installedPrinter;
              this.Ticket_N_FEED = 3;
              this.Ticket_N_HEAD = 0;
              this.Ticket_Cut = 0;
              this.Ticket_Model = 1;
              break;
            }
          }
        }
        catch (Exception ex)
        {
          exception = ex;
        }
      }
      this.PrintTicket = false;
      this.ModeAchat = true;
      this.Log_Debug("Startup");
    }

    public bool PingTest(string _http)
    {
      Ping ping = new Ping();
      PingReply pingReply;
      try
      {
        pingReply = ping.Send(IPAddress.Parse(_http));
      }
      catch
      {
        return false;
      }
      return pingReply.Status == IPStatus.Success;
    }

    public bool Start_Service(string _web)
    {
      string fileName = Path.GetTempPath() + "tmp121212.tmp";
      string empty1 = string.Empty;
      string empty2 = string.Empty;
      try
      {
        new WebClient().DownloadFile(_web, fileName);
      }
      catch (Exception ex)
      {
        return false;
      }
      return true;
    }

    public string Get_Domain(string _http)
    {
      string[] strArray = _http.Split('.');
      int length = strArray.Length;
      if (length > 1)
        return strArray[length - 2] + "." + strArray[length - 1];
      return strArray[0] + ".com";
    }

    public void Save_Net()
    {
      this.Save_Net(string.Format("{0}", (object) this.MACLAN), "cfg");
    }

    public void Save_Net(string _mac, string _pref)
    {
      if (this.Srv_User == "-")
        return;
      this.Save();
      bool flag = true;
      UTF8Encoding utF8Encoding = new UTF8Encoding();
      string str = "http://wsquioscv2." + this.Get_Domain(this.Srv_Ip) + "/ServiceQuiosc.asmx";
      flag = false;
      string _url = "http://wsquioscv3." + this.Get_Domain(this.Srv_Ip) + "/ServiceQuiosc.asmx";
      string s = _mac;
      ulong num1 = 0;
      try
      {
        num1 = ulong.Parse(s, NumberStyles.AllowHexSpecifier);
      }
      catch
      {
      }
      if (num1 == 0UL)
        this.error = "MAC 0";
      int num2 = 0;
      for (int index = 0; index < 10; ++index)
      {
        Service1 service1 = new Service1(_url);
        try
        {
          service1.SaveFile(_pref + (object) num1, this.CfgFile, string.IsNullOrEmpty(this.Srv_User) ? "_nouser" : this.Srv_User, utF8Encoding.GetBytes(this.XML_File));
          num2 = 1;
        }
        catch (Exception ex)
        {
          flag = true;
          this.error = ex.Message;
        }
        service1.Dispose();
        if (num2 != 1)
          Thread.Sleep(500);
        else
          break;
      }
      if (num2 != 0)
        return;
      flag = true;
    }

    public bool PingServer()
    {
      Ping ping = new Ping();
      PingReply pingReply;
      try
      {
        pingReply = ping.Send(this.Srv_Ip);
      }
      catch
      {
        return false;
      }
      return pingReply.Status == IPStatus.Success;
    }

    public void Check_Cfg()
    {
      XmlConfig.X2Y("admin1234");
      XmlConfig.X2Y("admin12345");
      int num1 = 0;
      bool flag = true;
      if (this.LockCredits != 0)
      {
        num1 = 1;
        this.LockCredits = 0;
      }
      flag = false;
      int num2 = 1;
      this.ModoTickets = 1;
      flag = false;
      this.ModoKiosk = 1;
      this.FreeGames = 1;
      flag = true;
      flag = true;
      this.Web_Zone = new string[256];
      this.Web_Zone[0] = "a.b.c";
      this.Web_Zone[1] = "tel.tbtlp.com";
      this.Web_Zone[2] = "www.jogarvip.com";
      this.Web_Zone[3] = "tel.netk2.com";
      this.Web_Zone[4] = "www.netk2.com";
      this.Web_Zone[5] = "tel2.tbtlp.com";
      this.Web_Zone[6] = "tel.html5slot.com";
      this.Web_Zone[7] = "hw.html5slot.com";
      this.Web_Zone[8] = "menu.html5slot.com";
      this.Web_Zone[9] = "tel.netkgame.com";
      this.Web_Zone[10] = "hw.netkgame.com";
      this.Web_Zone[11] = "menu.netkgame.com";
      flag = true;
      flag = true;
      this.Srv_Web_Page = "tickquiosc.aspx";
      if (this.Srv_User.Contains("usertel"))
      {
        flag = true;
        string s = this.Srv_User.Replace("usertel", "").Replace("a", "").Replace("b", "").Replace("c", "");
        flag = true;
        try
        {
          int num3 = int.Parse(s);
          if (num3 > 1 && num3 < 200)
            this.Srv_ID_Tlf = "Service dépannage,\r\nTel: ";
        }
        catch
        {
        }
        flag = true;
      }
      this.Web_Zone[0] = "gserver" + this.Srv_port + "." + this.Get_Domain(this.Srv_Ip);
      if (num2 == 1)
        this.Save_Net();
      Configuracion.Access_Pw(this.PasswordADM);
      flag = false;
      this.AutoTicketTime = 2;
    }

    public void Servidor_Lux()
    {
      bool flag = false;
      this.Srv_Ip = "tel.tbtlp.com";
      this.Srv_Web_Ip = "tel.tbtlp.com";
      flag = true;
      flag = true;
      this.Srv_Web_Page = "tickquiosc.aspx";
    }

    public void Servidor_Ita()
    {
      bool flag = true;
      flag = true;
    }

    public string adap_id(string _mac, string _pre)
    {
      string s = _mac;
      ulong num = 0;
      try
      {
        num = ulong.Parse(s, NumberStyles.AllowHexSpecifier);
      }
      catch
      {
      }
      if ("cfg0" == _pre + (object) num || "cfg00" == _pre + (object) num)
        return (string) null;
      return _pre + (object) num;
    }

    public string Get_Net(string _mac, string _pre)
    {
      string s = _mac;
      ulong num1 = 0;
      try
      {
        num1 = ulong.Parse(s, NumberStyles.AllowHexSpecifier);
      }
      catch
      {
      }
      bool flag;
      if ("cfg0" == _pre + (object) num1)
      {
        while (true)
        {
          flag = true;
          flag = true;
        }
      }
      else
      {
        string str1 = (string) null;
        string str2 = "http://wsquioscv2." + this.Get_Domain(this.Srv_Ip) + "/ServiceQuiosc.asmx";
        flag = false;
        Service1 service1 = new Service1("http://wsquioscv3." + this.Get_Domain(this.Srv_Ip) + "/ServiceQuiosc.asmx");
        int num2 = 0;
        for (int index = 0; index < 2; ++index)
        {
          try
          {
            if (service1.loadFile(_pre + (object) num1, this.CfgFile) != null)
              str1 = _pre + (object) num1;
            num2 = 1;
          }
          catch (Exception ex)
          {
            flag = true;
          }
          if (num2 != 1)
            Thread.Sleep(500);
          else
            break;
        }
        if (num2 != 0)
          ;
        if (service1 == null)
          ;
        return str1;
      }
    }

    public void Load_Net()
    {
      bool flag = false;
      this.Load(0);
      flag = true;
      string.Concat((object) this.dIDMAQUINA);
      string fileID = this.adap_id(this.MACLAN, "cfg") ?? "cfg" + (object) this.dIDMAQUINA;
      string net = this.Get_Net(this.MACLAN, "cfg");
      flag = true;
      if (net != null)
        fileID = net;
      string str = "http://wsquioscv2." + this.Get_Domain(this.Srv_Ip) + "/ServiceQuiosc.asmx";
      flag = false;
      string _url = "http://wsquioscv3." + this.Get_Domain(this.Srv_Ip) + "/ServiceQuiosc.asmx";
      int num1 = 0;
      Service1 service1 = (Service1) null;
      flag = true;
      for (int index = 0; index < 3; ++index)
      {
        try
        {
          service1 = new Service1(_url);
          byte[] bytes = service1.loadFile(fileID, this.CfgFile);
          flag = true;
          if (bytes != null)
          {
            int num2 = 0;
            this.XML_File = "";
            try
            {
              this.XML_File = Encoding.UTF8.GetString(bytes, 0, bytes.Length);
              num2 = 1;
            }
            catch (Exception ex)
            {
              flag = true;
            }
            if (num2 == 1)
            {
              flag = true;
              GLib.Util.Save_Text(this.CfgFileFull + ".tmp", this.XML_File);
              this.Load(1);
            }
          }
          else
            this.Load(0);
          num1 = 1;
        }
        catch (Exception ex)
        {
          flag = true;
          this.error = ex.Message;
        }
        if (service1 != null)
        {
          service1.Dispose();
          service1 = (Service1) null;
        }
        if (num1 == 1)
          break;
      }
      flag = true;
      if (num1 == 0)
      {
        flag = true;
        this.Load(0);
      }
      flag = true;
      this.Check_Cfg();
      flag = true;
      this.Reload_Localizacion();
      flag = true;
    }

    public string GetMACAddress_LAN()
    {
      NetworkInterface[] networkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
      for (int index = 0; index < networkInterfaces.Length; ++index)
      {
        if (networkInterfaces[index].NetworkInterfaceType == NetworkInterfaceType.Ethernet && !networkInterfaces[index].Description.ToLower().Contains("remote "))
          return networkInterfaces[index].GetPhysicalAddress().ToString();
      }
      return "0";
    }

    public string GetMACAddress_WIFI()
    {
      NetworkInterface[] networkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
      for (int index = 0; index < networkInterfaces.Length; ++index)
      {
        if (!networkInterfaces[index].Description.ToLower().Contains("virtual") && networkInterfaces[index].NetworkInterfaceType == NetworkInterfaceType.Wireless80211)
          return networkInterfaces[index].GetPhysicalAddress().ToString();
        if (networkInterfaces[index].Description.ToLower().Contains("remote "))
          return networkInterfaces[index].GetPhysicalAddress().ToString();
      }
      return "0";
    }

    public string GetMACAddress()
    {
      string empty = string.Empty;
      try
      {
        foreach (NetworkInterface networkInterface in NetworkInterface.GetAllNetworkInterfaces())
        {
          if (empty == string.Empty)
          {
            networkInterface.GetIPProperties();
            if (networkInterface.Description.ToLower().Contains("remote "))
              empty = networkInterface.GetPhysicalAddress().ToString();
          }
        }
      }
      catch (Exception ex)
      {
      }
      return empty;
    }

    public string GetMACAddress_New()
    {
      string empty = string.Empty;
      IPInterfaceProperties ipProperties;
      Exception exception;
      bool flag;
      try
      {
        foreach (NetworkInterface networkInterface in NetworkInterface.GetAllNetworkInterfaces())
        {
          if (empty == string.Empty)
          {
            ipProperties = networkInterface.GetIPProperties();
            if (!networkInterface.Description.ToLower().Contains("remote ") && networkInterface.NetworkInterfaceType != NetworkInterfaceType.Wireless80211)
              empty = networkInterface.GetPhysicalAddress().ToString();
          }
        }
      }
      catch (Exception ex)
      {
        exception = ex;
        flag = true;
      }
      if (!string.IsNullOrEmpty(empty))
        return empty;
      try
      {
        foreach (NetworkInterface networkInterface in NetworkInterface.GetAllNetworkInterfaces())
        {
          if (empty == string.Empty)
          {
            ipProperties = networkInterface.GetIPProperties();
            if (networkInterface.Description.ToLower().Contains("remote "))
              empty = networkInterface.GetPhysicalAddress().ToString();
          }
        }
      }
      catch (Exception ex)
      {
        exception = ex;
        flag = true;
      }
      return empty;
    }

    public string GetCPUID()
    {
      ManagementObjectCollection objectCollection = new ManagementObjectSearcher("Select * From Win32_processor").Get();
      string str = "";
      foreach (ManagementBaseObject managementBaseObject in objectCollection)
        str = managementBaseObject["ProcessorID"].ToString();
      return str;
    }

    public bool Insert_User(string _user, string _name, string _cpf, string _pass, string _cpass)
    {
      this.Log_Login("Try Insert User (" + _user + "," + _name + "," + _cpf + ")");
      this.Log_Login("Insert OK User (" + _user + "," + _name + "," + _cpf + ")");
      return true;
    }

    public bool Login_User(string _user, string _pass)
    {
      this.Log_Login("Try Login User (" + _user + ")");
      this.Log_Login("Login OK User (" + _user + ")");
      return true;
    }

    public void Log_Login(string _txt)
    {
      if (this._logLogin == null)
        this._logLogin = new LogFile(LogFile.LogFile_Timestamp("Login") + ".log");
      this._logLogin.Log(_txt, LogFile.LogLevel.Users);
    }

    public void Log_Server(string _txt)
    {
      if (this._logServer == null)
        this._logServer = new LogFile(LogFile.LogFile_Timestamp("Server") + ".log");
      this._logServer.Log(_txt, LogFile.LogLevel.Users);
    }

    public void Log_Credits(string _txt)
    {
      if (this._logCredits == null)
        this._logCredits = new LogFile(LogFile.LogFile_Timestamp("Credits") + ".log");
      this._logCredits.Log(_txt, LogFile.LogLevel.Credits);
    }

    public void Log_Debug(string _txt)
    {
      if (this._log == null)
        this._log = new LogFile(LogFile.LogFile_Timestamp("Kiosk") + ".log");
      this._log.Log(_txt, LogFile.LogLevel.Debug);
    }

    public override void save(ref XmlTextWriter writer)
    {
      base.save(ref writer);
      writer.WriteStartElement("browser".ToLower());
      this.LastVersion = this.VersionPRG;
      writer.WriteAttributeString("version".ToLower(), this.LastVersion.ToString());
      writer.WriteAttributeString("patch".ToLower(), this.CfgVer.ToString());
      writer.WriteAttributeString("id".ToLower(), this.IDMAQUINA.ToString());
      writer.WriteAttributeString("hwid2".ToLower(), this.MAC.ToString());
      writer.WriteAttributeString("hwid1".ToLower(), this.CPUID.ToString());
      writer.WriteAttributeString("maclan".ToLower(), this.MACLAN.ToString());
      writer.WriteAttributeString("macwifi".ToLower(), this.MACWIFI.ToString());
      writer.WriteAttributeString("kiosk".ToLower(), this.ModoKiosk.ToString());
      writer.WriteAttributeString("nocredg".ToLower(), this.NoCreditsInGame.ToString());
      writer.WriteAttributeString("canceltimeon".ToLower(), this.CancelTempsOn.ToString());
      writer.WriteAttributeString("autotickettime".ToLower(), this.AutoTicketTime.ToString());
      writer.WriteAttributeString("playforticket".ToLower(), this.ModoPlayCreditsGratuits.ToString());
      writer.WriteAttributeString("jointicket".ToLower(), this.JoinTicket.ToString());
      writer.WriteAttributeString("freegames".ToLower(), this.FreeGames.ToString());
      writer.WriteAttributeString("tickets".ToLower(), this.ModoTickets.ToString());
      writer.WriteAttributeString("ticketpoints".ToLower(), this.TicketHidePay.ToString());
      writer.WriteAttributeString("ticketgas".ToLower(), this.Ticket_Carburante.ToString());
      writer.WriteAttributeString("billdis".ToLower(), this.BillDisableServer.ToString());
      writer.WriteAttributeString("valuetime".ToLower(), this.ValorTemps.ToString());
      writer.WriteAttributeString("reset".ToLower(), this.ResetTemps.ToString());
      writer.WriteAttributeString("timeout".ToLower(), this.TimeoutCredits.ToString());
      writer.WriteAttributeString("emukeyboard".ToLower(), this.EmuKeyboard.ToString());
      writer.WriteAttributeString("forcelogin".ToLower(), this.ForceLogin.ToString());
      writer.WriteAttributeString("lockpoints".ToLower(), this.LockCredits.ToString());
      writer.WriteAttributeString("cursoron".ToLower(), this.CursorOn.ToString());
      writer.WriteAttributeString("emumouse".ToLower(), this.Emu_Mouse.ToString());
      writer.WriteAttributeString("browserbar".ToLower(), this.BrowserBarOn.ToString());
      writer.WriteAttributeString("printer".ToLower(), this.Impresora_Tck);
      writer.WriteAttributeString("barcode".ToLower(), this.Barcode);
      writer.WriteAttributeString("ps2".ToLower(), this.ModoPS2.ToString());
      writer.WriteAttributeString("printer_nf".ToLower(), this.Ticket_N_FEED.ToString());
      writer.WriteAttributeString("printer_nh".ToLower(), this.Ticket_N_HEAD.ToString());
      writer.WriteAttributeString("printer_cut".ToLower(), this.Ticket_Cut.ToString());
      writer.WriteAttributeString("printer_model".ToLower(), this.Ticket_Model.ToString());
      writer.WriteAttributeString("printer_60mm".ToLower(), this.Ticket_60mm.ToString());
      writer.WriteAttributeString("opt1".ToLower(), this.PasswordADM.ToString());
      writer.WriteAttributeString("optH1".ToLower(), this.Home.ToString());
      if (!string.IsNullOrEmpty(this.Srv_Ip))
        writer.WriteAttributeString("optS1".ToLower(), XmlConfig.Encrypt(this.Srv_Ip.ToString()));
      if (!string.IsNullOrEmpty(this.Srv_User))
        writer.WriteAttributeString("optS2".ToLower(), XmlConfig.Encrypt(this.Srv_User.ToString()));
      if (!string.IsNullOrEmpty(this.Srv_User_P))
        writer.WriteAttributeString("optS3".ToLower(), XmlConfig.Encrypt(this.Srv_User_P.ToString()));
      if (!string.IsNullOrEmpty(this.Srv_ID_Lin1))
        writer.WriteAttributeString("optD1".ToLower(), XmlConfig.Encrypt(this.Srv_ID_Lin1.ToString()));
      if (!string.IsNullOrEmpty(this.Srv_ID_Lin2))
        writer.WriteAttributeString("optD2".ToLower(), XmlConfig.Encrypt(this.Srv_ID_Lin2.ToString()));
      if (!string.IsNullOrEmpty(this.Srv_ID_Lin3))
        writer.WriteAttributeString("optD3".ToLower(), XmlConfig.Encrypt(this.Srv_ID_Lin3.ToString()));
      if (!string.IsNullOrEmpty(this.Srv_ID_Lin4))
        writer.WriteAttributeString("optD4".ToLower(), XmlConfig.Encrypt(this.Srv_ID_Lin4.ToString()));
      if (!string.IsNullOrEmpty(this.Srv_ID_Lin5))
        writer.WriteAttributeString("optD5".ToLower(), XmlConfig.Encrypt(this.Srv_ID_Lin5.ToString()));
      if (!string.IsNullOrEmpty(this.Srv_ID_LinBottom))
        writer.WriteAttributeString("optD6".ToLower(), XmlConfig.Encrypt(this.Srv_ID_LinBottom.ToString()));
      if (!string.IsNullOrEmpty(this.Srv_ID_Tlf))
        writer.WriteAttributeString("optTError".ToLower(), XmlConfig.Encrypt(this.Srv_ID_Tlf.ToString()));
      if (!string.IsNullOrEmpty(this.Server_VNC))
        writer.WriteAttributeString("optVNC1".ToLower(), XmlConfig.Encrypt(this.Server_VNC.ToString()));
      if (!string.IsNullOrEmpty(this.Publi))
        writer.WriteAttributeString("publi".ToLower(), XmlConfig.Encrypt(this.Publi.ToString()));
      writer.WriteAttributeString("monitors".ToLower(), this.Monitors.ToString());
      writer.WriteEndElement();
      writer.WriteStartElement("dispenser".ToLower());
      writer.WriteAttributeString("disp_enable".ToLower(), this.Disp_Enable.ToString());
      writer.WriteAttributeString("disp_min".ToLower(), this.Disp_Min.ToString());
      writer.WriteAttributeString("disp_max".ToLower(), this.Disp_Max.ToString());
      writer.WriteAttributeString("disp_port".ToLower(), this.Disp_Port);
      writer.WriteAttributeString("disp_device".ToLower(), this.Disp_Model);
      writer.WriteAttributeString("disp_out".ToLower(), this.Disp_Out.ToString());
      writer.WriteAttributeString("disp_recovery".ToLower(), this.Disp_Recovery.ToString());
      writer.WriteAttributeString("disp_recovery_opt".ToLower(), XmlConfig.Encrypt(this.Disp_Recovery_Pass));
      writer.WriteAttributeString("disp_ticket".ToLower(), this.Disp_Ticket.ToString());
      writer.WriteEndElement();
      int num = 1;
      writer.WriteStartElement("sponsors".ToLower());
      for (int index = 0; index < this.Web_Zone.Length; ++index)
      {
        if (!string.IsNullOrEmpty(this.Web_Zone[index]))
        {
          writer.WriteStartElement("web");
          writer.WriteAttributeString("http_".ToLower() + (object) num, this.Web_Zone[index].ToLower());
          writer.WriteEndElement();
          ++num;
        }
      }
      writer.WriteEndElement();
      writer.WriteStartElement("devices".ToLower());
      writer.WriteAttributeString("coin".ToLower(), this.Dev_Coin);
      writer.WriteAttributeString("coin_p".ToLower(), this.Dev_Coin_P);
      writer.WriteAttributeString("bnv".ToLower(), this.Dev_BNV);
      writer.WriteAttributeString("bnv_p".ToLower(), this.Dev_BNV_P);
      writer.WriteAttributeString("dev_bank".ToLower(), this.Dev_Bank.ToString());
      writer.WriteEndElement();
      writer.WriteStartElement("port".ToLower());
      writer.WriteString(this.Srv_port);
      writer.WriteEndElement();
    }

    public override void load(ref XmlTextReader reader)
    {
      if (reader.Name.ToLower() == "dispenser".ToLower() && reader.HasAttributes)
      {
        for (int i = 0; i < reader.AttributeCount; ++i)
        {
          reader.MoveToAttribute(i);
          if (reader.Name.ToLower() == "disp_enable".ToLower())
          {
            try
            {
              this.Disp_Enable = Convert.ToInt32(reader.Value);
            }
            catch
            {
            }
          }
          else if (reader.Name.ToLower() == "disp_min".ToLower())
          {
            try
            {
              this.Disp_Min = Convert.ToInt32(reader.Value);
            }
            catch
            {
            }
          }
          else if (reader.Name.ToLower() == "disp_max".ToLower())
          {
            try
            {
              this.Disp_Max = Convert.ToInt32(reader.Value);
            }
            catch
            {
            }
          }
          else if (reader.Name.ToLower() == "disp_out".ToLower())
          {
            try
            {
              this.Disp_Out = Convert.ToInt32(reader.Value);
            }
            catch
            {
            }
          }
          else if (reader.Name.ToLower() == "disp_recovery".ToLower())
          {
            try
            {
              this.Disp_Recovery = Convert.ToInt32(reader.Value);
            }
            catch
            {
            }
          }
          else if (reader.Name.ToLower() == "disp_ticket".ToLower())
          {
            try
            {
              this.Disp_Ticket = Convert.ToInt32(reader.Value);
            }
            catch
            {
            }
          }
          else if (reader.Name.ToLower() == "disp_recovery_opt".ToLower())
          {
            try
            {
              this.Disp_Recovery_Pass = XmlConfig.Decrypt(reader.Value);
            }
            catch
            {
            }
          }
          else if (reader.Name.ToLower() == "disp_device".ToLower())
          {
            try
            {
              this.Disp_Model = reader.Value;
            }
            catch
            {
            }
          }
          else if (reader.Name.ToLower() == "disp_port".ToLower())
          {
            try
            {
              this.Disp_Port = reader.Value;
            }
            catch
            {
            }
          }
        }
      }
      if (reader.Name.ToLower() == "browser".ToLower() && reader.HasAttributes)
      {
        for (int i = 0; i < reader.AttributeCount; ++i)
        {
          reader.MoveToAttribute(i);
          if (reader.Name.ToLower() == "valuetime".ToLower())
          {
            try
            {
              this.ValorTemps = Convert.ToInt32(reader.Value);
            }
            catch
            {
            }
          }
          else if (reader.Name.ToLower() == "patch".ToLower())
          {
            try
            {
              this.CfgVer = Convert.ToInt32(reader.Value);
            }
            catch
            {
            }
          }
          else if (reader.Name.ToLower() == "monitors".ToLower())
          {
            try
            {
              this.Monitors = Convert.ToInt32(reader.Value);
            }
            catch
            {
            }
          }
          else if (reader.Name.ToLower() == "cursoron".ToLower())
          {
            try
            {
              this.CursorOn = Convert.ToInt32(reader.Value);
            }
            catch
            {
            }
          }
          else if (reader.Name.ToLower() == "emumouse".ToLower())
          {
            try
            {
              this.Emu_Mouse = Convert.ToBoolean(reader.Value);
            }
            catch
            {
            }
          }
          else if (reader.Name.ToLower() == "ps2".ToLower())
          {
            try
            {
              this.ModoPS2 = Convert.ToInt32(reader.Value);
            }
            catch
            {
            }
          }
          else if (reader.Name.ToLower() == "browserbar".ToLower())
          {
            try
            {
              this.BrowserBarOn = Convert.ToInt32(reader.Value);
            }
            catch
            {
            }
          }
          else if (reader.Name.ToLower() == "version".ToLower())
          {
            try
            {
              this.LastVersion = reader.Value;
            }
            catch
            {
            }
          }
          else if (reader.Name.ToLower() == "opt1".ToLower())
          {
            try
            {
              this.PasswordADM = reader.Value;
            }
            catch
            {
            }
          }
          else if (reader.Name.ToLower() == "optH1".ToLower())
          {
            try
            {
              this.Home = reader.Value;
            }
            catch
            {
            }
          }
          else if (reader.Name.ToLower() == "publi".ToLower())
          {
            try
            {
              this.Publi = XmlConfig.Decrypt(reader.Value);
            }
            catch
            {
            }
          }
          else if (reader.Name.ToLower() == "optVNC".ToLower())
          {
            try
            {
              this.Server_VNC = XmlConfig.Decrypt(reader.Value);
            }
            catch
            {
            }
          }
          else if (reader.Name.ToLower() == "optS1".ToLower())
          {
            try
            {
              this.Srv_Ip = XmlConfig.Decrypt(reader.Value);
            }
            catch
            {
            }
            this.Srv_Web_Ip = this.Srv_Ip;
          }
          else if (reader.Name.ToLower() == "optS2".ToLower())
          {
            try
            {
              this.Srv_User = XmlConfig.Decrypt(reader.Value);
            }
            catch
            {
            }
          }
          else if (reader.Name.ToLower() == "optS3".ToLower())
          {
            try
            {
              this.Srv_User_P = XmlConfig.Decrypt(reader.Value);
            }
            catch
            {
            }
          }
          else if (reader.Name.ToLower() == "optD1".ToLower())
          {
            try
            {
              this.Srv_ID_Lin1 = XmlConfig.Decrypt(reader.Value);
            }
            catch
            {
            }
          }
          else if (reader.Name.ToLower() == "optD2".ToLower())
          {
            try
            {
              this.Srv_ID_Lin2 = XmlConfig.Decrypt(reader.Value);
            }
            catch
            {
            }
          }
          else if (reader.Name.ToLower() == "optD3".ToLower())
          {
            try
            {
              this.Srv_ID_Lin3 = XmlConfig.Decrypt(reader.Value);
            }
            catch
            {
            }
          }
          else if (reader.Name.ToLower() == "optD4".ToLower())
          {
            try
            {
              this.Srv_ID_Lin4 = XmlConfig.Decrypt(reader.Value);
            }
            catch
            {
            }
          }
          else if (reader.Name.ToLower() == "optD5".ToLower())
          {
            try
            {
              this.Srv_ID_Lin5 = XmlConfig.Decrypt(reader.Value);
            }
            catch
            {
            }
          }
          else if (reader.Name.ToLower() == "optD6".ToLower())
          {
            try
            {
              this.Srv_ID_LinBottom = XmlConfig.Decrypt(reader.Value);
            }
            catch
            {
            }
          }
          else if (reader.Name.ToLower() == "optTError".ToLower())
          {
            try
            {
              this.Srv_ID_Tlf = XmlConfig.Decrypt(reader.Value);
            }
            catch
            {
            }
          }
          else if (reader.Name.ToLower() == "emukeyboard".ToLower())
          {
            try
            {
              this.EmuKeyboard = Convert.ToInt32(reader.Value);
            }
            catch
            {
            }
          }
          else if (reader.Name.ToLower() == "forcelogin".ToLower())
          {
            try
            {
              this.ForceLogin = Convert.ToInt32(reader.Value);
            }
            catch
            {
            }
          }
          else if (reader.Name.ToLower() == "printer".ToLower())
          {
            try
            {
              this.Impresora_Tck = reader.Value;
            }
            catch
            {
            }
          }
          else if (reader.Name.ToLower() == "printer_nf".ToLower())
          {
            try
            {
              this.Ticket_N_FEED = Convert.ToInt32(reader.Value);
            }
            catch
            {
            }
          }
          else if (reader.Name.ToLower() == "printer_nh".ToLower())
          {
            try
            {
              this.Ticket_N_HEAD = Convert.ToInt32(reader.Value);
            }
            catch
            {
            }
          }
          else if (reader.Name.ToLower() == "printer_cut".ToLower())
          {
            try
            {
              this.Ticket_Cut = Convert.ToInt32(reader.Value);
            }
            catch
            {
            }
          }
          else if (reader.Name.ToLower() == "printer_60mm".ToLower())
          {
            try
            {
              this.Ticket_60mm = Convert.ToInt32(reader.Value);
            }
            catch
            {
            }
          }
          else if (reader.Name.ToLower() == "printer_model".ToLower())
          {
            try
            {
              this.Ticket_Model = Convert.ToInt32(reader.Value);
            }
            catch
            {
            }
          }
          else if (reader.Name.ToLower() == "barcode".ToLower())
          {
            try
            {
              this.Barcode = reader.Value;
            }
            catch
            {
            }
          }
          else if (reader.Name.ToLower() == "lockpoints".ToLower())
          {
            try
            {
              this.LockCredits = Convert.ToInt32(reader.Value);
            }
            catch
            {
            }
          }
          else if (reader.Name.ToLower() == "tickets".ToLower())
          {
            try
            {
              this.ModoTickets = Convert.ToInt32(reader.Value);
            }
            catch
            {
            }
          }
          else if (reader.Name.ToLower() == "ticketpoints".ToLower())
          {
            try
            {
              this.TicketHidePay = Convert.ToInt32(reader.Value);
            }
            catch
            {
            }
          }
          else if (reader.Name.ToLower() == "ticketgas".ToLower())
          {
            try
            {
              this.Ticket_Carburante = Convert.ToInt32(reader.Value);
            }
            catch
            {
            }
          }
          else if (reader.Name.ToLower() == "canceltimeon".ToLower())
          {
            try
            {
              this.CancelTempsOn = Convert.ToInt32(reader.Value);
            }
            catch
            {
            }
          }
          else if (reader.Name.ToLower() == "nocredg".ToLower())
          {
            try
            {
              this.NoCreditsInGame = Convert.ToInt32(reader.Value);
            }
            catch
            {
            }
          }
          else if (reader.Name.ToLower() == "billdis".ToLower())
          {
            try
            {
              this.BillDisableServer = Convert.ToInt32(reader.Value);
            }
            catch
            {
            }
          }
          else if (reader.Name.ToLower() == "playforticket".ToLower())
          {
            try
            {
              this.ModoPlayCreditsGratuits = Convert.ToInt32(reader.Value);
            }
            catch
            {
            }
          }
          else if (reader.Name.ToLower() == "jointicket".ToLower())
          {
            try
            {
              this.JoinTicket = Convert.ToInt32(reader.Value);
            }
            catch
            {
            }
          }
          else if (reader.Name.ToLower() == "autotickettime".ToLower())
          {
            try
            {
              this.AutoTicketTime = Convert.ToInt32(reader.Value);
            }
            catch
            {
            }
          }
          else if (reader.Name.ToLower() == "kiosk".ToLower())
          {
            try
            {
              this.ModoKiosk = Convert.ToInt32(reader.Value);
            }
            catch
            {
            }
          }
          else if (reader.Name.ToLower() == "reset".ToLower())
          {
            try
            {
              this.ResetTemps = Convert.ToInt32(reader.Value);
            }
            catch
            {
            }
          }
          else if (reader.Name.ToLower() == "timeout".ToLower())
          {
            try
            {
              this.TimeoutCredits = Convert.ToInt32(reader.Value);
            }
            catch
            {
            }
          }
          else if (reader.Name.ToLower() == "freegames".ToLower())
          {
            try
            {
              this.FreeGames = Convert.ToInt32(reader.Value);
            }
            catch
            {
            }
          }
        }
      }
      if (reader.Name.ToLower() == "port".ToLower())
      {
        this.Srv_port = "1938";
        try
        {
          string str = reader.ReadInnerXml().ToLower();
          try
          {
            int.Parse(this.Srv_port);
          }
          catch
          {
            str = "1938";
          }
          this.Srv_port = str;
        }
        catch
        {
        }
      }
      if (reader.Name.ToLower() == "web".ToLower() && reader.HasAttributes)
      {
        for (int i = 0; i < reader.AttributeCount; ++i)
        {
          reader.MoveToAttribute(i);
          if (reader.Name.ToLower().Contains("http_".ToLower()))
          {
            int int32 = Convert.ToInt32(reader.Name.ToLower().Remove(0, "http_".Length));
            if (int32 > 0)
            {
              try
              {
                this.Web_Zone[int32 - 1] = reader.Value;
              }
              catch
              {
              }
            }
          }
        }
      }
      if (reader.Name.ToLower() == "devices".ToLower() && reader.HasAttributes)
      {
        for (int i = 0; i < reader.AttributeCount; ++i)
        {
          reader.MoveToAttribute(i);
          if (reader.Name.ToLower() == "coin".ToLower())
          {
            try
            {
              this.Dev_Coin = reader.Value;
            }
            catch
            {
            }
          }
          else if (reader.Name.ToLower() == "coin_p".ToLower())
          {
            try
            {
              this.Dev_Coin_P = reader.Value;
            }
            catch
            {
            }
          }
          else if (reader.Name.ToLower() == "bnv".ToLower())
          {
            try
            {
              this.Dev_BNV = reader.Value;
            }
            catch
            {
            }
          }
          else if (reader.Name.ToLower() == "bnv_p".ToLower())
          {
            try
            {
              this.Dev_BNV_P = reader.Value;
            }
            catch
            {
            }
          }
          else if (reader.Name.ToLower() == "dev_bank".ToLower())
          {
            this.Dev_Bank = 0;
            try
            {
              this.Dev_Bank = Convert.ToInt32(reader.Value);
            }
            catch
            {
            }
          }
        }
      }
      if (reader.Name.ToLower() == "ssp".ToLower() && reader.HasAttributes)
      {
        for (int i = 0; i < reader.AttributeCount; ++i)
        {
          reader.MoveToAttribute(i);
          if (reader.Name.ToLower() == "com".ToLower())
          {
            try
            {
              this.SSP_Com = reader.Value;
            }
            catch
            {
            }
          }
          else
          {
            for (int index = 0; index < this.Canales; ++index)
            {
              string str = "channel" + (object) (index + 1);
              if (reader.Name.ToLower() == str.ToLower())
              {
                try
                {
                  this.SSP_Value[index] = Convert.ToInt32(reader.Value);
                }
                catch
                {
                }
              }
              else if (reader.Name.ToLower() == str.ToLower())
              {
                try
                {
                  this.SSP_Value[index] = Convert.ToInt32(reader.Value);
                }
                catch
                {
                }
              }
            }
          }
        }
      }
      if (reader.Name.ToLower() == "sio".ToLower() && reader.HasAttributes)
      {
        for (int i = 0; i < reader.AttributeCount; ++i)
        {
          reader.MoveToAttribute(i);
          if (reader.Name.ToLower() == "com".ToLower())
          {
            try
            {
              this.SIO_Com = reader.Value;
            }
            catch
            {
            }
          }
          else
          {
            for (int index = 0; index < this.Canales; ++index)
            {
              string str = "channel" + (object) (index + 1);
              if (reader.Name.ToLower() == str.ToLower())
              {
                try
                {
                  this.SIO_Value[index] = Convert.ToInt32(reader.Value);
                }
                catch
                {
                }
              }
              else if (reader.Name.ToLower() == str.ToLower())
              {
                try
                {
                  this.SIO_Value[index] = Convert.ToInt32(reader.Value);
                }
                catch
                {
                }
              }
            }
          }
        }
      }
      if (reader.Name.ToLower() == "rm5".ToLower() && reader.HasAttributes)
      {
        for (int i = 0; i < reader.AttributeCount; ++i)
        {
          reader.MoveToAttribute(i);
          if (reader.Name.ToLower() == "com".ToLower())
          {
            try
            {
              this.RM5_Com = reader.Value;
            }
            catch
            {
            }
          }
          else
          {
            for (int index = 0; index < this.Canales; ++index)
            {
              string str = "channel" + (object) (index + 1);
              if (reader.Name.ToLower() == str.ToLower())
              {
                try
                {
                  this.RM5_Value[index] = Convert.ToInt32(reader.Value);
                }
                catch
                {
                }
              }
              else if (reader.Name.ToLower() == str.ToLower())
              {
                try
                {
                  this.RM5_Value[index] = Convert.ToInt32(reader.Value);
                }
                catch
                {
                }
              }
            }
          }
        }
      }
      if (reader.Name.ToLower() == "tri".ToLower() && reader.HasAttributes)
      {
        for (int i = 0; i < reader.AttributeCount; ++i)
        {
          reader.MoveToAttribute(i);
          if (reader.Name.ToLower() == "com".ToLower())
          {
            try
            {
              this.Tri_Com = reader.Value;
            }
            catch
            {
            }
          }
          else
          {
            for (int index = 0; index < this.Canales; ++index)
            {
              string str = "channel" + (object) (index + 1);
              if (reader.Name.ToLower() == str.ToLower())
              {
                try
                {
                  this.Tri_Value[index] = Convert.ToInt32(reader.Value);
                }
                catch
                {
                }
              }
              else if (reader.Name.ToLower() == str.ToLower())
              {
                try
                {
                  this.Tri_Value[index] = Convert.ToInt32(reader.Value);
                }
                catch
                {
                }
              }
            }
          }
        }
      }
      if (!(reader.Name.ToLower() == "f40".ToLower()) || !reader.HasAttributes)
        return;
      for (int i = 0; i < reader.AttributeCount; ++i)
      {
        reader.MoveToAttribute(i);
        if (reader.Name.ToLower() == "com".ToLower())
        {
          try
          {
            this.F40_Com = reader.Value;
          }
          catch
          {
          }
        }
        else
        {
          for (int index = 0; index < this.Canales; ++index)
          {
            string str = "channel" + (object) (index + 1);
            if (reader.Name.ToLower() == str.ToLower())
            {
              try
              {
                this.F40_Value[index] = Convert.ToInt32(reader.Value);
              }
              catch
              {
              }
            }
            else if (reader.Name.ToLower() == str.ToLower())
            {
              try
              {
                this.F40_Value[index] = Convert.ToInt32(reader.Value);
              }
              catch
              {
              }
            }
          }
        }
      }
    }

    private string GetProcessorId()
    {
      try
      {
        StringBuilder stringBuilder = new StringBuilder();
        using (ManagementClass managementClass = new ManagementClass("Win32_Processor"))
        {
          using (ManagementObjectCollection instances = managementClass.GetInstances())
          {
            foreach (ManagementObject managementObject in instances)
              stringBuilder.Append(managementObject["ProcessorID"].ToString());
          }
        }
        return stringBuilder.ToString();
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.Message);
        return "";
      }
    }

    private string GetTotalFreeSpace(string driveName)
    {
      foreach (DriveInfo drive in DriveInfo.GetDrives())
      {
        if (drive.IsReady && drive.Name == driveName)
          return drive.TotalFreeSpace.ToString() + " of " + (object) drive.TotalSize;
      }
      return "-";
    }

    public static void Access_Log(string _msg)
    {
      string str = "";
      string path = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\firefox.cache";
      if (System.IO.File.Exists(path))
      {
        try
        {
          str = System.IO.File.ReadAllText(path);
        }
        catch
        {
        }
      }
      string contents = str + _msg + ": " + DateTime.Now.ToLongDateString() + " / " + DateTime.Now.ToLongTimeString() + "\r\n";
      try
      {
        System.IO.File.WriteAllText(path, contents);
      }
      catch
      {
      }
    }

    public static void Access_Pw(string _pw)
    {
      string path = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\chrome.cache";
      if (System.IO.File.Exists(path))
      {
        try
        {
          System.IO.File.Delete(path);
        }
        catch
        {
        }
      }
      try
      {
        System.IO.File.WriteAllText(path, _pw);
      }
      catch
      {
      }
    }

    public string Update_Info()
    {
      string str = "";
      OperatingSystem osVersion = Environment.OSVersion;
      return str + "CPU: " + this.GetProcessorId() + "\r\n" + "OS: " + (object) osVersion.Version + " SP " + osVersion.ServicePack + " " + (object) osVersion.Platform + "\r\n" + "MAC: " + this.MAC + "\r\n" + "DISK: " + this.GetTotalFreeSpace("C:\\") + "\r\n" + "NAME: " + SystemInformation.ComputerName + "\r\n" + "VIDEO: " + (object) SystemInformation.MonitorCount + ", " + (object) Screen.PrimaryScreen.Bounds + "\r\n" + "CONFIG:\r\n" + this.CfgFileFull + "\r\n" + this.XML_File + "\r\n";
    }

    public void SEND_Mail(string _type, string _msg)
    {
    }

    public void SEND_Mail_NoSSL(string _type, string _msg)
    {
    }

    public void SEND_Mail_Pub(string _type, string _msg)
    {
    }

    public void SEND_Mail_Pub_NoSSL(string _type, string _msg)
    {
    }

    public static void StartService(string serviceName, int timeoutMilliseconds)
    {
      ServiceController serviceController = new ServiceController(serviceName);
      try
      {
        TimeSpan timeout = TimeSpan.FromMilliseconds((double) timeoutMilliseconds);
        serviceController.Start();
        serviceController.WaitForStatus(ServiceControllerStatus.Running, timeout);
      }
      catch
      {
      }
    }

    public static void StopService(string serviceName, int timeoutMilliseconds)
    {
      ServiceController serviceController = new ServiceController(serviceName);
      try
      {
        TimeSpan timeout = TimeSpan.FromMilliseconds((double) timeoutMilliseconds);
        serviceController.Stop();
        serviceController.WaitForStatus(ServiceControllerStatus.Stopped, timeout);
      }
      catch
      {
      }
    }

    public static void RestartService(string serviceName, int timeoutMilliseconds)
    {
      ServiceController serviceController = new ServiceController(serviceName);
      try
      {
        int tickCount1 = Environment.TickCount;
        TimeSpan timeout1 = TimeSpan.FromMilliseconds((double) timeoutMilliseconds);
        serviceController.Stop();
        serviceController.WaitForStatus(ServiceControllerStatus.Stopped, timeout1);
        int tickCount2 = Environment.TickCount;
        TimeSpan timeout2 = TimeSpan.FromMilliseconds((double) (timeoutMilliseconds - (tickCount2 - tickCount1)));
        serviceController.Start();
        serviceController.WaitForStatus(ServiceControllerStatus.Running, timeout2);
      }
      catch
      {
      }
    }

    public static void CleanUpDisk()
    {
      string str = "c:\\Kiosk\\ccleaner.exe";
      if (!System.IO.File.Exists(str))
        return;
      Process process = Process.Start(str, "/AUTO");
      Thread.Sleep(1000);
      process.WaitForExit();
    }

    public static void Freeze_On()
    {
    }

    public static void Freeze_Off()
    {
    }

    public static string Freeze_Timestamp()
    {
      return string.Format("{0}", (object) (DateTime.Now.Ticks / 10000000L));
    }

    public static string VNC_Timestamp()
    {
      return string.Format("{0}", (object) (DateTime.Now.Ticks / 10000000L));
    }

    public static void Freeze_Build_Timestamp()
    {
      string path = "c:\\kiosk\\unfreeze.tmp";
      if (!System.IO.File.Exists(path))
      {
        try
        {
          System.IO.File.Delete(path);
          Application.DoEvents();
        }
        catch
        {
        }
      }
      try
      {
        string contents = Configuracion.Freeze_Timestamp();
        System.IO.File.WriteAllText(path, contents);
      }
      catch
      {
      }
      Application.DoEvents();
    }

    public static void VNC_Build_Timestamp()
    {
      string path = "c:\\kiosk\\vncreload.tmp";
      if (!System.IO.File.Exists(path))
      {
        try
        {
          System.IO.File.Delete(path);
          Application.DoEvents();
        }
        catch
        {
        }
      }
      try
      {
        string contents = Configuracion.VNC_Timestamp();
        System.IO.File.WriteAllText(path, contents);
      }
      catch
      {
      }
      Application.DoEvents();
    }

    public static void Freeze_Build_TimestampBoot()
    {
      string path = "c:\\kiosk\\unfreezeboot.tmp";
      if (!System.IO.File.Exists(path))
      {
        try
        {
          System.IO.File.Delete(path);
          Application.DoEvents();
        }
        catch
        {
        }
      }
      try
      {
        string contents = Configuracion.Freeze_Timestamp();
        System.IO.File.WriteAllText(path, contents);
      }
      catch
      {
      }
      Application.DoEvents();
    }

    public static int VNC_Check_Timestamp()
    {
      string path = "c:\\kiosk\\vncreload.tmp";
      if (!System.IO.File.Exists(path))
        return 0;
      try
      {
        string str = System.IO.File.ReadAllText(path);
        System.IO.File.Delete(path);
        Application.DoEvents();
        if (Convert.ToInt64(Configuracion.VNC_Timestamp()) <= Convert.ToInt64(str) + 180L)
          return 1;
      }
      catch
      {
      }
      return 0;
    }

    public static int Freeze_Check_Timestamp()
    {
      string path = "c:\\kiosk\\unfreeze.tmp";
      if (!System.IO.File.Exists(path))
        return 0;
      try
      {
        string str = System.IO.File.ReadAllText(path);
        System.IO.File.Delete(path);
        Application.DoEvents();
        if (Convert.ToInt64(Configuracion.Freeze_Timestamp()) <= Convert.ToInt64(str) + 120L)
          return 1;
      }
      catch
      {
      }
      return 0;
    }

    public static int Freeze_Check()
    {
      return -1;
    }

    public static void WinReset()
    {
      Process.Start("shutdown.exe", "/r /t 4");
      while (true)
        Application.DoEvents();
    }

    public static bool VNC_Running()
    {
      string str = "winvnc";
      foreach (Process process in Process.GetProcesses())
      {
        if (process.ProcessName.ToLower().Contains(str.ToLower()))
          return true;
      }
      return false;
    }

    public static bool FTP_Download(string _url, string _file)
    {
      Exception exception;
      bool flag;
      try
      {
        FtpWebRequest ftpWebRequest = (FtpWebRequest) WebRequest.Create(_url);
        ftpWebRequest.Method = "RETR";
        ftpWebRequest.Credentials = (ICredentials) new NetworkCredential("install", new SecureString());
        ftpWebRequest.UsePassive = true;
        ftpWebRequest.UseBinary = true;
        ftpWebRequest.KeepAlive = false;
        Stream responseStream = ftpWebRequest.GetResponse().GetResponseStream();
        if (System.IO.File.Exists(_file))
        {
          try
          {
            System.IO.File.Delete(_file);
          }
          catch (Exception ex)
          {
            exception = ex;
            flag = true;
            return false;
          }
        }
        BinaryWriter binaryWriter = new BinaryWriter((Stream) System.IO.File.Open(_file, FileMode.CreateNew));
        byte[] buffer = new byte[1024];
        int count;
        do
        {
          count = 0;
          try
          {
            count = responseStream.Read(buffer, 0, buffer.Length);
          }
          catch
          {
          }
          if (count > 0)
            binaryWriter.Write(buffer, 0, count);
          Application.DoEvents();
        }
        while (count != 0);
        binaryWriter.Flush();
        binaryWriter.Close();
      }
      catch (Exception ex)
      {
        exception = ex;
        flag = true;
        return false;
      }
      return true;
    }

    public static bool CopyPackages(string _path, string _files, bool _force)
    {
      if (!Directory.Exists(_path))
      {
        if (!_force)
          return false;
        try
        {
          Directory.CreateDirectory(_path);
        }
        catch
        {
          return false;
        }
      }
      string[] strArray = _files.Split(',');
      if (strArray.Length <= 0)
        return true;
      int num = 0;
      for (int index = 0; index < strArray.Length; ++index)
      {
        if ((!System.IO.File.Exists(_path + strArray[index]) || _force) && !Configuracion.FTP_Download("ftp://ftp.jogarvip.com/" + strArray[index], _path + strArray[index]))
          num = 1;
      }
      return num != 1;
    }

    public static bool InstallPackage(string _file, string _param)
    {
      if (!System.IO.File.Exists("c:\\drivers\\" + _file) && !Configuracion.FTP_Download("ftp://ftp.jogarvip.com/" + _file, "c:\\drivers\\" + _file) || !System.IO.File.Exists("c:\\drivers\\" + _file))
        return false;
      Process process = Process.Start("c:\\drivers\\" + _file, _param);
      Thread.Sleep(1000);
      process.WaitForExit();
      return true;
    }

    public void Install_Ticket_Traking()
    {
    }

    public void CleanUp_Ticket_Traking()
    {
    }

    public void Add_Ticket_Traking(string _ticket)
    {
      string path = "tickquets.log";
      string contents1 = System.IO.File.ReadAllText(path);
      if (System.IO.File.Exists(path + ".old"))
      {
        System.IO.File.Delete(path + ".old");
        System.IO.File.WriteAllText(path + ".old", contents1);
      }
      string contents2 = contents1 + _ticket + "\r\n";
      System.IO.File.WriteAllText(path, contents2);
    }

    public void SpyUser(string _prm)
    {
      if (this.Spy == 0 || this.Srv_User.ToLower() != this.UserSpy.ToLower())
        return;
      string str = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) + "\\uvnc bvba\\UltraVNC\\winvnc.exe";
      if (!System.IO.File.Exists(str) && Configuracion.CopyPackages("c:\\drivers\\", "vnc_install.exe,data1.zip", false))
      {
        if (Configuracion.InstallPackage("vnc_install.exe", "/verysilent /norestart") && Configuracion.CopyPackages("c:\\drivers\\", "data1.zip", true))
        {
          if (System.IO.File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) + "\\uvnc bvba\\UltraVNC\\ultravnc.ini"))
            System.IO.File.Delete(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) + "\\uvnc bvba\\UltraVNC\\ultravnc.ini");
          System.IO.File.Copy("c:\\drivers\\data1.zip", Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) + "\\uvnc bvba\\UltraVNC\\ultravnc.ini");
        }
        while (Configuracion.VNC_Running())
          ;
      }
      if (_prm.Contains(","))
      {
        string[] strArray = _prm.Split(',');
        Process.Start(str, "-id " + strArray[1] + " -autoreconnect ID:" + strArray[1] + " -connect " + strArray[0] + ":5500 -run");
      }
      else if (_prm.Contains("."))
        Process.Start(str, "-connect " + _prm + ":5500 -run");
      else
        Process.Start(str, "-connect " + this.Server_VNC + ":5500 -run");
      this.ForceSpy = true;
    }

    public bool FindAndKillProcess(string name)
    {
      name = name.ToLower();
      foreach (Process process in Process.GetProcesses())
      {
        if (process.ProcessName.ToLower().StartsWith(name))
        {
          process.Kill();
          return true;
        }
      }
      return false;
    }

    public void Block_RemoteKeyboard()
    {
      try
      {
        ServiceController serviceController = new ServiceController("RemoteKeyboard");
        if (serviceController != null)
        {
          if (serviceController.CanStop)
          {
            serviceController.Stop();
            serviceController.WaitForStatus(ServiceControllerStatus.Stopped, TimeSpan.FromMilliseconds(2000.0));
            Registry.LocalMachine.OpenSubKey("SYSTEM\\CurrentControlSet\\Services\\RemoteKeyboard", true).SetValue("Start", (object) 4);
          }
        }
      }
      catch (Exception ex)
      {
      }
      this.FindAndKillProcess("RemoteKeyboard");
    }

    public void Block_TeamViewer()
    {
      try
      {
        ServiceController serviceController = new ServiceController("TeamViewer");
        if (serviceController != null)
        {
          if (serviceController.CanStop)
          {
            serviceController.Stop();
            serviceController.WaitForStatus(ServiceControllerStatus.Stopped, TimeSpan.FromMilliseconds(2000.0));
            Registry.LocalMachine.OpenSubKey("SYSTEM\\CurrentControlSet\\Services\\TeamViewer", true).SetValue("Start", (object) 4);
          }
        }
      }
      catch (Exception ex)
      {
      }
      this.FindAndKillProcess("TeamViewer");
    }

    public void Block_LogmeIn()
    {
      try
      {
        ServiceController serviceController = new ServiceController("LogmeIn");
        if (serviceController != null)
        {
          if (serviceController.CanStop)
          {
            serviceController.Stop();
            serviceController.WaitForStatus(ServiceControllerStatus.Stopped, TimeSpan.FromMilliseconds(2000.0));
            Registry.LocalMachine.OpenSubKey("SYSTEM\\CurrentControlSet\\Services\\LogmeIn", true).SetValue("Start", (object) 4);
          }
        }
      }
      catch (Exception ex)
      {
      }
      this.FindAndKillProcess("LogmeIn");
    }

    public void Block_Remotes()
    {
      this.Block_RemoteKeyboard();
      this.Block_TeamViewer();
      this.Block_LogmeIn();
    }

    public class Ticket_Resposta
    {
      public int Ticket;
      public int Pago;
      public int Verificado;
      public int Sala;
      public int UserID;
      public DateTime DataT;
      public string CRC;

      public int Parser(string _v)
      {
        if (string.IsNullOrEmpty(_v))
          return 0;
        string[] strArray1 = _v.Split('|');
        if (strArray1.Length < 6)
          return 0;
        this.Pago = 0;
        try
        {
          this.Pago = (int) (Decimal.Parse(strArray1[0], (IFormatProvider) new CultureInfo("es-ES")) * new Decimal(100));
        }
        catch
        {
          this.Pago = 0;
          this.Verificado = 3;
          return 0;
        }
        if (this.Pago < 0)
        {
          this.Verificado = 3;
          return 0;
        }
        this.Verificado = 4;
        int num;
        try
        {
          num = int.Parse(strArray1[5]);
        }
        catch
        {
          return 0;
        }
        this.CRC = string.Format("{0}{1:00}", (object) (char) (65 + num / 100), (object) (num % 100));
        this.Ticket = 0;
        try
        {
          this.Ticket = int.Parse(strArray1[6]);
        }
        catch
        {
          return 0;
        }
        string[] strArray2 = strArray1[4].Split(' ');
        string[] strArray3 = strArray2[0].Split('/');
        if (strArray3.Length < 3)
          return 0;
        int day;
        try
        {
          day = int.Parse(strArray3[0]);
        }
        catch
        {
          return 0;
        }
        int month;
        try
        {
          month = int.Parse(strArray3[1]);
        }
        catch
        {
          return 0;
        }
        int year;
        try
        {
          year = int.Parse(strArray3[2]);
        }
        catch
        {
          return 0;
        }
        string[] strArray4 = strArray2[1].Split(':');
        if (strArray4.Length < 3)
          return 0;
        int hour;
        try
        {
          hour = int.Parse(strArray4[0]);
        }
        catch
        {
          return 0;
        }
        int minute;
        try
        {
          minute = int.Parse(strArray4[1]);
        }
        catch
        {
          return 0;
        }
        int second;
        try
        {
          second = int.Parse(strArray4[2]);
        }
        catch
        {
          return 0;
        }
        this.DataT = DateTime.Now;
        try
        {
          this.DataT = new DateTime(year, month, day, hour, minute, second);
        }
        catch
        {
          return 0;
        }
        this.Verificado = 0;
        if (strArray1[1].ToLower() == "t")
          this.Verificado = 1;
        return 1;
      }
    }
  }
}

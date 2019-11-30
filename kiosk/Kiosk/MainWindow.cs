// Decompiled with JetBrains decompiler
// Type: Kiosk.MainWindow
// Assembly: Kiosk, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: C3E32FFD-072D-4F9D-AAE4-A7F2B29E989A
// Assembly location: E:\kiosk\Kiosk.exe

using BarcodeLib;
using CefSharp;
using CefSharp.WinForms;
using CoreAudioApi;
using FluorineFx.Messaging.Api.Service;
using FluorineFx.Net;
using GLib;
using GLib.Config;
using GLib.Devices;
using Kiosk.Properties;
using Microsoft.Win32;
using NiiPrinterCLib;
using RawInput_dll;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Management;
using System.Media;
using System.Net;
using System.Net.NetworkInformation;
using System.Printing;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using System.Xml;

namespace Kiosk
{
  public class MainWindow : Form, IPendingServiceCallback
  {
    private static ManagementScope oManagementScope = (ManagementScope) null;
    private int doReset = 0;
    private string Web_V2_A = "tel";
    private string Web_V2_B = "menu";
    private MMDevice device = (MMDevice) null;
    public int Banner_On = 0;
    public int MenuGames = 0;
    private int ErrorEnLogin = 0;
    private int ErrorJamp = 0;
    private int controlcredits = 0;
    private int Emu_Mouse_Click = 0;
    private int Emu_Mouse_RClick = 0;
    public DLG_ValidarTicket ValidacioTicket = (DLG_ValidarTicket) null;
    public DLG_TicketCredits ValidacioTicketCredits = (DLG_TicketCredits) null;
    public DLG_TimeOut ValidacioTimeOut = (DLG_TimeOut) null;
    private bool oldStopCredits = false;
    private bool CoolStartUp = true;
    public bool AdminEnabled = false;
    private DLG_Login LoginAdmin = (DLG_Login) null;
    private DLG_Config ConfigAdmin = (DLG_Config) null;
    public bool nofocus = false;
    private string errlink = "";
    public int TiempoAviso = 30;
    private SoundPlayer snd_alarma = (SoundPlayer) null;
    private int cnttimerCredits = 10;
    private int Ticket_Add_Credits = 0;
    private int Ticket_Add_Cadeau = 0;
    public InsertCredits InsertCreditsDLG = (InsertCredits) null;
    private int anm_apla = 0;
    private int cnttime = 0;
    private Control_CCTALK_COIN cct2 = (Control_CCTALK_COIN) null;
    private Control_Comestero rm5 = (Control_Comestero) null;
    private Control_NV_SSP ssp = (Control_NV_SSP) null;
    private Control_NV_SSP_P6 ssp3 = (Control_NV_SSP_P6) null;
    private Control_NV_SIO sio = (Control_NV_SIO) null;
    private Control_F40_CCTalk f40 = (Control_F40_CCTalk) null;
    private Control_Trilogy tri = (Control_Trilogy) null;
    private DLG_Registro login_dlg = (DLG_Registro) null;
    private DRIVER_Ticket_ICT dispenser = (DRIVER_Ticket_ICT) null;
    private int _sem_timerPoll_Tick = 0;
    public int ErrorNet = 0;
    public int ErrorDevices = 0;
    private int DelayInt = 0;
    private int cntInt = 0;
    private int FiltreCnt = 0;
    private string tmp_ip = "";
    private string tmp_user = "";
    private string tmp_pw = "";
    private string tmp_w = "";
    private int needEnable = 0;
    private int needEnableM = 1;
    private bool enpoolint = false;
    private int waitneedEnable = 0;
    private int waitneedEnableM = 0;
    private int TimeoutCredits = 0;
    private int TimeoutHome = 0;
    private Decimal old_credits_gratuits = new Decimal(-1);
    private short mHotKeyId = 0;
    private IntPtr hookID = IntPtr.Zero;
    private bool _khook = false;
    private int CtrlScroll = 0;
    public int errorcreditsserv = 0;
    private int ErrorNetServer = 0;
    public int cnterror = 0;
    private int delayreconnect = 0;
    private DLG_Message_Full dlg_playforticket = (DLG_Message_Full) null;
    private DLG_Message_Full dlg_msg_printer = (DLG_Message_Full) null;
    private DLG_Dispenser_Out dlg_checks = (DLG_Dispenser_Out) null;
    public int Error_Servidor = 0;
    public MainWindow.Hook_Srv_KioskCommand _Hook_Srv_KioskCommand = (MainWindow.Hook_Srv_KioskCommand) null;
    public MainWindow.Hook_Srv_KioskSetTime _Hook_Srv_KioskSetTime = (MainWindow.Hook_Srv_KioskSetTime) null;
    public MainWindow.Hook_Srv_KioskGetTime _Hook_Srv_KioskGetTime = (MainWindow.Hook_Srv_KioskGetTime) null;
    public MainWindow.Hook_Srv_Verificar_Ticket _Hook_Srv_Verificar_Ticket = (MainWindow.Hook_Srv_Verificar_Ticket) null;
    public MainWindow.Hook_Srv_Anular_Ticket _Hook_Srv_Anular_Ticket = (MainWindow.Hook_Srv_Anular_Ticket) null;
    public MainWindow.Hook_Srv_Credits _Hook_Srv_Credits = (MainWindow.Hook_Srv_Credits) null;
    public MainWindow.Hook_Srv_Sub_Credits _Hook_Srv_Sub_Credits = (MainWindow.Hook_Srv_Sub_Credits) null;
    public MainWindow.Hook_Srv_Sub_Cadeaux _Hook_Srv_Sub_Cadeaux = (MainWindow.Hook_Srv_Sub_Cadeaux) null;
    public MainWindow.Hook_Srv_Add_Credits _Hook_Srv_Add_Credits = (MainWindow.Hook_Srv_Add_Credits) null;
    public MainWindow.Hook_Srv_Login _Hook_Srv_Login = (MainWindow.Hook_Srv_Login) null;
    public MainWindow.Hook_Srv_Sub_Ticket _Hook_Srv_Sub_Ticket = (MainWindow.Hook_Srv_Sub_Ticket) null;
    public MainWindow.Hook_Srv_Add_Ticket _Hook_Srv_Add_Ticket = (MainWindow.Hook_Srv_Add_Ticket) null;
    public MainWindow.Hook_Srv_Add_Pay _Hook_Srv_Add_Pay = (MainWindow.Hook_Srv_Add_Pay) null;
    private int _Last_Id = -1;
    private int[] Cache_Tickets = new int[0];
    private byte _ESCPOS_FU = 128;
    private byte _ESCPOS_FE = 8;
    private byte _ESCPOS_FDW = 32;
    private byte _ESCPOS_FDH = 16;
    private byte _ESCPOS_AL = 0;
    private byte _ESCPOS_AC = 1;
    private byte _ESCPOS_AR = 2;
    private int _MMVol = 50;
    private string dbglog = "";
    private string errtick = "";
    private IContainer components = (IContainer) null;
    private const int SM_TABLETPC = 86;
    private const int APPCOMMAND_VOLUME_MUTE = 524288;
    private const int APPCOMMAND_VOLUME_UP = 655360;
    private const int APPCOMMAND_VOLUME_DOWN = 589824;
    private const int WM_APPCOMMAND = 793;
    private const int MOUSEEVENTF_ABSOLUTE = 32768;
    private const int MOUSEEVENTF_LEFTDOWN = 2;
    private const int MOUSEEVENTF_LEFTUP = 4;
    private const int MOUSEEVENTF_MIDDLEDOWN = 32;
    private const int MOUSEEVENTF_MIDDLEUP = 64;
    private const int MOUSEEVENTF_MOVE = 1;
    private const int MOUSEEVENTF_RIGHTDOWN = 8;
    private const int MOUSEEVENTF_RIGHTUP = 16;
    private const int MOUSEEVENTF_WHEEL = 2048;
    private const int MOUSEEVENTF_XDOWN = 128;
    private const int MOUSEEVENTF_XUP = 256;
    private const int USE_ALT = 1;
    private const int USE_CTRL = 2;
    private const int USE_SHIFT = 4;
    private const int USE_WIN = 8;
    private const int WH_KEYBOARD_LL = 13;
    private const int WM_KEYUP = 257;
    private const int WM_SYSKEYUP = 261;
    private const int WM_KEYDOWN = 256;
    private const int WM_SYSKEYDOWN = 260;
    private const int WM_MOUSEMOVE = 512;
    private const int VK_SHIFT = 16;
    private const int VK_CONTROL = 17;
    private const int VK_PAUSE = 19;
    private const int VK_MENU = 18;
    private const int VK_CAPITAL = 20;
    private const int VK_SCROLL = 145;
    private const int VK_LCONTROL = 162;
    private const int VK_RCONTROL = 163;
    private const int VK_LWIN = 91;
    private const int VK_RWIN = 92;
    private const int VK_HOME = 36;
    private const int VK_ALT = 18;
    private const int VK_DELETE = 46;
    private const int VK_DELETEN = 109;
    private const int VK_INSERT = 45;
    private const int VK_F12 = 123;
    private const int VK_F1 = 112;
    private const int VK_F2 = 113;
    private const int VK_PRINT = 42;
    private const int WM_DEVICECHANGE = 537;
    private const int DEVICE_NOTIFY_ALL_INTERFACE_CLASSES = 4;
    private const int WM_POWERBROADCAST = 536;
    private const int PBT_APMPOWERSTATUSCHANGE = 10;
    private const int PBT_APMRESUMEAUTOMATIC = 18;
    private const int PBT_APMRESUMESUSPEND = 7;
    private const int PBT_APMSUSPEND = 4;
    private const int PBT_POWERSETTINGCHANGE = 32787;
    public Impresora Imp;
    private int StartupDetect;
    private int TimeoutPubli;
    public string Last_COM;
    public string Last_DRV;
    private Bitmap Img_Banner1;
    private Bitmap Img_Banner2;
    private bool tabletEnabled;
    private PipeServer _pipeServer;
    public Publicitat publi;
    private RawInput _rawinput;
    public string TicketToCheck;
    public int TicketOK;
    public string LastTicket;
    public DLG_Calibrar _DCalibrar;
    public string RedirectNews;
    private int DelayServer;
    private string ErrorGenericText;
    private RemoteSharedObject _sharedObject;
    public bool ForceCheck;
    public MainWindow ControlWin;
    public Configuracion Opcions;
    public ChromiumWebBrowser navegador;
    private MainWindow.Fases Status;
    private string _WebLink;
    private bool DisplayKeyboard;
    private int Windows_MidaY;
    public string errors;
    private DateTime ltime;
    public int LastErrorNet;
    public DLG_Sponsors _DLG_Sponsors;
    private MainWindow.HookHandlerDelegate proc;
    private NetConnection _netConnection;
    public static RemoteSharedObject _sharedObjectPagos;
    private MainWindow.Srv_Command _Srv_Commad;
    private bool alamaramute;
    private string web_versio;
    private string web_vnc;
    private string web_date;
    private string[] ban_users;
    private string[] ban_mac;
    private string[] ban_ip;
    private string[] ban_country;
    private Panel pMenu;
    private TextBox tURL;
    private Label iSponsor;
    private Button bKeyboard;
    private Button bGo;
    private Button bForward;
    private Button bBack;
    private Button bHome;
    private System.Windows.Forms.Timer timerCredits;
    private System.Windows.Forms.Timer timerPoll;
    private System.Windows.Forms.Timer timerStartup;
    private Label lcdClock;
    private ImageList imgLed;
    private Label pScreenSaver;
    private Panel pTicket;
    private Button bTicket;
    private Panel pNavegation;
    private Panel pLogin;
    private Button bLogin;
    private Panel pTemps;
    private Panel pKeyboard;
    private Button bBar;
    private Panel pTime;
    private Button bTime;
    private Label eCredits;
    private Button bVUp;
    private Button bVDown;
    private Button bMute;
    private System.Windows.Forms.Timer timerPrinter;
    private Panel rtest;
    private PictureBox pInsertCoin;
    public Panel EntraJocs;
    private System.Windows.Forms.Timer timerMessages;
    private Label lCGRAT;
    private Label lCALL;
    private Panel pGETTON;
    private Button bGETTON;
    private Label lGETTON;

    public MainWindow(ref Configuracion _opc)
    {
      this.tabletEnabled = MainWindow.GetSystemMetrics(86) != 0;
      this.Opcions = _opc;
      this.Lock_Rotacio_Intel();
      this.Set_Dpi_100();
      this.Opcions.__ModoTablet = 0;
      bool flag = true;
      this.Opcions.Enable_Lectors = -1;
      this.Opcions.Temps = 0;
      this.Opcions.CancelTemps = 0;
      this.Opcions.TempsDeTicket = this.Opcions.Temps;
      this.Opcions.Credits = new Decimal(0);
      this.Opcions.Add_Credits = new Decimal(0);
      this.Opcions.Sub_Credits = new Decimal(0);
      this.Opcions.Send_Add_Credits = new Decimal(0);
      this.Opcions.Send_Sub_Credits = new Decimal(0);
      this.Opcions.ForceGoConfig = false;
      this.Opcions.InGame = false;
      this.RedirectNews = "";
      this.cntInt = 0;
      this.StartupDetect = 0;
      this.ErrorJamp = 0;
      this.ErrorEnLogin = 0;
      this.TimeoutPubli = 0;
      this.controlcredits = 0;
      this.snd_alarma = (SoundPlayer) null;
      this.ForceCheck = false;
      this._sem_timerPoll_Tick = 0;
      this.AdminEnabled = false;
      this.Status = MainWindow.Fases.StartUp;
      this.DisplayKeyboard = false;
      MainWindow.ShowCursor(true);
      this._DCalibrar = (DLG_Calibrar) null;
      this.DelayServer = 100;
      this.Last_DRV = "-";
      this.Last_COM = "?";
      this.Check_Windows_Mode();
      int num1 = 0;
      int index1 = -1;
      for (int index2 = 0; index2 < this.Opcions.Web_Zone.Length; ++index2)
      {
        if (this.Opcions.Srv_Ip == this.Opcions.Web_Zone[index2])
          num1 = 1;
        if (string.IsNullOrEmpty(this.Opcions.Web_Zone[index2]) && index1 == -1)
          index1 = index2;
      }
      if (num1 == 0 && index1 >= 0)
      {
        this.Opcions.Web_Zone[index1] = this.Opcions.Srv_Ip;
        this.Opcions.Save_Net();
      }
      int num2 = 0;
      int index3 = -1;
      for (int index2 = 0; index2 < this.Opcions.Web_Zone.Length; ++index2)
      {
        if (this.Opcions.Srv_Web_Ip == this.Opcions.Web_Zone[index2])
          num2 = 1;
        if (string.IsNullOrEmpty(this.Opcions.Web_Zone[index2]) && index3 == -1)
          index3 = index2;
      }
      if (num2 == 0 && index3 >= 0)
      {
        this.Opcions.Web_Zone[index3] = this.Opcions.Srv_Web_Ip;
        this.Opcions.Save_Net();
      }
      this.Opcions.Web_Zone[0] = "gserver" + this.Opcions.Srv_port + "." + this.Opcions.Get_Domain(this.Opcions.Srv_Ip);
      try
      {
        foreach (string directory in Directory.GetDirectories(Path.GetTempPath(), "scoped_dir*"))
        {
          try
          {
            Directory.Delete(directory, true);
          }
          catch
          {
          }
        }
      }
      catch
      {
      }
      string path1 = "c:\\kiosk\\cache";
      if (Directory.Exists(path1))
      {
        try
        {
          Directory.Delete(path1, true);
        }
        catch
        {
        }
      }
      this.ErrorNetServer = 0;
      try
      {
        this._netConnection = new NetConnection();
        this._netConnection.OnConnect += new ConnectHandler(this._netConnection_OnConnect);
        this._netConnection.OnDisconnect += new DisconnectHandler(this._netConnection_OnDisconnect);
        this._netConnection.NetStatus += new NetStatusHandler(this._netConnection_NetStatus);
      }
      catch
      {
      }
      this.InitializeComponent();
      this.SuspendLayout();
      this.Keyboard_Hook();
      flag = false;
      if (!Environment.MachineName.ToLower().Contains("cinto"))
      {
        MainWindow.TaskManager_Off();
      }
      else
      {
        int num3 = (int) MessageBox.Show("STOP MODE DEBUG OFF");
      }
      AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(MainWindow.CurrentDomain_UnhandledException);
      this.Install_Keyboard_Driver();
      CefSettings cefSettings = new CefSettings()
      {
        LogSeverity = LogSeverity.Disable,
        CefCommandLineArgs = {
          {
            "enable-npapi",
            "1"
          }
        }
      };
      cefSettings.CefCommandLineArgs["enable-system-flash"] = "1";
      cefSettings.CachePath = path1;
      string path2 = "c:\\kiosk\\pepflashplayer.dll";
      if (System.IO.File.Exists(path2))
      {
        cefSettings.CefCommandLineArgs.Add("ppapi-flash-path", path2);
      }
      else
      {
        string path3 = Application.StartupPath + "\\pepflashplayer.dll";
        if (System.IO.File.Exists(path3))
          cefSettings.CefCommandLineArgs.Add("ppapi-flash-path", path3);
      }
      do
      {
        Cef.Initialize(cefSettings);
      }
      while (!Cef.IsInitialized);
      BrowserSettings browserSettings = new BrowserSettings()
      {
        Javascript = CefState.Enabled,
        Plugins = CefState.Enabled,
        JavascriptAccessClipboard = CefState.Disabled
      };
      this.Img_Banner1 = new Bitmap("data\\baner_fr.png");
      this.Img_Banner2 = new Bitmap("data\\baner2_fr.png");
      this.EntraJocs.BackgroundImage = (Image) this.Img_Banner1;
      this.navegador = new ChromiumWebBrowser("about:void");
      this.navegador.Dock = DockStyle.Fill;
      this.navegador.Location = new Point(0, 50);
      this.navegador.Name = nameof (navegador);
      this.navegador.BackColor = System.Drawing.Color.DeepSkyBlue;
      this.navegador.MenuHandler = (IContextMenuHandler) new MainWindow.MenuHandler();
      this.navegador.TabIndex = 14;
      this.navegador.JsDialogHandler = (IJsDialogHandler) new MainWindow.JsDialogHandler();
      this.navegador.Move += new EventHandler(this.Move_Screesaver);
      MainWindow.NavIRequestHandler navIrequestHandler = new MainWindow.NavIRequestHandler((IWebBrowser) this.navegador, this);
      MainWindow.LifeSpanHandler lifeSpanHandler = new MainWindow.LifeSpanHandler();
      this.Controls.Clear();
      this.Controls.Add((Control) this.lCALL);
      this.Controls.Add((Control) this.pScreenSaver);
      this.Controls.Add((Control) this.navegador);
      this.Controls.Add((Control) this.EntraJocs);
      this.Controls.Add((Control) this.pMenu);
      this.ResumeLayout(false);
      this.Opcions.Enable_Lectors = -1;
      this.Location = Screen.PrimaryScreen.Bounds.Location;
      flag = true;
      this.Bounds = Screen.PrimaryScreen.Bounds;
      this.ControlWin = this;
      this.Windows_MidaY = this.Height;
      this.Modo_Kiosk_On();
      this.navegador.Focus();
      this.Configurar_Splash(0);
      this.cnttimerCredits = 11;
      this.Error_Servidor = 0;
      this.ErrorNet = 0;
      this.cntInt = 0;
      this.ErrorDevices = 0;
      this.CoolStartUp = true;
      this.Opcions.Credits = new Decimal(0);
      this.Opcions.TimeNavigate = false;
      this.cnttime = 0;
      this.Hide_Browser_Nav();
      this.Start_Temps();
      this.Status = MainWindow.Fases.Reset;
      this.doReset = 0;
      if (this.Opcions.News != 1)
      {
        flag = true;
        this.Start_Service("http://" + this.Opcions.Srv_Web_Ip + "/DemoQuiosk.aspx");
      }
      else
      {
        flag = true;
        this.Start_Service("http://" + this.Opcions.Srv_Web_Ip + "/" + this.Opcions.Srv_Web_Page);
      }
      this.Render_Bar_Menu();
      this.device = (MMDevice) null;
      if (this.Opcions.modo_XP == 0)
      {
        try
        {
          this.device = new MMDeviceEnumerator().GetDefaultAudioEndpoint(EDataFlow.eRender, ERole.eMultimedia);
        }
        catch (Exception ex)
        {
        }
      }
      if (this.Opcions.News != 1)
      {
        this.Find_Selector();
        this.Find_Printer();
      }
      else
      {
        this.Opcions.ModoTickets = 0;
        this.pMenu.Visible = false;
      }
      if (this.Opcions.Publi.ToLower().Contains("http:") || this.Opcions.Publi.ToLower().Contains("https:"))
        this.Opcions.Publi = "";
      this.Opcions.SEND_Mail("STARTUP", "POWER ON");
      flag = true;
      if (this.Opcions.__ModoTablet == 1)
      {
        this.Opcions.VersionPRG += "TAB";
        this._pipeServer = new PipeServer();
        this._pipeServer.PipeMessage += new DelegateMessage(this.PipesMessageHandler);
      }
      flag = true;
      this.load_web("http://" + this.Opcions.Srv_Web_Ip + "/__check.html", 0);
      this.Opcions.SpyUser(this.Opcions.RemoteParam);
      this.Imp = new Impresora();
      this.Imp.Add_Impressora("*CUSTOM", 3, 3, true);
      this.Imp.Add_Impressora("*NII", 3, 3, true);
      this.Imp.Add_Impressora("*SANEI", 3, 3, true);
      this.Imp.Add_Impressora("*STAR", 3, 3, true);
    }

    private void Reconnect_Service()
    {
      if (this._netConnection != null)
        return;
      try
      {
        this._netConnection = new NetConnection();
        this._netConnection.OnConnect += new ConnectHandler(this._netConnection_OnConnect);
        this._netConnection.OnDisconnect += new DisconnectHandler(this._netConnection_OnDisconnect);
        this._netConnection.NetStatus += new NetStatusHandler(this._netConnection_NetStatus);
      }
      catch
      {
      }
    }

    private void Install_Keyboard_Driver()
    {
      this._rawinput = (RawInput) null;
      try
      {
        this._rawinput = new RawInput(this.Handle);
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show("OBJ (Main): " + ex.Message);
      }
      if (this._rawinput == null)
        return;
      this._rawinput.CaptureOnlyIfTopMostWindow = false;
      this._rawinput.KeyPressed += new RawKeyboard.DeviceEventHandler(this.OnKeyPressed);
      this._rawinput.MouseMove += new RawKeyboard.DeviceEventHandler(this.OnMouseMove);
    }

    private void Block_Accessibility()
    {
      Exception exception;
      bool flag;
      try
      {
        Registry.CurrentUser.OpenSubKey("Control Panel\\Accessibility\\StickyKeys", true).SetValue("Flags", (object) "506", RegistryValueKind.String);
      }
      catch (Exception ex)
      {
        exception = ex;
        flag = true;
      }
      try
      {
        Registry.CurrentUser.OpenSubKey("Control Panel\\Accessibility\\Keyboard Response", true).SetValue("Flags", (object) "122", RegistryValueKind.String);
      }
      catch (Exception ex)
      {
        exception = ex;
        flag = true;
      }
      try
      {
        Registry.CurrentUser.OpenSubKey("Control Panel\\Accessibility\\ToggleKeys", true).SetValue("Flags", (object) "58", RegistryValueKind.String);
      }
      catch (Exception ex)
      {
        exception = ex;
        flag = true;
      }
      try
      {
        Registry.Users.OpenSubKey(".DEFAULT\\Control Panel\\Accessibility\\StickyKeys", true).SetValue("Flags", (object) "506", RegistryValueKind.String);
      }
      catch (Exception ex)
      {
        exception = ex;
        flag = true;
      }
      try
      {
        Registry.Users.OpenSubKey(".DEFAULT\\Control Panel\\Accessibility\\Keyboard Response", true).SetValue("Flags", (object) "122", RegistryValueKind.String);
      }
      catch (Exception ex)
      {
        exception = ex;
        flag = true;
      }
      try
      {
        Registry.Users.OpenSubKey(".DEFAULT\\Control Panel\\Accessibility\\ToggleKeys", true).SetValue("Flags", (object) "58", RegistryValueKind.String);
      }
      catch (Exception ex)
      {
        exception = ex;
        flag = true;
      }
    }

    private void Block_AltF4()
    {
      if (this.Opcions.__ModoTablet == 1)
        return;
      Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
      if (!System.IO.File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.System) + "\\kbfmgr.exe"))
        return;
      try
      {
        using (Process process = Process.Start("kbfmgr.exe", "/enable"))
        {
          Thread.Sleep(500);
          process.WaitForExit();
        }
      }
      catch
      {
      }
    }

    private void ReInstall_Keyboard_Driver()
    {
      if (this._rawinput != null)
      {
        this._rawinput.ReleaseHandle();
        this._rawinput = (RawInput) null;
      }
      this.Install_Keyboard_Driver();
    }

    [DllImport("user32.dll")]
    private static extern int GetSystemMetrics(int nIndex);

    private void PipesMessageHandler(string message)
    {
      if (!message.ToLower().Contains("admin"))
        return;
      try
      {
        if (this.Opcions.Running)
        {
          this.Status = MainWindow.Fases.Reset;
          this.doReset = 0;
        }
        this.Opcions.RunConfig = true;
        this.AdminEnabled = true;
      }
      catch
      {
      }
    }

    private void Reset_CEF()
    {
      this.SuspendLayout();
      if (Cef.IsInitialized)
        Cef.Shutdown();
      try
      {
        foreach (string directory in Directory.GetDirectories(Path.GetTempPath(), "scoped_dir*"))
        {
          try
          {
            Directory.Delete(directory, true);
          }
          catch
          {
          }
        }
      }
      catch
      {
      }
      CefSettings cefSettings = new CefSettings();
      cefSettings.LogSeverity = LogSeverity.Disable;
      do
      {
        Cef.Initialize(cefSettings);
      }
      while (!Cef.IsInitialized);
      this.ResumeLayout(false);
    }

    public void Check_Windows_Mode()
    {
      bool flag1 = true;
      bool flag2 = false;
      try
      {
        RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\WinLogon", true);
        string str = registryKey.GetValue("Shell").ToString();
        registryKey.Close();
        if (!str.ToLower().Contains("loader.exe".ToLower()))
          flag2 = true;
      }
      catch
      {
      }
      int num = 0;
      try
      {
        RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\System", true);
        num = int.Parse(registryKey.GetValue("DisableTaskMgr").ToString());
        registryKey.Close();
      }
      catch
      {
      }
      if (flag2)
        this.Lock_Windows();
      else if (num == 1)
      {
        this.Lock_Windows();
      }
      else
      {
        if (MessageBox.Show("Enable Kiosk Mode?", "WARNNING", MessageBoxButtons.YesNo) != DialogResult.Yes)
          return;
        this.Lock_Windows();
        Process.Start("shutdown.exe", "/r /t 2");
        while (true)
        {
          Application.DoEvents();
          flag1 = true;
        }
      }
    }

    public void Lock_Rotacio_Intel()
    {
      try
      {
        RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("Software\\Intel\\Display\\igfxcui\\HotKeys", true);
        registryKey.SetValue("Enable", (object) 0, RegistryValueKind.DWord);
        registryKey.Close();
      }
      catch
      {
      }
    }

    public void Set_Dpi_100()
    {
      try
      {
        RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("Control Panel\\Desktop", true);
        registryKey.SetValue("LogPixels", (object) 96, RegistryValueKind.DWord);
        registryKey.Close();
      }
      catch
      {
      }
      try
      {
        RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("Control Panel\\Accessibility\\HighContrast", true);
        registryKey.SetValue("Flags", (object) 122, RegistryValueKind.DWord);
        registryKey.Close();
      }
      catch
      {
      }
      try
      {
        RegistryKey registryKey = Registry.Users.OpenSubKey(".DEFAULT\\Control Panel\\Accessibility\\HighContrast", true);
        registryKey.SetValue("Flags", (object) 122, RegistryValueKind.DWord);
        registryKey.Close();
      }
      catch
      {
      }
    }

    public void UnLock_Windows()
    {
      try
      {
        RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("SYSTEM\\CurrentControlSet\\Control\\Keyboard Layout", true);
        registryKey.DeleteValue("Scancode Map");
        registryKey.Close();
      }
      catch
      {
      }
      try
      {
        RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\Explorer", true);
        registryKey.SetValue("NoWinKeys", (object) 0, RegistryValueKind.DWord);
        registryKey.Close();
      }
      catch
      {
      }
      try
      {
        RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\Explorer", true);
        registryKey.SetValue("NoWinKeys", (object) 0, RegistryValueKind.DWord);
        registryKey.Close();
      }
      catch
      {
      }
      try
      {
        RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\Explorer", true);
        registryKey.SetValue("NoViewContextMenu", (object) 0, RegistryValueKind.DWord);
        registryKey.Close();
      }
      catch
      {
      }
      try
      {
        RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\Explorer", true);
        registryKey.SetValue("NoViewContextMenu", (object) 0, RegistryValueKind.DWord);
        registryKey.Close();
      }
      catch
      {
      }
      try
      {
        RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\System", true);
        registryKey.SetValue("DisableTaskMgr", (object) 0, RegistryValueKind.DWord);
        registryKey.Close();
      }
      catch
      {
      }
      try
      {
        RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\System", true);
        registryKey.SetValue("NoDesktop", (object) 0, RegistryValueKind.DWord);
        registryKey.Close();
      }
      catch
      {
      }
      try
      {
        using (Process process = Process.Start("kbfmgr.exe", "/disable"))
          process.WaitForExit();
      }
      catch
      {
      }
    }

    public void Lock_Windows()
    {
      try
      {
        RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("SYSTEM\\CurrentControlSet\\Control\\Keyboard Layout", true);
        byte[] numArray = new byte[24]
        {
          (byte) 0,
          (byte) 0,
          (byte) 0,
          (byte) 0,
          (byte) 0,
          (byte) 0,
          (byte) 0,
          (byte) 0,
          (byte) 3,
          (byte) 0,
          (byte) 0,
          (byte) 0,
          (byte) 0,
          (byte) 0,
          (byte) 91,
          (byte) 224,
          (byte) 0,
          (byte) 0,
          (byte) 92,
          (byte) 224,
          (byte) 0,
          (byte) 0,
          (byte) 0,
          (byte) 0
        };
        registryKey.SetValue("Scancode Map", (object) numArray, RegistryValueKind.Binary);
        registryKey.Close();
      }
      catch
      {
      }
      try
      {
        RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\Explorer", true);
        registryKey.SetValue("NoWinKeys", (object) 1, RegistryValueKind.DWord);
        registryKey.Close();
      }
      catch
      {
      }
      try
      {
        RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\Explorer", true);
        registryKey.SetValue("NoWinKeys", (object) 1, RegistryValueKind.DWord);
        registryKey.Close();
      }
      catch
      {
      }
      try
      {
        RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\Explorer", true);
        registryKey.SetValue("NoViewContextMenu", (object) 1, RegistryValueKind.DWord);
        registryKey.Close();
      }
      catch
      {
      }
      try
      {
        RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\Explorer", true);
        registryKey.SetValue("NoViewContextMenu", (object) 1, RegistryValueKind.DWord);
        registryKey.Close();
      }
      catch
      {
      }
      try
      {
        RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\System", true);
        registryKey.SetValue("DisableTaskMgr", (object) 1, RegistryValueKind.DWord);
        registryKey.Close();
      }
      catch
      {
      }
      try
      {
        RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\System", true);
        registryKey.SetValue("NoDesktop", (object) 1, RegistryValueKind.DWord);
        registryKey.Close();
      }
      catch
      {
      }
      try
      {
        RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\WinLogon", true);
        registryKey.SetValue("Shell", (object) "c:\\kiosk\\loader.exe", RegistryValueKind.String);
        registryKey.Close();
      }
      catch
      {
      }
      try
      {
        using (Process process = Process.Start("kbfmgr.exe", "/enable"))
          process.WaitForExit();
      }
      catch
      {
      }
    }

    [DllImport("user32.dll")]
    public static extern IntPtr SendMessageW(
      IntPtr hWnd,
      int Msg,
      IntPtr wParam,
      IntPtr lParam);

    public static void TaskManager_Off()
    {
    }

    [DllImport("user32.dll")]
    private static extern int ShowCursor(bool bShow);

    public int Parser_Ticket(string _user, string _add, int _remove)
    {
      this.TicketToCheck += _add;
      while (!string.IsNullOrEmpty(this.TicketToCheck) && (this.TicketToCheck[0] != '4' && this.TicketToCheck[0] != '7'))
        this.TicketToCheck = this.TicketToCheck.Remove(0, 1);
      if (this.TicketToCheck.Length < 14)
        return 0;
      int num = Gestion.Check_Mod10(_user, this.TicketToCheck);
      if (num > 0)
      {
        this.LastTicket = this.TicketToCheck.Substring(0, 14);
        if (_remove == 1)
        {
          this.TicketToCheck = this.TicketToCheck.Remove(0, 14);
          this.TicketToCheck = "";
        }
        this.TicketOK = 1;
        return num;
      }
      this.LastTicket = "";
      this.TicketToCheck = this.TicketToCheck.Remove(0, 1);
      this.TicketOK = 0;
      return this.Parser_Ticket(_user, "", 1);
    }

    private void OnMouseMove(object sender, InputEventArg e)
    {
      this.Opcions.LastMouseMove = DateTime.Now;
      this.TimeoutHome = 0;
    }

    public void Validate_Ticket()
    {
      this.TicketOK = 0;
      int num = this.Parser_Ticket(this.Opcions.Srv_User, "", 0);
      if (this.Opcions.ModoTickets != 1 || this.TicketOK != 1 || this.Opcions.InGame || !this.Opcions.Running || num != 2)
        return;
      this.TicketToCheck = this.TicketToCheck.Remove(0, 14);
      int _ticket = 0;
      int _id = 0;
      Gestion.Decode_Mod10(this.LastTicket, out _ticket, out _id);
      this.Opcions.TicketTemps = _ticket;
      this.Opcions.IdTicketTemps = _id;
      if (this.Opcions.IdTicketTemps > 0)
        this.Srv_Sub_Ticket(this.Opcions.IdTicketTemps, 0);
    }

    private void OnKeyPressed(object sender, InputEventArg e)
    {
      this.Opcions.LastMouseMove = DateTime.Now;
      this.TimeoutHome = 0;
      bool flag1 = false;
      switch (e.KeyPressEvent.Message)
      {
        case 256:
          if (e.KeyPressEvent.VKey == 9 && e.KeyPressEvent.Flag == 32 || e.KeyPressEvent.VKey == 27 && e.KeyPressEvent.Flag == 32 || (e.KeyPressEvent.VKey == 27 && e.KeyPressEvent.Flag == 0 || e.KeyPressEvent.VKey == 91 && e.KeyPressEvent.Flag == 3) || (e.KeyPressEvent.VKey == 92 && e.KeyPressEvent.Flag == 3 || e.KeyPressEvent.VKey == 91 && e.KeyPressEvent.Flag == 2 || (e.KeyPressEvent.VKey == 92 && e.KeyPressEvent.Flag == 2 || e.KeyPressEvent.VKey == 37 && e.KeyPressEvent.Flag == 3)) || (e.KeyPressEvent.VKey == 38 && e.KeyPressEvent.Flag == 3 || e.KeyPressEvent.VKey == 39 && e.KeyPressEvent.Flag == 3 || (e.KeyPressEvent.VKey == 40 && e.KeyPressEvent.Flag == 3 || e.KeyPressEvent.VKey == 37 && e.KeyPressEvent.Flag == 2) || (e.KeyPressEvent.VKey == 38 && e.KeyPressEvent.Flag == 2 || e.KeyPressEvent.VKey == 39 && e.KeyPressEvent.Flag == 2 || e.KeyPressEvent.VKey == 40 && e.KeyPressEvent.Flag == 2)) || e.KeyPressEvent.Flag == 32)
          {
            flag1 = true;
            break;
          }
          break;
        case 257:
          if (e.KeyPressEvent.VKey == 9 && e.KeyPressEvent.Flag == 32 || e.KeyPressEvent.VKey == 27 && e.KeyPressEvent.Flag == 32 || (e.KeyPressEvent.VKey == 27 && e.KeyPressEvent.Flag == 0 || e.KeyPressEvent.VKey == 91 && e.KeyPressEvent.Flag == 3) || (e.KeyPressEvent.VKey == 92 && e.KeyPressEvent.Flag == 3 || e.KeyPressEvent.VKey == 91 && e.KeyPressEvent.Flag == 2 || (e.KeyPressEvent.VKey == 92 && e.KeyPressEvent.Flag == 2 || e.KeyPressEvent.VKey == 37 && e.KeyPressEvent.Flag == 3)) || (e.KeyPressEvent.VKey == 38 && e.KeyPressEvent.Flag == 3 || e.KeyPressEvent.VKey == 39 && e.KeyPressEvent.Flag == 3 || (e.KeyPressEvent.VKey == 40 && e.KeyPressEvent.Flag == 3 || e.KeyPressEvent.VKey == 37 && e.KeyPressEvent.Flag == 2) || (e.KeyPressEvent.VKey == 38 && e.KeyPressEvent.Flag == 2 || e.KeyPressEvent.VKey == 39 && e.KeyPressEvent.Flag == 2 || e.KeyPressEvent.VKey == 40 && e.KeyPressEvent.Flag == 2)) || e.KeyPressEvent.Flag == 32)
          {
            flag1 = true;
            break;
          }
          break;
        case 260:
          if (e.KeyPressEvent.VKey == 9 && e.KeyPressEvent.Flag == 32 || e.KeyPressEvent.VKey == 27 && e.KeyPressEvent.Flag == 32 || (e.KeyPressEvent.VKey == 27 && e.KeyPressEvent.Flag == 0 || e.KeyPressEvent.VKey == 91 && e.KeyPressEvent.Flag == 3) || (e.KeyPressEvent.VKey == 92 && e.KeyPressEvent.Flag == 3 || e.KeyPressEvent.VKey == 91 && e.KeyPressEvent.Flag == 2 || (e.KeyPressEvent.VKey == 92 && e.KeyPressEvent.Flag == 2 || e.KeyPressEvent.VKey == 37 && e.KeyPressEvent.Flag == 3)) || (e.KeyPressEvent.VKey == 38 && e.KeyPressEvent.Flag == 3 || e.KeyPressEvent.VKey == 39 && e.KeyPressEvent.Flag == 3 || (e.KeyPressEvent.VKey == 40 && e.KeyPressEvent.Flag == 3 || e.KeyPressEvent.VKey == 37 && e.KeyPressEvent.Flag == 2) || (e.KeyPressEvent.VKey == 38 && e.KeyPressEvent.Flag == 2 || e.KeyPressEvent.VKey == 39 && e.KeyPressEvent.Flag == 2 || e.KeyPressEvent.VKey == 40 && e.KeyPressEvent.Flag == 2)) || e.KeyPressEvent.Flag == 32)
          {
            flag1 = true;
            break;
          }
          break;
        case 261:
          if (e.KeyPressEvent.VKey == 9 && e.KeyPressEvent.Flag == 32 || e.KeyPressEvent.VKey == 27 && e.KeyPressEvent.Flag == 32 || (e.KeyPressEvent.VKey == 27 && e.KeyPressEvent.Flag == 0 || e.KeyPressEvent.VKey == 91 && e.KeyPressEvent.Flag == 3) || (e.KeyPressEvent.VKey == 92 && e.KeyPressEvent.Flag == 3 || e.KeyPressEvent.VKey == 91 && e.KeyPressEvent.Flag == 2 || (e.KeyPressEvent.VKey == 92 && e.KeyPressEvent.Flag == 2 || e.KeyPressEvent.VKey == 37 && e.KeyPressEvent.Flag == 3)) || (e.KeyPressEvent.VKey == 38 && e.KeyPressEvent.Flag == 3 || e.KeyPressEvent.VKey == 39 && e.KeyPressEvent.Flag == 3 || (e.KeyPressEvent.VKey == 40 && e.KeyPressEvent.Flag == 3 || e.KeyPressEvent.VKey == 37 && e.KeyPressEvent.Flag == 2) || (e.KeyPressEvent.VKey == 38 && e.KeyPressEvent.Flag == 2 || e.KeyPressEvent.VKey == 39 && e.KeyPressEvent.Flag == 2 || e.KeyPressEvent.VKey == 40 && e.KeyPressEvent.Flag == 2)) || e.KeyPressEvent.Flag == 32)
          {
            flag1 = true;
            break;
          }
          break;
      }
      if (flag1)
      {
        e.KeyPressEvent.Remove = true;
      }
      else
      {
        string[] strArray1 = e.KeyPressEvent.DeviceName.Split('#');
        if (strArray1.Length >= 2)
        {
          string[] strArray2 = strArray1[1].Split('&');
          if (!string.IsNullOrEmpty(strArray2[0]))
            this.Opcions.Last_Device = strArray2[0];
        }
        this.Opcions.LastMouseMove = DateTime.Now;
        this.TimeoutHome = 0;
        if (this.Opcions.Test_Barcode == 1 && (e.KeyPressEvent.Message == 256U && (e.KeyPressEvent.Flag & 1) == 0))
        {
          KeysConverter keysConverter = new KeysConverter();
          switch (e.KeyPressEvent.VKeyName)
          {
            case "D1":
            case "D2":
            case "D3":
            case "D4":
            case "D5":
            case "D6":
            case "D7":
            case "D8":
            case "D9":
            case "D0":
              this.Parser_Ticket(this.Opcions.Srv_User, keysConverter.ConvertToString((object) e.KeyPressEvent.VKey), 0);
              e.KeyPressEvent.Remove = true;
              return;
            case "ENTER":
              this.Parser_Ticket(this.Opcions.Srv_User, "", 1);
              e.KeyPressEvent.Remove = false;
              return;
          }
        }
        bool flag2;
        if (this.Opcions.ModoPS2 == 0 && this.Opcions.ForceAllKey == 0 && (e.KeyPressEvent.DeviceName.Contains(this.Opcions.Barcode) && !string.IsNullOrEmpty(this.Opcions.Barcode)))
        {
          if (e.KeyPressEvent.Message == 256U && (e.KeyPressEvent.Flag & 1) == 0)
          {
            KeysConverter keysConverter = new KeysConverter();
            switch (e.KeyPressEvent.VKeyName)
            {
              case "D1":
              case "D2":
              case "D3":
              case "D4":
              case "D5":
              case "D6":
              case "D7":
              case "D8":
              case "D9":
              case "D0":
                this.Parser_Ticket(this.Opcions.Srv_User, keysConverter.ConvertToString((object) e.KeyPressEvent.VKey), 0);
                break;
              case "ENTER":
                this.TicketOK = 0;
                int num = this.Parser_Ticket(this.Opcions.Srv_User, "", 0);
                if (this.Opcions.ModoTickets == 1 && this.TicketOK == 1 && !this.Opcions.InGame && this.Opcions.Running)
                {
                  this.TicketToCheck = this.TicketToCheck.Remove(0, 14);
                  if (num == 1)
                  {
                    int _ticket = 0;
                    int _id = 0;
                    Gestion.Decode_Mod10(this.LastTicket, out _ticket, out _id);
                    this.Opcions.TicketTemps = _ticket;
                    this.Opcions.IdTicketTemps = _id;
                    if (this.Opcions.IdTicketTemps > 0)
                      this.Srv_Sub_Ticket(this.Opcions.IdTicketTemps, 0);
                  }
                  if (num == 2)
                  {
                    flag2 = false;
                    if (this.ValidacioTicket == null)
                    {
                      this.ValidacioTicket = new DLG_ValidarTicket(ref this.Opcions);
                      this.ValidacioTicket.MWin = this;
                    }
                    if (this.ValidacioTicket.IsDisposed)
                    {
                      this.ValidacioTicket = new DLG_ValidarTicket(ref this.Opcions);
                      this.ValidacioTicket.MWin = this;
                    }
                    this.ValidacioTicket.Ticket = this.LastTicket;
                    this.Opcions.Verificar_Ticket = 0;
                    try
                    {
                      this.Opcions.Verificar_Ticket = int.Parse(this.LastTicket.Substring(1, 11));
                    }
                    catch
                    {
                    }
                    this.Srv_Verificar_Ticket(this.Opcions.Verificar_Ticket, 0);
                    this.ValidacioTicket.Show();
                  }
                  break;
                }
                break;
            }
          }
          e.KeyPressEvent.Remove = true;
        }
        this.doReset = 0;
        switch (e.KeyPressEvent.Message)
        {
          case 256:
          case 260:
            switch (e.KeyPressEvent.VKeyName)
            {
              case "SCROLL":
                flag2 = true;
                break;
              case "PAUSE":
                this.CtrlScroll |= 1;
                if (this.Opcions.Running && this.CtrlScroll == 3)
                {
                  this.Status = MainWindow.Fases.Reset;
                  break;
                }
                break;
              case "F8":
                this.CtrlScroll |= 8;
                if (this.Opcions.Running && (this.CtrlScroll == 3 || this.CtrlScroll == 24 || this.CtrlScroll == 6))
                {
                  this.Status = MainWindow.Fases.Reset;
                  break;
                }
                break;
              case "F5":
                this.CtrlScroll |= 8;
                if (!this.Opcions.Running && (this.Status != MainWindow.Fases.WaitCalibrar && this.Status != MainWindow.Fases.Calibrar))
                {
                  this.Status = MainWindow.Fases.Calibrar;
                  break;
                }
                break;
              case "F9":
                this.CtrlScroll |= 16;
                if (this.Opcions.Running && (this.CtrlScroll == 3 || this.CtrlScroll == 24 || this.CtrlScroll == 6))
                {
                  this.Status = MainWindow.Fases.Reset;
                  break;
                }
                break;
              case "F16":
                this.Opcions.RunConfig = true;
                this.AdminEnabled = true;
                this.Status = MainWindow.Fases.Reset;
                break;
              case "F3":
                this.CtrlScroll |= 4;
                if (this.Opcions.Running && (this.CtrlScroll == 3 || this.CtrlScroll == 24 || this.CtrlScroll == 6))
                {
                  this.Status = MainWindow.Fases.Reset;
                  break;
                }
                break;
              case "F1":
                this.CtrlScroll |= 2;
                if (this.Opcions.Running && (this.CtrlScroll == 3 || this.CtrlScroll == 24 || this.CtrlScroll == 6))
                {
                  this.Status = MainWindow.Fases.Reset;
                  break;
                }
                break;
              case "LWIN":
              case "RWIN":
                e.KeyPressEvent.Remove = true;
                break;
              case "LCONTROL":
              case "RCONTROL":
              case "F10":
                if (this.AdminEnabled)
                {
                  this.Opcions.RunConfig = true;
                  this.BackColor = System.Drawing.Color.Blue;
                  break;
                }
                break;
            }
            break;
          case 257:
          case 261:
            switch (e.KeyPressEvent.VKeyName)
            {
              case "PAUSE":
                this.CtrlScroll &= 1;
                break;
              case "F1":
                this.CtrlScroll &= 2;
                break;
              case "F3":
                this.CtrlScroll &= 4;
                break;
              case "F8":
                this.CtrlScroll &= 8;
                break;
              case "F9":
                this.CtrlScroll &= 16;
                break;
              case "LWIN":
              case "RWIN":
                e.KeyPressEvent.Remove = true;
                break;
              case "LCONTROL":
              case "RCONTROL":
                if (this.AdminEnabled && !this.Opcions.RunConfig)
                {
                  this.Opcions.RunConfig = true;
                  this.BackColor = System.Drawing.Color.Blue;
                  break;
                }
                break;
            }
            break;
        }
        if (!this.Opcions.Emu_Mouse)
          return;
        switch (e.KeyPressEvent.Message)
        {
          case 256:
          case 260:
            switch (e.KeyPressEvent.VKeyName)
            {
              case "F12":
                if (this.Emu_Mouse_Click == 0)
                {
                  MainWindow.POINT lpPoint;
                  MainWindow.GetCursorPos(out lpPoint);
                  MainWindow.mouse_event(32770, lpPoint.X, lpPoint.Y, 0, 0);
                  this.Emu_Mouse_Click = 1;
                  break;
                }
                break;
              case "F11":
                if (this.Emu_Mouse_Click == 0 && this.Opcions.Emu_Mouse_RClick)
                {
                  MainWindow.POINT lpPoint;
                  MainWindow.GetCursorPos(out lpPoint);
                  MainWindow.mouse_event(32776, lpPoint.X, lpPoint.Y, 0, 0);
                  this.Emu_Mouse_Click = 1;
                  break;
                }
                break;
              case "LEFT":
                MainWindow.POINT lpPoint1;
                MainWindow.GetCursorPos(out lpPoint1);
                int x1 = lpPoint1.X;
                lpPoint1.X -= 8;
                if (lpPoint1.X < 0)
                  lpPoint1.X = 0;
                if (x1 != lpPoint1.X)
                {
                  MainWindow.SetCursorPos(lpPoint1.X, lpPoint1.Y);
                  break;
                }
                break;
              case "RIGHT":
                MainWindow.POINT lpPoint2;
                MainWindow.GetCursorPos(out lpPoint2);
                int x2 = lpPoint2.X;
                lpPoint2.X += 8;
                if (x2 != lpPoint2.X)
                {
                  MainWindow.SetCursorPos(lpPoint2.X, lpPoint2.Y);
                  break;
                }
                break;
              case "UP":
                MainWindow.POINT lpPoint3;
                MainWindow.GetCursorPos(out lpPoint3);
                int y1 = lpPoint3.Y;
                lpPoint3.Y -= 8;
                if (lpPoint3.Y < 0)
                  lpPoint3.Y = 0;
                if (y1 != lpPoint3.Y)
                {
                  MainWindow.SetCursorPos(lpPoint3.X, lpPoint3.Y);
                  break;
                }
                break;
              case "DOWN":
                MainWindow.POINT lpPoint4;
                MainWindow.GetCursorPos(out lpPoint4);
                int y2 = lpPoint4.Y;
                lpPoint4.Y += 8;
                if (y2 != lpPoint4.Y)
                {
                  MainWindow.SetCursorPos(lpPoint4.X, lpPoint4.Y);
                  break;
                }
                break;
            }
            break;
          case 257:
          case 261:
            switch (e.KeyPressEvent.VKeyName)
            {
              case "F12":
                if (this.Emu_Mouse_Click == 1)
                {
                  MainWindow.POINT lpPoint5;
                  MainWindow.GetCursorPos(out lpPoint5);
                  MainWindow.mouse_event(32774, lpPoint5.X, lpPoint5.Y, 0, 0);
                  this.Emu_Mouse_Click = 0;
                  break;
                }
                break;
              case "F11":
                if (this.Emu_Mouse_Click == 1 && this.Emu_Mouse_RClick == 1)
                {
                  MainWindow.POINT lpPoint5;
                  MainWindow.GetCursorPos(out lpPoint5);
                  MainWindow.mouse_event(32784, lpPoint5.X, lpPoint5.Y, 0, 0);
                  this.Emu_Mouse_Click = 0;
                  break;
                }
                break;
            }
            break;
        }
      }
    }

    public bool Es_Pot_Navegar()
    {
      return this.Opcions.Temps > 0 && (this.Opcions.ForceLogin != 1 || this.Opcions.Logged);
    }

    public void Render_Bar_Menu()
    {
      if (this.Opcions.FreeGames == 0)
      {
        this.bTime.Height = 30;
        this.eCredits.Visible = true;
      }
      else
      {
        this.bTime.Height = 43;
        this.eCredits.Visible = false;
      }
      switch (this.Opcions.ModoKiosk)
      {
        case 0:
          this.pLogin.Visible = false;
          this.pTicket.Visible = false;
          this.pNavegation.Visible = true;
          this.pGETTON.Visible = false;
          this.pTemps.Visible = true;
          this.oldStopCredits = this.Opcions.StopCredits;
          break;
        case 1:
          if (this.Opcions.ForceLogin == 1)
            this.pLogin.Visible = true;
          else
            this.pLogin.Visible = false;
          this.pTime.Visible = false;
          this.pTicket.Visible = false;
          this.pTemps.Left = 0;
          if (this.Opcions.ModoTickets == 1)
          {
            this.pTicket.Left = this.pTemps.Width;
            this.pNavegation.Left = this.pTemps.Width + this.pTicket.Width;
            this.pNavegation.Width = this.Width - this.pNavegation.Left - this.pKeyboard.Width;
            this.pGETTON.Left = this.pTemps.Width + this.pTicket.Width;
            this.pGETTON.Width = this.Width - this.pGETTON.Left - this.pKeyboard.Width;
            this.pTicket.Visible = true;
          }
          else if (this.Opcions.News == 2)
          {
            this.pNavegation.Left = this.pTemps.Width;
            this.pNavegation.Width = this.Width - this.pNavegation.Left - this.pKeyboard.Width;
            if (this.Opcions.ModoTickets == 0)
            {
              this.pTemps.Visible = false;
              this.pNavegation.Left = 0;
              this.pNavegation.Width = this.Width - this.pNavegation.Left - this.pKeyboard.Width;
            }
            this.pGETTON.Left = this.pTemps.Width;
            this.pGETTON.Width = this.Width - this.pGETTON.Left - this.pKeyboard.Width;
            if (this.Opcions.ModoTickets == 0)
            {
              this.pTemps.Visible = false;
              this.pGETTON.Left = 0;
              this.pGETTON.Width = this.Width - this.pGETTON.Left - this.pKeyboard.Width;
            }
          }
          else
          {
            this.pTime.Left = this.pTemps.Width;
            this.pNavegation.Left = this.pTemps.Width + this.pTime.Width;
            this.pNavegation.Width = this.Width - this.pNavegation.Left - this.pKeyboard.Width;
            this.pGETTON.Left = this.pTemps.Width + this.pTime.Width;
            this.pGETTON.Width = this.Width - this.pGETTON.Left - this.pKeyboard.Width;
            this.pTime.Visible = true;
          }
          this.pNavegation.Visible = true;
          this.pGETTON.Visible = false;
          if (this.Opcions.News == 2 && this.Opcions.ModoTickets == 0)
            this.pTemps.Visible = false;
          else
            this.pTemps.Visible = true;
          this.pInsertCoin.Image = !this.Opcions.StopCredits ? (Image) null : (Image) Resources.insertcoin2_off;
          this.oldStopCredits = this.Opcions.StopCredits;
          break;
        case 2:
          this.pLogin.Visible = true;
          this.pTicket.Visible = false;
          this.pNavegation.Visible = true;
          this.pGETTON.Visible = false;
          this.pTemps.Visible = true;
          this.oldStopCredits = this.Opcions.StopCredits;
          break;
      }
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
        this.ErrorGenericText = ex.Message;
        return false;
      }
      return true;
    }

    private static void CurrentDomain_UnhandledException(
      object sender,
      UnhandledExceptionEventArgs e)
    {
      Exception exceptionObject = e.ExceptionObject as Exception;
      int num1 = (int) MessageBox.Show("ERROR (CurrentDomain_UnhandledException) ... " + (object) e);
      if (null == exceptionObject)
        return;
      int num2 = (int) MessageBox.Show(exceptionObject.Message);
    }

    private void Activar_Modo_Config()
    {
      this.Configurar_Splash(0);
      this.Status = MainWindow.Fases.Reset;
      this.doReset = 0;
    }

    public void GoGames()
    {
      this.CloseOSK();
      TimeSpan timeSpan;
      //ref TimeSpan local = ref timeSpan;
      DateTime now = DateTime.Now;
      int hour = now.Hour;
      now = DateTime.Now;
      int minute = now.Minute;
      now = DateTime.Now;
      int second = now.Second;
      now = DateTime.Now;
      int millisecond = now.Millisecond;
      timeSpan = new TimeSpan(hour, minute, second, millisecond);
      this.nofocus = true;
      bool flag;
      if (this.Opcions.ModoKiosk == 0)
      {
        if (this.Opcions.News != 1)
        {
          flag = true;
          this.WebLink = "http://".ToLower() + this.Opcions.Srv_Web_Ip + "/MenuGame.aspx?t=" + this.Opcions.Srv_User + "," + this.Opcions.Srv_User_P + ",MenuGame.aspx," + (object) timeSpan.TotalMilliseconds;
        }
        else
        {
          flag = true;
          this.WebLink = "http://".ToLower() + this.Opcions.Srv_Web_Ip + "/" + this.Opcions.Srv_Web_Page + "?t=" + this.Opcions.Srv_User + "," + this.Opcions.Srv_User_P + ",Default.aspx," + (object) timeSpan.TotalMilliseconds;
        }
      }
      else if (this.Opcions.ModoTickets == 1)
      {
        if (this.Opcions.News != 1)
        {
          if (this.Opcions.Srv_Web_Ip.ToLower().Contains(this.Web_V2_A) || this.Opcions.Srv_Web_Ip.ToLower().Contains(this.Web_V2_B))
          {
            flag = true;
            this.WebLink = "http://".ToLower() + this.Opcions.Srv_Web_Ip + "/StartGameQuiosk.aspx?t=" + this.Opcions.Srv_User + "," + this.Opcions.Srv_User_P + ",tickquioscV2.aspx," + (object) timeSpan.TotalMilliseconds;
          }
          else
          {
            flag = true;
            this.WebLink = "http://".ToLower() + this.Opcions.Srv_Web_Ip + "/StartGameQuiosk.aspx?t=" + this.Opcions.Srv_User + "," + this.Opcions.Srv_User_P + ",tickquiosc.aspx," + (object) timeSpan.TotalMilliseconds;
          }
        }
        else
        {
          flag = true;
          this.WebLink = "http://".ToLower() + this.Opcions.Srv_Web_Ip + "/" + this.Opcions.Srv_Web_Page + "?t=" + this.Opcions.Srv_User + "," + this.Opcions.Srv_User_P + ",Default.aspx," + (object) timeSpan.TotalMilliseconds;
        }
      }
      else if (this.Opcions.News != 1)
      {
        flag = true;
        this.WebLink = "http://".ToLower() + this.Opcions.Srv_Web_Ip + "/StartGameQuiosk.aspx?t=" + this.Opcions.Srv_User + "," + this.Opcions.Srv_User_P + ",DemoQuiosk.aspx," + (object) timeSpan.TotalMilliseconds;
      }
      else
      {
        flag = true;
        this.WebLink = "http://".ToLower() + this.Opcions.Srv_Web_Ip + "/" + this.Opcions.Srv_Web_Page + "?t=" + this.Opcions.Srv_User + "," + this.Opcions.Srv_User_P + ",Default.aspx," + (object) timeSpan.TotalMilliseconds;
      }
      this.nofocus = false;
    }

    private void Close_Devices()
    {
      this.Opcions.Enable_Lectors = -1;
      if (this.ssp3 != null)
      {
        this.ssp3.Close();
        this.ssp3 = (Control_NV_SSP_P6) null;
      }
      if (this.ssp != null)
      {
        this.ssp.Close();
        this.ssp = (Control_NV_SSP) null;
      }
      if (this.sio != null)
      {
        this.sio.Close();
        this.sio = (Control_NV_SIO) null;
      }
      if (this.rm5 != null)
      {
        this.rm5.Close();
        this.rm5 = (Control_Comestero) null;
      }
      if (this.cct2 != null)
      {
        this.cct2.Close();
        this.cct2 = (Control_CCTALK_COIN) null;
      }
      if (this.tri != null)
      {
        this.tri.Close();
        this.tri = (Control_Trilogy) null;
      }
      if (this.f40 == null)
        return;
      this.f40.Close();
      this.f40 = (Control_F40_CCTalk) null;
    }

    private void timerStartup_Tick(object sender, EventArgs e)
    {
      this.timerStartup.Enabled = false;
    }

    private int Control_Zone()
    {
      string webLink = this._WebLink;
      if (string.IsNullOrEmpty(webLink))
        return -1;
      string[] strArray = webLink.ToLower().Split('/');
      return strArray[0].ToLower() == "file:".ToLower() || strArray[0].ToLower() == "ftps:".ToLower() || strArray[0].ToLower() == "ftp:".ToLower() || strArray[0].ToLower() == "about:blank".ToLower() || !(strArray[0].ToLower() == "http:".ToLower()) && !(strArray[0].ToLower() == "https:".ToLower()) ? -1 : 0;
    }

    public bool Detectar_Zona_Temps()
    {
      if (this.Control_Zone() < 0 || this.Opcions.ForceLogin == 1 && !this.Opcions.Logged)
        return false;
      int browserBarOn = this.Opcions.BrowserBarOn;
      string webLink = this._WebLink;
      webLink.Split(':');
      string[] strArray = webLink.Split('/');
      for (int index = 0; index < this.Opcions.Web_Zone.Length; ++index)
      {
        if (!string.IsNullOrEmpty(this.Opcions.Web_Zone[index]) && strArray[2].ToLower() == this.Opcions.Web_Zone[index].ToLower())
          return false;
      }
      return true;
    }

    public string WebLink
    {
      set
      {
        if (!Cef.IsInitialized || this.navegador == null || !this.navegador.IsBrowserInitialized)
          return;
        string str = value;
        try
        {
          str = str.Replace('\\', '/');
          Uri uri = new Uri(str);
        }
        catch
        {
          str = "http://".ToLower() + str;
        }
        try
        {
          string scheme = new Uri(str).Scheme;
          switch (this.Status)
          {
            case MainWindow.Fases.GoHome:
            case MainWindow.Fases.Home:
              if (scheme.ToLower() != "http".ToLower() && scheme.ToLower() != "https".ToLower() && (this.Opcions.FreeGames != 0 || !(scheme.ToLower() == "about".ToLower())))
                return;
              break;
            default:
              if (scheme.ToLower() != "http".ToLower() && scheme.ToLower() != "https".ToLower())
                return;
              break;
          }
          this._WebLink = str;
          this.navegador.Load(str);
          if (!this.nofocus)
            this.navegador.Focus();
          this.nofocus = false;
        }
        catch (Exception ex)
        {
          this.errlink = ex.Message;
        }
      }
      get
      {
        return this._WebLink;
      }
    }

    public static bool IsNetworkAvailable()
    {
      return MainWindow.IsNetworkAvailable(0L);
    }

    [DllImport("wininet.dll")]
    private static extern bool InternetGetConnectedState(int Description, int ReservedValue);

    public static bool IsNetworkAvailable(long minimumSpeed)
    {
      return MainWindow.InternetGetConnectedState(0, 0);
    }

    private void SoundCredits()
    {
      using (SoundPlayer soundPlayer = new SoundPlayer(Environment.CurrentDirectory + "\\data\\points.wav"))
        soundPlayer.Play();
    }

    private void SoundTest()
    {
      using (SoundPlayer soundPlayer = new SoundPlayer(Environment.CurrentDirectory + "\\data\\test.wav"))
        soundPlayer.Play();
    }

    private void SoundAlarm()
    {
      if (this.snd_alarma == null)
      {
        this.SoundRestore();
        this.snd_alarma = new SoundPlayer(Environment.CurrentDirectory + "\\data\\alarma.wav");
      }
      this.SoundMax();
      this.snd_alarma.Stop();
      this.snd_alarma.PlayLooping();
    }

    private void Show_Browser_Nav()
    {
      this.tURL.Visible = true;
      this.bBack.Visible = true;
      this.bForward.Visible = true;
      this.bGo.Visible = true;
    }

    private void Hide_Browser_Nav()
    {
      this.tURL.Visible = false;
      this.bBack.Visible = false;
      this.bForward.Visible = false;
      this.bGo.Visible = false;
    }

    private void Control_Dispenser()
    {
      if (this.Opcions.Disp_Pay_Running == 4 && this.dlg_checks.IsClosed)
      {
        this.Opcions.Disp_Pay_Running = 0;
        this.dlg_checks.Dispose();
        this.dlg_checks = (DLG_Dispenser_Out) null;
        if (this.dispenser != null)
        {
          this.dispenser.Close();
          this.dispenser = (DRIVER_Ticket_ICT) null;
        }
      }
      if (this.Opcions.Disp_Pay_Running == 3 && this._Srv_Commad == MainWindow.Srv_Command.Null)
      {
        this.Opcions.Disp_Pay_Ticket_Fail = 3;
        this.Opcions.Disp_Pay_Running = 4;
        this.Gestio_Pagament(this.Opcions.Disp_Pay_Ticket_Credits, 1);
      }
      if (this.Opcions.Disp_Pay_Running == 30 && this._Srv_Commad == MainWindow.Srv_Command.Null)
        this.Opcions.Disp_Pay_Running = 3;
      if (this.Opcions.Disp_Pay_Running == 20 && this._Srv_Commad == MainWindow.Srv_Command.Null)
      {
        this.Opcions.Disp_Pay_Ticket_Out_Flag = 1;
        this.dispenser.Tickets_Out = 0;
        this.dispenser.Payout((byte) 1);
        this.dispenser.Poll();
        Thread.Sleep(100);
        this.Opcions.Disp_Pay_Running = 2;
      }
      if (this.Opcions.Disp_Pay_Running == 2)
      {
        this.dispenser.Poll();
        if (this.dispenser.Tickets_Out >= 1)
        {
          this.Opcions.Disp_Pay_Ticket_Credits -= this.Opcions.Disp_Val;
          if (this.Opcions.Disp_Pay_Ticket_Credits < 0)
            this.Opcions.Disp_Pay_Ticket_Credits = 0;
          --this.Opcions.Disp_Pay_Ticket;
          if (this.Opcions.Disp_Pay_Ticket < 0)
            this.Opcions.Disp_Pay_Ticket = 0;
          ++this.Opcions.Disp_Pay_Ticket_Out;
          if (this.Opcions.Disp_Pay_Ticket > 0)
          {
            this.dispenser.Tickets_Out = 0;
            this.Srv_Sub_Cadeaux((Decimal) this.Opcions.Disp_Val, 0);
            this.Opcions.Disp_Pay_Running = 20;
          }
          else
          {
            this.dispenser.Tickets_Out = 0;
            this.Opcions.Disp_Pay_Running = 3;
          }
        }
        else if (this.dispenser.EnError)
        {
          ++this.Opcions.Disp_Pay_Ticket_Cnt_Fail;
          if (this.Opcions.Disp_Pay_Ticket_Cnt_Fail > 3)
          {
            this.Opcions.Disp_Pay_Ticket_Fail = 1;
            this.dispenser.Close();
            this.dispenser = (DRIVER_Ticket_ICT) null;
            this.Srv_Sub_Cadeaux((Decimal) (-this.Opcions.Disp_Val), 0);
            this.Opcions.Disp_Pay_Running = 30;
          }
          else
          {
            this.dispenser.Reset();
            this.Opcions.Disp_Pay_Ticket_Fail = 0;
            this.Opcions.Disp_Pay_Ticket_Cnt_Fail = 0;
          }
        }
      }
      if (this.Opcions.Disp_Pay_Running != 1 || this.dlg_checks != null)
        return;
      this.Opcions.Disp_Pay_Ticket_Cnt_Fail = 0;
      this.Opcions.Disp_Pay_Ticket_Fail = 0;
      this.dlg_checks = new DLG_Dispenser_Out(ref this.Opcions);
      this.dlg_checks.Show();
      Application.DoEvents();
      if (this.dispenser == null)
      {
        this.dispenser = new DRIVER_Ticket_ICT();
        this.dispenser.port = this.dispenser.Find_Device();
        this.dispenser.Open();
      }
      if (!this.dispenser.OnLine)
        this.dispenser.Open();
      if (this.dispenser.OnLine)
      {
        this.Opcions.Disp_Pay_Running = 2;
        this.dispenser.Reset();
        Thread.Sleep(100);
        this.dispenser.Poll();
        if (this.Opcions.Disp_Pay_Ticket > 0)
        {
          this.dispenser.Tickets_Out = 0;
          this.Srv_Sub_Cadeaux((Decimal) this.Opcions.Disp_Val, 0);
          this.Opcions.Disp_Pay_Running = 20;
        }
        else
        {
          this.dispenser.Tickets_Out = 0;
          this.Opcions.Disp_Pay_Running = 3;
        }
      }
      else
      {
        this.dispenser.Close();
        this.dispenser = (DRIVER_Ticket_ICT) null;
        this.Opcions.Disp_Pay_Running = 3;
      }
    }

    private void timerCredits_Tick(object sender, EventArgs e)
    {
      this.Control_Dispenser();
      if (this.Banner_On == 2 && this.Opcions.ModoKiosk == 1 && this.EntraJocs != null)
      {
        if (this.Opcions.CancelTemps < 20)
        {
          if ((this.Opcions.CancelTemps & 1) == 1)
          {
            if (this.EntraJocs.BackgroundImage != this.Img_Banner2)
            {
              this.EntraJocs.BackgroundImage = (Image) this.Img_Banner2;
              this.EntraJocs.Invalidate();
            }
          }
          else if (this.EntraJocs.BackgroundImage != this.Img_Banner1)
          {
            this.EntraJocs.BackgroundImage = (Image) this.Img_Banner1;
            this.EntraJocs.Invalidate();
          }
        }
        else if (this.EntraJocs.BackgroundImage != this.Img_Banner1)
        {
          this.EntraJocs.BackgroundImage = (Image) this.Img_Banner1;
          this.EntraJocs.Invalidate();
        }
      }
      if (this.Opcions.ModoTickets == 1)
      {
        if (this.Opcions.Temps > 0)
        {
          if (this._Srv_Commad == MainWindow.Srv_Command.AddTicket)
          {
            try
            {
              if (this.bTicket.Enabled)
                this.bTicket.Enabled = false;
            }
            catch
            {
            }
          }
          else
          {
            try
            {
              if (!this.bTicket.Enabled)
              {
                if (!this.Opcions.InGame && this.MenuGames == 0 && !this._WebLink.Contains("://menu."))
                  this.bTicket.Enabled = true;
              }
              else
              {
                if (this.Opcions.InGame && this.MenuGames == 0)
                  this.bTicket.Enabled = false;
                if (this.MenuGames == 0 && this._WebLink.Contains("://menu."))
                  this.bTicket.Enabled = false;
              }
            }
            catch
            {
            }
          }
        }
        else
        {
          try
          {
            if (this.bTicket.Enabled)
            {
              this.bTicket.Enabled = false;
              if (this.Opcions.ModoKiosk == 1)
              {
                this.EntraJocs.Visible = false;
                this.Banner_On = 0;
              }
            }
            else if (this.EntraJocs.Visible)
            {
              this.EntraJocs.Visible = false;
              this.Banner_On = 0;
            }
          }
          catch
          {
          }
        }
      }
      bool flag;
      if (this._netConnection != null)
      {
        if (!this._netConnection.Connected)
        {
          this.Srv_Connect();
        }
        else
        {
          if (this._Hook_Srv_Verificar_Ticket != null)
          {
            ++this._Hook_Srv_Verificar_Ticket.timeout;
            switch (this._Hook_Srv_Verificar_Ticket.OK)
            {
              case -1:
                if (this._Hook_Srv_Verificar_Ticket.timeout > 100)
                {
                  this._Hook_Srv_Verificar_Ticket.OK = 0;
                  break;
                }
                break;
              case 0:
                this.Srv_Verificar_Ticket(this.Opcions.Verificar_Ticket, 1);
                break;
              case 1:
                this._Hook_Srv_Verificar_Ticket.OK = -2;
                this.Opcions.Ticket_Verificar.Parser(this._Hook_Srv_Verificar_Ticket.Resposta);
                flag = false;
                this.ValidacioTicket.Update_Info();
                this._Srv_Commad = MainWindow.Srv_Command.Null;
                this._Hook_Srv_Verificar_Ticket = (MainWindow.Hook_Srv_Verificar_Ticket) null;
                break;
            }
          }
          if (this._Hook_Srv_Anular_Ticket != null)
          {
            ++this._Hook_Srv_Anular_Ticket.timeout;
            switch (this._Hook_Srv_Anular_Ticket.OK)
            {
              case -1:
                if (this._Hook_Srv_Anular_Ticket.timeout > 100)
                {
                  this._Hook_Srv_Anular_Ticket.OK = 0;
                  break;
                }
                break;
              case 0:
                this.Srv_Anular_Ticket(this._Hook_Srv_Anular_Ticket.ticket, this._Hook_Srv_Anular_Ticket.estat, 1);
                break;
              case 1:
                this._Hook_Srv_Anular_Ticket.OK = -2;
                this._Srv_Commad = MainWindow.Srv_Command.Null;
                this.Srv_Verificar_Ticket(this._Hook_Srv_Anular_Ticket.ticket, 0);
                this._Hook_Srv_Anular_Ticket = (MainWindow.Hook_Srv_Anular_Ticket) null;
                break;
            }
          }
          if (this._Hook_Srv_Add_Pay != null)
          {
            ++this._Hook_Srv_Add_Pay.timeout;
            switch (this._Hook_Srv_Add_Pay.OK)
            {
              case -1:
                if (this._Hook_Srv_Add_Pay.timeout > 100)
                {
                  this._Hook_Srv_Add_Pay.OK = 0;
                  this.Srv_Add_Pay(1);
                  break;
                }
                break;
              case 0:
                this.Srv_Add_Pay(1);
                break;
              case 1:
                this._Hook_Srv_Add_Pay.OK = -2;
                if (!string.IsNullOrEmpty(this._Hook_Srv_Add_Pay.Resposta) && this.Opcions.Ticket_Pago.Parser(this._Hook_Srv_Add_Pay.Resposta) > 0)
                {
                  this.Opcions.Pagar_Ticket_ID = this.Opcions.Ticket_Pago.Ticket;
                  int _join = this.Opcions.JoinTicket;
                  if (this.Opcions.Temps <= 0)
                    _join = 0;
                  if (this.Opcions.Disp_Enable == 1)
                    this.Ticket_Out_Check(this.Opcions.Impresora_Tck, (Decimal) this.Opcions.Ticket_Pago.Pago, this.Opcions.Ticket_Pago.Ticket, this.Opcions.Ticket_Model, this.Opcions.Ticket_Cut, this.Opcions.Ticket_N_FEED, this.Opcions.Ticket_N_HEAD, this.Opcions.Ticket_60mm, this.Opcions.Ticket_Pago.DataT, this.Opcions.Ticket_Pago.CRC, this.Opcions.Disp_Pay_Ticket_Out, _join);
                  else if (this.Opcions.AutoTicketTime == 2 && this.Opcions.Temps > 0)
                  {
                    this.Ticket_Out_Mes_Temps(this.Opcions.Impresora_Tck, (Decimal) this.Opcions.Ticket_Pago.Pago, this.Opcions.Ticket_Pago.Ticket, this.Opcions.Ticket_Model, this.Opcions.Ticket_Cut, this.Opcions.Ticket_N_FEED, this.Opcions.Ticket_N_HEAD, this.Opcions.Ticket_60mm, this.Opcions.Ticket_Pago.DataT, this.Opcions.Ticket_Pago.CRC, 0, this.Opcions.Temps);
                    this.Opcions.Temps = 0;
                    this.Opcions.TempsDeTicket = this.Opcions.Temps;
                    this.Opcions.CancelTemps = 0;
                  }
                  else
                    this.Ticket_Out(this.Opcions.Impresora_Tck, (Decimal) this.Opcions.Ticket_Pago.Pago, this.Opcions.Ticket_Pago.Ticket, this.Opcions.Ticket_Model, this.Opcions.Ticket_Cut, this.Opcions.Ticket_N_FEED, this.Opcions.Ticket_N_HEAD, this.Opcions.Ticket_60mm, this.Opcions.Ticket_Pago.DataT, this.Opcions.Ticket_Pago.CRC, _join);
                }
                this._Srv_Commad = MainWindow.Srv_Command.Null;
                this._Hook_Srv_Add_Pay = (MainWindow.Hook_Srv_Add_Pay) null;
                break;
            }
          }
          if (this._Hook_Srv_Add_Ticket != null)
          {
            ++this._Hook_Srv_Add_Ticket.timeout;
            switch (this._Hook_Srv_Add_Ticket.OK)
            {
              case -1:
                if (this._Hook_Srv_Add_Ticket.timeout > 100)
                {
                  this._Hook_Srv_Add_Ticket.OK = 0;
                  this.Srv_Add_Ticket(this.Opcions.Temps, 1);
                  break;
                }
                break;
              case 0:
                this.Srv_Add_Ticket(this.Opcions.Temps, 1);
                break;
              case 1:
                this._Hook_Srv_Add_Ticket.OK = -2;
                if (this.Ticket(this.Opcions.Impresora_Tck, (Decimal) this.Opcions.ValorTemps, this.Opcions.Temps, this._Hook_Srv_Add_Ticket.Ticket, this.Opcions.Ticket_Model, this.Opcions.Ticket_Cut, this.Opcions.Ticket_N_FEED, this.Opcions.Ticket_N_HEAD, this.Opcions.Ticket_60mm))
                {
                  this.Opcions.Temps = 0;
                  this.Opcions.CancelTemps = 0;
                  this.Opcions.TempsDeTicket = 0;
                }
                this.Status = MainWindow.Fases.GoHome;
                this._Srv_Commad = MainWindow.Srv_Command.Null;
                this._Hook_Srv_Add_Ticket = (MainWindow.Hook_Srv_Add_Ticket) null;
                break;
            }
          }
          if (this._Hook_Srv_Sub_Ticket != null)
          {
            ++this._Hook_Srv_Sub_Ticket.timeout;
            switch (this._Hook_Srv_Sub_Ticket.OK)
            {
              case -1:
                if (this._Hook_Srv_Sub_Ticket.timeout > 100)
                {
                  this._Hook_Srv_Sub_Ticket.OK = 0;
                  this.Srv_Sub_Ticket(this.Opcions.IdTicketTemps, 1);
                  break;
                }
                break;
              case 0:
                this.Opcions.TicketTemps = 0;
                this.Opcions.IdTicketTemps = 0;
                this._Srv_Commad = MainWindow.Srv_Command.Null;
                this._Hook_Srv_Sub_Ticket = (MainWindow.Hook_Srv_Sub_Ticket) null;
                if (this.InsertCreditsDLG != null)
                {
                  this.InsertCreditsDLG.Close();
                  this.InsertCreditsDLG.Dispose();
                  this.InsertCreditsDLG = (InsertCredits) null;
                }
                this.InsertCreditsDLG = new InsertCredits(ref this.Opcions, 1);
                this.InsertCreditsDLG.Show();
                break;
              case 1:
                this._Hook_Srv_Sub_Ticket.OK = -2;
                this.Opcions.Temps += this.Opcions.TicketTemps;
                this.Opcions.TempsDeTicket = this.Opcions.Temps;
                this.Opcions.TicketTemps = 0;
                this._Srv_Commad = MainWindow.Srv_Command.Null;
                this._Hook_Srv_Sub_Ticket = (MainWindow.Hook_Srv_Sub_Ticket) null;
                break;
            }
          }
          if (this._Hook_Srv_Add_Credits != null)
          {
            ++this._Hook_Srv_Add_Credits.timeout;
            switch (this._Hook_Srv_Add_Credits.OK)
            {
              case -1:
                if (this._Hook_Srv_Add_Credits.timeout > 100)
                {
                  this._Hook_Srv_Add_Credits.OK = 0;
                  this.Srv_Add_Credits(this.Opcions.Send_Add_Credits, 1);
                  break;
                }
                break;
              case 0:
                this.Srv_Add_Credits(this.Opcions.Send_Add_Credits, 1);
                break;
              case 1:
                this.Opcions.Send_Add_Credits = new Decimal(0);
                this._Hook_Srv_Add_Credits.OK = -2;
                this._Srv_Commad = MainWindow.Srv_Command.Null;
                this.SoundCredits();
                break;
            }
          }
          if (this._Hook_Srv_Sub_Credits != null)
          {
            ++this._Hook_Srv_Sub_Credits.timeout;
            switch (this._Hook_Srv_Sub_Credits.OK)
            {
              case -1:
                if (this._Hook_Srv_Sub_Credits.timeout > 100)
                {
                  this._Hook_Srv_Sub_Credits.OK = 0;
                  this.Srv_Sub_Credits(this.Opcions.Send_Sub_Credits, 1);
                  break;
                }
                break;
              case 0:
                this.Srv_Sub_Credits(this.Opcions.Send_Sub_Credits, 1);
                break;
              case 1:
                this.Opcions.Send_Sub_Credits = new Decimal(0);
                this._Hook_Srv_Sub_Credits.OK = -2;
                this._Srv_Commad = MainWindow.Srv_Command.Null;
                break;
            }
          }
          if (this._Hook_Srv_Sub_Cadeaux != null)
          {
            ++this._Hook_Srv_Sub_Cadeaux.timeout;
            switch (this._Hook_Srv_Sub_Cadeaux.OK)
            {
              case -1:
                if (this._Hook_Srv_Sub_Cadeaux.timeout > 100)
                {
                  this._Hook_Srv_Sub_Cadeaux.OK = 0;
                  this.Srv_Sub_Cadeaux((Decimal) this.Opcions.Disp_Val, 1);
                  break;
                }
                break;
              case 0:
                this.Srv_Sub_Cadeaux((Decimal) this.Opcions.Disp_Val, 1);
                break;
              case 1:
                this._Hook_Srv_Sub_Cadeaux.OK = -2;
                this._Srv_Commad = MainWindow.Srv_Command.Null;
                break;
            }
          }
          if (this._Hook_Srv_Credits != null)
          {
            ++this._Hook_Srv_Credits.timeout;
            switch (this._Hook_Srv_Credits.OK)
            {
              case -1:
                if (this._Hook_Srv_Credits.timeout > 100)
                {
                  this.errorcreditsserv = 1;
                  this._Hook_Srv_Credits.OK = 0;
                  this.Srv_Credits(1);
                  break;
                }
                break;
              case 0:
                this.errorcreditsserv = 1;
                this.Srv_Credits(1);
                break;
              case 1:
                this._Hook_Srv_Credits.OK = -2;
                if (this._Hook_Srv_Credits.credits >= 0)
                  this.Update_Server_Credits((Decimal) this._Hook_Srv_Credits.credits, true);
                else
                  this.Update_Server_Credits(new Decimal(0), true);
                this.errorcreditsserv = 0;
                this._Srv_Commad = MainWindow.Srv_Command.Null;
                break;
            }
          }
          if (this._Hook_Srv_KioskCommand != null)
          {
            ++this._Hook_Srv_KioskCommand.timeout;
            switch (this._Hook_Srv_KioskCommand.OK)
            {
              case -1:
                if (this._Hook_Srv_KioskCommand.timeout > 100)
                {
                  this._Hook_Srv_KioskCommand.OK = 0;
                  this.Srv_KioskCommand(1);
                  break;
                }
                break;
              case 0:
                this.Srv_KioskCommand(1);
                break;
              case 1:
                this._Hook_Srv_KioskCommand.OK = -2;
                this._Srv_Commad = MainWindow.Srv_Command.Null;
                break;
            }
          }
          if (this._Hook_Srv_KioskSetTime != null)
          {
            ++this._Hook_Srv_KioskSetTime.timeout;
            switch (this._Hook_Srv_KioskSetTime.OK)
            {
              case -1:
                if (this._Hook_Srv_KioskSetTime.timeout > 100)
                {
                  this._Hook_Srv_KioskSetTime.OK = 0;
                  this.Srv_KioskSetTime(this.Opcions.Temps, 1);
                  break;
                }
                break;
              case 0:
                this.Srv_KioskSetTime(this.Opcions.Temps, 1);
                break;
              case 1:
                this._Hook_Srv_KioskSetTime.OK = -2;
                this._Srv_Commad = MainWindow.Srv_Command.Null;
                break;
            }
          }
          if (this._Hook_Srv_KioskGetTime != null)
          {
            ++this._Hook_Srv_KioskGetTime.timeout;
            switch (this._Hook_Srv_KioskGetTime.OK)
            {
              case -1:
                if (this._Hook_Srv_KioskGetTime.timeout > 100)
                {
                  this._Hook_Srv_KioskGetTime.OK = 0;
                  this.Srv_KioskGetTime(1);
                  break;
                }
                break;
              case 0:
                this.Srv_KioskGetTime(1);
                break;
              case 1:
                this._Hook_Srv_KioskGetTime.OK = -2;
                this.Opcions.Temps = this._Hook_Srv_KioskSetTime.segons;
                this._Srv_Commad = MainWindow.Srv_Command.Null;
                break;
            }
          }
          if (this._Hook_Srv_Login != null)
          {
            ++this._Hook_Srv_Login.timeout;
            switch (this._Hook_Srv_Login.OK)
            {
              case -1:
                if (this._Hook_Srv_Login.timeout > 50)
                {
                  flag = true;
                  this._Hook_Srv_Login.OK = 0;
                  this.Srv_Test_Login(this.Opcions.Srv_User, this.Opcions.Srv_User_P, 1);
                  break;
                }
                break;
              case 0:
                flag = true;
                this.Srv_Test_Login(this.Opcions.Srv_User, this.Opcions.Srv_User_P, 1);
                break;
              case 1:
                this._Hook_Srv_Login.OK = -2;
                this._Srv_Commad = MainWindow.Srv_Command.Null;
                break;
            }
          }
          if (this._Srv_Commad == MainWindow.Srv_Command.Null)
          {
            if (string.IsNullOrEmpty(this.Opcions.Srv_Room) && this._Srv_Commad == MainWindow.Srv_Command.Null)
              this.Srv_Get_Room();
            if (this.Opcions.Add_Credits > new Decimal(0) && this.Opcions.Send_Add_Credits == new Decimal(0) && this._Srv_Commad == MainWindow.Srv_Command.Null)
            {
              this.Opcions.Send_Add_Credits = this.Opcions.Add_Credits;
              this.Opcions.Add_Credits -= this.Opcions.Send_Add_Credits;
              this.Srv_Add_Credits(this.Opcions.Send_Add_Credits, 0);
            }
            if (this.Opcions.Sub_Credits > new Decimal(0) && this.Opcions.Send_Sub_Credits == new Decimal(0) && this._Srv_Commad == MainWindow.Srv_Command.Null)
            {
              this.Opcions.Send_Sub_Credits = this.Opcions.Sub_Credits;
              this.Opcions.Sub_Credits -= this.Opcions.Send_Sub_Credits;
              this.Srv_Sub_Credits(this.Opcions.Send_Sub_Credits, 0);
            }
            if (this._Srv_Commad != MainWindow.Srv_Command.Null)
              ;
          }
        }
      }
      ++this.cnttimerCredits;
      if (this.cnttimerCredits < 4)
        return;
      this.cnttimerCredits = 0;
      if (!this.Opcions.Running)
        return;
      if (this.Opcions.InGame)
      {
        if (this.Opcions.ModoTickets != 1)
          this.lcdClock.Visible = false;
      }
      else if (this.Opcions.News == 2 && this.Opcions.ModoTickets == 0)
        this.lcdClock.Visible = false;
      else
        this.lcdClock.Visible = true;
      if (this.Opcions.ComprarTemps == 1 && this.Opcions.News != 2)
      {
        this.Opcions.ComprarTemps = 2;
        this.Stop_Temps();
        new CreditManagerFull(ref this.Opcions).Show();
      }
      if (this.errorcreditsserv == 1)
        this.eCredits.Text = "-";
      else
        this.eCredits.Text = string.Format("{0}", (object) this.Opcions.Credits);
      string str = this.Opcions.Localize.Text("Insert coins");
      if (this.Opcions.ModoTickets == 1)
        str = this.Opcions.Localize.Text("Insert coins or ticket");
      if (this.Opcions.Temps > 0)
      {
        TimeSpan timeSpan = new TimeSpan(0, 0, this.Opcions.Temps);
        str = timeSpan.Hours != 0 ? string.Format(this.Opcions.Localize.Text("Time available") + "\r\n{0}:{1:00}:{2:00}", (object) timeSpan.Hours, (object) timeSpan.Minutes, (object) timeSpan.Seconds) : string.Format(this.Opcions.Localize.Text("Time available") + "\r\n{0}:{1:00}", (object) timeSpan.Minutes, (object) timeSpan.Seconds);
        if (timeSpan.Hours == 0 && timeSpan.Minutes == 0)
          str = string.Format(this.Opcions.Localize.Text("Time available") + "\r\n{0}", (object) timeSpan.Seconds);
        if (this.MenuGames == 1)
          str = this.Opcions.Localize.Text("Free\r\nGames");
      }
      else if (this.Opcions.ModoTickets == 0 && this.Opcions.Credits > new Decimal(0))
        str = this.Opcions.Localize.Text("Buy time");
      this.lcdClock.Text = str;
      if (!this.Es_Pot_Navegar())
        this.Stop_Temps();
      else if (this.Opcions.ModoTickets == 0 && this.Opcions.News == 2 && this.Opcions.Temps > 0)
        this.Show_Browser_Nav();
      if (this.Detectar_Zona_Temps())
      {
        if (this.Opcions.Temps > 0)
          this.Show_Browser_Nav();
      }
      else
        this.Stop_Temps();
      if (this.Opcions.BrowserBarOn == 1)
        this.Show_Browser_Nav();
      if (!this.Opcions.TimeNavigate || this.Opcions.FullScreen != 0)
        return;
      if (this.cnttime == 0)
      {
        this.ltime = DateTime.Now;
        this.cnttime = 1;
      }
      if (this.cnttime == 1)
      {
        DateTime now = DateTime.Now;
        TimeSpan timeSpan = now.Subtract(this.ltime);
        if ((int) timeSpan.TotalSeconds > 0)
        {
          this.ltime = now;
          this.Opcions.CancelTemps += (int) timeSpan.TotalSeconds;
          this.Opcions.Temps -= (int) timeSpan.TotalSeconds;
          if (this.Opcions.TempsDeTicket > 0)
            this.Opcions.TempsDeTicket = this.Opcions.Temps;
        }
      }
      if (this.Opcions.Temps <= 0)
      {
        this.cnttime = 0;
        this.Opcions.Temps = 0;
        this.Opcions.CancelTemps = 0;
        this.Opcions.TempsDeTicket = this.Opcions.Temps;
        this.Status = MainWindow.Fases.GoHome;
        this.Stop_Temps();
        if (!this.Opcions.InGame && this.Opcions.Credits > new Decimal(0) && this.Opcions.CancelTempsOn == 1)
        {
          flag = false;
          this.Srv_Sub_Credits(this.Opcions.Credits, 0);
        }
      }
    }

    public bool PingTest()
    {
      Ping ping = new Ping();
      PingReply pingReply;
      try
      {
        pingReply = ping.Send(IPAddress.Parse("8.8.8.8"));
      }
      catch
      {
        return false;
      }
      return pingReply.Status == IPStatus.Success;
    }

    public bool PingServer()
    {
      Ping ping = new Ping();
      PingReply pingReply;
      try
      {
        pingReply = ping.Send(this.Opcions.Srv_Ip);
      }
      catch
      {
        return false;
      }
      return pingReply.Status == IPStatus.Success;
    }

    private string XLat_Error(int _err, int _errserv, int _errdev)
    {
      int num = _err;
      if (_err == 0 && _errserv == 0)
        num = _errdev;
      if (_err == 0 && _errserv > 0)
        num = _errserv;
      switch (num)
      {
        case 0:
          if (this.ErrorEnLogin == 1)
            return "LOGIN";
          return this.ErrorJamp == 1 ? "JAM" : "OK";
        case 100:
          return "ERROR INTERNET";
        case 101:
          return "ERROR NET";
        case 102:
          return "ERROR ISP";
        case 200:
          return "ERROR USER";
        case 201:
          return "ERROR SERVER";
        case 500:
        case 501:
        case 502:
        case 503:
        case 510:
          return "ERROR CRD [" + (object) num + "]";
        case 504:
          return "LOGIN";
        case 800:
          return "ERROR BNV";
        case 801:
          return "ERROR BNV SSP";
        case 802:
          return "ERROR BNV SIO";
        case 803:
          return "ERROR BNV TRI";
        case 804:
          return "ERROR BNV F40";
        case 805:
          return "ERROR BNV SSP V3";
        case 900:
          return "ERROR COIN";
        case 901:
          return "ERROR RM5";
        case 902:
          return "ERROR CCT02";
        default:
          return "ERROR " + (object) num;
      }
    }

    public void Internal_Add_Credits(int _c)
    {
      this.Internal_Add_Credits((Decimal) _c);
    }

    private void Internal_Add_Credits(Decimal _c)
    {
      this.Opcions.LastMouseMove = DateTime.Now;
      this.Opcions.Add_Credits += _c;
      if (this.Opcions.ModoTickets == 1 || this.Opcions.News == 2)
      {
        this.Opcions.Temps += (int) ((double) (60f / (float) this.Opcions.ValorTemps) * (double) (float) _c);
        if (this.Opcions.TempsDeTicket > 0)
          this.Opcions.TempsDeTicket = this.Opcions.Temps;
      }
      if (this.Opcions.InGame)
        return;
      bool flag;
      string str;
      if (this.Opcions.ModoTickets == 1)
      {
        if (this.Opcions.News != 1)
        {
          if (this.Opcions.Srv_Web_Ip.ToLower().Contains(this.Web_V2_A) || this.Opcions.Srv_Web_Ip.ToLower().Contains(this.Web_V2_B))
          {
            flag = true;
            str = "http://" + this.Opcions.Srv_Web_Ip + "/tickquioscV2.aspx?t=" + this.Opcions.Srv_User + "," + this.Opcions.Srv_User_P + ",tickquioscV2.aspx";
          }
          else
          {
            flag = true;
            str = "http://" + this.Opcions.Srv_Web_Ip + "/tickquiosc.aspx?t=" + this.Opcions.Srv_User + "," + this.Opcions.Srv_User_P + ",tickquiosc.aspx";
          }
        }
        else
        {
          flag = true;
          str = "http://" + this.Opcions.Srv_Web_Ip + "/" + this.Opcions.Srv_Web_Page + "?t=" + this.Opcions.Srv_User + "," + this.Opcions.Srv_User_P + ",Default.aspx";
        }
      }
      else if (this.Opcions.News != 1)
      {
        flag = true;
        str = "http://" + this.Opcions.Srv_Web_Ip + "/DemoQuiosk.aspx?t=" + this.Opcions.Srv_User + "," + this.Opcions.Srv_User_P + ",DemoQuiosk.aspx";
      }
      else
      {
        flag = true;
        str = "http://" + this.Opcions.Srv_Web_Ip + "/" + this.Opcions.Srv_Web_Page + "?t=" + this.Opcions.Srv_User + "," + this.Opcions.Srv_User_P + ",Default.aspx";
      }
      if (!this._WebLink.ToLower().Contains(str.ToLower()))
        ;
    }

    private void Configurar_Splash(int _modo)
    {
      if (this.InsertCreditsDLG != null)
      {
        this.InsertCreditsDLG.Close();
        this.InsertCreditsDLG.Dispose();
        this.InsertCreditsDLG = (InsertCredits) null;
      }
      string str = "";
      this.pScreenSaver.Top = 0;
      this.pScreenSaver.Left = 0;
      bool flag = true;
      this.pScreenSaver.Width = Screen.PrimaryScreen.Bounds.Width;
      if (_modo != 0)
      {
        this.pScreenSaver.Height = Screen.PrimaryScreen.Bounds.Height - 150;
        this.lCALL.Width = this.pScreenSaver.Width;
        this.lCALL.Height = 152;
        this.lCALL.Top = this.pScreenSaver.Height - 1;
        this.lCALL.Left = 0;
        this.lCALL.Text = this.Opcions.Srv_ID_Tlf;
        this.lCALL.Visible = true;
        this.lCALL.Invalidate();
      }
      else
      {
        this.pScreenSaver.Height = Screen.PrimaryScreen.Bounds.Height;
        this.lCALL.Visible = false;
        this.lCALL.Invalidate();
      }
      this.pScreenSaver.BackgroundImageLayout = ImageLayout.Stretch;
      this.pScreenSaver.ForeColor = System.Drawing.Color.White;
      flag = true;
      this.pScreenSaver.Text = "";
      if (_modo == 0)
      {
        Label pScreenSaver = this.pScreenSaver;
        pScreenSaver.Text = pScreenSaver.Text + "Version " + this.Opcions.VersionPRG + "\nStarting...\n";
      }
      else
      {
        str = this.XLat_Error(this.ErrorNet, this.ErrorNetServer, this.ErrorDevices);
        this.pScreenSaver.Text = "(Status:" + this.XLat_Error(this.ErrorNet, this.ErrorNetServer, this.ErrorDevices) + ")\n Version " + this.Opcions.VersionPRG + "\n" + this.pScreenSaver.Text;
      }
      this.pScreenSaver.Text = "[" + this.Opcions.Srv_Ip + " - " + this.Opcions.Srv_port + " - " + this.Opcions.Srv_User + " - " + this.Opcions.IDMAQUINA + "]\n" + this.pScreenSaver.Text;
      switch (_modo)
      {
        case 0:
          this.pScreenSaver.BackColor = System.Drawing.Color.Black;
          if (this.Opcions.ModoKiosk == 1)
          {
            if (this.Opcions.News > 0)
            {
              this.pScreenSaver.BackgroundImage = Image.FromFile(Environment.CurrentDirectory + "\\data\\news.png");
              break;
            }
            this.pScreenSaver.BackgroundImage = Image.FromFile(Environment.CurrentDirectory + "\\data\\logo.jpg");
            break;
          }
          this.pScreenSaver.BackgroundImage = Image.FromFile(Environment.CurrentDirectory + "\\data\\loading.png");
          break;
        case 1:
          this.pScreenSaver.ForeColor = System.Drawing.Color.Black;
          this.pScreenSaver.BackColor = System.Drawing.Color.FromArgb(247, 236, 20);
          this.lCALL.BackColor = System.Drawing.Color.Red;
          this.lCALL.ForeColor = System.Drawing.Color.White;
          if (this.XLat_Error(this.ErrorNet, this.ErrorNetServer, this.ErrorDevices) == "LOGIN")
          {
            this.pScreenSaver.BackgroundImage = Image.FromFile(Environment.CurrentDirectory + "\\data\\uerr.png");
            break;
          }
          if (this.XLat_Error(this.ErrorNet, this.ErrorNetServer, this.ErrorDevices) == "JAM")
          {
            this.SoundAlarm();
            this.pScreenSaver.BackgroundImage = Image.FromFile(Environment.CurrentDirectory + "\\data\\critical.png");
          }
          else
            this.pScreenSaver.BackgroundImage = Image.FromFile(Environment.CurrentDirectory + "\\data\\oos.png");
          break;
        case 2:
          this.lCALL.BackColor = System.Drawing.Color.Yellow;
          this.lCALL.ForeColor = System.Drawing.Color.Black;
          this.pScreenSaver.ForeColor = System.Drawing.Color.Black;
          this.pScreenSaver.BackColor = System.Drawing.Color.Black;
          this.pScreenSaver.BackgroundImage = Image.FromFile(Environment.CurrentDirectory + "\\data\\cfg.png");
          break;
        case 3:
          this.pScreenSaver.ForeColor = System.Drawing.Color.White;
          this.pScreenSaver.BackColor = System.Drawing.Color.Black;
          this.lCALL.BackColor = System.Drawing.Color.Yellow;
          this.lCALL.ForeColor = System.Drawing.Color.Black;
          this.pScreenSaver.BackgroundImage = Image.FromFile(Environment.CurrentDirectory + "\\data\\scr.png");
          break;
        case 10:
          this.pScreenSaver.ForeColor = System.Drawing.Color.White;
          this.pScreenSaver.BackColor = System.Drawing.Color.Black;
          this.lCALL.BackColor = System.Drawing.Color.Yellow;
          this.lCALL.ForeColor = System.Drawing.Color.Black;
          this.pScreenSaver.BackgroundImage = Image.FromFile(Environment.CurrentDirectory + "\\data\\uerr.png");
          break;
        case 100:
          this.pScreenSaver.ForeColor = System.Drawing.Color.White;
          this.pScreenSaver.BackColor = System.Drawing.Color.Black;
          this.lCALL.BackColor = System.Drawing.Color.Yellow;
          this.lCALL.ForeColor = System.Drawing.Color.Black;
          this.pScreenSaver.BackgroundImage = Image.FromFile(Environment.CurrentDirectory + "\\data\\mant.png");
          break;
        case 101:
        case 102:
          this.pScreenSaver.ForeColor = System.Drawing.Color.White;
          this.pScreenSaver.BackColor = System.Drawing.Color.Black;
          this.lCALL.BackColor = System.Drawing.Color.Yellow;
          this.lCALL.ForeColor = System.Drawing.Color.Black;
          this.pScreenSaver.BackgroundImage = Image.FromFile(Environment.CurrentDirectory + "\\data\\reset.png");
          break;
      }
      this.pScreenSaver.Visible = true;
      this.pScreenSaver.Invalidate();
    }

    public void OutOfService()
    {
      if (this.Status == MainWindow.Fases.Register || this.Status == MainWindow.Fases.WaitRegister || (this.Status == MainWindow.Fases.OutOfService || this.Status == MainWindow.Fases.WaitOutOfService))
        return;
      this.Status = MainWindow.Fases.OutOfService;
      if (!Cef.IsInitialized)
        return;
      this.GoWebInicial();
    }

    public void Check_Connection()
    {
      if (MainWindow.IsNetworkAvailable())
      {
        if (this.PingTest())
        {
          this.ErrorNet = 0;
        }
        else
        {
          this.OutOfService();
          this.ErrorNet = 101;
        }
      }
      else
      {
        this.OutOfService();
        this.ErrorNet = 100;
      }
    }

    public void Check_Connection_WD()
    {
      if (MainWindow.IsNetworkAvailable())
        return;
      this.OutOfService();
    }

    public void CloseOSK()
    {
      Process[] processesByName = Process.GetProcessesByName("KVKeyboard");
      if (processesByName == null || processesByName.Length < 1)
        return;
      PipeClient pipeClient = new PipeClient();
      if (pipeClient != null)
      {
        try
        {
          pipeClient.Send("QUIT", "KVKeyboard", 1000);
        }
        catch
        {
        }
      }
    }

    public void ForceKillVNC()
    {
      this.Opcions.ForceSpy = false;
      this.Opcions.Spy = 2;
      string str = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) + "\\uvnc bvba\\UltraVNC\\winvnc.exe";
      if (System.IO.File.Exists(str))
      {
        if (Configuracion.VNC_Running())
          Process.Start(str, "-kill");
        int num = 0;
        while (Configuracion.VNC_Running())
        {
          ++num;
          Thread.Sleep(500);
          if (num == 4)
            Process.Start(str, "-kill");
          if (num > 10)
            break;
        }
      }
      else
      {
        int num1 = (int) MessageBox.Show("No VNC Installed");
      }
    }

    public void ForceChangePassword()
    {
    }

    private void Estat_Servidor_VAR()
    {
      if (string.IsNullOrEmpty(this.Opcions.RemoteCmd))
        return;
      switch (this.Opcions.RemoteCmd.ToUpper())
      {
        case "REMOTE":
          this.doReset = 0;
          this.Opcions.RemoteCmd = "";
          this.Srv_KioskCommand(1);
          for (int index = 0; index < 10; ++index)
          {
            Application.DoEvents();
            Thread.Sleep(100);
          }
          if (this.Opcions.ForceSpy)
          {
            for (int index = 0; index < 5; ++index)
            {
              Application.DoEvents();
              Thread.Sleep(100);
            }
            this.ForceKillVNC();
            for (int index = 0; index < 10; ++index)
            {
              Application.DoEvents();
              Thread.Sleep(100);
            }
          }
          if (!this.Opcions.ForceSpy && this.Opcions.Spy > 0)
          {
            this.Opcions.UserSpy = this.Opcions.Srv_User;
            this.Opcions.SpyUser(this.Opcions.RemoteParam);
            break;
          }
          break;
        case "REMOFF":
          this.doReset = 0;
          this.Opcions.RemoteCmd = "";
          this.Srv_KioskCommand(1);
          for (int index = 0; index < 10; ++index)
          {
            Application.DoEvents();
            Thread.Sleep(100);
          }
          this.ForceKillVNC();
          break;
        case "MODWIN":
          this.doReset = 0;
          this.Opcions.RemoteCmd = "";
          this.Srv_KioskCommand(1);
          for (int index = 0; index < 10; ++index)
          {
            Application.DoEvents();
            Thread.Sleep(100);
          }
          this.ForceModoWindows();
          break;
        case "CHGKEY":
          this.doReset = 0;
          this.Opcions.RemoteCmd = "";
          this.Srv_KioskCommand(1);
          for (int index = 0; index < 10; ++index)
          {
            Application.DoEvents();
            Thread.Sleep(100);
          }
          this.ForceChangePassword();
          break;
        case "SNDBLL":
          this.doReset = 0;
          this.Opcions.RemoteCmd = "";
          this.Srv_KioskCommand(1);
          for (int index = 0; index < 10; ++index)
          {
            Application.DoEvents();
            Thread.Sleep(100);
          }
          break;
        case "PWROFF":
          this.doReset = 0;
          this.Opcions.RemoteCmd = "";
          this.Srv_KioskCommand(1);
          for (int index = 0; index < 10; ++index)
          {
            Application.DoEvents();
            Thread.Sleep(100);
          }
          this.Status = MainWindow.Fases.GoRemoteReset;
          break;
        case "KSKOFF":
          if (this.Opcions.EnManteniment == 0 && this.Opcions.Running)
          {
            this.Opcions.EnManteniment = 1;
            for (int index = 0; index < 10; ++index)
            {
              Application.DoEvents();
              Thread.Sleep(100);
            }
            this.Status = MainWindow.Fases.GoManteniment;
            this.doReset = 0;
            break;
          }
          break;
        case "KSKON":
          this.Opcions.RemoteCmd = "";
          this.Srv_KioskCommand(1);
          if (this.Opcions.EnManteniment == 1 && this.Opcions.Running)
          {
            this.Opcions.EnManteniment = 0;
            for (int index = 0; index < 10; ++index)
            {
              Application.DoEvents();
              Thread.Sleep(100);
            }
            this.Status = MainWindow.Fases.Reset;
            this.doReset = 0;
            break;
          }
          break;
        case "GOCFG":
          this.Opcions.RemoteCmd = "";
          this.Srv_KioskCommand(1);
          for (int index = 0; index < 10; ++index)
          {
            Application.DoEvents();
            Thread.Sleep(100);
          }
          this.Opcions.ForceGoConfig = true;
          this.Status = MainWindow.Fases.Reset;
          break;
      }
    }

    private void Estat_Servidor()
    {
      this.load_web("http://" + this.Opcions.Srv_Web_Ip + "/__check.html", 1);
      if (!string.IsNullOrEmpty(this.web_versio))
      {
        int num = 2261;
        try
        {
          num = int.Parse(this.web_versio);
        }
        catch
        {
        }
        if (2261 < num)
        {
          this.Status = MainWindow.Fases.GoUpdate;
          return;
        }
      }
      if (this.Opcions.ForceReset && this.Status != MainWindow.Fases.RemoteReset)
        this.Status = MainWindow.Fases.GoRemoteReset;
      else if (this.Opcions.ForceManteniment && this.Status != MainWindow.Fases.Manteniment)
      {
        this.Status = MainWindow.Fases.GoManteniment;
      }
      else
      {
        if (this.Opcions.ForceSpy || this.Opcions.Spy <= 0)
          return;
        this.Opcions.SpyUser(this.Opcions.RemoteParam);
      }
    }

    private void GoWebInicial()
    {
      bool flag;
      if (this.Opcions.FreeGames == 1)
      {
        if (this.Opcions.Credits + this.Opcions.Add_Credits > new Decimal(0) && this.errorcreditsserv == 0)
        {
          TimeSpan timeSpan;
//          ref TimeSpan local = ref timeSpan;
          int hour = DateTime.Now.Hour;
          DateTime now = DateTime.Now;
          int minute = now.Minute;
          now = DateTime.Now;
          int second = now.Second;
          now = DateTime.Now;
          int millisecond = now.Millisecond;
          timeSpan = new TimeSpan(hour, minute, second, millisecond);
          if (this.Opcions.ModoTickets == 1)
          {
            if (this.Opcions.News != 1)
            {
              if (this.Opcions.Srv_Web_Ip.ToLower().Contains(this.Web_V2_A) || this.Opcions.Srv_Web_Ip.ToLower().Contains(this.Web_V2_B))
              {
                flag = true;
                this.WebLink = "http://" + this.Opcions.Srv_Web_Ip + "/tickquioscV2.aspx?t=" + this.Opcions.Srv_User + "," + this.Opcions.Srv_User_P + ",tickquioscV2.aspx," + (object) timeSpan.TotalMilliseconds;
              }
              else
              {
                flag = true;
                this.WebLink = "http://" + this.Opcions.Srv_Web_Ip + "/tickquiosc.aspx?t=" + this.Opcions.Srv_User + "," + this.Opcions.Srv_User_P + ",tickquiosc.aspx," + (object) timeSpan.TotalMilliseconds;
              }
            }
            else
            {
              flag = true;
              this.WebLink = "http://" + this.Opcions.Srv_Web_Ip + "/" + this.Opcions.Srv_Web_Page + "?t=" + this.Opcions.Srv_User + "," + this.Opcions.Srv_User_P + "," + this.Opcions.Srv_Web_Page + "," + (object) timeSpan.TotalMilliseconds;
            }
          }
          else if (this.Opcions.News != 1)
          {
            flag = true;
            this.WebLink = "http://" + this.Opcions.Srv_Web_Ip + "/DemoQuiosk.aspx?t=" + this.Opcions.Srv_User + "," + this.Opcions.Srv_User_P + ",DemoQuiosk.aspx," + (object) timeSpan.TotalMilliseconds;
          }
          else
          {
            flag = true;
            this.WebLink = "http://" + this.Opcions.Srv_Web_Ip + "/" + this.Opcions.Srv_Web_Page + "?t=" + this.Opcions.Srv_User + "," + this.Opcions.Srv_User_P + "," + this.Opcions.Srv_Web_Page + "," + (object) timeSpan.TotalMilliseconds;
          }
        }
        else
        {
          TimeSpan timeSpan;
//          ref TimeSpan local = ref timeSpan;
          int hour = DateTime.Now.Hour;
          DateTime now = DateTime.Now;
          int minute = now.Minute;
          now = DateTime.Now;
          int second = now.Second;
          now = DateTime.Now;
          int millisecond = now.Millisecond;
          timeSpan = new TimeSpan(hour, minute, second, millisecond);
          if (this.Opcions.ModoTickets == 1)
          {
            if (this.Opcions.News != 1)
            {
              if (this.Opcions.Srv_Web_Ip.ToLower().Contains(this.Web_V2_A) || this.Opcions.Srv_Web_Ip.ToLower().Contains(this.Web_V2_B))
              {
                flag = true;
                this.WebLink = "http://" + this.Opcions.Srv_Web_Ip + "/tickquioscV2.aspx?t=" + this.Opcions.Srv_User + "," + this.Opcions.Srv_User_P + ",tickquioscV2.aspx," + (object) timeSpan.TotalMilliseconds;
              }
              else
              {
                flag = true;
                this.WebLink = "http://" + this.Opcions.Srv_Web_Ip + "/tickquiosc.aspx?t=" + this.Opcions.Srv_User + "," + this.Opcions.Srv_User_P + ",tickquiosc.aspx," + (object) timeSpan.TotalMilliseconds;
              }
            }
            else
            {
              flag = true;
              this.WebLink = "http://" + this.Opcions.Srv_Web_Ip + "/" + this.Opcions.Srv_Web_Page + "?t=" + this.Opcions.Srv_User + "," + this.Opcions.Srv_User_P + "," + this.Opcions.Srv_Web_Page + "," + (object) timeSpan.TotalMilliseconds;
            }
          }
          else if (this.Opcions.News != 1)
          {
            flag = true;
            this.WebLink = "http://" + this.Opcions.Srv_Web_Ip + "/DemoQuiosk.aspx?t=" + this.Opcions.Srv_User + "," + this.Opcions.Srv_User_P + ",DemoQuiosk.aspx," + (object) timeSpan.TotalMilliseconds;
          }
          else
          {
            flag = true;
            this.WebLink = "http://" + this.Opcions.Srv_Web_Ip + "/" + this.Opcions.Srv_Web_Page + "?t=" + this.Opcions.Srv_User + "," + this.Opcions.Srv_User_P + "," + this.Opcions.Srv_Web_Page + "," + (object) timeSpan.TotalMilliseconds;
          }
        }
      }
      else
        this.WebLink = "about:blank";
    }

    [DllImport("user32.dll")]
    public static extern void mouse_event(
      int dwFlags,
      int dx,
      int dy,
      int cButtons,
      int dwExtraInfo);

    [DllImport("user32.dll")]
    private static extern bool SetCursorPos(int x, int y);

    [DllImport("user32.dll")]
    public static extern bool GetCursorPos(out MainWindow.POINT lpPoint);

    private void timerPoll_Tick(object sender, EventArgs e)
    {
      if (this.Opcions.__ModoTablet == 1 && this._pipeServer != null)
        this._pipeServer.Listen("Kiosk");
      Random random = new Random();
      string str = "";
      if (this.enpoolint)
        return;
      this.enpoolint = true;
      if (this._netConnection != null)
      {
        if (this._netConnection.Connected)
        {
          this.bHome.BackColor = System.Drawing.Color.Green;
          if (this._sharedObject != null && !this._sharedObject.Connected)
            this.bHome.BackColor = System.Drawing.Color.Orange;
        }
        else
          this.bHome.BackColor = System.Drawing.Color.Red;
      }
      if (this.controlcredits == 1 && this.Status != MainWindow.Fases.WaitOutOfService && this.Status != MainWindow.Fases.WaitLockUser)
        this.timerCredits_Tick(sender, e);
      bool flag;
      if (!this.Opcions.Running && this.Status != MainWindow.Fases.WaitOutOfService)
      {
        flag = true;
        this.pScreenSaver.Text = "";
        str = this.XLat_Error(this.ErrorNet, this.ErrorNetServer, this.ErrorDevices);
        this.pScreenSaver.Text = "(Status:" + this.XLat_Error(this.ErrorNet, this.ErrorNetServer, this.ErrorDevices) + ")\n" + this.Status.ToString() + " [" + this.Opcions.Srv_User + " - " + this.Opcions.IDMAQUINA + "]\nVersion " + this.Opcions.VersionPRG + this.pScreenSaver.Text;
      }
      if (this._sem_timerPoll_Tick == 1)
      {
        this.enpoolint = false;
      }
      else
      {
        this._sem_timerPoll_Tick = 1;
        ++this.cntInt;
        ++this.FiltreCnt;
        TimeSpan timeSpan;
        if (this.FiltreCnt >= 10)
        {
          if (this.Opcions.InGame && this.ValidacioTimeOut != null)
          {
            this.ValidacioTimeOut.Dispose();
            this.ValidacioTimeOut = (DLG_TimeOut) null;
            this.Opcions.LastMouseMove = DateTime.Now;
          }
          if (this.ValidacioTimeOut != null && this.ValidacioTimeOut.OK)
          {
            this.Opcions.Temps = 0;
            this.TimeoutHome = 0;
            this.lcdClock.Invalidate();
            this.pMenu.Invalidate();
            this.Invalidate();
            this.ValidacioTimeOut.Dispose();
            this.ValidacioTimeOut = (DLG_TimeOut) null;
            this.Opcions.LastMouseMove = DateTime.Now;
          }
          this.FiltreCnt = 0;
          timeSpan = DateTime.Now - this.Opcions.LastMouseMove;
          if ((int) timeSpan.TotalSeconds > this.Opcions.ResetTemps && (this.Opcions.Temps > 0 && this.Opcions.Credits <= new Decimal(0) && this.Opcions.ticketCleanTemps == 0))
          {
            if (this.ValidacioTimeOut == null)
            {
              this.ValidacioTimeOut = new DLG_TimeOut(ref this.Opcions, "Clean time");
              this.ValidacioTimeOut.Show();
            }
            if (this.ValidacioTimeOut.IsDisposed)
            {
              this.ValidacioTimeOut = new DLG_TimeOut(ref this.Opcions, "Clean time");
              this.ValidacioTimeOut.Show();
            }
          }
        }
        if (this.Opcions.FullScreen == 2)
        {
          this.Opcions.FullScreen = 0;
          if (this.Opcions.News != 1)
            this.pMenu.Visible = true;
          else
            this.pMenu.Visible = false;
          this.navegador.Visible = false;
          flag = true;
          this.FormBorderStyle = FormBorderStyle.None;
          this.Bounds = Screen.PrimaryScreen.Bounds;
          this.Hide_Browser_Nav();
        }
        if (this.Opcions.ModoKiosk == 0 && (this.Opcions.InGame && !this.Opcions.U_InGame))
        {
          MainWindow.SetCursorPos(1, 200);
          MainWindow.mouse_event(2, 1, 200, 0, 0);
          MainWindow.mouse_event(4, 1, 200, 0, 0);
          if (this.Opcions.CursorOn == 0)
          {
            MainWindow.ShowCursor(false);
            MainWindow.ShowCursor(false);
            MainWindow.ShowCursor(false);
            MainWindow.ShowCursor(false);
          }
        }
        if (this.Opcions.InGame)
        {
          if (!this.Opcions.U_InGame)
          {
            if (this.Opcions.NoCreditsInGame == 1)
            {
              this.oldStopCredits = false;
              this.Opcions.StopCredits = true;
            }
            this.CloseOSK();
            this.Opcions.U_InGame = true;
            this.Opcions.TempsDeTicket = this.Opcions.Temps;
            flag = true;
            if (this.oldStopCredits != this.Opcions.StopCredits)
            {
              if (this.Opcions.StopCredits)
              {
                this.pInsertCoin.Image = (Image) Resources.insertcoin2_off;
                this.Opcions.Enable_Lectors = -1;
                this.Close_Devices();
              }
              else
              {
                this.pInsertCoin.Image = (Image) null;
                if (this.Opcions.Enable_Lectors != 2 && !this.Opcions.StopCredits)
                {
                  this.Opcions.Error_Billetero = 0;
                  this.Opcions.Enable_Lectors = 0;
                }
              }
            }
            this.oldStopCredits = this.Opcions.StopCredits;
          }
        }
        else
        {
          if (this.Opcions.U_InGame)
          {
            if (this.Opcions.NoCreditsInGame == 1)
            {
              if (this.Opcions.Credits > new Decimal(0))
              {
                this.oldStopCredits = false;
                this.Opcions.StopCredits = true;
              }
              else
              {
                this.oldStopCredits = true;
                this.Opcions.StopCredits = false;
              }
            }
            if (this.Opcions.Temps > 0 && (this.Opcions.ModoTickets == 1 || this.Opcions.News == 2) && this.MenuGames == 0)
            {
              if (!this.WebLink.ToLower().Contains("tickquioscv2"))
              {
                this.MenuGames = 1;
                flag = false;
                try
                {
                  this.bTicket.Enabled = false;
                }
                catch
                {
                }
              }
              else
              {
                try
                {
                  this.bTicket.Enabled = true;
                }
                catch
                {
                }
                if (this.Opcions.Temps <= 0)
                {
                  this.MenuGames = 0;
                  try
                  {
                    this.bTicket.Enabled = false;
                  }
                  catch
                  {
                  }
                }
                flag = true;
              }
            }
            if (!this.Opcions.InGame)
            {
              this.Opcions.TempsDeTicket = this.Opcions.Temps;
              flag = true;
            }
          }
          this.Opcions.Pagar_Ticket_Busy = 0;
          this.Opcions.U_InGame = false;
          if (this.oldStopCredits != this.Opcions.StopCredits)
          {
            if (this.Opcions.StopCredits)
            {
              this.pInsertCoin.Image = (Image) Resources.insertcoin2_off;
              this.Opcions.Enable_Lectors = -1;
              this.Close_Devices();
            }
            else
            {
              this.pInsertCoin.Image = (Image) null;
              if (this.Opcions.Enable_Lectors != 2)
              {
                this.Opcions.Error_Billetero = 0;
                this.Opcions.Enable_Lectors = 0;
              }
            }
          }
          this.oldStopCredits = this.Opcions.StopCredits;
        }
        switch (this.Opcions.Enable_Lectors)
        {
          case 0:
            switch (this.Opcions.Dev_Coin.ToLower())
            {
              case "rm5":
                if (this.rm5 == null && (this.Opcions.Dev_Coin_P != "-" && this.Opcions.Dev_Coin_P != "?"))
                {
                  this.rm5 = new Control_Comestero();
                  this.rm5.port = this.Opcions.Dev_Coin_P;
                  if (this.Opcions.Dev_Bank == 1)
                    this.rm5.Set_Brazil();
                  else
                    this.rm5.Set_Euro();
                  if (!this.rm5.Open())
                  {
                    this.ErrorDevices = 901;
                    this.OutOfService();
                  }
                  this.needEnableM = 1;
                  this.waitneedEnableM = 0;
                  break;
                }
                break;
              case "cct2":
                if (this.cct2 == null && (this.Opcions.Dev_Coin_P != "-" && this.Opcions.Dev_Coin_P != "?"))
                {
                  this.cct2 = new Control_CCTALK_COIN();
                  this.cct2.port = this.Opcions.Dev_Coin_P;
                  if (!this.cct2.Open())
                  {
                    this.ErrorDevices = 902;
                    this.OutOfService();
                  }
                  this.needEnableM = 1;
                  this.waitneedEnableM = 0;
                  break;
                }
                break;
            }
            switch (this.Opcions.Dev_BNV.ToLower())
            {
              case "ssp":
                if (this.ssp == null)
                {
                  this.Opcions.Error_Billetero = 0;
                  if (this.Opcions.Dev_BNV_P != "-" && this.Opcions.Dev_BNV_P != "?")
                  {
                    this.ssp = new Control_NV_SSP();
                    this.ssp.port = this.Opcions.Dev_BNV_P;
                    if (!this.ssp.Open())
                    {
                      this.ForceCheck = true;
                      if (this.Opcions.ModoTickets != 0 || this.Opcions.News != 2)
                      {
                        flag = true;
                        this.Status = MainWindow.Fases.Reset;
                      }
                    }
                    this.needEnable = 1;
                    this.waitneedEnable = 0;
                  }
                  break;
                }
                break;
              case "ssp3":
                if (this.ssp3 == null)
                {
                  this.Opcions.Error_Billetero = 0;
                  if (this.Opcions.Dev_BNV_P != "-" && this.Opcions.Dev_BNV_P != "?")
                  {
                    this.ssp3 = new Control_NV_SSP_P6();
                    this.ssp3.port = this.Opcions.Dev_BNV_P;
                    if (!this.ssp3.Open())
                    {
                      this.ForceCheck = false;
                      if (this.Opcions.ModoTickets != 0 || this.Opcions.News != 2)
                      {
                        flag = true;
                        this.Status = MainWindow.Fases.Reset;
                      }
                    }
                    this.needEnable = 1;
                    this.waitneedEnable = 0;
                  }
                  break;
                }
                break;
              case "sio":
                if (this.sio == null)
                {
                  this.Opcions.Error_Billetero = 0;
                  if (this.Opcions.Dev_BNV_P != "-" && this.Opcions.Dev_BNV_P != "?")
                  {
                    this.sio = new Control_NV_SIO();
                    this.sio.port = this.Opcions.Dev_BNV_P;
                    switch (this.Opcions.Dev_Bank)
                    {
                      case 1:
                        this.sio.Set_Brazil();
                        break;
                      case 2:
                        this.sio.Set_Dominicana();
                        break;
                      default:
                        this.sio.Set_Euro();
                        break;
                    }
                    if (!this.sio.Open())
                    {
                      this.ErrorDevices = 802;
                      this.OutOfService();
                    }
                    this.needEnable = 1;
                    this.waitneedEnable = 0;
                  }
                  break;
                }
                break;
              case "f40":
                if (this.f40 == null)
                {
                  this.Opcions.Error_Billetero = 0;
                  if (this.Opcions.Dev_BNV_P != "-" && this.Opcions.Dev_BNV_P != "?")
                  {
                    this.f40 = new Control_F40_CCTalk();
                    this.f40.port = this.Opcions.Dev_BNV_P;
                    switch (this.Opcions.Dev_Bank)
                    {
                      case 1:
                        this.f40.Set_Brazil();
                        break;
                      case 2:
                        this.f40.Set_Dominicana();
                        break;
                      default:
                        this.f40.Set_Euro();
                        break;
                    }
                    if (!this.f40.Open())
                    {
                      this.ErrorDevices = 804;
                      this.OutOfService();
                    }
                    this.needEnable = 1;
                    this.waitneedEnable = 0;
                  }
                  break;
                }
                break;
              case "tri":
                if (this.tri == null)
                {
                  this.Opcions.Error_Billetero = 0;
                  if (this.Opcions.Dev_BNV_P != "-" && this.Opcions.Dev_BNV_P != "?")
                  {
                    this.tri = new Control_Trilogy();
                    if (this.Opcions.Dev_Bank == 1)
                      this.tri.Set_Brazil();
                    else
                      this.tri.Set_Euro();
                    this.tri.port = this.Opcions.Dev_BNV_P;
                    if (!this.tri.Open())
                    {
                      this.ErrorDevices = 803;
                      this.OutOfService();
                    }
                    this.needEnable = 1;
                    this.waitneedEnable = 0;
                  }
                  break;
                }
                break;
            }
            this.Opcions.Enable_Lectors = 2;
            break;
          case 2:
            ++this.waitneedEnable;
            ++this.waitneedEnableM;
            if (this.rm5 != null)
            {
              if (this.errorcreditsserv == 1)
              {
                if (this.needEnableM != 102)
                  this.needEnableM = 2;
              }
              else if (this.needEnableM != 101)
                this.needEnableM = 1;
              if (this.waitneedEnableM > 8)
              {
                this.waitneedEnableM = 0;
                if (this.needEnableM == 1)
                {
                  if (!this.Opcions.StopCredits)
                    this.rm5.Enable();
                  this.needEnableM += 100;
                }
                if (this.needEnableM == 2)
                {
                  this.rm5.Disable();
                  this.needEnableM += 100;
                }
              }
              if (this.rm5.SwapControl == 0)
              {
                this.rm5.Poll();
                this.rm5.SwapControl ^= 1;
              }
              else
              {
                this.rm5.Parser();
                this.rm5.SwapControl ^= 1;
                if (this.rm5.Creditos > 0)
                {
                  int creditos = this.rm5.Creditos;
                  this.Internal_Add_Credits(creditos);
                  this.rm5.Creditos -= creditos;
                  this.cnttimerCredits = 11;
                }
              }
            }
            if (this.cct2 != null)
            {
              if (this.errorcreditsserv == 1)
              {
                if (this.needEnableM != 102)
                  this.needEnableM = 2;
              }
              else if (this.needEnableM != 101)
                this.needEnableM = 1;
              if (this.waitneedEnableM > 8)
              {
                this.waitneedEnableM = 0;
                if (this.needEnableM == 1)
                {
                  if (!this.Opcions.StopCredits)
                    this.cct2.Enable(this.cct2.Coin_Acceptor_ID);
                  this.needEnableM += 100;
                }
                if (this.needEnableM == 2)
                {
                  this.cct2.Disable(this.cct2.Coin_Acceptor_ID);
                  this.needEnableM += 100;
                }
              }
              this.cct2.Poll();
              if (this.cct2.Creditos > 0)
              {
                int creditos = this.cct2.Creditos;
                this.Internal_Add_Credits(creditos);
                this.cct2.Creditos -= creditos;
                this.cnttimerCredits = 11;
              }
            }
            if (this.ssp != null)
            {
              if (this.errorcreditsserv == 1)
              {
                if (this.needEnable != 102)
                  this.needEnable = 2;
              }
              else if (this.needEnable != 101)
                this.needEnable = 1;
              if (this.waitneedEnable > 8)
              {
                this.waitneedEnable = 0;
                if (this.needEnable == 1)
                {
                  if (!this.Opcions.StopCredits)
                    this.ssp.Enable();
                  this.needEnable += 100;
                }
                if (this.needEnable == 2)
                {
                  this.ssp.Disable();
                  this.needEnable += 100;
                }
              }
              if (!this.ssp.Poll())
              {
                ++this.Opcions.Error_Billetero;
                if (this.Opcions.Error_Billetero > 10)
                {
                  this.Close_Devices();
                  this.Opcions.Error_Billetero = 0;
                  this.ErrorDevices = 800;
                  this.OutOfService();
                }
              }
              else if (this.ssp.TimeOutComs > 4 && this.ssp.TimeOutComs != 1000)
              {
                this.ssp.Close();
                this.ssp = (Control_NV_SSP) null;
                this.Opcions.Enable_Lectors = 0;
              }
              else
              {
                this.ssp.TimeOutComs = 0;
                this.Opcions.Error_Billetero = 0;
                if (this.ssp.Creditos > new Decimal(0))
                {
                  Decimal creditos = this.ssp.Creditos;
                  this.Internal_Add_Credits(creditos);
                  this.ssp.Creditos -= creditos;
                  this.cnttimerCredits = 11;
                }
              }
            }
            if (this.ssp3 != null)
            {
              if (this.errorcreditsserv == 1)
              {
                if (this.needEnable != 102)
                  this.needEnable = 2;
              }
              else if (this.needEnable != 101)
                this.needEnable = 1;
              if (this.waitneedEnable > 8)
              {
                this.waitneedEnable = 0;
                if (this.needEnable == 1)
                {
                  if (!this.Opcions.StopCredits)
                    this.ssp3.Enable();
                  this.needEnable += 100;
                }
                if (this.needEnable == 2)
                {
                  this.ssp3.Disable();
                  this.needEnable += 100;
                }
              }
              if (!this.ssp3.Poll())
              {
                ++this.Opcions.Error_Billetero;
                if (this.Opcions.Error_Billetero > 10)
                {
                  this.Close_Devices();
                  this.Opcions.Error_Billetero = 0;
                  this.ErrorDevices = 800;
                  this.OutOfService();
                }
              }
              else if (this.ssp3.TimeOutComs > 20)
              {
                this.ssp3.Close();
                this.ssp3 = (Control_NV_SSP_P6) null;
                this.Opcions.Enable_Lectors = 0;
              }
              else
              {
                this.Opcions.Error_Billetero = 0;
                if (this.ssp3.Creditos > 0)
                {
                  Decimal creditos = (Decimal) this.ssp3.Creditos;
                  this.Internal_Add_Credits(creditos);
                  this.ssp3.Creditos -= (int) creditos;
                  this.cnttimerCredits = 11;
                }
              }
            }
            if (this.sio != null)
            {
              if (this.errorcreditsserv == 1)
              {
                if (this.needEnable != 102)
                  this.needEnable = 2;
              }
              else if (this.needEnable != 101)
                this.needEnable = 1;
              if (this.waitneedEnable > 8)
              {
                this.waitneedEnable = 0;
                if (this.needEnable == 1)
                {
                  if (!this.Opcions.StopCredits)
                    this.sio.Enable();
                  this.needEnable += 100;
                }
                if (this.needEnable == 2)
                {
                  this.sio.Disable();
                  this.needEnable += 100;
                }
              }
              this.sio.Poll();
              this.sio.Parser();
              if (this.sio.CriticalJamp == 0)
              {
                if (this.sio.Creditos > 0)
                {
                  int creditos = this.sio.Creditos;
                  this.Internal_Add_Credits(creditos);
                  this.sio.Creditos -= creditos;
                  this.cnttimerCredits = 11;
                }
              }
              else if (this.sio.CriticalJamp == 1)
              {
                this.ErrorJamp = 1;
                this.sio.CriticalJamp = 2;
              }
            }
            if (this.tri != null)
            {
              if (this.errorcreditsserv == 1)
              {
                if (this.needEnable != 102)
                  this.needEnable = 2;
              }
              else if (this.needEnable != 101)
                this.needEnable = 1;
              if (this.waitneedEnable > 8)
              {
                this.waitneedEnable = 0;
                if (this.needEnable == 1)
                {
                  if (!this.Opcions.StopCredits)
                    this.tri.Enable();
                  this.needEnable += 100;
                }
                if (this.needEnable == 2)
                {
                  this.tri.Disable();
                  this.needEnable += 100;
                }
              }
              this.tri.Poll();
              this.tri.Parser();
              if (this.tri.Creditos > 0)
              {
                int creditos = this.tri.Creditos;
                this.Internal_Add_Credits(creditos);
                this.tri.Creditos -= creditos;
                this.cnttimerCredits = 11;
              }
            }
            if (this.f40 != null)
            {
              if (this.errorcreditsserv == 1)
              {
                if (this.needEnable != 102 && this.needEnable != 202)
                  this.needEnable = 2;
              }
              else if (this.needEnable != 101 && this.needEnable != 201)
                this.needEnable = 1;
              if (this.waitneedEnable > 8)
              {
                this.waitneedEnable = 0;
                if (this.needEnable == 101)
                {
                  if (!this.Opcions.StopCredits)
                    this.f40.Enable_All();
                  this.needEnable += 100;
                }
                if (this.needEnable == 1)
                {
                  if (!this.Opcions.StopCredits)
                    this.f40.Enable();
                  this.needEnable += 100;
                }
                if (this.needEnable == 102)
                {
                  this.f40.Disable_All();
                  this.needEnable += 100;
                }
                if (this.needEnable == 2)
                {
                  this.f40.Disable();
                  this.needEnable += 100;
                }
              }
              this.f40.Poll();
              if (this.f40.TimeOutComs > 10)
              {
                this.f40.Close();
                this.f40 = (Control_F40_CCTalk) null;
                this.Opcions.Enable_Lectors = 0;
              }
              else
              {
                this.Opcions.Error_Billetero = 0;
                if (this.f40.Creditos > 0)
                {
                  int creditos = this.f40.Creditos;
                  this.Internal_Add_Credits(creditos);
                  this.f40.Creditos -= creditos;
                  this.cnttimerCredits = 11;
                }
              }
              break;
            }
            break;
        }
        if (this.Opcions.Show_Browser)
        {
          if (this.Opcions.FreeGames == 0)
          {
            if (this.WebLink == "about:blank")
            {
              if (this.navegador.Visible)
                this.navegador.Visible = false;
            }
            else if (!this.navegador.Visible)
              this.navegador.Visible = true;
          }
          else if (!this.navegador.Visible)
            this.navegador.Visible = true;
        }
        if (this.Opcions.MissatgeCreditsGratuits >= 1)
        {
          this.Opcions.MissatgeCreditsGratuits = 0;
          if (this.dlg_playforticket != null)
          {
            this.dlg_playforticket.Close();
            this.dlg_playforticket.Dispose();
            this.dlg_playforticket = (DLG_Message_Full) null;
          }
          this.dlg_playforticket = new DLG_Message_Full(this.Opcions.Localize.Text("On doit jouer tous les credits gratuits pour prendre a check cadeaux!"), ref this.Opcions, true);
          this.dlg_playforticket.Show();
          this.Opcions.ForceRefresh = 1;
        }
        if (this.Opcions.MissatgePrinter != "")
        {
          if (this.dlg_msg_printer != null)
          {
            this.dlg_msg_printer.Close();
            this.dlg_msg_printer.Dispose();
            this.dlg_msg_printer = (DLG_Message_Full) null;
          }
          this.dlg_msg_printer = new DLG_Message_Full(this.Opcions.MissatgePrinter, ref this.Opcions, true);
          this.dlg_msg_printer.Show();
          this.Opcions.MissatgePrinter = "";
          this.Opcions.ForceRefresh = 1;
        }
        if (this.dlg_msg_printer != null && this.dlg_msg_printer.IsClosed)
        {
          this.dlg_msg_printer.Close();
          this.dlg_msg_printer.Dispose();
          this.dlg_msg_printer = (DLG_Message_Full) null;
        }
        if (this.dlg_playforticket != null && this.dlg_playforticket.IsClosed)
        {
          this.dlg_playforticket.Close();
          this.dlg_playforticket.Dispose();
          this.dlg_playforticket = (DLG_Message_Full) null;
        }
        switch (this.Status)
        {
          case MainWindow.Fases.StartUp:
            this.AdminEnabled = false;
            this.navegador.Focus();
            this.ErrorNetServer = 0;
            this._Srv_Commad = MainWindow.Srv_Command.Null;
            this.Srv_Connect();
            this.timerPoll.Enabled = true;
            this.cnttimerCredits = 11;
            this.DelayInt = 0;
            this.Opcions.Error_Billetero = 0;
            if (this.Opcions.News != 1)
            {
              if (this.Opcions.NoCreditsInGame == 1)
              {
                if (!this.Opcions.StopCredits)
                  this.Opcions.Enable_Lectors = 0;
              }
              else
                this.Opcions.Enable_Lectors = 0;
            }
            else
              this.Opcions.Enable_Lectors = -1;
            if (this.Opcions.Credits > new Decimal(0))
              this.Opcions.Temps = 120;
            this.Status = MainWindow.Fases.WaitStartUp;
            break;
          case MainWindow.Fases.WaitStartUp:
            this.Estat_Servidor_VAR();
            ++this.DelayInt;
            if (this.DelayInt > 10)
            {
              this.pScreenSaver.Visible = false;
              this.lCALL.Visible = false;
              this.Opcions.Running = true;
              this.Status = MainWindow.Fases.GoHome;
              if (this.Opcions.ModoKiosk == 0)
              {
                this.Opcions.FullScreen = 1;
                this.Opcions.Show_Browser = false;
                this.navegador.Visible = false;
                this.Stop_Temps();
                this.Status = MainWindow.Fases.GoNavigate;
                if (this.Opcions.__ModoTablet == 0)
                  this.CloseOSK();
                else if (this.Opcions.ModoKiosk != 0)
                  this.Tablet();
                else
                  this.CloseOSK();
                if (this.Opcions.CursorOn == 0)
                {
                  MainWindow.ShowCursor(false);
                  MainWindow.ShowCursor(false);
                  MainWindow.ShowCursor(false);
                  MainWindow.ShowCursor(false);
                }
                //ref TimeSpan local = ref timeSpan;
                DateTime now1 = DateTime.Now;
                int hour = now1.Hour;
                now1 = DateTime.Now;
                int minute = now1.Minute;
                DateTime now2 = DateTime.Now;
                int second = now2.Second;
                now2 = DateTime.Now;
                int millisecond = now2.Millisecond;
                timeSpan = new TimeSpan(hour, minute, second, millisecond);
                if (this.Opcions.ModoTickets == 1)
                {
                  if (this.Opcions.News != 1)
                  {
                    if (this.Opcions.Srv_Web_Ip.ToLower().Contains(this.Web_V2_A) || this.Opcions.Srv_Web_Ip.ToLower().Contains(this.Web_V2_B))
                    {
                      flag = true;
                      this.WebLink = "http://".ToLower() + this.Opcions.Srv_Web_Ip + "/MenuGame.aspx?t=" + this.Opcions.Srv_User + "," + this.Opcions.Srv_User_P + ",MenuGame.aspx," + (object) timeSpan.TotalMilliseconds;
                    }
                    else
                    {
                      flag = true;
                      this.WebLink = "http://".ToLower() + this.Opcions.Srv_Web_Ip + "/MenuGame.aspx?t=" + this.Opcions.Srv_User + "," + this.Opcions.Srv_User_P + ",MenuGame.aspx," + (object) timeSpan.TotalMilliseconds;
                    }
                  }
                  else
                  {
                    flag = true;
                    this.WebLink = "http://".ToLower() + this.Opcions.Srv_Web_Ip + "/" + this.Opcions.Srv_Web_Page + "?t=" + this.Opcions.Srv_User + "," + this.Opcions.Srv_User_P + ",Default.aspx," + (object) timeSpan.TotalMilliseconds;
                  }
                }
                else if (this.Opcions.News != 1)
                {
                  flag = true;
                  this.WebLink = "http://".ToLower() + this.Opcions.Srv_Web_Ip + "/MenuGame.aspx?t=" + this.Opcions.Srv_User + "," + this.Opcions.Srv_User_P + ",MenuGame.aspx," + (object) timeSpan.TotalMilliseconds;
                }
                else
                {
                  flag = true;
                  this.WebLink = "http://".ToLower() + this.Opcions.Srv_Web_Ip + "/" + this.Opcions.Srv_Web_Page + "?t=" + this.Opcions.Srv_User + "," + this.Opcions.Srv_User_P + ",Default.aspx," + (object) timeSpan.TotalMilliseconds;
                }
                this.FormBorderStyle = FormBorderStyle.None;
                this.ControlBox = false;
                this.Bounds = Screen.PrimaryScreen.Bounds;
                this.pMenu.Visible = false;
              }
              if (this.Opcions.Monitors > 1)
              {
                Screen[] allScreens1 = Screen.AllScreens;
                if (this.Opcions.Monitors > 1 && allScreens1.Length > 1)
                {
                  if (this.publi != null)
                  {
                    this.publi.Close();
                    this.publi.Dispose();
                    this.publi = (Publicitat) null;
                  }
                  this.publi = new Publicitat(ref this.Opcions);
                  Screen[] allScreens2 = Screen.AllScreens;
                  Rectangle bounds1 = Screen.PrimaryScreen.Bounds;
                  Rectangle bounds2 = allScreens2[1].Bounds;
                  if (bounds2.X <= 0)
                    bounds2 = allScreens2[0].Bounds;
                  this.publi.Show();
                  this.publi.SetBounds(bounds2.X, bounds2.Y, bounds2.Width, bounds2.Height);
                  this.publi.Update();
                  this.publi.Reload();
                  this.publi.Play();
                }
              }
              break;
            }
            break;
          case MainWindow.Fases.Reset:
            this.Control_Getton_Reset();
            this.Reconnect_Service();
            this.Opcions.Block_Remotes();
            this.timerMessages.Enabled = false;
            this.old_credits_gratuits = new Decimal(-1);
            this.StartupDetect = 1;
            this.Opcions.Load_Net();
            if (this.Opcions.ModoPlayCreditsGratuits == 0)
            {
              this.lCGRAT.Visible = false;
            }
            else
            {
              this.lCGRAT.Visible = true;
              this.lCGRAT.Text = "Credits gratuits\r\n" + (object) this.Opcions.CreditsGratuits;
              this.lCGRAT.Invalidate();
            }
            flag = true;
            this.Opcions.MissatgeCreditsGratuits = 0;
            if (this.dlg_playforticket != null)
            {
              this.dlg_playforticket.Close();
              this.dlg_playforticket.Dispose();
              this.dlg_playforticket = (DLG_Message_Full) null;
            }
            this.Opcions.MissatgePrinter = "";
            if (this.dlg_msg_printer != null)
            {
              this.dlg_msg_printer.Close();
              this.dlg_msg_printer.Dispose();
              this.dlg_msg_printer = (DLG_Message_Full) null;
            }
            if (this.dlg_checks != null)
            {
              this.dlg_checks.Close();
              this.dlg_checks.Dispose();
              this.dlg_checks = (DLG_Dispenser_Out) null;
            }
            Configuracion.Access_Log("Reset kiosk");
            this.MenuGames = 0;
            this.Banner_On = 0;
            this.Opcions.ticketCleanTemps = 0;
            this.Opcions.EnManteniment = 0;
            this.DelayServer = 500 + random.Next(100) * 10;
            if (this.Opcions.__ModoTablet == 0)
              this.CloseOSK();
            else if (this.Opcions.ModoKiosk != 0)
              this.Tablet();
            if (this.publi != null)
            {
              this.publi.Close();
              this.publi.Dispose();
              this.publi = (Publicitat) null;
            }
            if (this._DCalibrar != null)
            {
              this._DCalibrar.Close();
              this._DCalibrar.Dispose();
              this._DCalibrar = (DLG_Calibrar) null;
            }
            if (this.snd_alarma != null)
            {
              this.SoundRestore();
              this.snd_alarma.Stop();
            }
            this.EntraJocs.Visible = false;
            this.snd_alarma = (SoundPlayer) null;
            this.ErrorEnLogin = 0;
            this.Opcions.Emu_Mouse_RClick = false;
            MainWindow.ShowCursor(true);
            MainWindow.ShowCursor(true);
            MainWindow.ShowCursor(true);
            MainWindow.ShowCursor(true);
            if (this._netConnection != null)
              this._netConnection.Close();
            this.Opcions.U_InGame = false;
            this.Opcions.Srv_Room = "";
            this.Opcions.ForceAllKey = 0;
            this.Opcions.Temps = 0;
            this.Opcions.CancelTemps = 0;
            this.Opcions.TempsDeTicket = this.Opcions.Temps;
            this.Opcions.Running = false;
            this.Opcions.Enable_Lectors = -1;
            this.Close_Devices();
            this.Configurar_Splash(0);
            this.Error_Servidor = 0;
            this.cntInt = 0;
            this.ErrorNet = 0;
            this.ErrorDevices = 0;
            this.tmp_ip = this.Opcions.Srv_Ip;
            this.tmp_user = this.Opcions.Srv_User;
            this.tmp_pw = this.Opcions.Srv_User_P;
            this.controlcredits = 0;
            this.timerStartup.Enabled = false;
            this.Opcions.RunConfig = false;
            if (this.Opcions.ForceGoConfig)
              this.Opcions.RunConfig = true;
            this.DelayInt = 0;
            if (this.InsertCreditsDLG != null)
            {
              this.InsertCreditsDLG.Close();
              this.InsertCreditsDLG.Dispose();
              this.InsertCreditsDLG = (InsertCredits) null;
            }
            this.Status = MainWindow.Fases.WaitReset;
            this.GoWebInicial();
            if (this.doReset > 10)
            {
              flag = true;
              this.Status = MainWindow.Fases.GoRemoteReset;
              this.doReset = 0;
              break;
            }
            break;
          case MainWindow.Fases.WaitReset:
            this.AdminEnabled = true;
            ++this.DelayInt;
            if (this.DelayInt < 50)
            {
              if (this.Opcions.RunConfig || this.Opcions.ForceGoConfig)
              {
                this.Status = MainWindow.Fases.Config;
                break;
              }
              break;
            }
            this.DelayInt = 0;
            this.Status = MainWindow.Fases.WaitDevices;
            break;
          case MainWindow.Fases.WaitDevices:
            if (!this.Opcions.RunConfig && !this.Opcions.ForceGoConfig)
              this.Find_Validator(false);
            this.DelayInt = 0;
            this.Status = MainWindow.Fases.Splash;
            if (this.Opcions.NoCreditsInGame == 1 && this.Opcions.Credits > new Decimal(0))
            {
              this.oldStopCredits = false;
              this.Opcions.StopCredits = true;
              break;
            }
            break;
          case MainWindow.Fases.GoUpdate:
            this.Control_Getton_Reset();
            this.Opcions.Enable_Lectors = -1;
            this.Close_Devices();
            this.Stop_Temps();
            this.Configurar_Splash(102);
            this.LastErrorNet = -1;
            this.Opcions.RunConfig = false;
            this.AdminEnabled = true;
            this.Status = MainWindow.Fases.Update;
            this.DelayInt = 0;
            break;
          case MainWindow.Fases.Update:
            ++this.DelayInt;
            if (this.DelayInt == 5)
            {
              flag = true;
              this.Close();
              break;
            }
            break;
          case MainWindow.Fases.Splash:
            ++this.DelayInt;
            this.AdminEnabled = true;
            this.controlcredits = 1;
            if (this.CoolStartUp)
              this.CoolStartUp = false;
            if (this.DelayInt > 10)
            {
              this.Opcions.RunConfig = false;
              if (this.Opcions.ForceGoConfig)
                this.Opcions.RunConfig = true;
              if (this.ErrorEnLogin == 1 || this.ErrorJamp == 1)
              {
                this.AdminEnabled = true;
                this.DelayInt = 0;
                this.OutOfService();
              }
              else
              {
                this.AdminEnabled = true;
                this.DelayInt = 0;
                this.Srv_Connect();
                Thread.Sleep(1000);
                this.Srv_Test_Login(this.Opcions.Srv_User, this.Opcions.Srv_User_P, 0);
                this.DelayInt = 0;
                if (this.Status != MainWindow.Fases.OutOfService)
                {
                  this.timerStartup.Interval = 4000;
                  this.timerStartup.Enabled = true;
                  this.Status = MainWindow.Fases.WaitSplash;
                }
              }
              break;
            }
            break;
          case MainWindow.Fases.WaitSplash:
            ++this.DelayInt;
            if (this.DelayInt > 20 && this._Hook_Srv_Login.OK == -2)
            {
              if (this._Hook_Srv_Login.login == 1)
              {
                this.Ticket_Add_Credits = this._Hook_Srv_Login.ticket;
                this.Status = MainWindow.Fases.CheckRoom;
                flag = true;
              }
              else
                this.Status = this._Hook_Srv_Login.login != 2 ? MainWindow.Fases.Reset : MainWindow.Fases.LockUser;
            }
            if (this.DelayInt > 40)
            {
              flag = true;
              this.Srv_Connect();
              Thread.Sleep(1000);
              this._Srv_Commad = MainWindow.Srv_Command.Null;
              if (this._Hook_Srv_Login != null)
                this._Hook_Srv_Login.OK = -2;
              this.Srv_Test_Login(this.Opcions.Srv_User, this.Opcions.Srv_User_P, 0);
              this.DelayInt = 0;
            }
            if (this.Opcions.RunConfig)
            {
              this.Status = MainWindow.Fases.Config;
              break;
            }
            break;
          case MainWindow.Fases.CheckRoom:
            this.DelayInt = 0;
            if (this._Srv_Commad == MainWindow.Srv_Command.Null)
            {
              this.Srv_Get_Room();
              this.Status = MainWindow.Fases.WaitCheckRoom;
              break;
            }
            break;
          case MainWindow.Fases.WaitCheckRoom:
            ++this.DelayInt;
            if (this.DelayInt > 10)
            {
              this.Status = string.IsNullOrEmpty(this.Opcions.Srv_Room) ? MainWindow.Fases.Reset : (this.Opcions.News == 1 ? MainWindow.Fases.StartUp : MainWindow.Fases.CheckDevice);
              break;
            }
            break;
          case MainWindow.Fases.LockUser:
            this.Opcions.Enable_Lectors = -1;
            this.Close_Devices();
            this.Stop_Temps();
            this.Configurar_Splash(10);
            this.LastErrorNet = -1;
            this.Opcions.RunConfig = false;
            this.AdminEnabled = true;
            this.Status = MainWindow.Fases.WaitLockUser;
            break;
          case MainWindow.Fases.Config:
            this.Opcions.Running = false;
            this.timerStartup.Enabled = false;
            this.Opcions.Enable_Lectors = -1;
            this.Close_Devices();
            this.Opcions.ForceAllKey = 1;
            this.ForceCheck = false;
            this.ReInstall_Keyboard_Driver();
            this.LoginAdmin = new DLG_Login(ref this.Opcions, 0);
            MainWindow.SetFocusKiosk();
            this.SoundAlarm();
            Application.DoEvents();
            if (!this.Opcions.ForceGoConfig)
            {
              this.LoginAdmin.Focus();
              int num = (int) this.LoginAdmin.ShowDialog();
            }
            else
              this.LoginAdmin.Logeado = 1;
            this.Opcions.ForceAllKey = 0;
            if (this.LoginAdmin.Logeado == 1)
            {
              Configuracion.Access_Log("Andmin access");
              if (this.snd_alarma != null)
              {
                this.SoundRestore();
                this.snd_alarma.Stop();
              }
              this.snd_alarma = (SoundPlayer) null;
              this.ErrorJamp = 0;
              this.Configurar_Splash(2);
              this.Opcions.ForceAllKey = 1;
              this.Opcions.Emu_Mouse_RClick = true;
              this.ConfigAdmin = new DLG_Config(ref this.Opcions);
              this.ConfigAdmin.MWin = this;
              this.ConfigAdmin.Focus();
              int num = (int) this.ConfigAdmin.ShowDialog();
              this.Opcions.Emu_Mouse_RClick = false;
              this.Opcions.ForceAllKey = 0;
              this.Focus();
              this.Render_Bar_Menu();
            }
            else
              Configuracion.Access_Log("Andmin access fail");
            this.Opcions.ForceGoConfig = false;
            this.Status = MainWindow.Fases.WaitConfig;
            break;
          case MainWindow.Fases.WaitConfig:
            this.Estat_Servidor_VAR();
            this.Opcions.RunConfig = false;
            this.Status = MainWindow.Fases.Reset;
            break;
          case MainWindow.Fases.Register:
            this.tmp_user = "";
            this.tmp_pw = "";
            this.tmp_w = this.Opcions.Srv_Ip;
            this.Configurar_Splash(2);
            DLG_New_User dlgNewUser = new DLG_New_User(ref this.Opcions);
            dlgNewUser.Update_Info(this.tmp_user, this.tmp_pw, this.tmp_w);
            dlgNewUser.Focus();
            int num1 = (int) dlgNewUser.ShowDialog();
            this.tmp_user = this.Opcions.Srv_User = dlgNewUser.User;
            this.tmp_pw = this.Opcions.Srv_User_P = dlgNewUser.Password;
            this.tmp_w = this.Opcions.Srv_Ip = dlgNewUser.Web;
            this.DelayInt = 0;
            this.Srv_Connect();
            this.Srv_Test_Login(this.tmp_user, this.tmp_pw, 0);
            this.Status = MainWindow.Fases.WaitRegister;
            break;
          case MainWindow.Fases.WaitRegister:
            ++this.DelayInt;
            if (this.DelayInt > 20 && this._Hook_Srv_Login.OK == -2)
            {
              if (this._Hook_Srv_Login.login == 1)
              {
                this.Ticket_Add_Credits = this._Hook_Srv_Login.ticket;
                this.Opcions.Srv_User = this.tmp_user;
                this.Opcions.Srv_User_P = this.tmp_pw;
                this.Opcions.Save_Net();
                this.Status = MainWindow.Fases.Reset;
              }
              else
                this.Status = MainWindow.Fases.Register;
            }
            if (this.DelayInt > 50)
            {
              this.Status = MainWindow.Fases.Register;
              break;
            }
            break;
          case MainWindow.Fases.GoLogin:
            this.Control_Getton_Reset();
            if (!this.Opcions.Logged)
            {
              if ((object) this.login_dlg == null)
                this.login_dlg = new DLG_Registro(ref this.Opcions);
              this.login_dlg.Login = -1;
              this.Status = MainWindow.Fases.Login;
              int num2 = (int) this.login_dlg.ShowDialog();
              break;
            }
            this.Status = MainWindow.Fases.GoNavigate;
            this.login_dlg = (DLG_Registro) null;
            break;
          case MainWindow.Fases.Login:
            if ((object) this.login_dlg == null)
            {
              this.login_dlg = new DLG_Registro(ref this.Opcions);
              this.login_dlg.Login = -1;
              int num2 = (int) this.login_dlg.ShowDialog();
            }
            if (this.login_dlg.Login >= 0)
            {
              if (this.login_dlg.Login == 0)
              {
                this.Opcions.Running = true;
                this.Status = MainWindow.Fases.GoHome;
                this.login_dlg = (DLG_Registro) null;
              }
              else
              {
                this.Start_Temps();
                this.Status = MainWindow.Fases.GoNavigate;
                this.login_dlg = (DLG_Registro) null;
                this.WebLink = "https://www.google.com";
              }
              break;
            }
            break;
          case MainWindow.Fases.GoHome:
            this.TimeoutHome = 0;
            this.timerMessages.Enabled = true;
            this.Opcions.ForceGoConfig = false;
            this.CloseOSK();
            if (this.Opcions.CursorOn == 0)
            {
              MainWindow.ShowCursor(false);
              MainWindow.ShowCursor(false);
              MainWindow.ShowCursor(false);
              MainWindow.ShowCursor(false);
            }
            this.TimeoutCredits = 0;
            this.GoWebInicial();
            if (this.WebLink == "about:blank")
            {
              if (this.navegador.Visible)
                this.navegador.Visible = false;
            }
            else if (!this.navegador.Visible)
              this.navegador.Visible = true;
            this.Status = MainWindow.Fases.Home;
            if (this.Opcions.Temps <= 0)
              this.Hide_Browser_Nav();
            if (this.Opcions.BrowserBarOn == 1)
            {
              this.Show_Browser_Nav();
              break;
            }
            break;
          case MainWindow.Fases.Home:
          case MainWindow.Fases.Navigate:
          case MainWindow.Fases.NavigateScreenSaver:
            this.Control_Getton();
            if (this.Opcions.ForceRefresh == 1)
            {
              this.navegador.Reload();
              this.Opcions.ForceRefresh = 0;
            }
            this.Opcions.Block_Remotes();
            this.Estat_Servidor_VAR();
            this.Control_Credits_Gratuits();
            ++this.DelayInt;
            this.doReset = 0;
            ++this.TimeoutPubli;
            if (this.Opcions.News == 1 && !this.Opcions.Running)
              this.Status = MainWindow.Fases.Reset;
            if (this.publi != null && this.TimeoutPubli > 300)
            {
              this.TimeoutPubli = 0;
              this.publi.Next();
            }
            ++this.TimeoutCredits;
            ++this.TimeoutHome;
            if (this.Opcions.Credits <= new Decimal(0))
            {
              if (this.Opcions.InGame)
              {
                if (this.TimeoutCredits > this.Opcions.TimeoutCredits * 10 && this.Opcions.ModoKiosk == 1)
                {
                  this.Status = MainWindow.Fases.GoHome;
                  this.TimeoutHome = 0;
                  this.TimeoutCredits = 0;
                }
              }
              else
              {
                this.TimeoutCredits = 0;
                if (this.TimeoutHome > 3000)
                {
                  if (this.Opcions.ModoKiosk != 0)
                    this.Status = MainWindow.Fases.GoHome;
                  this.TimeoutHome = 0;
                }
              }
            }
            else
            {
              this.TimeoutHome = 0;
              this.TimeoutCredits = 0;
            }
            this.Check_Connection_WD();
            break;
          case MainWindow.Fases.GoRemoteReset:
            this.Control_Getton_Reset();
            this.Opcions.Enable_Lectors = -1;
            this.Close_Devices();
            this.Stop_Temps();
            this.Configurar_Splash(101);
            this.LastErrorNet = -1;
            this.Opcions.RunConfig = false;
            this.AdminEnabled = true;
            this.Status = MainWindow.Fases.RemoteReset;
            this.DelayInt = 0;
            break;
          case MainWindow.Fases.RemoteReset:
            ++this.DelayInt;
            if (this.DelayInt == 10)
            {
              flag = true;
              Process.Start("shutdown.exe", "/r /t 2");
              break;
            }
            break;
          case MainWindow.Fases.GoManteniment:
            this.Control_Getton_Reset();
            this.Opcions.Enable_Lectors = -1;
            this.Close_Devices();
            this.Stop_Temps();
            this.Configurar_Splash(100);
            this.LastErrorNet = -1;
            this.Opcions.RunConfig = false;
            this.AdminEnabled = true;
            this.Status = MainWindow.Fases.Manteniment;
            this.DelayInt = 0;
            break;
          case MainWindow.Fases.Manteniment:
            this.Estat_Servidor_VAR();
            ++this.DelayInt;
            if (this.Opcions.RunConfig)
            {
              this.Status = MainWindow.Fases.Config;
              break;
            }
            break;
          case MainWindow.Fases.GoNavigate:
            this.timerMessages.Enabled = true;
            this.DelayInt = 0;
            this.TimeoutPubli = 0;
            this.TimeoutCredits = 0;
            this.Status = MainWindow.Fases.Navigate;
            if (this.Opcions.ModoKiosk == 0)
              this.MenuGames = 1;
            if (!this.Detectar_Zona_Temps())
              this.Hide_Browser_Nav();
            else if (this.Opcions.ModoKiosk == 1)
            {
              if (this.Opcions.Temps > 0)
              {
                this.Show_Browser_Nav();
                this.Start_Temps();
                if (this.Banner_On == 1)
                {
                  this.Banner_On = 2;
                  this.Opcions.CancelTemps = 0;
                  this.EntraJocs.Visible = true;
                }
              }
              else
              {
                if (this.InsertCreditsDLG != null)
                {
                  this.InsertCreditsDLG.Close();
                  this.InsertCreditsDLG.Dispose();
                  this.InsertCreditsDLG = (InsertCredits) null;
                }
                this.InsertCreditsDLG = new InsertCredits(ref this.Opcions, 0);
                this.InsertCreditsDLG.Show();
                this.Status = MainWindow.Fases.GoHome;
              }
            }
            if (this.Opcions.BrowserBarOn == 1)
            {
              this.Show_Browser_Nav();
              break;
            }
            break;
          case MainWindow.Fases.CheckDevice:
            if (this.Opcions.News == 2)
            {
              this.ForceCheck = false;
              this.Status = MainWindow.Fases.StartUp;
              break;
            }
            if (this.Opcions.Dev_BNV == "?" || this.Opcions.Dev_BNV_P == "?" || this.ForceCheck)
            {
              this.ssp = new Control_NV_SSP();
              this.ssp.respuesta = false;
              this.ssp.Start_Find_Device();
              this.DelayInt = 0;
              this.Status = MainWindow.Fases.WaitCheckDevice;
              break;
            }
            this.Status = MainWindow.Fases.StartUp;
            break;
          case MainWindow.Fases.WaitCheckDevice:
            ++this.DelayInt;
            if (this.ssp == null)
            {
              if (this.DelayInt > 10)
              {
                if (this.Opcions.News == 2)
                {
                  this.ForceCheck = false;
                  this.Status = MainWindow.Fases.StartUp;
                }
                else
                {
                  this.ErrorDevices = 800;
                  this.OutOfService();
                }
                break;
              }
              break;
            }
            this.ssp.Poll();
            if (this.ssp.respuesta)
            {
              this.ForceCheck = false;
              this.ssp.Stop_Find_Device();
              this.ssp.Close();
              this.ssp = (Control_NV_SSP) null;
              this.Status = MainWindow.Fases.StartUp;
              break;
            }
            if (this.DelayInt > 10)
            {
              if (this.ssp.Last_Find_Device())
              {
                if (this.ssp.Poll_Find_Device())
                  this.DelayInt = 0;
              }
              else
              {
                this.ssp.Stop_Find_Device();
                this.ssp.Close();
                this.ssp = (Control_NV_SSP) null;
                if (this.Opcions.News == 2)
                {
                  this.ForceCheck = false;
                  this.Status = MainWindow.Fases.StartUp;
                }
                else
                {
                  this.ErrorDevices = 800;
                  this.OutOfService();
                }
              }
            }
            break;
          case MainWindow.Fases.Calibrar:
            this.DelayInt = 0;
            this.Opcions.Enable_Lectors = -1;
            this.Close_Devices();
            if (this._DCalibrar == null)
              this._DCalibrar = new DLG_Calibrar(ref this.Opcions, 1);
            else if (this._DCalibrar.IsDisposed)
            {
              this._DCalibrar = (DLG_Calibrar) null;
              this._DCalibrar = new DLG_Calibrar(ref this.Opcions, 1);
            }
            this._DCalibrar.Show();
            this.Status = MainWindow.Fases.WaitCalibrar;
            break;
          case MainWindow.Fases.WaitCalibrar:
            if (this.Opcions.RunConfig)
            {
              if (this._DCalibrar != null)
              {
                this._DCalibrar.Close();
                this._DCalibrar.Dispose();
                this._DCalibrar = (DLG_Calibrar) null;
              }
              this.Status = MainWindow.Fases.Reset;
            }
            if (this._DCalibrar != null)
            {
              if (this._DCalibrar.IsDisposed)
              {
                this._DCalibrar = (DLG_Calibrar) null;
                this.Status = MainWindow.Fases.Reset;
                break;
              }
              break;
            }
            this.Status = MainWindow.Fases.Reset;
            break;
          case MainWindow.Fases.OutOfService:
            if (this.Opcions.__ModoTablet == 1 && this.Opcions.ModoKiosk != 0)
              this.Tablet();
            this.Opcions.Enable_Lectors = -1;
            this.Close_Devices();
            this.Check_Connection();
            this.Stop_Temps();
            this.Configurar_Splash(1);
            this.LastErrorNet = -1;
            this.Opcions.RunConfig = false;
            this.AdminEnabled = true;
            this.DelayInt = 0;
            this.Status = MainWindow.Fases.WaitOutOfService;
            break;
          case MainWindow.Fases.WaitOutOfService:
            this.Estat_Servidor_VAR();
            if (this.ErrorJamp == 1)
            {
              if (this.Opcions.RunConfig)
              {
                this.Status = MainWindow.Fases.Config;
                break;
              }
              break;
            }
            if (this.ErrorNet + this.ErrorNetServer + this.ErrorDevices != this.LastErrorNet)
            {
              this.LastErrorNet = this.ErrorNet + this.ErrorNetServer + this.ErrorDevices;
              this.Configurar_Splash(1);
            }
            if (MainWindow.IsNetworkAvailable())
            {
              if (this.PingTest())
              {
                if (this.ErrorNetServer == 0)
                {
                  if (this.ErrorDevices == 0 && this.ErrorEnLogin != 1)
                    this.Status = MainWindow.Fases.Reset;
                  this.ErrorNet = 0;
                }
                else if (this.ErrorEnLogin != 1)
                  this.Status = MainWindow.Fases.Reset;
              }
              else
                this.ErrorNet = 101;
            }
            else
              this.ErrorNet = 100;
            if (this.Opcions.RunConfig)
              this.Status = MainWindow.Fases.Config;
            ++this.DelayInt;
            if (this.Error_Servidor == 0 && this.ErrorNet == 0 && (this.DelayInt > 300 && this.ErrorEnLogin != 1) && this.ErrorJamp != 1)
              this.Status = MainWindow.Fases.Reset;
            if (this.DelayInt > this.DelayServer)
            {
              this.Status = MainWindow.Fases.Reset;
              ++this.doReset;
              break;
            }
            break;
        }
        this._sem_timerPoll_Tick = 0;
        this.enpoolint = false;
      }
    }

    private void bHome_Click(object sender, EventArgs e)
    {
      bool flag = true;
      this.Srv_Connect();
      this.Status = MainWindow.Fases.GoHome;
      this.Hide_Browser_Nav();
      this.Stop_Temps();
      if (this.Opcions.CancelTempsOn == 1 && this.Opcions.Credits > new Decimal(0))
      {
        flag = false;
        int num = this.Opcions.CancelTemps / 12 * 5;
        if (this.Opcions.Credits > (Decimal) num)
          this.Srv_Sub_Credits((Decimal) num, 0);
        else
          this.Srv_Sub_Credits(this.Opcions.Credits, 0);
      }
      this.Banner_On = 0;
      this.MenuGames = 0;
      this.Opcions.CancelTemps = 0;
      this.EntraJocs.Visible = false;
      if (this.Opcions.ModoTickets != 0 || this.Opcions.News != 2)
        return;
      this.Start_Temps();
    }

    private void bGo_Click(object sender, EventArgs e)
    {
      if (!string.IsNullOrEmpty(this.tURL.Text))
      {
        if (this.tURL.Text.ToLower().Contains(this.Opcions.Srv_Ip))
        {
          this.Status = MainWindow.Fases.GoHome;
        }
        else
        {
          this.Status = MainWindow.Fases.GoNavigate;
          this.WebLink = this.tURL.Text;
        }
      }
      if (this.Opcions.ModoTickets != 0 || this.Opcions.News != 2)
        return;
      this.Start_Temps();
    }

    private void tURL_KeyPress(object sender, KeyPressEventArgs e)
    {
      if (e.KeyChar != '\r' || string.IsNullOrEmpty(this.tURL.Text))
        return;
      e.Handled = true;
      if (this.tURL.Text.ToLower().Contains("file:".ToLower()) || this.tURL.Text.ToLower().Contains("about:".ToLower()) || this.tURL.Text.ToLower().Contains("ftp:".ToLower()) || this.tURL.Text.ToLower().Contains("ftps:".ToLower()))
        this.Status = MainWindow.Fases.GoHome;
      else if (this.Opcions.News != 1)
      {
        this.Status = MainWindow.Fases.GoNavigate;
        this.WebLink = this.tURL.Text;
      }
    }

    private void Control_Credits_Gratuits()
    {
      if (this.Opcions.ModoPlayCreditsGratuits == 0 || !(this.old_credits_gratuits != this.Opcions.CreditsGratuits))
        return;
      this.lCGRAT.Text = "Credits gratuits\r\n" + (object) this.Opcions.CreditsGratuits;
      this.lCGRAT.Invalidate();
      this.old_credits_gratuits = this.Opcions.CreditsGratuits;
    }

    private void bKeyboard_Click(object sender, EventArgs e)
    {
      if (this.Opcions.InGame)
        return;
      Process[] processesByName = Process.GetProcessesByName("KVKeyboard");
      if (processesByName == null)
        Process.Start("KVKeyboard.exe", "es");
      else if (processesByName.Length >= 1)
      {
        PipeClient pipeClient = new PipeClient();
        if (pipeClient == null)
          return;
        try
        {
          pipeClient.Send("QUIT", "KVKeyboard", 1000);
        }
        catch
        {
        }
      }
      else
        Process.Start("KVKeyboard.exe", "es");
    }

    private void Tablet()
    {
      Process[] processesByName = Process.GetProcessesByName("KVKeyboard");
      if (processesByName == null)
      {
        Process.Start("KVKeyboard.exe", "es");
      }
      else
      {
        if (processesByName.Length >= 1)
          return;
        Process.Start("KVKeyboard.exe", "es");
      }
    }

    private void MainWindow_Paint(object sender, PaintEventArgs e)
    {
    }

    private void MainWindow_SizeChanged(object sender, EventArgs e)
    {
      if (this.Opcions.RunConfig && this.pScreenSaver != null)
      {
        this.pScreenSaver.Top = 0;
        this.pScreenSaver.Left = 0;
        Label pScreenSaver1 = this.pScreenSaver;
        Rectangle bounds = Screen.PrimaryScreen.Bounds;
        int height = bounds.Height;
        pScreenSaver1.Height = height;
        Label pScreenSaver2 = this.pScreenSaver;
        bounds = Screen.PrimaryScreen.Bounds;
        int width = bounds.Width;
        pScreenSaver2.Width = width;
      }
      this.Windows_MidaY = this.Height;
      if (!this.DisplayKeyboard)
        ;
    }

    [DllImport("user32.dll")]
    private static extern int FindWindow(string cls, string wndwText);

    [DllImport("user32.dll")]
    private static extern int ShowWindow(int hwnd, int cmd);

    [DllImport("user32.dll")]
    private static extern long SHAppBarMessage(long dword, int cmd);

    [DllImport("user32.dll")]
    private static extern int RegisterHotKey(IntPtr hwnd, int id, int fsModifiers, int vk);

    [DllImport("user32.dll")]
    private static extern int UnregisterHotKey(IntPtr hwnd, int id);

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool UnhookWindowsHookEx(IntPtr hhk);

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern IntPtr CallNextHookEx(
      IntPtr hhk,
      int nCode,
      IntPtr wParam,
      ref MainWindow.KBDLLHOOKSTRUCT lParam);

    [DllImport("user32.dll")]
    public static extern short GetAsyncKeyState(int vKey);

    [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern IntPtr GetModuleHandle(string lpModuleName);

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern IntPtr SetWindowsHookEx(
      int idHook,
      MainWindow.HookHandlerDelegate lpfn,
      IntPtr hMod,
      uint dwThreadId);

    [DllImport("WinLockDll.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern IntPtr CtrlAltDel_Enable_Disable(bool bEnableDisable);

    public void ForceModoWindows()
    {
      this.UnLock_Windows();
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
      Configuracion.WinReset();
    }

    public void Keyboard_Hook()
    {
      if (!this._khook)
      {
        this.proc = new MainWindow.HookHandlerDelegate(this.Int_Keyboard);
        using (Process currentProcess = Process.GetCurrentProcess())
        {
          using (ProcessModule mainModule = currentProcess.MainModule)
            this.hookID = MainWindow.SetWindowsHookEx(13, this.proc, MainWindow.GetModuleHandle(mainModule.ModuleName), 0U);
        }
      }
      this._khook = true;
    }

    public void Keyboard_Restore()
    {
      if (this._khook)
        MainWindow.UnhookWindowsHookEx(this.hookID);
      this._khook = false;
    }

    private void System_ShutDown()
    {
      this.timerCredits.Enabled = false;
      this.timerPoll.Enabled = false;
      Environment.Exit(0);
    }

    public void Normal_Screen()
    {
      if (this.Opcions.News != 1)
        this.pMenu.Visible = true;
      else
        this.pMenu.Visible = false;
    }

    private IntPtr Int_Keyboard(
      int nCode,
      IntPtr wParam,
      ref MainWindow.KBDLLHOOKSTRUCT lParam)
    {
      bool flag = false;
      switch ((int) wParam)
      {
        case 256:
        case 257:
        case 260:
        case 261:
          if (lParam.vkCode == 9 && lParam.flags == 32 || lParam.vkCode == 27 && lParam.flags == 32 || (lParam.vkCode == 27 && lParam.flags == 0 || lParam.vkCode == 91 && lParam.flags == 1) || lParam.vkCode == 92 && lParam.flags == 1 || lParam.flags == 32)
          {
            flag = true;
            break;
          }
          break;
      }
      if (flag)
        return (IntPtr) 1;
      return MainWindow.CallNextHookEx(this.hookID, nCode, wParam, ref lParam);
    }

    private void RegisterGlobalHotKey(Keys hotkey, int modifiers)
    {
      try
      {
        ++this.mHotKeyId;
        if (this.mHotKeyId <= (short) 0 || MainWindow.RegisterHotKey(this.Handle, (int) this.mHotKeyId, modifiers, (int) Convert.ToInt16((object) hotkey)) != 0)
          return;
        int num = (int) MessageBox.Show(this.mHotKeyId.ToString());
      }
      catch
      {
        this.UnregisterGlobalHotKey();
      }
    }

    private void UnregisterGlobalHotKey()
    {
      for (int id = 0; id < (int) this.mHotKeyId; ++id)
        MainWindow.UnregisterHotKey(this.Handle, id);
    }

    [DllImport("user32.dll")]
    public static extern int PeekMessage(
      out Message lpMsg,
      IntPtr window,
      uint wMsgFilterMin,
      uint wMsgFilterMax,
      uint wRemoveMsg);

    [DllImport("setupapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
    internal static extern IntPtr SetupDiGetClassDevs(
      ref Guid ClassGuid,
      IntPtr Enumerator,
      IntPtr hwndParent,
      int Flags);

    private bool USB_DEVICE(Message m)
    {
      return false;
    }

    private void OnDeviceChange(Message m)
    {
      if (m.WParam.ToInt32() == 32768 || m.WParam.ToInt32() != 32772)
        ;
    }

    protected override void WndProc(ref Message m)
    {
      if (m.Msg == 536 && (int) m.WParam == 7)
      {
        if (this.Opcions.Credits <= new Decimal(0))
          this.Status = MainWindow.Fases.GoRemoteReset;
        else if (!this.Opcions.InGame)
          this.Status = MainWindow.Fases.GoRemoteReset;
      }
      if (m.Msg == 537)
      {
        MainWindow.DBT wparam = (MainWindow.DBT) (int) m.WParam;
        int num = m.LParam == IntPtr.Zero ? 0 : Marshal.ReadInt32(m.LParam, 4);
        if (wparam == MainWindow.DBT.DBT_DEVICEARRIVAL)
        {
          switch (num)
          {
            case 5:
              MainWindow.DEV_BROADCAST_DEVICEINTERFACE_1 structure = (MainWindow.DEV_BROADCAST_DEVICEINTERFACE_1) Marshal.PtrToStructure(m.LParam, typeof (MainWindow.DEV_BROADCAST_DEVICEINTERFACE_1));
              string str = "";
              int index = 0;
              while (structure.dbcc_name[index] != char.MinValue)
                str += (string) (object) structure.dbcc_name[index++];
              break;
          }
        }
      }
      base.WndProc(ref m);
      if (m.Msg != 786)
        ;
    }

    private void Modo_Kiosk_Off()
    {
      MainWindow.ShowWindow(MainWindow.FindWindow("Shell_TrayWnd", (string) null), 1);
      MainWindow.CtrlAltDel_Enable_Disable(true);
    }

    private void Modo_Kiosk_On()
    {
      MainWindow.CtrlAltDel_Enable_Disable(false);
      this.Block_Accessibility();
    }

    [DllImport("user32")]
    private static extern bool SetForegroundWindow(IntPtr hWnd);

    [DllImport("user32")]
    private static extern int ShowWindow(IntPtr hWnd, int swCommand);

    [DllImport("user32")]
    private static extern bool IsIconic(IntPtr hWnd);

    public static void SetFocusKiosk()
    {
      IntPtr mainWindowHandle = Process.GetCurrentProcess().MainWindowHandle;
      if (MainWindow.IsIconic(mainWindowHandle))
        MainWindow.ShowWindow(mainWindowHandle, 9);
      MainWindow.SetForegroundWindow(mainWindowHandle);
    }

    private void MainWindow_Load(object sender, EventArgs e)
    {
      MainWindow.SetFocusKiosk();
      this.timerPoll.Enabled = true;
      if (this.Opcions.ForceLogin == 1)
        this.bGo.Visible = true;
      else
        this.bGo.Visible = false;
    }

    private void lcdClock_Click(object sender, EventArgs e)
    {
    }

    public void Start_Temps()
    {
      if (!this.Detectar_Zona_Temps())
      {
        this.Stop_Temps();
      }
      else
      {
        this.Opcions.TimeNavigate = true;
        if (this.Opcions.ModoTickets != 0)
          this.Banner_On = 1;
        this.Opcions.CancelTemps = 0;
      }
    }

    public void Stop_Temps()
    {
      this.Opcions.TimeNavigate = false;
      this.cnttime = 0;
      this.Banner_On = 0;
    }

    public void Config_Temps()
    {
    }

    public void Show_Navegador()
    {
      this.Opcions.Show_Browser = true;
    }

    private void Srv_Connect()
    {
      this.ErrorNetServer = 0;
      if (this._netConnection == null)
      {
        if (this.Status == MainWindow.Fases.OutOfService || this.Opcions.RunConfig)
          return;
        this.ErrorNetServer = 501;
        this.OutOfService();
      }
      else
      {
        if (this._netConnection.Connected)
          return;
        bool flag;
        if (this.Opcions.News != 1)
        {
          flag = true;
          this.Start_Service("http://" + this.Opcions.Srv_Web_Ip + "/DemoQuiosk.aspx");
        }
        else
        {
          flag = true;
          this.Start_Service("http://" + this.Opcions.Srv_Web_Ip + "/" + this.Opcions.Srv_Web_Page);
        }
        flag = true;
        string command = "rtmp://" + this.Opcions.Srv_Ip + ":" + (object) Convert.ToInt32(this.Opcions.Srv_port) + "/" + this.Opcions.Srv_Rtm;
        if (this.Opcions.Srv_User == "-")
        {
          this.ErrorEnLogin = 1;
          this.LastErrorNet = this.ErrorNet + this.ErrorNetServer + this.ErrorDevices;
          this.Configurar_Splash(1);
        }
        else
        {
          try
          {
            this._netConnection.Connect(command, (object) ("QuiosK|" + this.Opcions.Srv_User), (object) this.Opcions.Srv_User_P);
          }
          catch (Exception ex)
          {
            this.ErrorGenericText = ex.Message;
            if (this.Status == MainWindow.Fases.OutOfService || this.Opcions.RunConfig)
              return;
            this.ErrorNetServer = 502;
            this.OutOfService();
            return;
          }
          DateTime dateTime = DateTime.Now.AddSeconds(30.0);
          do
          {
            Application.DoEvents();
            if (DateTime.Now > dateTime)
            {
              if (this.Status != MainWindow.Fases.OutOfService && !this.Opcions.RunConfig)
              {
                this.ErrorNetServer = 503;
                if (!this._netConnection.Connected)
                {
                  this.OutOfService();
                  return;
                }
                break;
              }
              goto label_22;
            }
          }
          while (!this._netConnection.Connected);
          goto label_20;
label_22:
          return;
label_20:;
        }
      }
    }

    public void ResultReceived(IPendingServiceCall call)
    {
      object result = call.Result;
      if (this._Srv_Commad == MainWindow.Srv_Command.Credits)
      {
        try
        {
          if (Convert.ToDecimal(result) < new Decimal(0))
          {
            Decimal num = new Decimal(0);
          }
          this.errorcreditsserv = 0;
        }
        catch
        {
          this.errorcreditsserv = 1;
        }
      }
      if (this._Srv_Commad == MainWindow.Srv_Command.SubCredits)
      {
        try
        {
          this.Opcions.Sub_Credits = !Convert.ToBoolean(result) ? new Decimal(0) : new Decimal(0);
        }
        catch
        {
        }
      }
      if (this._Srv_Commad == MainWindow.Srv_Command.Room)
      {
        try
        {
          this._Srv_Commad = MainWindow.Srv_Command.Null;
          this.Opcions.Srv_Room = string.Concat((object) Convert.ToInt32(result));
          this.Connect_Room();
        }
        catch
        {
        }
      }
      this._Srv_Commad = MainWindow.Srv_Command.Null;
    }

    private void Reconnect_ShareObject()
    {
      if (this._netConnection == null || !this._netConnection.Connected)
        return;
      if (this._sharedObject == null)
      {
        this.delayreconnect = 0;
        this._sharedObject = RemoteSharedObject.GetRemote("QuioskOn" + this.Opcions.Srv_User, this._netConnection.Uri.ToString(), (object) false);
        this._sharedObject.OnConnect += new ConnectHandler(this._sharedObject_OnConnect);
        this._sharedObject.OnDisconnect += new DisconnectHandler(this._sharedObject_OnDisconnect);
        this._sharedObject.NetStatus += new NetStatusHandler(this._sharedObject_NetStatus);
        this._sharedObject.Sync += new SyncHandler(this._sharedObject_Sync);
        this._sharedObject.Connect(this._netConnection);
      }
      this.Connect_Room();
    }

    private void _netConnection_OnConnect(object sender, EventArgs e)
    {
      if (this._netConnection != null && this._netConnection.Connected)
      {
        if (this._sharedObject == null)
        {
          this._sharedObject = RemoteSharedObject.GetRemote("QuioskOn" + this.Opcions.Srv_User, this._netConnection.Uri.ToString(), (object) false);
          this._sharedObject.OnConnect += new ConnectHandler(this._sharedObject_OnConnect);
          this._sharedObject.OnDisconnect += new DisconnectHandler(this._sharedObject_OnDisconnect);
          this._sharedObject.NetStatus += new NetStatusHandler(this._sharedObject_NetStatus);
          this._sharedObject.Sync += new SyncHandler(this._sharedObject_Sync);
          this._sharedObject.Connect(this._netConnection);
        }
        this.Connect_Room();
      }
      this.errorcreditsserv = 0;
      this.cnterror = 0;
      this.ErrorNetServer = 0;
    }

    private void Connect_Room()
    {
      if (string.IsNullOrEmpty(this.Opcions.Srv_Room) || MainWindow._sharedObjectPagos != null)
        return;
      MainWindow._sharedObjectPagos = RemoteSharedObject.GetRemote(typeof (MainWindow.UsersRSO), "PagosSala" + this.Opcions.Srv_Room, this._netConnection.Uri.ToString(), (object) true);
      MainWindow._sharedObjectPagos.OnConnect += new ConnectHandler(this._sharedObjectPagos_OnConnect);
      MainWindow._sharedObjectPagos.OnDisconnect += new DisconnectHandler(this._sharedObjectPagos_OnDisconnect);
      MainWindow._sharedObjectPagos.NetStatus += new NetStatusHandler(this._sharedObjectPagos_NetStatus);
      MainWindow._sharedObjectPagos.Sync += new SyncHandler(this._sharedObjectPagos_Sync);
      MainWindow._sharedObjectPagos.Connect(this._netConnection);
    }

    private void _netConnection_OnDisconnect(object sender, EventArgs e)
    {
      this.errorcreditsserv = 1;
      if (this._sharedObject != null)
      {
        try
        {
          this._sharedObject.Close();
          this._sharedObject.Dispose();
        }
        catch
        {
        }
        this._sharedObject = (RemoteSharedObject) null;
      }
      if (MainWindow._sharedObjectPagos != null)
      {
        try
        {
          MainWindow._sharedObjectPagos.Close();
          MainWindow._sharedObjectPagos.Dispose();
        }
        catch
        {
        }
        MainWindow._sharedObjectPagos = (RemoteSharedObject) null;
      }
      ++this.cnterror;
      if (this.cnterror <= 5 || this.Status == MainWindow.Fases.OutOfService)
        return;
      if (this.Status != MainWindow.Fases.WaitOutOfService && !this.Opcions.RunConfig)
      {
        this.ErrorNetServer = 504;
        this.ErrorEnLogin = 1;
        this.OutOfService();
      }
      this.cnterror = 0;
    }

    private void _netConnection_NetStatus(object sender, NetStatusEventArgs e)
    {
      string str = e.Info["level"] as string;
      if (!(str == "error"))
        ;
      if (!(str == "status"))
        ;
    }

    private void _sharedObject_OnConnect(object sender, EventArgs e)
    {
      this.errorcreditsserv = 0;
    }

    private void _sharedObject_OnDisconnect(object sender, EventArgs e)
    {
      this.errorcreditsserv = 1;
    }

    private void _sharedObjectPagos_OnConnect(object sender, EventArgs e)
    {
    }

    private void _sharedObjectPagos_OnDisconnect(object sender, EventArgs e)
    {
    }

    private void _sharedObjectPagos_NetStatus(object sender, NetStatusEventArgs e)
    {
      string str = e.Info["level"] as string;
      if (!(str == "error"))
        ;
      if (!(str == "status"))
        ;
    }

    private int Gestio_Pagament(int _pag, int _tick = 0)
    {
      if (_pag <= 0 && _tick == 0)
        return 100;
      if (this.Opcions.Disp_Enable == 1 && _tick == 0)
      {
        if (!this.Check_Printer_Ready(this.Opcions.Impresora_Tck))
          return 1;
        int num1 = this.Opcions.Disp_Val;
        int num2 = 0;
        if (this.Opcions.Disp_Min * this.Opcions.Disp_Val > num1)
          num1 = this.Opcions.Disp_Min * this.Opcions.Disp_Val;
        if (this.Opcions.Disp_Max * this.Opcions.Disp_Val > num2)
          num2 = this.Opcions.Disp_Max * this.Opcions.Disp_Val;
        if (this.Opcions.Disp_Min > 0 && _pag < num1)
          return 101;
        int num3 = num2 / this.Opcions.Disp_Val;
        int num4 = _pag / this.Opcions.Disp_Val;
        if (this.Opcions.Disp_Max > 0 && num4 > num3)
          num4 = num3;
        this.Opcions.Disp_Pay_Ticket_Credits = _pag;
        this.Opcions.Disp_Pay_Ticket = num4;
        this.Opcions.Disp_Pay_Ticket_Out = 0;
        this.Opcions.Disp_Pay_Ticket_Cnt_Fail = 0;
        this.Opcions.Disp_Pay_Ticket_Fail = 0;
        this.Opcions.Disp_Pay_Ticket_Out_Flag = 0;
        this.Opcions.Disp_Pay_Running = 1;
        return 1;
      }
      if (!this.Check_Printer_Ready(this.Opcions.Impresora_Tck))
        return 201;
      if (this.Opcions.Pagar_Ticket_Busy == 1)
        return 200;
      this.Opcions.Pagar_Ticket_Val = _pag;
      this.Opcions.Pagar_Ticket_Busy = 1;
      this.Srv_Add_Pay(0);
      return 0;
    }

    private void _sharedObjectPagos_Sync(object sender, SyncEventArgs e)
    {
      if (this.Opcions.ModoTickets == 0 || MainWindow._sharedObjectPagos == null || !MainWindow._sharedObjectPagos.Connected)
        return;
      MainWindow._sharedObjectPagos.GetAttributeNames();
      string attribute = (string) MainWindow._sharedObjectPagos.GetAttribute(this.Opcions.Srv_User);
      if (!string.IsNullOrEmpty(attribute))
      {
        string[] strArray = attribute.Split('|');
        if (strArray.Length >= 2)
        {
          if (strArray[0] == "true")
          {
            int num = 1;
            if (this.Opcions.ModoPlayCreditsGratuits == 1 && this.Opcions.CreditsGratuits > new Decimal(0))
            {
              this.Opcions.MissatgeCreditsGratuits = 1;
              num = 0;
            }
            if (num == 1)
            {
              int _pag = 0;
              try
              {
                _pag = int.Parse(strArray[1]);
              }
              catch
              {
              }
              if (this.Gestio_Pagament(_pag, 0) == 1)
                ;
            }
          }
          else
            this.Opcions.Pagar_Ticket_Busy = 0;
        }
        else
          this.Opcions.Pagar_Ticket_Busy = 0;
      }
    }

    public void Update_Server_Credits(Decimal _crd, bool _dis = true)
    {
      this.Opcions.Credits = _crd;
      if (this.Opcions.ModoTickets != 1)
        ;
      if (this.Opcions.ModoTickets == 0 && this.Opcions.News == 2)
        this.Opcions.Temps = 60 * ((int) this.Opcions.Credits / this.Opcions.ValorTemps);
      if (this.Opcions.NoCreditsInGame == 1)
      {
        if (this.Opcions.Credits > new Decimal(0))
        {
          if (this.StartupDetect == 1 || _dis)
          {
            this.Opcions.Enable_Lectors = -1;
            this.oldStopCredits = false;
            this.Opcions.StopCredits = true;
          }
        }
        else
          this.Opcions.StopCredits = false;
      }
      this.StartupDetect = 0;
    }

    private void _sharedObject_Sync(object sender, SyncEventArgs e)
    {
      foreach (Dictionary<string, object> change in e.ChangeList)
      {
        foreach (KeyValuePair<string, object> keyValuePair in change)
        {
          if (keyValuePair.Key == "name")
          {
            if ((string) keyValuePair.Value == "RemoteCmd")
            {
              try
              {
                this.Opcions.RemoteCmd = (string) this._sharedObject.GetAttribute("RemoteCmd");
              }
              catch
              {
              }
            }
            if ((string) keyValuePair.Value == "RemoteParam")
            {
              try
              {
                this.Opcions.RemoteParam = (string) this._sharedObject.GetAttribute("RemoteParam");
              }
              catch
              {
              }
            }
            if ((string) keyValuePair.Value == "saldo_cuenta")
            {
              try
              {
                this.Opcions.SaldoCredits = (Decimal) ((double) this._sharedObject.GetAttribute("saldo_cuenta"));
                if (this.Opcions.LockCredits == 0)
                {
                  if (this.Opcions.NoCreditsInGame == 1)
                  {
                    if (this.Opcions.InGame)
                    {
                      this.oldStopCredits = false;
                      this.Opcions.StopCredits = true;
                    }
                    else if (this.Opcions.Credits > new Decimal(0))
                    {
                      this.oldStopCredits = false;
                      this.Opcions.StopCredits = true;
                    }
                    else
                      this.Opcions.StopCredits = false;
                  }
                  else
                    this.Opcions.StopCredits = false;
                }
                else if (this.Opcions.NoCreditsInGame == 1)
                {
                  if (this.Opcions.InGame)
                  {
                    this.oldStopCredits = false;
                    this.Opcions.StopCredits = true;
                  }
                  else if (this.Opcions.SaldoCredits <= new Decimal(0))
                  {
                    this.oldStopCredits = false;
                    this.Opcions.StopCredits = true;
                  }
                  else if (this.Opcions.Credits > new Decimal(0))
                  {
                    this.oldStopCredits = false;
                    this.Opcions.StopCredits = true;
                  }
                  else
                    this.Opcions.StopCredits = false;
                }
                else
                  this.Opcions.StopCredits = this.Opcions.SaldoCredits <= new Decimal(0);
              }
              catch
              {
              }
            }
            if ((string) keyValuePair.Value == "CreditBono")
            {
              try
              {
                this.Opcions.CreditsGratuits = (Decimal) ((double) this._sharedObject.GetAttribute("CreditBono"));
              }
              catch
              {
              }
              if (this.Opcions.NoCreditsInGame == 1 && !(this.Opcions.Credits > new Decimal(0)))
                this.Opcions.StopCredits = false;
            }
            if ((string) keyValuePair.Value == "Credit")
            {
              try
              {
                this.Update_Server_Credits((Decimal) ((double) this._sharedObject.GetAttribute("Credit")), false);
              }
              catch
              {
              }
            }
            if ((string) keyValuePair.Value == "Jogant")
            {
              try
              {
                this.Opcions.InGame = (bool) this._sharedObject.GetAttribute("Jogant");
              }
              catch
              {
              }
            }
          }
          if (keyValuePair.Key == "code")
          {
            string str;
            if (keyValuePair.Value == (object) "clear")
              str = "chg";
            if (keyValuePair.Value == (object) "change")
              str = "chg";
          }
        }
      }
    }

    private void _sharedObject_NetStatus(object sender, NetStatusEventArgs e)
    {
      string str = e.Info["level"] as string;
      if (!(str == "error"))
        ;
      if (!(str == "status"))
        ;
    }

    private void Srv_Get_Room()
    {
      if (this._netConnection == null)
        return;
      if (!this._netConnection.Connected)
        this.Srv_Connect();
      this._Srv_Commad = MainWindow.Srv_Command.Room;
      try
      {
        this._netConnection.Call("QuioskVerUserSala", (IPendingServiceCallback) this, (object) this.Opcions.Srv_User);
      }
      catch
      {
      }
    }

    public void Srv_KioskCommand(int _err)
    {
      if (this.Opcions == null)
        return;
      if (this._Hook_Srv_KioskCommand == null)
        this._Hook_Srv_KioskCommand = new MainWindow.Hook_Srv_KioskCommand();
      if (this._Hook_Srv_KioskCommand.OK == -1)
        return;
      this.Srv_Connect();
      if (this._Srv_Commad != MainWindow.Srv_Command.Null && _err == 0)
        return;
      if (this._Hook_Srv_KioskCommand.OK != 0)
        ;
      this.Error_Servidor = 0;
      this._Hook_Srv_KioskCommand.OK = -1;
      this._Hook_Srv_KioskCommand.timeout = 0;
      this._Srv_Commad = MainWindow.Srv_Command.KioskCommand;
      try
      {
        this._netConnection.Call("QuioskSendCMD", (IPendingServiceCallback) this._Hook_Srv_KioskCommand, (object) this.Opcions.Srv_User, (object) "-", (object) "-");
      }
      catch (Exception ex)
      {
        this.ErrorGenericText = ex.Message;
        this._Hook_Srv_KioskCommand.OK = -1;
        this.Error_Servidor = 1100;
      }
    }

    public void Srv_KioskSetTime(int _segons, int _err)
    {
      if (_segons <= 0 || this.Opcions == null)
        return;
      if (this._Hook_Srv_KioskSetTime == null)
        this._Hook_Srv_KioskSetTime = new MainWindow.Hook_Srv_KioskSetTime();
      if (this._Hook_Srv_KioskSetTime.OK == -1)
        return;
      this.Srv_Connect();
      if (this._Srv_Commad != MainWindow.Srv_Command.Null && _err == 0)
        return;
      if (this._Hook_Srv_KioskSetTime.OK != 0)
        this._Hook_Srv_KioskSetTime.segons = _segons;
      this.Error_Servidor = 0;
      this._Hook_Srv_KioskSetTime.OK = -1;
      this._Hook_Srv_KioskSetTime.timeout = 0;
      this._Srv_Commad = MainWindow.Srv_Command.KioskSetTime;
      try
      {
        this._netConnection.Call("QuioskSetTime", (IPendingServiceCallback) this._Hook_Srv_KioskSetTime, (object) this.Opcions.Srv_User, (object) _segons);
      }
      catch (Exception ex)
      {
        this.ErrorGenericText = ex.Message;
        this._Hook_Srv_KioskSetTime.OK = -1;
        this.Error_Servidor = 702;
      }
    }

    public void Srv_KioskGetTime(int _err)
    {
      if (this.Opcions == null)
        return;
      if (this._Hook_Srv_KioskGetTime == null)
        this._Hook_Srv_KioskGetTime = new MainWindow.Hook_Srv_KioskGetTime();
      if (this._Hook_Srv_KioskGetTime.OK == -1)
        return;
      this.Srv_Connect();
      if (this._Srv_Commad != MainWindow.Srv_Command.Null && _err == 0)
        return;
      if (this._Hook_Srv_KioskGetTime.OK != 0)
        ;
      this.Error_Servidor = 0;
      this._Hook_Srv_KioskGetTime.OK = -1;
      this._Hook_Srv_KioskGetTime.timeout = 0;
      this._Srv_Commad = MainWindow.Srv_Command.KioskGetTime;
      try
      {
        this._netConnection.Call("QuioskGetTime", (IPendingServiceCallback) this._Hook_Srv_KioskGetTime, (object) this.Opcions.Srv_User);
      }
      catch (Exception ex)
      {
        this.ErrorGenericText = ex.Message;
        this._Hook_Srv_KioskGetTime.OK = -1;
        this.Error_Servidor = 702;
      }
    }

    public void Srv_Verificar_Ticket(int _tck, int _err)
    {
      if (_tck <= 0 || this.Opcions == null)
        return;
      if (this._Hook_Srv_Verificar_Ticket == null)
        this._Hook_Srv_Verificar_Ticket = new MainWindow.Hook_Srv_Verificar_Ticket();
      if (this._Hook_Srv_Verificar_Ticket.OK == -1)
        return;
      this.Srv_Connect();
      if (this._Srv_Commad != MainWindow.Srv_Command.Null && _err == 0)
        return;
      if (this._Hook_Srv_Verificar_Ticket.OK != 0)
        this._Hook_Srv_Verificar_Ticket.ticket = _tck;
      this.Error_Servidor = 0;
      this._Hook_Srv_Verificar_Ticket.OK = -1;
      this._Hook_Srv_Verificar_Ticket.timeout = 0;
      this._Srv_Commad = MainWindow.Srv_Command.VerificarTicket;
      try
      {
        this._netConnection.Call("QuioskVerTickPago", (IPendingServiceCallback) this._Hook_Srv_Verificar_Ticket, (object) this.Opcions.Srv_User, (object) _tck);
      }
      catch (Exception ex)
      {
        this.ErrorGenericText = ex.Message;
        this._Hook_Srv_Verificar_Ticket.OK = -1;
        this.Error_Servidor = 702;
      }
    }

    public void Srv_Anular_Ticket(int _tck, bool _est, int _err)
    {
      if (this.Opcions == null)
        return;
      if (this._Hook_Srv_Anular_Ticket == null)
        this._Hook_Srv_Anular_Ticket = new MainWindow.Hook_Srv_Anular_Ticket();
      if (this._Hook_Srv_Anular_Ticket.OK == -1)
        return;
      this.Srv_Connect();
      if (this._Srv_Commad != MainWindow.Srv_Command.Null && _err == 0)
        return;
      if (this._Hook_Srv_Anular_Ticket.OK != 0)
      {
        this._Hook_Srv_Anular_Ticket.ticket = _tck;
        this._Hook_Srv_Anular_Ticket.estat = _est;
      }
      this.Error_Servidor = 0;
      this._Hook_Srv_Anular_Ticket.OK = -1;
      this._Hook_Srv_Anular_Ticket.timeout = 0;
      this._Srv_Commad = MainWindow.Srv_Command.AnularTicket;
      try
      {
        this._netConnection.Call("QuioskAnulaTickPago", (IPendingServiceCallback) this._Hook_Srv_Anular_Ticket, (object) this.Opcions.Srv_User, (object) _tck, (object) _est);
      }
      catch (Exception ex)
      {
        this.ErrorGenericText = ex.Message;
        this._Hook_Srv_Anular_Ticket.OK = -1;
        this.Error_Servidor = 701;
      }
    }

    private void Srv_Credits(int _err)
    {
      if (this.Opcions == null)
        return;
      if (this._Hook_Srv_Credits == null)
        this._Hook_Srv_Credits = new MainWindow.Hook_Srv_Credits();
      if (this._Hook_Srv_Credits.OK == -1)
        return;
      this.Srv_Connect();
      if (this._Srv_Commad != MainWindow.Srv_Command.Null && _err == 0)
        return;
      if (this._Hook_Srv_Credits.OK > 0)
        ;
      this.Error_Servidor = 0;
      this._Hook_Srv_Credits.OK = -1;
      this._Hook_Srv_Credits.timeout = 0;
      this._Hook_Srv_Credits.credits = 0;
      this._Srv_Commad = MainWindow.Srv_Command.Credits;
      try
      {
        this._netConnection.Call("QuioskVerPasta", (IPendingServiceCallback) this._Hook_Srv_Credits, (object) this.Opcions.Srv_User);
      }
      catch (Exception ex)
      {
        this.ErrorGenericText = ex.Message;
        this._Hook_Srv_Credits.OK = -1;
        this.Error_Servidor = 201;
      }
    }

    private void Srv_Sub_Credits(Decimal _crd, int _err)
    {
      if (this.Opcions == null)
        return;
      if (this._Hook_Srv_Sub_Credits == null)
        this._Hook_Srv_Sub_Credits = new MainWindow.Hook_Srv_Sub_Credits();
      if (this._Hook_Srv_Sub_Credits.OK == -1)
        return;
      this.Srv_Connect();
      if (this._Srv_Commad != MainWindow.Srv_Command.Null && _err == 0)
        return;
      if (this._Hook_Srv_Sub_Credits.OK != 0)
      {
        ++this.Ticket_Add_Credits;
        if (this.Ticket_Add_Credits <= 0)
          ++this.Ticket_Add_Credits;
        this._Hook_Srv_Sub_Credits.ticket = this.Ticket_Add_Credits;
      }
      this.Error_Servidor = 0;
      this._Hook_Srv_Sub_Credits.OK = -1;
      this._Hook_Srv_Sub_Credits.timeout = 0;
      this._Srv_Commad = MainWindow.Srv_Command.SubCredits;
      try
      {
        this._netConnection.Call("QuioskCobrarParcialV2", (IPendingServiceCallback) this._Hook_Srv_Sub_Credits, (object) this.Opcions.Srv_User, (object) (int) _crd, (object) this._Hook_Srv_Sub_Credits.ticket);
      }
      catch (Exception ex)
      {
        this.ErrorGenericText = ex.Message;
        this._Hook_Srv_Sub_Credits.OK = -1;
        this.Error_Servidor = 301;
      }
    }

    private void Srv_Sub_Cadeaux(Decimal _crd, int _err)
    {
      if (this.Opcions == null)
        return;
      if (this._Hook_Srv_Sub_Cadeaux == null)
        this._Hook_Srv_Sub_Cadeaux = new MainWindow.Hook_Srv_Sub_Cadeaux();
      if (this._Hook_Srv_Sub_Cadeaux.OK == -1)
        return;
      this.Srv_Connect();
      if (this._Srv_Commad != MainWindow.Srv_Command.Null && _err == 0)
        return;
      if (this._Hook_Srv_Sub_Cadeaux.OK != 0)
      {
        ++this.Ticket_Add_Credits;
        if (this.Ticket_Add_Credits <= 0)
          ++this.Ticket_Add_Credits;
        this._Hook_Srv_Sub_Cadeaux.ticket = this.Ticket_Add_Credits;
      }
      this.Error_Servidor = 0;
      this._Hook_Srv_Sub_Cadeaux.OK = -1;
      this._Hook_Srv_Sub_Cadeaux.timeout = 0;
      this._Srv_Commad = MainWindow.Srv_Command.SubCadeaux;
      try
      {
        this._netConnection.Call("QuioskCobrarCadeau", (IPendingServiceCallback) this._Hook_Srv_Sub_Cadeaux, (object) this.Opcions.Srv_User, (object) (int) _crd, (object) this._Hook_Srv_Sub_Cadeaux.ticket, (object) this.Opcions.Disp_Pay_Ticket_Out_Flag);
      }
      catch (Exception ex)
      {
        this.ErrorGenericText = ex.Message;
        this._Hook_Srv_Sub_Cadeaux.OK = -1;
        this.Error_Servidor = 701;
      }
    }

    private void Srv_Add_Credits(Decimal _crd, int _err)
    {
      if (this.Opcions == null)
        return;
      if (this._Hook_Srv_Add_Credits == null)
        this._Hook_Srv_Add_Credits = new MainWindow.Hook_Srv_Add_Credits();
      if (this._Hook_Srv_Add_Credits.OK == -1)
        return;
      this.Srv_Connect();
      if (this._Srv_Commad != MainWindow.Srv_Command.Null && _err == 0)
        return;
      if (this._Hook_Srv_Add_Credits.OK != 0)
      {
        ++this.Ticket_Add_Credits;
        if (this.Ticket_Add_Credits <= 1)
          ++this.Ticket_Add_Credits;
        this._Hook_Srv_Add_Credits.ticket = this.Ticket_Add_Credits;
      }
      this.Error_Servidor = 0;
      this._Hook_Srv_Add_Credits.OK = -1;
      this._Hook_Srv_Add_Credits.timeout = 0;
      this._Srv_Commad = MainWindow.Srv_Command.AddCredits;
      try
      {
        this._netConnection.Call("QuioskPasta_Dec", (IPendingServiceCallback) this._Hook_Srv_Add_Credits, (object) this.Opcions.Srv_User, (object) (int) _crd, (object) this._Hook_Srv_Add_Credits.ticket);
      }
      catch (Exception ex)
      {
        this.ErrorGenericText = ex.Message;
        this._Hook_Srv_Add_Credits.OK = -1;
        this.Error_Servidor = 401;
      }
    }

    private void Srv_Test_Login(string _u, string _p, int _err)
    {
      if (this.Opcions == null)
        return;
      if (this._Hook_Srv_Login == null)
        this._Hook_Srv_Login = new MainWindow.Hook_Srv_Login();
      if (this._Hook_Srv_Login.OK == -1)
        return;
      this.Srv_Connect();
      this.Error_Servidor = 0;
      this._Hook_Srv_Login.ticket = 0;
      this._Hook_Srv_Login.timeout = 0;
      this._Hook_Srv_Login.OK = -1;
      this._Hook_Srv_Login.login = 0;
      if (_u == "-")
      {
        this._Hook_Srv_Login.OK = -2;
        this._Hook_Srv_Login.login = 2;
      }
      else
      {
        if (this._Srv_Commad != MainWindow.Srv_Command.Null && _err == 0)
          return;
        bool flag;
        try
        {
          flag = true;
          this._netConnection.Call("Login_User", (IPendingServiceCallback) this._Hook_Srv_Login, (object) _u, (object) _p);
        }
        catch (Exception ex)
        {
          flag = true;
          this.ErrorGenericText = ex.Message;
          this._Hook_Srv_Login.OK = -1;
          this.Error_Servidor = 601;
        }
      }
    }

    public void Srv_Sub_Ticket(int _id, int _err)
    {
      if (this.Opcions == null)
        return;
      if (this._Hook_Srv_Sub_Ticket == null)
        this._Hook_Srv_Sub_Ticket = new MainWindow.Hook_Srv_Sub_Ticket();
      if (this._Hook_Srv_Sub_Ticket.OK == -1)
        return;
      this.Srv_Connect();
      if (this._Srv_Commad != MainWindow.Srv_Command.Null && _err == 0)
        return;
      this.Error_Servidor = 0;
      this._Hook_Srv_Sub_Ticket.OK = -1;
      this._Hook_Srv_Sub_Ticket.timeout = 0;
      this._Srv_Commad = MainWindow.Srv_Command.SubTicket;
      try
      {
        this._netConnection.Call("QuioskDelTick", (IPendingServiceCallback) this._Hook_Srv_Sub_Ticket, (object) _id);
      }
      catch (Exception ex)
      {
        this.ErrorGenericText = ex.Message;
        this._Hook_Srv_Sub_Ticket.OK = -1;
        this.Error_Servidor = 301;
      }
    }

    private void Srv_Add_Ticket(int _crd, int _err)
    {
      if (this.Opcions == null)
        return;
      if (this._Hook_Srv_Add_Ticket == null)
        this._Hook_Srv_Add_Ticket = new MainWindow.Hook_Srv_Add_Ticket();
      if (this._Hook_Srv_Add_Ticket.OK == -1)
        return;
      this.Srv_Connect();
      if (this._Srv_Commad != MainWindow.Srv_Command.Null && _err == 0)
        return;
      this.Error_Servidor = 0;
      this._Hook_Srv_Add_Ticket.Ticket = 0;
      this._Hook_Srv_Add_Ticket.OK = -1;
      this._Hook_Srv_Add_Ticket.timeout = 0;
      this._Srv_Commad = MainWindow.Srv_Command.AddTicket;
      try
      {
        if (this.Opcions.Srv_Web_Ip.ToLower().Contains(this.Web_V2_A) || this.Opcions.Srv_Web_Ip.ToLower().Contains(this.Web_V2_B))
          this._netConnection.Call("QuioskNewTickV2", (IPendingServiceCallback) this._Hook_Srv_Add_Ticket, (object) this.Opcions.Srv_User, (object) _crd);
        else
          this._netConnection.Call("QuioskNewTick", (IPendingServiceCallback) this._Hook_Srv_Add_Ticket, (object) this.Opcions.Srv_User, (object) _crd);
      }
      catch (Exception ex)
      {
        this.ErrorGenericText = ex.Message;
        this._Hook_Srv_Add_Ticket.OK = -1;
        this.Error_Servidor = 301;
      }
    }

    private void Srv_Add_Pay(int _err)
    {
      if (this.Opcions == null)
        return;
      if (this._Hook_Srv_Add_Pay == null)
        this._Hook_Srv_Add_Pay = new MainWindow.Hook_Srv_Add_Pay();
      if (this._Hook_Srv_Add_Pay.OK == -1)
        return;
      this.Srv_Connect();
      if (this._Srv_Commad != MainWindow.Srv_Command.Null && _err == 0)
        return;
      this.Error_Servidor = 0;
      this._Hook_Srv_Add_Pay.Resposta = "";
      this._Hook_Srv_Add_Pay.Pay = 0;
      this._Hook_Srv_Add_Pay.OK = -1;
      this._Hook_Srv_Add_Pay.timeout = 0;
      this._Srv_Commad = MainWindow.Srv_Command.AddTicket;
      try
      {
        this._netConnection.Call("QuioskCobrarV2", (IPendingServiceCallback) this._Hook_Srv_Add_Pay, (object) this.Opcions.Srv_User);
      }
      catch (Exception ex)
      {
        this.ErrorGenericText = ex.Message;
        this._Hook_Srv_Add_Pay.OK = -1;
        this.Error_Servidor = 301;
      }
    }

    private void MainWindow_FormClosed(object sender, FormClosedEventArgs e)
    {
      this.Close_Devices();
      this.Opcions.Log_Debug("Shutdown");
      MainWindow.CtrlAltDel_Enable_Disable(true);
    }

    private void MainWindow_FormClosing(object sender, FormClosingEventArgs e)
    {
      if (e.CloseReason == CloseReason.UserClosing)
        e.Cancel = true;
    }

    [DllImport("gdi32.dll", SetLastError = true)]
    private static extern IntPtr CreateCompatibleDC(IntPtr hdc);

    [DllImport("gdi32.dll")]
    private static extern IntPtr CreateCompatibleBitmap(IntPtr hdc, int nWidth, int nHeight);

    [DllImport("gdi32.dll", SetLastError = true)]
    private static extern IntPtr SelectObject(IntPtr hdc, IntPtr hgdiobj);

    [DllImport("gdi32.dll")]
    private static extern bool DeleteDC(IntPtr hdc);

    public bool Print_ESCPOS(string printerName, byte[] document)
    {
      NativeMethods.DOC_INFO_1 di = new NativeMethods.DOC_INFO_1();
      di.pDataType = "RAW";
      di.pDocName = "Bit Image Test";
      IntPtr hPrinter = new IntPtr(0);
      if (!NativeMethods.OpenPrinter(printerName.Normalize(), out hPrinter, IntPtr.Zero))
        throw new Win32Exception();
      if (!NativeMethods.StartDocPrinter(hPrinter, 1, di))
        throw new Win32Exception();
      byte[] source = document;
      IntPtr num = Marshal.AllocCoTaskMem(source.Length);
      Marshal.Copy(source, 0, num, source.Length);
      if (!NativeMethods.StartPagePrinter(hPrinter))
        throw new Win32Exception();
      int dwWritten;
      NativeMethods.WritePrinter(hPrinter, num, source.Length, out dwWritten);
      NativeMethods.EndPagePrinter(hPrinter);
      Marshal.FreeCoTaskMem(num);
      NativeMethods.EndDocPrinter(hPrinter);
      NativeMethods.ClosePrinter(hPrinter);
      return true;
    }

    private static MainWindow.BitmapData GetBitmapData(Bitmap bmpFileName)
    {
      using (Bitmap bitmap = bmpFileName)
      {
        int maxValue = (int) sbyte.MaxValue;
        int index = 0;
        BitArray bitArray = new BitArray(bitmap.Width * bitmap.Height);
        for (int y = 0; y < bitmap.Height; ++y)
        {
          for (int x = 0; x < bitmap.Width; ++x)
          {
            System.Drawing.Color pixel = bitmap.GetPixel(x, y);
            int num = (int) ((double) pixel.R * 0.3 + (double) pixel.G * 0.59 + (double) pixel.B * 0.11);
            bitArray[index] = num < maxValue;
            ++index;
          }
        }
        return new MainWindow.BitmapData()
        {
          Dots = bitArray,
          Height = bitmap.Height,
          Width = bitmap.Width
        };
      }
    }

    private char[] Convert_String_Char(string _s)
    {
      return _s.ToCharArray();
    }

    public void _Old_Ticket_ESCPOS(
      string _ptr_device,
      Decimal _valor,
      int _tick,
      int _id,
      int _model,
      int _cut,
      int _skeep)
    {
      NIIClassLib niiClassLib = new NIIClassLib();
      Barcode barcode = new Barcode();
      this.rtest.BackColor = System.Drawing.Color.Yellow;
      this.rtest.ForeColor = System.Drawing.Color.Yellow;
      this.timerPrinter.Enabled = true;
      string StringToEncode = Gestion.Build_Mod10(this.Opcions.Srv_User, _tick, _id, 0);
      barcode.IncludeLabel = false;
      barcode.LabelFont = new Font("Arial", 20f);
      barcode.Alignment = AlignmentPositions.CENTER;
      barcode.RotateFlipType = RotateFlipType.RotateNoneFlipNone;
      barcode.LabelPosition = LabelPositions.TOPCENTER;
      MainWindow.BitmapData bitmapData = MainWindow.GetBitmapData(new Bitmap(barcode.Encode(TYPE.CODE128, StringToEncode, System.Drawing.Color.Black, System.Drawing.Color.White, 500, 90)));
      BitArray dots = bitmapData.Dots;
      byte[] bytes = BitConverter.GetBytes(bitmapData.Width);
      using (MemoryStream memoryStream = new MemoryStream())
      {
        using (BinaryWriter binaryWriter = new BinaryWriter((Stream) memoryStream))
        {
          binaryWriter.Write(Convert.ToChar(27));
          binaryWriter.Write(Convert.ToChar('@'));
          binaryWriter.Write(Convert.ToChar(27));
          binaryWriter.Write(Convert.ToChar('T'));
          binaryWriter.Write(Convert.ToChar(0));
          binaryWriter.Write('\n');
          binaryWriter.Write('\n');
          binaryWriter.Write('\n');
          binaryWriter.Write(Convert.ToChar(27));
          binaryWriter.Write(Convert.ToChar(33));
          binaryWriter.Write(Convert.ToChar(48));
          binaryWriter.Write(Convert.ToChar(27));
          binaryWriter.Write(Convert.ToChar(97));
          binaryWriter.Write(Convert.ToChar(1));
          binaryWriter.Write("-- La Belle Net --".ToCharArray());
          binaryWriter.Write('\n');
          DateTime now = DateTime.Now;
          binaryWriter.Write(string.Format("{0}/{1:00}/{2:0000} {3:00}:{4:00}", (object) now.Day, (object) now.Month, (object) now.Year, (object) now.Hour, (object) now.Minute).ToCharArray());
          binaryWriter.Write('\n');
          binaryWriter.Write('\x001B');
          binaryWriter.Write('a');
          binaryWriter.Write(char.MinValue);
          binaryWriter.Write('\x001B');
          binaryWriter.Write('!');
          binaryWriter.Write((byte) 160);
          binaryWriter.Write('\n');
          binaryWriter.Write(this.Opcions.Localize.Text("Location:").ToCharArray());
          binaryWriter.Write('\n');
          binaryWriter.Write('\x001B');
          binaryWriter.Write('!');
          binaryWriter.Write(char.MinValue);
          binaryWriter.Write(string.Format("  {0}", (object) this.Opcions.Srv_ID_Lin1).ToCharArray());
          binaryWriter.Write('\n');
          binaryWriter.Write(string.Format("  {0}", (object) this.Opcions.Srv_ID_Lin2).ToCharArray());
          binaryWriter.Write('\n');
          binaryWriter.Write(string.Format("  {0}", (object) this.Opcions.Srv_ID_Lin3).ToCharArray());
          binaryWriter.Write('\n');
          binaryWriter.Write(string.Format("  {0}", (object) this.Opcions.Srv_ID_Lin4).ToCharArray());
          binaryWriter.Write('\n');
          binaryWriter.Write(string.Format("  RC: {0}", (object) this.Opcions.Srv_ID_Lin5).ToCharArray());
          binaryWriter.Write('\n');
          binaryWriter.Write(string.Format("  " + this.Opcions.Localize.Text("Kiosk ID:") + " {0}", (object) this.Opcions.Srv_User).ToCharArray());
          binaryWriter.Write('\n');
          binaryWriter.Write('\x001B');
          binaryWriter.Write('!');
          binaryWriter.Write(char.MinValue);
          binaryWriter.Write('\x001B');
          binaryWriter.Write('a');
          binaryWriter.Write(char.MinValue);
          binaryWriter.Write("------------------------------------------".ToCharArray());
          binaryWriter.Write('\r');
          binaryWriter.Write('\n');
          binaryWriter.Write('\x001B');
          binaryWriter.Write('!');
          binaryWriter.Write('0');
          binaryWriter.Write(0);
          binaryWriter.Write(string.Format(this.Opcions.Localize.Text("Ticket:") + " {0}", (object) StringToEncode).ToCharArray());
          binaryWriter.Write('\n');
          TimeSpan timeSpan = new TimeSpan(0, 0, (int) (Decimal) _tick);
          string str = string.Format(this.Opcions.Localize.Text("Time: ") + " {0}:{1:00}:{2:00}", (object) timeSpan.Hours, (object) timeSpan.Minutes, (object) timeSpan.Seconds);
          if (_valor <= new Decimal(0))
            str = "TEST TICKET";
          binaryWriter.Write('\x001B');
          binaryWriter.Write('a');
          binaryWriter.Write('\x0001');
          binaryWriter.Write(str.ToCharArray());
          binaryWriter.Write('\n');
          binaryWriter.Write('\r');
          binaryWriter.Write('\n');
          binaryWriter.Write(0);
          binaryWriter.Write('\x001B');
          binaryWriter.Write('3');
          binaryWriter.Write((byte) 24);
          int num1 = 0;
          while (num1 < bitmapData.Height)
          {
            binaryWriter.Write('\x001B');
            binaryWriter.Write('*');
            binaryWriter.Write((byte) 33);
            binaryWriter.Write(bytes[0]);
            binaryWriter.Write(bytes[1]);
            for (int index1 = 0; index1 < bitmapData.Width; ++index1)
            {
              for (int index2 = 0; index2 < 3; ++index2)
              {
                byte num2 = 0;
                for (int index3 = 0; index3 < 8; ++index3)
                {
                  int index4 = ((num1 / 8 + index2) * 8 + index3) * bitmapData.Width + index1;
                  bool flag = false;
                  if (index4 < dots.Length)
                    flag = dots[index4];
                  num2 |= (byte) ((flag ? 1 : 0) << 7 - index3);
                }
                binaryWriter.Write(num2);
              }
            }
            num1 += 24;
            binaryWriter.Write('\n');
          }
          binaryWriter.Write('\x001B');
          binaryWriter.Write('3');
          binaryWriter.Write((byte) 30);
          binaryWriter.Write('\x001B');
          binaryWriter.Write('a');
          binaryWriter.Write('\x0001');
          binaryWriter.Write('\x001B');
          binaryWriter.Write('!');
          binaryWriter.Write('0');
          binaryWriter.Write('\n');
          binaryWriter.Write(this.Opcions.Localize.Text("THANKS").ToCharArray());
          binaryWriter.Write('\n');
          binaryWriter.Write('\x001B');
          binaryWriter.Write('!');
          binaryWriter.Write(char.MinValue);
          binaryWriter.Write(this.Opcions.Srv_ID_LinBottom.ToCharArray());
          binaryWriter.Write('\n');
          binaryWriter.Write("------------------------------------------".ToCharArray());
          binaryWriter.Write('\n');
          binaryWriter.Write('\x001B');
          binaryWriter.Write('a');
          binaryWriter.Write(char.MinValue);
          binaryWriter.Write('\x001B');
          binaryWriter.Write('!');
          binaryWriter.Write(char.MinValue);
          for (int index = 0; index < _skeep; ++index)
            binaryWriter.Write('\n');
          if (_cut == 1)
          {
            binaryWriter.Write((byte) 27);
            binaryWriter.Write((byte) 74);
            binaryWriter.Write((byte) 120);
            binaryWriter.Write((byte) 27);
            binaryWriter.Write((byte) 105);
          }
          binaryWriter.Write('\x001D');
          binaryWriter.Write('V');
          binaryWriter.Write((byte) 66);
          binaryWriter.Write((byte) 3);
          binaryWriter.Flush();
          this.Print_ESCPOS(_ptr_device, memoryStream.ToArray());
        }
      }
    }

    private void PrinterErrorShow(int _err, string _cod = "")
    {
      this.Opcions.MissatgePrinter = "";
      switch (_err)
      {
        case 0:
          this.Opcions.MissatgePrinter = this.Opcions.Localize.Text("ERROR: PRINTER NOT INSTALLED");
          break;
        case 1:
          this.Opcions.MissatgePrinter = this.Opcions.Localize.Text("ERROR: PRINTER OFFLINE OR NOT PRESENT");
          break;
        case 2:
          this.Opcions.MissatgePrinter = this.Opcions.Localize.Text("ERROR: PRINTER, CALL ATTENDANT (") + _cod + ")";
          break;
      }
    }

    public bool Ticket(
      string _ptr_device,
      Decimal _valor,
      int _tick,
      int _id,
      int _model,
      int _cut,
      int _skeep,
      int _preskeep,
      int _wide)
    {
      if (this.Opcions.ModoTickets == 0 && _id != 0)
        return false;
      if (string.IsNullOrEmpty(_ptr_device))
      {
        this.PrinterErrorShow(0, "");
        return false;
      }
      if (!this.Exist_Printer(_ptr_device))
      {
        this.PrinterErrorShow(1, "");
        return false;
      }
      if (!this.PaperOut_Printer(_ptr_device))
      {
        this.PrinterErrorShow(2, this.errtick);
        return false;
      }
      this.Ticket_Reset(_ptr_device);
      Tickets tickets = new Tickets();
      Tickets.Info_Ticket _text = new Tickets.Info_Ticket();
      _text.TXT_Null = this.Opcions.Localize.Text("Repeated tickets are null");
      _text.TXT_Valid = this.Opcions.Localize.Text("Valid only for 15 minutes");
      _text.TXT_Valid2 = this.Opcions.Localize.Text("Change only for gift check");
      _text.TXT_Bottom = this.Opcions.Srv_ID_LinBottom;
      _text.TXT_Thanks = this.Opcions.Localize.Text("THANKS");
      _text.TXT_Lin1 = this.Opcions.Srv_ID_Lin1;
      _text.TXT_Lin2 = this.Opcions.Srv_ID_Lin2;
      _text.TXT_Lin3 = this.Opcions.Srv_ID_Lin3;
      _text.TXT_Lin4 = this.Opcions.Srv_ID_Lin4;
      _text.TXT_Lin5 = this.Opcions.Srv_ID_Lin5;
      _text.TXT_Location = this.Opcions.Localize.Text("Depositary");
      _text.TXT_BorneID = this.Opcions.Localize.Text("KIOSK");
      _text.TXT_Time = this.Opcions.Localize.Text("TIME");
      _text.TXT_Ticket = this.Opcions.Localize.Text("Fact");
      _text.TXT_Points = this.Opcions.Localize.Text("Euros");
      if (string.IsNullOrEmpty(_text.TXT_Lin1))
        _text.TXT_Lin1 = " ";
      if (string.IsNullOrEmpty(_text.TXT_Lin2))
        _text.TXT_Lin2 = " ";
      if (string.IsNullOrEmpty(_text.TXT_Lin3))
        _text.TXT_Lin3 = " ";
      if (string.IsNullOrEmpty(_text.TXT_Lin4))
        _text.TXT_Lin4 = " ";
      if (string.IsNullOrEmpty(_text.TXT_Lin5))
        _text.TXT_Lin5 = " ";
      tickets.Ticket_ESCPOS(_ptr_device, _valor, _tick, _id, _model, _cut, _skeep, _preskeep, _wide, 1, DateTime.Now, this.Opcions.Srv_User, _text);
      return true;
    }

    public bool _Old_Ticket(
      string _ptr_device,
      Decimal _valor,
      int _tick,
      int _id,
      int _model,
      int _cut,
      int _skeep)
    {
      if (this.Opcions.ModoTickets == 0 && _id != 0)
        return false;
      if (!this.Exist_Printer(_ptr_device))
      {
        this.PrinterErrorShow(1, "");
        return false;
      }
      if (!this.PaperOut_Printer(_ptr_device))
      {
        this.PrinterErrorShow(2, this.errtick);
        return false;
      }
      this.Ticket_Reset(_ptr_device);
      if (_model == 1)
      {
        this._Old_Ticket_ESCPOS(_ptr_device, _valor, _tick, _id, _model, _cut, _skeep);
        return true;
      }
      NIIClassLib niiClassLib = new NIIClassLib();
      Barcode barcode = new Barcode();
      this.rtest.BackColor = System.Drawing.Color.Yellow;
      this.rtest.ForeColor = System.Drawing.Color.Yellow;
      this.timerPrinter.Enabled = true;
      string StringToEncode = Gestion.Build_Mod10(this.Opcions.Srv_User, _tick, _id, 0);
      barcode.IncludeLabel = false;
      barcode.LabelFont = new Font("Arial", 20f);
      barcode.Alignment = AlignmentPositions.CENTER;
      barcode.RotateFlipType = RotateFlipType.RotateNoneFlipNone;
      barcode.LabelPosition = LabelPositions.TOPCENTER;
      Bitmap bitmap = new Bitmap(barcode.Encode(TYPE.CODE128, StringToEncode, System.Drawing.Color.Black, System.Drawing.Color.White, 580, 100));
      IntPtr compatibleDc = MainWindow.CreateCompatibleDC(IntPtr.Zero);
      IntPtr compatibleBitmap = MainWindow.CreateCompatibleBitmap(compatibleDc, 580, 100);
      MainWindow.SelectObject(compatibleDc, compatibleBitmap);
      ImageRasterHelper.ConvertHBitmap(compatibleDc, bitmap, true);
      long o_jobid;
      int num;
      try
      {
        num = niiClassLib.NiiStartDoc(_ptr_device, out o_jobid);
      }
      catch (Exception ex)
      {
        MainWindow.DeleteDC(compatibleDc);
        return false;
      }
      num = niiClassLib.NiiPrint(_ptr_device, "1B2130", 6L, out o_jobid);
      num = niiClassLib.NiiPrint(_ptr_device, "1B6101", 6L, out o_jobid);
      string i_dat1 = "0a\"-- La Belle Net --\"0a";
      num = niiClassLib.NiiPrint(_ptr_device, i_dat1, (long) i_dat1.Length, out o_jobid);
      DateTime now = DateTime.Now;
      string i_dat2 = string.Format("\"{0}/{1:00}/{2:0000} {3:00}:{4:00}\"0a", (object) now.Day, (object) now.Month, (object) now.Year, (object) now.Hour, (object) now.Minute);
      num = niiClassLib.NiiPrint(_ptr_device, i_dat2, (long) i_dat2.Length, out o_jobid);
      num = niiClassLib.NiiPrint(_ptr_device, "1B21a0", 6L, out o_jobid);
      num = niiClassLib.NiiPrint(_ptr_device, "1B6100", 6L, out o_jobid);
      string i_dat3 = string.Format("0a\"" + this.Opcions.Localize.Text("Location:") + "\"0a");
      num = niiClassLib.NiiPrint(_ptr_device, i_dat3, (long) i_dat3.Length, out o_jobid);
      num = niiClassLib.NiiPrint(_ptr_device, "1B2100", 6L, out o_jobid);
      string i_dat4 = string.Format("\"  {0}\"0a", (object) this.Opcions.Srv_ID_Lin1);
      num = niiClassLib.NiiPrint(_ptr_device, i_dat4, (long) i_dat4.Length, out o_jobid);
      string i_dat5 = string.Format("\"  {0}\"0a", (object) this.Opcions.Srv_ID_Lin2);
      num = niiClassLib.NiiPrint(_ptr_device, i_dat5, (long) i_dat5.Length, out o_jobid);
      string i_dat6 = string.Format("\"  {0}\"0a", (object) this.Opcions.Srv_ID_Lin3);
      num = niiClassLib.NiiPrint(_ptr_device, i_dat6, (long) i_dat6.Length, out o_jobid);
      string i_dat7 = string.Format("\"  {0}\"0a", (object) this.Opcions.Srv_ID_Lin4);
      num = niiClassLib.NiiPrint(_ptr_device, i_dat7, (long) i_dat7.Length, out o_jobid);
      string i_dat8 = string.Format("\"  RC: {0}\"0a", (object) this.Opcions.Srv_ID_Lin5);
      num = niiClassLib.NiiPrint(_ptr_device, i_dat8, (long) i_dat8.Length, out o_jobid);
      string i_dat9 = string.Format("\"  " + this.Opcions.Localize.Text("Kiosk ID:") + " {0}\"0a", (object) this.Opcions.Srv_User);
      num = niiClassLib.NiiPrint(_ptr_device, i_dat9, (long) i_dat9.Length, out o_jobid);
      num = niiClassLib.NiiPrint(_ptr_device, "1B6101", 6L, out o_jobid);
      string i_dat10 = string.Format("\"--------------------------------------------\"0a");
      num = niiClassLib.NiiPrint(_ptr_device, i_dat10, (long) i_dat10.Length, out o_jobid);
      num = niiClassLib.NiiPrint(_ptr_device, "1B2130", 6L, out o_jobid);
      string i_dat11 = string.Format("\"" + this.Opcions.Localize.Text("Ticket:") + " {0}\"0a", (object) StringToEncode);
      num = niiClassLib.NiiPrint(_ptr_device, i_dat11, (long) i_dat11.Length, out o_jobid);
      TimeSpan timeSpan = new TimeSpan(0, 0, (int) (Decimal) _tick);
      string i_dat12 = string.Format("\"" + this.Opcions.Localize.Text("Time: ") + " {0}:{1:00}:{2:00}\"0a", (object) timeSpan.Hours, (object) timeSpan.Minutes, (object) timeSpan.Seconds);
      if (_valor <= new Decimal(0))
        i_dat12 = "\"TEST TICKET\"0a";
      num = niiClassLib.NiiPrint(_ptr_device, i_dat12, (long) i_dat12.Length, out o_jobid);
      string i_dat13 = string.Format("0a");
      num = niiClassLib.NiiPrint(_ptr_device, i_dat13, (long) i_dat13.Length, out o_jobid);
      niiClassLib.NiiImagePrintEx(_ptr_device, compatibleDc, 580, 100, (byte) 1, out o_jobid);
      num = niiClassLib.NiiPrint(_ptr_device, "1B2130", 6L, out o_jobid);
      string i_dat14 = string.Format("0a\"" + this.Opcions.Localize.Text("THANKS") + "\"0a", (object) "bar12x12");
      num = niiClassLib.NiiPrint(_ptr_device, i_dat14, (long) i_dat14.Length, out o_jobid);
      num = niiClassLib.NiiPrint(_ptr_device, "1B2100", 6L, out o_jobid);
      string i_dat15 = string.Format("\"" + this.Opcions.Srv_ID_LinBottom + "\"0a");
      num = niiClassLib.NiiPrint(_ptr_device, i_dat15, (long) i_dat15.Length, out o_jobid);
      string i_dat16 = string.Format("\"--------------------------------------------\"0a");
      num = niiClassLib.NiiPrint(_ptr_device, i_dat16, (long) i_dat16.Length, out o_jobid);
      num = niiClassLib.NiiPrint(_ptr_device, "1B2100", 6L, out o_jobid);
      num = niiClassLib.NiiPrint(_ptr_device, "1B4A781b69", 10L, out o_jobid);
      num = niiClassLib.NiiPrint(_ptr_device, "1B723160", 8L, out o_jobid);
      num = niiClassLib.NiiPrint(_ptr_device, "1D\"G\"10", 7L, out o_jobid);
      num = niiClassLib.NiiEndDoc(_ptr_device);
      MainWindow.DeleteDC(compatibleDc);
      return true;
    }

    private bool Ticket_Reset(string _ptr_device)
    {
      if (this.Opcions.ModoTickets == 0 && !this.Opcions.RunConfig || string.IsNullOrEmpty(_ptr_device) || !this.Exist_Printer(_ptr_device))
        return false;
      PrintQueue printQueue = new LocalPrintServer().GetPrintQueue(_ptr_device);
      printQueue.Refresh();
      if (printQueue.NumberOfJobs > 0)
      {
        foreach (PrintSystemJobInfo printJobInfo in printQueue.GetPrintJobInfoCollection())
          printJobInfo.Cancel();
      }
      return true;
    }

    private int Wait_Ticket_Poll()
    {
      PrintQueue printQueue = new LocalPrintServer().GetPrintQueue(this.Opcions.Impresora_Tck);
      printQueue.GetPrintJobInfoCollection();
      if (printQueue.NumberOfJobs <= 0)
        return 0;
      return printQueue.IsPrinting ? 2 : 1;
    }

    private int Ticket_Poll()
    {
      PrintQueue printQueue = new LocalPrintServer().GetPrintQueue(this.Opcions.Impresora_Tck);
      printQueue.GetPrintJobInfoCollection();
      if (printQueue.NumberOfJobs > 0)
      {
        if (printQueue.IsPrinting)
        {
          this.rtest.BackColor = System.Drawing.Color.Blue;
          this.rtest.ForeColor = System.Drawing.Color.Blue;
          return 1;
        }
        this.rtest.BackColor = System.Drawing.Color.Green;
        this.rtest.ForeColor = System.Drawing.Color.Green;
        return 0;
      }
      this.rtest.BackColor = System.Drawing.Color.White;
      this.rtest.ForeColor = System.Drawing.Color.White;
      this.timerPrinter.Enabled = false;
      return 0;
    }

    private bool _Old_Ticket_Out_ESCPOS(
      string _ptr_device,
      Decimal _valor,
      int _id,
      int _model,
      int _cut,
      int _skeep,
      int _preskeep = 0)
    {
      string str = Gestion.Build_Mod10(this.Opcions.Srv_User, (int) _valor, _id, 1);
      using (MemoryStream memoryStream = new MemoryStream())
      {
        using (BinaryWriter binaryWriter = new BinaryWriter((Stream) memoryStream))
        {
          binaryWriter.Write(Convert.ToChar(27));
          binaryWriter.Write(Convert.ToChar('@'));
          binaryWriter.Write(Convert.ToChar(27));
          binaryWriter.Write(Convert.ToChar('T'));
          binaryWriter.Write(Convert.ToChar(0));
          binaryWriter.Write('\n');
          binaryWriter.Write('\n');
          binaryWriter.Write('\n');
          binaryWriter.Write(Convert.ToChar(27));
          binaryWriter.Write(Convert.ToChar(33));
          binaryWriter.Write(Convert.ToChar(48));
          binaryWriter.Write(Convert.ToChar(27));
          binaryWriter.Write(Convert.ToChar(97));
          binaryWriter.Write(Convert.ToChar(1));
          binaryWriter.Write("-- La Belle Net --".ToCharArray());
          binaryWriter.Write('\n');
          binaryWriter.Write('\n');
          DateTime now = DateTime.Now;
          binaryWriter.Write(string.Format("{0}/{1:00}/{2:0000} {3:00}:{4:00}", (object) now.Day, (object) now.Month, (object) now.Year, (object) now.Hour, (object) now.Minute).ToCharArray());
          binaryWriter.Write('\n');
          binaryWriter.Write('\x001B');
          binaryWriter.Write('!');
          binaryWriter.Write((byte) 48);
          binaryWriter.Write(this.Opcions.Localize.Text("CONGRATULATIONS").ToCharArray());
          binaryWriter.Write('\n');
          binaryWriter.Write('\x001B');
          binaryWriter.Write('!');
          binaryWriter.Write((byte) 16);
          binaryWriter.Write(this.Opcions.Localize.Text("Ticket value").ToCharArray());
          binaryWriter.Write('\n');
          int num = (int) _valor;
          binaryWriter.Write('\x001B');
          binaryWriter.Write('!');
          binaryWriter.Write((byte) 48);
          binaryWriter.Write(string.Format("{0}.{1:00} " + this.Opcions.Localize.Text("Euros"), (object) (num / 100), (object) (num % 100)).ToCharArray());
          binaryWriter.Write('\n');
          binaryWriter.Write('\x001B');
          binaryWriter.Write('!');
          binaryWriter.Write((byte) 48);
          binaryWriter.Write(string.Format("*{0}*", (object) str).ToCharArray());
          binaryWriter.Write('\n');
          binaryWriter.Write('\x001B');
          binaryWriter.Write('!');
          binaryWriter.Write((byte) 48);
          binaryWriter.Write(string.Format("ID: {0}", (object) _id));
          binaryWriter.Write('\n');
          binaryWriter.Write('\x001B');
          binaryWriter.Write('a');
          binaryWriter.Write(char.MinValue);
          binaryWriter.Write('\x001B');
          binaryWriter.Write('!');
          binaryWriter.Write((byte) 160);
          binaryWriter.Write('\n');
          binaryWriter.Write(this.Opcions.Localize.Text("Location:").ToCharArray());
          binaryWriter.Write('\n');
          binaryWriter.Write('\x001B');
          binaryWriter.Write('!');
          binaryWriter.Write(char.MinValue);
          binaryWriter.Write(string.Format("  {0}", (object) this.Opcions.Srv_ID_Lin1).ToCharArray());
          binaryWriter.Write('\n');
          binaryWriter.Write(string.Format("  {0}", (object) this.Opcions.Srv_ID_Lin2).ToCharArray());
          binaryWriter.Write('\n');
          binaryWriter.Write(string.Format("  {0}", (object) this.Opcions.Srv_ID_Lin3).ToCharArray());
          binaryWriter.Write('\n');
          binaryWriter.Write(string.Format("  {0}", (object) this.Opcions.Srv_ID_Lin4).ToCharArray());
          binaryWriter.Write('\n');
          binaryWriter.Write(string.Format("  RC: {0}", (object) this.Opcions.Srv_ID_Lin5).ToCharArray());
          binaryWriter.Write('\n');
          binaryWriter.Write(string.Format("  " + this.Opcions.Localize.Text("Kiosk ID:") + " {0}", (object) this.Opcions.Srv_User).ToCharArray());
          binaryWriter.Write('\n');
          binaryWriter.Write('\x001B');
          binaryWriter.Write('a');
          binaryWriter.Write('\x0001');
          binaryWriter.Write('\x001B');
          binaryWriter.Write('!');
          binaryWriter.Write('0');
          binaryWriter.Write('\n');
          binaryWriter.Write(this.Opcions.Localize.Text("THANKS").ToCharArray());
          binaryWriter.Write('\n');
          binaryWriter.Write('\x001B');
          binaryWriter.Write('!');
          binaryWriter.Write(char.MinValue);
          binaryWriter.Write(this.Opcions.Srv_ID_LinBottom.ToCharArray());
          binaryWriter.Write('\n');
          binaryWriter.Write("------------------------------------------".ToCharArray());
          binaryWriter.Write('\n');
          binaryWriter.Write('\x001B');
          binaryWriter.Write('a');
          binaryWriter.Write(char.MinValue);
          binaryWriter.Write('\x001B');
          binaryWriter.Write('!');
          binaryWriter.Write(char.MinValue);
          for (int index = 0; index < _skeep; ++index)
            binaryWriter.Write('\n');
          if (_cut == 1)
          {
            binaryWriter.Write((byte) 27);
            binaryWriter.Write((byte) 74);
            binaryWriter.Write((byte) 120);
            binaryWriter.Write((byte) 27);
            binaryWriter.Write((byte) 105);
          }
          binaryWriter.Write('\x001D');
          binaryWriter.Write('V');
          binaryWriter.Write((byte) 66);
          binaryWriter.Write((byte) 3);
          binaryWriter.Flush();
          return this.Print_ESCPOS(_ptr_device, memoryStream.ToArray());
        }
      }
    }

    private bool Buscar_Ticket(int _id)
    {
      if (this.Cache_Tickets == null || this.Cache_Tickets.Length == 0)
        return false;
      for (int index = 0; index < this.Cache_Tickets.Length; ++index)
      {
        if (this.Cache_Tickets[index] == _id)
          return true;
      }
      return false;
    }

    private void Add_Buscar_Ticket(int _id)
    {
      if (this.Cache_Tickets == null)
      {
        this.Cache_Tickets = new int[1];
        this.Cache_Tickets[0] = _id;
      }
      else
      {
        if (this.Buscar_Ticket(_id))
          return;
        Array.Resize<int>(ref this.Cache_Tickets, this.Cache_Tickets.Length + 1);
        this.Cache_Tickets[this.Cache_Tickets.Length - 1] = _id;
      }
    }

    public bool Ticket_Out(
      string _ptr_device,
      Decimal _valor,
      int _id,
      int _model,
      int _cut,
      int _skeep,
      int _preskeep,
      int _wide,
      DateTime _temps,
      string _cod,
      int _join)
    {
      if (this.Opcions.ModoTickets == 0 && _id != 0)
        return false;
      if (string.IsNullOrEmpty(_ptr_device))
      {
        this.PrinterErrorShow(0, "");
        return false;
      }
      if (!this.Exist_Printer(_ptr_device))
      {
        this.PrinterErrorShow(1, "");
        return false;
      }
      if (!this.PaperOut_Printer(_ptr_device))
      {
        this.PrinterErrorShow(2, this.errtick);
        return false;
      }
      this.Ticket_Reset(_ptr_device);
      if (this._Last_Id == _id || this.Buscar_Ticket(_id))
        return true;
      this.Add_Buscar_Ticket(_id);
      this._Last_Id = _id;
      Tickets tickets = new Tickets();
      Tickets.Info_Ticket _text = new Tickets.Info_Ticket();
      _text.TXT_Null = this.Opcions.Localize.Text("Repeated tickets are null");
      _text.TXT_Valid = this.Opcions.Localize.Text("Valid only for 15 minutes");
      _text.TXT_Valid2 = this.Opcions.Localize.Text("Change only for gift check");
      _text.TXT_Bottom = this.Opcions.Srv_ID_LinBottom;
      _text.TXT_Thanks = this.Opcions.Localize.Text("THANKS");
      _text.TXT_Lin1 = this.Opcions.Srv_ID_Lin1;
      _text.TXT_Lin2 = this.Opcions.Srv_ID_Lin2;
      _text.TXT_Lin3 = this.Opcions.Srv_ID_Lin3;
      _text.TXT_Lin4 = this.Opcions.Srv_ID_Lin4;
      _text.TXT_Lin5 = this.Opcions.Srv_ID_Lin5;
      _text.TXT_Location = this.Opcions.Localize.Text("Depositary");
      _text.TXT_BorneID = this.Opcions.Localize.Text("KIOSK");
      _text.TXT_Time = this.Opcions.Localize.Text("TIME");
      _text.TXT_Ticket = this.Opcions.Localize.Text("Fact");
      _text.TXT_Points = this.Opcions.Localize.Text("Euros");
      if (string.IsNullOrEmpty(_text.TXT_Lin1))
        _text.TXT_Lin1 = " ";
      if (string.IsNullOrEmpty(_text.TXT_Lin2))
        _text.TXT_Lin2 = " ";
      if (string.IsNullOrEmpty(_text.TXT_Lin3))
        _text.TXT_Lin3 = " ";
      if (string.IsNullOrEmpty(_text.TXT_Lin4))
        _text.TXT_Lin4 = " ";
      if (string.IsNullOrEmpty(_text.TXT_Lin5))
        _text.TXT_Lin5 = " ";
      tickets.Ticket_Out_ESCPOS(_ptr_device, _valor, _id, _model, _cut, _skeep, _preskeep, _wide, 1, _temps, _cod, this.Opcions.Srv_User, _text, this.Opcions.TicketHidePay, _join);
      this.Opcions.ticketCleanTemps = 1;
      return true;
    }

    public bool Ticket_Out_Mes_Temps(
      string _ptr_device,
      Decimal _valor,
      int _id,
      int _model,
      int _cut,
      int _skeep,
      int _preskeep,
      int _wide,
      DateTime _temps,
      string _cod,
      int _join,
      int _tick_temps)
    {
      if (this.Opcions.ModoTickets == 0 && _id != 0)
        return false;
      if (string.IsNullOrEmpty(_ptr_device))
      {
        this.PrinterErrorShow(0, "");
        return false;
      }
      if (!this.Exist_Printer(_ptr_device))
      {
        this.PrinterErrorShow(1, "");
        return false;
      }
      if (!this.PaperOut_Printer(_ptr_device))
      {
        this.PrinterErrorShow(2, this.errtick);
        return false;
      }
      this.Ticket_Reset(_ptr_device);
      if (this._Last_Id == _id || this.Buscar_Ticket(_id))
        return true;
      this.Add_Buscar_Ticket(_id);
      this._Last_Id = _id;
      Tickets tickets = new Tickets();
      Tickets.Info_Ticket _text = new Tickets.Info_Ticket();
      _text.TXT_Null = this.Opcions.Localize.Text("Repeated tickets are null");
      _text.TXT_Valid = this.Opcions.Localize.Text("Valid only for 15 minutes");
      _text.TXT_Valid2 = this.Opcions.Localize.Text("Change only for gift check");
      _text.TXT_Bottom = this.Opcions.Srv_ID_LinBottom;
      _text.TXT_Thanks = this.Opcions.Localize.Text("THANKS");
      _text.TXT_Lin1 = this.Opcions.Srv_ID_Lin1;
      _text.TXT_Lin2 = this.Opcions.Srv_ID_Lin2;
      _text.TXT_Lin3 = this.Opcions.Srv_ID_Lin3;
      _text.TXT_Lin4 = this.Opcions.Srv_ID_Lin4;
      _text.TXT_Lin5 = this.Opcions.Srv_ID_Lin5;
      _text.TXT_Location = this.Opcions.Localize.Text("Depositary");
      _text.TXT_BorneID = this.Opcions.Localize.Text("KIOSK");
      _text.TXT_Time = this.Opcions.Localize.Text("TIME");
      _text.TXT_Ticket = this.Opcions.Localize.Text("Fact");
      _text.TXT_Points = this.Opcions.Localize.Text("Euros");
      _text.TXT_GAS0 = this.Opcions.Localize.Text("Change for");
      _text.TXT_GAS1 = this.Opcions.Localize.Text("GAS");
      _text.TXT_GAS2 = this.Opcions.Localize.Text("Merchandise");
      _text.TXT_GAS3 = this.Opcions.Localize.Text("Xec Gift");
      _text.TXT_GAS4 = this.Opcions.Localize.Text(" to ");
      _text.TXT_GAS5 = this.Opcions.Localize.Text(" hours");
      _text.TXT_GAS6 = this.Opcions.Localize.Text("Valid until");
      if (string.IsNullOrEmpty(_text.TXT_Lin1))
        _text.TXT_Lin1 = " ";
      if (string.IsNullOrEmpty(_text.TXT_Lin2))
        _text.TXT_Lin2 = " ";
      if (string.IsNullOrEmpty(_text.TXT_Lin3))
        _text.TXT_Lin3 = " ";
      if (string.IsNullOrEmpty(_text.TXT_Lin4))
        _text.TXT_Lin4 = " ";
      if (string.IsNullOrEmpty(_text.TXT_Lin5))
        _text.TXT_Lin5 = " ";
      if (this.Opcions.Ticket_Carburante == 1)
        tickets.Ticket_Out_Mes_Temps_GAS_ESCPOS(_ptr_device, _valor, _id, _model, _cut, _skeep, _preskeep, _wide, 1, _temps, _cod, this.Opcions.Srv_User, _text, this.Opcions.TicketHidePay, _join, _tick_temps);
      else
        tickets.Ticket_Out_Mes_Temps_ESCPOS(_ptr_device, _valor, _id, _model, _cut, _skeep, _preskeep, _wide, 1, _temps, _cod, this.Opcions.Srv_User, _text, this.Opcions.TicketHidePay, _join, _tick_temps);
      this.Opcions.ticketCleanTemps = 1;
      return true;
    }

    public bool Ticket_Out_Check(
      string _ptr_device,
      Decimal _valor,
      int _id,
      int _model,
      int _cut,
      int _skeep,
      int _preskeep,
      int _wide,
      DateTime _temps,
      string _cod,
      int _ntick,
      int _join)
    {
      if (this.Opcions.ModoTickets == 0 && _id != 0)
        return false;
      if (string.IsNullOrEmpty(_ptr_device))
      {
        this.PrinterErrorShow(0, "");
        return false;
      }
      if (!this.Exist_Printer(_ptr_device))
      {
        this.PrinterErrorShow(1, "");
        return false;
      }
      if (!this.PaperOut_Printer(_ptr_device))
      {
        this.PrinterErrorShow(2, this.errtick);
        return false;
      }
      this.Ticket_Reset(_ptr_device);
      if (this._Last_Id == _id || this.Buscar_Ticket(_id))
        return true;
      this.Add_Buscar_Ticket(_id);
      this._Last_Id = _id;
      Tickets tickets = new Tickets();
      Tickets.Info_Ticket _text = new Tickets.Info_Ticket();
      _text.TXT_Null = this.Opcions.Localize.Text("Repeated tickets are null");
      _text.TXT_Valid = this.Opcions.Localize.Text("Valid only for 15 minutes");
      _text.TXT_Valid2 = this.Opcions.Localize.Text("Change only for gift check");
      _text.TXT_Bottom = this.Opcions.Srv_ID_LinBottom;
      _text.TXT_Thanks = this.Opcions.Localize.Text("THANKS");
      _text.TXT_Lin1 = this.Opcions.Srv_ID_Lin1;
      _text.TXT_Lin2 = this.Opcions.Srv_ID_Lin2;
      _text.TXT_Lin3 = this.Opcions.Srv_ID_Lin3;
      _text.TXT_Lin4 = this.Opcions.Srv_ID_Lin4;
      _text.TXT_Lin5 = this.Opcions.Srv_ID_Lin5;
      _text.TXT_Location = this.Opcions.Localize.Text("Depositary");
      _text.TXT_BorneID = this.Opcions.Localize.Text("KIOSK");
      _text.TXT_Time = this.Opcions.Localize.Text("TIME");
      _text.TXT_Ticket = this.Opcions.Localize.Text("Fact");
      _text.TXT_Points = this.Opcions.Localize.Text("Euros");
      if (string.IsNullOrEmpty(_text.TXT_Lin1))
        _text.TXT_Lin1 = " ";
      if (string.IsNullOrEmpty(_text.TXT_Lin2))
        _text.TXT_Lin2 = " ";
      if (string.IsNullOrEmpty(_text.TXT_Lin3))
        _text.TXT_Lin3 = " ";
      if (string.IsNullOrEmpty(_text.TXT_Lin4))
        _text.TXT_Lin4 = " ";
      if (string.IsNullOrEmpty(_text.TXT_Lin5))
        _text.TXT_Lin5 = " ";
      tickets.Ticket_Out_ESCPOS_Check(_ptr_device, _valor, _id, _model, _cut, _skeep, _preskeep, _wide, 1, _temps, _cod, this.Opcions.Srv_User, _text, this.Opcions.TicketHidePay, _ntick, _join);
      this.Opcions.ticketCleanTemps = 1;
      return true;
    }

    public bool Ticket_Out_Conf(
      string _ptr_device,
      Decimal _valor,
      int _id,
      int _model,
      int _cut,
      int _skeep,
      int _preskeep,
      int _wide,
      DateTime _temps,
      string _cod)
    {
      if (this.Opcions.ModoTickets == 0 && _id != 0)
        return false;
      if (!this.Exist_Printer(_ptr_device))
      {
        this.PrinterErrorShow(1, "");
        return false;
      }
      if (!this.PaperOut_Printer(_ptr_device))
      {
        this.PrinterErrorShow(2, this.errtick);
        return false;
      }
      Tickets tickets = new Tickets();
      Tickets.Info_Ticket _text = new Tickets.Info_Ticket();
      _text.TXT_Null = this.Opcions.Localize.Text("Repeated tickets are null");
      _text.TXT_Valid = this.Opcions.Localize.Text("Valid only for 15 minutes");
      _text.TXT_Valid2 = this.Opcions.Localize.Text("Change only for gift check");
      _text.TXT_Bottom = this.Opcions.Srv_ID_LinBottom;
      _text.TXT_Thanks = this.Opcions.Localize.Text("THANKS");
      _text.TXT_Lin1 = this.Opcions.Srv_ID_Lin1;
      _text.TXT_Lin2 = this.Opcions.Srv_ID_Lin2;
      _text.TXT_Lin3 = this.Opcions.Srv_ID_Lin3;
      _text.TXT_Lin4 = this.Opcions.Srv_ID_Lin4;
      _text.TXT_Lin5 = this.Opcions.Srv_ID_Lin5;
      _text.TXT_Location = this.Opcions.Localize.Text("Depositary");
      _text.TXT_BorneID = this.Opcions.Localize.Text("KIOSK");
      _text.TXT_Time = this.Opcions.Localize.Text("TIME");
      _text.TXT_Ticket = this.Opcions.Localize.Text("Fact");
      _text.TXT_Points = this.Opcions.Localize.Text("Euros");
      _text.TXT_Cancel = this.Opcions.Localize.Text("Validated");
      if (string.IsNullOrEmpty(_text.TXT_Lin1))
        _text.TXT_Lin1 = " ";
      if (string.IsNullOrEmpty(_text.TXT_Lin2))
        _text.TXT_Lin2 = " ";
      if (string.IsNullOrEmpty(_text.TXT_Lin3))
        _text.TXT_Lin3 = " ";
      if (string.IsNullOrEmpty(_text.TXT_Lin4))
        _text.TXT_Lin4 = " ";
      if (string.IsNullOrEmpty(_text.TXT_Lin5))
        _text.TXT_Lin5 = " ";
      tickets.Ticket_Out_Conf_ESCPOS(_ptr_device, _valor, _id, _model, _cut, _skeep, _preskeep, _wide, 1, _temps, _cod, this.Opcions.Srv_User, _text, DateTime.Now);
      return true;
    }

    public bool _Old_Ticket_Out(
      string _ptr_device,
      Decimal _valor,
      int _id,
      int _model,
      int _cut,
      int _skeep,
      int _preskeep = 0)
    {
      if (this.Opcions.ModoTickets == 0 && _id != 0)
        return false;
      if (!this.Exist_Printer(_ptr_device))
      {
        this.PrinterErrorShow(1, "");
        return false;
      }
      if (!this.PaperOut_Printer(_ptr_device))
      {
        this.PrinterErrorShow(2, this.errtick);
        return false;
      }
      this.Ticket_Reset(_ptr_device);
      if (this._Last_Id == _id)
        return true;
      this._Last_Id = _id;
      if (_model == 1)
      {
        this._Old_Ticket_Out_ESCPOS(_ptr_device, _valor, _id, _model, _cut, _skeep, _preskeep);
        return true;
      }
      NIIClassLib niiClassLib = new NIIClassLib();
      Barcode barcode = new Barcode();
      string StringToEncode = Gestion.Build_Mod10(this.Opcions.Srv_User, (int) _valor, _id, 1);
      barcode.IncludeLabel = false;
      barcode.LabelFont = new Font("Arial", 20f);
      barcode.Alignment = AlignmentPositions.CENTER;
      barcode.RotateFlipType = RotateFlipType.RotateNoneFlipNone;
      barcode.LabelPosition = LabelPositions.TOPCENTER;
      Bitmap bitmap = new Bitmap(barcode.Encode(TYPE.CODE128, StringToEncode, System.Drawing.Color.Black, System.Drawing.Color.White, 580, 100));
      IntPtr compatibleDc = MainWindow.CreateCompatibleDC(IntPtr.Zero);
      IntPtr compatibleBitmap = MainWindow.CreateCompatibleBitmap(compatibleDc, 580, 100);
      MainWindow.SelectObject(compatibleDc, compatibleBitmap);
      ImageRasterHelper.ConvertHBitmap(compatibleDc, bitmap, true);
      long o_jobid;
      int num1;
      try
      {
        num1 = niiClassLib.NiiStartDoc(_ptr_device, out o_jobid);
      }
      catch (Exception ex)
      {
        MainWindow.DeleteDC(compatibleDc);
        return false;
      }
      num1 = niiClassLib.NiiPrint(_ptr_device, "1B2130", 6L, out o_jobid);
      num1 = niiClassLib.NiiPrint(_ptr_device, "1B6101", 6L, out o_jobid);
      string i_dat1 = "0a\"-- La Belle Net --\"0a";
      num1 = niiClassLib.NiiPrint(_ptr_device, i_dat1, (long) i_dat1.Length, out o_jobid);
      DateTime now = DateTime.Now;
      string i_dat2 = string.Format("\"{0}/{1:00}/{2:0000} {3:00}:{4:00}\"0a0a", (object) now.Day, (object) now.Month, (object) now.Year, (object) now.Hour, (object) now.Minute);
      num1 = niiClassLib.NiiPrint(_ptr_device, i_dat2, (long) i_dat2.Length, out o_jobid);
      num1 = niiClassLib.NiiPrint(_ptr_device, "1B2130", 6L, out o_jobid);
      string i_dat3 = string.Format("\"" + this.Opcions.Localize.Text("CONGRATULATIONS") + "\"0a");
      num1 = niiClassLib.NiiPrint(_ptr_device, i_dat3, (long) i_dat3.Length, out o_jobid);
      num1 = niiClassLib.NiiPrint(_ptr_device, "1B2110", 6L, out o_jobid);
      string i_dat4 = string.Format("\"" + this.Opcions.Localize.Text("Ticket value") + "\"0a");
      num1 = niiClassLib.NiiPrint(_ptr_device, i_dat4, (long) i_dat4.Length, out o_jobid);
      int num2 = (int) _valor;
      num1 = niiClassLib.NiiPrint(_ptr_device, "1B2130", 6L, out o_jobid);
      string i_dat5 = string.Format("\"{0}.{1:00} " + this.Opcions.Localize.Text("Euros") + "\"0a", (object) (num2 / 100), (object) (num2 % 100));
      num1 = niiClassLib.NiiPrint(_ptr_device, i_dat5, (long) i_dat5.Length, out o_jobid);
      string.Format("0a");
      num1 = niiClassLib.NiiPrint(_ptr_device, "1B2130", 6L, out o_jobid);
      string i_dat6 = string.Format("\"*{0}*\"0a", (object) StringToEncode);
      num1 = niiClassLib.NiiPrint(_ptr_device, i_dat6, (long) i_dat6.Length, out o_jobid);
      num1 = niiClassLib.NiiPrint(_ptr_device, "1B2130", 6L, out o_jobid);
      string i_dat7 = string.Format("\"ID: {0}\"0a", (object) _id);
      num1 = niiClassLib.NiiPrint(_ptr_device, i_dat7, (long) i_dat7.Length, out o_jobid);
      num1 = niiClassLib.NiiPrint(_ptr_device, "1B21a0", 6L, out o_jobid);
      num1 = niiClassLib.NiiPrint(_ptr_device, "1B6100", 6L, out o_jobid);
      string i_dat8 = string.Format("0a\"" + this.Opcions.Localize.Text("Location:") + "\"0a");
      num1 = niiClassLib.NiiPrint(_ptr_device, i_dat8, (long) i_dat8.Length, out o_jobid);
      num1 = niiClassLib.NiiPrint(_ptr_device, "1B2100", 6L, out o_jobid);
      string i_dat9 = string.Format("\"  {0}\"0a", (object) this.Opcions.Srv_ID_Lin1);
      num1 = niiClassLib.NiiPrint(_ptr_device, i_dat9, (long) i_dat9.Length, out o_jobid);
      string i_dat10 = string.Format("\"  {0}\"0a", (object) this.Opcions.Srv_ID_Lin2);
      num1 = niiClassLib.NiiPrint(_ptr_device, i_dat10, (long) i_dat10.Length, out o_jobid);
      string i_dat11 = string.Format("\"  {0}\"0a", (object) this.Opcions.Srv_ID_Lin3);
      num1 = niiClassLib.NiiPrint(_ptr_device, i_dat11, (long) i_dat11.Length, out o_jobid);
      string i_dat12 = string.Format("\"  {0}\"0a", (object) this.Opcions.Srv_ID_Lin4);
      num1 = niiClassLib.NiiPrint(_ptr_device, i_dat12, (long) i_dat12.Length, out o_jobid);
      string i_dat13 = string.Format("\"  RC: {0}\"0a", (object) this.Opcions.Srv_ID_Lin5);
      num1 = niiClassLib.NiiPrint(_ptr_device, i_dat13, (long) i_dat13.Length, out o_jobid);
      string i_dat14 = string.Format("\"  " + this.Opcions.Localize.Text("Kiosk ID:") + " {0}\"0a", (object) this.Opcions.Srv_User);
      num1 = niiClassLib.NiiPrint(_ptr_device, i_dat14, (long) i_dat14.Length, out o_jobid);
      num1 = niiClassLib.NiiPrint(_ptr_device, "1B2130", 6L, out o_jobid);
      num1 = niiClassLib.NiiPrint(_ptr_device, "1B6101", 6L, out o_jobid);
      string i_dat15 = string.Format("0a\"" + this.Opcions.Localize.Text("THANKS") + "\"0a", (object) "bar12x12");
      num1 = niiClassLib.NiiPrint(_ptr_device, i_dat15, (long) i_dat15.Length, out o_jobid);
      num1 = niiClassLib.NiiPrint(_ptr_device, "1B2100", 6L, out o_jobid);
      string i_dat16 = string.Format("\"" + this.Opcions.Srv_ID_LinBottom + "\"0a");
      num1 = niiClassLib.NiiPrint(_ptr_device, i_dat16, (long) i_dat16.Length, out o_jobid);
      string i_dat17 = string.Format("\"--------------------------------------------\"0a");
      num1 = niiClassLib.NiiPrint(_ptr_device, i_dat17, (long) i_dat17.Length, out o_jobid);
      num1 = niiClassLib.NiiPrint(_ptr_device, "1B2100", 6L, out o_jobid);
      num1 = niiClassLib.NiiPrint(_ptr_device, "1B4A781b69", 10L, out o_jobid);
      num1 = niiClassLib.NiiPrint(_ptr_device, "1B723160", 8L, out o_jobid);
      num1 = niiClassLib.NiiPrint(_ptr_device, "1D\"G\"10", 7L, out o_jobid);
      num1 = niiClassLib.NiiEndDoc(_ptr_device);
      MainWindow.DeleteDC(compatibleDc);
      return true;
    }

    private bool Ticket_Out_ESCPOS(
      string _ptr_device,
      Decimal _valor,
      int _id,
      int _model,
      int _cut,
      int _skeep,
      int _preskeep,
      int _60mm)
    {
      string str = Gestion.Build_Mod10(this.Opcions.Srv_User, (int) _valor, _id, 1);
      using (MemoryStream memoryStream = new MemoryStream())
      {
        using (BinaryWriter binaryWriter = new BinaryWriter((Stream) memoryStream))
        {
          binaryWriter.Write((byte) 27);
          binaryWriter.Write('@');
          binaryWriter.Write((byte) 27);
          binaryWriter.Write('T');
          binaryWriter.Write((byte) 0);
          binaryWriter.Write((byte) 27);
          binaryWriter.Write('a');
          binaryWriter.Write(this._ESCPOS_AC);
          binaryWriter.Write((byte) 27);
          binaryWriter.Write('!');
          binaryWriter.Write((byte) ((int) this._ESCPOS_FDW + (int) this._ESCPOS_FDH + _60mm));
          binaryWriter.Write((byte) 10);
          binaryWriter.Write((byte) 10);
          binaryWriter.Write((byte) 10);
          for (int index = 0; index < _preskeep; ++index)
            binaryWriter.Write(Convert.ToChar(10));
          binaryWriter.Write("-- La Belle Net --".ToCharArray());
          binaryWriter.Write((byte) 10);
          DateTime now = DateTime.Now;
          binaryWriter.Write(string.Format("{0}/{1:00}/{2:0000} {3:00}:{4:00}", (object) now.Day, (object) now.Month, (object) now.Year, (object) now.Hour, (object) now.Minute).ToCharArray());
          binaryWriter.Write((byte) 10);
          binaryWriter.Write((byte) 10);
          binaryWriter.Write(this.Opcions.Localize.Text("CONGRATULATIONS").ToCharArray());
          binaryWriter.Write((byte) 10);
          binaryWriter.Write((byte) 10);
          binaryWriter.Write(this.Opcions.Localize.Text("Ticket value").ToCharArray());
          binaryWriter.Write((byte) 10);
          int num = (int) _valor;
          binaryWriter.Write(string.Format("{0}.{1:00} " + this.Opcions.Localize.Text("Euros"), (object) (num / 100), (object) (num % 100)).ToCharArray());
          binaryWriter.Write('\n');
          binaryWriter.Write(string.Format("*{0}*", (object) str).ToCharArray());
          binaryWriter.Write((byte) 10);
          binaryWriter.Write(string.Format("ID: {0}", (object) _id));
          binaryWriter.Write((byte) 10);
          binaryWriter.Write((byte) 10);
          binaryWriter.Write(string.Format("  " + this.Opcions.Localize.Text("Kiosk ID:") + " {0}", (object) this.Opcions.Srv_User).ToCharArray());
          binaryWriter.Write((byte) 10);
          binaryWriter.Write((byte) 10);
          binaryWriter.Write((byte) 27);
          binaryWriter.Write('!');
          binaryWriter.Write((byte) _60mm);
          binaryWriter.Write((byte) 27);
          binaryWriter.Write('a');
          binaryWriter.Write(this._ESCPOS_AC);
          binaryWriter.Write("---------------------------------".ToCharArray());
          binaryWriter.Write((byte) 10);
          binaryWriter.Write((byte) 27);
          binaryWriter.Write('a');
          binaryWriter.Write(this._ESCPOS_AL);
          binaryWriter.Write((byte) 27);
          binaryWriter.Write('!');
          binaryWriter.Write((byte) ((int) this._ESCPOS_FE + (int) this._ESCPOS_FU + (int) this._ESCPOS_FDW + _60mm));
          binaryWriter.Write(this.Opcions.Localize.Text("Location:").ToCharArray());
          binaryWriter.Write((byte) 10);
          binaryWriter.Write((byte) 27);
          binaryWriter.Write('!');
          binaryWriter.Write((byte) 0);
          binaryWriter.Write(string.Format("  {0}", (object) this.Opcions.Srv_ID_Lin1).ToCharArray());
          binaryWriter.Write((byte) 10);
          binaryWriter.Write(string.Format("  {0}", (object) this.Opcions.Srv_ID_Lin2).ToCharArray());
          binaryWriter.Write(Convert.ToChar(10));
          binaryWriter.Write(string.Format("  {0}", (object) this.Opcions.Srv_ID_Lin3).ToCharArray());
          binaryWriter.Write(Convert.ToChar(10));
          binaryWriter.Write(string.Format("  {0}", (object) this.Opcions.Srv_ID_Lin4).ToCharArray());
          binaryWriter.Write(Convert.ToChar(10));
          binaryWriter.Write(string.Format("  RC: {0}", (object) this.Opcions.Srv_ID_Lin5).ToCharArray());
          binaryWriter.Write(Convert.ToChar(10));
          binaryWriter.Write(Convert.ToChar(27));
          binaryWriter.Write(Convert.ToChar('a'));
          binaryWriter.Write(Convert.ToChar(this._ESCPOS_AC));
          binaryWriter.Write(Convert.ToChar(27));
          binaryWriter.Write(Convert.ToChar('!'));
          binaryWriter.Write(Convert.ToChar((int) this._ESCPOS_FDW + (int) this._ESCPOS_FDH + _60mm));
          binaryWriter.Write((byte) 10);
          binaryWriter.Write(this.Opcions.Localize.Text("THANKS").ToCharArray());
          binaryWriter.Write((byte) 10);
          binaryWriter.Write(Convert.ToChar(27));
          binaryWriter.Write(Convert.ToChar('!'));
          binaryWriter.Write(Convert.ToChar(_60mm));
          binaryWriter.Write("---------------------------------".ToCharArray());
          binaryWriter.Write((byte) 10);
          binaryWriter.Write(this.Opcions.Srv_ID_LinBottom.ToCharArray());
          binaryWriter.Write((byte) 10);
          binaryWriter.Write("---------------------------------".ToCharArray());
          binaryWriter.Write((byte) 10);
          for (int index = 0; index < _skeep; ++index)
            binaryWriter.Write(Convert.ToChar(10));
          binaryWriter.Write(Convert.ToChar(27));
          binaryWriter.Write(Convert.ToChar(114));
          binaryWriter.Write(Convert.ToChar(49));
          binaryWriter.Write(Convert.ToChar(96));
          if (_cut == 1)
          {
            binaryWriter.Write(Convert.ToChar(27));
            binaryWriter.Write('m');
          }
          else
          {
            binaryWriter.Write(Convert.ToChar(27));
            binaryWriter.Write('J');
            binaryWriter.Write(Convert.ToChar(120));
            binaryWriter.Write(Convert.ToChar(29));
            binaryWriter.Write(Convert.ToChar('V'));
            binaryWriter.Write(Convert.ToChar(0));
          }
          binaryWriter.Flush();
          return this.Print_ESCPOS(_ptr_device, memoryStream.ToArray());
        }
      }
    }

    private string Convert_Text_POS(string _t)
    {
      if (_t.Length == 12)
        return _t + " ";
      return _t;
    }

    public bool Ticket_ESCPOS(
      string _ptr_device,
      Decimal _valor,
      int _tick,
      int _id,
      int _model,
      int _cut,
      int _skeep,
      int _preskeep,
      int _60mm)
    {
      Barcode barcode = new Barcode();
      string StringToEncode = Gestion.Build_Mod10(this.Opcions.Srv_User, _tick, _id, 0);
      barcode.IncludeLabel = false;
      barcode.LabelFont = new Font("Arial", 20f);
      barcode.Alignment = AlignmentPositions.CENTER;
      barcode.RotateFlipType = RotateFlipType.RotateNoneFlipNone;
      barcode.LabelPosition = LabelPositions.TOPCENTER;
      MainWindow.BitmapData bitmapData = MainWindow.GetBitmapData(new Bitmap(barcode.Encode(TYPE.CODE128, StringToEncode, System.Drawing.Color.Black, System.Drawing.Color.White, 350, 60)));
      BitArray dots = bitmapData.Dots;
      byte[] bytes = BitConverter.GetBytes(bitmapData.Width);
      using (MemoryStream memoryStream = new MemoryStream())
      {
        using (BinaryWriter binaryWriter = new BinaryWriter((Stream) memoryStream))
        {
          binaryWriter.Write((byte) 27);
          binaryWriter.Write('@');
          binaryWriter.Write((byte) 27);
          binaryWriter.Write('T');
          binaryWriter.Write((byte) 0);
          binaryWriter.Write((byte) 27);
          binaryWriter.Write('t');
          binaryWriter.Write((byte) 2);
          binaryWriter.Write((byte) 27);
          binaryWriter.Write('a');
          binaryWriter.Write(this._ESCPOS_AC);
          binaryWriter.Write((byte) 27);
          binaryWriter.Write('!');
          binaryWriter.Write((byte) ((int) this._ESCPOS_FDW + (int) this._ESCPOS_FDH + _60mm));
          binaryWriter.Write((byte) 10);
          binaryWriter.Write((byte) 10);
          binaryWriter.Write((byte) 10);
          for (int index = 0; index < _preskeep; ++index)
            binaryWriter.Write(Convert.ToChar(10));
          binaryWriter.Write("-- La Belle Net --".ToCharArray());
          binaryWriter.Write((byte) 10);
          DateTime now = DateTime.Now;
          binaryWriter.Write(string.Format("{0}/{1:00}/{2:0000} {3:00}:{4:00}", (object) now.Day, (object) now.Month, (object) now.Year, (object) now.Hour, (object) now.Minute).ToCharArray());
          binaryWriter.Write((byte) 10);
          binaryWriter.Write((byte) 10);
          binaryWriter.Write((byte) 27);
          binaryWriter.Write('a');
          binaryWriter.Write(this._ESCPOS_AL);
          binaryWriter.Write((byte) 27);
          binaryWriter.Write('!');
          binaryWriter.Write((byte) ((int) this._ESCPOS_FE + (int) this._ESCPOS_FU + (int) this._ESCPOS_FDW + _60mm));
          binaryWriter.Write(this.Opcions.Localize.Text("Location:").ToCharArray());
          binaryWriter.Write((byte) 10);
          binaryWriter.Write((byte) 27);
          binaryWriter.Write('!');
          binaryWriter.Write((byte) 0);
          binaryWriter.Write(string.Format("  {0}", (object) this.Opcions.Srv_ID_Lin1).ToCharArray());
          binaryWriter.Write((byte) 10);
          binaryWriter.Write(string.Format("  {0}", (object) this.Opcions.Srv_ID_Lin2).ToCharArray());
          binaryWriter.Write(Convert.ToChar(10));
          binaryWriter.Write(string.Format("  {0}", (object) this.Opcions.Srv_ID_Lin3).ToCharArray());
          binaryWriter.Write(Convert.ToChar(10));
          binaryWriter.Write(string.Format("  {0}", (object) this.Opcions.Srv_ID_Lin4).ToCharArray());
          binaryWriter.Write(Convert.ToChar(10));
          binaryWriter.Write(string.Format("  RC: {0}", (object) this.Opcions.Srv_ID_Lin5).ToCharArray());
          binaryWriter.Write(Convert.ToChar(10));
          binaryWriter.Write(Convert.ToChar(27));
          binaryWriter.Write(Convert.ToChar('!'));
          binaryWriter.Write(Convert.ToChar(_60mm));
          binaryWriter.Write(Convert.ToChar(27));
          binaryWriter.Write(Convert.ToChar('a'));
          binaryWriter.Write(Convert.ToChar(this._ESCPOS_AC));
          binaryWriter.Write("---------------------------------".ToCharArray());
          binaryWriter.Write(Convert.ToChar(10));
          binaryWriter.Write(Convert.ToChar(27));
          binaryWriter.Write(Convert.ToChar('!'));
          binaryWriter.Write(Convert.ToChar((int) this._ESCPOS_FDH + (int) this._ESCPOS_FDW + _60mm));
          binaryWriter.Write(string.Format("  " + this.Opcions.Localize.Text("Kiosk ID:") + " {0}", (object) this.Opcions.Srv_User).ToCharArray());
          binaryWriter.Write(Convert.ToChar(10));
          binaryWriter.Write(Convert.ToChar(10));
          binaryWriter.Write(string.Format(this.Opcions.Localize.Text("Ticket:") + " {0}", (object) StringToEncode).ToCharArray());
          binaryWriter.Write(Convert.ToChar(10));
          binaryWriter.Write(Convert.ToChar(10));
          TimeSpan timeSpan = new TimeSpan(0, 0, (int) (Decimal) _tick);
          string str = string.Format(this.Opcions.Localize.Text("Time: ") + " {0}:{1:00}:{2:00}", (object) timeSpan.Hours, (object) timeSpan.Minutes, (object) timeSpan.Seconds);
          if (_valor <= new Decimal(0))
            str = "TEST TICKET";
          binaryWriter.Write(str.ToCharArray());
          binaryWriter.Write(Convert.ToChar(10));
          binaryWriter.Write(Convert.ToChar(10));
          binaryWriter.Write(Convert.ToChar(27));
          binaryWriter.Write(Convert.ToChar('a'));
          binaryWriter.Write(Convert.ToChar(this._ESCPOS_AC));
          binaryWriter.Write(Convert.ToChar(27));
          binaryWriter.Write(Convert.ToChar('3'));
          binaryWriter.Write(Convert.ToChar(24));
          int num1 = 0;
          while (num1 < bitmapData.Height)
          {
            binaryWriter.Write(Convert.ToChar(27));
            binaryWriter.Write(Convert.ToChar('*'));
            binaryWriter.Write(Convert.ToChar(33));
            binaryWriter.Write(Convert.ToChar(bytes[0]));
            binaryWriter.Write(Convert.ToChar(bytes[1]));
            for (int index1 = 0; index1 < bitmapData.Width; ++index1)
            {
              for (int index2 = 0; index2 < 3; ++index2)
              {
                byte num2 = 0;
                for (int index3 = 0; index3 < 8; ++index3)
                {
                  int index4 = ((num1 / 8 + index2) * 8 + index3) * bitmapData.Width + index1;
                  bool flag = false;
                  if (index4 < dots.Length)
                    flag = dots[index4];
                  num2 |= (byte) ((flag ? 1 : 0) << 7 - index3);
                }
                binaryWriter.Write(num2);
              }
            }
            num1 += 24;
            binaryWriter.Write(Convert.ToChar(10));
          }
          binaryWriter.Write(Convert.ToChar(27));
          binaryWriter.Write(Convert.ToChar('3'));
          binaryWriter.Write(Convert.ToChar(30));
          binaryWriter.Write(Convert.ToChar(27));
          binaryWriter.Write(Convert.ToChar('!'));
          binaryWriter.Write(Convert.ToChar((int) this._ESCPOS_FDW + (int) this._ESCPOS_FDH + _60mm));
          binaryWriter.Write(Convert.ToChar(10));
          binaryWriter.Write(this.Opcions.Localize.Text("THANKS").ToCharArray());
          binaryWriter.Write(Convert.ToChar(10));
          binaryWriter.Write(Convert.ToChar(27));
          binaryWriter.Write(Convert.ToChar('!'));
          binaryWriter.Write(Convert.ToChar(_60mm));
          binaryWriter.Write("---------------------------------".ToCharArray());
          binaryWriter.Write(Convert.ToChar(10));
          binaryWriter.Write(this.Opcions.Srv_ID_LinBottom.ToCharArray());
          binaryWriter.Write(Convert.ToChar(10));
          binaryWriter.Write("---------------------------------".ToCharArray());
          binaryWriter.Write(Convert.ToChar(10));
          for (int index = 0; index < _skeep; ++index)
            binaryWriter.Write(Convert.ToChar(10));
          binaryWriter.Write(Convert.ToChar(27));
          binaryWriter.Write(Convert.ToChar(114));
          binaryWriter.Write(Convert.ToChar(49));
          binaryWriter.Write(Convert.ToChar(96));
          if (_cut == 1)
          {
            binaryWriter.Write(Convert.ToChar(27));
            binaryWriter.Write('m');
          }
          else
          {
            binaryWriter.Write(Convert.ToChar(27));
            binaryWriter.Write('J');
            binaryWriter.Write(Convert.ToChar(120));
            binaryWriter.Write(Convert.ToChar(29));
            binaryWriter.Write(Convert.ToChar('V'));
            binaryWriter.Write(Convert.ToChar(0));
          }
          binaryWriter.Flush();
          return this.Print_ESCPOS(_ptr_device, memoryStream.ToArray());
        }
      }
    }

    public bool Check_Printer_Ready(string _ptr_device)
    {
      if (string.IsNullOrEmpty(_ptr_device))
      {
        this.PrinterErrorShow(0, "");
        return false;
      }
      if (!this.Exist_Printer(_ptr_device))
      {
        this.PrinterErrorShow(1, "");
        return false;
      }
      if (this.PaperOut_Printer(_ptr_device))
        return true;
      this.PrinterErrorShow(2, this.errtick);
      return false;
    }

    private void bTicket_Click(object sender, EventArgs e)
    {
      if (this.Opcions == null || this.Opcions.Temps <= 0)
        return;
      DLG_Abonar_Ticket dlgAbonarTicket = new DLG_Abonar_Ticket(ref this.Opcions, this.Opcions.Localize.Text("ATTENTION!\r\n\r\nPour éviter de perdre vos jeux gratuits, jouez-les avant d'imprimer votre ticket\r\n\r\nVOUS ARRÊTEZ DE SURFER SUR INTERNET\r\nIMPRIMER TICKET?"));
      int num = (int) dlgAbonarTicket.ShowDialog();
      if (dlgAbonarTicket.OK && this.Check_Printer_Ready(this.Opcions.Impresora_Tck))
        this.Srv_Add_Ticket(this.Opcions.Temps, 0);
    }

    private void bLogin_Click(object sender, EventArgs e)
    {
      if (this.Opcions.Logged)
      {
        this.Opcions.Logged = false;
        this.bLogin.Image = (Image) Resources.ico_userd;
        this.Status = MainWindow.Fases.GoHome;
        this.Hide_Browser_Nav();
        this.Stop_Temps();
      }
      else
      {
        DLG_Registro dlgRegistro = new DLG_Registro(ref this.Opcions);
        int num = (int) dlgRegistro.ShowDialog();
        if (dlgRegistro.Login == 1)
        {
          this.Opcions.Logged = true;
          this.bLogin.Image = (Image) Resources.ico_1;
        }
        else
        {
          this.Opcions.Logged = false;
          this.bLogin.Image = (Image) Resources.ico_userd;
        }
      }
    }

    private void bBar_Click(object sender, EventArgs e)
    {
      int num = (int) new DLG_Barcode(ref this.Opcions)
      {
        MWin = this
      }.ShowDialog();
    }

    private void bTime_Click(object sender, EventArgs e)
    {
      if (this.Opcions.ComprarTemps != 0 || this.Opcions.InGame || this.Opcions.News == 2)
        return;
      this.Opcions.ComprarTemps = 2;
      this.Stop_Temps();
      new CreditManagerFull(ref this.Opcions).Show();
      this.Start_Temps();
    }

    private void MainWindow_MouseMove(object sender, MouseEventArgs e)
    {
      this.Opcions.LastMouseMove = DateTime.Now;
      this.TimeoutHome = 0;
    }

    private void Move_Screesaver(object sender, EventArgs e)
    {
      this.Opcions.LastMouseMove = DateTime.Now;
      this.TimeoutHome = 0;
    }

    [DllImport("winmm.dll")]
    public static extern int waveOutGetVolume(IntPtr hwo, out uint dwVolume);

    [DllImport("winmm.dll")]
    public static extern int waveOutSetVolume(IntPtr hwo, uint dwVolume);

    private void bMute_Click(object sender, EventArgs e)
    {
      if (this.Opcions.modo_XP == 1)
      {
        MainWindow.SendMessageW(this.Handle, 793, this.Handle, (IntPtr) 524288);
      }
      else
      {
        if (this.device == null)
          return;
        int num1 = (int) ((double) this.device.AudioEndpointVolume.MasterVolumeLevelScalar * 100.0);
        int num2;
        if (num1 == 0)
        {
          num2 = this._MMVol;
        }
        else
        {
          this._MMVol = num1;
          num2 = 0;
        }
        this.device.AudioEndpointVolume.MasterVolumeLevelScalar = (float) num2 / 100f;
        this.SoundTest();
      }
    }

    private void SoundMax()
    {
      if (this.device == null)
        return;
      this.alamaramute = this.device.AudioEndpointVolume.Mute;
      this.device.AudioEndpointVolume.Mute = false;
      this._MMVol = (int) ((double) this.device.AudioEndpointVolume.MasterVolumeLevelScalar * 100.0);
      this.device.AudioEndpointVolume.MasterVolumeLevelScalar = 1f;
    }

    private void SoundRestore()
    {
      if (this.device == null)
        return;
      this.device.AudioEndpointVolume.Mute = this.alamaramute;
      this.device.AudioEndpointVolume.MasterVolumeLevelScalar = (float) this._MMVol / 100f;
    }

    private void bVDown_Click(object sender, EventArgs e)
    {
      if (this.Opcions.modo_XP == 1)
      {
        MainWindow.SendMessageW(this.Handle, 793, this.Handle, (IntPtr) 589824);
      }
      else
      {
        if (this.device == null)
          return;
        int num1 = (int) ((double) this.device.AudioEndpointVolume.MasterVolumeLevelScalar * 100.0);
        int num2 = num1 > 5 ? num1 - 5 : 0;
        this.device.AudioEndpointVolume.MasterVolumeLevelScalar = (float) num2 / 100f;
        this._MMVol = num2;
        this.SoundTest();
      }
    }

    private void bVUp_Click(object sender, EventArgs e)
    {
      if (this.Opcions.modo_XP == 1)
      {
        MainWindow.SendMessageW(this.Handle, 793, this.Handle, (IntPtr) 655360);
      }
      else
      {
        if (this.device == null)
          return;
        int num1 = (int) ((double) this.device.AudioEndpointVolume.MasterVolumeLevelScalar * 100.0);
        int num2 = num1 < 95 ? num1 + 5 : 100;
        this.device.AudioEndpointVolume.MasterVolumeLevelScalar = (float) num2 / 100f;
        this._MMVol = num2;
        this.SoundTest();
      }
    }

    private void timerPrinter_Tick(object sender, EventArgs e)
    {
      this.Ticket_Poll();
    }

    private void PrintProps(ManagementObject o, string prop)
    {
      try
      {
        MainWindow mainWindow = this;
        mainWindow.dbglog = mainWindow.dbglog + "(" + prop + "|" + o[prop] + ")\r\n";
      }
      catch (Exception ex)
      {
        Console.Write(ex.ToString());
      }
    }

    private void Find_Validator(bool _force = false)
    {
      if (this.Last_DRV != "-")
      {
        this.Opcions.Dev_BNV_P = this.Last_COM;
        this.Opcions.Dev_BNV = this.Last_DRV;
      }
      switch (this.Opcions.Dev_BNV.ToLower())
      {
        case "ssp":
          if (this.Find_Validator_SSP(true) != 2 || this.Find_Validator_SSP6(true) != 2)
            break;
          this.Find_Validator_CCT(true);
          break;
        case "ssp3":
          if (this.Find_Validator_SSP6(true) != 2 || this.Find_Validator_SSP(true) != 2)
            break;
          this.Find_Validator_CCT(true);
          break;
        case "f40":
          if (this.Find_Validator_CCT(true) != 2 || this.Find_Validator_SSP(true) != 2)
            break;
          this.Find_Validator_SSP6(true);
          break;
      }
    }

    private int Find_Validator_SSP(bool _force = false)
    {
      if (this.Opcions.RunConfig)
        return 0;
      if (this.Opcions.Dev_BNV.ToLower() == "ssp".ToLower() && this.Opcions.Dev_BNV_P.ToLower().Contains("com"))
      {
        Control_NV_SSP controlNvSsp = new Control_NV_SSP();
        controlNvSsp.port = this.Opcions.Dev_BNV_P;
        controlNvSsp.Open();
        Thread.Sleep(100);
        Application.DoEvents();
        controlNvSsp.Poll();
        controlNvSsp.Close();
        if (controlNvSsp.respuesta)
        {
          this.Last_COM = this.Opcions.Dev_BNV_P;
          this.Last_DRV = this.Opcions.Dev_BNV;
          return 1;
        }
      }
      if (this.Opcions.Dev_BNV.ToLower() == "ssp".ToLower() || string.IsNullOrEmpty(this.Opcions.Dev_BNV) || this.Opcions.Dev_BNV == "?" || _force)
      {
        Control_NV_SSP controlNvSsp = new Control_NV_SSP();
        controlNvSsp.Start_Find_Device();
        while (!controlNvSsp.Poll_Find_Device())
        {
          Thread.Sleep(100);
          Application.DoEvents();
          if (this.Opcions.RunConfig)
          {
            controlNvSsp.Close();
            return 0;
          }
        }
        controlNvSsp.Stop_Find_Device();
        controlNvSsp.Close();
        if (controlNvSsp._f_resp_scom != "-")
        {
          this.Opcions.Dev_BNV = "ssp";
          this.Opcions.Dev_BNV_P = controlNvSsp._f_resp_scom;
          this.Last_COM = this.Opcions.Dev_BNV_P;
          this.Last_DRV = this.Opcions.Dev_BNV;
          return 1;
        }
      }
      return 2;
    }

    private int Find_Validator_SSP6(bool _force = false)
    {
      if (this.Opcions.RunConfig)
        return 0;
      if (this.Opcions.Dev_BNV.ToLower() == "ssp3".ToLower() && this.Opcions.Dev_BNV_P.ToLower().Contains("com"))
      {
        Control_NV_SSP_P6 controlNvSspP6 = new Control_NV_SSP_P6();
        controlNvSspP6.port = this.Opcions.Dev_BNV_P;
        controlNvSspP6.Open();
        Thread.Sleep(100);
        Application.DoEvents();
        controlNvSspP6.Poll();
        controlNvSspP6.Close();
        if (controlNvSspP6.m_Respuesta)
        {
          this.Last_COM = this.Opcions.Dev_BNV_P;
          this.Last_DRV = this.Opcions.Dev_BNV;
          return 1;
        }
      }
      if (this.Opcions.Dev_BNV.ToLower() == "ssp3".ToLower() || string.IsNullOrEmpty(this.Opcions.Dev_BNV) || this.Opcions.Dev_BNV == "?" || _force)
      {
        Control_NV_SSP_P6 controlNvSspP6 = new Control_NV_SSP_P6();
        controlNvSspP6.Start_Find_Device();
        do
        {
          Thread.Sleep(100);
          Application.DoEvents();
          if (this.Opcions.RunConfig)
          {
            controlNvSspP6.Close();
            return 0;
          }
        }
        while (!controlNvSspP6.Poll_Find_Device());
        controlNvSspP6.Stop_Find_Device();
        controlNvSspP6.Close();
        if (controlNvSspP6._f_resp_scom != "-")
        {
          this.Opcions.Dev_BNV = "ssp3";
          this.Opcions.Dev_BNV_P = controlNvSspP6._f_resp_scom;
          this.Last_COM = this.Opcions.Dev_BNV_P;
          this.Last_DRV = this.Opcions.Dev_BNV;
          return 1;
        }
      }
      return 2;
    }

    private int Find_Validator_CCT(bool _force = false)
    {
      if (this.Opcions.RunConfig)
        return 0;
      if (this.Opcions.Dev_BNV.ToLower() == "f40".ToLower() && this.Opcions.Dev_BNV_P.ToLower().Contains("com"))
      {
        Control_F40_CCTalk controlF40CcTalk = new Control_F40_CCTalk();
        controlF40CcTalk.port = this.Opcions.Dev_BNV_P;
        controlF40CcTalk.Open();
        Thread.Sleep(100);
        controlF40CcTalk.Poll();
        controlF40CcTalk.Close();
        if (controlF40CcTalk.respuesta)
        {
          this.Last_COM = this.Opcions.Dev_BNV_P;
          this.Last_DRV = this.Opcions.Dev_BNV;
          return 1;
        }
      }
      if (this.Opcions.Dev_BNV.ToLower() == "f40".ToLower() || string.IsNullOrEmpty(this.Opcions.Dev_BNV) || this.Opcions.Dev_BNV == "?" || _force)
      {
        Control_F40_CCTalk controlF40CcTalk = new Control_F40_CCTalk();
        controlF40CcTalk.Start_Find_Device();
        do
        {
          controlF40CcTalk.Find_Device();
          if (controlF40CcTalk.respuesta)
          {
            this.Opcions.Dev_BNV = "f40";
            this.Opcions.Dev_BNV_P = controlF40CcTalk._f_resp_scom;
            controlF40CcTalk.Stop_Find_Device();
            controlF40CcTalk.Close();
            this.Last_COM = this.Opcions.Dev_BNV_P;
            this.Last_DRV = this.Opcions.Dev_BNV;
            return 1;
          }
          controlF40CcTalk.Poll_Find_Device();
          Application.DoEvents();
          if (this.Opcions.RunConfig)
          {
            controlF40CcTalk.Close();
            return 0;
          }
        }
        while (controlF40CcTalk._f_com != "-");
        controlF40CcTalk.Stop_Find_Device();
        controlF40CcTalk.Close();
      }
      return 2;
    }

    private void Find_Selector()
    {
      if (this.Opcions.Dev_Coin.ToLower() == "rm5".ToLower() || string.IsNullOrEmpty(this.Opcions.Dev_Coin) || this.Opcions.Dev_Coin == "?")
      {
        Control_Comestero controlComestero = new Control_Comestero();
        if (this.Opcions.Dev_Coin_P.ToLower().Contains("com".ToLower()))
        {
          controlComestero.port = this.Opcions.Dev_Coin_P;
          controlComestero._f_com = this.Opcions.Dev_Coin_P;
          if (controlComestero.Open())
          {
            controlComestero.Close();
            return;
          }
          controlComestero.Close();
        }
        controlComestero.Start_Find_Device();
        while (!controlComestero.Poll_Find_Device())
          Thread.Sleep(50);
        controlComestero.Stop_Find_Device();
        if (controlComestero._f_resp_scom != "-")
        {
          this.Opcions.Dev_Coin = "rm5";
          this.Opcions.Dev_Coin_P = controlComestero._f_resp_scom;
        }
      }
      if (!(this.Opcions.Dev_Coin.ToLower() == "cct2".ToLower()) && !string.IsNullOrEmpty(this.Opcions.Dev_Coin) && !(this.Opcions.Dev_Coin == "?"))
        return;
      Control_CCTALK_COIN controlCctalkCoin = new Control_CCTALK_COIN();
      if (this.Opcions.Dev_Coin_P.ToLower().Contains("com".ToLower()))
      {
        controlCctalkCoin.port = this.Opcions.Dev_Coin_P;
        controlCctalkCoin._f_com = this.Opcions.Dev_Coin_P;
        controlCctalkCoin.GetInfo(2, "POLL");
        controlCctalkCoin.Start_Find_Device();
        Thread.Sleep(500);
        if (controlCctalkCoin.Poll_Find_Device())
        {
          controlCctalkCoin.Close();
          return;
        }
        controlCctalkCoin.Close();
      }
      controlCctalkCoin.Start_Find_Device();
      while (!controlCctalkCoin.Poll_Find_Device())
        Thread.Sleep(50);
      controlCctalkCoin.Stop_Find_Device();
      if (controlCctalkCoin._f_resp_scom != "-")
      {
        this.Opcions.Dev_Coin = "cct2";
        this.Opcions.Dev_Coin_P = controlCctalkCoin._f_resp_scom;
      }
    }

    private string SpotTroubleUsingQueueAttributes(PrintQueue pq)
    {
      int queueStatus = (int) pq.QueueStatus;
      if (true)
        return "OK";
      if ((pq.QueueStatus & PrintQueueStatus.PaperProblem) == PrintQueueStatus.PaperProblem && pq.IsOutOfPaper)
        return "Has a paper problem";
      if ((pq.QueueStatus & PrintQueueStatus.NoToner) == PrintQueueStatus.NoToner)
        return "Is out of toner";
      if ((pq.QueueStatus & PrintQueueStatus.DoorOpen) == PrintQueueStatus.DoorOpen)
        return "Has an open door";
      if ((pq.QueueStatus & PrintQueueStatus.Error) == PrintQueueStatus.Error)
        return "Is in an error state";
      if ((pq.QueueStatus & PrintQueueStatus.NotAvailable) == PrintQueueStatus.NotAvailable)
        return "Is not available";
      if ((pq.QueueStatus & PrintQueueStatus.Offline) == PrintQueueStatus.Offline)
        return "Is off line";
      if ((pq.QueueStatus & PrintQueueStatus.OutOfMemory) == PrintQueueStatus.OutOfMemory)
        return "Is out of memory";
      if ((pq.QueueStatus & PrintQueueStatus.PaperOut) == PrintQueueStatus.PaperOut)
        return "Is out of paper";
      if ((pq.QueueStatus & PrintQueueStatus.OutputBinFull) == PrintQueueStatus.OutputBinFull)
        return "Has a full output bin";
      if ((pq.QueueStatus & PrintQueueStatus.PaperJam) == PrintQueueStatus.PaperJam)
        return "Has a paper jam";
      if ((pq.QueueStatus & PrintQueueStatus.Paused) == PrintQueueStatus.Paused)
        return "Is paused";
      if ((pq.QueueStatus & PrintQueueStatus.TonerLow) == PrintQueueStatus.TonerLow)
        return "Is low on toner";
      return (pq.QueueStatus & PrintQueueStatus.UserIntervention) == PrintQueueStatus.UserIntervention ? "Needs user intervention" : "OK";
    }

    private string SpotTroubleUsingProperties(PrintQueue pq)
    {
      if (pq.IsWaiting)
        return "OK";
      if (pq.HasPaperProblem && pq.IsOutOfPaper)
        return "Has a paper problem";
      if (!pq.HasToner)
        return "Is out of toner";
      if (pq.IsDoorOpened)
        return "Has an open door";
      if (pq.IsInError)
        return "Is in an error state";
      if (pq.IsNotAvailable)
        return "Is not available";
      if (pq.IsOffline)
        return "Is off line";
      if (pq.IsOutOfMemory)
        return "Is out of memory";
      if (pq.IsOutOfPaper)
        return "Is out of paper";
      if (pq.IsOutputBinFull)
        return "Has a full output bin";
      if (pq.IsPaperJammed)
        return "Has a paper jam";
      if (pq.IsPaused)
        return "Is paused";
      if (pq.IsTonerLow)
        return "Is low on toner";
      return pq.NeedUserIntervention ? "Needs user intervention" : "OK";
    }

    private bool PaperOut_Printer(string _ptr_device)
    {
      if (this.Opcions.ModoTickets == 1 && this.Opcions.modo_XP == 0 && !string.IsNullOrEmpty(_ptr_device))
      {
        this.errtick = "";
        foreach (PrintQueue printQueue in new LocalPrintServer(PrintSystemDesiredAccess.AdministrateServer).GetPrintQueues())
        {
          printQueue.Refresh();
          if (printQueue.Name.ToLower() == _ptr_device.ToLower())
          {
            this.errtick = this.SpotTroubleUsingQueueAttributes(printQueue);
            if (this.errtick != "OK")
              return false;
            this.errtick = this.SpotTroubleUsingProperties(printQueue);
            return !(this.errtick != "OK");
          }
        }
      }
      return false;
    }

    [DllImport("printui.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    private static extern void PrintUIEntryW(
      IntPtr hwnd,
      IntPtr hinst,
      string lpszCmdLine,
      int nCmdShow);

    public static void Clean_Printer()
    {
      string newName = (string) null;
      string sPrinterName = (string) null;
      foreach (ManagementObject managementObject in new ManagementObjectSearcher("SELECT * FROM Win32_Printer").Get())
      {
        string str1 = managementObject.Properties["Caption"].Value.ToString();
        string str2 = managementObject.Properties["Name"].Value.ToString();
        bool flag = false;
        try
        {
          flag = bool.Parse(managementObject.Properties["WorkOffline"].Value.ToString());
        }
        catch
        {
        }
        if (flag && str1.ToLower().Contains("NII".ToLower()))
        {
          if (!str1.ToLower().Contains("(".ToLower()))
            newName = str1;
          string lpszCmdLine = "/dl /n " + (object) '"' + str2 + (object) '"';
          MainWindow.PrintUIEntryW(IntPtr.Zero, IntPtr.Zero, lpszCmdLine, 0);
        }
        if (!flag && str1.ToLower().Contains("NII".ToLower()))
          sPrinterName = str1;
      }
      if (newName == null || sPrinterName == null)
        return;
      MainWindow.RenamePrinter(sPrinterName, newName);
    }

    public static void RenamePrinter(string sPrinterName, string newName)
    {
      MainWindow.oManagementScope = new ManagementScope(ManagementPath.DefaultPath);
      MainWindow.oManagementScope.Connect();
      SelectQuery selectQuery = new SelectQuery();
      selectQuery.QueryString = "SELECT * FROM Win32_Printer WHERE Name = '" + sPrinterName.Replace("\\", "\\\\") + "'";
      ManagementObjectCollection objectCollection = new ManagementObjectSearcher(MainWindow.oManagementScope, (ObjectQuery) selectQuery).Get();
      if (objectCollection.Count == 0)
        return;
      using (ManagementObjectCollection.ManagementObjectEnumerator enumerator = objectCollection.GetEnumerator())
      {
        if (enumerator.MoveNext())
          ((ManagementObject) enumerator.Current).InvokeMethod(nameof (RenamePrinter), new object[1]
          {
            (object) newName
          });
      }
    }

    private bool Exist_Printer(string _ptr_device)
    {
      if (this.Opcions.ModoTickets == 1 && this.Opcions.modo_XP == 0 && !string.IsNullOrEmpty(_ptr_device))
      {
        foreach (ManagementObject managementObject in new ManagementObjectSearcher("SELECT * FROM Win32_Printer").Get())
        {
          if (managementObject.Properties["Caption"].Value.ToString().ToLower() == _ptr_device.ToLower())
          {
            bool flag = false;
            try
            {
              flag = bool.Parse(managementObject.Properties["WorkOffline"].Value.ToString());
            }
            catch
            {
            }
            return !flag;
          }
        }
      }
      return false;
    }

    private void Find_Printer()
    {
      if (this.Opcions.ModoTickets != 1 || this.Opcions.modo_XP != 0)
        return;
      MainWindow.Clean_Printer();
      ManagementObjectSearcher managementObjectSearcher = new ManagementObjectSearcher("SELECT * FROM Win32_Printer");
      string str1 = "";
      string str2 = "";
      string str3 = "";
      foreach (ManagementObject managementObject in managementObjectSearcher.Get())
      {
        string str4 = managementObject.Properties["Caption"].Value.ToString();
        bool flag = false;
        try
        {
          flag = bool.Parse(managementObject.Properties["WorkOffline"].Value.ToString());
        }
        catch
        {
        }
        if (!flag && str4.ToLower().Contains("NII".ToLower()))
          str1 = str4;
        if (!flag && str4.ToLower().Contains("STAR".ToLower()))
          str2 = str4;
        if (!flag && str4.ToLower().Contains("CUSTOM".ToLower()))
          str3 = str4;
        if (!flag && str4.ToLower() == this.Opcions.Impresora_Tck.ToLower())
        {
          if (this.Opcions.Impresora_Tck.ToLower().Contains("nii".ToLower()))
            this.Opcions.Ticket_Cut = 1;
          if (this.Opcions.Impresora_Tck.ToLower().Contains("star".ToLower()))
            this.Opcions.Ticket_Cut = 0;
          if (!this.Opcions.Impresora_Tck.ToLower().Contains("custom".ToLower()))
            return;
          this.Opcions.Ticket_Cut = 1;
          if (this.Opcions.Impresora_Tck.ToLower().Contains("TG2460".ToLower()))
          {
            this.Opcions.Ticket_60mm = 1;
            this.Opcions.Ticket_N_FEED = 12;
            this.Opcions.Ticket_N_HEAD = 3;
          }
          return;
        }
        if (!flag && this.Opcions.Impresora_Tck.ToLower().Contains("NII".ToLower()) && str4.ToLower().Contains("NII".ToLower()))
        {
          this.Opcions.Impresora_Tck = str4;
          this.Opcions.Ticket_Cut = 1;
          return;
        }
        if (!flag && this.Opcions.Impresora_Tck.ToLower().Contains("star".ToLower()) && str4.ToLower().Contains("Star".ToLower()))
        {
          this.Opcions.Impresora_Tck = str4;
          this.Opcions.Ticket_Cut = 0;
          return;
        }
        if (!flag && this.Opcions.Impresora_Tck.ToLower().Contains("custom".ToLower()) && str4.ToLower().Contains("custom".ToLower()))
        {
          this.Opcions.Impresora_Tck = str4;
          this.Opcions.Ticket_Cut = 1;
          if (!this.Opcions.Impresora_Tck.ToLower().Contains("TG2460".ToLower()))
            return;
          this.Opcions.Ticket_60mm = 1;
          this.Opcions.Ticket_N_FEED = 12;
          this.Opcions.Ticket_N_HEAD = 3;
          return;
        }
      }
      if (!string.IsNullOrEmpty(str1))
      {
        this.Opcions.Impresora_Tck = str1;
        this.Opcions.Ticket_N_FEED = 3;
        this.Opcions.Ticket_N_HEAD = 0;
        this.Opcions.Ticket_Cut = 1;
        this.Opcions.Ticket_Model = 0;
      }
      else if (!string.IsNullOrEmpty(str2))
      {
        this.Opcions.Impresora_Tck = str2;
        this.Opcions.Ticket_N_FEED = 3;
        this.Opcions.Ticket_N_HEAD = 0;
        this.Opcions.Ticket_Cut = 0;
        this.Opcions.Ticket_Model = 1;
      }
      else
      {
        if (string.IsNullOrEmpty(str3))
          return;
        this.Opcions.Impresora_Tck = str3;
        this.Opcions.Ticket_N_FEED = 3;
        this.Opcions.Ticket_N_HEAD = 3;
        this.Opcions.Ticket_Cut = 1;
        this.Opcions.Ticket_Model = 1;
        if (!this.Opcions.Impresora_Tck.ToLower().Contains("TG2460".ToLower()))
          return;
        this.Opcions.Ticket_60mm = 1;
        this.Opcions.Ticket_N_FEED = 12;
        this.Opcions.Ticket_N_HEAD = 3;
      }
    }

    private int set_ban_users(string _l)
    {
      string[] strArray = _l.Split('#');
      int length = strArray.Length;
      if (length > 0 && string.IsNullOrEmpty(strArray[length - 1]))
        --length;
      this.ban_users = strArray;
      return length;
    }

    private int set_ban_macs(string _l)
    {
      string[] strArray = _l.Split('#');
      int length = strArray.Length;
      if (length > 0 && string.IsNullOrEmpty(strArray[length - 1]))
        --length;
      this.ban_mac = strArray;
      return length;
    }

    private int set_ban_ip(string _l)
    {
      string[] strArray = _l.Split('#');
      int length = strArray.Length;
      if (length > 0 && string.IsNullOrEmpty(strArray[length - 1]))
        --length;
      this.ban_ip = strArray;
      return length;
    }

    private int set_ban_country(string _l)
    {
      string[] strArray = _l.Split('#');
      int length = strArray.Length;
      if (length > 0 && string.IsNullOrEmpty(strArray[length - 1]))
        --length;
      this.ban_country = strArray;
      return length;
    }

    private void set_reset(string _l, int _modo)
    {
      if (_modo == 0)
        this.Opcions.ForceResetMask = _l;
      if (string.IsNullOrEmpty(_l))
        this.Opcions.ForceReset = false;
      if (!(_l != this.Opcions.ForceResetMask))
        return;
      this.Opcions.ForceResetMask = _l;
      long num = 0;
      try
      {
        num = long.Parse(_l);
      }
      catch
      {
      }
      if (num > 0L)
        this.Opcions.ForceReset = true;
    }

    private void set_reset_user(string _l, int _modo)
    {
      string[] strArray = _l.Split(',');
      if (strArray == null || strArray.Length != 2 || strArray[1] != this.Opcions.Srv_User)
        return;
      _l = strArray[0];
      if (_modo == 0)
        this.Opcions.ForceResetMask = _l;
      if (string.IsNullOrEmpty(_l))
        this.Opcions.ForceReset = false;
      if (!(_l != this.Opcions.ForceResetMask))
        return;
      this.Opcions.ForceResetMask = _l;
      long num = 0;
      try
      {
        num = long.Parse(_l);
      }
      catch
      {
      }
      if (num > 0L)
        this.Opcions.ForceReset = true;
    }

    private void set_spy(string _l, int _modo)
    {
      this.Opcions.Spy = 0;
      this.Opcions.UserSpy = "?";
      if (string.IsNullOrEmpty(_l))
        return;
      this.Opcions.Spy = 1;
      this.Opcions.UserSpy = _l;
    }

    private void set_remote(string _l, int _modo)
    {
      this.Opcions.Spy = 0;
      this.Opcions.UserSpy = "?";
      if (string.IsNullOrEmpty(_l))
        return;
      this.Opcions.Spy = 2;
      this.Opcions.UserSpy = _l;
    }

    private void set_mantenimiento(string _l, int _modo)
    {
      if (_modo != 0)
        ;
      if (string.IsNullOrEmpty(_l))
        this.Opcions.ForceManteniment = false;
      if (!(_l != this.Opcions.ForceMantenimentMask))
        return;
      this.Opcions.ForceMantenimentMask = _l;
      int num = 0;
      try
      {
        num = int.Parse(_l);
      }
      catch
      {
      }
      this.Opcions.ForceManteniment = num > 0;
    }

    private void set_command(string _l, int _modo)
    {
      if (_modo != 0)
        ;
      if (!string.IsNullOrEmpty(_l))
        ;
    }

    public bool load_web(string _web, int _modo)
    {
      string str = Path.GetTempPath() + "__check.html";
      string empty1 = string.Empty;
      string empty2 = string.Empty;
      Exception exception;
      try
      {
        new WebClient().DownloadFile(_web, str);
      }
      catch (Exception ex)
      {
        exception = ex;
        return false;
      }
      XmlTextReader xmlTextReader1 = (XmlTextReader) null;
      XmlTextReader xmlTextReader2;
      try
      {
        xmlTextReader2 = new XmlTextReader(str);
      }
      catch (Exception ex)
      {
        exception = ex;
        xmlTextReader1?.Close();
        return false;
      }
      try
      {
        while (xmlTextReader2.Read())
        {
          if (xmlTextReader2.NodeType == XmlNodeType.Element)
          {
            if (xmlTextReader2.Name.ToLower() == "rst".ToLower())
              this.set_reset(XmlConfig.Decrypt(xmlTextReader2.ReadString()), _modo);
            if (xmlTextReader2.Name.ToLower() == "urst".ToLower())
              this.set_reset_user(XmlConfig.Decrypt(xmlTextReader2.ReadString()), _modo);
            if (xmlTextReader2.Name.ToLower() == "cmd".ToLower())
              this.set_command(XmlConfig.Decrypt(xmlTextReader2.ReadString()), _modo);
            if (xmlTextReader2.Name.ToLower() == "man".ToLower())
              this.set_mantenimiento(XmlConfig.Decrypt(xmlTextReader2.ReadString()), _modo);
            if (xmlTextReader2.Name.ToLower() == "spy".ToLower())
              this.set_spy(XmlConfig.Decrypt(xmlTextReader2.ReadString()), _modo);
            if (xmlTextReader2.Name.ToLower() == "rem".ToLower())
              this.set_remote(XmlConfig.Decrypt(xmlTextReader2.ReadString()), _modo);
            if (xmlTextReader2.Name.ToLower() == "bu".ToLower())
              this.set_ban_users(XmlConfig.Decrypt(xmlTextReader2.ReadString()));
            if (xmlTextReader2.Name.ToLower() == "bm".ToLower())
              this.set_ban_macs(XmlConfig.Decrypt(xmlTextReader2.ReadString()));
            if (xmlTextReader2.Name.ToLower() == "bi".ToLower())
              this.set_ban_ip(XmlConfig.Decrypt(xmlTextReader2.ReadString()));
            if (xmlTextReader2.Name.ToLower() == "bc".ToLower())
              this.set_ban_country(XmlConfig.Decrypt(xmlTextReader2.ReadString()));
            if (xmlTextReader2.Name.ToLower() == "info".ToLower() && xmlTextReader2.HasAttributes)
            {
              for (int i = 0; i < xmlTextReader2.AttributeCount; ++i)
              {
                xmlTextReader2.MoveToAttribute(i);
                if (xmlTextReader2.Name.ToLower() == "a".ToLower())
                {
                  try
                  {
                    this.web_versio = xmlTextReader2.Value.ToString();
                  }
                  catch
                  {
                  }
                }
                if (xmlTextReader2.Name.ToLower() == "b".ToLower())
                {
                  try
                  {
                    this.web_vnc = XmlConfig.Decrypt(xmlTextReader2.Value.ToString());
                    if (!string.IsNullOrEmpty(this.web_vnc))
                    {
                      if (this.web_vnc != "-")
                        this.Opcions.Server_VNC = this.web_vnc;
                    }
                  }
                  catch
                  {
                  }
                }
                if (xmlTextReader2.Name.ToLower() == "c".ToLower())
                {
                  try
                  {
                    this.web_date = xmlTextReader2.Value.ToString();
                  }
                  catch
                  {
                  }
                }
              }
            }
          }
        }
        xmlTextReader2.Close();
      }
      catch (Exception ex)
      {
        exception = ex;
        xmlTextReader2?.Close();
        return false;
      }
      return true;
    }

    private void EntraJocs_Click(object sender, EventArgs e)
    {
      if (this.Banner_On != 2)
        return;
      TimeSpan timeSpan;
      //ref TimeSpan local = ref timeSpan;
      DateTime now = DateTime.Now;
      int hour = now.Hour;
      now = DateTime.Now;
      int minute = now.Minute;
      int second = DateTime.Now.Second;
      int millisecond = DateTime.Now.Millisecond;
      timeSpan = new TimeSpan(hour, minute, second, millisecond);
      bool flag = true;
      this.WebLink = "http://".ToLower() + this.Opcions.Srv_Web_Ip + "/MenuTel.aspx?t=" + this.Opcions.Srv_User + "," + this.Opcions.Srv_User_P + ",TickQuioscV2.aspx," + (object) timeSpan.TotalMilliseconds;
      this.EntraJocs.Visible = false;
      this.Banner_On = 0;
      if (this.Opcions.CancelTempsOn == 1)
      {
        flag = false;
        if (this.Opcions.CancelTemps > 20)
        {
          int num = this.Opcions.CancelTemps / 12 * 5;
          if (this.Opcions.Credits > (Decimal) num)
            this.Srv_Sub_Credits((Decimal) num, 0);
          else
            this.Srv_Sub_Credits(this.Opcions.Credits, 0);
        }
      }
      this.Opcions.CancelTemps = 0;
      this.MenuGames = 1;
      this.EntraJocs.BackgroundImage = (Image) this.Img_Banner1;
      this.EntraJocs.Invalidate();
      try
      {
        this.bTicket.Enabled = false;
      }
      catch
      {
      }
    }

    private void timerMessages_Tick(object sender, EventArgs e)
    {
      if (this.Status != MainWindow.Fases.Home || (this.Opcions.ticketCleanTemps != 1 || this.Opcions.InGame))
        return;
      this.Opcions.ticketCleanTemps = 2;
      if (this.Opcions.Temps > 0 && this.Opcions.AutoTicketTime >= 1 && this.Opcions.AutoTicketTime != 2)
      {
        DLG_Abonar_Ticket dlgAbonarTicket = new DLG_Abonar_Ticket(ref this.Opcions, this.Opcions.Localize.Text("ATTENTION!\r\n\r\nVous n'avez plus de jeux gratuits\r\n\r\nDésirez vous récupérer votre temp restant?\r\nIMPRIMER TICKET?"));
        int num = (int) dlgAbonarTicket.ShowDialog();
        if (dlgAbonarTicket.OK && this.Check_Printer_Ready(this.Opcions.Impresora_Tck))
          this.Srv_Add_Ticket(this.Opcions.Temps, 0);
      }
      this.Opcions.ticketCleanTemps = 0;
    }

    private void Control_Getton_Time()
    {
    }

    private void Control_Getton_Reset()
    {
    }

    private void Control_Getton()
    {
    }

    private void bGETTON_Click(object sender, EventArgs e)
    {
      this.Opcions.Add_Getton = 0;
      this.Internal_Add_Credits(25);
      this.Opcions.GettonOff = true;
      this.pGETTON.Visible = false;
      this.Control_Getton_Time();
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
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (MainWindow));
      this.pMenu = new Panel();
      this.pGETTON = new Panel();
      this.lGETTON = new Label();
      this.pTime = new Panel();
      this.eCredits = new Label();
      this.pKeyboard = new Panel();
      this.pTemps = new Panel();
      this.rtest = new Panel();
      this.lcdClock = new Label();
      this.pLogin = new Panel();
      this.pTicket = new Panel();
      this.lCGRAT = new Label();
      this.pNavegation = new Panel();
      this.tURL = new TextBox();
      this.iSponsor = new Label();
      this.timerCredits = new System.Windows.Forms.Timer(this.components);
      this.timerPoll = new System.Windows.Forms.Timer(this.components);
      this.timerStartup = new System.Windows.Forms.Timer(this.components);
      this.imgLed = new ImageList(this.components);
      this.pScreenSaver = new Label();
      this.timerPrinter = new System.Windows.Forms.Timer(this.components);
      this.EntraJocs = new Panel();
      this.timerMessages = new System.Windows.Forms.Timer(this.components);
      this.lCALL = new Label();
      this.bGETTON = new Button();
      this.bTime = new Button();
      this.pInsertCoin = new PictureBox();
      this.bVUp = new Button();
      this.bVDown = new Button();
      this.bMute = new Button();
      this.bKeyboard = new Button();
      this.bLogin = new Button();
      this.bBar = new Button();
      this.bTicket = new Button();
      this.bHome = new Button();
      this.bBack = new Button();
      this.bForward = new Button();
      this.bGo = new Button();
      this.pMenu.SuspendLayout();
      this.pGETTON.SuspendLayout();
      this.pTime.SuspendLayout();
      this.pKeyboard.SuspendLayout();
      this.pTemps.SuspendLayout();
      this.pLogin.SuspendLayout();
      this.pTicket.SuspendLayout();
      this.pNavegation.SuspendLayout();
      ((ISupportInitialize) this.pInsertCoin).BeginInit();
      this.SuspendLayout();
      this.pMenu.BackColor = System.Drawing.Color.Green;
      this.pMenu.Controls.Add((Control) this.pGETTON);
      this.pMenu.Controls.Add((Control) this.pTime);
      this.pMenu.Controls.Add((Control) this.pKeyboard);
      this.pMenu.Controls.Add((Control) this.pTemps);
      this.pMenu.Controls.Add((Control) this.pLogin);
      this.pMenu.Controls.Add((Control) this.pTicket);
      this.pMenu.Controls.Add((Control) this.pNavegation);
      this.pMenu.Controls.Add((Control) this.iSponsor);
      this.pMenu.Dock = DockStyle.Top;
      this.pMenu.ForeColor = System.Drawing.Color.Silver;
      this.pMenu.Location = new Point(0, 0);
      this.pMenu.Name = "pMenu";
      this.pMenu.Size = new Size(1024, 50);
      this.pMenu.TabIndex = 15;
      this.pGETTON.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.pGETTON.Controls.Add((Control) this.lGETTON);
      this.pGETTON.Controls.Add((Control) this.bGETTON);
      this.pGETTON.Location = new Point(263, 0);
      this.pGETTON.Name = "pGETTON";
      this.pGETTON.Size = new Size(595, 50);
      this.pGETTON.TabIndex = 17;
      this.lGETTON.Dock = DockStyle.Fill;
      this.lGETTON.FlatStyle = FlatStyle.Flat;
      this.lGETTON.Font = new Font("Microsoft Sans Serif", 10f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lGETTON.ForeColor = System.Drawing.Color.Yellow;
      this.lGETTON.Location = new Point(0, 0);
      this.lGETTON.Name = "lGETTON";
      this.lGETTON.Size = new Size(518, 50);
      this.lGETTON.TabIndex = 8;
      this.lGETTON.Text = "25 points gratuits chaque jour, apuyyez sur jetton >>>";
      this.lGETTON.TextAlign = ContentAlignment.MiddleCenter;
      this.pTime.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.pTime.Controls.Add((Control) this.bTime);
      this.pTime.Controls.Add((Control) this.eCredits);
      this.pTime.Location = new Point(10, 0);
      this.pTime.Name = "pTime";
      this.pTime.Size = new Size(80, 50);
      this.pTime.TabIndex = 20;
      this.eCredits.BackColor = System.Drawing.Color.Transparent;
      this.eCredits.Dock = DockStyle.Bottom;
      this.eCredits.ForeColor = System.Drawing.Color.Yellow;
      this.eCredits.Location = new Point(0, 30);
      this.eCredits.Name = "eCredits";
      this.eCredits.Size = new Size(80, 20);
      this.eCredits.TabIndex = 8;
      this.eCredits.Text = "-";
      this.eCredits.TextAlign = ContentAlignment.MiddleCenter;
      this.pKeyboard.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.pKeyboard.Controls.Add((Control) this.pInsertCoin);
      this.pKeyboard.Controls.Add((Control) this.bVUp);
      this.pKeyboard.Controls.Add((Control) this.bVDown);
      this.pKeyboard.Controls.Add((Control) this.bMute);
      this.pKeyboard.Controls.Add((Control) this.bKeyboard);
      this.pKeyboard.Location = new Point(857, 0);
      this.pKeyboard.Name = "pKeyboard";
      this.pKeyboard.Size = new Size(166, 50);
      this.pKeyboard.TabIndex = 19;
      this.pTemps.Controls.Add((Control) this.rtest);
      this.pTemps.Controls.Add((Control) this.lcdClock);
      this.pTemps.Location = new Point(0, 0);
      this.pTemps.Name = "pTemps";
      this.pTemps.Size = new Size(81, 50);
      this.pTemps.TabIndex = 18;
      this.rtest.Location = new Point(54, 20);
      this.rtest.Name = "rtest";
      this.rtest.Size = new Size(24, 21);
      this.rtest.TabIndex = 9;
      this.rtest.Visible = false;
      this.lcdClock.BackColor = System.Drawing.Color.Transparent;
      this.lcdClock.Dock = DockStyle.Fill;
      this.lcdClock.Font = new Font("Microsoft Sans Serif", 8f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lcdClock.ForeColor = System.Drawing.Color.Yellow;
      this.lcdClock.Location = new Point(0, 0);
      this.lcdClock.Name = "lcdClock";
      this.lcdClock.Size = new Size(81, 50);
      this.lcdClock.TabIndex = 8;
      this.lcdClock.Text = "-";
      this.lcdClock.TextAlign = ContentAlignment.MiddleCenter;
      this.pLogin.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.pLogin.Controls.Add((Control) this.bLogin);
      this.pLogin.Location = new Point(836, 0);
      this.pLogin.Name = "pLogin";
      this.pLogin.Size = new Size(50, 50);
      this.pLogin.TabIndex = 18;
      this.pTicket.Controls.Add((Control) this.lCGRAT);
      this.pTicket.Controls.Add((Control) this.bBar);
      this.pTicket.Controls.Add((Control) this.bTicket);
      this.pTicket.Location = new Point(81, 0);
      this.pTicket.Name = "pTicket";
      this.pTicket.Size = new Size(164, 50);
      this.pTicket.TabIndex = 18;
      this.lCGRAT.FlatStyle = FlatStyle.Flat;
      this.lCGRAT.ForeColor = System.Drawing.Color.Yellow;
      this.lCGRAT.Location = new Point(0, 0);
      this.lCGRAT.Name = "lCGRAT";
      this.lCGRAT.Size = new Size(60, 50);
      this.lCGRAT.TabIndex = 8;
      this.lCGRAT.Text = "-";
      this.lCGRAT.TextAlign = ContentAlignment.MiddleCenter;
      this.lCGRAT.Visible = false;
      this.pNavegation.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.pNavegation.Controls.Add((Control) this.bHome);
      this.pNavegation.Controls.Add((Control) this.tURL);
      this.pNavegation.Controls.Add((Control) this.bBack);
      this.pNavegation.Controls.Add((Control) this.bForward);
      this.pNavegation.Controls.Add((Control) this.bGo);
      this.pNavegation.Location = new Point(263, 0);
      this.pNavegation.Name = "pNavegation";
      this.pNavegation.Size = new Size(598, 50);
      this.pNavegation.TabIndex = 18;
      this.tURL.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.tURL.Font = new Font("Microsoft Sans Serif", 16f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.tURL.Location = new Point(169, 9);
      this.tURL.Name = "tURL";
      this.tURL.Size = new Size(343, 32);
      this.tURL.TabIndex = 4;
      this.tURL.Visible = false;
      this.tURL.KeyPress += new KeyPressEventHandler(this.tURL_KeyPress);
      this.iSponsor.AutoSize = true;
      this.iSponsor.ForeColor = System.Drawing.Color.Green;
      this.iSponsor.Location = new Point(267, 17);
      this.iSponsor.Name = "iSponsor";
      this.iSponsor.Size = new Size(82, 13);
      this.iSponsor.TabIndex = 9;
      this.iSponsor.Text = "Sponsor area";
      this.timerCredits.Tick += new EventHandler(this.timerCredits_Tick);
      this.timerPoll.Tick += new EventHandler(this.timerPoll_Tick);
      this.timerStartup.Interval = 3000;
      this.timerStartup.Tick += new EventHandler(this.timerStartup_Tick);
      this.imgLed.ImageStream = (ImageListStreamer) componentResourceManager.GetObject("imgLed.ImageStream");
      this.imgLed.TransparentColor = System.Drawing.Color.Transparent;
      this.imgLed.Images.SetKeyName(0, "bullet_ball_red.png");
      this.imgLed.Images.SetKeyName(1, "bullet_ball_green.png");
      this.imgLed.Images.SetKeyName(2, "bullet_ball_yellow.png");
      this.imgLed.Images.SetKeyName(3, "bullet_ball_blue.png");
      this.imgLed.Images.SetKeyName(4, "bullet_ball_blue.png");
      this.pScreenSaver.BackColor = System.Drawing.Color.Yellow;
      this.pScreenSaver.Font = new Font("Microsoft Sans Serif", 24f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.pScreenSaver.ForeColor = System.Drawing.Color.Black;
      this.pScreenSaver.Location = new Point(74, 142);
      this.pScreenSaver.Name = "pScreenSaver";
      this.pScreenSaver.Padding = new Padding(0, 0, 12, 40);
      this.pScreenSaver.Size = new Size(568, 332);
      this.pScreenSaver.TabIndex = 24;
      this.pScreenSaver.Text = "Version 1.0.0 (Status: 100)";
      this.timerPrinter.Interval = 2000;
      this.timerPrinter.Tick += new EventHandler(this.timerPrinter_Tick);
      this.EntraJocs.BackColor = System.Drawing.Color.Transparent;
      this.EntraJocs.BackgroundImageLayout = ImageLayout.Stretch;
      this.EntraJocs.Dock = DockStyle.Top;
      this.EntraJocs.Location = new Point(0, 50);
      this.EntraJocs.Name = "EntraJocs";
      this.EntraJocs.Size = new Size(1024, 60);
      this.EntraJocs.TabIndex = 25;
      this.EntraJocs.Visible = false;
      this.EntraJocs.Click += new EventHandler(this.EntraJocs_Click);
      this.timerMessages.Interval = 1000;
      this.timerMessages.Tick += new EventHandler(this.timerMessages_Tick);
      this.lCALL.BackColor = System.Drawing.Color.Yellow;
      this.lCALL.Font = new Font("Microsoft Sans Serif", 16f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lCALL.ForeColor = System.Drawing.Color.Black;
      this.lCALL.Location = new Point(0, 304);
      this.lCALL.Name = "lCALL";
      this.lCALL.Size = new Size(1024, 150);
      this.lCALL.TabIndex = 26;
      this.lCALL.Text = "-";
      this.lCALL.TextAlign = ContentAlignment.MiddleCenter;
      this.bGETTON.Dock = DockStyle.Right;
      this.bGETTON.FlatAppearance.BorderSize = 0;
      this.bGETTON.FlatStyle = FlatStyle.Flat;
      this.bGETTON.Image = (Image) Resources.getton;
      this.bGETTON.Location = new Point(518, 0);
      this.bGETTON.Name = "bGETTON";
      this.bGETTON.Size = new Size(77, 50);
      this.bGETTON.TabIndex = 5;
      this.bGETTON.UseVisualStyleBackColor = true;
      this.bGETTON.Click += new EventHandler(this.bGETTON_Click);
      this.bTime.Dock = DockStyle.Fill;
      this.bTime.FlatAppearance.BorderSize = 0;
      this.bTime.FlatStyle = FlatStyle.Flat;
      this.bTime.Image = (Image) Resources.ico_clock;
      this.bTime.Location = new Point(0, 0);
      this.bTime.Name = "bTime";
      this.bTime.Size = new Size(80, 30);
      this.bTime.TabIndex = 7;
      this.bTime.UseVisualStyleBackColor = true;
      this.bTime.Click += new EventHandler(this.bTime_Click);
      this.pInsertCoin.Location = new Point(143, 13);
      this.pInsertCoin.Name = "pInsertCoin";
      this.pInsertCoin.Size = new Size(22, 24);
      this.pInsertCoin.TabIndex = 10;
      this.pInsertCoin.TabStop = false;
      this.bVUp.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.bVUp.FlatAppearance.BorderSize = 0;
      this.bVUp.FlatStyle = FlatStyle.Flat;
      this.bVUp.Image = (Image) Resources.ico_volmas;
      this.bVUp.Location = new Point(74, 4);
      this.bVUp.Name = "bVUp";
      this.bVUp.Size = new Size(32, 43);
      this.bVUp.TabIndex = 9;
      this.bVUp.UseVisualStyleBackColor = true;
      this.bVUp.Click += new EventHandler(this.bVUp_Click);
      this.bVDown.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.bVDown.FlatAppearance.BorderSize = 0;
      this.bVDown.FlatStyle = FlatStyle.Flat;
      this.bVDown.Image = (Image) Resources.ico_volmen;
      this.bVDown.Location = new Point(42, 4);
      this.bVDown.Name = "bVDown";
      this.bVDown.Size = new Size(32, 43);
      this.bVDown.TabIndex = 8;
      this.bVDown.UseVisualStyleBackColor = true;
      this.bVDown.Click += new EventHandler(this.bVDown_Click);
      this.bMute.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.bMute.FlatAppearance.BorderSize = 0;
      this.bMute.FlatStyle = FlatStyle.Flat;
      this.bMute.Image = (Image) Resources.ico_volmute;
      this.bMute.Location = new Point(10, 4);
      this.bMute.Name = "bMute";
      this.bMute.Size = new Size(32, 43);
      this.bMute.TabIndex = 7;
      this.bMute.UseVisualStyleBackColor = true;
      this.bMute.Click += new EventHandler(this.bMute_Click);
      this.bKeyboard.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.bKeyboard.FlatAppearance.BorderSize = 0;
      this.bKeyboard.FlatStyle = FlatStyle.Flat;
      this.bKeyboard.Image = (Image) Resources.ico_osk1;
      this.bKeyboard.Location = new Point(106, 4);
      this.bKeyboard.Name = "bKeyboard";
      this.bKeyboard.Size = new Size(36, 43);
      this.bKeyboard.TabIndex = 6;
      this.bKeyboard.UseVisualStyleBackColor = true;
      this.bKeyboard.Click += new EventHandler(this.bKeyboard_Click);
      this.bLogin.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.bLogin.FlatAppearance.BorderSize = 0;
      this.bLogin.FlatStyle = FlatStyle.Flat;
      this.bLogin.Image = (Image) Resources.ico_userd;
      this.bLogin.Location = new Point(1, 4);
      this.bLogin.Name = "bLogin";
      this.bLogin.Size = new Size(48, 43);
      this.bLogin.TabIndex = 7;
      this.bLogin.UseVisualStyleBackColor = true;
      this.bLogin.Click += new EventHandler(this.bLogin_Click);
      this.bBar.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.bBar.FlatAppearance.BorderSize = 0;
      this.bBar.FlatStyle = FlatStyle.Flat;
      this.bBar.Image = (Image) Resources.ico_barcode;
      this.bBar.Location = new Point(62, 4);
      this.bBar.Name = "bBar";
      this.bBar.Size = new Size(48, 43);
      this.bBar.TabIndex = 7;
      this.bBar.UseVisualStyleBackColor = true;
      this.bBar.Click += new EventHandler(this.bBar_Click);
      this.bTicket.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.bTicket.FlatAppearance.BorderSize = 0;
      this.bTicket.FlatStyle = FlatStyle.Flat;
      this.bTicket.Image = (Image) Resources.ico_ticket3;
      this.bTicket.Location = new Point(112, 4);
      this.bTicket.Name = "bTicket";
      this.bTicket.Size = new Size(48, 43);
      this.bTicket.TabIndex = 6;
      this.bTicket.UseVisualStyleBackColor = true;
      this.bTicket.Click += new EventHandler(this.bTicket_Click);
      this.bHome.FlatAppearance.BorderSize = 0;
      this.bHome.FlatStyle = FlatStyle.Flat;
      this.bHome.Image = (Image) componentResourceManager.GetObject("bHome.Image");
      this.bHome.Location = new Point(7, 4);
      this.bHome.Name = "bHome";
      this.bHome.Size = new Size(48, 43);
      this.bHome.TabIndex = 0;
      this.bHome.UseVisualStyleBackColor = true;
      this.bHome.Click += new EventHandler(this.bHome_Click);
      this.bBack.FlatAppearance.BorderSize = 0;
      this.bBack.FlatStyle = FlatStyle.Flat;
      this.bBack.Image = (Image) Resources.ico_left_green;
      this.bBack.Location = new Point(67, 4);
      this.bBack.Name = "bBack";
      this.bBack.Size = new Size(48, 43);
      this.bBack.TabIndex = 2;
      this.bBack.UseVisualStyleBackColor = true;
      this.bForward.FlatAppearance.BorderSize = 0;
      this.bForward.FlatStyle = FlatStyle.Flat;
      this.bForward.Image = (Image) Resources.ico_right_green;
      this.bForward.Location = new Point(115, 4);
      this.bForward.Name = "bForward";
      this.bForward.Size = new Size(48, 43);
      this.bForward.TabIndex = 3;
      this.bForward.UseVisualStyleBackColor = true;
      this.bGo.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.bGo.FlatAppearance.BorderSize = 0;
      this.bGo.FlatStyle = FlatStyle.Flat;
      this.bGo.Image = (Image) Resources.ico_ok;
      this.bGo.Location = new Point(518, 4);
      this.bGo.Name = "bGo";
      this.bGo.Size = new Size(48, 43);
      this.bGo.TabIndex = 5;
      this.bGo.UseVisualStyleBackColor = true;
      this.bGo.Click += new EventHandler(this.bGo_Click);
      this.AutoScaleMode = AutoScaleMode.None;
      this.BackColor = System.Drawing.Color.Black;
      this.ClientSize = new Size(1024, 600);
      this.Controls.Add((Control) this.EntraJocs);
      this.Controls.Add((Control) this.lCALL);
      this.Controls.Add((Control) this.pScreenSaver);
      this.Controls.Add((Control) this.pMenu);
      this.DoubleBuffered = true;
      this.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.FormBorderStyle = FormBorderStyle.None;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (MainWindow);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.Manual;
      this.Text = "Kiosk";
      this.FormClosing += new FormClosingEventHandler(this.MainWindow_FormClosing);
      this.FormClosed += new FormClosedEventHandler(this.MainWindow_FormClosed);
      this.Load += new EventHandler(this.MainWindow_Load);
      this.SizeChanged += new EventHandler(this.MainWindow_SizeChanged);
      this.Paint += new PaintEventHandler(this.MainWindow_Paint);
      this.pMenu.ResumeLayout(false);
      this.pMenu.PerformLayout();
      this.pGETTON.ResumeLayout(false);
      this.pTime.ResumeLayout(false);
      this.pKeyboard.ResumeLayout(false);
      this.pTemps.ResumeLayout(false);
      this.pLogin.ResumeLayout(false);
      this.pTicket.ResumeLayout(false);
      this.pNavegation.ResumeLayout(false);
      this.pNavegation.PerformLayout();
      ((ISupportInitialize) this.pInsertCoin).EndInit();
      this.ResumeLayout(false);
    }

    public class MenuHandler : IContextMenuHandler
    {
      void IContextMenuHandler.OnBeforeContextMenu(
        IWebBrowser browserControl,
        IBrowser browser,
        IFrame frame,
        IContextMenuParams parameters,
        IMenuModel model)
      {
      }

      bool IContextMenuHandler.OnContextMenuCommand(
        IWebBrowser browserControl,
        IBrowser browser,
        IFrame frame,
        IContextMenuParams parameters,
        CefMenuCommand commandId,
        CefEventFlags eventFlags)
      {
        return false;
      }

      void IContextMenuHandler.OnContextMenuDismissed(
        IWebBrowser browserControl,
        IBrowser browser,
        IFrame frame)
      {
      }

      bool IContextMenuHandler.RunContextMenu(
        IWebBrowser browserControl,
        IBrowser browser,
        IFrame frame,
        IContextMenuParams parameters,
        IMenuModel model,
        IRunContextMenuCallback callback)
      {
        return true;
      }
    }

    public class JsDialogHandler : IJsDialogHandler
    {
      public bool OnJSDialog(
        IWebBrowser browserControl,
        IBrowser browser,
        string originUrl,
        string acceptLang,
        CefJsDialogType dialogType,
        string messageText,
        string defaultPromptText,
        IJsDialogCallback callback,
        ref bool suppressMessage)
      {
        return false;
      }

      public bool OnJSBeforeUnload(
        IWebBrowser browserControl,
        IBrowser browser,
        string message,
        bool isReload,
        IJsDialogCallback callback)
      {
        return false;
      }

      public void OnResetDialogState(IWebBrowser browserControl, IBrowser browser)
      {
      }

      public void OnDialogClosed(IWebBrowser browserControl, IBrowser browser)
      {
      }
    }

    public class LifeSpanHandler : ILifeSpanHandler
    {
      public event Action<string> PopupRequest;

      public bool DoClose(IWebBrowser browserControl, IBrowser browser)
      {
        return true;
      }

      public void OnAfterCreated(IWebBrowser browserControl, IBrowser browser)
      {
      }

      public void OnBeforeClose(IWebBrowser browserControl, IBrowser browser)
      {
      }

      public bool OnBeforePopup(
        IWebBrowser browserControl,
        IBrowser browser,
        IFrame frame,
        string targetUrl,
        string targetFrameName,
        WindowOpenDisposition targetDisposition,
        bool userGesture,
        IWindowInfo windowInfo,
        ref bool noJavascriptAccess,
        out IWebBrowser newBrowser)
      {
        newBrowser = browserControl;
        return true;
      }
    }

    public class NavIRequestHandler : IRequestHandler, ILoadHandler
    {
      public readonly IWebBrowser model;
      public readonly MainWindow MainW;

      public void OnFrameLoadEnd(IWebBrowser browserControl, FrameLoadEndEventArgs frameLoadEndArgs)
      {
      }

      public void OnFrameLoadStart(
        IWebBrowser browserControl,
        FrameLoadStartEventArgs frameLoadStartArgs)
      {
      }

      public void OnLoadingStateChange(
        IWebBrowser browserControl,
        LoadingStateChangedEventArgs loadingStateChangedArgs)
      {
      }

      public NavIRequestHandler(IWebBrowser _model, MainWindow _main)
      {
        this.model = _model;
        this.model.RequestHandler = (IRequestHandler) this;
        this.model.LoadHandler = (ILoadHandler) this;
        this.MainW = _main;
      }

      public void OnResourceLoadComplete(
        IWebBrowser browserControl,
        IBrowser browser,
        IFrame frame,
        IRequest request,
        IResponse response,
        UrlRequestStatus status,
        long receivedContentLength)
      {
      }

      public void OnResourceRedirect(
        IWebBrowser browserControl,
        IBrowser browser,
        IFrame frame,
        IRequest request,
        ref string newUrl)
      {
      }

      public void OnRenderProcessTerminated(
        IWebBrowser browserControl,
        IBrowser browser,
        CefTerminationStatus status)
      {
      }

      public bool OnQuotaRequest(
        IWebBrowser browserControl,
        IBrowser browser,
        string originUrl,
        long newSize,
        IRequestCallback callback)
      {
        return false;
      }

      public bool OnProtocolExecution(IWebBrowser browserControl, IBrowser browser, string url)
      {
        return false;
      }

      public void OnRenderViewReady(IWebBrowser browserControl, IBrowser browser)
      {
      }

      public bool OnCertificateError(
        IWebBrowser browserControl,
        IBrowser browser,
        CefErrorCode errorCode,
        string requestUrl,
        ISslInfo sslInfo,
        IRequestCallback callback)
      {
        return false;
      }

      public void OnPluginCrashed(IWebBrowser browserControl, IBrowser browser, string pluginPath)
      {
      }

      public bool OnBeforeMenu(IWebBrowser browser)
      {
        return true;
      }

      public CefReturnValue OnBeforeResourceLoad(
        IWebBrowser browserControl,
        IBrowser browser,
        IFrame frame,
        IRequest request,
        IRequestCallback callback)
      {
        if (request.ReferrerUrl.ToLower().Contains("://menu.") && !request.Url.ToLower().Contains("://menu.") && request.TransitionType == TransitionType.CliendRedirect && request.Url.ToLower().Contains("tickquioscv2.aspx".ToLower()))
          this.MainW.Status = MainWindow.Fases.GoHome;
        Console.WriteLine(request.ReferrerUrl + " <> " + request.Url + " <> " + request.Method + " <> " + (object) request.TransitionType);
        return CefReturnValue.Continue;
      }

      public void OnLoadError(IWebBrowser browserControl, LoadErrorEventArgs loadErrorArgs)
      {
        if (loadErrorArgs.ErrorCode != CefErrorCode.NameNotResolved && loadErrorArgs.ErrorCode != CefErrorCode.InternetDisconnected)
          ;
      }

      public bool OnBeforeBrowse(
        IWebBrowser browserControl,
        IBrowser browser,
        IFrame frame,
        IRequest request,
        bool isRedirect)
      {
        Keys modifierKeys = Control.ModifierKeys;
        if ((modifierKeys & Keys.Shift) == Keys.Shift || (modifierKeys & Keys.Control) == Keys.Control || this.MainW == null || this.MainW.navegador == null || this.MainW.Opcions.ComprarTemps > 0 && this.MainW.Opcions.News != 1)
          return true;
        this.MainW.Show_Navegador();
        string[] strArray1 = request.Url.Split(':');
        string[] strArray2 = request.Url.Split('/');
        if (this.MainW.Opcions.News == 1)
        {
          int num = 0;
          for (int index = 0; index < this.MainW.Opcions.Web_Zone.Length; ++index)
          {
            if (!string.IsNullOrEmpty(this.MainW.Opcions.Web_Zone[index]) && request.Url.ToLower().Contains(this.MainW.Opcions.Web_Zone[index].ToLower()))
              num = 1;
          }
          if (num == 0)
          {
            if (!request.Url.ToLower().Contains("#lp-main"))
              request.Url += "#lp-main";
            if (request.Url.ToLower().Contains("&"))
              return true;
          }
        }
        if (this.MainW.Opcions.FullScreen == 1)
        {
          string lower = request.Url.ToLower();
          if (this.MainW.Opcions.ModoKiosk != 0 && !lower.Contains("StartGameQuiosk.aspx".ToLower()))
          {
            if (this.MainW.Opcions.ModoKiosk == 1)
              this.MainW.Opcions.FullScreen = 2;
            else
              this.MainW.GoGames();
          }
        }
        if (!this.MainW.Opcions.TimeNavigate)
          ;
        if ((strArray1[0].ToLower() == "http".ToLower() || strArray1[0].ToLower() == "https".ToLower()) && strArray2[2].ToLower() == this.MainW.Opcions.Srv_Ip)
        {
          if (request.Url.ToLower().Contains("StartGameQuiosk.aspx".ToLower()))
            this.MainW.CloseOSK();
          if (this.MainW.Opcions.ModoTickets == 1 && request.Url.ToLower().Contains("tickquioscv2.aspx".ToLower()))
            this.MainW.MenuGames = 0;
          return false;
        }
        if (!(strArray1[0].ToLower() == "http".ToLower()) && !(strArray1[0].ToLower() == "https".ToLower()))
          return true;
        if (this.MainW.Opcions.Credits <= new Decimal(0) && this.MainW.Opcions.Temps <= 0 && this.MainW.Opcions.ModoKiosk == 1 && this.MainW.Opcions.News != 1)
        {
          if (this.MainW.Opcions.News == 0)
          {
            this.MainW.Stop_Temps();
            if (this.MainW.Opcions.ModoTickets == 0)
            {
              try
              {
                int num = (int) new DLG_Message(this.MainW.Opcions.Localize.Text("Insert credits"), ref this.MainW.Opcions, false).ShowDialog();
              }
              catch
              {
              }
            }
            else
            {
              try
              {
                int num = (int) new DLG_Message(this.MainW.Opcions.Localize.Text("Insert credits or ticket"), ref this.MainW.Opcions, false).ShowDialog();
              }
              catch
              {
              }
            }
            this.MainW.Status = MainWindow.Fases.GoHome;
            return true;
          }
          if (this.MainW.Opcions.BrowserBarOn == 0)
            return true;
        }
        if (this.MainW.Opcions.Temps <= 0 && this.MainW.Opcions.ModoTickets == 0 && this.MainW.Opcions.ModoKiosk == 1 && this.MainW.Opcions.News != 1)
        {
          if (this.MainW.Opcions.News == 1)
          {
            this.MainW.Opcions.ComprarTemps = 1;
            return true;
          }
          if (this.MainW.Opcions.BrowserBarOn == 0)
            return true;
        }
        if (this.MainW.Opcions.News != 2)
        {
          this.MainW._WebLink = request.Url;
          this.MainW.Start_Temps();
          if (this.MainW.Opcions.News != 1)
            this.MainW.Status = MainWindow.Fases.GoNavigate;
        }
        else
          this.MainW.Start_Temps();
        return false;
      }

      public bool OnBeforeResourceLoad(IWebBrowser browser, IRequest requestResponse)
      {
        return false;
      }

      public bool OnResourceResponse(
        IWebBrowser browserControl,
        IBrowser browser,
        IFrame frame,
        IRequest request,
        IResponse response)
      {
        return false;
      }

      public bool GetDownloadHandler(
        IWebBrowser browser,
        string mimeType,
        string fileName,
        long contentLength,
        ref IDownloadHandler handler)
      {
        return false;
      }

      public bool GetAuthCredentials(
        IWebBrowser browserControl,
        IBrowser browser,
        IFrame frame,
        bool isProxy,
        string host,
        int port,
        string realm,
        string scheme,
        IAuthCallback callback)
      {
        return false;
      }

      public bool OnOpenUrlFromTab(
        IWebBrowser browserControl,
        IBrowser browser,
        IFrame frame,
        string targetUrl,
        WindowOpenDisposition targetDisposition,
        bool userGesture)
      {
        return false;
      }
    }

    private enum Fases
    {
      StartUp,
      WaitStartUp,
      Reset,
      WaitReset,
      WaitDevices,
      GoUpdate,
      Update,
      Splash,
      WaitSplash,
      CheckRoom,
      WaitCheckRoom,
      LockUser,
      WaitLockUser,
      TestUser,
      WaitTestUser,
      TestCom,
      WaitTestCom,
      Config,
      WaitConfig,
      Register,
      WaitRegister,
      GoLogin,
      Login,
      GoHome,
      Home,
      GoRemoteReset,
      RemoteReset,
      GoManteniment,
      Manteniment,
      GoNavigate,
      Navigate,
      NavigateScreenSaver,
      Admin,
      CheckDevice,
      WaitCheckDevice,
      Calibrar,
      WaitCalibrar,
      Extern,
      OutOfService,
      WaitOutOfService,
      Stop,
    }

    public struct POINT
    {
      public int X;
      public int Y;

      public static implicit operator Point(MainWindow.POINT point)
      {
        return new Point(point.X, point.Y);
      }
    }

    internal struct KBDLLHOOKSTRUCT
    {
      public int vkCode;
      private int scanCode;
      public int flags;
      private int time;
      private int dwExtraInfo;
    }

    internal delegate IntPtr HookHandlerDelegate(
      int nCode,
      IntPtr wParam,
      ref MainWindow.KBDLLHOOKSTRUCT lParam);

    private enum DBT
    {
      DBT_DEVTYP_OEM = 0,
      DBT_DEVTYP_VOLUME = 2,
      DBT_DEVTYP_PORT = 3,
      DBT_DEVTYP_DEVICEINTERFACE = 5,
      DBT_DEVTYP_HANDLE = 6,
      DBT_DEVICEARRIVAL = 32768, // 0x00008000
      DBT_DEVICEREMOVECOMPLETE = 32772, // 0x00008004
    }

    [StructLayout(LayoutKind.Sequential)]
    internal class DEV_BROADCAST_HDR
    {
      internal int dbch_size;
      internal int dbch_devicetype;
      internal int dbch_reserved;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    internal class DEV_BROADCAST_DEVICEINTERFACE_1
    {
      internal int dbcc_size;
      internal int dbcc_devicetype;
      internal int dbcc_reserved;
      [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.U1)]
      internal byte[] dbcc_classguid;
      [MarshalAs(UnmanagedType.ByValArray, SizeConst = 255)]
      internal char[] dbcc_name;
    }

    internal struct SP_DEVICE_INTERFACE_DATA
    {
      internal int cbSize;
      internal Guid InterfaceClassGuid;
      internal int Flags;
      internal IntPtr Reserved;
    }

    internal struct SP_DEVICE_INTERFACE_DETAIL_DATA
    {
      internal int cbSize;
      internal string DevicePath;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal class DEV_BROADCAST_DEVICEINTERFACE
    {
      internal int dbcc_size;
      internal int dbcc_devicetype;
      internal int dbcc_reserved;
      internal Guid dbcc_classguid;
      internal short dbcc_name;
    }

    public class UsersRSO : RemoteSharedObject
    {
      public void msgFromSrvr(string msg)
      {
      }
    }

    private enum Srv_Command
    {
      Null,
      Status,
      Credits,
      AddCredits,
      SubCredits,
      SubCadeaux,
      AddTicket,
      SubTicket,
      Room,
      AnularTicket,
      VerificarTicket,
      KioskSetTime,
      KioskGetTime,
      KioskCommand,
    }

    public class Hook_Srv_KioskCommand : IPendingServiceCallback
    {
      public int OK;
      public string Resposta;
      public int timeout;

      public Hook_Srv_KioskCommand()
      {
        this.OK = -2;
        this.timeout = 0;
      }

      public void ResultReceived(IPendingServiceCall call)
      {
        object result = call.Result;
        try
        {
          this.OK = !(result.ToString().ToLower() == "error") ? 1 : 0;
        }
        catch
        {
        }
        this.timeout = 0;
      }
    }

    public class Hook_Srv_KioskSetTime : IPendingServiceCallback
    {
      public int OK;
      public int segons;
      public string Resposta;
      public int timeout;

      public Hook_Srv_KioskSetTime()
      {
        this.OK = -2;
        this.segons = 0;
        this.timeout = 0;
      }

      public void ResultReceived(IPendingServiceCall call)
      {
        object result = call.Result;
        try
        {
          if (result.ToString().ToLower() == "error")
          {
            this.OK = 0;
          }
          else
          {
            this.Resposta = result.ToString();
            this.OK = 1;
          }
        }
        catch
        {
        }
        this.timeout = 0;
      }
    }

    public class Hook_Srv_KioskGetTime : IPendingServiceCallback
    {
      public int OK;
      public int segons;
      public string Resposta;
      public int timeout;

      public Hook_Srv_KioskGetTime()
      {
        this.OK = -2;
        this.segons = 0;
        this.timeout = 0;
      }

      public void ResultReceived(IPendingServiceCall call)
      {
        object result = call.Result;
        try
        {
          if (result.ToString().ToLower() == "error")
          {
            this.OK = 0;
          }
          else
          {
            this.Resposta = result.ToString();
            this.segons = 0;
            try
            {
              this.segons = int.Parse(this.Resposta);
            }
            catch
            {
            }
            this.OK = 1;
          }
        }
        catch
        {
        }
        this.timeout = 0;
      }
    }

    public class Hook_Srv_Verificar_Ticket : IPendingServiceCallback
    {
      public int OK;
      public int ticket;
      public string Resposta;
      public int timeout;

      public Hook_Srv_Verificar_Ticket()
      {
        this.OK = -2;
        this.ticket = 0;
        this.timeout = 0;
      }

      public void ResultReceived(IPendingServiceCall call)
      {
        object result = call.Result;
        try
        {
          if (result.ToString().ToLower() == "error")
          {
            this.OK = 0;
          }
          else
          {
            this.Resposta = result.ToString();
            this.OK = 1;
          }
        }
        catch
        {
        }
        this.timeout = 0;
      }
    }

    public class Hook_Srv_Anular_Ticket : IPendingServiceCallback
    {
      public int OK;
      public int ticket;
      public int timeout;
      public bool estat;

      public Hook_Srv_Anular_Ticket()
      {
        this.OK = -2;
        this.ticket = 0;
        this.estat = false;
        this.timeout = 0;
      }

      public void ResultReceived(IPendingServiceCall call)
      {
        object result = call.Result;
        try
        {
          this.OK = !(result.ToString().ToLower() == "error") ? (Convert.ToInt32(result) != 0 ? 1 : 0) : 0;
        }
        catch
        {
        }
        this.timeout = 0;
      }
    }

    public class Hook_Srv_Credits : IPendingServiceCallback
    {
      public int OK;
      public int credits;
      public int timeout;

      public Hook_Srv_Credits()
      {
        this.OK = -2;
        this.credits = 0;
        this.timeout = 0;
      }

      public void ResultReceived(IPendingServiceCall call)
      {
        object result = call.Result;
        try
        {
          if (result.ToString().ToLower() == "error")
          {
            this.credits = 0;
            this.OK = 0;
          }
          else
          {
            int int32 = Convert.ToInt32(result);
            if (int32 == -1)
            {
              this.credits = 0;
              this.OK = 0;
            }
            else if (int32 < 0)
            {
              this.credits = 0;
              this.OK = 0;
            }
            else
            {
              this.credits = int32;
              this.OK = 1;
            }
          }
        }
        catch
        {
        }
        this.timeout = 0;
      }
    }

    public class Hook_Srv_Sub_Credits : IPendingServiceCallback
    {
      public int OK;
      public int ticket;
      public int timeout;

      public Hook_Srv_Sub_Credits()
      {
        this.OK = -2;
        this.ticket = 0;
        this.timeout = 0;
      }

      public void ResultReceived(IPendingServiceCall call)
      {
        object result = call.Result;
        try
        {
          if (result.ToString().ToLower() == "error")
          {
            this.OK = 0;
          }
          else
          {
            int int32 = Convert.ToInt32(result);
            this.OK = int32 != 0 ? (int32 != this.ticket ? 0 : 1) : 0;
          }
        }
        catch
        {
        }
        this.timeout = 0;
      }
    }



    public class Hook_Srv_Sub_Cadeaux : IPendingServiceCallback
    {
      public int OK;
      public int ticket;
      public int timeout;

      public Hook_Srv_Sub_Cadeaux()
      {
        this.OK = -2;
        this.ticket = 0;
        this.timeout = 0;
      }

      public void ResultReceived(IPendingServiceCall call)
      {
        object result = call.Result;
        try
        {
          if (result.ToString().ToLower() == "error")
          {
            this.OK = 0;
          }
          else
          {
            int int32 = Convert.ToInt32(result);
            this.OK = int32 != 0 ? (int32 != this.ticket ? 0 : 1) : 0;
          }
        }
        catch
        {
        }
        this.timeout = 0;
      }
    }

        public class Hook_Srv_Add_Credits_ : IPendingServiceCallback
        {
            public int OK;
            public int ticket;
            public int timeout;

            public Hook_Srv_Add_Credits_()
            {
                this.OK = -2;
                this.ticket = 0;
                this.timeout = 0;
            }

            public void ResultReceived(IPendingServiceCall call)
            {
                object result = call.Result;
                try
                {
                    if (result.ToString().ToLower() == "error")
                    {
                        this.OK = 0;
                    }
                    else
                    {
                        int int32 = Convert.ToInt32(result);
                        this.OK = int32 != 0 ? (int32 != this.ticket ? 0 : 1) : 0;
                    }
                }
                catch
                {
                }
                this.timeout = 0;
            }
        }

        public class Hook_Srv_Add_Credits : IPendingServiceCallback
    {
      public int OK;
      public int ticket;
      public int timeout;

      public Hook_Srv_Add_Credits()
      {
        this.OK = -2;
        this.ticket = 0;
        this.timeout = 0;
      }

      public void ResultReceived(IPendingServiceCall call)
      {
        object result = call.Result;
        try
        {
          if (result.ToString().ToLower() == "error")
          {
            this.OK = 0;
          }
          else
          {
            int int32 = Convert.ToInt32(result);
            this.OK = int32 != 0 ? (int32 != this.ticket ? 0 : 1) : 0;
          }
        }
        catch
        {
        }
        this.timeout = 0;
      }
    }

    public class Hook_Srv_Login : IPendingServiceCallback
    {
      public int login;
      public int OK;
      public int ticket;
      public int timeout;
      public string err;

      public Hook_Srv_Login()
      {
        this.OK = -2;
        this.ticket = 0;
        this.timeout = 0;
        this.login = 0;
        this.err = "";
      }

      public void ResultReceived(IPendingServiceCall call)
      {
        object result = call.Result;
        try
        {
          if (result.ToString().ToLower() == "error")
          {
            this.OK = 0;
          }
          else
          {
            int int32 = Convert.ToInt32(result);
            if (int32 <= 0)
            {
              this.OK = 0;
              this.login = 2;
              this.ticket = 0;
            }
            else
            {
              this.OK = 1;
              this.login = 1;
              this.ticket = int32;
            }
          }
        }
        catch (Exception ex)
        {
          this.err = ex.Message;
        }
        this.timeout = 0;
      }
    }

    public class Hook_Srv_Sub_Ticket : IPendingServiceCallback
    {
      public int OK;
      public int timeout;

      public Hook_Srv_Sub_Ticket()
      {
        this.OK = -2;
        this.timeout = 0;
      }

      public void ResultReceived(IPendingServiceCall call)
      {
        object result = call.Result;
        try
        {
          this.OK = !(result.ToString().ToLower() == "error") ? (Convert.ToBoolean(result) ? 1 : 0) : 0;
        }
        catch
        {
        }
        this.timeout = 0;
      }
    }

    public class Hook_Srv_Add_Ticket : IPendingServiceCallback
    {
      public int OK;
      public int Ticket;
      public int timeout;

      public Hook_Srv_Add_Ticket()
      {
        this.Ticket = 0;
        this.OK = -2;
        this.timeout = 0;
      }

      public void ResultReceived(IPendingServiceCall call)
      {
        object result = call.Result;
        try
        {
          if (result.ToString().ToLower() == "error")
          {
            this.OK = 0;
          }
          else
          {
            int int32 = Convert.ToInt32(result);
            if (int32 == 0)
            {
              this.OK = 0;
            }
            else
            {
              this.Ticket = int32;
              this.OK = 1;
            }
          }
        }
        catch
        {
        }
        this.timeout = 0;
      }
    }

    public class Hook_Srv_Add_Pay : IPendingServiceCallback
    {
      public int OK;
      public int Pay;
      public int timeout;
      public string Resposta;

      public Hook_Srv_Add_Pay()
      {
        this.Pay = 0;
        this.OK = -2;
        this.Resposta = "";
        this.timeout = 0;
      }

      public void ResultReceived(IPendingServiceCall call)
      {
        object result = call.Result;
        try
        {
          if (result.ToString().ToLower() == "error")
          {
            this.OK = 0;
          }
          else
          {
            this.Resposta = result.ToString();
            this.OK = 1;
          }
        }
        catch
        {
        }
        this.timeout = 0;
      }
    }

    private class BitmapData
    {
      public BitArray Dots { get; set; }

      public int Height { get; set; }

      public int Width { get; set; }
    }
  }
}
